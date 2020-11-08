using BlackJackBLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class GameControl // Now now, what is a game without a controller? :o......its boring thats what it is :D (jokes aside, this is for the app to comunicate with
                            // the libraries gameservice as well as display data etc. It doesnt make sense putting console logic in the library, messes with reusability)

    {
        private GameService _gameService;


        private static readonly IEnumerable<string> _inGameInputOptionsDisplay = new List<string>()
        {
            "1. Hit me",
            "2. Stand"
        };

        private static readonly IEnumerable<string> _afterGameInputOptionsDisplay = new List<string>()
        {
            "1. Play Again",
            "2. Exit"
        };

        public GameControl()
        {
            _gameService = new GameService();
            _gameService.StartNewGame(); // When a game is started the dealer will 2 cards to the player and himself
        }

        public void PersonPlay() // Recursive function (indirect) to avoid repeating this ugly code:D
        {
            if (_gameService.GameOver) //If the game is not over we assume the dealer hasnt played yet or the player is not bust;
            {
                Console.WriteLine("Dealers Hand :");
                Console.WriteLine();

                var PlayerHand = _gameService.GetPlayerHands(GameService.PlayerType.Dealer);

                foreach (var card in PlayerHand)
                {
                    Console.WriteLine($"{card.Name} of {card.Suit}");
                }

                Console.WriteLine();

                Console.WriteLine(_gameService.DecideWinner());

                foreach (var option in _afterGameInputOptionsDisplay)
                {
                    Console.WriteLine(option);
                }

                var input = Console.ReadLine();

                var validated = ValidateInput(input);

                if (!validated)
                {
                    Console.WriteLine("Invalid input, please read input option and enter valid input");
                    Console.WriteLine();
                    PersonPlay();
                }
                else
                {
                    ActionUserInput(Math.Abs(int.Parse(input)));
                }
            }
            else
            {
                DisplayPlayerHand();

                foreach (var option in _inGameInputOptionsDisplay)
                {
                    Console.WriteLine(option);
                }

                var input = Console.ReadLine();

                var validated = ValidateInput(input);

                if (!validated)
                {
                    Console.WriteLine("Invalid input, please read input option and enter valid input");
                    Console.WriteLine();
                    PersonPlay();
                }
                else
                {
                    ActionUserInput(Math.Abs(int.Parse(input)));
                }
            }
        }

        public void CpuPlay()
        {
            Console.WriteLine("Dealer is beginning his turn.....");
            Console.WriteLine();

            _gameService.PlayTurn(GameService.PlayerType.Dealer, GameService.TurnAction.Hit);

            PersonPlay(); // go back to recursive function
        }

        private bool ValidateInput(string input)
        {
            var isInt = int.TryParse(input, out int inputValue); // Incase our user decides to get frisky

            if (isInt)
            {
                if (_gameService.GameOver)
                {
                    var exists = Enum.IsDefined(typeof(AfterGameOptions), Math.Abs(inputValue));

                    return exists;
                }
                else
                {
                    var exists = Enum.IsDefined(typeof(InGameOptions),Math.Abs(inputValue));

                    return exists;
                }
            }

            return false;
        }

        private void ActionUserInput(int input)
        {

            if (_gameService.GameOver)
            {
                var option = (AfterGameOptions)Convert.ToInt32(input);

                switch (option)
                {
                    case AfterGameOptions.PlayAgain:
                        _gameService.StartNewGame();
                        PersonPlay(); // Go back to recursive function once new game has been initialized
                        break;
                    case AfterGameOptions.Exit:
                        Environment.Exit(-1);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var option = (InGameOptions)Convert.ToInt32(input);

                switch (option)
                {
                    case InGameOptions.Stand:
                        _gameService.PlayTurn(GameService.PlayerType.Player, GameService.TurnAction.Stand); //If player stands then recursive function exist and goes back to main
                        CpuPlay(); // Dealers turn to play
                        break;
                    case InGameOptions.HitMe:
                        var bust = _gameService.PlayTurn(GameService.PlayerType.Player, GameService.TurnAction.Hit);

                        if (!bust) // This is to fix bug where player is able to hit themselves while be bust and never giving the dealer their turn
                        {
                            PersonPlay();
                        }
                        else
                        {
                            DisplayPlayerHand();
                            CpuPlay(); // again if user is bust then control must go to dealer
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private void DisplayPlayerHand()
        {
            Console.WriteLine("Your Hand :");
            Console.WriteLine();

            var PlayerHand = _gameService.GetPlayerHands(GameService.PlayerType.Player);

            foreach (var card in PlayerHand)
            {
                Console.WriteLine($"{card.Name} of {card.Suit}");
            }

            Console.WriteLine();
        }

        private enum InGameOptions
        {
            HitMe = 1,
            Stand = 2,
        }

        private enum AfterGameOptions
        {
            PlayAgain = 1,
            Exit = 2
        }
    }
}
