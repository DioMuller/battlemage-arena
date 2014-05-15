using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BattlemageArena.Core.Input;
using BattlemageArena.GameLogic.Behaviors;
using BattlemageArena.GameLogic.Entities;
using BattlemageArena.GameLogic.Screens;
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

        public void Reinitialize()
        {
            _behaviors = new List<NetworkBehavior>();
            _counter = 1;
        }

        public void CreateSession()
        {
            if( _session != null ) _session.Dispose();
            if (Gamer.SignedInGamers.Count == 0)
            {
                SignIn();
            }
            else
            {

                _session = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 2);
                _session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(Host_GamerJoined);
                _session.GamerLeft += new EventHandler<GamerLeftEventArgs>(Host_GamerLeft);

                GameMain.ChangeState(GameState.WaitingPlayers);

                IsHost = true;
            }
        }

        public void SearchForGame()
        {
            if (Gamer.SignedInGamers.Count == 0)
            {
                SignIn();
            }
            else
            {
                if (_session != null) _session.Dispose();
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

        public void SignIn()
        {
            if (Gamer.SignedInGamers.Count == 0)
            {
                if (Guide.IsVisible == false)
                    Guide.ShowSignIn(1, false);
            }
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
                _session.StartGame();
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
                int id;

                if (type == "Player")
                {
                    id = _reader.ReadInt32();
                    NetworkBehavior net = _behaviors.OfType<NetPlayerBehavior>().FirstOrDefault((b) => b.Id == id);

                    if (net != null)
                    {
                        net.ReceiveData(_reader);
                    }
                }
                if (type == "Fireball")
                {
                    id = _reader.ReadInt32();
                    NetworkBehavior net = _behaviors.OfType<NetFireballBehavior>().FirstOrDefault((b) => b.Id == id);

                    if (net != null)
                    {
                        net.ReceiveData(_reader);
                    }
                }
                if (type == "CreatePlayer")
                {
                    id = _reader.ReadInt32();
                    string name = _reader.ReadString();
                    Vector2 position = _reader.ReadVector2();
                    int health = _reader.ReadInt32();
                    Color color = _reader.ReadColor();

                    if (id != _session.LocalGamers[0].Id)
                    {
                        Player player = new Player(GameMain.CurrentLevel, position, color, new GenericInput(), name)
                        {
                            Health = health
                        };
                        player.Behaviors.Add(new NetPlayerBehavior(player, id));
                        GameMain.CurrentLevel.AddEntity(player);
                    }
                }
                if (type == "CreateFireball")
                {
                    id = _reader.ReadInt32();
                    Vector2 position = _reader.ReadVector2();
                    Direction direction = (Direction)_reader.ReadInt32();
                    Color color = _reader.ReadColor();

                    Fireball fireball = new Fireball(GameMain.CurrentLevel, position, color, direction);
                    fireball.Behaviors.Add(new NetFireballBehavior(fireball, id));
                    GameMain.CurrentLevel.AddEntity(fireball);
                }
                if (type == "RequestFireball")
                {
                    Vector2 position = _reader.ReadVector2();
                    Direction direction = (Direction)_reader.ReadInt32();
                    Color color = _reader.ReadColor();

                    Fireball fireball = new Fireball(GameMain.CurrentLevel, position, color, direction);
                    fireball.Behaviors.Add(new NetFireballBehavior(fireball));
                    //GameMain.CurrentLevel.AddEntity(fireball);
                    CreateFireball(fireball);
                }
            }

            _session.Update();
        }
        #endregion Game Cycle

        #region Helper Methods

        public void CreatePlayer(Player player)
        {
            NetPlayerBehavior b = player.GetBehavior<NetPlayerBehavior>();

            if (b != null)
            {
                _writer.Write("CreatePlayer");
                _writer.Write(b.Id);

                _writer.Write(player.Name);
                _writer.Write(player.Position);
                _writer.Write(player.Health);
                _writer.Write(player.Color);

                _session.LocalGamers[0].SendData(_writer, SendDataOptions.ReliableInOrder);
            }
            else
            {
                throw new Exception("Could not create other player: Player doesn't have NetworkBehavior.");
            }
        }

        public void CreateFireball(Fireball fireball)
        {
            if (IsHost)
            {
                NetFireballBehavior b = new NetFireballBehavior(fireball);
                fireball.Behaviors.Add(b);

                _writer.Write("CreateFireball");
                _writer.Write(b.Id);
            }
            else
            {
                _writer.Write("RequestFireball");
            }

            _writer.Write(fireball.Position);
            _writer.Write((int) fireball.Direction);
            _writer.Write(fireball.Color);

            _session.LocalGamers[0].SendData(_writer, SendDataOptions.ReliableInOrder);

            if (IsHost)
            {
                GameMain.CurrentLevel.AddEntity(fireball);
            }            
        }
        #endregion Helper Methods
    }
}
