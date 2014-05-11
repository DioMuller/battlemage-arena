using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Input;
using BattlemageArena.Core.Sprites;
using BattlemageArena.GameLogic.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattlemageArena.GameLogic.Behaviors
{
    class ControllableBehavior : Behavior
    {
        #region Attributes
        /// <summary>
        /// Shot delay time.
        /// </summary>
        private float _delayTime = 0.0f;
        #endregion Attributes

        #region Properties
        private Player Parent { get { return Entity as Player; } }
        public GenericInput Input { get; set; }
        #endregion Properties

        #region Constructors
        public ControllableBehavior(Player parent, GenericInput input) : base(parent)
        {
            Input = input;
            _delayTime = 500.0f;
        }
        #endregion Constructors

        #region Methods

        public override void Update(GameTime gameTime)
        {
            if (Parent.Dead) return;
            if (_delayTime > 0.0f)
            {
                _delayTime -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                #region Movement

                Parent.Move(Input.LeftDirectional*gameTime.ElapsedGameTime.Milliseconds);

                #endregion Movement

                #region Shooting

                Parent.ChangeAnimation(Parent.DirectionIndex);

                if (Input.FaceButtonA == ButtonState.Pressed)
                {
                    GameMain.CurrentLevel.AddFireball(Parent.Position, Parent.Color, Parent.Direction);
                    
                    Parent.ChangeAnimation(4 + Parent.DirectionIndex);
                    _delayTime = 500.0f;
                }

                #endregion Shooting
            }
        }

        #endregion Methods
    }
}
