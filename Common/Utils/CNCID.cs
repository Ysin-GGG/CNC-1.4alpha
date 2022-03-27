using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

using static Terraria.ModLoader.ModContent;

namespace CNC.Common.Utils
{
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

        public static class Colors
        {
            public static Color Expert => Main.DiscoColor;
            public static Color Master => new Color((byte)((double)byte.MaxValue * ((float)Main.mouseTextColor / byte.MaxValue)), (byte)(Main.masterColor * 200.0 * ((float)Main.mouseTextColor / byte.MaxValue)), 0, Main.mouseTextColor);
            public static Color Barbarian => CNCUtils.AccurateColor(168, 61, 51, 255, false);
            public static Color Defender => CNCUtils.AccurateColor(46, 109, 153, 255, false);
            public static Color Crusader => CNCUtils.AccurateColor(219, 173, 89, 255, false);
            public static Color Ranger => CNCUtils.AccurateColor(57, 153, 46, 255, false);
            public static Color Gunman => CNCUtils.AccurateColor(126, 229, 218, 255, false);
            public static Color Thrower => CNCUtils.AccurateColor(109, 81, 57, 255, false);
            public static Color Musketeer => CNCUtils.AccurateColor(46, 109, 153, 255, false);
            public static Color Cleric => CNCUtils.AccurateColor(232, 232, 11, 255, false);
            public static Color Wizard => CNCUtils.AccurateColor(112, 45, 179, 255, false);
            public static Color SpellBreaker => CNCUtils.AccurateColor(126, 161, 229, 255, false);
            public static Color Druid => CNCUtils.AccurateColor(188, 242, 170, 255, false);
            public static Color Shaman => CNCUtils.AccurateColor(122, 36, 70, 255, false);
            public static Color Warlock => CNCUtils.AccurateColor(76, 46, 77, 255, false);
            public static Color Monk => CNCUtils.AccurateColor(255, 255, 255, 255, false);
            public static Color Assassin => CNCUtils.AccurateColor(31, 32, 38, 255, false);
            public static Color Generic => CNCUtils.AccurateColor(0, 0, 0, 0, true);
            public static Color Melee => CNCUtils.AccurateColor(217, 60, 60, 0, true);
            public static Color Ranged => CNCUtils.AccurateColor(67, 230, 57, 0, true);
            public static Color Magic => CNCUtils.AccurateColor(53, 157, 255, 0, true);
            public static Color Summon => CNCUtils.AccurateColor(224, 44, 230, 0, true);
            public static Color Throwing => CNCUtils.AccurateColor(255, 255, 255, 0, true);
            //public static Color Generic => CNCUtils.AccurateColor(255, 196, 85, 0, true);
            //public static Color Generic => CNCUtils.AccurateColor(179, 175, 50, 0, true);
            //public static Color Generic => CNCUtils.AccurateColor(139, 29, 242, 0, true);
            public static Color GenericCrit => CNCUtils.AccurateColor(0, 0, 0, 0, true);
            public static Color MeleeCrit => CNCUtils.AccurateColor(163, 29, 29, 0, true);
            public static Color RangedCrit => CNCUtils.AccurateColor(31, 171, 27, 0, true);
            public static Color MagicCrit => CNCUtils.AccurateColor(61, 128, 192, 0, true);
            public static Color SummonCrit => CNCUtils.AccurateColor(224, 44, 230, 0, true);
            public static Color ThrowingCrit => CNCUtils.AccurateColor(255, 255, 255, 0, true);
            //public static Color GenericCrit => CNCUtils.AccurateColor(255, 196, 85, 0, true);
            //public static Color GenericCrit => CNCUtils.AccurateColor(179, 175, 50, 0, true);
            //public static Color GenericCrit => CNCUtils.AccurateColor(103, 43, 159, 0, true);
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
            public const short MaxObject = 19;
            #region ID
            public const short MiracleGem = 1;
            public const short DestineScroll = 2;
            public const short FireGrimoire = 3;
            public const short IceTooth = 4;
            public const short Superconductor = 5;
            public const short ArgenteusClaw = 6;
            public const short SpiritWood = 7;
            public const short TranquilityGem = 8;
            public const short SpiritSapling = 9;
            public const short TranquilityDirt = 10;
            public const short TranquilityStone = 11;
            public const short ArgenteusTree = 12;
            public const short TheLightOfAuzzsia = 13;
            public const short EmberOfNien = 14;
            public const short FragmentarySoulDagger = 15;
            public const short SpiritIronGreatsword = 16;
            public const short MysticalLight = 17;
            public const short ShortDagger = 18;
            public const short SpiritSparkingWand = 19;
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
                { "MiracleGem", MiracleGem },
                { "DestineScroll", DestineScroll },
                { "FireGrimoire", FireGrimoire },
                { "IceTooth", IceTooth },
                { "Superconductor", Superconductor },
                { "ArgenteusClaw", ArgenteusClaw },
                { "SpiritWood", SpiritWood },
                { "TranquilityGem", TranquilityGem },
                { "SpiritSapling", SpiritSapling },
                { "TranquilityDirt", TranquilityDirt },
                { "TranquilityStone", TranquilityStone },
                { "ArgenteusTree", ArgenteusTree },
                { "TheLightOfAuzzsia", TheLightOfAuzzsia },
                { "EmberOfNien", EmberOfNien },
                { "FragmentarySoulDagger", FragmentarySoulDagger },
                { "SpiritIronGreatsword", SpiritIronGreatsword },
                { "MysticalLight", MysticalLight },
                { "ShortDagger", ShortDagger },
                { "SpiritSparkingWand", SpiritSparkingWand },
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
                { "MiracleGem", LegacyObject[Collection["MiracleGem"]] },
                { "DestineScroll", LegacyObject[Collection["DestineScroll"]] },
                { "FireGrimoire", LegacyObject[Collection["FireGrimoire"]] },
                { "IceTooth", LegacyObject[Collection["IceTooth"]] },
                { "Superconductor", LegacyObject[Collection["Superconductor"]] },
                { "ArgenteusClaw", LegacyObject[Collection["ArgenteusClaw"]] },
                { "SpiritWood", LegacyObject[Collection["SpiritWood"]] },
                { "TranquilityGem", LegacyObject[Collection["TranquilityGem"]] },
                { "SpiritSapling", LegacyObject[Collection["SpiritSapling"]] },
                { "TranquilityDirt", LegacyObject[Collection["TranquilityDirt"]] },
                { "TranquilityStone", LegacyObject[Collection["TranquilityStone"]] },
                { "ArgenteusTree", LegacyObject[Collection["ArgenteusTree"]] },
                { "TheLightOfAuzzsia", LegacyObject[Collection["TheLightOfAuzzsia"]] },
                { "EmberOfNien", LegacyObject[Collection["EmberOfNien"]] },
                { "FragmentarySoulDagger", LegacyObject[Collection["FragmentarySoulDagger"]] },
                { "SpiritIronGreatsword", LegacyObject[Collection["SpiritIronGreatsword"]] },
                { "MysticalLight", LegacyObject[Collection["MysticalLight"]] },
                { "ShortDagger", LegacyObject[Collection["ShortDagger"]] },
                { "SpiritSparkingWand", LegacyObject[Collection["SpiritSparkingWand"]] },
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {

                { MiracleGem, ItemType<Content.Items.MiracleGem>() },
                { DestineScroll, ItemType<Content.Items.DestineScroll>() },
                { FireGrimoire, ItemType<Content.Items.Weapons.Magic.FireGrimoire>() },
                { IceTooth, ItemType<Content.Items.Weapons.Magic.IceTooth>() },
                //{ Superconductor, ItemType<Content.Items.Weapons.Magic.Superconductor>() },
                { ArgenteusClaw, ItemType<Content.Items.Weapons.Ranged.ArgenteusClaw>() },
                { SpiritWood, ItemType<Content.Items.SpiritWood>() },
                { TranquilityGem, ItemType<Content.Items.TranquilityGem>() },
                { SpiritSapling, ItemType<Content.Items.SpiritSapling>() },
                { TranquilityDirt, ItemType<Content.Items.TranquilityDirt>() },
                { TranquilityStone, ItemType<Content.Items.TranquilityStone>() },
                //{ ArgenteusTree, ItemType<Content.Items.>() },
                //{ TheLightOfAuzzsia, ItemType<Content.Items.>() },
                //{ EmberOfNien, ItemType<Content.Items.>() },
                //{ FragmentarySoulDagger, ItemType<Content.Items.>() },
                //{ SpiritIronGreatsword, ItemType<Content.Items.>() },
                //{ MysticalLight, ItemType<Content.Items.>() },
                //{ ShortDagger, ItemType<Content.Items.>() },
                //{ SpiritSparkingWand, ItemType<Content.Items.>() },
            };

            public static Dictionary<int, short> LegacyObjectReturn => new Dictionary<int, short>()
            {

                { LegacyObject[MiracleGem], MiracleGem },
                { LegacyObject[DestineScroll], DestineScroll },
                { LegacyObject[FireGrimoire], FireGrimoire },
                { LegacyObject[IceTooth], IceTooth },
                //{ LegacyObject[Superconductor], Superconductor },
                { LegacyObject[ArgenteusClaw], ArgenteusClaw },
                { LegacyObject[SpiritWood], SpiritWood },
                { LegacyObject[TranquilityGem], TranquilityGem },
                { LegacyObject[SpiritSapling], SpiritSapling },
                { LegacyObject[TranquilityDirt], TranquilityDirt },
                { LegacyObject[TranquilityStone], TranquilityStone },
                //{ LegacyObject[ArgenteusTree], ArgenteusTree },
                //{ LegacyObject[TheLightOfAuzzsia], TheLightOfAuzzsia },
                //{ LegacyObject[EmberOfNien], EmberOfNien },
                //{ LegacyObject[FragmentarySoulDagger], FragmentarySoulDagger },
                //{ LegacyObject[SpiritIronGreatsword], SpiritIronGreatsword },
                //{ LegacyObject[MysticalLight], MysticalLight },
                //{ LegacyObject[ShortDagger], ShortDagger },
                //{ LegacyObject[SpiritSparkingWand], SpiritSparkingWand },
            };
        }

        public static class Inscription
        {
            public const short MaxObject = 26 + 34;
            #region ID
            public static Content.Inscription Power => new Content.Inscription(0, 0);
            public static Content.Inscription Chanter => new Content.Inscription(1, 0);
            public static Content.Inscription Economization => new Content.Inscription(2, 0);
            public static Content.Inscription Arcana => new Content.Inscription(3, 0);
            public static Content.Inscription Charging => new Content.Inscription(4, 0);
            public static Content.Inscription Reiki => new Content.Inscription(5, 0);
            public static Content.Inscription Assimilation => new Content.Inscription(6, 0);
            public static Content.Inscription MagicPower => new Content.Inscription(7, 0);
            public static Content.Inscription MagicAdept => new Content.Inscription(8, 0);
            public static Content.Inscription Saint => new Content.Inscription(9, 0);
            public static Content.Inscription Evangel => new Content.Inscription(10, 0);
            public static Content.Inscription Destroyer => new Content.Inscription(11, 0);
            public static Content.Inscription Dexterity => new Content.Inscription(12, 0);
            public static Content.Inscription MeleeAdept => new Content.Inscription(13, 0);
            public static Content.Inscription Indignation => new Content.Inscription(14, 0);
            public static Content.Inscription Force => new Content.Inscription(15, 0);
            public static Content.Inscription Smiting => new Content.Inscription(16, 0);
            public static Content.Inscription Shockwave => new Content.Inscription(17, 0);
            public static Content.Inscription RaidAdept => new Content.Inscription(18, 0);
            public static Content.Inscription Murder => new Content.Inscription(19, 0);
            public static Content.Inscription Torture => new Content.Inscription(20, 0);
            public static Content.Inscription Assassination => new Content.Inscription(21, 0);
            public static Content.Inscription EagleEye => new Content.Inscription(22, 0);
            public static Content.Inscription Penetration => new Content.Inscription(23, 0);
            public static Content.Inscription EnchantedMagazine => new Content.Inscription(24, 0);
            public static Content.Inscription EnchantedBowstring => new Content.Inscription(25, 0);

            public static Content.Inscription S_Fireball => new Content.Inscription(0, CNCID.Item.FireGrimoire);
            public static Content.Inscription S_Incinerating => new Content.Inscription(1, CNCID.Item.FireGrimoire);
            public static Content.Inscription S_Scorching => new Content.Inscription(2, CNCID.Item.FireGrimoire);
            public static Content.Inscription S_Enthusiasm => new Content.Inscription(3, CNCID.Item.FireGrimoire);
            public static Content.Inscription S_Gelidity => new Content.Inscription(0, CNCID.Item.IceTooth);
            public static Content.Inscription S_Glacier => new Content.Inscription(1, CNCID.Item.IceTooth);
            public static Content.Inscription S_Iceflake => new Content.Inscription(2, CNCID.Item.IceTooth);
            public static Content.Inscription S_Shattering => new Content.Inscription(3, CNCID.Item.IceTooth);
            public static Content.Inscription S_FrostWave => new Content.Inscription(4, CNCID.Item.IceTooth);
            public static Content.Inscription S_Charging => new Content.Inscription(0, CNCID.Item.Superconductor);
            public static Content.Inscription S_SpellLightningChain => new Content.Inscription(1, CNCID.Item.Superconductor);
            public static Content.Inscription S_ArcaneOverload => new Content.Inscription(2, CNCID.Item.Superconductor);
            public static Content.Inscription S_SpellReconstruction => new Content.Inscription(3, CNCID.Item.Superconductor);
            public static Content.Inscription S_SpellAugmentated => new Content.Inscription(4, CNCID.Item.Superconductor);
            public static Content.Inscription S_StableSpell => new Content.Inscription(5, CNCID.Item.Superconductor);
            public static Content.Inscription S_SilveryArrow => new Content.Inscription(0, CNCID.Item.ArgenteusClaw);
            public static Content.Inscription S_Sanctity => new Content.Inscription(1, CNCID.Item.ArgenteusClaw);
            public static Content.Inscription S_Exorcism => new Content.Inscription(2, CNCID.Item.ArgenteusClaw);
            public static Content.Inscription S_SilveryFlame => new Content.Inscription(0, CNCID.Item.ArgenteusTree);
            public static Content.Inscription S_Judgement => new Content.Inscription(1, CNCID.Item.ArgenteusTree);
            public static Content.Inscription S_SilveryShield => new Content.Inscription(2, CNCID.Item.ArgenteusTree);
            public static Content.Inscription S_SilveryVow => new Content.Inscription(3, CNCID.Item.ArgenteusTree);
            public static Content.Inscription S_Rock => new Content.Inscription(0, CNCID.Item.TheLightOfAuzzsia);
            public static Content.Inscription S_ActiveChant => new Content.Inscription(1, CNCID.Item.TheLightOfAuzzsia);
            public static Content.Inscription S_Fragmentation => new Content.Inscription(2, CNCID.Item.TheLightOfAuzzsia);
            public static Content.Inscription S_DizzyStrike => new Content.Inscription(0, CNCID.Item.EmberOfNien);
            public static Content.Inscription S_EmberPrayer => new Content.Inscription(1, CNCID.Item.EmberOfNien);
            public static Content.Inscription S_EmberOath => new Content.Inscription(2, CNCID.Item.EmberOfNien);
            public static Content.Inscription S_TameHead => new Content.Inscription(0, CNCID.Item.FragmentarySoulDagger);
            public static Content.Inscription S_SoulScorching => new Content.Inscription(1, CNCID.Item.FragmentarySoulDagger);
            public static Content.Inscription S_IndulgentGhost => new Content.Inscription(2, CNCID.Item.FragmentarySoulDagger);
            public static Content.Inscription S_Decapitation => new Content.Inscription(0, CNCID.Item.SpiritIronGreatsword);
            public static Content.Inscription S_ConsciousMetal => new Content.Inscription(1, CNCID.Item.SpiritIronGreatsword);
            public static Content.Inscription S_BlessingOfDahl => new Content.Inscription(0, CNCID.Item.MysticalLight);
            #endregion
            public static Dictionary<Point, string> Collection => new Dictionary<Point, string>()
            {
                { new Point(0, 0), "Power" },
                { new Point(1, 0), "Chanter" },
                { new Point(2, 0), "Economization" },
                { new Point(3, 0), "Arcana" },
                { new Point(4, 0), "Charging" },
                { new Point(5, 0), "Reiki" },
                { new Point(6, 0), "Assimilation" },
                { new Point(7, 0), "MagicPower" },
                { new Point(8, 0), "MagicAdept" },
                { new Point(9, 0), "Saint" },
                { new Point(10, 0), "Evangel" },
                { new Point(11, 0), "Destroyer" },
                { new Point(12, 0), "Dexterity" },
                { new Point(13, 0), "MeleeAdept" },
                { new Point(14, 0), "Indignation" },
                { new Point(15, 0), "Force" },
                { new Point(16, 0), "Smiting" },
                { new Point(17, 0), "Shockwave" },
                { new Point(18, 0), "RaidAdept" },
                { new Point(19, 0), "Murder" },
                { new Point(20, 0), "Torture" },
                { new Point(21, 0), "Assassination" },
                { new Point(22, 0), "EagleEye" },
                { new Point(23, 0), "Penetration" },
                { new Point(24, 0), "EnchantedMagazine" },
                { new Point(25, 0), "EnchantedBowstring" },
                { new Point(0, CNCID.Item.FireGrimoire), "S_Fireball" },
                { new Point(1, CNCID.Item.FireGrimoire), "S_Incinerating" },
                { new Point(2, CNCID.Item.FireGrimoire), "S_Scorching" },
                { new Point(3, CNCID.Item.FireGrimoire), "S_Enthusiasm" },
                { new Point(0, CNCID.Item.IceTooth), "S_Gelidity" },
                { new Point(1, CNCID.Item.IceTooth), "S_Glacier" },
                { new Point(2, CNCID.Item.IceTooth), "S_Iceflake" },
                { new Point(3, CNCID.Item.IceTooth), "S_Shattering" },
                { new Point(4, CNCID.Item.IceTooth), "S_FrostWave" },
                { new Point(0, CNCID.Item.Superconductor), "S_Charging" },
                { new Point(1, CNCID.Item.Superconductor), "S_SpellLightningChain" },
                { new Point(2, CNCID.Item.Superconductor), "S_ArcaneOverload" },
                { new Point(3, CNCID.Item.Superconductor), "S_SpellReconstruction" },
                { new Point(4, CNCID.Item.Superconductor), "S_SpellAugmentated" },
                { new Point(5, CNCID.Item.Superconductor), "S_StableSpell" },
                { new Point(0, CNCID.Item.ArgenteusClaw), "S_SilveryArrow" },
                { new Point(1, CNCID.Item.ArgenteusClaw), "S_Sanctity" },
                { new Point(2, CNCID.Item.ArgenteusClaw), "S_Exorcism" },
                { new Point(0, CNCID.Item.ArgenteusTree), "S_SilveryFlame" },
                { new Point(1, CNCID.Item.ArgenteusTree), "S_Judgement" },
                { new Point(2, CNCID.Item.ArgenteusTree), "S_SilveryShield" },
                { new Point(3, CNCID.Item.ArgenteusTree), "S_SilveryVow" },
                { new Point(0, CNCID.Item.TheLightOfAuzzsia), "S_Rock" },
                { new Point(1, CNCID.Item.TheLightOfAuzzsia), "S_ActiveChant" },
                { new Point(2, CNCID.Item.TheLightOfAuzzsia), "S_Fragmentation" },
                { new Point(0, CNCID.Item.EmberOfNien), "S_DizzyStrike" },
                { new Point(1, CNCID.Item.EmberOfNien), "S_EmberPrayer" },
                { new Point(2, CNCID.Item.EmberOfNien), "S_EmberOath" },
                { new Point(0, CNCID.Item.FragmentarySoulDagger), "S_TameHead" },
                { new Point(1, CNCID.Item.FragmentarySoulDagger), "S_SoulScorching" },
                { new Point(2, CNCID.Item.FragmentarySoulDagger), "S_IndulgentGhost" },
            };

            public static Dictionary<string, Content.Inscription> LegacyCollection => new Dictionary<string, Content.Inscription>()
            {
                { "Power", Power },
                { "Chanter", Chanter },
                { "Economization", Economization },
                { "Arcana", Arcana },
                { "Charging", Charging },
                { "Reiki", Reiki },
                { "Assimilation", Assimilation },
                { "MagicPower", MagicPower },
                { "MagicAdept", MagicAdept },
                { "Saint", Saint },
                { "Evangel", Evangel },
                { "Destroyer", Destroyer },
                { "Dexterity", Dexterity },
                { "MeleeAdept", MeleeAdept },
                { "Indignation", Indignation },
                { "Force", Force },
                { "Smiting", Smiting },
                { "Shockwave", Shockwave },
                { "RaidAdept", RaidAdept },
                { "Murder", Murder },
                { "Torture", Torture },
                { "Assassination", Assassination },
                { "EagleEye", EagleEye },
                { "Penetration", Penetration },
                { "EnchantedMagazine", EnchantedMagazine },
                { "EnchantedBowstring", EnchantedBowstring },
                { "S_Fireball", S_Fireball },
                { "S_Incinerating", S_Incinerating },
                { "S_Scorching", S_Scorching },
                { "S_Enthusiasm", S_Enthusiasm },
                { "S_Gelidity", S_Gelidity },
                { "S_Glacier", S_Glacier },
                { "S_Iceflake", S_Iceflake },
                { "S_Shattering", S_Shattering },
                { "S_FrostWave", S_FrostWave },
                { "S_Charging", S_Charging },
                { "S_SpellLightningChain", S_SpellLightningChain },
                { "S_ArcaneOverload", S_ArcaneOverload },
                { "S_SpellReconstruction", S_SpellReconstruction },
                { "S_SpellAugmentated", S_SpellAugmentated },
                { "S_StableSpell", S_StableSpell },
                { "S_SilveryArrow", S_SilveryArrow },
                { "S_Sanctity", S_Sanctity },
                { "S_Exorcism", S_Exorcism },
                { "S_SilveryFlame", S_SilveryFlame },
                { "S_Judgement", S_Judgement },
                { "S_SilveryShield", S_SilveryShield },
                { "S_SilveryVow", S_SilveryVow },
                { "S_Rock", S_Rock },
                { "S_ActiveChant", S_ActiveChant },
                { "S_Fragmentation", S_Fragmentation },
                { "S_DizzyStrike", S_DizzyStrike },
                { "S_EmberPrayer", S_EmberPrayer },
                { "S_EmberOath", S_EmberOath },
                { "S_TameHead", S_TameHead },
                { "S_SoulScorching", S_SoulScorching },
                { "S_IndulgentGhost", S_IndulgentGhost },
            };

            public static Dictionary<Point, Content.Inscription> LegacyObject => new Dictionary<Point, Content.Inscription>()
            {
                { new Point(0, 0), Power },
                { new Point(1, 0), Chanter },
                { new Point(2, 0), Economization },
                { new Point(3, 0), Arcana },
                { new Point(4, 0), Charging },
                { new Point(5, 0), Reiki },
                { new Point(6, 0), Assimilation },
                { new Point(7, 0), MagicPower },
                { new Point(8, 0), MagicAdept },
                { new Point(9, 0), Saint },
                { new Point(10, 0), Evangel },
                { new Point(11, 0), Destroyer },
                { new Point(12, 0), Dexterity },
                { new Point(13, 0), MeleeAdept },
                { new Point(14, 0), Indignation },
                { new Point(15, 0), Force },
                { new Point(16, 0), Smiting },
                { new Point(17, 0), Shockwave },
                { new Point(18, 0), RaidAdept },
                { new Point(19, 0), Murder },
                { new Point(20, 0), Torture },
                { new Point(21, 0), Assassination },
                { new Point(22, 0), EagleEye },
                { new Point(23, 0), Penetration },
                { new Point(24, 0), EnchantedMagazine },
                { new Point(25, 0), EnchantedBowstring },
                { new Point(0, CNCID.Item.FireGrimoire), S_Fireball },
                { new Point(1, CNCID.Item.FireGrimoire), S_Incinerating },
                { new Point(2, CNCID.Item.FireGrimoire), S_Scorching },
                { new Point(3, CNCID.Item.FireGrimoire), S_Enthusiasm },
                { new Point(0, CNCID.Item.IceTooth), S_Gelidity },
                { new Point(1, CNCID.Item.IceTooth), S_Glacier },
                { new Point(2, CNCID.Item.IceTooth), S_Iceflake },
                { new Point(3, CNCID.Item.IceTooth), S_Shattering },
                { new Point(4, CNCID.Item.IceTooth), S_FrostWave },
                { new Point(0, CNCID.Item.Superconductor), S_Charging },
                { new Point(1, CNCID.Item.Superconductor), S_SpellLightningChain },
                { new Point(2, CNCID.Item.Superconductor), S_ArcaneOverload },
                { new Point(3, CNCID.Item.Superconductor), S_SpellReconstruction },
                { new Point(4, CNCID.Item.Superconductor), S_SpellAugmentated },
                { new Point(5, CNCID.Item.Superconductor), S_StableSpell },
                { new Point(0, CNCID.Item.ArgenteusClaw), S_SilveryArrow },
                { new Point(1, CNCID.Item.ArgenteusClaw), S_Sanctity },
                { new Point(2, CNCID.Item.ArgenteusClaw), S_Exorcism },
                { new Point(0, CNCID.Item.ArgenteusTree), S_SilveryFlame },
                { new Point(1, CNCID.Item.ArgenteusTree), S_Judgement },
                { new Point(2, CNCID.Item.ArgenteusTree), S_SilveryShield },
                { new Point(3, CNCID.Item.ArgenteusTree), S_SilveryVow },
                { new Point(0, CNCID.Item.TheLightOfAuzzsia), S_Rock },
                { new Point(1, CNCID.Item.TheLightOfAuzzsia), S_ActiveChant },
                { new Point(2, CNCID.Item.TheLightOfAuzzsia), S_Fragmentation },
                { new Point(0, CNCID.Item.EmberOfNien), S_DizzyStrike },
                { new Point(1, CNCID.Item.EmberOfNien), S_EmberPrayer },
                { new Point(2, CNCID.Item.EmberOfNien), S_EmberOath },
                { new Point(0, CNCID.Item.FragmentarySoulDagger), S_TameHead },
                { new Point(1, CNCID.Item.FragmentarySoulDagger), S_SoulScorching },
                { new Point(2, CNCID.Item.FragmentarySoulDagger), S_IndulgentGhost },
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