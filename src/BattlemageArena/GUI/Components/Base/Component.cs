﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattlemageArena.GUI.Components
{
    #region Delegates
    /// <summary>
    /// Called when the component is selected.
    /// </summary>
    public delegate void SelectDelegate();

    /// <summary>
    /// Selection was changed.
    /// </summary>
    /// <param name="value">Change value.</param>
    public delegate void SelectionChangeDelegate(float value);
    #endregion Delegates

    public class Component
    {
        #region Properties
        /// <summary>
        /// Is the component currently selected?
        /// </summary>
        public bool Selected { get; set; }
        
        /// <summary>
        /// Will component highlight when selected? 
        /// </summary>
        public bool Selectable { get; set; }
        
        /// <summary>
        /// Component Position.
        /// </summary>
        public Rectangle Position { get; set; }
        /// <summary>
        /// Selected texture.
        /// </summary>
        public Texture2D SelectedTexture { get; set; }

        /// <summary>
        /// Component identifier.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Is the component visible?
        /// </summary>
        public bool Drawable { get; set; }

        public bool Visible
        {
            get { return Drawable && Selectable; }
            set { Drawable = Selectable = value; }
        }
        #endregion Properties

        #region Delegates
        public SelectDelegate Select { get; protected set; }

        public SelectionChangeDelegate SelectionChanged { get; protected set; }
        #endregion Delegates

        #region Constructor
        /// <summary>
        /// Creates component and initializes the texture with a default value.
        /// </summary>
        public Component(string name)
        {
            SelectedTexture = GameContent.LoadContent<Texture2D>("images/menuselected");
            Name = name;
            Visible = true;
        }
        #endregion Constructor
        
        #region Methods
        /// <summary>
        /// Draw the component
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch, float transparency = 1.0f)
        {
            if( Selected && Selectable ) spriteBatch.Draw(SelectedTexture, Position, Color.White * transparency);
        }
        #endregion Methods
    }
}
