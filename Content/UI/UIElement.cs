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
        public bool dead1 = false;//不继承父类的鼠标互动
        public bool dead2 = false;//不可鼠标互动
        public bool dead3 = false;//不绘制
        public bool dead4 = false;//不刷新ui
        public bool dead5 = false;//完全不刷新ui
        public bool mouseOver = false;//鼠标悬浮于ui上（以屏幕坐标为准）
        public bool mouseOver2 = false;//鼠标悬浮于ui上（以父ui坐标为准）
        public bool Dead { set { dead1 = dead2 = dead3 = dead4 = dead5 = value; } }//禁用该UIElement
        public bool VirtualEvent { get => !dead1; internal set => dead1 = !value; }
        public bool MouseDead { get => dead2; internal set => dead2 = value; }
        public bool Visible { get => !dead3; internal set => dead3 = !value; }
        public bool Updating { get => !dead4; internal set => dead4 = !value; }
        public bool Updating2 { get => !dead5; internal set => dead5 = dead4 = !value; }

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
            dead1 = false;//不继承父类的鼠标互动
            dead2 = false;//不可鼠标互动
            dead3 = false;//不绘制
            dead4 = false;//不刷新ui
            dead5 = false;//完全不刷新ui
            Dead = false;
            mouseOver = false;
            mouseOver2 = false;
            Initialize();
        }

        public sealed override void Update(GameTime gameTime)
        {
            if (dead5) return;
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
            if (!dead4) UpdateUI(gameTime);
            Recalculate();
        }

        protected sealed override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (dead3) return;
            base.DrawSelf(spriteBatch);
            Rectangle rect = Rect;
            rect.X = (int)Dimensions.X;
            rect.Y = (int)Dimensions.Y;
            Draw(spriteBatch, rect);
        }

        public sealed override void MouseDown(UIMouseEvent evt)
        {
            if (dead2) return;
            bool basic = !dead1;
            Mouse(evt, true, ref basic);
            if (basic) base.MouseDown(evt);
        }

        public sealed override void MouseUp(UIMouseEvent evt)
        {
            if (dead2) return;
            bool basic = !dead1;
            Mouse(evt, false, ref basic);
            if (basic) base.MouseUp(evt);
        }

        public sealed override void MouseOver(UIMouseEvent evt)
        {
            if (dead2) return;
            bool basic = !dead1;
            MouseCheck(evt, true, ref basic);
            if (basic) base.MouseOver(evt);
        }

        public sealed override void MouseOut(UIMouseEvent evt)
        {
            if (dead2) return;
            bool basic = !dead1;
            MouseCheck(evt, false, ref basic);
            if (basic) base.MouseOut(evt);
        }

        public sealed override void Click(UIMouseEvent evt)
        {
            if (dead2) return;
            bool basic = !dead1;
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