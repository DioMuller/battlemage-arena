using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.Core.Input
{
    public class KeyboardInput : GenericInput
    {
        #region Constructor
        /// <summary>
        /// Creates Keyboard Input instance for player [index].
        /// </summary>
        public KeyboardInput()
            : base()
        {
        }
        #endregion Constructor

        #region Properties
        /// <summary>
        /// Left Directional/Stick
        /// </summary>
        public override Vector2 LeftDirectional
        {
            get
            {
                Vector2 direction = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) direction.Y -= 1f;
                if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) direction.Y += 1f;
                if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) direction.X -= 1f;
                if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) direction.X += 1f;

                if( direction != Vector2.Zero ) direction.Normalize();
                return direction;
            }
        }
        /// <summary>
        /// Right Directional/Stick
        /// </summary>
        public override Vector2 RightDirectional
        {
            get
            {
                Vector2 direction = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.Up)) direction.Y -= 1f;
                if (Keyboard.GetState().IsKeyDown(Keys.Down)) direction.Y += 1f;
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) direction.X -= 1f;
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) direction.X += 1f;

                return direction;
            }
        }

        /// <summary>
        /// D-Pad Left direction.
        /// </summary>
        public override ButtonState DirectionLeft
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left) ? ButtonState.Pressed : ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Right direction.
        /// </summary>
        public override ButtonState DirectionRight
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right) ? ButtonState.Pressed : ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Up direction.
        /// </summary>
        public override ButtonState DirectionUp
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up) ? ButtonState.Pressed : ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Down direction.
        /// </summary>
        public override ButtonState DirectionDown
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down) ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        /// <summary>
        /// Face Button on the A position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonA
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.Z) || Keyboard.GetState().IsKeyDown(Keys.Space) ? ButtonState.Pressed : ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the B position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonB
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.X) ? ButtonState.Pressed : ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the X position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonX
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.C) ? ButtonState.Pressed : ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the Y position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonY
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.V) ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        /// <summary>
        /// Start Button
        /// </summary>
        public override ButtonState StartButton
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.Escape) ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        /// <summary>
        /// Select Button
        /// </summary>
        public override ButtonState SelectButton
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.Tab) ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        /// <summary>
        /// Button on the Left Bumper position. 
        /// </summary>
        public override ButtonState LeftBumper
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? ButtonState.Pressed : ButtonState.Released;
            }
        }
        /// <summary>
        /// Button on the Right Bumper position. 
        /// </summary>
        public override ButtonState RightBumper
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.RightShift) ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        /// <summary>
        /// Left directional click.
        /// </summary>
        public override ButtonState LeftClick
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.OemComma) ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        /// <summary>
        /// Right directional click.
        /// </summary>
        public override ButtonState RightClick
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.OemPeriod) ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        /// <summary>
        /// Button on the Left Trigger position. 
        /// </summary>
        public override float LeftTrigger
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.Q) ? 1f : 0f;
            }
        }
        /// <summary>
        /// Button on the Right Trigger position. 
        /// </summary>
        public override float RightTrigger
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.E) ? 1f : 0f;
            }
        }
        #endregion Properties
    }
}
