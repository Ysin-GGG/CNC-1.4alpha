using System;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Items;
using CNC.Common.Global.Players;

namespace CNC.Content.UI.InscriptionMenu
{
    public class WeaponList : UIState
    {
        public UIPanel Background;
        public UIPanel MainBox;
        public UIButton[] WeaponIcon;

        private Rectangle rect => new Rectangle(0, 0, (int)screen.X + 1, (int)screen.Y + 1);
        private const float blackSize = 200f;
        public static bool Visible = false;
        public static int index = -1;
        public static bool mouseLeft = false;
        public static bool mouseLeftRelease = true;
        public static int NoWay = 0;

        public override string Name => "InscriptionMenu";

        public override void Initialize()
        {
            CanDrag = false;
            CanOutScreen = true;
            Offset = Vector2.Zero;
            Size = screen + Vector2.One;
            Visible = false;
            WeaponIcon = new UIButton[50];
            children.Add(Background = new UIPanel());
            children.Add(MainBox = new UIPanel());
            for (int i = 0; i < 50; i++) AddButton(ref MainBox, ref WeaponIcon[i]);
            id = 3;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            float scale = MathHelper.Min((rect.Width - blackSize) / GetImage("InscriptionWindow").Width, (rect.Height - blackSize) / GetImage("InscriptionWindow").Height);
            Size = rect.Size();
            Offset = Vector2.Zero;

            mouseLeft = !mouseLeftRelease;
            mouseLeftRelease = Main.mouseLeftRelease;

            Background.SetRect(Vector2.Zero, rect.Size());
            Background.SetBox(GetImage("DarkScreen"), Color.White * .75f, null, Vector2.Zero);

            MainBox.SetBox(GetImage("MainBox"), corner: new Vector2(8f));
            MainBox.Size = (MainBox.image.Size() * new Vector2(10, 5) + new Vector2(16f) * 2 + new Vector2(4f * 9f, 4f * 5f)) * scale;
            MainBox.Center = rect.Center();

            for (int i = 0; i < 50; i++)
            {
                if (WeaponIcon[i].IsMouseHovering && mouseLeft && mouseLeftRelease) SelectWeapon(i);
                WeaponIcon[i].soundClick = null;// -1;
                WeaponIcon[i].SetIcon(null, Color.Transparent);
                WeaponIcon[i].SetBox(GetImage("ButtonBox"), Color.White * .7f, Color.White);
                WeaponIcon[i].SetRect((new Vector2(16f) + new Vector2(i % 10, i / 10) * (WeaponIcon[i].box.Size() + new Vector2(4f))) * scale, WeaponIcon[i].box.Size() * scale);
                WeaponIcon[i].SetDisplay("");
                if (player.inventory[i] != null && player.inventory[i].type > 0)
                {
                    Item item = player.inventory[i];
                    WeaponIcon[i].SetIcon(Terraria.GameContent.TextureAssets.Item[item.type].Value, Color.White);
                    WeaponIcon[i].SetDisplay(item.Name);
                    if (!item.GetGlobalItem<InscriptionSettings>().CanUpgrade) WeaponIcon[i].display += '\n' + CNCLocalization.GetText("UI.Enhancement.NoLevel");
                    else if (!item.GetGlobalItem<InscriptionSettings>().CanUpgrade2) WeaponIcon[i].display += '\n' + CNCLocalization.GetText("UI.Enhancement.MaxLevel");

                    Rectangle frame = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(WeaponIcon[i].icon) : WeaponIcon[i].icon.Frame(1, 1, 0, 0);
                    WeaponIcon[i].iconDrawRect = frame;
                    WeaponIcon[i].iconSize = frame.Size() * scale;
                    WeaponIcon[i].iconOffset = new Vector2(8f) * scale;
                    if (WeaponIcon[i].iconSize.X > WeaponIcon[i].width - 8f * 2f * scale)
                        WeaponIcon[i].iconSize = WeaponIcon[i].iconSize / (WeaponIcon[i].iconSize.X / (WeaponIcon[i].width - 8f * 2f * scale));
                    if (WeaponIcon[i].iconSize.Y > WeaponIcon[i].height - 8f * 2f * scale)
                        WeaponIcon[i].iconSize = WeaponIcon[i].iconSize / (WeaponIcon[i].iconSize.Y / (WeaponIcon[i].height - 8f * 2f * scale));
                    WeaponIcon[i].iconOffset = WeaponIcon[i].box.Size() * scale / 2f - WeaponIcon[i].iconSize / 2;
                }
            }
            if (NoWay > 0) NoWay--;
            if (Visible) player.mouseInterface = true;
            DrawCursor();
        }

        private void SelectWeapon(int idx)
        {
            if (NoWay > 0) { NoWay = 0; return; }
            if (player.inventory[idx].type > 0)
            {
                if (player.inventory[idx].GetGlobalItem<InscriptionSettings>().CanUpgrade2)
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);//10
                    WeaponList.index = idx;
                    Visible = false;
                    mouseLeft = false;
                    mouseLeftRelease = true;
                    return;
                }
            }
            SoundEngine.PlaySound(SoundID.MenuOpen);//11
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            if (!MainBox.IsMouseHovering)
            {
                Visible = false;
                SoundEngine.PlaySound(SoundID.MenuOpen);//11
            }
        }
    }
}