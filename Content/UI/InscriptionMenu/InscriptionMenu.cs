using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Players;

namespace CNC.Content.UI.InscriptionMenu
{
    public class InscriptionMenu : UIState
    {
        public UIPanel Background;
        public UIPanel MainBox;

        private Rectangle rect => new Rectangle(0, 0, (int)screen.X + 1, (int)screen.Y + 1);
        private const float blackSize = 200f;
        public static bool Visible = false;

        public override string Name => "InscriptionMenu";

        public override void Initialize()
        {
            CanDrag = false;
            CanOutScreen = true;
            Offset = Vector2.Zero;
            Size = screen + Vector2.One;
            Visible = false;
            children.Add(Background = new UIPanel());
            children.Add(MainBox = new UIPanel());
            id = 3;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            float scale = MathHelper.Min((rect.Width - blackSize) / GetImage("InscriptionWindow").Width, (rect.Height - blackSize) / GetImage("InscriptionWindow").Height);
            Size = rect.Size();
            Offset = Vector2.Zero;

            Background.SetRect(Vector2.Zero, rect.Size());
            Background.SetBox(GetImage("DarkScreen"), corner: Vector2.Zero);

            MainBox.SetBox(GetImage("InscriptionWindow"), corner: Vector2.Zero);
            MainBox.Size = MainBox.image.Size();
            MainBox.Center = rect.Center();

            if (Visible)
            {
                if (Main.playerInventory && !DarkScreen.Visible) DarkScreen.Visible = true;
                player.mouseInterface = true;
            }
            DrawCursor();
        }
    }
}