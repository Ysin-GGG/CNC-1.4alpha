
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;

namespace CNC.Content.Tiles.Tranquility
{
	public class SpiritSapling : CNCTile
	{
        public override void ResetStaticDefaults()
        {
            Clone(20);
            /*
            noAttach = true;
            randFrame = false;
            lavaBreak = true;
            */
            DustType = DustID.t_LivingWood;
            AdjTiles = new int[] { TileID.Saplings };
            MapColor = new Color(136, 232, 201);
            tile.CopyFrom(GetStyle(1, 2));
            tile.Width = 1;
            tile.Height = 2;
            tile.Origin = new Point16(0, 1);
            tile.AnchorBottom = new AnchorData(AnchorType.SolidTile, tile.Width, 0);
            tile.UsesCustomCanPlace = true;
            tile.CoordinateHeights = new[] { 16, 18 };
            tile.CoordinateWidth = 16;
            tile.CoordinatePadding = 2;
            tile.AnchorValidTiles = new int[] { CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGrass] };
            tile.StyleHorizontal = true;
            tile.DrawFlipHorizontal = true;
            tile.WaterPlacement = LiquidPlacement.NotAllowed;
            tile.LavaDeath = true;
            tile.RandomStyleRange = 3;
            tile.StyleMultiplier = 3;
            addTile();
        }

        public override void Update(Tile tile, Tile safeTile, Point pos)
        {
            int x = pos.X;
            int y = pos.Y;
            if (true)
            {
                bool growSucess; // A bool to see if the tree growing was sucessful.
                // Style 0 is for the ExampleTree sapling, and style 1 is for ExamplePalmTree, so here we check frameX to call the correct method.
                // Any pixels before 54 on the tilesheet are for ExampleTree while any pixels above it are for ExamplePalmTree
                if (safeTile.TileFrameX < 54) growSucess = WorldGen.GrowTree(x, y);
                else growSucess = WorldGen.GrowPalmTree(x, y);
                // A flag to check if a player is near the sapling
                bool isPlayerNear = WorldGen.PlayerLOS(x, y);
                //If growing the tree was a sucess and the player is near, show growing effects
                if (growSucess && isPlayerNear) WorldGen.TreeGrowFXCheck(x, y);
            }
        }
        
        public override void SpriteEffect(Tile tile, Point pos, ref bool randH, ref SpriteEffects effect) => randH = true;
    }
}