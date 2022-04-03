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
        public UIImage TargetInscriptionBox;
        public UIButton PreviousInscription;
        public UIButton NextInscription;
        public UIButton EnhanceRequirement;//强化按钮
        public UIButton TargetWeaponIcon;//中心武器图标
        public UIButton[] InscriptionIcon1;//已选铭文
        public UIButton[] InscriptionIcon2;//待选铭文

        private Rectangle rect => new Rectangle(0, 0, (int)screen.X + 1, (int)screen.Y + 1);
        private const float blackSize = 200f;
        public static bool Visible = false;
        public Item TargetWeapon => (WeaponList.index < 0 || player.inventory[WeaponList.index].type <= 0) ? null : player.inventory[WeaponList.index];
        public InscriptionSettings TargetWeaponSettings => (TargetWeapon != null && TargetWeapon.type > 0) ? TargetWeapon.GetGlobalItem<InscriptionSettings>() : null;
        public Inscription TargetInsciption => (TargetWeapon != null && TargetWeapon.type > 0 && TargetWeaponSettings != null && TargetWeaponSettings.CanUpgrade2) ? TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level][page] : null;
        private int pageMax => (TargetWeapon != null && TargetWeapon.type > 0 && TargetWeaponSettings != null && TargetWeaponSettings.CanUpgrade2) ? TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level].Length : 0;
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
            AddButton(ref MainBox, ref InscriptionIcon2[0], new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, -2)));
            AddButton(ref MainBox, ref InscriptionIcon2[1], new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, -1)));
            AddButton(ref MainBox, ref InscriptionIcon2[2]);
            AddButton(ref MainBox, ref InscriptionIcon2[3], new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, 1)));
            AddButton(ref MainBox, ref InscriptionIcon2[4], new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, 2)));
            AddButton(ref MainBox, ref TargetWeaponIcon, new MouseEvent((evt, e) => OpenWeaponList(evt, (UIElement)e)));
            AddButton(ref MainBox, ref EnhanceRequirement, new MouseEvent((evt, e) => OnEnhance(evt, (UIElement)e, TargetInsciption)));
            AddButton(ref MainBox, ref PreviousInscription, new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, -1)));
            AddButton(ref MainBox, ref NextInscription, new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, 1)));
            InscriptionIcon2[2].Append(TargetInscriptionBox = new UIImage());
            TargetWeaponIcon.OnRightClick += ClearWeapon;
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

            if (page > pageMax) page = 0;

            for (int i = 0; i < InscriptionIcon1.Length; i++) InscriptionIcon1[i].Dead = true;

            TargetWeaponIcon.MouseDead = WeaponList.Visible;
            if (TargetWeapon != null && TargetWeaponSettings.CanUpgrade)//绘制图标文本
            {
                Item item = TargetWeapon;
                string itemName = item.ModItem.Name;
                string itemDesc = $"[{item.Name}]";
                if (ModContent.Request<Texture2D>($"CNC/Assets/Inscription/Item/{itemName}").Value != null)
                {
                    for (int i = 0; i < item.ToolTip.Lines; i++) itemDesc += '\n' + item.ToolTip.GetLine(i);
                    TargetWeaponIcon.icon = ModContent.Request<Texture2D>($"CNC/Assets/Inscription/Item/{itemName}").Value;
                    TargetWeaponIcon.display = itemDesc + "\n------------\n" + CNCLocalization.GetText($"UI.Enhancement.SelectWeapon");
                }
                for (int i = 0; i < TargetWeaponSettings.levelMax - 1; i++)
                    InscriptionIcon1[i].Dead = false;
            }

            if (TargetWeapon != null && TargetWeaponSettings != null)
                for (int i = 0; i < TargetWeaponSettings.levelMax - 1; i++)//绘制已有铭文槽
                {
                    Vector2 pos = new Vector2(-60, 0).RotatedBy(-MyMath.pi / TargetWeaponSettings.levelMax * (i + 1));//此处角度和半径用负值，逆时针旋转绘制
                    InscriptionIcon1[i].SetIcon(GetImage("InscriptionBox"), Color.White * .9f, Color.White);
                    InscriptionIcon1[i].SetRect(pos * posScale * scale, InscriptionIcon1[i].icon.Size() * scale);
                    InscriptionIcon1[i].SetDisplay(Language.GetTextValue("LegacyInterface.23") + '\n' + Lang.tip[1], Color.White);
                    InscriptionIcon1[i].Center = new Vector2(MainBox.Center.X, TargetWeaponIcon.Center.Y) - MainBox.Offset + pos * posScale * scale;
                    InscriptionIcon1[i].MouseDead = WeaponList.Visible;
                    int innate = i + TargetWeaponSettings.innateness;//排除固有铭文
                    if (innate >= TargetWeaponSettings.inscription.Length) continue;//超过已有铭文数则不绘制
                    Inscription inscription = TargetWeaponSettings.inscription[innate];
                    if (i < TargetWeaponSettings.level)
                    {
                        InscriptionIcon1[i].icon = ModContent.Request<Texture2D>($"CNC/Assets/Inscription/{(inscription.Name.StartsWith("S_") ? "Special" : "Common")}/{inscription.Name}").Value;
                        string desc = "";
                        string desc2 = inscription.GetTooltip();
                        while (desc != desc2) { desc = desc2; Inscription.GetFirstDamageCommon(ref desc2); }
                        InscriptionIcon1[i].display = $"[{inscription.displayName}]\n{desc}";
                    }
                }

            for (int i = 0; i < InscriptionIcon2.Length; i++)//绘制待选铭文槽
            {
                int middle = i - 2;
                Vector2 pos = new Vector2(85 + 25 * middle, 97) + (middle == 0 ? Vector2.Zero : new Vector2(10 * middle / Math.Abs(middle), 0));
                InscriptionIcon2[i].SetIcon(GetImage("InscriptionBox"), Color.White * .9f, Color.White);
                InscriptionIcon2[i].SetRect(pos * posScale * scale, InscriptionIcon2[i].icon.Size() * scale);
                InscriptionIcon2[i].SetDisplay(Language.GetTextValue("LegacyInterface.23") + '\n' + Lang.tip[1], Color.White);
                InscriptionIcon2[i].MouseDead = WeaponList.Visible;
                if (TargetWeapon == null || TargetWeaponSettings == null || !TargetWeaponSettings.CanUpgrade2) continue;
                int num = TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level].Length;
                Inscription inscription = TargetWeaponSettings.inscriptionList[TargetWeaponSettings.level][(middle + page + num) % num];
                InscriptionIcon2[i].icon = ModContent.Request<Texture2D>($"CNC/Assets/Inscription/{(inscription.Name.StartsWith("S_") ? "Special" : "Common")}/{inscription.Name}").Value;
                string desc = "";
                string desc2 = inscription.GetTooltip();
                while (desc != desc2) { desc = desc2; Inscription.GetFirstDamageCommon(ref desc2); }
                InscriptionIcon2[i].display = $"[{inscription.displayName}]\n{desc}";
            }
            TargetInscriptionBox.SetImage(GetImage("CenterInscription"), CNCUtils.AccurateColor(CNCUtils.AccurateColor(Color.White)));
            TargetInscriptionBox.SetRect(Vector2.Zero, TargetInscriptionBox.image.Size() * scale);

            if (Visible)
            {
                if (Main.playerInventory && !DarkScreen.Visible) DarkScreen.Visible = true;
                player.mouseInterface = true;
            }

            MouseText.textOffset = new Vector2(16f);
            for (int i = 0; i < InscriptionIcon2.Length; i++) if (InscriptionIcon2[i].IsMouseHovering) mouseText = InscriptionIcon2[i].display; else InscriptionIcon2[i].display = "";
            /*
            mouseText = "";
            for (int i = 0; i < InscriptionIcon1.Length; i++) if (InscriptionIcon1[i].IsMouseHovering) mouseText = InscriptionIcon1[i].display; else InscriptionIcon1[i].display = "";
            for (int i = 0; i < InscriptionIcon2.Length; i++) if (InscriptionIcon2[i].IsMouseHovering) mouseText = InscriptionIcon2[i].display; else InscriptionIcon2[i].display = "";
            if (EnhanceRequirement.IsMouseHovering) mouseText = CNCLocalization.GetText($"UI.Enhancement.CanEnhance") + '\n' + EnhanceRequirement.display;
            else EnhanceRequirement.display = "";
            if (TargetWeaponIcon.IsMouseHovering) mouseText = TargetWeaponIcon.display;
            else TargetWeaponIcon.display = "";
            if (PreviousInscription.IsMouseHovering) mouseText = TargetWeaponIcon.display;
            else PreviousInscription.display = "";
            if (NextInscription.IsMouseHovering) mouseText = TargetWeaponIcon.display;
            else NextInscription.display = "";
            */
            if (!WeaponList.Visible) DrawCursor();
            else mouseText = "";
        }

        private void ChangePage(UIMouseEvent evt, UIElement e, int num)
        {
            if (pageMax == 0) page = 0;
            else page = (page + num + pageMax) % pageMax;
        }

        private void OnEnhance(UIMouseEvent evt, UIElement e, Inscription inscription)
        {
            if (inscription == null || WeaponList.index < 0) return;
            page = 0;
            SoundEngine.PlaySound(2, player.Center, 37);
            player.inventory[WeaponList.index].GetGlobalItem<InscriptionSettings>().level++;
            player.inventory[WeaponList.index].GetGlobalItem<InscriptionSettings>().SetInscription(-1, inscription);
        }

        private void OpenWeaponList(UIMouseEvent evt, UIElement e)
        {
            WeaponList.Visible = true;
            WeaponList.NoWay = 2;
            MouseText.Dead = true;
            TargetWeaponIcon.display = "";
            mouseText = "";
        }

        private void ClearWeapon(UIMouseEvent evt, Terraria.UI.UIElement e)
        {
            WeaponList.index = -1;
            SoundEngine.PlaySound(11);
            mouseText = TargetWeaponIcon.display = "";
        }
    }
}