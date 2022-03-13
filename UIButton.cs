using Terraria;
using Terraria.UI;
using Terraria.Audio;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.UI
{
    public class UIButton : UIElement
    {
        public UIText text;
        public Texture2D icon;
        public Color iconColor;
        public Color iconColorOver;
        public Texture2D box;
        public Color boxColor;
        public Color boxColorOver;
        public float boxCorner;
        public string display;
        public Color displayColor;
        public Vector2 displayOffset;
        public int iconDrawStyle;
        public int boxDrawStyle;
        public float transformSpeed;
        public int soundClick;
        private float _alpha;

        public UIButton()
        {
            text = new UIText();
            icon = null;
            iconColor = Color.White;
            iconColorOver = Color.White;
            box = null;
            boxColor = Color.Transparent;
            boxColorOver = Color.White;
            boxCorner = 16f;
            display = "";
            displayColor = Color.White;
            displayOffset = Vector2.Zero;
            iconDrawStyle = 0;
            boxDrawStyle = 0;
            soundClick = 10;
            transformSpeed = .1f;
            _alpha = 0;
            Append(text);
        }

        public void SetIcon(Texture2D image, Color color = default, Color? colorOver = null)
        {
            this.icon = image;
            this.iconColor = TryGetValue(color, Color.Transparent);
            this.iconColorOver = TryGetValue(colorOver, iconColor);
        }

        public void SetBox(Texture2D image, Color color = default, Color? colorOver = null, float corner = 16f)
        {
            this.box = image;
            this.boxColor = TryGetValue(color, Color.Transparent);
            this.boxColorOver = TryGetValue(colorOver, boxColor);
            this.boxCorner = corner;
        }

        public void SetDisplay(string text, Color color = default, Vector2 offset = default)
        {
            display = text;
            displayColor = TryGetValue(color, Color.White);
            displayOffset = offset == default ? new Vector2(16f) : offset;
        }

        protected override void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            if (box != null)
            {
                Color _color = Color.Lerp(boxColor, boxColorOver, _alpha);
                if (boxCorner <= 0f) spriteBatch.Draw(box, rect, null, _color, 0, Vector2.Zero, (SpriteEffects)boxDrawStyle, 0);
                else
                {
                    Rectangle _rect = rect;
                    int mw = (int)boxCorner;
                    int mh = (int)boxCorner;
                    int _mw = mw * 2;
                    int _mh = mh * 2;
                    int _w = box.Width;
                    int _h = box.Height;
                    int w = _rect.Width < mw ? mw : _rect.Width;
                    int h = _rect.Height < mh ? mh : _rect.Height;
                    int x = _rect.X;
                    int y = _rect.Y;
                    int xw = x + w;
                    int yh = y + h;
                    Rectangle[] dRect = new Rectangle[9]
                    {
                        new Rectangle(x, y, mw, mh),
                        new Rectangle(x + mw, y, w - _mw, mh),
                        new Rectangle(x + w - mw, y, mw, mh),
                        new Rectangle(x, y + mh, mw, h - _mh),
                        new Rectangle(x + mw, y + mh, w - _mw, h - _mh),
                        new Rectangle(x + w - mw, y + mh, mw, h - _mh),
                        new Rectangle(x, y + h - mh, mw, mh),
                        new Rectangle(x + mw, y + h - mh, w - _mw, mh),
                        new Rectangle(x + w - mw, y + h - mh, mw, mh),
                    };
                    Rectangle?[] sRect = new Rectangle?[9]
                    {
                        new Rectangle(0, 0, mw, mh),
                        new Rectangle(mw, 0, _w - _mw, mh),
                        new Rectangle(_w - mw, 0, mw, mh),
                        new Rectangle(0, mh, mw, _h - _mh),
                        new Rectangle(mw, mh, _w - _mw, _h - _mh),
                        new Rectangle(_w - mw, mh, mw, _h - _mh),
                        new Rectangle(0, _h - mh, mw, mh),
                        new Rectangle(mw, _h - mh, _w - _mw, mh),
                        new Rectangle(_w - mw, _h - mh, mw, mh),
                    };
                    for (int i = 0; i < 9; i++) spriteBatch.Draw(box, dRect[i], sRect[i], _color);
                }
            }
            if (icon != null)
            {
                Color _color = Color.Lerp(iconColor, iconColorOver, _alpha);
                spriteBatch.Draw(icon, rect, null, _color, 0, Vector2.Zero, (SpriteEffects)iconDrawStyle, 0);
            }
            if (display != "" && IsMouseHovering)
            {
                //Main.LocalPlayer.mouseInterface = true;
                /*
                Vector2 pos = Main.MouseScreen + displayOffset;
                Vector2 size = Font.MeasureString(display);
                if (pos.X + size.X > screen.X) pos.X = screen.X - size.X;
                if (pos.Y + size.Y > screen.Y) pos.Y = screen.Y - size.Y;
                Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Font, display, pos, CNCUtils.AccurateColor(Color.Black), 0, Vector2.Zero, Vector2.One);
                Main.spriteBatch.DrawString(Font, display, pos, CNCUtils.AccurateColor(displayColor), 0, Vector2.Zero, 1, 0, 0);
                */
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _alpha += (IsMouseHovering ? 1 : -1) * transformSpeed;
            if (_alpha > 1f) _alpha = 1f;
            if (_alpha < 0f) _alpha = 0f;
            /*
            if (text.Visible = IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                text.text = display;
                text.textColor = displayColor;
                text.textOffset = displayOffset;
            }
            */
        }

        public override void MouseCheck(UIMouseEvent evt, bool over, ref bool basic)
        {
            Rectangle rect = new Rectangle(0, 0, (int)screen.X, (int)screen.Y);
            if (basic = !rect.Contains(Main.MouseScreen.ToPoint())) return;
            basic = true;
            SoundEngine.PlaySound(12);
        }

        public override void Click(UIMouseEvent evt, ref bool basic)
        {
            Rectangle rect = new Rectangle(0, 0, (int)screen.X, (int)screen.Y);
            if (basic = !rect.Contains(Main.MouseScreen.ToPoint())) return;
            basic = true;
            SoundEngine.PlaySound(soundClick);
        }
    }
}