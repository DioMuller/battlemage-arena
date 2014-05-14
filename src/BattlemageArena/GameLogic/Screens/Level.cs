using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Input;
using BattlemageArena.GameLogic.Behaviors;
using BattlemageArena.GameLogic.Entities;
using BattlemageArena.GameLogic.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattlemageArena.GameLogic.Screens
{
    public class Level
    {
        #region Attributes
        protected Rectangle _bounds;
        protected Texture2D _background;
        protected SpriteFont _font;
        protected SpriteFont _winFont;

        protected List<Entity> _entities;
        protected Stack<Entity> _toAdd;
        protected Stack<Entity> _toRemove;

        protected bool _gameEnded;
        protected Color _winnerColor;
        protected string _winnerText;
        protected Vector2 _textOrigin;
        protected Vector2 _screenCenter;
        protected float _winnerTimer;

        private SoundEffect _fireballSfx;
        #endregion Attributes

        #region Static Attributes
        private static Vector2[] positions = { new Vector2(10, 10), new Vector2(820, 460), new Vector2(10, 460), new Vector2(820, 10), new Vector2(400, 10), new Vector2(400, 460), new Vector2(10, 230), new Vector2(820,230),     };
        private static GenericInput[] inputs =
        {
            new KeyboardInput(), new GamepadInput(PlayerIndex.One),
            new GamepadInput(PlayerIndex.Two), new GamepadInput(PlayerIndex.Three), new GamepadInput(PlayerIndex.Four)
        };

        private static Color[] colors = {Color.CornflowerBlue, Color.Red, Color.Yellow, Color.Green, Color.Pink, Color.Brown, Color.Gray, Color.Orange};

        private static string[] names = {"Blue", "Red", "Yellow", "Green", "Pink", "Brown", "Gray", "Orange"};
        #endregion Static Attributes

        #region Properties

        public NetworkBehavior NetController { get; set; }
        #endregion Properties

        #region Constructor

        public Level(string background, int width, int height, int playerCount, bool useKeyboard, GameState gameType)
        {
            int diff = useKeyboard ? 0 : 1;

            if (playerCount < 2) playerCount = 2;
            if (playerCount > 4) playerCount = 4;

            _entities = new List<Entity>();
            _toRemove = new Stack<Entity>();
            _toAdd = new Stack<Entity>();

            _bounds = new Rectangle(0, 0, width, height);
            _background = GameContent.LoadContent<Texture2D>(background);
            _font = GameContent.LoadContent<SpriteFont>("Fonts/SmallFont");
            _winFont = GameContent.LoadContent<SpriteFont>("Fonts/BigFont");
            _textOrigin = Vector2.Zero;
            _screenCenter = new Vector2(width / 2.0f, height / 2.0f);

            _fireballSfx = GameContent.LoadContent<SoundEffect>("SFX/Fireball");

            _gameEnded = false;
            _winnerColor = Color.Black;
            _winnerText = String.Empty;

            if (gameType == GameState.PlayingLocal)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    _entities.Add(new Player(this, positions[i], colors[i], inputs[i + diff], names[i]));
                }
            }
            else if (gameType == GameState.PlayingHost || gameType == GameState.PlayingClient)
            {
                int mod = (gameType == GameState.PlayingHost) ? 0 : 1;
                Player local = new Player(this, positions[mod], colors[mod], inputs[diff], GameMain.CurrentSession.LocalGamers[0].DisplayName);
                local.Behaviors.Add(new NetPlayerBehavior(local));
                GameMain.Connection.CreatePlayer(local);
            }
        }
        #endregion Constructor

        #region Game Cycle Methods
        public void Update(GameTime gameTime)
        {
            #region Entity Control
            // Adds new entities.
            while (_toAdd.Count != 0)
            {
                _entities.Add(_toAdd.Pop());
            }

            // Removes old entities on to remove list.
            while (_toRemove.Count != 0)
            {
                _entities.Remove(_toRemove.Pop());
            }
            #endregion Entity Control

            #region Endgame Control
            // Checks for game exit/reset.
            if (!_gameEnded)
            {
                // Check if someone won.
                var players = _entities.OfType<Player>().Where( pl => !pl.Dead);
                int count = players.Count();
                if (count <= 1)
                {
                    if (players.Count() == 0)
                    {
                        _winnerColor = Color.White;
                        _winnerText = "Draw!";
                    }
                    else
                    {
                        Player player = players.First();
                        _winnerColor = player.Color;
                        _winnerText = player.Name + " Wins!";
                    }

                    _winnerTimer = 5000;

                    _textOrigin = (_winFont.MeasureString(_winnerText) / 2);
                    _gameEnded = true;
                }
            }
            else
            {
                _winnerTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (_winnerTimer < 0.0f)
                {
                    GameMain.ChangeState(GameState.TitleScreen);
                }
            }
            #endregion Endgame Control

            // And finally, updates all entities.
            foreach (Entity entity in _entities)
            {
                entity.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _bounds, Color.White);

            foreach (Entity entity in _entities)
            {
                entity.Draw(gameTime, spriteBatch);
            }

            if (_gameEnded)
            {
                spriteBatch.DrawString(_winFont, _winnerText, _screenCenter, _winnerColor, 0.0f, _textOrigin, Vector2.One, SpriteEffects.None, 1.0f);
            }
        }
        #endregion Game Cycle Methods

        #region Helper Methods
        public void AddEntity(Entity entity)
        {
            if (!_toAdd.Contains(entity))
                _toAdd.Push(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if( !_toRemove.Contains(entity))
                _toRemove.Push(entity);
        }

        public IEnumerable<T> GetCollisions<T>(Rectangle rect) where T : Entity
        {
            return _entities.OfType<T>().Where((ent) => ent.BoundingBox.Intersects(rect));
        }

        /// <summary>
        /// Checks if rect is on bounds.
        /// </summary>
        /// <param name="rect">Rectangle to be tested.</param>
        /// <returns>Is the rectangle on the bounds for the level?</returns>
        public bool IsOnBounds(Rectangle rect)
        {
            if (rect.X < _bounds.X || rect.Y < _bounds.Y ||
                rect.X + rect.Width > _bounds.X + _bounds.Width ||
                rect.Y + rect.Height > _bounds.Y + _bounds.Height)
            {
                return false;
            }

            return true;
        }
        #endregion Helper Methods

        #region Game Specific Methods

        public void AddFireball(Vector2 position, Color color, Direction direction)
        {
            AddEntity(new Fireball(this, position, color, direction));
            _fireballSfx.Play();
        }
        #endregion Game Specific Methods
    }
}
