using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Input;
using BattlemageArena.GameLogic.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattlemageArena.Core.Level
{
    public class Level
    {
        #region Attributes
        private Rectangle _bounds;
        private Texture2D _background;
        private List<Entity> _entities;
        private Stack<Entity> _toAdd; 
        private Stack<Entity> _toRemove; 
        #endregion Attributes

        #region Static Attributes
        private static Vector2[] positions = { new Vector2(10, 10), new Vector2(820, 460), new Vector2(10, 460), new Vector2(820, 10) };
        private static GenericInput[] inputs =
        {
            new KeyboardInput(), new GamepadInput(PlayerIndex.One),
            new GamepadInput(PlayerIndex.Two), new GamepadInput(PlayerIndex.Three), new GamepadInput(PlayerIndex.Four)
        };

        private static Color[] colors = {Color.Blue, Color.Red, Color.Yellow, Color.Green};
        #endregion Static Attributes

        #region Constructor

        public Level(string background, int width, int height, int playerCount, bool useKeyboard)
        {
            int diff = useKeyboard ? 0 : 1;

            if (playerCount < 2) playerCount = 2;
            if (playerCount > 4) playerCount = 4;

            _entities = new List<Entity>();
            _toRemove = new Stack<Entity>();
            _toAdd = new Stack<Entity>();

            _bounds = new Rectangle(0, 0, width, height);
            _background = GameContent.LoadContent<Texture2D>(background);

            for (int i = 0; i < playerCount; i++)
            {
                _entities.Add(new Player(this, positions[i], colors[i], inputs[i + diff]));
            }
        }
        #endregion Constructor

        #region Methods
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

        public void Update(GameTime gameTime)
        {
            while (_toAdd.Count != 0)
            {
                _entities.Add(_toAdd.Pop());
            }

            while (_toRemove.Count != 0)
            {
                _entities.Remove(_toRemove.Pop());
            }

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
        }

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
        #endregion Methods
    }
}
