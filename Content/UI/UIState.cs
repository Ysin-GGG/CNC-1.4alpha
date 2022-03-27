using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.UI
{
    public abstract class UIState : Terraria.UI.UIState
    {
        public Rectangle Rect { get => new Rectangle((int)left, (int)top, (int)width, (int)height); set { left = value.X; top = value.Y; width = value.X; height = value.Y; } }
        public Vector2 Center { get => Offset + Size / 2; set { Offset = value - Size / 2; } }
        public Vector2 Offset { get => new Vector2(left, top); set { left = value.X; top = value.Y; } }
        private Vector2 offset;
        public float left { get => Left.Pixels; set => Left.Set(value, 0); }
        public float top { get => Top.Pixels; set => Top.Set(value, 0); }
        public Vector2 Size { get => new Vector2(width, height); set { width = value.X; height = value.Y; } }
        public float width { get => Width.Pixels; set => Width.Set(value, 0); }
        public float height { get => Height.Pixels; set => Height.Set(value, 0); }
        public Vector2 Scale;
        public bool CanDrag { get; set; }
        public bool CanOutScreen { get; set; }
        public bool Dragging { get; set; }
        public Vector2 screen => Main.ScreenSize.ToVector2();
        public DynamicSpriteFont Font = Const.Font[4];
        public CalculatedStyle Dimensions => base.GetDimensions();
        public List<UIElement> children = new List<UIElement>();
        public List<UIElement> children2 = new List<UIElement>();
        public UIText MouseText;
        public UIImage[] Cursor;
        public int id = -1;//UI的id，mod config悬浮文本框需要
        public Player player => Main.LocalPlayer;
        public virtual string Name => "";// System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
        public string mouseText { get => MouseText.text; set { if (MouseText.Visible = value.Length > 0) MouseText.text = value;else MouseText.text = ""; } }// System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;

        public Texture2D GetImage(string key) => ModContent.Request<Texture2D>($"CNC/Content/UI/{Name}/Images/{key}").Value;

        public sealed override void OnInitialize()
        {
            base.OnInitialize();
            Offset = offset = Vector2.Zero;
            Size = Vector2.Zero;
            Scale = Vector2.One;
            CanOutScreen = CanDrag = Dragging = false;
            MouseText = new UIText();
            Cursor = new UIImage[6];
            Initialize();
            children.Add(MouseText);
            for (int i = 0; i < 6; i++) children2.Add(Cursor[i] = new UIImage());
            for (int i = 0; i < children.Count; i++) Append(children[i]);
            for (int i = 0; i < children2.Count; i++) Append(children2[i]);
        }

        protected void AddButton(ref UIPanel parent, ref UIButton button, MouseEvent evt = null)
        {
            button = new UIButton();
            button.OnMouseOver += ShowText;
            button.OnMouseOut += HideText;
            if (evt != null) button.OnClick += evt;
            parent.Append(button);
        }

        protected void AddButton(ref UIElement parent, ref UIButton button, MouseEvent evt = null)
        {
            button = new UIButton();
            button.OnMouseOver += ShowText;
            button.OnMouseOut += HideText;
            if (evt != null) button.OnClick += evt;
            parent.Append(button);
        }

        protected void ResetButton(ref UIButton button, Vector2 pos, float scale, string icon, string box, string name, string arg = "", Vector2 offset = default, string UIName = default)
        {
            offset = offset == default ? Vector2.Zero : offset;
            button.SetIcon(GetImage(icon), Color.White * .8f, CNCUtils.AccurateColor(Color.White));
            button.SetBox(GetImage(box), Color.Transparent, Color.White, 0f);
            button.SetRect(offset + pos * scale, button.icon.Size() * scale);
            button.SetDisplay(Common.Localization.CNCLocalization.GetText($"UI.{(UIName == default ? Name : UIName)}.{name}", arg));
            button.transformSpeed = .15f;
            button.text.Visible = false;
            if (button.IsMouseHovering) mouseText = button.display;
        }

        public sealed override void Update(GameTime gameTime)
        {
            if (Dragging) Offset = new Vector2(Main.mouseX, Main.mouseY) - offset;
            else if(!CanOutScreen)
            {
                if (Offset.X < 0) left = 0;
                if (Offset.Y < 0) top = 0;
                if (Offset.X + Size.X > screen.X) left = screen.X - Size.X;
                if (Offset.Y + Size.Y > screen.Y) top = screen.Y - Size.Y;
            }
            MouseText.boxColor = Color.Transparent;
            MouseText.textBorder = new Vector2(12f, 8f);
            MouseText.textPosition = Main.MouseScreen.ToPoint().ToVector2();
            if (!new Rectangle(0, 0, (int)screen.X, (int)screen.Y).Contains(Main.MouseScreen.ToPoint()))
            {
                MouseText.Visible = false;
                MouseText.text = "";
            }
            if (id >= 0)
            {
                if ((byte)Options.hoverTextBox[id] == 0)
                {
                    MouseText.box = null;
                    MouseText.boxColor = Color.Transparent;
                }
                if ((byte)Options.hoverTextBox[id] == 1)
                {
                    MouseText.box = TextureAssets.InventoryBack13.Value;
                    MouseText.boxColor = new Color(23, 25, 81, (int)byte.MaxValue) * Options.hoverTextBoxAlpha[id];
                }
                if ((byte)Options.hoverTextBox[id] == 2)
                {
                    MouseText.box = ModContent.Request<Texture2D>($"CNC/Content/UI/MainBar/Images/ButtonBox").Value;
                    MouseText.boxColor = Color.White * Options.hoverTextBoxAlpha[id];
                }
            }
            for (int i = 0; i < Cursor.Length; i++) Cursor[i].active = Cursor[i].Visible = false;
            Recalculate();
            UpdateUI(gameTime);
            base.Update(gameTime);
        }

        public void DrawCursor()
        {
            Color cursorColor = player.hasRainbowCursor ? Main.DiscoColor : Main.cursorColor;
            Vector2 pos = Main.MouseScreen + new Vector2(2f);
            Texture2D cursor = TextureAssets.Cursors[0].Value;
            Vector2 size = cursor.Size() * new Vector2(Main.cursorScale);
            for (int i = 0; i < Cursor.Length; i++) Cursor[i].active = Cursor[i].Visible = true;
            for (int i = 0; i < 4; i++)
            {
                Vector2[] bonus =
                {
                    new Vector2(0f, 1f),
                    new Vector2(1f, 0f),
                    new Vector2(0f, -1f),
                    new Vector2(-1f, 0f),
                };
                Cursor[i].SetImage(TextureAssets.Cursors[11].Value, Main.MouseBorderColor);
                Cursor[i].SetRect(pos + bonus[i], Cursor[i].image.Size() * Main.cursorScale * 1.1f);
                Cursor[i].origin = new Vector2(2f);
            }
            Cursor[4].SetImage(cursor, new Color((int)(cursorColor.R * 0.200000002980232), (int)(cursorColor.G * 0.200000002980232), (int)(cursorColor.B * 0.200000002980232), (int)(cursorColor.A * 0.5)));
            Cursor[4].SetRect(pos + Vector2.One, size * 1.1f);
            Cursor[5].SetImage(cursor, cursorColor);
            Cursor[5].SetRect(pos, size);
        }

        protected sealed override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Draw(spriteBatch);
        }

        public sealed override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (CanDrag)
            {
                offset = evt.MousePosition - Offset;
                Dragging = true;
                DragStart(evt);
            }
            Mouse(evt, true);
        }

        public sealed override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            if (CanDrag)
            {
                Offset = evt.MousePosition - offset;
                Dragging = false;
                Recalculate();
                DragEnd(evt);
            }
            Mouse(evt, false);
        }

        protected new virtual void Draw(SpriteBatch spriteBatch) { }
        public new virtual void Initialize() { }
        public virtual void UpdateUI(GameTime gameTime) { }
        public virtual void DragStart(UIMouseEvent evt) { }
        public virtual void DragEnd(UIMouseEvent evt) { }
        public virtual void Mouse(UIMouseEvent evt, bool down) { }

        public virtual void ShowText(UIMouseEvent evt, Terraria.UI.UIElement e) => ShowText(evt, (UIButton)e);
        public virtual void ShowText(UIMouseEvent evt, UIButton e)
        {
            UIButton button = (UIButton)e;
            MouseText.Visible = true;
            MouseText.text = button.display;
            MouseText.textOffset = button.displayOffset;
        }

        public virtual void HideText(UIMouseEvent evt, Terraria.UI.UIElement e)
        {
            MouseText.Visible = false;
            MouseText.text = "";
        }

        public virtual void HideText(UIMouseEvent evt, UIButton e) => HideText(evt, e);
    }
}