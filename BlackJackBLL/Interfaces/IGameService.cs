using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackBLL.Interfaces
{
    public interface IGameService
    {
        void StartNewGame();
        bool PlayTurn(Services.GameService.PlayerType player ,Services.GameService.TurnAction turnAction);
        IEnumerable<Card> GetPlayerHands(Services.GameService.PlayerType playerType);
        string DecideWinner();
    }
}
