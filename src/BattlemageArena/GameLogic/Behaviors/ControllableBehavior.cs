using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.Core.Input;
using Microsoft.Xna.Framework;

namespace BattlemageArena.GameLogic.Behaviors
{
    public class ControllableBehavior : Behavior
    {
        #region Attributes
        private GenericInput _input;
        #endregion Attributes

        #region Properties
        public float MovementSpeed { get; set; }
        #endregion Properties

        #region Constructors
        public ControllableBehavior(Entity parent, GenericInput input) : base(parent)
        {
            this._input = input;

            //Player didn't set movement speed
            if (MovementSpeed < 0.0001f) MovementSpeed = 0.2f;
        }
        #endregion Constructors

        #region Methods
        public override void Update(GameTime gameTime)
        {
            this.Entity.Position += _input.LeftDirectional * gameTime.ElapsedGameTime.Milliseconds * MovementSpeed;
        }
        #endregion Methods
    }
}
