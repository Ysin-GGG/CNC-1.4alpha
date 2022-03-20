using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using CNC.Common.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.UI.MainBar
{
    public class MainBar : UIState
    {
        public UIPanel MainBox;
        public UIButton CollapseBar;
        public List<UIButton> FuncButton = new List<UIButton>();
        private Vector2 _size = new Vector2(16f, 64f);
        private bool Show;
        private byte collapseStyle => (byte)Options.closeEdge;
        private float collapseOffset => Options.offset;

        public override string Name => "MainBar";

        private readonly string[] buttonName =
        {
            "ClassesMenu",
            "Enhancement",
            "Archive",
        };

        public override void Initialize()
        {
            Show = false;
            CanDrag = false;
            CanOutScreen = true;
            Scale = new Vector2(Main.UIScale);
            Offset = new Vector2(64f, screen.Y);
            MainBox = new UIPanel();
            MainBox.Size = Size = _size;
            MainBox.SetBox(GetImage("MainBox"), corner: new Vector2(8f));
            CollapseBar = new UIButton();
            CollapseBar.OnClick += Collapse;
            CollapseBar.OnMouseOver += ShowText;
            CollapseBar.OnMouseOut += HideText;
            children.Add(MainBox);
            children.Add(CollapseBar);
            AddButton(buttonName[0], OpenClassesMenu);
            AddButton(buttonName[1], OpenInscriptionMenu);
            for (int i = 2; i < buttonName.Length; i++) AddButton(buttonName[i]);
            for (int i = buttonName.Length - 1; i >= 0; i--) MainBox.Append(FuncButton[i]);
            Size = _size;
            id = 1;
        }

        private UIButton AddButton(string key, MouseEvent evt = null)
        {
            int count = FuncButton.Count;
            _size.X += count > 0 ? 56f : 48f;
            UIButton button = new UIButton();
            button.OnMouseOver += ShowText;
            button.OnMouseOut += HideText;
            if (evt != null) button.OnClick += evt;
            FuncButton.Add(button);
            ResetButton();
            return FuncButton[count];
        }

        private void ResetButton()
        {
            for(int i = 0; i < FuncButton.Count; i++)
            {
                Vector2 offset = new Vector2(8f + 56f * i, 8f);
                FuncButton[i].icon = GetImage(buttonName[i]) ;
                FuncButton[i].SetRect(offset, new Vector2(48f));
                FuncButton[i].SetBox(GetImage("ButtonBox"), Color.Transparent, Color.White, 8f);
                FuncButton[i].SetDisplay(CNCLocalization.GetText($"UI.{buttonName[i]}.Name"), Color.Transparent);
                FuncButton[i].text.Visible = false;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            //Show = collapseStyle < 2 ? CollapseBar.iconDrawStyle != 0 : CollapseBar.iconDrawStyle == 0;
            MainBox.Size = Size = _size;
            MainBox.SetBox(GetImage("MainBox"), corner: new Vector2(8f));
            MainBox.color = Color.White * .5f;
            ResetButton();
            Vector2[] size =
            {
                new Vector2(37f, 12f),
                new Vector2(12f, 37f),
                new Vector2(37f, 12f),
                new Vector2(12f, 37f),
            };
            Vector2[] offset =
            {
                new Vector2((Size.X - size[0].X) / 2, -size[0].Y),
                new Vector2(width, (Size.Y - size[1].Y) / 2),
                new Vector2((Size.X - size[2].X) / 2, height),
                new Vector2(-size[3].X, (Size.Y - size[1].Y) / 2),
            };
            CollapseBar.SetRect(offset[collapseStyle], size[collapseStyle]);
            CollapseBar.icon = GetImage("CollapseBarArrow" + (collapseStyle % 2 + 1).ToString());
            CollapseBar.SetBox(GetImage("CollapseBar" + (collapseStyle % 2 + 1).ToString()), Color.White, Color.White, 0f);
            CollapseBar.SetDisplay(CNCLocalization.GetText("UI.Function." + (Show ? "Collapse" : "Expand"), Color.Transparent));
            CollapseBar.text.Visible = false;
            if (collapseStyle < 2) CollapseBar.boxDrawStyle = 0;
            else CollapseBar.boxDrawStyle = 2 - collapseStyle % 2;
            if (IsMouseHovering) Main.LocalPlayer.mouseInterface = true;
            float num = Options.movementSpeed;
            float limit;
            float padding = height / 3;
            if (collapseStyle % 2 == 0) left = (screen.X - width) / 2 + (screen.X - width)  / 2 * collapseOffset;
            if (collapseStyle % 2 == 1) top = (screen.Y - height) / 2 + (screen.Y - height) / 2 * collapseOffset;
            if (Show)
            {
                switch (collapseStyle)
                {
                    case 0:
                        limit = screen.Y - height - padding;
                        top = top - num < limit ? limit : top - num;
                        CollapseBar.iconDrawStyle = 2;
                        break;
                    case 1:
                        limit = padding;
                        left = left + num > limit ? limit : left + num;
                        CollapseBar.iconDrawStyle = 1;
                        break;
                    case 2:
                        limit = padding;
                        top = top + num > limit ? limit : top + num;
                        CollapseBar.iconDrawStyle = 0;
                        break;
                    case 3:
                        limit = screen.X - width - padding;
                        left = left - num < limit ? limit : left - num;
                        CollapseBar.iconDrawStyle = 0;
                        break;
                }
            }
            else
            {
                switch (collapseStyle)
                {
                    case 0:
                        limit = screen.Y - height - padding;
                        if (top < limit) top = limit;
                        limit = screen.Y;
                        top = (top + num) > limit ? limit : (top + num);
                        CollapseBar.iconDrawStyle = 0;
                        break;
                    case 1:
                        limit = padding;
                        if (left > limit) left = limit;
                        limit = -width - 1;
                        left = (left - num) < limit ? limit : (left - num);
                        CollapseBar.iconDrawStyle = 0;
                        break;
                    case 2:
                        limit = padding;
                        if (top > limit) top = limit;
                        limit = -height - 1;
                        top = (top - num) < limit ? limit : (top - num);
                        CollapseBar.iconDrawStyle = 2;
                        break;
                    case 3:
                        limit = screen.X - width - height / 3;
                        if (left < limit) left = limit;
                        limit = screen.X;
                        left = (left + num) > limit ? limit : (left + num);
                        CollapseBar.iconDrawStyle = 1;
                        break;
                }
            }
        }

        private void Collapse(UIMouseEvent evt, Terraria.UI.UIElement e)
        {
            CollapseBar.soundClick = (Show = !Show) ? 10 : 11;
        }

        private void OpenClassesMenu(UIMouseEvent evt, Terraria.UI.UIElement e)
        {
            ClassesMenu.DarkScreen.Visible = true;
        }

        private void OpenInscriptionMenu(UIMouseEvent evt, Terraria.UI.UIElement e)
        {
            InscriptionMenu.DarkScreen.Visible = true;
        }
    }
}