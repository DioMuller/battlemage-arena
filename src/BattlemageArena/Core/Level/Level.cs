using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattlemageArena.Core.Level
{
    class Level : DrawableGameComponent
    {
        #region Attributes
        private Rectangle _bounds;
        #endregion Attributes

        #region Constructor

        public Level(Game game, int width, int height) : base(game)
        {
            
        }
        #endregion Constructor
    }
}
