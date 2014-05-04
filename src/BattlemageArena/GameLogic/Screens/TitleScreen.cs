using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core;
using BattlemageArena.Core.Input;
using BattlemageArena.Core.Sprites;
using BattlemageArena.GameLogic.Entities;
using BattlemageArena.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BattlemageArena.GameLogic.Screens
{
    class TitleScreen
    {
        #region Attributes
        private Rectangle _bounds;
        private Rectangle _titlePosition;

        private Texture2D _background;
        private SpriteFont _font;
        private Texture2D _logo;

        private string _startMessage;
        private Vector2 _startPosition;
        private Vector2 _startOrigin;

        private float _transparency;
        private float _transparencyDiff;

        private ComponentList _options;
        private bool _showOptions;

        private bool _firstKeyUp;

        #endregion Attributes

        #region Constructor
        public TitleScreen(string background, int width, int height)
        {
            #region Title Screen Assets initialization
            _bounds = new Rectangle(0, 0, width, height);
            _background = GameContent.LoadContent<Texture2D>(background);
            _logo = GameContent.LoadContent<Texture2D>("Images/Logo");
            _font = GameContent.LoadContent<SpriteFont>("Fonts/SmallFont");

            Vector2 bg_center = new Vector2(_logo.Width / 2.0f, _logo.Height / 2.0f);
            Vector2 screen_center = new Vector2(width / 2.0f, height / 2.0f);
            _titlePosition = new Rectangle( (int) (screen_center.X - bg_center.X), 
                                            (int) (screen_center.Y * 0.5 - bg_center.Y),
                                            (int)(_logo.Width),
                                            (int)(_logo.Height));

            _startMessage = "Press START or ENTER to start the game";
            Vector2 startSize = _font.MeasureString(_startMessage);
            _startOrigin = new Vector2(startSize.X / 2, startSize.Y / 2);
            _startPosition = new Vector2(screen_center.X, screen_center.Y * 1.7f);

            _transparency = 0.9f;
            _transparencyDiff = 0.001f;
            #endregion Title Screen Assets initialization

            #region Options Initialization
            _options = new ComponentList();
            
            SelectionBox controller = new SelectionBox("Use Keyboard?");
            controller.AddOption("Yes");
            controller.AddOption("No");
            controller.SelectOption(GameMain.UseKeyboard ? "Yes" : "No");
            _options.AddComponent(controller);

            SelectionBox players = new SelectionBox("Players");
            players.AddOption("2");
            players.AddOption("3");
            players.AddOption("4");
            players.SelectOption(GameMain.PlayerCount.ToString());
            _options.AddComponent(players);

            _options.ValueChanged += () =>
            {
                string useKeyboard = _options.GetValue("Use Keyboard?");
                string playersCount = _options.GetValue("Players");

                GameMain.UseKeyboard = (useKeyboard == "Yes");
                GameMain.PlayerCount = int.Parse(playersCount);
            };

            _options.AddComponent(new Button("Search For Game", () =>
            {
                // TODO: Add network logic.
                _showOptions = false;
                GameMain.StartGame();
            }));

            _options.AddComponent(new Button("Host Game", () =>
            {
                // TODO: Add network logic.
                _showOptions = false;
                GameMain.StartGame();
            }));

            _options.AddComponent(new Button("Local Game", () =>
            {
                _showOptions = false;
                GameMain.StartGame();
            }));

            _options.Position = new Rectangle(30, 200, width - 60, height - 220);
            _showOptions = false;
            #endregion Options Initialization

            _firstKeyUp = false;
        }
        #endregion Constructor

        #region Game Cycle Methods
        public void Update(GameTime gameTime)
        {
            if (_transparency < 0.0f || _transparency > 1.0f)
            {
                _transparencyDiff *= -1;
            }

            _transparency += (_transparencyDiff*gameTime.ElapsedGameTime.Milliseconds);

            if( ! _showOptions )
            {
                if (MultipleInput.GetInstance().StartButton == ButtonState.Pressed)
                {
                    ChangeState(true);
                }
                else
                {
                    _firstKeyUp = true;
                }
            }
            else
            {
                _options.Update(gameTime);

                if (MultipleInput.GetInstance().FaceButtonB == ButtonState.Pressed)
                {
                    ChangeState(false);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _bounds, Color.White);
            spriteBatch.Draw(_logo, _titlePosition, Color.White);
            if (_showOptions)
            {
                _options.Draw(gameTime, spriteBatch);
            }
            else
            {
                spriteBatch.DrawString(_font, _startMessage, _startPosition, Color.White * _transparency, 0.0f, _startOrigin, Vector2.One, SpriteEffects.None, 1.0f);   
            }
        }

        public void ChangeState(bool showOptions)
        {
            if (_firstKeyUp)
            {
                _showOptions = showOptions;
            }
        }
        #endregion Game Cycle Methods
    }
}
