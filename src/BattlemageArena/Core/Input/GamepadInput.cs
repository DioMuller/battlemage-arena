using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattlemageArena.Core.Input
{
    /// <summary>
    /// Gamepad input.
    /// </summary>
    public class GamepadInput : GenericInput
    {
        #region Attributes
        /// <summary>
        /// Player index.
        /// </summary>
        private PlayerIndex _index;
        /// <summary>
        /// Direction diff for multiply with direction vector.
        /// </summary>
        private Vector2 directionDifference;
        #endregion Attributes

        #region Constructor
        /// <summary>
        /// Creates Gamepad Input instance for player [index].
        /// </summary>
        /// <param name="index"></param>
        public GamepadInput(PlayerIndex index) : base()
        {
            _index = index;
            directionDifference = new Vector2(1, -1);
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
                return GamePad.GetState(_index).ThumbSticks.Left * directionDifference;
            }
        }
        /// <summary>
        /// Right Directional/Stick
        /// </summary>
        public override Vector2 RightDirectional
        {
            get
            {
                return GamePad.GetState(_index).ThumbSticks.Right;
            }
        }

        /// <summary>
        /// D-Pad Left direction.
        /// </summary>
        public override ButtonState DirectionLeft
        {
            get
            {
                return GamePad.GetState(_index).DPad.Left;
            }
        }
        /// <summary>
        /// D-Pad Right direction.
        /// </summary>
        public override ButtonState DirectionRight
        {
            get
            {
                return GamePad.GetState(_index).DPad.Right;
            }
        }
        /// <summary>
        /// D-Pad Up direction.
        /// </summary>
        public override ButtonState DirectionUp
        {
            get
            {
                return GamePad.GetState(_index).DPad.Up;
            }
        }
        /// <summary>
        /// D-Pad Down direction.
        /// </summary>
        public override ButtonState DirectionDown
        {
            get
            {
                return GamePad.GetState(_index).DPad.Down;
            }
        }

        /// <summary>
        /// Face Button on the A position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonA
        {
            get
            {
                return GamePad.GetState(_index).Buttons.A;
            }
        }
        /// <summary>
        /// Face Button on the B position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonB
        {
            get
            {
                return GamePad.GetState(_index).Buttons.B;
            }
        }
        /// <summary>
        /// Face Button on the X position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonX
        {
            get
            {
                return GamePad.GetState(_index).Buttons.X;
            }
        }
        /// <summary>
        /// Face Button on the Y position (On the Xbox 360 Controller). 
        /// </summary>
        public override ButtonState FaceButtonY
        {
            get
            {
                return GamePad.GetState(_index).Buttons.Y;
            }
        }

        /// <summary>
        /// Start Button
        /// </summary>
        public override ButtonState StartButton
        {
            get
            {
                return GamePad.GetState(_index).Buttons.Start;
            }
        }

        /// <summary>
        /// Select Button
        /// </summary>
        public override ButtonState SelectButton
        {
            get
            {
                return GamePad.GetState(_index).Buttons.Back;
            }
        }

        /// <summary>
        /// Button on the Left Bumper position. 
        /// </summary>
        public override ButtonState LeftBumper
        {
            get
            {
                return GamePad.GetState(_index).Buttons.LeftShoulder;
            }
        }
        /// <summary>
        /// Button on the Right Bumper position. 
        /// </summary>
        public override ButtonState RightBumper
        {
            get
            {
                return GamePad.GetState(_index).Buttons.RightShoulder;
            }
        }

        /// <summary>
        /// Left directional click.
        /// </summary>
        public override ButtonState LeftClick
        {
            get
            {
                return GamePad.GetState(_index).Buttons.LeftStick;
            }
        }

        /// <summary>
        /// Right directional click.
        /// </summary>
        public override ButtonState RightClick
        {
            get
            {
                return GamePad.GetState(_index).Buttons.RightStick;
            }
        }

        /// <summary>
        /// Button on the Left Trigger position. 
        /// </summary>
        public override float LeftTrigger
        {
            get
            {
                return GamePad.GetState(_index).Triggers.Left;
            }
        }
        /// <summary>
        /// Button on the Right Trigger position. 
        /// </summary>
        public override float RightTrigger
        {
            get
            {
                return GamePad.GetState(_index).Triggers.Right;
            }
        }
        #endregion Properties
    }
}
