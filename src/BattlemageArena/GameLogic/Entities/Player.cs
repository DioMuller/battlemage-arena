using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Input;
using BattlemageArena.Core.Level;
using BattlemageArena.Core.Sprites;
using Microsoft.Xna.Framework;
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
        #endregion

        #region Properties
        /// <summary>
        /// Player Movement Speed
        /// </summary>
        public float MovementSpeed { get; set; }
        #endregion Properties

        #region Constructors
        public Player(Level level, Vector2 position, Color color, GenericInput inputMethod)
        {
            Sprite = new Sprite("Sprites/mage", new Point(64, 64), 100);
            Sprite.Origin = new Vector2(32, 32);
            Position = position + Origin;

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

            Sprite.ChangeAnimation(0);
        }
        #endregion Constructors

        #region Methods
        public override void Update(GameTime gameTime)
        {
            #region Movement
            Move(_input.LeftDirectional * gameTime.ElapsedGameTime.Milliseconds * MovementSpeed);
            #endregion Movement

            #region Shooting

            if (_input.FaceButtonA == ButtonState.Pressed)
            {
                if (!_shot)
                {
                    _level.AddEntity(new Fireball(_level, Position, Color, _currentDirection));
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
        #endregion Methods
    }
}
