using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.ObjectData;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CNC.Common.Utils;
using CNC.Common.Localization;
using Terraria.DataStructures;

namespace CNC.Common.Global.Tiles
{
    public abstract class CNCTile : ModTile
    {
        public bool Solid { get => Main.tileSolid[Type]; set => Main.tileSolid[Type] = value; }
        public bool SolidTop { get => Main.tileSolidTop[Type]; set => Main.tileSolidTop[Type] = value; }
        //public int Merge { set => Main.tileMerge[Type][value] = Main.tileMerge[value][Type] = !Main.tileMerge[value][Type]; }
        public bool MergeDirt { get => Main.tileMergeDirt[Type]; set => Main.tileMergeDirt[Type] = value; }
        public bool RandFrame { get => !Main.tileFrameImportant[Type]; set => Main.tileFrameImportant[Type] = !value; }
        //public bool IsStone { get => Main.tileStone[Type]; set => Main.tileStone[Type] = value; }
        public bool IsBrick { get => Main.tileBrick[Type]; set => Main.tileBrick[Type] = value; }
        public bool BlockLight { get => Main.tileBlockLight[Type]; set => Main.tileBlockLight[Type] = value; }

        public bool IsGrass { get => TileID.Sets.Grass[Type]; set => TileID.Sets.Grass[Type] = value; }
        public bool IsStone { get => TileID.Sets.Stone[Type]; set => TileID.Sets.Stone[Type] = value; }

        public TileObjectData tile => TileObjectData.newTile;
        public TileObjectData subTile => TileObjectData.newSubTile;
        public TileObjectData alternate => TileObjectData.newAlternate;
        public void addTile(int type = -1) => TileObjectData.addTile(type < 0 ? Type : type);
        public void addAlternate(int style) => TileObjectData.addAlternate(style);
        public int showIcon = 0;
        public Color MapColor = Color.White;

        public override void SetStaticDefaults()
        {
            ModTranslation DisplayName = CreateMapEntryName();
            DisplayName.SetDefault(CNCLocalization.GetText($"Tile.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Tile.{Name}", 7));
            if (CNCLocalization.TextExist($"Item.{Name}")) ItemDrop = CNCID.Item.LegacyCollection[Name];
            ResetStaticDefaults();
            AddMapEntry(MapColor, DisplayName);
        }

        public sealed override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) => ModifyBreak(Main.tile[i, j],  new Point(i, j), ref fail, ref effectOnly, ref noItem);

        public sealed override void KillMultiTile(int i, int j, int frameX, int frameY) => OnBreakTiles(Main.tile[i, j], new Point(i, j), new Point(frameX, frameY));

        public sealed override void MouseOver(int i, int j) => MouseOver(Main.tile[i, j], new Point(i, j), false, Const.Me);

        public sealed override void MouseOverFar(int i, int j) => MouseOver(Main.tile[i, j], new Point(i, j), true, Const.Me);

        public sealed override bool RightClick(int i, int j) => RightClick(Main.tile[i, j], new Point(i, j), Const.Me);

        public sealed override void RandomUpdate(int i, int j) => Update(Main.tile[i, j], Framing.GetTileSafely(i, j), new Point(i, j));

        public sealed override void NumDust(int i, int j, bool fail, ref int num) => HitDust(Main.tile[i, j], new Point(i, j), !fail, ref num);

        public sealed override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            bool randH = false;
            SpriteEffects se = spriteEffects;
            SpriteEffect(Main.tile[i, j], new Point(i, j), ref randH, ref se);
            spriteEffects = randH ? (SpriteEffects)(i % 2) : se;
        }

        public virtual void ResetStaticDefaults() { }

        /// <summary> 鼠标悬浮事件 </summary>
        /// <param name="far">鼠标未悬停在物块上</param>
        /// <param name="player">当前玩家</param>
        public virtual void MouseOver(Tile tile, Point pos, bool far, Player player) { }
        public virtual bool RightClick(Tile tile, Point pos, Player player) => false;
        public virtual void ModifyBreak(Tile tile, Point pos, ref bool fail, ref bool effectOnly, ref bool noItem) { }
        public virtual void OnBreakTiles(Tile tile, Point pos, Point frame) { }
        public virtual void Update(Tile tile, Tile safeTile, Point pos) { }
        public virtual void HitDust(Tile tile, Point pos, bool broke, ref int amount) { }
        public virtual void SpriteEffect(Tile tile, Point pos, ref bool randH, ref SpriteEffects effect) { }
        
        public static void SetTile(Point pos, short type, bool isWall = false, bool flag = true, int size = 0) => CNCUtils.SetTile(pos, CNCID.Tile.LegacyObject[type], isWall, flag, size);

        public TileObjectData GetStyle(int x = 1, int y = 1)
        {
            if (x == 1)
            {
                if (y == 1) return TileObjectData.Style1x1;
                if (y == 2) return TileObjectData.Style1x2;
                return TileObjectData.Style1xX;
            }
            if (x == 2)
            {
                if (y == 1) return TileObjectData.Style2x1;
                if (y == 2) return TileObjectData.Style2x2;
                return TileObjectData.Style2xX;
            }
            if (x == 3)
            {
                if (y == 2) return TileObjectData.Style3x2;
                if (y == 3) return TileObjectData.Style3x3;
                if (y == 4) return TileObjectData.Style3x4;
                return TileObjectData.Style3x3Wall;
            }
            if (x == 4) return TileObjectData.Style4x2;
            if (x == 5) return TileObjectData.Style5x4;
            if (x == 6) return TileObjectData.Style6x3;
            return null;
        }

        public bool Merge(int type, bool? value = null) => Main.tileMerge[Type][type] = Main.tileMerge[type][Type] = value == null ? !Main.tileMerge[type][Type] : (bool)value;

        public void Merge(int[] type) { for (int i = 0; i < type.Length; i++) Merge(type[i]); }
        
        public void Clone(int type)
        {
            TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[Type] = TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[type];
            TileID.Sets.AllowLightInWater[Type] = TileID.Sets.AllowLightInWater[type];
            TileID.Sets.AllowsSaveCompressionBatching[Type] = TileID.Sets.AllowsSaveCompressionBatching[type];
            TileID.Sets.AllTiles[Type] = TileID.Sets.AllTiles[type];
            TileID.Sets.AvoidedByNPCs[Type] = TileID.Sets.AvoidedByNPCs[type];
            TileID.Sets.BasicChest[Type] = TileID.Sets.BasicChest[type];
            TileID.Sets.BasicChestFake[Type] = TileID.Sets.BasicChestFake[type];
            TileID.Sets.BasicDresser[Type] = TileID.Sets.BasicDresser[type];
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = TileID.Sets.BlockMergesWithMergeAllBlock[type];
            TileID.Sets.BlocksStairs[Type] = TileID.Sets.BlocksStairs[type];
            TileID.Sets.BlocksStairsAbove[Type] = TileID.Sets.BlocksStairsAbove[type];
            TileID.Sets.BlocksWaterDrawingBehindSelf[Type] = TileID.Sets.BlocksWaterDrawingBehindSelf[type];
            TileID.Sets.Boulders[Type] = TileID.Sets.Boulders[type];
            TileID.Sets.BreakableWhenPlacing[Type] = TileID.Sets.BreakableWhenPlacing[type];
            TileID.Sets.CanBeClearedDuringGeneration[Type] = TileID.Sets.CanBeClearedDuringGeneration[type];
            TileID.Sets.CanBeClearedDuringOreRunner[Type] = TileID.Sets.CanBeClearedDuringOreRunner[type];
            TileID.Sets.CanBeDugByShovel[Type] = TileID.Sets.CanBeDugByShovel[type];
            TileID.Sets.CanBeSatOnForNPCs[Type] = TileID.Sets.CanBeSatOnForNPCs[type];
            TileID.Sets.CanBeSatOnForPlayers[Type] = TileID.Sets.CanBeSatOnForPlayers[type];
            TileID.Sets.CanBeSleptIn[Type] = TileID.Sets.CanBeSleptIn[type];
            TileID.Sets.CanDropFromRightClick[Type] = TileID.Sets.CanDropFromRightClick[type];
            TileID.Sets.ChecksForMerge[Type] = TileID.Sets.ChecksForMerge[type];
            TileID.Sets.Clock[Type] = TileID.Sets.Clock[type];
            TileID.Sets.Clouds[Type] = TileID.Sets.Clouds[type];
            TileID.Sets.CommonSapling[Type] = TileID.Sets.CommonSapling[type];
            TileID.Sets.ConveyorDirection[Type] = TileID.Sets.ConveyorDirection[type];
            TileID.Sets.Corrupt[Type] = TileID.Sets.Corrupt[type];
            TileID.Sets.CountsAsGemTree[Type] = TileID.Sets.CountsAsGemTree[type];
            TileID.Sets.CountsAsHoneySource[Type] = TileID.Sets.CountsAsHoneySource[type];
            TileID.Sets.CountsAsLavaSource[Type] = TileID.Sets.CountsAsLavaSource[type];
            TileID.Sets.CountsAsWaterSource[Type] = TileID.Sets.CountsAsWaterSource[type];
            TileID.Sets.CrackedBricks[Type] = TileID.Sets.CrackedBricks[type];
            TileID.Sets.Crimson[Type] = TileID.Sets.Crimson[type];
            TileID.Sets.DisableSmartCursor[Type] = TileID.Sets.DisableSmartCursor[type];
            TileID.Sets.DisableSmartInteract[Type] = TileID.Sets.DisableSmartInteract[type];
            TileID.Sets.DoesntGetReplacedWithTileReplacement[Type] = TileID.Sets.DoesntGetReplacedWithTileReplacement[type];
            TileID.Sets.DoesntPlaceWithTileReplacement[Type] = TileID.Sets.DoesntPlaceWithTileReplacement[type];
            TileID.Sets.DrawFlipMode[Type] = TileID.Sets.DrawFlipMode[type];
            TileID.Sets.DrawsWalls[Type] = TileID.Sets.DrawsWalls[type];
            TileID.Sets.DrawTileInSolidLayer[Type] = TileID.Sets.DrawTileInSolidLayer[type];
            TileID.Sets.Falling[Type] = TileID.Sets.Falling[type];
            TileID.Sets.ForcedDirtMerging[Type] = TileID.Sets.ForcedDirtMerging[type];
            TileID.Sets.FramesOnKillWall[Type] = TileID.Sets.FramesOnKillWall[type];
            TileID.Sets.FriendlyFairyCanLureTo[Type] = TileID.Sets.FriendlyFairyCanLureTo[type];
            TileID.Sets.GemsparkFramingTypes[Type] = TileID.Sets.GemsparkFramingTypes[type];
            TileID.Sets.GeneralPlacementTiles[Type] = TileID.Sets.GeneralPlacementTiles[type];
            TileID.Sets.GetsCheckedForLeaves[Type] = TileID.Sets.GetsCheckedForLeaves[type];
            TileID.Sets.GetsDestroyedForMeteors[Type] = TileID.Sets.GetsDestroyedForMeteors[type];
            TileID.Sets.Grass[Type] = TileID.Sets.Grass[type];
            TileID.Sets.GrassSpecial[Type] = TileID.Sets.GrassSpecial[type];
            TileID.Sets.Hallow[Type] = TileID.Sets.Hallow[type];
            TileID.Sets.HasOutlines[Type] = TileID.Sets.HasOutlines[type];
            TileID.Sets.HasSlopeFrames[Type] = TileID.Sets.HasSlopeFrames[type];
            TileID.Sets.HellSpecial[Type] = TileID.Sets.HellSpecial[type];
            TileID.Sets.HousingWalls[Type] = TileID.Sets.HousingWalls[type];
            TileID.Sets.Ices[Type] = TileID.Sets.Ices[type];
            TileID.Sets.IcesSlush[Type] = TileID.Sets.Ices[type];
            TileID.Sets.IcesSnow[Type] = TileID.Sets.IcesSnow[type];
            TileID.Sets.IgnoredByGrowingSaplings[Type] = TileID.Sets.IgnoredByGrowingSaplings[type];
            TileID.Sets.IgnoredByNpcStepUp[Type] = TileID.Sets.IgnoredByNpcStepUp[type];
            TileID.Sets.IgnoredInHouseScore[Type] = TileID.Sets.IgnoredInHouseScore[type];
            TileID.Sets.IgnoreSmartCursorPriorityAxe[Type] = TileID.Sets.IgnoreSmartCursorPriorityAxe[type];
            TileID.Sets.IgnoresNearbyHalfbricksWhenDrawn[Type] = TileID.Sets.IgnoresNearbyHalfbricksWhenDrawn[type];
            TileID.Sets.IgnoresTileReplacementDropCheckWhenBeingPlaced[Type] = TileID.Sets.IgnoresTileReplacementDropCheckWhenBeingPlaced[type];
            TileID.Sets.InteractibleByNPCs[Type] = TileID.Sets.InteractibleByNPCs[type];
            TileID.Sets.IsAContainer[Type] = TileID.Sets.IsAContainer[type];
            TileID.Sets.IsAMechanism[Type] = TileID.Sets.IsAMechanism[type];
            TileID.Sets.IsATreeTrunk[Type] = TileID.Sets.IsATreeTrunk[type];
            TileID.Sets.IsATrigger[Type] = TileID.Sets.IsATrigger[type];
            TileID.Sets.IsBeam[Type] = TileID.Sets.IsBeam[type];
            TileID.Sets.isDesertBiomeSand[Type] = TileID.Sets.isDesertBiomeSand[type];
            TileID.Sets.IsShakeable[Type] = TileID.Sets.IsShakeable[type];
            TileID.Sets.IsSkippedForNPCSpawningGroundTypeCheck[Type] = TileID.Sets.IsSkippedForNPCSpawningGroundTypeCheck[type];
            TileID.Sets.IsValidSpawnPoint[Type] = TileID.Sets.IsValidSpawnPoint[type];
            TileID.Sets.IsVine[Type] = TileID.Sets.IsVine[type];
            TileID.Sets.JungleSpecial[Type] = TileID.Sets.JungleSpecial[type];
            TileID.Sets.Leaves[Type] = TileID.Sets.Leaves[type];
            TileID.Sets.MergesWithClouds[Type] = TileID.Sets.MergesWithClouds[type];
            TileID.Sets.Mud[Type] = TileID.Sets.Mud[type];
            TileID.Sets.NeedsGrassFraming[Type] = TileID.Sets.NeedsGrassFraming[type];
            TileID.Sets.NeedsGrassFramingDirt[Type] = TileID.Sets.NeedsGrassFramingDirt[type];
            TileID.Sets.NonSolidSaveSlopes[Type] = TileID.Sets.NonSolidSaveSlopes[type];
            TileID.Sets.NotReallySolid[Type] = TileID.Sets.NotReallySolid[type];
            TileID.Sets.Ore[Type] = TileID.Sets.Ore[type];
            TileID.Sets.OreMergesWithMud[Type] = TileID.Sets.OreMergesWithMud[type];
            TileID.Sets.Paintings[Type] = TileID.Sets.Paintings[type];
            TileID.Sets.Platforms[Type] = TileID.Sets.Platforms[type];
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = TileID.Sets.PreventsTileRemovalIfOnTopOfIt[type];
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = TileID.Sets.PreventsTileReplaceIfOnTopOfIt[type];
            TileID.Sets.ReplaceTileBreakDown[Type] = TileID.Sets.ReplaceTileBreakDown[type];
            TileID.Sets.ReplaceTileBreakUp[Type] = TileID.Sets.ReplaceTileBreakUp[type];
            TileID.Sets.ResetsHalfBrickPlacementAttempt[Type] = TileID.Sets.ResetsHalfBrickPlacementAttempt[type];
            TileID.Sets.ReverseVineThreads[Type] = TileID.Sets.ReverseVineThreads[type];
            TileID.Sets.SlowlyDiesInWater[Type] = TileID.Sets.SlowlyDiesInWater[type];
            TileID.Sets.SmartCursorPickaxePriorityOverride[Type] = TileID.Sets.SmartCursorPickaxePriorityOverride[type];
            TileID.Sets.Snow[Type] = TileID.Sets.Snow[type];
            TileID.Sets.SpreadOverground[Type] = TileID.Sets.SpreadOverground[type];
            TileID.Sets.SpreadUnderground[Type] = TileID.Sets.SpreadUnderground[type];
            TileID.Sets.Stone[Type] = TileID.Sets.Stone[type];
            TileID.Sets.SwaysInWindBasic[Type] = TileID.Sets.SwaysInWindBasic[type];
            TileID.Sets.TeamTiles[Type] = TileID.Sets.TeamTiles[type];
            TileID.Sets.TileInteractRead[Type] = TileID.Sets.TileInteractRead[type];
            TileID.Sets.tileMossBrick[Type] = TileID.Sets.tileMossBrick[type];
            TileID.Sets.Torch[Type] = TileID.Sets.Torch[type];
            TileID.Sets.TouchDamageHot[Type] = TileID.Sets.TouchDamageHot[type];
            TileID.Sets.TouchDamageOther[Type] = TileID.Sets.TouchDamageOther[type];
            TileID.Sets.TouchDamageSands[Type] = TileID.Sets.TouchDamageSands[type];
            TileID.Sets.TouchDamageVines[Type] = TileID.Sets.TouchDamageVines[type];
            TileID.Sets.TreeSapling[Type] = TileID.Sets.TreeSapling[type];
            TileID.Sets.VineThreads[Type] = TileID.Sets.VineThreads[type];
            Main.tileAlch[Type] = Main.tileAlch[type];
            Main.tileAxe[Type] = Main.tileAxe[type];
            Main.tileBlendAll[Type] = Main.tileBlendAll[type];
            Main.tileBlockLight[Type] = Main.tileBlockLight[type];
            Main.tileBouncy[Type] = Main.tileBouncy[type];
            Main.tileBrick[Type] = Main.tileBrick[type];
            Main.tileContainer[Type] = Main.tileContainer[type];
            Main.tileCracked[Type] = Main.tileCracked[type];
            Main.tileCut[Type] = Main.tileCut[type];
            Main.tileDungeon[Type] = Main.tileDungeon[type];
            Main.tileFlame[Type] = Main.tileFlame[type];
            Main.tileFrame[Type] = Main.tileFrame[type];
            Main.tileFrameCounter[Type] = Main.tileFrameCounter[type];
            Main.tileFrameImportant[Type] = Main.tileFrameImportant[type];
            Main.tileHammer[Type] = Main.tileHammer[type];
            Main.tileLargeFrames[Type] = Main.tileLargeFrames[type];
            Main.tileLavaDeath[Type] = Main.tileLavaDeath[type];
            Main.tileLighted[Type] = Main.tileLighted[type];
            Main.tileMerge[Type] = Main.tileMerge[type];
            Main.tileMergeDirt[Type] = Main.tileMergeDirt[type];
            Main.tileMoss[Type] = Main.tileMoss[type];
            Main.tileNoAttach[Type] = Main.tileNoAttach[type];
            Main.tileNoFail[Type] = Main.tileNoFail[type];
            Main.tileNoSunLight[Type] = Main.tileNoSunLight[type];
            Main.tileObsidianKill[Type] = Main.tileObsidianKill[type];
            Main.tileOreFinderPriority[Type] = Main.tileOreFinderPriority[type];
            Main.tilePile[Type] = Main.tilePile[type];
            Main.tileRope[Type] = Main.tileRope[type];
            Main.tileSand[Type] = Main.tileSand[type];
            Main.tileShine[Type] = Main.tileShine[type];
            Main.tileShine2[Type] = Main.tileShine2[type];
            Main.tileSign[Type] = Main.tileSign[type];
            Main.tileSolid[Type] = Main.tileSolid[type];
            Main.tileSolidTop[Type] = Main.tileSolidTop[type];
            Main.tileSpelunker[Type] = Main.tileSpelunker[type];
            Main.tileStone[Type] = Main.tileStone[type];
            Main.tileTable[Type] = Main.tileTable[type];
            Main.tileWaterDeath[Type] = Main.tileWaterDeath[type];
            for (int i = 0; i < Main.tileMerge[Type].Length; i++) Main.tileMerge[i][Type] = Main.tileMerge[Type][i];
        }
    }
}