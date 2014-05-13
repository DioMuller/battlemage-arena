using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattlemageArena.Core.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.Core.Entities
{
    /// <summary>
    /// Game Entity.
    /// </summary>
    public class Entity
    {
        #region Attributes
        /// <summary>
        /// Children to be removed.
        /// </summary>
        public Stack<Entity> _childrenToRemove;
        #endregion Attributes

        #region Properties
        /// <summary>
        /// Parent entity.
        /// </summary>
        public Entity Parent { get; set; }

        /// <summary>
        /// A list of this entity children.
        /// </summary>
        public List<Entity> Children { get; set; }

        /// <summary>
        /// Entity tag. Can be used to describe entity or as an auxiliary property.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Contains all the entities behavior, that will be used during the entity's update logic.
        /// </summary>
        public List<Behavior> Behaviors { get; private set; }

        /// <summary>
        /// Entity position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Entity rotation.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Entity sprite.
        /// </summary>
        public Sprite Sprite { get; set; }

        /// <summary>
        /// Entity follows parent?
        /// </summary>
        public bool FollowParent { get; set; }

        /// <summary>
        /// Draw entity?
        /// </summary>
        public bool Visible { get; set; }

        public Color Color { get; set; }

        public bool Removed { get; set; }

        public Vector2 Size
        {
            get
            {
                return new Vector2(Sprite.FrameSize.X, Sprite.FrameSize.Y);
            }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(Convert.ToInt32(Position.X - Sprite.Origin.X),
                                     Convert.ToInt32(Position.Y - Sprite.Origin.Y),
                                     Sprite.FrameSize.X,
                                     Sprite.FrameSize.Y);
            }
        }


        public Vector2 Origin
        {
            get { return Sprite.Origin; }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Creates entity with default parameters.
        /// </summary>
        public Entity()
        {
            Behaviors = new List<Behavior>();
            Children = new List<Entity>();
            _childrenToRemove = new Stack<Entity>();

            Visible = true;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Entity update method.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            Vector2 oldPosition = Position;

            while (_childrenToRemove.Count != 0)
            {
                Children.Remove(_childrenToRemove.Pop());
            }

            foreach (Behavior b in Behaviors)
            {
                if (b.IsActive) b.Update(gameTime);
            }

            foreach (Entity e in Children)
            {
                e.Update(gameTime);
            }

            Sprite.Update(gameTime);
        }


        /// <summary>
        /// Entity drawing method.
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        /// <param name="spriteBatch">SpriteBatch to be used for drawing.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Vector2 offset = (Parent != null && FollowParent) ? Parent.Position : Vector2.Zero;

                Sprite.Draw(gameTime, spriteBatch, Position + offset, Color, Rotation);

                foreach (Entity e in Children)
                {
                    e.Draw(gameTime, spriteBatch);
                }

                if (Color == Color.Transparent)
                {
                    Color = Color.White;
                }
            }
        }

        /// <summary>
        /// Gets behavior by type.
        /// </summary>
        /// <typeparam name="T">Behavior type</typeparam>
        /// <returns>Behavior of type or null.</returns>
        public T GetBehavior<T>() where T : Behavior
        {
            return Behaviors.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets children by type.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Entity of type or null.</returns>
        public T GetChildren<T>() where T : Entity
        {
            return Children.OfType<T>().First();
        }

        /// <summary>
        /// Removes children
        /// </summary>
        /// <param name="children">Children to be removed.</param>
        public void RemoveChildren(Entity children)
        {
            _childrenToRemove.Push(children);
        }
        #endregion Methods
    }
}
