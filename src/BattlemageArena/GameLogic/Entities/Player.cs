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

        private int _health = 5;
        /// <summary>
        /// Shot delay time.
        /// </summary>
        private float _delayTime = 0.0f;

        private SpriteFont _font;
        private Vector2 _nameSize;
        private Vector2 _healthSize;
        private Vector2 _textDiff;

        private SoundEffect _fireballSfx;
        private SoundEffect _deathSfx;
        private SoundEffect _hitSfx;

        private float _dyingTime = 0.0f;
        #endregion

        #region Properties
        /// <summary>
        /// Player Movement Speed
        /// </summary>
        public float MovementSpeed { get; set; }

        public string Name { get; set; }

        public bool Dead { get; set; }
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

            Sprite.Animations.Add(new Animation("walking_down", 2, 0, 8));
            Sprite.Animations.Add(new Animation("walking_up", 0, 0, 8));
            Sprite.Animations.Add(new Animation("walking_right", 3, 0, 8));
            Sprite.Animations.Add(new Animation("walking_left", 1, 0, 8));

            Sprite.Animations.Add(new Animation("fireball_down", 6, 0, 6));
            Sprite.Animations.Add(new Animation("fireball_up", 4, 0, 6));
            Sprite.Animations.Add(new Animation("fireball_right", 7, 0, 6));
            Sprite.Animations.Add(new Animation("fireball_left", 5, 0, 6));

            Sprite.Animations.Add(new Animation("dying", 8, 0, 5));
            Sprite.Animations.Add(new Animation("dead", 8, 5, 5));

            _currentDirection = Direction.Down;
            _level = level;

            _delayTime = 500.0f;

            _input = inputMethod;

            Dead = false;
            
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
            base.Update(gameTime);
            #region Dying

            if (Dead)
            {
                if (_dyingTime > 0.0f)
                {
                        _dyingTime -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    Sprite.ChangeAnimation(9);
                }
                return;
            }

            if (_health <= 0)
            {
                Sprite.ChangeAnimation(8);
                _dyingTime = 400.0f;
                Dead = true;
                _deathSfx.Play();
                return;
            }
            #endregion Dying

            if (_delayTime > 0.0f)
            {
                _delayTime -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {

                #region Movement

                Move(_input.LeftDirectional*gameTime.ElapsedGameTime.Milliseconds*MovementSpeed);

                #endregion Movement

                #region Shooting

                Sprite.ChangeAnimation((int) _currentDirection);

                if (_input.FaceButtonA == ButtonState.Pressed)
                {
                    _level.AddEntity(new Fireball(_level, Position, Color, _currentDirection));
                    _fireballSfx.Play();
                    Sprite.ChangeAnimation(4 + (int) _currentDirection);
                    _delayTime = 500.0f;
                }

                #endregion Shooting
            }
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
