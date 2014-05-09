using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;

namespace BattlemageArena.GameLogic.Net
{
    class NetworkConnection
    {
        public static bool SignIn()
        {
            if (Gamer.SignedInGamers.Count == 0)
            {
                if (Guide.IsVisible == false)
                {
                    Guide.ShowSignIn(1, false);

                    return (Gamer.SignedInGamers.Count != 0);
                }

                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
