using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace BattlemageArena.GameLogic.Net
{
    class NetworkConnection
    {
        #region Attributes
        private NetworkSession _session;
        private GameMain _game;
        #endregion Attributes

        #region Constructors

        public NetworkConnection(GameMain game)
        {
            _game = game;
        }
        #endregion Constructors

        #region Methods
        public bool SignIn()
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

        public void CreateSession()
        {
            _session = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 2);
            _session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(Host_GamerJoined);
            _session.GamerLeft += new EventHandler<GamerLeftEventArgs>(Host_GamerLeft); 
            
            // TODO: Change game state to WAIT FOR PLAYERS
        }

        public void SearchForGame()
        {
            AvailableNetworkSessionCollection sessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1, null);
            if (sessions.Count > 0)
            {
                AvailableNetworkSession mySession = sessions[0]; 
                _session = NetworkSession.Join(mySession); 
                _session.GamerLeft += new EventHandler<GamerLeftEventArgs>(Client_GamerLeft); 
                //StartGame(); 
                //_gameState = GameState.PlayingAsPlayer;
            } 
        }
        #endregion Methods

        #region Event Handlers
        void Host_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            if (_session.RemoteGamers.Count <= 1)
            {
                //StartGame(); 
                //state = GameState.PlayingAsHost;
            }
        }

        void Host_GamerLeft(object sender, GamerLeftEventArgs e)
        {
            _session.Dispose(); 
            //EndGame();
        }

        void Client_GamerLeft(object sender, GamerLeftEventArgs e)
        {
            _session.Dispose(); 
            //EndGame();
        } 
        #endregion Event Handlers
    }
}
