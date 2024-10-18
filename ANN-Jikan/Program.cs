using ANN_Jikan.ServiceProviders.ANN;
using ANN_Jikan.ServiceProviders.Jikan;
using Spectre.Console;

namespace ANN_Jikan
{
    internal class Program
    {
        enum SelectActions
        {
            Search,
            CurrentlyAiring,
            Exit,
        }

        static JikanService jikanService = new JikanService();
        static ANNService annService = new ANNService();

        static async Task Main(string[] args)
        {
            try
            {
                var panel = new Panel(
                    Align.Center(
                        new Markup(
                            "[white bold]ANN - Jikan Newsfeed[/]\n[white dim]The Latest of Anime News[/]"
                        )
                    )
                )
                    .HeavyBorder()
                    .BorderStyle("lightsteelblue bold")
                    .Padding(2, 2, 2, 2)
                    .Expand();
                AnsiConsole.Write(panel);

                var run = true;

                while (run)
                {
                    Console.WriteLine("");
                    AnsiConsole.Write(Separator("Main Menu"));

                    var action = AnsiConsole.Prompt(
                        new SelectionPrompt<(SelectActions, string)>()
                            .Title("[deepskyblue1 bold]?[/] Choose your action:")
                            .HighlightStyle("skyblue1")
                            .AddChoices(
                                new[]
                                {
                                    (SelectActions.Search, "Search for Anime"),
                                    (SelectActions.CurrentlyAiring, "Show popular currently airing Anime"),
                                    (SelectActions.Exit, "Exit application"),
                                }
                            )
                            .UseConverter(action => action.Item2)
                    );

                    AnsiConsole.MarkupLine(
                        $"[lightgreen bold]✓[/] Choose your action: [lightgreen bold]{action.Item2}[/]"
                    );

                    switch (action.Item1)
                    {
                        case SelectActions.Search:
                            await SearchAnime();
                            break;
                        case SelectActions.CurrentlyAiring:
                            await CurrentlyAiring();
                            break;
                        case SelectActions.Exit:
                            run = false;
                            AnsiConsole.WriteLine("\n👋 Exiting application...");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
            }
        }

        private static Rule Separator(string title = "", string colour = "lightcoral")
        {
            return new Rule($"[white bold]{title}[/]")
                .LeftJustified()
                .HeavyBorder()
                .RuleStyle($"{colour} dim");
        }

        private static async Task SearchAnime()
        {
            Console.WriteLine("");
            AnsiConsole.Write(Separator("🔍 Search Anime"));

            var query = AnsiConsole.Ask<string>("[deepskyblue1]?[/] Enter anime name to search:");

            Console.Write("\x1B[1F\x1B[J");

            var searchResults = await AnsiConsole
                .Status()
                .StartAsync(
                    $"Searching for anime: ${query}...",
                    async ctx => await jikanService.Search(query)
                );

            Console.Write("\x1B[1F\x1B[J");
            AnsiConsole.MarkupLine(
                $"[lightgreen bold]✓[/] Searching for anime: [lightgreen bold]{query}[/]"
            );

            if (searchResults.Count == 0)
            {
                AnsiConsole.MarkupLine(
                    $"[orangered1 bold]✗[/] No results found for [orangered1 bold]{query}[/]"
                );
                return;
            }

            var table = new Table().Border(TableBorder.Rounded).BorderStyle("skyblue1 bold");

            table.AddColumn("ID");
            table.AddColumn("Title");
            table.AddColumn("Ratings");

            for (var i = 0; i < searchResults.Count; i++)
            {
                table.AddRow(
                    i.ToString(),
                    searchResults[i].title_english?.ToString() ?? searchResults[i].title,
                    searchResults[i].score?.ToString() ?? "N/A"
                );
            }

            AnsiConsole.Write(table);
            var id = SelectTableID(searchResults.Count);
            if (id == -1)
                return;

            var annID = await jikanService.GetANNId(searchResults[id].mal_id);
            if (annID == null)
            {
                AnsiConsole.MarkupLine(
                    $"[orangered1 bold]✗[/] No ANN ID found for anime ID [orangered1 bold]{searchResults[id].mal_id}[/]"
                );
                return;
            }
            await ListArticle(annID.Value);
        }

        private static async Task CurrentlyAiring()
        {
            Console.WriteLine("");
            AnsiConsole.Write(Separator("📺 Currently Airing"));

            var airingRes = await AnsiConsole
                .Status()
                .StartAsync("Getting popular currently airing anime...", async ctx => await jikanService.GetPopularAiring());

            var table = new Table().Border(TableBorder.Rounded).BorderStyle("skyblue1 bold");
            table.AddColumn("ID");
            table.AddColumn("Title");
            table.AddColumn("Ratings");

            for (var i = 0; i < airingRes.Count; i++)
            {
                table.AddRow(
                    i.ToString(),
                    airingRes[i].title_english?.ToString() ?? airingRes[i].title,
                    airingRes[i].score?.ToString() ?? "N/A"
                );
            }
            
            AnsiConsole.Write(table);
            var id = SelectTableID(airingRes.Count);
            if (id == -1)
                return;

            var annID = await jikanService.GetANNId(airingRes[id].mal_id);
            if (annID == null)
            {
                AnsiConsole.MarkupLine(
                    $"[orangered1 bold]✗[/] No ANN ID found for anime ID [orangered1 bold]{airingRes[id].mal_id}[/]"
                );
                return;
            }
            await ListArticle(annID.Value);
        }

        private static async Task ListArticle(int annID)
        {
            Console.WriteLine("");
            AnsiConsole.Write(Separator("📰 Select Article"));

            var newsRes = await AnsiConsole
                .Status()
                .StartAsync("Getting articles...", async ctx => await annService.GetNews(annID));

            if (newsRes.Count == 0)
            {
                AnsiConsole.MarkupLine(
                    $"[orangered1 bold]✗[/] No news found for anime ID [orangered1 bold]{annID}[/]"
                );
                return;
            }

            var table = new Table().Border(TableBorder.Rounded).BorderStyle("skyblue1 bold");
            table.AddColumn("ID");
            table.AddColumn("Title");
            table.AddColumn("Date");

            for (var i = 0; i < newsRes.Count; i++)
            {
                table.AddRow(i.ToString(), newsRes[i].title, newsRes[i].date);
            }

            AnsiConsole.Write(table);
            var id = SelectTableID(newsRes.Count);
            if (id == -1)
                return;

            Console.WriteLine(newsRes[id].url);

            try
            {
                var article = await AnsiConsole
                    .Status()
                    .StartAsync(
                        $"Getting article \"{newsRes[id].title}\"...",
                        async ctx => await annService.GetNewsArticle(newsRes[id].url)
                    );

                var panel = new Panel(article)
                    .RoundedBorder()
                    .BorderStyle("skyblue1 bold")
                    .Expand()
                    .Padding(4, 2, 4, 2)
                    .Expand();
                panel.Header = new PanelHeader($"Article: {newsRes[id].title}", Justify.Center);
                AnsiConsole.Write(panel);
            }
            catch (Exception)
            {
                var panel = new Panel("ERROR: Failed to load article" + $"\nURL: {newsRes[id].url}")
                    .RoundedBorder()
                    .BorderStyle("orangered1 bold")
                    .Expand()
                    .Padding(4, 2, 4, 2)
                    .Expand();
                panel.Header = new PanelHeader($"Article: {newsRes[id].title}", Justify.Center);
                AnsiConsole.Write(panel);
            }
        }

        private static int SelectTableID(int count)
        {
            var valid = false;
            var animeID = "";
            var id = 0;
            Console.WriteLine("");
            AnsiConsole.MarkupLine("[deepskyblue1]Select an ID from the table[/]");
            while (!valid)
            {
                animeID = AnsiConsole.Ask<string>(
                    "[deepskyblue1]?[/] Enter Table ID ([grey54]type EXIT to quite menu[/]):"
                );

                if (animeID == "EXIT")
                    return -1;

                if (int.TryParse(animeID, out id))
                    valid = true;
                else
                    AnsiConsole.MarkupLine(
                        "[red bold]✗[/] Invalid input, please enter a valid ID from the table"
                    );

                if (id < 0 || id >= count)
                {
                    valid = false;
                    AnsiConsole.MarkupLine(
                        "[red bold]✗[/] Invalid ID, please enter a valid ID from the table"
                    );
                }
            }

            Console.Write("\x1B[1F\x1B[J");
            AnsiConsole.MarkupLine($"[lightgreen bold]✓[/] Enter ID: [lightgreen bold]{id}[/]");

            return id;
        }
    }
}
