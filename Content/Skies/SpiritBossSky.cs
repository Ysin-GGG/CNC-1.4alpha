using Terraria;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.Skies
{
    public class SpiritBossSky : BiomeSky
    {
        public override void SetDefaults()
        {
            this.skyColor = Color.Black;
            this.tileColor = Color.Black * .75f;
            this.cloudAlpha = 1f;
        }
    }
}