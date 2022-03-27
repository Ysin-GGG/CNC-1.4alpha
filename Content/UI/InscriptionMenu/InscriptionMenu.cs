using System;
using Terraria;
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
    public class InscriptionMenu : UIState
    {
        public UIPanel Background;
        public UIPanel MainBox;
        public UIButton PreviousInscription;
        public UIButton NextInscription;
        public UIButton EnhanceRequirement;
        public UIButton TargetWeaponIcon;
        public UIButton[] InscriptionIcon1;//已选铭文
        public UIButton[] InscriptionIcon2;//待选铭文

        private Rectangle rect => new Rectangle(0, 0, (int)screen.X + 1, (int)screen.Y + 1);
        private const float blackSize = 200f;
        public static bool Visible = false;
        public Item[] Weapon = new Item[50];
        public Item TargetWeapon => player.HeldItem;
        public InscriptionSettings TargetWeaponSettings => TargetWeapon.GetGlobalItem<InscriptionSettings>();
        public Inscription TargetInsciption => TargetWeaponSettings.CanUpgrade2 ? TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level][page] : null;
        private int pageMax => TargetWeaponSettings.CanUpgrade2 ? TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level].Length : 0;
        private int page = 0;

        public override string Name => "InscriptionMenu";

        public override void Initialize()
        {
            CanDrag = false;
            CanOutScreen = true;
            Offset = Vector2.Zero;
            Size = screen + Vector2.One;
            Visible = false;
            InscriptionIcon1 = new UIButton[CNC.maxInscriptions];
            InscriptionIcon2 = new UIButton[5];
            children.Add(Background = new UIPanel());
            children.Add(MainBox = new UIPanel());
            for (int i = 0; i < CNC.maxInscriptions; i++) AddButton(ref MainBox, ref InscriptionIcon1[i]);
            for (int i = 0; i < 5; i++) AddButton(ref MainBox, ref InscriptionIcon2[i]);
            AddButton(ref MainBox, ref TargetWeaponIcon);
            AddButton(ref MainBox, ref EnhanceRequirement, new MouseEvent((evt, e) => OnEnhance(evt, (UIElement)e, TargetInsciption)));
            AddButton(ref MainBox, ref PreviousInscription, new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, -1)));
            AddButton(ref MainBox, ref NextInscription, new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, 1)));
            id = 3;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            float posScale = 3f;
            float scale = MathHelper.Min((rect.Width - blackSize) / GetImage("InscriptionWindow").Width, (rect.Height - blackSize) / GetImage("InscriptionWindow").Height);
            Size = rect.Size();
            Offset = Vector2.Zero;

            Background.SetRect(Vector2.Zero, rect.Size());
            Background.SetBox(GetImage("DarkScreen"), corner: Vector2.Zero);

            MainBox.SetBox(GetImage("InscriptionWindow"), corner: Vector2.Zero);
            MainBox.Size = MainBox.image.Size() * scale;
            MainBox.Center = rect.Center();

            ResetButton(ref EnhanceRequirement, new Vector2(158f, 24f) * posScale, scale, "EnhanceRequirement", "EnhanceRequirementBox", "EnhanceRequirement", Language.GetTextValue("LegacyInterface.23"), UIName: "Enhancement");
            ResetButton(ref PreviousInscription, new Vector2(74f, 104f) * posScale, scale, "SwitchInscription", "SwitchInscriptionBox", "PreviousInscription", UIName: "Enhancement");
            ResetButton(ref NextInscription, new Vector2(109f, 104f) * posScale, scale, "SwitchInscription", "SwitchInscriptionBox", "NextInscription", UIName: "Enhancement");
            NextInscription.iconDrawStyle = 1;

            TargetWeaponIcon.SetIcon(GetImage("NoneWeaponIcon"), Color.White * .9f, new Color(255, 255, 255, 255));
            TargetWeaponIcon.SetRect(new Vector2(77f, 21f) * posScale * scale, TargetWeaponIcon.icon.Size() * scale);
            TargetWeaponIcon.SetDisplay(CNCLocalization.GetText($"UI.Enhancement.SelectWeapon"));
            TargetWeaponIcon.text.Visible = false;
            if (EnhanceRequirement.IsMouseHovering) mouseText = CNCLocalization.GetText($"UI.Enhancement.CanEnhance") + '\n' + mouseText;
            if (TargetWeaponIcon.IsMouseHovering) mouseText = TargetWeaponIcon.display;

            if (page > pageMax) page = 0;

            for (int i = 0; i < InscriptionIcon1.Length; i++)
                InscriptionIcon1[i].active = InscriptionIcon1[i].Visible = false;

            if (TargetWeaponSettings.CanUpgrade)//绘制图标文本
            {
                Item item = TargetWeapon;
                string itemName = item.ModItem.Name;
                string itemDesc = $"[{item.Name}]";
                for (int i = 0; i < item.ToolTip.Lines; i++)
                    itemDesc += '\n' + item.ToolTip.GetLine(i);
                if (ModContent.Request<Texture2D>($"CNC/Assets/Textures/Inscription/Item/{itemName}").Value != null)
                {
                    TargetWeaponIcon.icon = ModContent.Request<Texture2D>($"CNC/Assets/Textures/Inscription/Item/{itemName}").Value;
                    if (TargetWeaponIcon.IsMouseHovering) mouseText = itemDesc + "\n------------\n" + mouseText;
                }
                for (int i = 0; i < TargetWeaponSettings.levelMax; i++)
                    InscriptionIcon1[i].active = InscriptionIcon1[i].Visible = true;
            }

            for (int i = 0; i < TargetWeaponSettings.levelMax; i++)//绘制已有铭文槽
            {
                Vector2 pos = new Vector2(-60, 0).RotatedBy(-MyMath.pi / (TargetWeaponSettings.levelMax + 1) * (i + 1));//此处角度和半径用负值，逆时针旋转绘制
                InscriptionIcon1[i].SetIcon(GetImage("InscriptionBox"), Color.White * .9f, Color.White);
                InscriptionIcon1[i].SetRect(pos * posScale * scale, InscriptionIcon1[i].icon.Size() * scale);
                InscriptionIcon1[i].SetDisplay(Language.GetTextValue("LegacyInterface.23") + '\n' + Lang.tip[1], Color.White);
                InscriptionIcon1[i].Center = new Vector2(MainBox.Center.X, TargetWeaponIcon.Center.Y) - MainBox.Offset + pos * posScale * scale;
                int innate = i + TargetWeaponSettings.innateness;//排除固有铭文
                if (innate >= TargetWeaponSettings.inscription.Length) continue;//超过已有铭文数则不绘制
                Inscription inscription = TargetWeaponSettings.inscription[innate];
                if (i < TargetWeaponSettings.level)
                {
                    InscriptionIcon1[i].icon = ModContent.Request<Texture2D>($"CNC/Assets/Textures/Inscription/{(inscription.Name.StartsWith("S_") ? "Special" : "Common")}/{inscription.Name}").Value;
                    string desc = "";
                    string desc2 = inscription.GetTooltip();
                    while (desc != desc2) { desc = desc2; Inscription.GetFirstDamageCommon(ref desc2); }
                    InscriptionIcon1[i].display = $"[{inscription.displayName}]\n{desc}";
                }
            }

            for (int i = 0; i < InscriptionIcon2.Length; i++)//绘制待选铭文槽
            {
                int middle = i - 2;
                Vector2 pos = new Vector2(85 + 25 * middle, 98) + (middle == 0 ? Vector2.Zero : new Vector2(10 * middle / Math.Abs(middle), 0));
                InscriptionIcon2[i].SetIcon(GetImage("InscriptionBox"), Color.White * .9f, Color.White);
                InscriptionIcon2[i].SetRect(pos * posScale * scale, InscriptionIcon2[i].icon.Size() * scale);
                InscriptionIcon2[i].SetDisplay(Language.GetTextValue("LegacyInterface.23") + '\n' + Lang.tip[1], Color.White);
                if (!TargetWeaponSettings.CanUpgrade2) continue;
                int num = TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level].Length;
                Inscription inscription = TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level][(middle + page + num) % num];
                InscriptionIcon2[i].icon = ModContent.Request<Texture2D>($"CNC/Assets/Textures/Inscription/{(inscription.Name.StartsWith("S_") ? "Special" : "Common")}/{inscription.Name}").Value;
                string desc = "";
                string desc2 = inscription.GetTooltip();
                while (desc != desc2) { desc = desc2; Inscription.GetFirstDamageCommon(ref desc2); }
                InscriptionIcon2[i].display = $"[{inscription.displayName}]\n{desc}";
            }

            if (Visible)
            {
                if (Main.playerInventory && !DarkScreen.Visible) DarkScreen.Visible = true;
                player.mouseInterface = true;
            }
            DrawCursor();
        }

        private void ChangePage(UIMouseEvent evt, UIElement e, int num)
        {
            if (pageMax == 0) page = 0;
            else page = (page + num + pageMax) % pageMax;
        }

        private void OnEnhance(UIMouseEvent evt, UIElement e, Inscription inscription)
        {
            if (inscription == null) return;
            SoundEngine.PlaySound(2, player.Center, 37);
            player.HeldItem.GetGlobalItem<InscriptionSettings>().level++;
            player.HeldItem.GetGlobalItem<InscriptionSettings>().SetInscription(-1, inscription);
        }
    }
}