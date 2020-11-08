using BlackJackBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace BlackJackBLL.Services
{
    public class GameService : IGameService // Outside application will only communicate with the game service in this library, everything else i encapsulated in here
    {
        private Dealer _dealer;
        private Person _player;

        public GameService()
        {
            _dealer = new Dealer();
            _player = new Person();
        }

        public bool GameOver { get; private set; }

        public void StartNewGame()
        {
            _dealer.CollectCards(); // Collect cards from from players and his hand and putt it back to the deck

            if (GameOver)
            {
                _player.Reinitialize();
                _dealer.Reinitialize(); // resets properties

                GameOver = false;
            }

            for (int i = 0; i < 2; i++) // Deal 2 cards to each player
            {
                var card = Dealer.DealCard();

                _player.Hand.Add(card);
            }


            for (int i = 0; i < 2; i++)
            {
                var card = Dealer.DealCard();

                _dealer.Hand.Add(card);
            }
        }

        public bool PlayTurn(PlayerType player, TurnAction turnAction)
        {
            if (player == PlayerType.Player)
            {
                switch (turnAction)
                {
                    case TurnAction.Hit:
                        _player.HitMe();
                        break;
                    case TurnAction.Stand:
                        _player.Stand = true;
                        break;
                    default:
                        break;
                }

                return _player.Bust; // return property instead of doing a double check after hit

            }
            else //Dealer (Dealer will only call hit)
            {
                while (!_dealer.Stand && !_dealer.Bust)
                {
                    _dealer.HitMe();
                }

                GameOver = true; // Dealer will be done with turn when code hits here

                return true;
            }
        }

        public IEnumerable<Card> GetPlayerHands(PlayerType playerType)
        {
            if (playerType == PlayerType.Dealer)
            {
                return _dealer.Hand;
            }
            else
            {
                return _player.Hand;
            }
        }

        public string DecideWinner()
        {
            if (GameOver != true)
            {
                throw new InvalidOperationException("Game must end before deciding winner, please check if dealer is finished playing their turn");
            }

            if (!_dealer.Bust && !_player.Bust) // Only start score comparisons when both players are not bust
            {
                if (_dealer.Total < _player.Total)
                {
                    return $"YOU WON!!!!!!........ Dealer : {_dealer.Total}............ You : {_player.Total}";
                }
                else
                {
                    return $"DEALER WON!!!!!!........ Dealer : {_dealer.Total}............ You : {_player.Total}";
                }
            }
            else
            {
                if (_dealer.Bust && _player.Bust)
                {
                    return $"DRAW!!!!!!........ Dealer : {_dealer.Total}............ You : {_player.Total}";
                }
                else if (_player.Bust)
                    return $"DEALER WON!!!!!!........ Dealer : {_dealer.Total}............ You : {_player.Total}";

                return $"YOU WON!!!!!!........ Dealer : {_dealer.Total}............ You : {_player.Total}";
            }

        }

        public enum TurnAction
        {
            Hit,
            Stand
        }

        public enum PlayerType
        {
            Dealer,
            Player
        }
    }
}
