using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Capture;
using Microsoft.Xna.Framework;
using CNC.Common.Utils;
using CNC.Common.Localization;
using Terraria.IO;
using Terraria.WorldBuilding;
using System.Collections.Generic;

namespace CNC.Content.Biomes
{
    public class Tranquility : ModBiome
    {
        protected static string key => "CNC/Assets/Backgrounds/Tranquility";
        //public override bool IsPrimaryBiome => true;
        //public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle");
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CNC/SurfaceBackground");
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("CNC/UndergroundBackground");
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/Tranquility");
        public override string BestiaryIcon => "CNC/Assets/Bestiary/Tranquility_Icon";
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(CNCLocalization.GetText("BiomeName.Tranquility", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText("BiomeName.Tranquility", 7));
        }
        
        public override bool IsBiomeActive(Player player)
        {
            bool b1 = ModContent.GetInstance<TileCount>().blockCount >= 40;
            bool b2 = player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight || player.ZoneOverworldHeight;
            bool res = player.GetModPlayer<Common.Global.Players.AllPlayer>().zone[0] = b1 && b2;
            CNC.SetSky("Tranquility", res);
            return res;
        }

        class TileCount : ModSystem
        {
            public int blockCount;

            public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
            {
                int[] count = {
                    tileCounts[CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGrass]],
                    tileCounts[CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityStone]],
                };
                blockCount = count[0] + count[1];
            }
        }

        class SurfaceBackground : ModSurfaceBackgroundStyle
        {
            public override void ModifyFarFades(float[] fades, float transitionSpeed)
            {
                for (int i = 0; i < fades.Length; i++)
                {
                    if (i == Slot) fades[i] += transitionSpeed;
                    else fades[i] -= transitionSpeed;
                    if (fades[i] > 1f) fades[i] = 1f;
                    if (fades[i] < 0f) fades[i] = 0f;
                }
            }

            public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot(key + "SurfaceFar");
            public override int ChooseMiddleTexture() => BackgroundTextureLoader.GetBackgroundSlot(key + "SurfaceMiddle");
            public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) => BackgroundTextureLoader.GetBackgroundSlot(key + "SurfaceClose");
        }

        class UndergroundBackground : ModUndergroundBackgroundStyle
        {
            public override void FillTextureArray(int[] textureSlots)
            {
                textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(key + "DirtLine");
                textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(key + "DirtLayer");
                textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(key + "RockLine");
                textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(key + "RockLayer");
            }
        }

        class System : ModSystem
        {
            public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
            {
                int idx = tasks.FindIndex(genpass => genpass.Name == "Tile Cleanup");
                tasks.Insert(idx, new WorldGenPass("CNC: Zone Tranquility", 1896.76f));
            }
        }

        class WorldGenPass : GenPass
        {
            public WorldGenPass(string name, float loadWeight) : base(name, loadWeight) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = CNCLocalization.GetText("WorldGen.Pass.Tranquility");
                int xMax = Main.maxTilesX;
                int yMax = Main.maxTilesY;
                Point mainPos = new Point(Main.spawnTileX, Main.spawnTileY);
                ushort[] type =
                {
                    (ushort)CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityDirt],
                    (ushort)CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityStone],
                    (ushort)CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGrass],
                    (ushort)CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityPlants1],
                    (ushort)CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityPlants2],
                    (ushort)CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityVines],
                };
                int dir = Main.spawnTileX < Main.dungeonX ? 1 : -1;
                int beginX = mainPos.X - xMax / 15;
                int endX = mainPos.X + xMax / 15;
                int beginY = mainPos.Y - yMax / 8;
                int endY = mainPos.Y + yMax / 3;
                for (int i = beginX; i < endX; i++)
                {
                    for (int j = beginY; j < endY; j++)
                    {
                        progress.Set((i - beginX + 1) * (j - beginY + 1) / (endX - beginX) / (endY - beginY));
                        ushort tile = Framing.GetTileSafely(i, j).TileType;
                        int distX = Math.Abs(i - mainPos.X);
                        int distY = Math.Abs(j - mainPos.Y);
                        if (tile >= 0 && tile <= 2) tile = type[tile];
                        if (tile == 3) tile = type[3];
                        if (tile == 73) tile = type[4];
                        if (tile == 52) tile = type[5];
                        Main.tile[i, j].TileType = tile;
                    }
                }
            }
        }
    }
}