using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.Core.Sprites
{
    /// <summary>
    /// Represents an sprite animation.
    /// </summary>
    public class Animation
    {
        #region Attributes
        /// <summary>
        /// Spritesheet line.
        /// </summary>
        public int _line;

        /// <summary>
        /// Animation start.
        /// </summary>
        public int _start;

        /// <summary>
        /// Animation end.
        /// </summary>
        public int _end;

        /// <summary>
        /// Current column.
        /// </summary>
        private int _currentColumn;
        #endregion Attributes

        #region Properties
        /// <summary>
        /// Animation name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Current frame
        /// </summary>
        public Frame CurrentFrame
        {
            get
            {
                return new Frame() { Line = _line, Column = _currentColumn };
            }
        }

        /// <summary>
        /// Increments and returns next frame.
        /// </summary>
        public Frame NextFrame
        {
            get
            {
                _currentColumn++;
                if (_currentColumn > _end) _currentColumn = _start;

                return new Frame() { Line = _line, Column = _currentColumn };
            }
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Initializes animation with start and end.
        /// </summary>
        /// <param name="name">Animation name.</param>
        /// <param name="line">Spritesheet line</param>
        /// <param name="start">Spritesheet first column.</param>
        /// <param name="end">Spritesheet last column.</param>
        public Animation(string name, int line, int start, int end)
        {
            Name = name;
            _line = line;
            _start = start;
            _end = end;
            _currentColumn = _start;
        }
        #endregion Constructor
    }
}
