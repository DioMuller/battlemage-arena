using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BattlemageArena.GameLogic.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace BattlemageArena.GameLogic.Net
{
    public class NetworkConnection
    {
        #region Attributes
        private NetworkSession _session;
        private GameMain _game;
        private PacketReader _reader;
        private PacketWriter _writer;
        private int _counter;
        private List<NetworkBehavior> _behaviors; 
        #endregion Attributes

        #region Properties
        public LocalNetworkGamer LocalHost { get { return _session.LocalGamers[0]; } }
        public bool IsHost { get; private set; }
        public NetworkSession Session { get { return _session; } }
        #endregion Properties

        #region Constructors

        public NetworkConnection(GameMain game)
        {
            _game = game;
            _counter = 1;

            _reader = new PacketReader();
            _writer = new PacketWriter();

            _behaviors = new List<NetworkBehavior>();
        }
        #endregion Constructors

        #region Methods

        public void CreateSession()
        {
            _session = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 2);
            _session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(Host_GamerJoined);
            _session.GamerLeft += new EventHandler<GamerLeftEventArgs>(Host_GamerLeft);

            GameMain.ChangeState(GameState.WaitingPlayers);

            IsHost = true;
        }

        public void SearchForGame()
        {
            AvailableNetworkSessionCollection sessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1, null);
            if (sessions.Count > 0)
            {
                AvailableNetworkSession mySession = sessions[0]; 
                _session = NetworkSession.Join(mySession); 
                _session.GamerLeft += new EventHandler<GamerLeftEventArgs>(Client_GamerLeft);

                GameMain.ChangeState(GameState.PlayingClient);

                IsHost = false;
            } 
        }

        public int GetUniqueValue()
        {
            if( !_session.IsHost ) throw new Exception("Only hosts can generate unique values.");
                
            return ++_counter;
        }

        public void AddBehavior(NetworkBehavior behavior)
        {
            _behaviors.Add(behavior);
        }
        #endregion Methods

        #region Event Handlers
        void Host_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            /*if (GameMain.CurrentState == GameState.WaitingPlayers &&
            _session.RemoteGamers.Count == GameMain.PlayerCount)*/
            if( _session.RemoteGamers.Count >= 1 )
            {
                GameMain.ChangeState(GameState.PlayingHost);
            }
        }

        void Host_GamerLeft(object sender, GamerLeftEventArgs e)
        {
            _session.Dispose();

            if (_session.RemoteGamers.Count <= 0)
            {
                GameMain.ChangeState(GameState.TitleScreen);
            }
        }

        void Client_GamerLeft(object sender, GamerLeftEventArgs e)
        {
            _session.Dispose();

            if (_session.RemoteGamers.Count <= 0)
            {
                GameMain.ChangeState(GameState.TitleScreen);
            }
        } 
        #endregion Event Handlers

        #region Game Cycle
        public void Update(GameTime gameTime)
        {
            while (LocalHost.IsDataAvailable)
            {
                NetworkGamer sender;
                LocalHost.ReceiveData(_reader, out sender);
                string type = _reader.ReadString();
                int id = _reader.ReadInt32();
                NetworkBehavior net = _behaviors.FirstOrDefault((b) => b.Id == id);

                if (net != null)
                {
                    net.ReceiveData(_reader);
                }
            }
        }
        #endregion Game Cycle
    }
}
