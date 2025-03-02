
namespace Countdown.ConsoleApp
{
    using Countdown.Core;
    internal class Program
    {
        private const int MaxLetters = 9;
        private const int TotalRounds = 4;

        private static async Task Main(string[] args)
        {
            var game = new Game();
            Console.WriteLine("Welcome to Countdown!");

            // Ask user for consonants or vowels
            for (int round = 1; round <= TotalRounds; round++)
            {
                Console.WriteLine($"\n--- Round {round} ---");

                while (game.Letters.Count < MaxLetters)
                {
                    Console.WriteLine("Current letters: " + (game.Letters.Count > 0 ? string.Join(" ", game.Letters) : "(none)"));
                    int remaining = MaxLetters - game.Letters.Count;
                    Console.WriteLine($"You have {remaining} letter(s) left to pick.");
                    Console.Write("Choose letter type (C for Consonant, V for Vowel): ");

                    var input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Invalid input. Please enter 'C' or 'V'.");
                        continue;
                    }

                    char choice = char.ToUpper(input[0]);
                    try
                    {
                        if (choice == 'C')
                        {
                            game.AddConsonant();
                        }
                        else if (choice == 'V')
                        {
                            game.AddVowel();
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please enter 'C' for Consonant or 'V' for Vowel.");
                        }
                    }
                    catch (InsufficientSlotsException)
                    {
                        Console.WriteLine("\nYou must have at least 4 consonants and 3 vowels.\n");
                    }
                }

                Console.WriteLine("\nLetters chosen: " + string.Join(" ", game.Letters));

                // Get answer within 30 seconds
                Console.WriteLine("Please enter your answer: ");
                string answer = await Countdown.Util.Console.ReadLineWithTimeout(30);

                if (answer == null)
                {
                    Console.WriteLine("Time's up!");
                }
                else
                {
                    try
                    {
                        int score = game.Answer(answer);
                        Console.WriteLine($"Your answer is worth {score} points.");
                    }
                    catch (IllegalAnswerException ex)
                    {
                        Console.WriteLine($"That answer is not valid: {ex.Message}");
                    }
                }

                string best = game.BestAnswer();
                Console.WriteLine($"The best answer was: {best}, worth {best.Length} points.");

                Console.WriteLine($"\nYour current score is {game.Score}.\n");
                game.NextRound();
            }

            Console.WriteLine("\nGame over! Your final score is: " + game.Score);
        }
    }
}