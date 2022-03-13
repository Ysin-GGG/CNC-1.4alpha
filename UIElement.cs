using System;
using Terraria;
using Terraria.UI;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.UI
{
    public class UIElement : Terraria.UI.UIElement
    {
        public Rectangle Rect { get => new Rectangle((int)left, (int)top, (int)width, (int)height); set { left = value.X; top = value.Y; width = value.X; height = value.Y; } }
        public Vector2 Center { get => Offset + Size / 2; set { Offset = value - Size / 2; } }
        public Vector2 Offset { get => new Vector2(left, top); set { left = value.X; top = value.Y; } }
        public float left { get => Left.Pixels; set => Left.Pixels = value; }
        public float top { get => Top.Pixels; set => Top.Pixels = value; }
        public Vector2 Size { get => new Vector2(width, height); set { width = value.X; height = value.Y; } }
        public float width { get => Width.Pixels; set => Width.Pixels = value; }
        public float height { get => Height.Pixels; set => Height.Pixels = value; }
        public Vector2 screen => Main.ScreenSize.ToVector2();
        public DynamicSpriteFont Font = Const.Font[4];
        public CalculatedStyle Dimensions => base.GetDimensions();
        public bool active = true;
        public bool mouseOver = false;
        public bool mouseOver2 = false;
        public bool Visible { get => visible; internal set => visible = value; }
        private bool visible = true;

        protected Color TryGetValue(Color value1, Color value2) => value1 == default ? value2 : value1;
        protected Color TryGetValue(Color? value1, Color value2) => value1 == null ? value2 : (Color)value1;
        public void SetRect(Rectangle rect) => Rect = rect;
        public void SetRect(Vector2 offset, Vector2 size) { Offset = offset; Size = size; }
        public void SetRect(float offset, float size) => SetRect(new Vector2(offset), new Vector2(size));

        public sealed override void OnInitialize()
        {
            base.OnInitialize();
            SetPadding(0f);
            Size = Vector2.One;
            Offset = Vector2.Zero;
            active = true;
            mouseOver = false;
            Initialize();
        }

        public sealed override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            /*
            Rectangle parentRect = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentRect))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentRect.Right - Width.Pixels) + Offset.X;
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentRect.Bottom - Height.Pixels) + Offset.Y;
            }
            */
            float x = left;
            float y = top;
            mouseOver = Main.mouseX < x + width && Main.mouseY < y + height && Main.mouseX > x && Main.mouseY > y;
            x += (int)Dimensions.X; y += (int)Dimensions.Y;
            mouseOver2 = Main.mouseX < x + width && Main.mouseY < y + height && Main.mouseX > x && Main.mouseY > y;
            UpdateUI(gameTime);
            Recalculate();
        }

        protected sealed override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!visible) return;
            base.DrawSelf(spriteBatch);
            Rectangle rect = Rect;
            rect.X = (int)Dimensions.X;
            rect.Y = (int)Dimensions.Y;
            Draw(spriteBatch, rect);
        }

        public sealed override void MouseDown(UIMouseEvent evt)
        {
            bool basic = active;
            Mouse(evt, true, ref basic);
            if (basic) base.MouseDown(evt);
        }

        public sealed override void MouseUp(UIMouseEvent evt)
        {
            bool basic = active;
            Mouse(evt, false, ref basic);
            if (basic) base.MouseUp(evt);
        }

        public sealed override void MouseOver(UIMouseEvent evt)
        {
            bool basic = active;
            MouseCheck(evt, true, ref basic);
            if (basic) base.MouseOver(evt);
        }

        public sealed override void MouseOut(UIMouseEvent evt)
        {
            bool basic = active;
            MouseCheck(evt, false, ref basic);
            if (basic) base.MouseOut(evt);
        }

        public sealed override void Click(UIMouseEvent evt)
        {
            bool basic = active;
            Click(evt, ref basic);
            if (basic) base.Click(evt);
        }

        protected virtual void Draw(SpriteBatch spriteBatch, Rectangle rect) { }
        public new virtual void Initialize() { }
        public virtual void UpdateUI(GameTime gameTime) { }
        public virtual void DragStart(UIMouseEvent evt) { }
        public virtual void DragEnd(UIMouseEvent evt) { }
        public virtual void Mouse(UIMouseEvent evt, bool down, ref bool basic) { }
        public virtual void MouseCheck(UIMouseEvent evt, bool over, ref bool basic) { }
        public virtual void Click(UIMouseEvent evt, ref bool basic) { }
    }
}