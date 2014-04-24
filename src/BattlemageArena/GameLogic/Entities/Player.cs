using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Input;
using BattlemageArena.Core.Sprites;
using BattlemageArena.GameLogic.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattlemageArena.GameLogic.Entities
{
    public enum Direction
    {
        Down = 0,
        Up = 1,
        Right = 2,
        Left = 3
    }

    class Player : Entity
    {
        #region Attributes
        /// <summary>
        /// Current player direction.
        /// </summary>
        private Direction _currentDirection;

        /// <summary>
        /// Level where the player is playing.
        /// </summary>
        private Level _level;

        /// <summary>
        /// Input
        /// </summary>
        private GenericInput _input;

        /// <summary>
        /// Has the player shot?
        /// </summary>
        private bool _shot;

        private int _health = 5;

        private SpriteFont _font;
        private Vector2 _nameSize;
        private Vector2 _healthSize;
        private Vector2 _textDiff;

        private SoundEffect _fireballSfx;
        private SoundEffect _deathSfx;
        private SoundEffect _hitSfx;
        #endregion

        #region Properties
        /// <summary>
        /// Player Movement Speed
        /// </summary>
        public float MovementSpeed { get; set; }

        public string Name { get; set; }
        #endregion Properties

        #region Constructors
        public Player(Level level, Vector2 position, Color color, GenericInput inputMethod, string name)
        {
            Sprite = new Sprite("Sprites/mage", new Point(64, 64), 100);
            Sprite.Origin = new Vector2(32, 32);
            Position = position + Origin;
            Name = name;

            _font = GameContent.LoadContent<SpriteFont>("Fonts/SmallFont");
            _nameSize = _font.MeasureString(Name) / 2;
            _healthSize = _font.MeasureString(_health.ToString()) / 2;
            _textDiff = Vector2.UnitY * (Size.Y / 2);

            Color = color;

            Sprite.Animations.Add(new Animation("walking_down", 0, 0, 3));
            Sprite.Animations.Add(new Animation("walking_up", 0, 4, 7));
            Sprite.Animations.Add(new Animation("walking_right", 1, 0, 3));
            Sprite.Animations.Add(new Animation("walking_left", 1, 4, 7));

            _currentDirection = Direction.Down;
            _level = level;

            _shot = false;

            _input = inputMethod;
            
            //Player didn't set movement speed
            if (MovementSpeed < 0.0001f) MovementSpeed = 0.2f;

            _deathSfx = GameContent.LoadContent<SoundEffect>("SFX/Death");
            _fireballSfx = GameContent.LoadContent<SoundEffect>("SFX/Fireball");
            _hitSfx = GameContent.LoadContent<SoundEffect>("SFX/Explosion");

            Sprite.ChangeAnimation(0);
        }
        #endregion Constructors

        #region Methods
        public override void Update(GameTime gameTime)
        {
            #region Dying
            if (_health <= 0)
            {
                _level.RemoveEntity(this);
                _deathSfx.Play();
                return;
            }
            #endregion Dying

            #region Movement
            Move(_input.LeftDirectional * gameTime.ElapsedGameTime.Milliseconds * MovementSpeed);
            #endregion Movement

            #region Shooting
            if (_input.FaceButtonA == ButtonState.Pressed)
            {
                if (!_shot)
                {
                    _level.AddEntity(new Fireball(_level, Position, Color, _currentDirection));
                    _fireballSfx.Play();
                    _shot = true;
                }
            }
            else
            {
                _shot = false;
            }
            #endregion Shooting

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, Name, Position - _textDiff, Color, 0.0f, _nameSize, Vector2.One, SpriteEffects.None, 1.0f );
            spriteBatch.DrawString(_font, _health.ToString(), Position + _textDiff, Color, 0.0f, _healthSize, Vector2.One, SpriteEffects.None, 1.0f);
        }

        public void Move(Vector2 movement)
        {
            Vector2 newPosition = (Position) + movement;

            if (_level.IsOnBounds(new Rectangle((int) (newPosition.X - Origin.X), (int) (newPosition.Y - Origin.Y),
                                    (int) Size.X, (int) Size.Y)))
            {

                Direction newDirection = _currentDirection;

                if (movement == Vector2.Zero) return;

                if (Math.Abs(movement.X) > Math.Abs(movement.Y))
                {
                    if (movement.X < 0) newDirection = Direction.Left;
                    else newDirection = Direction.Right;
                }
                else
                {
                    if (movement.Y < 0) newDirection = Direction.Up;
                    else newDirection = Direction.Down;
                }

                if (newDirection != _currentDirection)
                {
                    _currentDirection = newDirection;
                    Sprite.ChangeAnimation((int)_currentDirection);
                }

                Position = newPosition;
            }
        }

        public void Hurt()
        {
            _health--;
            _hitSfx.Play();
        }
        #endregion Methods
    }
}
