using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.UI
{
    public class UIBar : UIElement
    {
        public Texture2D bar;
        public Texture2D fill;
        public Color barColor;
        public Color fillColor;
        public float leftBorder;
        public float rightBorder;
        public double progress;
        public string mouseText;

        public UIBar()
        {
            bar = null;
            fill = null;
            barColor = Color.White;
            fillColor = Color.White;
            leftBorder = 0f;
            rightBorder = 0f;
            progress = 0f;
            mouseText = "";
        }

        public void SetBar(Texture2D image, Color color = default)
        {
            bar = image;
            barColor = TryGetValue(color, Color.White);
        }

        public void SetProgress(double progress) => this.progress = (progress < 0) ? 0 : (progress > 1 ? 1 : progress);
        public void SetProgress(double value, double valueMax) => SetProgress(value / valueMax);

        protected override void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            if (bar != null)
            {
                Rectangle leftRect = new Rectangle(0, 0, (int)leftBorder, bar.Height);
                Rectangle middleRect = new Rectangle(leftRect.Width, 0, (int)(bar.Width - leftBorder - rightBorder), (int)Size.Y);
                Rectangle rightRect = new Rectangle((int)(bar.Width - rightBorder), 0, (int)rightBorder, bar.Height);
                //´ýÍêÉÆ
            }
        }
    }
}