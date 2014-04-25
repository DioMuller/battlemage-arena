using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattlemageArena.Core.Input
{
    /// <summary>
    /// Multiple Input class. Probably very costly, use on
    /// very specific cases, like menus.
    /// </summary>
    internal class MultipleInput : GenericInput
    {
        #region Static

        private static MultipleInput _instance;

        public static MultipleInput GetInstance()
        {
            if( _instance == null ) _instance = new MultipleInput();

            return _instance;
        }
        #endregion Static

        #region Attributes

        private List<GenericInput> _inputs;

        #endregion Attributes

        #region Constructors
        private MultipleInput() : base()
        {
            _inputs = new List<GenericInput>();
            _inputs.Add(new KeyboardInput());
            _inputs.Add(new GamepadInput(PlayerIndex.One));
            _inputs.Add(new GamepadInput(PlayerIndex.Two));
            _inputs.Add(new GamepadInput(PlayerIndex.Three));
            _inputs.Add(new GamepadInput(PlayerIndex.Four));
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Left Directional/Stick
        /// </summary>
        public override Vector2 LeftDirectional
        {
            get
            {
                var selected =
                    _inputs.Where(
                        (input) =>
                            (Math.Abs(input.LeftDirectional.X) > 0.1f || Math.Abs(input.LeftDirectional.Y) > 0.1f));
                return selected.Count() > 0 ? selected.First().LeftDirectional : Vector2.Zero;
            }
        }
        /// <summary>
        /// Right Directional/Stick
        /// </summary>
        public override Vector2 RightDirectional
        {
            get
            {
                var selected =
                _inputs.Where(
                    (input) =>
                        (Math.Abs(input.RightDirectional.X) > 0.1f || Math.Abs(input.RightDirectional.Y) > 0.1f));
                return selected.Count() > 0 ? selected.First().RightDirectional : Vector2.Zero;
            }
        }

        /// <summary>
        /// D-Pad Left direction.
        /// </summary>
        public override ButtonState DirectionLeft
        {
            get
            {
                return _inputs.Any((input) => input.DirectionLeft == ButtonState.Pressed)
                    ? ButtonState.Pressed
                    : ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Right direction.
        /// </summary>
        public override ButtonState DirectionRight
        {
            get
            {
                return _inputs.Any((input) => input.DirectionRight == ButtonState.Pressed)
                    ? ButtonState.Pressed
                    : ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Up direction.
        /// </summary>
        public override ButtonState DirectionUp
        {
            get
            {
                return _inputs.Any((input) => input.DirectionUp == ButtonState.Pressed)
                    ? ButtonState.Pressed
                    : ButtonState.Released;
            }
        }
        /// <summary>
        /// D-Pad Down direction.
        /// </summary>
        public override ButtonState DirectionDown
        {
            get
            {
                return _inputs.Any((input) => input.DirectionDown == ButtonState.Pressed)
                    ? ButtonState.Pressed
                    : ButtonState.Released;
            }
        }

        /// <summary>
        /// Face Button on the A position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonA
        {
            get
            {
                return _inputs.Any((input) => input.FaceButtonA == ButtonState.Pressed)
                    ? ButtonState.Pressed
                    : ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the B position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonB
        {
            get
            {
                return _inputs.Any((input) => input.FaceButtonB == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the X position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonX
        {
            get
            {
                return _inputs.Any((input) => input.FaceButtonX == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }
        /// <summary>
        /// Face Button on the Y position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonY
        {
            get
            {
                return _inputs.Any((input) => input.FaceButtonY == ButtonState.Pressed)
                 ? ButtonState.Pressed
                 : ButtonState.Released;
            }
        }

        /// <summary>
        /// Left directional click.
        /// </summary>
        public override ButtonState LeftClick
        {
            get
            {
                return _inputs.Any((input) => input.LeftClick == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }

        /// <summary>
        /// Right directional click.
        /// </summary>
        public override ButtonState RightClick
        {
            get
            {
                return _inputs.Any((input) => input.RightClick == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }

        /// <summary>
        /// Start Button
        /// </summary>
        public override ButtonState StartButton
        {
            get
            {
                return _inputs.Any((input) => input.StartButton == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }

        /// <summary>
        /// Select Button
        /// </summary>
        public override ButtonState SelectButton
        {
            get
            {
                return _inputs.Any((input) => input.SelectButton == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }

        /// <summary>
        /// Button on the Left Bumper position. 
        /// </summary>
        public override ButtonState LeftBumper
        {
            get
            {
                return _inputs.Any((input) => input.LeftBumper == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }
        /// <summary>
        /// Button on the Right Bumper position. 
        /// </summary>
        public override ButtonState RightBumper
        {
            get
            {
                return _inputs.Any((input) => input.RightBumper == ButtonState.Pressed)
                     ? ButtonState.Pressed
                     : ButtonState.Released;
            }
        }

        /// <summary>
        /// Button on the Left Trigger position. 
        /// </summary>
        public override float LeftTrigger
        {
            get
            {
                return _inputs.Max((input) => input.LeftTrigger);
            }
        }
        /// <summary>
        /// Button on the Right Trigger position. 
        /// </summary>
        public override float RightTrigger
        {
            get
            {
                return _inputs.Max((input) => input.RightTrigger);
            }
        }
        #endregion Properties
    }
}
