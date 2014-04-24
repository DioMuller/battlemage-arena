using System;
using System.Collections;
using System.Collections.Generic;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Sprites;
using BattlemageArena.GameLogic.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattlemageArena.Core.Extension;

namespace BattlemageArena.GameLogic.Entities
{
    public class Fireball : Entity
    {
        #region Attributes
        private Level _level;
        private Vector2 _direction;
        private float _movementSpeed = 0.3f;
        #endregion Attributes

        #region Constructor
        public Fireball(Level level, Vector2 position, Color color, Direction direction)
        {
            Sprite = new Sprite("Sprites/fireball", new Point(48, 48), 100);
            Sprite.Origin = new Vector2(24,24);
            Position = position;

            Color = color;

            Sprite.Animations.Add(new Animation("only", 0, 0, 0));

            switch (direction)
            {
                case Direction.Up:
                    Rotation = 180;
                    break;
                case Direction.Left:
                    Rotation = 90;
                    break;
                case Direction.Right:
                    Rotation = 270;
                    break;
            }

            _level = level;

            // Vector rotation is in Degrees.
            _direction = Vector2.UnitY.Rotate(Rotation);

            Sprite.Effect = SpriteEffects.FlipVertically;

            // Sprite rotation is in Radians.
            Rotation = (float) Math.PI * Rotation / 180.0f;
            
            Sprite.ChangeAnimation(0);
        }
        #endregion Constructor

        #region Methods

        public override void Update(GameTime gameTime)
        {
            // Update position and bounding box
            Position += (_direction*gameTime.ElapsedGameTime.Milliseconds*_movementSpeed);
            Rectangle bbox = BoundingBox;

            // Checks if is on level and destroys if not.
            if (!_level.IsOnBounds(bbox))
            {
                _level.RemoveEntity(this);
            }
            
            // Checks if collides with player of other team
            IEnumerable<Player> collisions = _level.GetCollisions<Player>(bbox);

            foreach (Player player in collisions)
            {
                if (player.Color != this.Color)
                {
                    player.Hurt();
                    _level.RemoveEntity(this);
                }
            }
            
            base.Update(gameTime);
        }

        #endregion Methods
    }
}