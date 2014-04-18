using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.Core.Sprites
{
    /// <summary>
    /// Represents a frame (line and column) on a spritesheet.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// Spritesheet line.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Spritesheet column.
        /// </summary>
        public int Column { get; set; }
    }
}
