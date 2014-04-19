using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlemageArena.Core.Sprites
{
    /// <summary>
    /// Sprite drawing/position/texture class.
    /// </summary>
    public class Sprite
    {
        #region Attributes
        /// <summary>
        /// Sprite texture.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Current animation
        /// </summary>
        private int _currentAnimation;

        /// <summary>
        /// Current frame time.
        /// </summary>
        private float _currentFrameTime;

        /// <summary>
        /// Current frame.
        /// </summary>
        private Frame _currentFrame;

        /// <summary>
        /// Source rect, what on the spritesheet will be drawn.
        /// </summary>
        private Rectangle _sourceRect;

        /// <summary>
        /// Sprite center.
        /// </summary>
        private Vector2 _center;
        #endregion Attributes

        #region Properties
        /// <summary>
        /// Frame Size.
        /// </summary>
        public Point FrameSize { get; private set; }

        /// <summary>
        /// Animation list.
        /// </summary>
        public List<Animation> Animations { get; private set; }

        /// <summary>
        /// Miliseconds per animation frame.
        /// </summary>
        public float MilisecondsPerFrame { get; set; }

        /// <summary>
        /// Entity origin.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Rendering effect.
        /// </summary>
        public SpriteEffects Effect { get; set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Creates sprite by file, frameSize and Milisecons per frame.
        /// </summary>
        /// <param name="texturePath"></param>
        /// <param name="frameSize"></param>
        /// <param name="msPerFrame"></param>
        public Sprite(string texturePath, Point frameSize, float msPerFrame )
        {
            _texture = GameContent.LoadContent<Texture2D>(texturePath);
            if(frameSize == default(Point))
                FrameSize = new Point(_texture.Width, _texture.Height);
            else
                FrameSize = frameSize;
            MilisecondsPerFrame = msPerFrame;
            Animations = new List<Animation>();

            _center = new Vector2((float)FrameSize.X / 2f, (float)FrameSize.Y / 2f);
            _currentFrameTime = 0;

            _currentAnimation = -1;
            _currentFrame = null;
            _sourceRect = Rectangle.Empty;
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Change the sprite current animation by index.
        /// </summary>
        /// <param name="index">Animation index.</param>
        public void ChangeAnimation(int index)
        {
            if (index != _currentAnimation)
            {
                if (index < 0 || index > Animations.Count) _currentAnimation = -1;
                else _currentAnimation = index;
            }
        }

        /// <summary>
        /// Change the sprite current animation by name.
        /// </summary>
        /// <param name="name">Animation name</param>
        public void ChangeAnimation(string name)
        {
            Animation animation = Animations.First((a) => a.Name == name);

            if (animation == null) ChangeAnimation(-1);
            else ChangeAnimation(Animations.IndexOf(animation));
        }

        /// <summary>
        /// Sprite update method.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (_currentAnimation >= 0)
            {
                _currentFrameTime += gameTime.ElapsedGameTime.Milliseconds;

                #region Updates Frame
                if (_currentFrameTime > MilisecondsPerFrame)
                {
                    _currentFrameTime -= MilisecondsPerFrame;

                    _currentFrame = Animations[_currentAnimation].NextFrame;
                    _sourceRect = new Rectangle(_currentFrame.Column * FrameSize.X, _currentFrame.Line * FrameSize.Y, FrameSize.X, FrameSize.Y);
                }
                else
                {
                    _currentFrame = Animations[_currentAnimation].CurrentFrame;
                }
                #endregion Updates Frame
            }
        }

        /// <summary>
        /// Sprite drawing method.
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        /// <param name="spriteBatch">SpriteBatch to be used for drawing.</param>
        /// <param name="position">Sprite position.</param>
        /// <param name="color">Sprite color.</param>
        /// <param name="rotation">Sprite rotation.</param>
        /// <param name="scale">Sprite scale.</param>
        /// <param name="layerDepth">Layer depth.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, Color color, float rotation = 0.0f, float scale = 1.0f, float layerDepth = 0f)
        {
            if (_currentAnimation >= 0)
                spriteBatch.Draw(_texture, position, _sourceRect, color, rotation, Origin, scale, Effect, layerDepth);
        }
        #endregion Methods
    }
}
