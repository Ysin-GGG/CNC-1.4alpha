using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace CNC.Content.Tiles.Trees
{
    public abstract class SpiritTree : CNCTree
    {
        public override int Dust => 78;
        public override int Drop => CNCID.Item.LegacyObject[Main.rand.NextBool(10) ? CNCID.Item.SpiritSapling : CNCID.Item.SpiritWood];
        public override int Growth => ModContent.GoreType<SpiritTreeFX>();
        public override string FullName => "CNC/Content/Tiles/Trees/SpiritTree";
        public override int SaplingGrowthType(ref int style) => ModContent.TileType<Tranquility.SpiritSapling>();

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { CNCID.Tile.TranquilityGrass };
        }

        public class SpiritTreeFX : ModGore
        {
            public override void SetStaticDefaults()
            {
                GoreID.Sets.SpecialAI[Type] = 3;
            }

            public override string Texture => "CNC/Content/Tiles/Trees/SpiritTreeFX";

            public override void OnSpawn(Gore gore, IEntitySource source)
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