using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.GameLogic.Net
{
    public enum GameState
    {
        TitleScreen,
        WaitingPlayers,
        SearchingGame,
        PlayingLocal,
        PlayingClient,
        PlayingHost,
        CreatingHost,
        GameEnded
    }
}
