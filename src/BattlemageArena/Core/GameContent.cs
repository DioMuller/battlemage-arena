using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace BattlemageArena.Core
{
    /// <summary>
    /// Manages Game Content.
    /// </summary>
    static public class GameContent
    {
        /// <summary>
        /// Content manager.
        /// </summary>
        private static ContentManager _content;

        /// <summary>
        /// Initializes game content.
        /// </summary>
        /// <param name="content"></param>
        static public void Initialize(ContentManager content)
        {
            _content = content;
        }

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <typeparam name="T">Content type.</typeparam>
        /// <param name="path">Content path (referent to the Content folder)</param>
        /// <returns>The desired content.</returns>
        static public T LoadContent<T>(string path)
        {
            if (_content == null) throw new InvalidOperationException("GameContent class must be initialized before its used");

            return _content.Load<T>(path);
        }
    }
}
