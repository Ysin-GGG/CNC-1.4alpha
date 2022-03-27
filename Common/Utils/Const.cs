using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

using static Terraria.ModLoader.ModContent;

namespace CNC.Common.Utils
{
    public static class Const
    {
        public const int max1 = Main.maxNPCs;
        public const int max2 = Main.maxPlayers;
        public const int max3 = Main.maxProjectiles;
        public const int max4 = Main.maxBuffTypes;
        public const int max5 = Main.maxItemTypes;
        public static int me => Main.myPlayer;
        public static Player Me => Main.LocalPlayer;
        public static Vector2 ScreenSize => new Vector2(Main.screenWidth, Main.screenHeight);
        public static Vector2 _02 => new Vector2(0f, 0f);
        public static Vector3 _03 => new Vector3(0f, 0f, 0f);
        public static Vector4 _04 => new Vector4(0f, 0f, 0f, 0f);
        public static Vector2 _12 => new Vector2(1f, 1f);
        public static Vector3 _13 => new Vector3(1f, 1f, 1f);
        public static Vector4 _14 => new Vector4(1f, 1f, 1f, 1f);

        public static DynamicSpriteFont[] Font => new DynamicSpriteFont[]
        {
            FontAssets.CombatText[0].Value,
            FontAssets.CombatText[1].Value,
            FontAssets.DeathText.Value,
            FontAssets.ItemStack.Value,
            FontAssets.MouseText.Value,
        };

        public enum Direction { left = -1, right = 1 }

        /// <summary> 字体类型代号 </summary>
        public enum FontType
        {
            CombatText,
            CombatTextCrit,
            DeathText,
            ItemStack,
            MouseText
        }

        /// <summary> UI层代号 </summary>
        public enum UILayerID
        {
            InterfaceLogic1 = 0,
            EmoteBubbles = 1,
            SmartCursorTargets = 2,
            LaserRuler = 3,
            Ruler = 4,
            GamepadLockOn = 5,
            TileGridOption = 6,
            TownNPCHouseBanners = 7,
            HideUIToggle = 8,
            WireSelection = 9,
            CaptureManagerCheck = 10,
            IngameOptions = 11,
            FancyUI = 12,
            AchievementCompletePopups = 13,
            EntityHealthBars = 14,
            InvasionProgressBars = 15,
            MapOrMinimap = 16,
            DiagnoseNet = 17,
            DiagnoseVideo = 18,
            SignTileBubble = 19,
            MPPlayerNames = 20,
            HairWindow = 21,
            DresserWindow = 22,
            NPCorSignDialog = 23,
            InterfaceLogic2 = 24,
            ResourceBars = 25,
            InterfaceLogic3 = 26,
            Inventory = 27,
            InfoAccessoriesBar = 28,
            SettingsButton = 29,
            Hotbar = 30,
            BuilderAccessoriesBar = 31,
            RadialHotbars = 32,
            MouseText = 33,
            PlayerChat = 34,
            DeathText = 35,
            Cursor = 36,
            DebugStuff = 37,
            MouseItemOrNPCHead = 38,
            MouseOver = 39,
            InteractItemIcon = 40,
            InterfaceLogic4 = 41,
        }

        /// <summary> 世界生成层代号 </summary>
        public enum GenPassID_1351
        {
            /// <summary> 初始化 </summary> 
            Reset = 0,
            /// <summary> 创建地形/生成世界版图 </summary> 
            Terrain = 1,
            /// <summary> 生成隧道 </summary> 
            Tunnels = 2,
            /// <summary> 添加沙子 </summary> 
            Sand = 3,
            /// <summary> 生成小山 </summary> 
            MountCaves = 4,
            /// <summary> 将土置于土后 </summary> 
            DirtWallBackgrounds = 5,
            /// <summary> 将岩石置于土中 </summary> 
            RocksInDirt = 6,
            /// <summary> 将土放入岩石间 </summary> 
            DirtInRocks = 7,
            /// <summary> 添加粘土 </summary> 
            Clay = 8,
            /// <summary> 随机挖洞 </summary> 
            SmallHoles = 9,
            /// <summary> 生成小洞穴 </summary> 
            DirtLayerCaves = 10,
            /// <summary> 生成大洞穴 </summary> 
            RockLayerCaves = 11,
            /// <summary> 生成地表洞穴 </summary> 
            SurfaceCaves = 12,
            /// <summary> 添加雪 </summary> 
            SlushCheck = 13,
            /// <summary> 生成草地? </summary> 
            Grass = 14,
            /// <summary> 生成丛林 </summary> 
            Jungle = 15,
            /// <summary> 凿刻大理石 </summary> 
            Marble = 16,
            /// <summary> 生长花岗岩 </summary> 
            Granite = 17,
            /// <summary> 添加更多草地 </summary> 
            MudCavesToGrass1 = 18,
            /// <summary> 沙漠化 </summary> 
            FullDesert = 19,
            /// <summary> 生成漂浮岛 </summary> 
            FloatingIslands = 20,
            /// <summary> 添加蘑菇地块 </summary> 
            MushroomPatches = 21,
            /// <summary> 将泥置于土中 </summary> 
            MudToDirt = 22,
            /// <summary> 添加泥沙 </summary> 
            Silt = 23,
            /// <summary> 添加发光物品(矿石等) </summary> 
            Shinies = 24,
            /// <summary> 添加蛛丝 </summary> 
            Webs = 25,
            /// <summary> 创建地狱 </summary> 
            Underworld = 26,
            /// <summary> 添加水体 </summary> 
            Lakes = 27,
            /// <summary> 生成地牢? </summary> 
            Dungeon = 28,
            /// <summary> 世界邪恶化 </summary> 
            Corruption = 29,
            /// <summary> 生成雪泥? </summary> 
            Slush = 30,
            /// <summary> 正在生成山洞 </summary> 
            MudCavesToGrass2 = 31,
            /// <summary> 正在创建沙滩 </summary> 
            Beaches = 32,
            /// <summary> 正在添加宝石 </summary> 
            Gems = 33,
            /// <summary> 沙子正在沉淀 </summary> 
            GravitatingSand = 34,
            /// <summary> 清理土背景 </summary> 
            CleanUpDirt = 35,
            /// <summary> 生成金字塔 </summary> 
            Pyramids = 36,
            /// <summary>  </summary> 
            DirtRockWallRunner = 37,
            /// <summary> 生成生命树 </summary> 
            LivingTrees = 38,
            /// <summary> 生成生命树背景墙 </summary> 
            WoodTreeWalls = 39,
            /// <summary> 放置祭坛 </summary> 
            Altars = 40,
            /// <summary> 使丛林变得潮湿? </summary> 
            WetJungle = 41,
            /// <summary>  </summary> 
            RemoveWaterFromSand = 42,
            /// <summary> 创建丛林废墟 </summary> 
            JungleTemple = 43,
            /// <summary> 创建黄蜂巢 </summary> 
            Hives = 44,
            /// <summary> 生成丛林箱 </summary> 
            JungleChests = 45,
            /// <summary> 令世界平坦 </summary> 
            SmoothWorld = 46,
            /// <summary> 安置液体 </summary> 
            SettleLiquids = 47,
            /// <summary> 创建瀑布 </summary> 
            Waterfalls = 48,
            /// <summary> 生成冰块? </summary> 
            Ice = 49,
            /// <summary> 将墙切片 </summary> 
            WallVariety = 50,
            /// <summary> 放置机关 </summary> 
            Traps = 51,
            /// <summary> 放置生命水晶 </summary> 
            LifeCrystals = 52,
            /// <summary> 放置雕像 </summary> 
            Statues = 53,
            /// <summary> 隐藏宝藏 </summary> 
            BuriedChests = 54,
            /// <summary> 隐藏更多宝藏 </summary> 
            SurfaceChests = 55,
            /// <summary> 隐藏丛林宝藏 </summary> 
            JungleChestsPlacement = 56,
            /// <summary> 隐藏水下宝藏 </summary> 
            WaterChests = 57,
            /// <summary> 扩大蜘蛛洞 </summary> 
            SpiderCaves = 58,
            /// <summary>  </summary> 
            GemCaves = 59,
            /// <summary> 青苔化 </summary> 
            Moss1 = 60,
            /// <summary>  </summary> 
            Temple = 61,
            /// <summary> 建造洞壁 </summary> 
            IceWalls = 62,
            /// <summary> 生成地下生命树 </summary> 
            JungleTrees = 63,
            /// <summary> 生成漂浮岛房屋 </summary> 
            FloatingIslandHouses = 64,
            /// <summary>  </summary> 
            QuickCleanup = 65,
            /// <summary> 放置易碎物品(罐子) </summary> 
            Pots = 66,
            /// <summary> 放置地狱熔炉 </summary> 
            Hellforge = 67,
            /// <summary> 扩大草地 </summary> 
            SpreadingGrass = 68,
            /// <summary>  </summary> 
            Piles = 69,
            /// <summary> 种仙人掌 </summary> 
            Moss2 = 70,
            /// <summary> 设置重生点 </summary> 
            SpawnPoint = 71,
            /// <summary> 放置草墙? </summary> 
            GrassWall = 72,
            /// <summary> 生成基导 </summary> 
            Guide = 73,
            /// <summary> 种向日葵 </summary> 
            Sunflowers = 74,
            /// <summary> 种树 </summary> 
            PlantingTrees = 75,
            /// <summary> 种草药 </summary> 
            Herbs = 76,
            /// <summary> 种染料植物 </summary> 
            DyePlants = 77,
            /// <summary>  </summary> 
            WebsAndHoney = 78,
            /// <summary> 种草 </summary> 
            Weeds = 79,
            /// <summary>  </summary> 
            MudCavesToGrass3 = 80,
            /// <summary>  </summary> 
            JunglePlants = 81,
            /// <summary> 种藤蔓 </summary> 
            Vines = 82,
            /// <summary> 种花 </summary> 
            Flowers = 83,
            /// <summary> 种蘑菇 </summary> 
            Mushrooms = 84,
            /// <summary>  </summary> 
            Stalac = 85,
            /// <summary>  </summary> 
            GemsInIceBiome = 86,
            /// <summary>  </summary> 
            RandomGems = 87,
            /// <summary>  </summary> 
            MossGrass = 88,
            /// <summary>  </summary> 
            MudsWallsInJungle = 89,
            /// <summary>  </summary> 
            Larva = 90,
            /// <summary> 再次安置液体 </summary> 
            SettleLiquidsAgain = 91,
            /// <summary>  </summary> 
            TileCleanup = 92,
            /// <summary>  </summary> 
            LihzahrdAltars = 93,
            /// <summary> 生成建筑物 </summary> 
            MicroBiomes = 94,
            /// <summary>  </summary> 
            FinalCleanup = 95,
        };

        private static readonly Dictionary<string, float> GenPassID = new Dictionary<string, float>()
        {
            { "Reset", 0.9667f },
            { "Terrain", 507.352f },
            { "Dunes", 239.7913f },
            { "Ocean Sand", 10.4129f },
            { "Sand Patches", 452.6755f },
            { "Tunnels", 4.3622f },
            { "Mount Caves", 49.9993f },
            { "Dirt Wall Backgrounds", 328.7817f },
            { "Rocks In Dirt", 1537.466f },
            { "Dirt In Rocks", 1515.23f },
            { "Clay", 314.8327f },
            { "Small Holes", 2955.926f },
            { "Dirt Layer Caves", 238.2545f },
            { "Rock Layer Caves", 2708.396f },
            { "Surface Caves", 42.3857f },
            { "Generate Ice Biome", 100.005f },
            { "Grass", 29.7885f },
            { "Jungle", 11205.83f },
            { "Marble", 5358.884f },
            { "Granite", 2142.664f },
            { "Mud Caves To Grass", 3319.761f },
            { "Full Desert", 9730.408f },
            { "Floating Islands", 1364.346f },
            { "Mushroom Patches", 743.7686f },
            { "Dirt To Mud", 351.3519f },
            { "Silt", 211.84f },
            { "Shinies", 237.4298f },
            { "Webs", 50.6646f },
            { "Underworld", 8936.494f },
            { "Lakes", 12.1766f },
            { "Corruption", 1094.237f },
            { "Dungeon", 477.1963f },
            { "Slush", 55.1857f },
            { "Mountain Caves", 11.4819f },
            { "Beaches", 7.8287f },
            { "Gems", 895.426f },
            { "Gravitating Sand", 933.5295f },
            { "Clean Up Dirt", 697.0276f },
            { "Pyramids", 6.6884f },
            { "Dirt Rock Wall Runner", 24.7648f },
            { "Living Trees", 4.937f },
            { "Wood Tree Walls", 76.8709f },
            { "Altars", 72.6607f },
            { "Wet Jungle", 23.492f },
            { "Remove Water From Sand", 22.0898f },
            { "Jungle Temple", 595.8422f },
            { "Hives", 371.392f },
            { "Jungle Chests", 0.5896f },
            { "Smooth World", 5841.608f },
            { "Settle Liquids", 9398.525f },
            { "Waterfalls", 4118.666f },
            { "Ice", 163.0777f },
            { "Wall Variety", 5264.021f },
            { "Life Crystals", 2.7582f },
            { "Statues", 64.5737f },
            { "Buried Chests", 1102.553f },
            { "Surface Chests", 12.8337f },
            { "Jungle Chests Placement", 1.3546f },
            { "Water Chests", 12.5981f },
            { "Spider Caves", 475.4143f },
            { "Gem Caves", 36.0143f },
            { "Moss", 655.8314f },
            { "Temple", 5.6917f },
            { "Ice Walls", 957.0317f },
            { "Jungle Trees", 817.2459f },
            { "Floating Island Houses", 1.5022f },
            { "Quick Cleanup", 1374.467f },
            { "Pots", 1638.609f },
            { "Hellforge", 2.8645f },
            { "Spreading Grass", 127.7581f },
            { "Place Fallen Log", 17.3377f },
            { "Traps", 562.9085f },
            { "Piles", 288.3675f },
            { "Spawn Point", 0.012f },
            { "Grass Wall", 604.9992f },
            { "Guide", 0.016f },
            { "Sunflowers", 4.1757f },
            { "Planting Trees", 325.0993f },
            { "Cactus & Coral", 31.6349f },
            { "Herbs", 120.1871f },
            { "Dye Plants", 226.6394f },
            { "Webs And Honey", 608.9524f },
            { "Weeds", 187.9759f },
            { "Mud Caves To Grass 2", 686.4958f },
            { "Jungle Plants", 1295.038f },
            { "Vines", 1132.555f },
            { "Flowers", 16.7723f },
            { "Mushrooms", 0.2294f },
            { "Gems In Ice Biome", 10.3092f },
            { "Random Gems", 18.4925f },
            { "Moss Grass", 687.742f },
            { "Muds Walls In Jungle", 89.7739f },
            { "Larva", 0.2074f },
            { "Settle Liquids Again", 7073.647f },
            { "Tile Cleanup", 1896.76f },
            { "Lihzahrd Altars", 0.0071f },
            { "Micro Biomes", 3547.43f },
            { "Stalac", 1180.906f },
            { "Remove Broken Traps", 1293.425f },
            { "Final Cleanup", 2080.294f}
        };
    }
}