using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattlemageArena.Core.Input
{
    /// <summary>
    /// Generic Input class.
    /// </summary>
    public class GenericInput
    {
        #region Properties
        /// <summary>
        /// Left Directional/Stick
        /// </summary>
        public virtual Vector2 LeftDirectional
        {
            get
            {
                return Vector2.Zero;
            }
        }
        /// <summary>
        /// Right Directional/Stick
        /// </summary>
        public virtual Vector2 RightDirectional
        {
            get
            {
                return Vector2.Zero;
            }
        }

        /// <summary>
        /// D-Pad Left direction.
        /// </summary>
        public virtual ButtonState DirectionLeft
        {
            get
            {
                return ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Right direction.
        /// </summary>
        public virtual ButtonState DirectionRight
        {
            get
            {
                return ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Up direction.
        /// </summary>
        public virtual ButtonState DirectionUp
        {
            get
            {
                return ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Down direction.
        /// </summary>
        public virtual ButtonState DirectionDown
        {
            get
            {
                return ButtonState.Released;
            }
        }

        /// <summary>
        /// Face Button on the A position (On the Xbox 360 Controller). 
        /// </summary>
        public virtual ButtonState FaceButtonA
        {
            get
            {
                return ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the B position (On the Xbox 360 Controller). 
        /// </summary>
        public virtual ButtonState FaceButtonB
        {
            get
            {
                return ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the X position (On the Xbox 360 Controller). 
        /// </summary>
        public virtual ButtonState FaceButtonX
        {
            get
            {
                return ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the Y position (On the Xbox 360 Controller). 
        /// </summary>
        public virtual ButtonState FaceButtonY
        {
            get
            {
                return ButtonState.Released;
            }
        }

        /// <summary>
        /// Left directional click.
        /// </summary>
        public virtual ButtonState LeftClick
        {
            get
            {
                return ButtonState.Released;
            }
        }

        /// <summary>
        /// Right directional click.
        /// </summary>
        public virtual ButtonState RightClick
        {
            get
            {
                return ButtonState.Released;
            }
        }

        /// <summary>
        /// Start Button
        /// </summary>
        public virtual ButtonState StartButton
        {
            get
            {
                return ButtonState.Released;
            }
        }

        /// <summary>
        /// Select Button
        /// </summary>
        public virtual ButtonState SelectButton
        {
            get
            {
                return ButtonState.Released;
            }
        }

        /// <summary>
        /// Button on the Left Bumper position. 
        /// </summary>
        public virtual ButtonState LeftBumper
        {
            get
            {
                return ButtonState.Released;
            }
        }
        /// <summary>
        /// Button on the Right Bumper position. 
        /// </summary>
        public virtual ButtonState RightBumper
        {
            get
            {
                return ButtonState.Released;
            }
        }

        /// <summary>
        /// Button on the Left Trigger position. 
        /// </summary>
        public virtual float LeftTrigger
        {
            get
            {
                return 0.0f;
            }
        }
        /// <summary>
        /// Button on the Right Trigger position. 
        /// </summary>
        public virtual float RightTrigger
        {
            get
            {
                return 0.0f;
            }
        }
        #endregion Properties
    }
}
