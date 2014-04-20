using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Sprites;
using Microsoft.Xna.Framework;

namespace BattlemageArena.Game.Entities
{
    class Player : Entity
    {
        public Player(Vector2 position, Color color)
        {
            Sprite = new Sprite("Sprites/mage", new Point(16, 16), 100);
            Sprite.Origin = new Vector2(8, 8);
            Position = position + Sprite.Origin;

            Color = color;

            Sprite.Animations.Add(new Animation("idle_down", 0, 0, 0));
            Sprite.Animations.Add(new Animation("idle_up", 0, 4, 4));
            Sprite.Animations.Add(new Animation("idle_right", 1, 0, 0));
            Sprite.Animations.Add(new Animation("idle_left", 1, 4, 4));

            Sprite.Animations.Add(new Animation("walking_down", 0, 0, 3));
            Sprite.Animations.Add(new Animation("walking_up", 0, 4, 7));
            Sprite.Animations.Add(new Animation("walking_right", 1, 0, 3));
            Sprite.Animations.Add(new Animation("walking_left", 1, 4, 7));

            Sprite.ChangeAnimation(4);
        }
    }
}
