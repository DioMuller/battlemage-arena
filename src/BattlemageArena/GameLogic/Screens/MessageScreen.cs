using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattlemageArena.GameLogic.Screens
{
    class MessageScreen
    {
        private Rectangle _bounds;
        private Rectangle _titlePosition;

        private Texture2D _background;
        private SpriteFont _font;
        private Texture2D _logo;

        private string _message;
        private Vector2 _messagePosition;
        private Vector2 _messageOrigin;

        public MessageScreen(string background, int width, int height, string message)
        {
            #region Title Screen Assets initialization

            _bounds = new Rectangle(0, 0, width, height);
            _background = GameContent.LoadContent<Texture2D>(background);
            _logo = GameContent.LoadContent<Texture2D>("Images/Logo");
            _font = GameContent.LoadContent<SpriteFont>("Fonts/SmallFont");

            Vector2 bg_center = new Vector2(_logo.Width / 2.0f, _logo.Height / 2.0f);
            Vector2 screen_center = new Vector2(width / 2.0f, height / 2.0f);
            _titlePosition = new Rectangle((int)(screen_center.X - bg_center.X),
                (int)(screen_center.Y * 0.5 - bg_center.Y),
                (int)(_logo.Width),
                (int)(_logo.Height));

            _message = message;
            Vector2 startSize = _font.MeasureString(_message);
            _messageOrigin = new Vector2(startSize.X / 2, startSize.Y / 2);
            _messagePosition = new Vector2(screen_center.X, screen_center.Y * 1.7f);

            #endregion Message Screen Assets initialization
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _bounds, Color.White);
            spriteBatch.Draw(_logo, _titlePosition, Color.White);
            spriteBatch.DrawString(_font, _message, _messagePosition, Color.White, 0.0f, _messageOrigin, Vector2.One, SpriteEffects.None, 1.0f);
        }
    }
}
