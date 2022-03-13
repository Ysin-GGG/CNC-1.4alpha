using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CNC.Common.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.UI.ClassesMenu
{
    public class DarkScreen : UIState
    {
        public UIPanel MainBox;

        private int blackTime;
        private const float alphaSpeed = .1f;
        private float alpha;
        public static bool Visible;

        public override string Name => "ClassesMenu";

        public override void Initialize()
        {
            CanDrag = false;
            CanOutScreen = true;
            Offset = new Vector2(0);
            Size = screen;
            alpha = 0f;
            blackTime = 0;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            alpha += (Visible ? 1 : -1) * alphaSpeed;
            if (alpha >= 1f)
            {
                alpha = 1f;
                if (blackTime++ > 15)
                {
                    Visible = false;
                    blackTime = 0;
                    ClassesMenu.Visible = !ClassesMenu.Visible;
                }
            }
            if (Visible)
            {
                Main.playerInventory = ClassesMenu.Visible;
            }
            if (!Visible && alpha <= 0f) alpha = 0f;
            if (Visible) Main.LocalPlayer.mouseInterface = true;
            
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            if (alpha > 0f)
            {
                Color cursorColor = Main.LocalPlayer.hasRainbowCursor ? Main.DiscoColor : Main.cursorColor;
                Vector2 bonus = Main.DrawThickCursor(false);
                spriteBatch.Draw(GetImage("DarkScreen"), new Rectangle(0, 0, (int)screen.X, (int)screen.Y), null, Color.White * alpha);
                spriteBatch.Draw(TextureAssets.Cursors[0].Value, Main.MouseScreen + bonus + Vector2.One, null, new Color((int)(cursorColor.R * 0.200000002980232), (int)(cursorColor.G * 0.200000002980232), (int)(cursorColor.B * 0.200000002980232), (int)(cursorColor.A * 0.5)), 0.0f, Vector2.Zero, Main.cursorScale, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(TextureAssets.Cursors[0].Value, Main.MouseScreen + bonus, null, cursorColor, 0.0f, Vector2.Zero, Main.cursorScale, SpriteEffects.None, 0.0f);
            }
        }
    }
}