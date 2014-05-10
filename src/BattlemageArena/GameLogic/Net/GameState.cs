using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.GameLogic.Net
{
    enum GameState
    {
        TitleScreen,
        WaitingPlayers,
        PlayingLocal,
        PlayingClient,
        PlayingHost,
        GameEnded
    }
}
