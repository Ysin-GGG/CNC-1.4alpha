using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;

using static Terraria.ModLoader.ModContent;

namespace CNC.Common.Utils
{
    public class MyMath
    {
        public const double E = 2.7182818284590451;
        public const double Pi = 3.1415926535897931;
        public const double Deg01 = Pi / 180;
        public const double Deg30 = Pi / 6;
        public const double Deg45 = Pi / 4;
        public const double Deg60 = Pi / 3;
        public const double Deg90 = Pi / 2;
        public const float e = 2.7182818f;
        public const float pi = 3.1415927f;
        public const float deg01 = pi / 180;
        public const float deg30 = pi / 6;
        public const float deg45 = pi / 4;
        public const float deg60 = pi / 3;
        public const float deg90 = pi / 2;
        public static float cos(double degree) => (float)Math.Cos(degree);
        public static float sin(double degree) => (float)Math.Sin(degree);
        public static float tan(double degree) => (float)Math.Tan(degree);
        public static float pow(double x, double a) => (float)Math.Pow(x, a);
        public static float GetZ(double x, double y) => (float)Math.Sqrt(x * x + y * y);
        public static Vector2 GetXY(double degree) => new Vector2(cos(degree), sin(degree));
    }

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
        public static Color ExpertColor => Main.DiscoColor;
        public static Color MasterColor => new Color((byte)((double)byte.MaxValue * ((float)Main.mouseTextColor / byte.MaxValue)), (byte)(Main.masterColor * 200.0 * ((float)Main.mouseTextColor / byte.MaxValue)), 0, Main.mouseTextColor);

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

    public static class CNCID
    {
        public static class Buff
        {
            public const short MaxObject = 8;
            #region ID
            public const short MiracleFox = 1;
            public const short Scorched = 2;
            public const short Enthused = 3;
            public const short Gelid = 4;
            public const short Glacier = 5;
            public const short Overloaded = 6;
            public const short Surged = 7;
            public const short Sanctified = 8;
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
                { "MiracleFox", MiracleFox },
                { "Scorched", Scorched },
                { "Enthused", Enthused },
                { "Gelid", Gelid },
                { "Glacier", Glacier },
                { "Overloaded", Overloaded },
                { "Surged", Surged },
                { "Sanctified", Sanctified },
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
                { "MiracleFox", LegacyObject[Collection["MiracleFox"]] },
                { "Scorched", LegacyObject[Collection["Scorched"]] },
                { "Enthused", LegacyObject[Collection["Enthused"]] },
                { "Gelid", LegacyObject[Collection["Gelid"]] },
                { "Glacier", LegacyObject[Collection["Glacier"]] },
                { "Overloaded", LegacyObject[Collection["Overloaded"]] },
                { "Surged", LegacyObject[Collection["Surged"]] },
                { "Sanctified", LegacyObject[Collection["Sanctified"]] },
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { MiracleFox, BuffType<Content.Mounts.MiracleFoxBuff>() },
                { Scorched, BuffType<Content.Buffs.Scorched>() },
                { Enthused, BuffType<Content.Buffs.Enthused>() },
                { Gelid, BuffType<Content.Buffs.Gelid>() },
                { Glacier, BuffType<Content.Buffs.Glacier>() },
                { Overloaded, BuffType<Content.Buffs.Scorched>() },
                { Surged, BuffType<Content.Buffs.Enthused>() },
                { Sanctified, BuffType<Content.Buffs.Gelid>() },
            };
        }

        public static class Clazz
        {
            public const byte MaxObject = 15;
            #region ID
            public const byte Barbarian = 11;//狂暴战士
            public const byte Defender = 12;//护卫
            public const byte Crusader = 13;//十字军
            public const byte Ranger = 21;//游侠
            public const byte Gunman = 22;//枪手
            public const byte Thrower = 23;//投手
            public const byte Musketeer = 24;//火枪手
            public const byte Cleric = 31;//牧师
            public const byte Wizard = 32;//巫师
            public const byte SpellBreaker = 33;//破法者
            public const byte Druid = 41;//德鲁伊
            public const byte Shaman = 42;//萨满
            public const byte Warlock = 43;//死灵法师
            public const byte Monk = 51;//武僧
            public const byte Assassin = 52;//刺杀者
            #endregion
            public static Dictionary<byte, string> Collection => new Dictionary<byte, string>()
            {
                { Barbarian, "Barbarian" },
                { Defender, "Defender" },
                { Crusader, "Crusader" },
                { Ranger, "Ranger" },
                { Gunman, "Gunman" },
                { Thrower, "Thrower" },
                { Musketeer, "Musketeer" },
                { Cleric, "Cleric" },
                { Wizard, "Wizard" },
                { SpellBreaker, "SpellBreaker" },
                { Druid, "Druid" },
                { Shaman, "Shaman" },
                { Warlock, "Warlock" },
                { Monk, "Monk" },
                { Assassin, "Assassin" },
            };

            public static Dictionary<byte, int> LegacyCollection => new Dictionary<byte, int>()
            {
                { Barbarian, 1 },
                { Defender, 2 },
                { Crusader, 3 },
                { Ranger, 4 },
                { Gunman, 5 },
                { Thrower, 6 },
                { Musketeer, 7 },
                { Cleric, 8 },
                { Wizard, 9 },
                { SpellBreaker, 10 },
                { Druid, 11 },
                { Shaman, 12 },
                { Warlock, 13 },
                { Monk, 14 },
                { Assassin, 15 },
            };
        }

        public static class State
        {
            public const byte MaxObject = 6;
            #region ID
            public const short Wrath = 1;
            public const short Cowardice = 2;
            public const short Blasphemy = 3;
            public const short Fluster = 4;
            public const short Restoring = 5;
            public const short Wraith = 6;
            #endregion

            public static Dictionary<short, string> Collection => new Dictionary<short, string>()
            {
                { Wrath, "Wrath" },
                { Cowardice, "Cowardice" },
                { Blasphemy, "Blasphemy" },
                { Fluster, "Fluster" },
                { Restoring, "Restoring" },
                { Wraith, "Wraith" },
            };
        }

        public static class Dust
        {
            public const short MaxObject = 0;
            #region ID
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
            };
        }

        public static class Item
        {
            public const short MaxObject = 11;
            #region ID
            public const short MiracleGem = 1;
            public const short DestineScroll = 2;
            public const short FireGrimoire = 3;
            public const short IceTooth = 4;
            public const short Superconductor = 5;
            public const short SilveryClaw = 6;
            public const short SpiritWood = 7;
            public const short TranquilityGem = 8;
            public const short SpiritSapling = 9;
            public const short TranquilityDirt = 10;
            public const short TranquilityStone = 11;
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
                { "MiracleGem", MiracleGem },
                { "DestineScroll", DestineScroll },
                { "FireGrimoire", FireGrimoire },
                { "IceTooth", IceTooth },
                { "Superconductor", Superconductor },
                { "SilveryClaw", SilveryClaw },
                { "SpiritWood", SpiritWood },
                { "TranquilityGem", TranquilityGem },
                { "SpiritSapling", SpiritSapling },
                { "TranquilityDirt", TranquilityDirt },
                { "TranquilityStone", TranquilityStone },
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
                { "MiracleGem", LegacyObject[Collection["MiracleGem"]] },
                { "DestineScroll", LegacyObject[Collection["DestineScroll"]] },
                { "FireGrimoire", LegacyObject[Collection["FireGrimoire"]] },
                { "IceTooth", LegacyObject[Collection["IceTooth"]] },
                { "Superconductor", LegacyObject[Collection["Superconductor"]] },
                { "SilveryClaw", LegacyObject[Collection["SilveryClaw"]] },
                { "SpiritWood", LegacyObject[Collection["SpiritWood"]] },
                { "TranquilityGem", LegacyObject[Collection["TranquilityGem"]] },
                { "SpiritSapling", LegacyObject[Collection["SpiritSapling"]] },
                { "TranquilityDirt", LegacyObject[Collection["TranquilityDirt"]] },
                { "TranquilityStone", LegacyObject[Collection["TranquilityStone"]] },
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {

                { MiracleGem, ItemType<Content.Items.MiracleGem>() },
                { DestineScroll, ItemType<Content.Items.DestineScroll>() },
                { FireGrimoire, ItemType<Content.Items.Weapons.Magic.FireGrimoire>() },
                { IceTooth, ItemType<Content.Items.Weapons.Magic.IceTooth>() },
                //{ Superconductor, ItemType<Content.Items.Weapons.Magic.Superconductor>() },
                { SilveryClaw, ItemType<Content.Items.Weapons.Ranged.SilveryClaw>() },
                { SpiritWood, ItemType<Content.Items.SpiritWood>() },
                { TranquilityGem, ItemType<Content.Items.TranquilityGem>() },
                { SpiritSapling, ItemType<Content.Items.SpiritSapling>() },
                { TranquilityDirt, ItemType<Content.Items.TranquilityDirt>() },
                { TranquilityStone, ItemType<Content.Items.TranquilityStone>() },
            };
        }

        public static class Inscription
        {
            public const short MaxObject = 0;
            #region ID
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
            };
        }

        public static class NPC
        {
            public const short MaxObject = 6;
            #region ID
            public const short Mentor = 1;
            public const short TranquilityPixie = 2;
            public const short PhantomBat = 3;
            public const short PhantomSlime = 4;
            public const short PhantomFlame = 5;
            public const short SpiritBoss = 6;
            public const short SpiritCrystal = 7;
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
                { "Mentor", Mentor },
                { "TranquilityPixie", TranquilityPixie },
                { "PhantomBat", PhantomBat },
                { "PhantomSlime", PhantomSlime },
                //{ "PhantomFlame", PhantomFlame },
                { "SpiritBoss", SpiritBoss },
                { "SpiritCrystal", SpiritCrystal },
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
                { "Mentor", LegacyObject[Collection["Mentor"]] },
                { "TranquilityPixie", LegacyObject[Collection["TranquilityPixie"]] },
                { "PhantomBat", LegacyObject[Collection["PhantomBat"]] },
                { "PhantomSlime", LegacyObject[Collection["PhantomSlime"]] },
                //{ "PhantomFlame", LegacyObject[Collection["PhantomFlame"]] },
                { "SpiritBoss", LegacyObject[Collection["SpiritBoss"]] },
                { "SpiritCrystal", LegacyObject[Collection["SpiritCrystal"]] },
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { Mentor, NPCType<Content.NPCs.TownNPCs.Mentor>() },
                { TranquilityPixie, NPCType<Content.NPCs.TranquilityPixie>() },
                { PhantomBat, NPCType<Content.NPCs.PhantomBat>() },
                { PhantomSlime, NPCType<Content.NPCs.PhantomSlime>() },
                //{ PhantomFlame, NPCType<Content.NPCs.PhantomFlame>() },
                { SpiritBoss, NPCType<Content.NPCs.SpiritBoss.SpiritBoss>() },
                { SpiritCrystal, NPCType<Content.NPCs.SpiritBoss.SpiritCrystal>() },
            };
        }

        public static class Projectile
        {
            public const short MaxObject = 5;
            #region ID
            public const short MentorMissile = 1;
            public const short SpiritMass = 2;
            public const short SpiritArrowV1 = 3;
            public const short SpiritArrowV2 = 4;
            public const short SpiritArrowV3 = 5;
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
                { "MentorMissile", MentorMissile },
                { "SpiritMass", SpiritMass },
                { "SpiritArrowV1", SpiritArrowV1 },
                { "SpiritArrowV2", SpiritArrowV2 },
                { "SpiritArrowV3", SpiritArrowV3 },
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
                { "MentorMissile", LegacyObject[Collection["MentorMissile"]] },
                { "SpiritMass", LegacyObject[Collection["SpiritMass"]] },
                { "SpiritArrowV1", LegacyObject[Collection["SpiritArrowV1"]] },
                { "SpiritArrowV2", LegacyObject[Collection["SpiritArrowV2"]] },
                { "SpiritArrowV3", LegacyObject[Collection["SpiritArrowV3"]] },
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { MentorMissile, ProjectileType<Content.Projectiles.MentorMissile>() },
                { SpiritMass, ProjectileType<Content.Projectiles.Hostile.SpiritMass>() },
                { SpiritArrowV1, ProjectileType<Content.Projectiles.Hostile.SpiritArrow.V1>() },
                { SpiritArrowV2, ProjectileType<Content.Projectiles.Hostile.SpiritArrow.V2>() },
                { SpiritArrowV3, ProjectileType<Content.Projectiles.Hostile.SpiritArrow.V3>() },
            };
        }

        public static class Tile
        {
            public const short MaxObject = 5;
            #region ID
            public const short TranquilityDirt = 1;
            public const short TranquilityStone = 2;
            public const short TranquilityGrass = 3;
            public const short SpiritWood = 4;
            public const short SpiritSapling = 5;
            public const short TranquilityGemSapling = 6;
            public const short TranquilityPlants1 = 7;
            public const short TranquilityPlants2 = 8;
            public const short TranquilityVines = 9;
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
                { "TranquilityDirt", TranquilityDirt },
                { "TranquilityStone", TranquilityStone },
                { "TranquilityGrass", TranquilityGrass },
                { "SpiritWood", SpiritWood },
                { "SpiritSapling", SpiritSapling },
                { "TranquilityGemSapling", TranquilityGemSapling },
                { "TranquilityPlants1", TranquilityPlants1 },
                { "TranquilityPlants2", TranquilityPlants2 },
                { "TranquilityVines", TranquilityVines },
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
                { "TranquilityDirt", LegacyObject[Collection["TranquilityDirt"]] },
                { "TranquilityStone", LegacyObject[Collection["TranquilityStone"]] },
                { "TranquilityGrass", LegacyObject[Collection["TranquilityGrass"]] },
                { "SpiritWood", LegacyObject[Collection["SpiritWood"]] },
                { "SpiritSapling", LegacyObject[Collection["SpiritSapling"]] },
                { "TranquilityGemSapling", LegacyObject[Collection["TranquilityGemSapling"]] },
                { "TranquilityPlants1", LegacyObject[Collection["TranquilityPlants1"]] },
                { "TranquilityPlants2", LegacyObject[Collection["TranquilityPlants2"]] },
                { "TranquilityVines", LegacyObject[Collection["TranquilityVines"]] },
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { TranquilityDirt, TileType<Content.Tiles.Tranquility.TranquilityDirt>() },
                { TranquilityStone, TileType<Content.Tiles.Tranquility.TranquilityStone>() },
                { TranquilityGrass, TileType<Content.Tiles.Tranquility.TranquilityGrass>() },
                { SpiritWood, TileType<Content.Tiles.Tranquility.SpiritWood>() },
                { SpiritSapling, TileType<Content.Tiles.Tranquility.SpiritSapling>() },
                //{ TranquilityGemSapling, TileType<Content.Tiles.Tranquility.TranquilityGemSapling>() },
                { TranquilityPlants1, TileType<Content.Tiles.Tranquility.TranquilityPlants.TranquilityPlants1>() },
                { TranquilityPlants2, TileType<Content.Tiles.Tranquility.TranquilityPlants.TranquilityPlants2>() },
                { TranquilityVines, TileType<Content.Tiles.Tranquility.TranquilityPlants.TranquilityVines>() },
            };
        }

        public static class Wall
        {
            public const short MaxObject = 0;
            #region ID
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
            };
        }
    }
}