using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.UI
{
    public class UIText : UIElement
    {
        public string text;
        public Color textColor;
        public Color textShadowColor;
        public Vector2 textBorder;
        public Vector2 textOffset;
        public Vector2 textPosition;
        public Texture2D box;
        public Color boxColor;
        public Vector2 boxCorner;
        
        public UIText()
        {
            text = "";
            textColor = Color.White;
            textShadowColor = Color.Black;
            textBorder = new Vector2(8f);
            textPosition = Vector2.Zero;
            box = Terraria.ModLoader.ModContent.Request<Texture2D>($"CNC/Content/UI/MainBar/Images/ButtonBox").Value; //null;
            boxColor = Color.Transparent;
            boxCorner = new Vector2(16f);
        }

        protected override void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            if (text != "" && Visible)
            {
                Vector2 pos = textPosition + textOffset;// Main.MouseScreen + textOffset;
                Vector2 size = Font.MeasureString(text);
                Vector2 offset = new Vector2(size.X + textBorder.X + 4f, size.Y + textBorder.Y + 6f);
                if (pos.X > screen.X - offset.X) pos.X = screen.X - offset.X;
                if (pos.Y > screen.Y - offset.Y) pos.Y = screen.Y - offset.Y;
                rect.X = (int)(pos.X - textBorder.X);
                rect.Y = (int)(pos.Y - textBorder.Y);   
                rect.Width = (int)(size.X + textBorder.X * 2);
                rect.Height = (int)(size.Y + textBorder.Y * 2 - 6f);
                if (rect.X + rect.Width  > screen.X) rect.X = (int)(screen.X - rect.Width);
                if (rect.Y + rect.Height > screen.Y) rect.Y = (int)(screen.Y - rect.Height);
                if (box != null)
                {
                    Color _color = boxColor;
                    if (boxCorner.X <= 0f || boxCorner.Y <= 0f) spriteBatch.Draw(box, rect, null, _color, 0, Vector2.Zero, 0, 0);
                    else
                    {
                        Rectangle _rect = rect;
                        int mw = (int)boxCorner.X;
                        int mh = (int)boxCorner.Y;
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
                Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, text, pos, CNCUtils.FlashColor(textShadowColor), 0, Vector2.Zero, Vector2.One);
                //Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, text, pos, CNCUtils.FlashColor(textColor), 0, Vector2.Zero, Vector2.One);
                DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, text, pos, textColor);
                //spriteBatch.DrawString(Font, text, pos, CNCUtils.FlashColor(textColor), 0, Vector2.Zero, 1, 0, 0);
            }
        }

        public override void Click(UIMouseEvent evt, ref bool basic)
        {
            basic = false;
        }

        public override void Mouse(UIMouseEvent evt, bool down, ref bool basic)
        {
            basic = false;
        }
    }
}