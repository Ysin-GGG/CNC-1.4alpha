using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Players;

namespace CNC.Content.UI.ClassesMenu
{
    public class ClassesMenu : UIState
    {
        private Player player => Main.LocalPlayer;
        private ClassesSettings modPlayer => player.GetModPlayer<ClassesSettings>();

        public UIPanel Background;
        public UIPanel MainBox;
        public UIImage ClassIcon;
        public UIButton PreviousClass;
        public UIButton NextClass;
        public UIButton Confirm;
        public UIButton Reset;
        public UIButton Preview;

        private Rectangle rect => new Rectangle(0, 0, (int)screen.X + 1, (int)screen.Y + 1);
        private const float blackSize = 200f;
        private const int pageMax = 6;
        private static int page = 0;
        private string[] clazz => new string[pageMax]
        {
            CNCID.Clazz.Collection[CNCID.Clazz.Barbarian],
            CNCID.Clazz.Collection[CNCID.Clazz.Crusader],
            CNCID.Clazz.Collection[CNCID.Clazz.Ranger],
            CNCID.Clazz.Collection[CNCID.Clazz.Musketeer],
            CNCID.Clazz.Collection[CNCID.Clazz.Cleric],
            //CNCID.Clazz.Collection[CNCID.Clazz.Wizard],
            CNCID.Clazz.Collection[CNCID.Clazz.Assassin],
        };
        private byte[] clazz2 => new byte[pageMax]
        {
            CNCID.Clazz.Barbarian,
            CNCID.Clazz.Crusader,
            CNCID.Clazz.Ranger,
            CNCID.Clazz.Musketeer,
            CNCID.Clazz.Cleric,
            //CNCID.Clazz.Wizard,
            CNCID.Clazz.Assassin,
        };

        private Dictionary<byte, short> clazz3 => new Dictionary<byte, short>()
        {
            { CNCID.Clazz.Barbarian, 0 },
            { CNCID.Clazz.Crusader, 1 },
            { CNCID.Clazz.Ranger, 2 },
            { CNCID.Clazz.Musketeer, 3 },
            { CNCID.Clazz.Cleric, 4 },
            //{ CNCID.Clazz.Wizard, 5 },
            { CNCID.Clazz.Assassin, 5 },
        };

        public static bool Visible = false;

        public override string Name => "ClassesMenu";

        public override void Initialize()
        {
            CanDrag = false;
            CanOutScreen = true;
            Offset = Vector2.Zero;
            Size = screen + Vector2.One;
            Visible = false;
            children.Add(Background = new UIPanel());
            children.Add(MainBox = new UIPanel());
            MainBox.Append(ClassIcon = new UIImage());
            AddButton(ref Confirm, new MouseEvent((evt, e) => SelectClass(evt, e)));
            AddButton(ref PreviousClass, new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, -1)));
            AddButton(ref NextClass, new MouseEvent((evt, e) => ChangePage(evt, (UIElement)e, 1)));
            AddButton(ref Reset, new MouseEvent((evt, e) => ResetClass(evt, e)));
            AddButton(ref Preview);
            PreviousClass.iconDrawStyle = 1;
            //page = clazz3[modPlayer.clazz];
            id = 2;
        }

        private void AddButton(ref UIButton button, MouseEvent evt = null)
        {
            button = new UIButton();
            button.OnMouseOver += ShowText;
            button.OnMouseOut += HideText;
            if (evt != null) button.OnClick += evt;
            MainBox.Append(button);
        }

        private void ResetButton(ref UIButton button, Vector2 pos, string icon, string box, string name, string arg = "")
        {
            float scale = MathHelper.Min((rect.Width - blackSize) / GetImage("Windows").Width, (rect.Height - blackSize) / GetImage("Windows").Height);
            button.SetIcon(GetImage(icon), Color.White * .8f, CNCUtils.AccurateColor(Color.White));
            button.SetBox(GetImage(box), Color.Transparent, Color.White, 0f);
            button.SetRect(pos, button.icon.Size() * scale);
            button.SetDisplay(CNCLocalization.GetText($"UI.{Name}.{name}", arg));
            button.transformSpeed = .15f;
            button.text.Visible = false;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            float scale = MathHelper.Min((rect.Width - blackSize) / GetImage("Windows").Width, (rect.Height - blackSize) / GetImage("Windows").Height);
            Size = rect.Size();
            Offset = Vector2.Zero;

            Background.SetRect(Vector2.Zero, rect.Size());
            Background.SetBox(GetImage("DarkScreen"), corner: Vector2.Zero);

            MainBox.SetBox(GetImage("Windows"), corner: Vector2.Zero);
            MainBox.Size = MainBox.image.Size() * scale;
            MainBox.Center = rect.Center();

            ResetButton(ref Confirm, new Vector2(297f, 288f) * scale, "Button1", "Box1", "Confirm", CNCLocalization.GetText("Class." + clazz[page]));
            ResetButton(ref PreviousClass, new Vector2(120f, 183f) * scale, "Button2", "Box2", "PreviousClass", CNCLocalization.GetText("Class." + clazz[(page - 1 + pageMax) % pageMax]));
            ResetButton(ref NextClass, new Vector2(MainBox.width, 0) + new Vector2(-120f - PreviousClass.icon.Width, 183f) * scale, "Button2", "Box2", "NextClass", CNCLocalization.GetText("Class." + clazz[(page + 1) % pageMax]));
            ResetButton(ref Reset, new Vector2(483f, 96f) * scale, "Button3", "Box3", "Reset", CNCLocalization.GetText($"Item.Name.DestineScroll") + " *1");

            Preview.SetIcon(GetImage("Preview"), Color.White, new Color(128, 128, 128, 255));
            Preview.SetRect(new Vector2(228f, 105f) * scale, Preview.icon.Size() * scale);
            Preview.SetDisplay(CNCLocalization.GetText($"ClassPreview.{clazz[page]}"));
            Preview.text.Visible = false;

            ClassIcon.SetImage(GetImage(clazz[page]));
            ClassIcon.SetRect(MainBox.Size / 2 - new Vector2(76.5f, 72f) * scale, GetImage(clazz[page]).Size() * scale);
            
            if (PreviousClass.IsMouseHovering) PreviousClass.display = CNCLocalization.GetText($"UI.{Name}.PreviousClass", CNCLocalization.GetText("Class." + clazz[(page - 1 + pageMax) % pageMax]));
            if (NextClass.IsMouseHovering) NextClass.display = CNCLocalization.GetText($"UI.{Name}.NextClass", CNCLocalization.GetText("Class." + clazz[(page + 1 + pageMax) % pageMax]));
            
            if (modPlayer.clazz > 0) Confirm.display += '\n' + CNCLocalization.GetText($"UI.{Name}.NeedReset");
            else Reset.display = CNCLocalization.GetText($"UI.{Name}.NoClass");
            if (modPlayer.clazz == clazz2[page]) Confirm.display = CNCLocalization.GetText($"UI.{Name}.SameClass");

            if (Confirm.IsMouseHovering) MouseText.text = Confirm.display;
            if (PreviousClass.IsMouseHovering) MouseText.text = PreviousClass.display;
            if (NextClass.IsMouseHovering) MouseText.text = NextClass.display;
            if (Reset.IsMouseHovering) MouseText.text = Reset.display;

            if (Visible)
            {
                if (Main.playerInventory && !DarkScreen.Visible) DarkScreen.Visible = true;
                player.mouseInterface = true;
            }
            DrawCursor();
        }

        private void ChangePage(UIMouseEvent evt, UIElement e, int num)
        {
            page = (page + num + pageMax) % pageMax;
        }

        private void SelectClass(UIMouseEvent evt, Terraria.UI.UIElement e)
        {
            if (modPlayer.clazz > 0)  Main.NewText(CNCLocalization.GetText($"UI.{Name}.NeedReset"), Color.Red);
            else if (modPlayer.clazz == clazz2[page]) Main.NewText(CNCLocalization.GetText($"UI.{Name}.SameClass"), Color.Red);
            else
            {
                string arg = CNCLocalization.GetText($"Class.{clazz[page]}");
                Main.NewText(CNCLocalization.GetText($"UI.{Name}.SelectClass", arg), Color.LightGreen);
                SoundEngine.PlaySound(2, player.Center, 37);
                SetClassDefaults(clazz2[page]);
            }
        }

        private void ResetClass(UIMouseEvent evt, Terraria.UI.UIElement e)
        {
            int id = CNCID.Item.LegacyObject[CNCID.Item.DestineScroll];
            if (modPlayer.clazz == 0)  Main.NewText(CNCLocalization.GetText($"UI.{Name}.NoClass"), Color.Red); 
            else if (CountItem(id, player) > 0)
            {
                ReduceItem(id, 1, player);
                Main.NewText(CNCLocalization.GetText($"UI.{Name}.ResetClass"), Color.LightGreen);
                SoundEngine.PlaySound(2, player.Center, 37);
                SetClassDefaults(0);
            }
            else Main.NewText(CNCLocalization.GetText($"UI.{Name}.NoMoney"), Color.Red);
        }

        private void SetClassDefaults(byte clazz)
        {
            Player p = player;
            ClassesSettings ap = modPlayer;
            ap.clazz = clazz;
            ap.ResetClassVar(true, p);
        }

        private static void ReduceItem(int type, int amount, Player player = null)
        {
            if (player == null) { player = Const.Me; }
            if (CountItem(type) < amount) { return; }
            if (type >= 0)
                for (int j = 0; j < amount; j++)
                    for (int i = 0; i < 50; i++)
                        if (player.inventory[i].type == type)
                        {
                            player.inventory[i].stack--;
                            break;
                        }
        }

        private static int CountItem(int type, Player player = null)
        {
            int count = 0;
            if (player == null) { player = Const.Me; }
            if (player.HasItem(type) && type >= 0)
                for (int i = 0; i < 50; i++)
                    if (player.inventory[i].type == type)
                        count += player.inventory[i].stack;
            return count;
        }
    }
}