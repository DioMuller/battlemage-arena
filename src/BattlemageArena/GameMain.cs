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

        private GameState _currentState;

        private readonly NetworkConnection _connection;

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
                    _title.Draw(gameTime, _spriteBatch);
                    break;
                case GameState.PlayingLocal:
                    _level.Draw(gameTime, _spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion Game Override Methods

        #region Methods

        internal void Reset()
        {
            _level = new Level("Images/arena", Width, Height, PlayerCount, UseKeyboard );
        }

        private void ChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.TitleScreen:
                    Reset();
                    _currentState = GameState.TitleScreen;
                    MediaPlayer.Play(_titleSong);
                    break;
                case GameState.PlayingLocal:
                        Reset();
                        _currentState = GameState.PlayingLocal;
                        MediaPlayer.Play(_gameSong);
                    break;
            }
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
