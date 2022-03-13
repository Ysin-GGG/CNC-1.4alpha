using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.UI
{
    public class UIPanel : UIElement
    {
        public Texture2D image;
        public Color color;
        public Color colorOver;
        public Vector2 corner;
        public float transformSpeed;
        private float _alpha;

        public UIPanel()
        {
            image = null;
            colorOver = color = Color.White;
            corner = new Vector2(16f);
            transformSpeed = .05f;
            _alpha = 0;
        }

        public void SetBox(Texture2D image, Color color = default, Color? colorOver = null, Vector2 corner = default)
        {
            this.image = image;
            this.color = TryGetValue(color, Color.White);
            this.colorOver = TryGetValue(colorOver, this.color);
            this.corner = corner;
        }

        protected override void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            Color _color = Color.Lerp(color, colorOver, _alpha);
            if (image != null)
            if (corner.X > 0 && corner.Y > 0) Drawing.DrawBox(spriteBatch, image, rect, _color, corner);
            else spriteBatch.Draw(image, rect, _color);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _alpha += (IsMouseHovering ? 1 : -1) * transformSpeed;
            if (_alpha > 1f) _alpha = 1f;
            if (_alpha < 0f) _alpha = 0f;
        }
    }
}