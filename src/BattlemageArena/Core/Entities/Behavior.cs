using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.Core.Entities
{
    /// <summary>
    /// Entity base behavior.
    /// </summary>
    public abstract class Behavior
    {
        #region Properties
        /// <summary>
        /// Parent entity.
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// Is the behavior active?
        /// </summary>
        public bool IsActive { get; set; }
        #endregion Properties

        #region Constructor
        public Behavior( Entity parent )
        {
            Entity = parent;
            IsActive = true;
        }
        #endregion Constructor

        #region Methods
        #region Virtual Methods
        /// <summary>
        /// This method is called when the behavior is attached to an entity.
        /// </summary>
        public virtual void Attached() { }
                
        /// <summary>
        /// Method to be executed when the behavior is deactivated.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Deactivated(GameTime gameTime) { }
        #endregion Virtual Methods

        #region Abstract Methods
        /// <summary>
        /// Executes an update logic on the attached entity.
        /// </summary>
        /// <param name="deltaTime"></param>
        public abstract void Update(GameTime gameTime);
        #endregion Abstract Methods

        #endregion Methods
    }
}
