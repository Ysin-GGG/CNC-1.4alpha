using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;

namespace CNC.Content.Tiles.Tranquility
{
    public class TranquilityStone : CNCTile
    {
        public override void ResetStaticDefaults()
        {
            Clone(1);
            Merge(CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityDirt], true);
            Merge(CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityStone], true);
            Merge(CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGrass], true);
            MapColor = new Color(125, 125, 125);
            DustType = DustID.Stone;
            SoundType = SoundID.Tink;
            SetModTree(new Trees.TranquilityGemTree());
            ItemDrop = CNCID.Item.LegacyObject[CNCID.Item.TranquilityStone];
        }
        /*
        public override void Update(Tile tile, Tile safeTile, Point pos)
        {
            if (WorldGen.genRand.Next(10) == 0) { AutoConvert(pos); }
        }

        private void AutoConvert(Point pos, int size = 4)
        {
            for (int x = pos.X - size; x <= pos.X + size; x++)
            {
                for (int y = pos.Y - size; y <= pos.Y + size; y++)
                {
                    if (WorldGen.InWorld(x, y, 1) && Math.Abs(x - pos.X) + Math.Abs(y - pos.Y) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[x, y].type;
                        int wall = Main.tile[x, y].wall;

                        if (TileID.Sets.Conversion.Grass[type])
                        {
                            Main.tile[x, y].type = (ushort)ModContent.TileType<TranquilGrass>();
                            WorldGen.SquareWallFrame(x, y, true);
                            NetMessage.SendTileSquare(-1, x, y, 1);
                        }
                        else if (TileID.Sets.Conversion.Stone[type])
                        {
                            Main.tile[x, y].type = (ushort)ModContent.TileType<TranquilStone>();
                            WorldGen.SquareWallFrame(x, y, true);
                            NetMessage.SendTileSquare(-1, x, y, 1);
                        }
                    }
                }
            }
        */
    }
}