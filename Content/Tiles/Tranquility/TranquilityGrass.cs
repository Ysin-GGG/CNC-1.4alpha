using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;

namespace CNC.Content.Tiles.Tranquility
{
    public class TranquilityGrass : CNCTile
    {
        public override void ResetStaticDefaults()
        {
            Clone(2);
            Merge(CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityDirt], true);
            Merge(CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityStone], true);
            Merge(CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGrass], true);
            MapColor = new Color(100, 218, 157);
            DustType = DustID.Grass;
            SetModTree(new Trees.SpiritTree());
        }

        public override void ModifyBreak(Tile tile, Point pos, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail) { return; }
            noItem = true;
            effectOnly = true;
            SetTile(pos, CNCID.Tile.TranquilityDirt);
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return CNCID.Tile.LegacyObject[CNCID.Tile.SpiritSapling];
        }
        /*
        public override void Update(Tile tile, Tile safeTile, Point pos)
        {
            int x = pos.X; int y = pos.Y - 1;
            int type = Main.tile[x, y].type;
            bool isPlants1 = type == 3 || type == 24 || type == 61 || type == 110 || type == 201;
            bool isPlants2 = type == 73 || type == 74 || type == 113;
            if (isPlants1 || isPlants2)
            {
                Tile plant = Main.tile[x, y];
                bool replace = false;
                if (type == 201 && plant.frameX == 270) { replace = true; }//如果为毒蘑菇
                if ((type == 24|| type == 110) && plant.frameX == 144) { replace = true; }//如果为邪恶蘑菇或神圣蘑菇
                if (replace)
                {
                    SetTile(Method.P(x, y), 3);
                    Main.tile[x, y].frameX = 144;//替换为普通蘑菇
                    WorldGen.SquareTileFrame(x, y, true);
                    if (CNC.Multiplayer) { NetMessage.SendTileSquare(-1, x, y, 1); }
                    return;
                }
                if ((type == 61 && plant.frameX == 144) || type == 71)//如果为丛林孢子或发光蘑菇
                {
                    Main.tile[x, y].type = 0;
                    Main.tile[x, y].active(false);//破坏该植物
                    WorldGen.SquareTileFrame(x, y, true);
                    if (CNC.Multiplayer) { NetMessage.SendTileSquare(-1, x, y, 1); }
                    return;
                }
                if ((type == 3) && plant.frameX == 144) { return; }//如果为普通蘑菇则不替换杂草类型
                short frameX = plant.frameX;
                SetTile(Method.P(x, y), CNC.GetType(3, isPlants1 ? 10 : 11));
                Main.tile[x, y].frameX = frameX;
                WorldGen.SquareTileFrame(x, y, true);
                if (CNC.Multiplayer) { NetMessage.SendTileSquare(-1, x, y, 1); }
            }
            if (WorldGen.genRand.Next(10) == 0) { AutoConvert(pos); }
            if (!Main.tile[pos.X, pos.Y - 1].active()) { SetTile(Method.P(pos.X, pos.Y - 1), CNC.GetType(3, 10)); }
        }

        private void AutoConvert(Point pos, int size = 4)
        {
            for (int x = pos.X - size; x <= pos.X + size; x++)
            {
                for (int y = pos.Y - size; y <= pos.Y + size; y++)
                {
                    if (WorldGen.InWorld(x, y, 1) && Math.Abs(x - pos.X) + Math.Abs(y - pos.Y) < Math.Sqrt(size * size * 2))
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
        }
        */
    }
}