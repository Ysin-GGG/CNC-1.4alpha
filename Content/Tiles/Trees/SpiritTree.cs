using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;

namespace CNC.Content.Tiles.Trees
{
    public class SpiritTree : CNCTree
    {
        public override int Dust => 78;
        public override int Drop => CNCID.Item.LegacyObject[Main.rand.NextBool(10) ? CNCID.Item.SpiritSapling : CNCID.Item.SpiritWood];
        public override int Growth => ModContent.GoreType<SpiritTreeFX>();
        public override string FullName => "CNC/Content/Tiles/Trees/SpiritTree";
        public override bool CanDropAcorn() => false;

        public class SpiritTreeFX : ModGore
        {
            public override void OnSpawn(Gore gore)
            {
                gore.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MyMath.pi * 2);
                gore.numFrames = 8;
                gore.frame = (byte)Main.rand.Next(8);
                gore.frameCounter = (byte)Main.rand.Next(8);
                UpdateType = 910;
            }
        }
    }
}