using System;
using System.Collections.Generic;
using System.Linq;
using BattlemageArena.Core;
using BattlemageArena.Core.Input;
using BattlemageArena.GameLogic.Net;
using BattlemageArena.GameLogic.Screens;
using BattlemageArena.Core.Sprites;
using BattlemageArena.GameLogic.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;

namespace BattlemageArena
{
    /// <summary>
    /// Main Game is where the magic happens.
    /// </summary>
    public class GameMain : Microsoft.Xna.Framework.Game
    {
        #region Const
        private const int Width = 960;
        private const int Height = 540;
        #endregion Const

        #region Attributes
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TitleScreen _title;
        private Level _level;
        private MessageScreen _hostingScreen;
        private MessageScreen _searchingScreen;

        private GameState _currentState;

        private NetworkConnection _connection;

        private Song _titleSong;
        private Song _gameSong;

        /// <summary>
        /// Instance for static methods.
        /// </summary>
        private static GameMain _instance;
        #endregion Attributes

        #region Properties
        public static int PlayerCount { get; set; }
        public static bool UseKeyboard { get; set; }

        public static Level CurrentLevel { get { return _instance._level; } }

        public static NetworkConnection Connection
        {
            get { return _instance._connection; }
        }

        public static GameState CurrentState { get { return _instance._currentState; } }

        public static NetworkSession CurrentSession { get { return _instance._connection.Session; } }
        #endregion Properties

        #region Constructors
        public GameMain()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _instance = this;
            _currentState = GameState.TitleScreen;

            _connection = new NetworkConnection(this);

            PlayerCount = 2;
            UseKeyboard = true;

            Components.Add(new GamerServicesComponent(this));
        }
        #endregion Constructors

        #region Game Override Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.Window.Title = "Battlemage Arena";
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;

            _graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameContent.Initialize(Content);

            _title = new TitleScreen("Images/arena", Width, Height);

            _hostingScreen = new MessageScreen("Images/arena", Width, Height, "Searching for Players...");
            _searchingScreen = new MessageScreen("Images/arena", Width, Height, "Searching for Games...");

            //Reset();

            _titleSong = GameContent.LoadContent<Song>("Music/Title");
            _gameSong = GameContent.LoadContent<Song>("Music/Arena");

            MediaPlayer.Play(_titleSong);
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            switch (_currentState)
            {
                    case GameState.TitleScreen:
                        _title.Update(gameTime);
                        break;
                    case GameState.PlayingLocal:
                       _level.Update(gameTime);
                       break;
                    case GameState.PlayingHost:
                    case GameState.PlayingClient:
                        _level.Update(gameTime);
                        _connection.Update(gameTime);
                        break;
                    case GameState.WaitingPlayers:
                    case GameState.CreatingHost:
                        _connection.Session.Update();
                        break;
                    case GameState.SearchingGame:
                        _connection.SearchForGame();
                        break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (_currentState)
            {
                case GameState.TitleScreen:
                    if( _connection.Session != null ) _connection.Session.Dispose();
                    _connection = new NetworkConnection(this);
                    _title.Draw(gameTime, _spriteBatch);
                    break;
                case GameState.PlayingHost:
                case GameState.PlayingClient:
                case GameState.PlayingLocal:
                    _level.Draw(gameTime, _spriteBatch);
                    break;
                case GameState.CreatingHost:
                case GameState.WaitingPlayers:
                    _hostingScreen.Draw(gameTime, _spriteBatch);
                    break;
                case GameState.SearchingGame:
                    _searchingScreen.Draw(gameTime, _spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion Game Override Methods

        #region Methods

        internal void Reset(GameState state)
        {
            //if( state == GameState.PlayingLocal || state == GameState.CreatingHost)
            _level = new Level("Images/arena", Width, Height, PlayerCount, UseKeyboard, state );
        }

        private void ChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.TitleScreen:
                    MediaPlayer.Play(_titleSong);
                    break;
                case GameState.PlayingLocal:
                case GameState.PlayingHost:
                case GameState.PlayingClient:
                    Reset(state);
                    MediaPlayer.Play(_gameSong);
                    break;
                case GameState.WaitingPlayers:
                    break;
                case GameState.CreatingHost:
                    Reset(state);
                    _connection.Reinitialize();
                    _connection.CreateSession();
                    break;
                case GameState.SearchingGame:
                    //_connection.SearchForGame();
                    break;
            }

            _currentState = state;
        }
        #endregion Methods

        #region Static Methods

        public static void ChangeState(GameState state)
        {
            _instance.ChangeGameState(state);
        }
        #endregion Static Methods
    }
}
