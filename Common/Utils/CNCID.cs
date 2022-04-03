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
            /// <summary> Áéºü </summary>
            public const short MiracleFox = 1;
            /// <summary> ×ÆÈÈ </summary>
            public const short Scorched = 2;
            /// <summary> ¼¤Çé </summary>
            public const short Enthused = 3;
            /// <summary> º®Ëª </summary>
            public const short Gelid = 4;
            /// <summary> ¼«º® </summary>
            public const short Glacier = 5;
            /// <summary> ³¬ÔØ </summary>
            public const short Overloaded = 6;
            /// <summary> ÄÜÁ¿Ó¿¶¯ </summary>
            public const short Surged = 7;
            /// <summary> Ê¥»¯ </summary>
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
            public const byte Barbarian = 11;//¿ñ±©Õ½Ê¿
            public const byte Defender = 12;//»¤ÎÀ
            public const byte Crusader = 13;//Ê®×Ö¾ü
            public const byte Ranger = 21;//ÓÎÏÀ
            public const byte Gunman = 22;//Ç¹ÊÖ
            public const byte Thrower = 23;//Í¶ÊÖ
            public const byte Musketeer = 24;//»ðÇ¹ÊÖ
            public const byte Cleric = 31;//ÄÁÊ¦
            public const byte Wizard = 32;//Î×Ê¦
            public const byte SpellBreaker = 33;//ÆÆ·¨Õß
            public const byte Druid = 41;//µÂÂ³ÒÁ
            public const byte Shaman = 42;//ÈøÂú
            public const byte Warlock = 43;//ËÀÁé·¨Ê¦
            public const byte Monk = 51;//ÎäÉ®
            public const byte Assassin = 52;//´ÌÉ±Õß
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
            /// <summary> ¾²Ú×±¦Ê¯ </summary>
            public const short MiracleGem = 1;
            /// <summary> Ãü¶¨¾íÖá </summary>
            public const short DestineScroll = 2;
            /// <summary> »ðÑæÄ§µä </summary>
            public const short FireGrimoire = 3;
            /// <summary> º®Ëª¼âÑÀ </summary>
            public const short IceTooth = 4;
            /// <summary> ½ðÊô·¨ÕÈ </summary>
            public const short Superconductor = 5;
            /// <summary> °¬Òòç¾°ÂÊ¥¹­ </summary>
            public const short ArgenteusClaw = 6;
            /// <summary> ÁéÄ¾ </summary>
            public const short SpiritWood = 7;
            /// <summary> ¾²Ú×ÄÜÁ¿Ê¯ </summary>
            public const short TranquilityGem = 8;
            /// <summary> ¾²Ú×Ê÷Ãç </summary>
            public const short SpiritSapling = 9;
            /// <summary> »ÒÄà </summary>
            public const short TranquilityDirt = 10;
            /// <summary> »ÒÑÒ </summary>
            public const short TranquilityStone = 11;
            /// <summary> °¬Òòç¾°ÂÊÄ½£ </summary>
            public const short ArgenteusOathKeeper = 12;
            /// <summary> °£×ÈÎ÷ÑÇÖ®¹â </summary>
            public const short TheLightOfAuzzsia = 13;
            /// <summary> ÈÛÌúÄá°² </summary>
            public const short EmberOfNien = 14;
            /// <summary> ²ÐÈ±Áé»êÖ®Ø° </summary>
            public const short FragmentarySoulDagger = 15;
            /// <summary> Áé½ðÊô¾Þ½£ </summary>
            public const short SpiritIronGreatsword = 16;
            /// <summary> Áé¹â </summary>
            public const short MysticalLight = 17;
            /// <summary> ¶ÌØ°Ê× </summary>
            public const short ShortDagger = 18;
            /// <summary> ÁéÄ¾Ä§ÕÈ </summary>
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
                { "ArgenteusOathKeeper", ArgenteusOathKeeper },
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
                { "ArgenteusOathKeeper", LegacyObject[Collection["ArgenteusOathKeeper"]] },
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
                //{ ArgenteusOathKeeper, ItemType<Content.Items.>() },
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
                //{ LegacyObject[ArgenteusOathKeeper], ArgenteusOathKeeper },
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
            /// <summary> ÉËº¦Ôö·ù </summary>
            public static Content.Inscription C_Power => new Content.Inscription(0, 0);
            /// <summary> ¸ßËÙÓ½³ª </summary>
            public static Content.Inscription C_Chanter => new Content.Inscription(1, 0);
            /// <summary> ÆÓËØ </summary>
            public static Content.Inscription C_Economization => new Content.Inscription(2, 0);
            /// <summary> °ÂÊõÕÆ¿Ø </summary>
            public static Content.Inscription C_Arcane => new Content.Inscription(3, 0);
            /// <summary> ³äÄÜ </summary>
            public static Content.Inscription C_Charging => new Content.Inscription(4, 0);
            /// <summary> ÃØ·¨ÄÜÁ÷ </summary>
            public static Content.Inscription C_Reiki => new Content.Inscription(5, 0);
            /// <summary> ÄÜÁ¿ÎüÊÕ </summary>
            public static Content.Inscription C_Assimilation => new Content.Inscription(6, 0);
            /// <summary> ¼¼ÄÜÔö·ù </summary>
            public static Content.Inscription C_SkillPower => new Content.Inscription(7, 0);
            /// <summary> ¿ìËÙÄý¾Û </summary>
            public static Content.Inscription C_MagicAdept => new Content.Inscription(8, 0);
            /// <summary> ÖÎÁÆ¾«Í¨ </summary>
            public static Content.Inscription C_Saint => new Content.Inscription(9, 0);
            /// <summary> ¾øÍûÆíµ» </summary>
            public static Content.Inscription C_Evangel => new Content.Inscription(10, 0);
            /// <summary> ÆÆÕÀ¶´Ï¤ </summary>
            public static Content.Inscription C_Destroyer => new Content.Inscription(11, 0);
            /// <summary> Çá±ã </summary>
            public static Content.Inscription C_Dexterity => new Content.Inscription(12, 0);
            /// <summary> ÔËÓÃ×ÔÈç </summary>
            public static Content.Inscription C_MeleeAdept => new Content.Inscription(13, 0);
            /// <summary> Òå·ßÌîâß </summary>
            public static Content.Inscription C_Indignation => new Content.Inscription(14, 0);
            /// <summary> ¾ÞÁ¦ </summary>
            public static Content.Inscription C_Force => new Content.Inscription(15, 0);
            /// <summary> ÔÎÑ£´ò»÷ </summary>
            public static Content.Inscription C_Smiting => new Content.Inscription(16, 0);
            /// <summary> ÔÎÑ£Ç¿»¯ </summary>
            public static Content.Inscription C_Shockwave => new Content.Inscription(17, 0);
            /// <summary> ÊìÁ· </summary>
            public static Content.Inscription C_RaidAdept => new Content.Inscription(18, 0);
            /// <summary> ÓÐÐ§ÇÐ¸î </summary>
            public static Content.Inscription C_Murder => new Content.Inscription(19, 0);
            /// <summary> ¾«×¼ÊÕ¸î </summary>
            public static Content.Inscription C_Torture => new Content.Inscription(20, 0);
            /// <summary> Éñ·æ </summary>
            public static Content.Inscription C_Assassination => new Content.Inscription(21, 0);
            /// <summary> Ó¥ÑÛ </summary>
            public static Content.Inscription C_EagleEye => new Content.Inscription(22, 0);
            /// <summary> ¹á´©´ò»÷ </summary>
            public static Content.Inscription C_Penetration => new Content.Inscription(23, 0);
            /// <summary> Ôö´óÈÝÁ¿ </summary>
            public static Content.Inscription C_EnchantedMagazine => new Content.Inscription(24, 0);
            /// <summary> ¸½Ä§¹­ÏÒ </summary>
            public static Content.Inscription C_EnchantedBowstring => new Content.Inscription(25, 0);

            /// <summary> »ðµ¯ </summary>
            public static Content.Inscription S_Fireball => new Content.Inscription(0, CNCID.Item.FireGrimoire);
            /// <summary> ·ÙÑæ </summary>
            public static Content.Inscription S_Incinerating => new Content.Inscription(1, CNCID.Item.FireGrimoire);
            /// <summary> ÈÛ»¯ </summary>
            public static Content.Inscription S_Scorching => new Content.Inscription(2, CNCID.Item.FireGrimoire);
            /// <summary> ÁÒÑæ¿ñÈÈ </summary>
            public static Content.Inscription S_Enthusiasm => new Content.Inscription(3, CNCID.Item.FireGrimoire);
            /// <summary> º®Àä </summary>
            public static Content.Inscription S_Gelidity => new Content.Inscription(0, CNCID.Item.IceTooth);
            /// <summary> ¶³½á </summary>
            public static Content.Inscription S_Glacier => new Content.Inscription(1, CNCID.Item.IceTooth);
            /// <summary> ±ùÁÑ </summary>
            public static Content.Inscription S_Iceflake => new Content.Inscription(2, CNCID.Item.IceTooth);
            /// <summary> Ëé±ù </summary>
            public static Content.Inscription S_Shattering => new Content.Inscription(3, CNCID.Item.IceTooth);
            /// <summary> Ëª¶³»·ÈÆ </summary>
            public static Content.Inscription S_FrostWave => new Content.Inscription(4, CNCID.Item.IceTooth);
            /// <summary> ¾Û¼¯µçÄÜ </summary>
            public static Content.Inscription S_Charging => new Content.Inscription(0, CNCID.Item.Superconductor);
            /// <summary> ÖäÎÄÉÁµçÁ´ </summary>
            public static Content.Inscription S_SpellLightningChain => new Content.Inscription(1, CNCID.Item.Superconductor);
            /// <summary> °ÂÊõ³¬ÔØ </summary>
            public static Content.Inscription S_ArcaneOverload => new Content.Inscription(2, CNCID.Item.Superconductor);
            /// <summary> ÖäÎÄÔö¹ã </summary>
            public static Content.Inscription S_SpellReconstruction => new Content.Inscription(3, CNCID.Item.Superconductor);
            /// <summary> ¾«½øÖäÎÄÑÐ¾¿ </summary>
            public static Content.Inscription S_SpellAugmentated => new Content.Inscription(4, CNCID.Item.Superconductor);
            /// <summary> ·¨ÊõÎÈ¶¨ </summary>
            public static Content.Inscription S_StableSpell => new Content.Inscription(5, CNCID.Item.Superconductor);
            /// <summary> Òø°×Ö®Ê¸ </summary>
            public static Content.Inscription S_SilveryArrow => new Content.Inscription(0, CNCID.Item.ArgenteusClaw);
            /// <summary> ÆÆ¿ÕÁ÷¹â </summary>
            public static Content.Inscription S_Sanctity => new Content.Inscription(1, CNCID.Item.ArgenteusClaw);
            /// <summary> Ð°¶ñ¿ËÐÇ </summary>
            public static Content.Inscription S_Exorcism => new Content.Inscription(2, CNCID.Item.ArgenteusClaw);
            /// <summary> ÒøÑæ </summary>
            public static Content.Inscription S_SilveryFlame => new Content.Inscription(0, CNCID.Item.ArgenteusOathKeeper);
            /// <summary> ÉñÊ¥³Í½ä </summary>
            public static Content.Inscription S_Judgement => new Content.Inscription(1, CNCID.Item.ArgenteusOathKeeper);
            /// <summary> Òø¶Ü </summary>
            public static Content.Inscription S_SilveryShield => new Content.Inscription(2, CNCID.Item.ArgenteusOathKeeper);
            /// <summary> Òø°×Ê÷Á¢ÊÄ </summary>
            public static Content.Inscription S_SilveryVow => new Content.Inscription(3, CNCID.Item.ArgenteusOathKeeper);
            /// <summary> ÑÒÊ¯ </summary>
            public static Content.Inscription S_Rock => new Content.Inscription(0, CNCID.Item.TheLightOfAuzzsia);
            /// <summary> ÒÆ¶¯Ê©·¨ </summary>
            public static Content.Inscription S_ActiveChant => new Content.Inscription(1, CNCID.Item.TheLightOfAuzzsia);
            /// <summary> ±¬ÁÑ </summary>
            public static Content.Inscription S_Fragmentation => new Content.Inscription(2, CNCID.Item.TheLightOfAuzzsia);
            /// <summary> ÔÎÑ£ </summary>
            public static Content.Inscription S_DizzyStrike => new Content.Inscription(0, CNCID.Item.EmberOfNien);
            /// <summary> ²ÐÈ±Áé»ê×çÖä </summary>
            public static Content.Inscription S_EmberPrayer => new Content.Inscription(1, CNCID.Item.EmberOfNien);
            /// <summary> ÈÛÌúÊ¥ÑÔ </summary>
            public static Content.Inscription S_EmberOath => new Content.Inscription(2, CNCID.Item.EmberOfNien);
            /// <summary> ²ÐÈ±Áé»ê×çÖä </summary>
            public static Content.Inscription S_TameHead => new Content.Inscription(0, CNCID.Item.FragmentarySoulDagger);
            /// <summary> ²»ÃðÁé»ê»ðÑæ </summary>
            public static Content.Inscription S_SoulScorching => new Content.Inscription(1, CNCID.Item.FragmentarySoulDagger);
            /// <summary> ¸½Ä§¹­ÏÒ </summary>
            public static Content.Inscription S_IndulgentGhost => new Content.Inscription(2, CNCID.Item.FragmentarySoulDagger);
            /// <summary> Õ¶Ê× </summary>
            public static Content.Inscription S_Decapitation => new Content.Inscription(0, CNCID.Item.SpiritIronGreatsword);
            /// <summary> Áé½ðÊô </summary>
            public static Content.Inscription S_ConsciousMetal => new Content.Inscription(1, CNCID.Item.SpiritIronGreatsword);
            /// <summary> ´ï¶û¸£ÓÓ </summary>
            public static Content.Inscription S_BlessingOfDahl => new Content.Inscription(0, CNCID.Item.MysticalLight);
            #endregion
            public static Dictionary<Point, string> Collection => new Dictionary<Point, string>()
            {
                { new Point(0, 0), "C_Power" },
                { new Point(1, 0), "C_Chanter" },
                { new Point(2, 0), "C_Economization" },
                { new Point(3, 0), "C_Arcane" },
                { new Point(4, 0), "C_Charging" },
                { new Point(5, 0), "C_Reiki" },
                { new Point(6, 0), "C_Assimilation" },
                { new Point(7, 0), "C_SkillPower" },
                { new Point(8, 0), "C_MagicAdept" },
                { new Point(9, 0), "C_Saint" },
                { new Point(10, 0), "C_Evangel" },
                { new Point(11, 0), "C_Destroyer" },
                { new Point(12, 0), "C_Dexterity" },
                { new Point(13, 0), "C_MeleeAdept" },
                { new Point(14, 0), "C_Indignation" },
                { new Point(15, 0), "C_Force" },
                { new Point(16, 0), "C_Smiting" },
                { new Point(17, 0), "C_Shockwave" },
                { new Point(18, 0), "C_RaidAdept" },
                { new Point(19, 0), "C_Murder" },
                { new Point(20, 0), "C_Torture" },
                { new Point(21, 0), "C_Assassination" },
                { new Point(22, 0), "C_EagleEye" },
                { new Point(23, 0), "C_Penetration" },
                { new Point(24, 0), "C_EnchantedMagazine" },
                { new Point(25, 0), "C_EnchantedBowstring" },
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
                { new Point(0, CNCID.Item.ArgenteusOathKeeper), "S_SilveryFlame" },
                { new Point(1, CNCID.Item.ArgenteusOathKeeper), "S_Judgement" },
                { new Point(2, CNCID.Item.ArgenteusOathKeeper), "S_SilveryShield" },
                { new Point(3, CNCID.Item.ArgenteusOathKeeper), "S_SilveryVow" },
                { new Point(0, CNCID.Item.TheLightOfAuzzsia), "S_Rock" },
                { new Point(1, CNCID.Item.TheLightOfAuzzsia), "S_ActiveChant" },
                { new Point(2, CNCID.Item.TheLightOfAuzzsia), "S_Fragmentation" },
                { new Point(0, CNCID.Item.EmberOfNien), "S_DizzyStrike" },
                { new Point(1, CNCID.Item.EmberOfNien), "S_EmberPrayer" },
                { new Point(2, CNCID.Item.EmberOfNien), "S_EmberOath" },
                { new Point(0, CNCID.Item.FragmentarySoulDagger), "S_TameHead" },
                { new Point(1, CNCID.Item.FragmentarySoulDagger), "S_SoulScorching" },
                { new Point(2, CNCID.Item.FragmentarySoulDagger), "S_IndulgentGhost" },
                { new Point(0, CNCID.Item.SpiritIronGreatsword), "S_Decapitation" },
                { new Point(1, CNCID.Item.SpiritIronGreatsword), "S_ConsciousMetal" },
                { new Point(0, CNCID.Item.MysticalLight), "S_BlessingOfDahl" },
            };

            public static Dictionary<string, Content.Inscription> LegacyCollection => new Dictionary<string, Content.Inscription>()
            {
                { "C_Power", C_Power },
                { "C_Chanter", C_Chanter },
                { "C_Economization", C_Economization },
                { "C_Arcane", C_Arcane },
                { "C_Charging", C_Charging },
                { "C_Reiki", C_Reiki },
                { "C_Assimilation", C_Assimilation },
                { "C_SkillPower", C_SkillPower },
                { "C_MagicAdept", C_MagicAdept },
                { "C_Saint", C_Saint },
                { "C_Evangel", C_Evangel },
                { "C_Destroyer", C_Destroyer },
                { "C_Dexterity", C_Dexterity },
                { "C_MeleeAdept", C_MeleeAdept },
                { "C_Indignation", C_Indignation },
                { "C_Force", C_Force },
                { "C_Smiting", C_Smiting },
                { "C_Shockwave", C_Shockwave },
                { "C_RaidAdept", C_RaidAdept },
                { "C_Murder", C_Murder },
                { "C_Torture", C_Torture },
                { "C_Assassination", C_Assassination },
                { "C_EagleEye", C_EagleEye },
                { "C_Penetration", C_Penetration },
                { "C_EnchantedMagazine", C_EnchantedMagazine },
                { "C_EnchantedBowstring", C_EnchantedBowstring },
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
                { "S_Decapitation", S_Decapitation },
                { "S_ConsciousMetal", S_ConsciousMetal },
                { "S_BlessingOfDahl", S_BlessingOfDahl },
            };

            public static Dictionary<Point, Content.Inscription> LegacyObject => new Dictionary<Point, Content.Inscription>()
            {
                { new Point(0, 0), C_Power },
                { new Point(1, 0), C_Chanter },
                { new Point(2, 0), C_Economization },
                { new Point(3, 0), C_Arcane },
                { new Point(4, 0), C_Charging },
                { new Point(5, 0), C_Reiki },
                { new Point(6, 0), C_Assimilation },
                { new Point(7, 0), C_SkillPower },
                { new Point(8, 0), C_MagicAdept },
                { new Point(9, 0), C_Saint },
                { new Point(10, 0), C_Evangel },
                { new Point(11, 0), C_Destroyer },
                { new Point(12, 0), C_Dexterity },
                { new Point(13, 0), C_MeleeAdept },
                { new Point(14, 0), C_Indignation },
                { new Point(15, 0), C_Force },
                { new Point(16, 0), C_Smiting },
                { new Point(17, 0), C_Shockwave },
                { new Point(18, 0), C_RaidAdept },
                { new Point(19, 0), C_Murder },
                { new Point(20, 0), C_Torture },
                { new Point(21, 0), C_Assassination },
                { new Point(22, 0), C_EagleEye },
                { new Point(23, 0), C_Penetration },
                { new Point(24, 0), C_EnchantedMagazine },
                { new Point(25, 0), C_EnchantedBowstring },
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
                { new Point(0, CNCID.Item.ArgenteusOathKeeper), S_SilveryFlame },
                { new Point(1, CNCID.Item.ArgenteusOathKeeper), S_Judgement },
                { new Point(2, CNCID.Item.ArgenteusOathKeeper), S_SilveryShield },
                { new Point(3, CNCID.Item.ArgenteusOathKeeper), S_SilveryVow },
                { new Point(0, CNCID.Item.TheLightOfAuzzsia), S_Rock },
                { new Point(1, CNCID.Item.TheLightOfAuzzsia), S_ActiveChant },
                { new Point(2, CNCID.Item.TheLightOfAuzzsia), S_Fragmentation },
                { new Point(0, CNCID.Item.EmberOfNien), S_DizzyStrike },
                { new Point(1, CNCID.Item.EmberOfNien), S_EmberPrayer },
                { new Point(2, CNCID.Item.EmberOfNien), S_EmberOath },
                { new Point(0, CNCID.Item.FragmentarySoulDagger), S_TameHead },
                { new Point(1, CNCID.Item.FragmentarySoulDagger), S_SoulScorching },
                { new Point(2, CNCID.Item.FragmentarySoulDagger), S_IndulgentGhost },
                { new Point(0, CNCID.Item.SpiritIronGreatsword), S_Decapitation },
                { new Point(1, CNCID.Item.SpiritIronGreatsword), S_ConsciousMetal },
                { new Point(0, CNCID.Item.MysticalLight), S_BlessingOfDahl },
            };
        }

        public static class NPC
        {
            public const short MaxObject = 6;
            #region ID
            /// <summary> µ¼Ê¦ </summary>
            public const short Mentor = 1;
            /// <summary> ¾²Ú×ÓÄÁé </summary>
            public const short TranquilityPixie = 2;
            /// <summary> ÁéÆøòùòð </summary>
            public const short PhantomBat = 3;
            /// <summary> ÁéÆøÊ·À³Ä· </summary>
            public const short PhantomSlime = 4;
            /// <summary> Áé»ð </summary>
            public const short PhantomFlame = 5;
            /// <summary> É­ÁÖÒâÖ¾ </summary>
            public const short SpiritBoss = 6;
            /// <summary> ÊØ»¤Ë®¾§ </summary>
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
            public const short MaxObject = 9;
            #region ID
            /// <summary> Ä§·¨·Éµ¯ </summary>
            public const short MentorMissile = 1;
            /// <summary> ÁéÆøÍÅ </summary>
            public const short SpiritMass = 2;
            /// <summary> ÁéÆøÊ¸ </summary>
            public const short SpiritArrowV1 = 3;
            /// <summary> ÁéÆøÊ¸ </summary>
            public const short SpiritArrowV2 = 4;
            /// <summary> ÁéÆøÊ¸ </summary>
            public const short SpiritArrowV3 = 5;
            /// <summary> »ðÇò </summary>
            public const short FireBall = 6;
            /// <summary> Ñ×±¬ </summary>
            public const short FireBallExplosion = 7;
            /// <summary> ±ùÑÀ </summary>
            public const short DentateIce = 8;
            /// <summary> ±ù»¨ </summary>
            public const short DentateIceExplosion = 9;
            /// <summary> Ëª³± </summary>
            public const short FrostWave = 10;
            #endregion
            public static Dictionary<string, short> Collection => new Dictionary<string, short>()
            {
                { "MentorMissile", MentorMissile },
                { "SpiritMass", SpiritMass },
                { "SpiritArrowV1", SpiritArrowV1 },
                { "SpiritArrowV2", SpiritArrowV2 },
                { "SpiritArrowV3", SpiritArrowV3 },
                { "FireBall", FireBall },
                { "FireBallExplosion", FireBallExplosion },
                { "DentateIce", DentateIce },
                { "DentateIceExplosion", DentateIceExplosion },
                { "FrostWave", FrostWave },
            };

            public static Dictionary<string, int> LegacyCollection => new Dictionary<string, int>()
            {
                { "MentorMissile", LegacyObject[Collection["MentorMissile"]] },
                { "SpiritMass", LegacyObject[Collection["SpiritMass"]] },
                { "SpiritArrowV1", LegacyObject[Collection["SpiritArrowV1"]] },
                { "SpiritArrowV2", LegacyObject[Collection["SpiritArrowV2"]] },
                { "SpiritArrowV3", LegacyObject[Collection["SpiritArrowV3"]] },
                { "FireBall", LegacyObject[Collection["FireBall"]] },
                { "FireBallExplosion", LegacyObject[Collection["FireBallExplosion"]] },
                { "DentateIce", LegacyObject[Collection["DentateIce"]] },
                { "DentateIceExplosion", LegacyObject[Collection["DentateIceExplosion"]] },
                { "FrostWave", LegacyObject[Collection["FrostWave"]] },
            };

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { MentorMissile, ProjectileType<Content.Projectiles.MentorMissile>() },
                { SpiritMass, ProjectileType<Content.Projectiles.Hostile.SpiritMass>() },
                { SpiritArrowV1, ProjectileType<Content.Projectiles.Hostile.SpiritArrow.V1>() },
                { SpiritArrowV2, ProjectileType<Content.Projectiles.Hostile.SpiritArrow.V2>() },
                { SpiritArrowV3, ProjectileType<Content.Projectiles.Hostile.SpiritArrow.V3>() },
                { FireBall, ProjectileType<Content.Projectiles.FireGrimoire.FireBall>() },
                { FireBallExplosion, ProjectileType<Content.Projectiles.FireGrimoire.FireBallExplosion>() },
                { DentateIce, ProjectileType<Content.Projectiles.IceTooth.DentateIce>() },
                { DentateIceExplosion, ProjectileType<Content.Projectiles.IceTooth.DentateIceExplosion>() },
                { FrostWave, ProjectileType<Content.Projectiles.IceTooth.FrostWave>() },
            };
        }

        public static class State
        {
            public const byte MaxObject = 6;
            #region ID
            /// <summary> ¿ñÅ­ </summary>
            public const short Wrath = 1;
            /// <summary> Ð¹Æø </summary>
            public const short Cowardice = 2;
            /// <summary> ÐÅÑöÁÑºÛ </summary>
            public const short Blasphemy = 3;
            /// <summary> »ÅÂÒ </summary>
            public const short Fluster = 4;
            /// <summary> Ìî³ä </summary>
            public const short Restoring = 5;
            /// <summary> ÒþÄä </summary>
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