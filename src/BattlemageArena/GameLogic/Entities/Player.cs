using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Input;
using BattlemageArena.Core.Level;
using BattlemageArena.Core.Sprites;
using BattlemageArena.GameLogic.Behaviors;
using Microsoft.Xna.Framework;

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
        #endregion

        #region Constructors
        public Player(Level level, Vector2 position, Color color, GenericInput inputMethod)
        {
            Sprite = new Sprite("Sprites/mage", new Point(64, 64), 100);
            Sprite.Origin = new Vector2(0,0);
            Position = position;

            Color = color;

            Sprite.Animations.Add(new Animation("walking_down", 0, 0, 3));
            Sprite.Animations.Add(new Animation("walking_up", 0, 4, 7));
            Sprite.Animations.Add(new Animation("walking_right", 1, 0, 3));
            Sprite.Animations.Add(new Animation("walking_left", 1, 4, 7));

            _currentDirection = Direction.Down;
            _level = level;

            Sprite.ChangeAnimation(0);

            Behaviors.Add(new ControllableBehavior(this, inputMethod));
        }
        #endregion Constructors

        #region Methods
        public override void Update(GameTime gameTime)
        {
            Vector2 oldPosition = Position;

            base.Update(gameTime);

            Vector2 positionDiff = Position - oldPosition;
            Direction newDirection = _currentDirection;

            if (positionDiff == Vector2.Zero) return;

            if (Math.Abs(positionDiff.X) > Math.Abs(positionDiff.Y))
            {
                if(positionDiff.X < 0) newDirection = Direction.Left;
                else newDirection = Direction.Right;
            }
            else
            {
                if (positionDiff.Y < 0) newDirection = Direction.Up;
                else newDirection = Direction.Down;                
            }

            if (newDirection != _currentDirection)
            {
                _currentDirection = newDirection;
                Sprite.ChangeAnimation((int) _currentDirection);
            }
        }

        public void Move(Vector2 movement)
        {
            Vector2 newPosition = (Position) + movement;

            if (_level.IsOnBounds(new Rectangle((int) newPosition.X, (int) newPosition.Y, 
                                    64, 64)))
            {
                Position = newPosition;
            }
        }
        #endregion Methods
    }
}
