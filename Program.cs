using Spectre.Console;

namespace ann_jikan
{
    internal class Program
    {
        public enum SelectActions
        {
            Search,
            CurrentlyAiring,
            Exit,
        }

        static async Task Main(string[] args)
        {
            // Console.WriteLine("\x1B[1F\x1B[J");
            var panel = new Panel(
                Align.Center(
                    new Markup("[white bold]ANN - Jikan[/]\n[white dim]The Latest of Anime News[/]")
                )
            )
                .HeavyBorder()
                .BorderStyle("lightsteelblue bold")
                .Padding(2, 2, 2, 2)
                .Expand();
            AnsiConsole.Write(panel);
            Console.WriteLine("");

            var run = true;

            while (run)
            {
                AnsiConsole.Write(Separator("Main Menu"));

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<(SelectActions, string)>()
                        .Title("[deepskyblue1 bold]?[/] Choose you action:")
                        .HighlightStyle("skyblue1")
                        .AddChoices(
                            new[]
                            {
                                (SelectActions.Search, "Search for Anime"),
                                (SelectActions.CurrentlyAiring, "Show currently airing Anime"),
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
                        // await SearchAnime();
                        break;
                    case SelectActions.CurrentlyAiring:
                        // await CurrentlyAiring();
                        break;
                    case SelectActions.Exit:
                        run = false;
                        AnsiConsole.WriteLine("\n👋 Exiting application...");
                        break;
                }
            }
        }

        private static Rule Separator(string title = "", string colour = "lightcoral")
        {
            return new Rule($"[white bold]{title}[/]")
                .LeftJustified()
                .HeavyBorder()
                .RuleStyle($"{colour} dim");
        }
    }
}
