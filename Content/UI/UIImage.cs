using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.UI
{
    public class UIImage : UIElement
    {
        public Texture2D image;
        public Rectangle? imageRect;
        public Rectangle? drawRect;
        public Color color;
        public Color colorOver;
        public Vector2 origin;
        public Vector2 drawOffset;
        public float rotation;
        public int drawStyle;
        public float transformSpeed;
        private float _alpha;
        
        public UIImage()
        { 
            image = null;
            imageRect = null;
            drawRect = null;
            color = Color.White;
            colorOver = Color.White;
            origin = Vector2.Zero;
            drawOffset = Vector2.Zero;
            rotation = 0;
            drawStyle = 0;
        }

        public void SetFrame(Point frameCount, Point frameCounterAdd)
        {

        }

        public void SetImage(Texture2D image, Color color = default, Color? colorOver = null)
        {
            this.image = image;
            this.color = TryGetValue(color, Color.White);
            this.colorOver = TryGetValue(colorOver, this.color);
        }

        protected override void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            //Rectangle _imageRect = imageRect == null ? new Rectangle(0, 0, (int)width, (int)height) : (Rectangle)imageRect;
            //Rectangle? drawRect = null;// new Rectangle((int)(frame.X * Size.X) + _imageRect.X, (int)(frame.Y * Size.Y) + _imageRect.Y, _imageRect.Width, _imageRect.Height);
            Color _color = Color.Lerp(color, colorOver, _alpha);
            //if (image != null) Vector2 scale = rect.Size() / image.Size();
            //if (drawRect!= null) scale = rect.Size() / ((Rectangle)drawRect).Size();
            if (image != null) spriteBatch.Draw(image, rect.TopLeft() + drawOffset, drawRect, _color, rotation, origin, rect.Size() / image.Size(), (SpriteEffects)drawStyle, 0);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _alpha += (IsMouseHovering ? 1 : -1) * transformSpeed;
            if (_alpha > 1f) _alpha = 1f;
            if (_alpha < 0f) _alpha = 0f;
            /*
            frameCounter += frameCounterAdd;
            if (frameCounter.X >= frameTickMax[frame.X].X)
            {
                frameCounter.X = 0;
                frame.X = (frame.X + 1) % frameCount.X;
            }
            if (frameCounter.Y >= frameTickMax[frame.Y].Y)
            {
                frameCounter.Y = 0;
                frame.Y = (frame.Y + 1) % frameCount.Y;
            }
            */
        }
    }
}