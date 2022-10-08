using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.VisualEffects
{
    public class Test : VisualEffect
    {
        public override void SetDefaults()
        {
            this.deadTime = 100;
            this.opacity = 1f;
        }

        public override void Draw(SpriteBatch spriteBatch, DrawLayer layer)
        {
            Texture2D t = CNC.GetTexture("Assets/Effects/Slash4");
            if (layer == DrawLayer.overDusts)
            {
                spriteBatch.Draw(t, position - Main.screenPosition, null, Color.White * .5f, 0f, t.Center(), 1f, 0, 0);
            }
        }
    }
}