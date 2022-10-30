using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Cb = CNC.Content.Buffs;
using Cd = CNC.Content.Dusts;
using Ci = CNC.Content.Items;
//using Cw = CNC.Content.Walls;
using Cp = CNC.Content.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace CNC.Common.Utils
{
    public static class CNCID
    {
        public abstract class Buff
        {
            public const short MaxObject = 12;
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
            /// <summary> Ñ£ÔÎ </summary>
            public const short Dizzy = 9;
            /// <summary> Á÷Ñª </summary>
            public const short Bleeding = 10;
            /// <summary> ÉñÊ¥»¤¶Ü </summary>
            public const short Blessed = 11;
            /// <summary> ÐÇºÛ </summary>
            public const short StarMark = 12;
            #endregion
            public static Dictionary<string, short> Collection
            {
                get
                {
                    Dictionary<string, short> dict = new Dictionary<string, short>();
                    foreach (var member in typeof(CNCID.Buff).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, (short)member.GetRawConstantValue());
                    return dict;
                }
            }

            public static Dictionary<string, int> LegacyCollection
            {
                get
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (var member in typeof(CNCID.Buff).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, LegacyObject[Collection[member.Name]]);
                    return dict;
                }
            }

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { MiracleFox, BuffType<Content.Mounts.MiracleFoxBuff>() },
                { Scorched, BuffType<Content.Buffs.Scorched>() },
                { Enthused, BuffType<Content.Buffs.Enthused>() },
                { Gelid, BuffType<Content.Buffs.Gelid>() },
                { Glacier, BuffType<Content.Buffs.Glacier>() },
                { Overloaded, BuffType<Content.Buffs.Overloaded>() },
                { Surged, BuffType<Content.Buffs.Surged>() },
                { Sanctified, BuffType<Content.Buffs.Sanctified>() },
                { Dizzy, BuffType<Content.Buffs.Dizzy>() },
                { Bleeding, BuffType<Content.Buffs.Bleeding>() },
                { Blessed, BuffType<Content.Buffs.Blessed>() },
                { StarMark, BuffType<Content.Buffs.StarMark>() },
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
            public static Color Barbarian => CNCUtils.FlashColor(168, 61, 51, 255, false);
            public static Color Defender => CNCUtils.FlashColor(46, 109, 153, 255, false);
            public static Color Crusader => CNCUtils.FlashColor(219, 173, 89, 255, false);
            public static Color Ranger => CNCUtils.FlashColor(57, 153, 46, 255, false);
            public static Color Gunman => CNCUtils.FlashColor(126, 229, 218, 255, false);
            public static Color Thrower => CNCUtils.FlashColor(109, 81, 57, 255, false);
            public static Color Musketeer => CNCUtils.FlashColor(46, 109, 153, 255, false);
            public static Color Cleric => CNCUtils.FlashColor(232, 232, 11, 255, false);
            public static Color Wizard => CNCUtils.FlashColor(112, 45, 179, 255, false);
            public static Color SpellBreaker => CNCUtils.FlashColor(126, 161, 229, 255, false);
            public static Color Druid => CNCUtils.FlashColor(188, 242, 170, 255, false);
            public static Color Shaman => CNCUtils.FlashColor(122, 36, 70, 255, false);
            public static Color Warlock => CNCUtils.FlashColor(76, 46, 77, 255, false);
            public static Color Monk => CNCUtils.FlashColor(255, 255, 255, 255, false);
            public static Color Assassin => CNCUtils.FlashColor(31, 32, 38, 255, false);
            public static Color Generic => CNCUtils.FlashColor(0, 0, 0, 0, true);
            public static Color Melee => CNCUtils.FlashColor(217, 60, 60, 0, true);
            public static Color Ranged => CNCUtils.FlashColor(67, 230, 57, 0, true);
            public static Color Magic => CNCUtils.FlashColor(53, 157, 255, 0, true);
            public static Color Summon => CNCUtils.FlashColor(224, 44, 230, 0, true);
            public static Color Throwing => CNCUtils.FlashColor(255, 255, 255, 0, true);
            //public static Color Generic => CNCUtils.FlashColor(255, 196, 85, 0, true);
            //public static Color Generic => CNCUtils.FlashColor(179, 175, 50, 0, true);
            //public static Color Generic => CNCUtils.FlashColor(139, 29, 242, 0, true);
            public static Color GenericCrit => CNCUtils.FlashColor(0, 0, 0, 0, true);
            public static Color MeleeCrit => CNCUtils.FlashColor(163, 29, 29, 0, true);
            public static Color RangedCrit => CNCUtils.FlashColor(31, 171, 27, 0, true);
            public static Color MagicCrit => CNCUtils.FlashColor(61, 128, 192, 0, true);
            public static Color SummonCrit => CNCUtils.FlashColor(224, 44, 230, 0, true);
            public static Color ThrowingCrit => CNCUtils.FlashColor(255, 255, 255, 0, true);
            //public static Color GenericCrit => CNCUtils.FlashColor(255, 196, 85, 0, true);
            //public static Color GenericCrit => CNCUtils.FlashColor(179, 175, 50, 0, true);
            //public static Color GenericCrit => CNCUtils.FlashColor(103, 43, 159, 0, true);
            public static Color SpiritBoss => CNCUtils.FlashColor(178, 214, 231, 255, true);
            public static Color SpiritBoss2 => CNCUtils.FlashColor(178, 214, 231, 0, false);
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

            public static Dictionary<string, short> Collection
            {
                get
                {
                    Dictionary<string, short> dict = new Dictionary<string, short>();
                    foreach (var member in typeof(CNCID.Item).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, (short)member.GetRawConstantValue());
                    return dict;
                }
            }

            public static Dictionary<string, int> LegacyCollection
            {
                get
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (var member in typeof(CNCID.Item).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, LegacyObject[Collection[member.Name]]);
                    return dict;
                }
            }

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { MiracleGem, ItemType<Content.Items.MiracleGem>() },
                { DestineScroll, ItemType<Content.Items.DestineScroll>() },
                { FireGrimoire, ItemType<Content.Items.Weapons.Magic.FireGrimoire>() },
                { IceTooth, ItemType<Content.Items.Weapons.Magic.IceTooth>() },
                { Superconductor, ItemType<Content.Items.Weapons.Magic.Superconductor>() },
                { ArgenteusClaw, ItemType<Content.Items.Weapons.Ranged.ArgenteusClaw>() },
                { SpiritWood, ItemType<Content.Items.SpiritWood>() },
                { TranquilityGem, ItemType<Content.Items.TranquilityGem>() },
                { SpiritSapling, ItemType<Content.Items.SpiritSapling>() },
                { TranquilityDirt, ItemType<Content.Items.TranquilityDirt>() },
                { TranquilityStone, ItemType<Content.Items.TranquilityStone>() },
                { ArgenteusOathKeeper, ItemType<Content.Items.Weapons.Melee.TwoHandedSword.ArgenteusOathKeeper>() },
                { TheLightOfAuzzsia, ItemType<Content.Items.Weapons.Magic.Sceptre.TheLightOfAuzzsia>() },
                { EmberOfNien, ItemType<Content.Items.Weapons.Melee.Hammer.EmberOfNien>() },
                //{ FragmentarySoulDagger, ItemType<Content.Items.>() },
                //{ SpiritIronGreatsword, ItemType<Content.Items.>() },
                //{ MysticalLight, ItemType<Content.Items.>() },
                //{ ShortDagger, ItemType<Content.Items.>() },
                //{ SpiritSparkingWand, ItemType<Content.Items.>() },
            };
        }

        public static class Inscription
        {
            public const short MaxObject = 26 + 39;
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
            /// <summary> ¾Û±ä </summary>
            public static Content.Inscription S_Fusion => new Content.Inscription(1, CNCID.Item.Superconductor);
            /// <summary> ¹ýÔØ </summary>
            public static Content.Inscription S_Overload => new Content.Inscription(2, CNCID.Item.Superconductor);
            /// <summary> ±¬ÁÑÉÁµç </summary>
            public static Content.Inscription S_RadialLightning => new Content.Inscription(3, CNCID.Item.Superconductor);
            /// <summary> Á¬ËøÉÁµç </summary>
            public static Content.Inscription S_ChainedLightning => new Content.Inscription(4, CNCID.Item.Superconductor);
            /// <summary> ÌìÇ´ </summary>
            public static Content.Inscription S_Thunderstorm => new Content.Inscription(5, CNCID.Item.Superconductor);
            /// <summary> µçÄÜ»¤¶Ü </summary>
            public static Content.Inscription S_ElectricBarrier => new Content.Inscription(6, CNCID.Item.Superconductor);
            /// <summary> °ÂÊõ³¬ÔØ </summary>
            public static Content.Inscription S_ArcaneOverload => new Content.Inscription(7, CNCID.Item.Superconductor);
            /// <summary> ¼¤»¯ </summary>
            public static Content.Inscription S_Intensity => new Content.Inscription(8, CNCID.Item.Superconductor);
            /// <summary> »î»¯ </summary>
            public static Content.Inscription S_Activation => new Content.Inscription(9, CNCID.Item.Superconductor);
            /// <summary> Í¬»¯ </summary>
            public static Content.Inscription S_Resonance => new Content.Inscription(10, CNCID.Item.Superconductor);
            /// <summary> Òø°×Ö®Ê¸ </summary>
            public static Content.Inscription S_SilveryArrow => new Content.Inscription(0, CNCID.Item.ArgenteusClaw);
            /// <summary> ÆÆ¿ÕÁ÷¹â </summary>
            public static Content.Inscription S_Sanctity => new Content.Inscription(1, CNCID.Item.ArgenteusClaw);
            /// <summary> Ð°¶ñ¿ËÐÇ </summary>
            public static Content.Inscription S_Exorcism => new Content.Inscription(2, CNCID.Item.ArgenteusClaw);
            /// <summary> ÉñÊ¥³Í½ä </summary>
            public static Content.Inscription S_Judgement => new Content.Inscription(0, CNCID.Item.ArgenteusOathKeeper);
            /// <summary> Òø¶Ü </summary>
            public static Content.Inscription S_SilveryShield => new Content.Inscription(1, CNCID.Item.ArgenteusOathKeeper);
            /// <summary> ÒøÉ«»ð»¨ </summary>
            public static Content.Inscription S_SilverySpark => new Content.Inscription(2, CNCID.Item.ArgenteusOathKeeper);
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
                { new Point(1, CNCID.Item.Superconductor), "S_Fusion" },
                { new Point(2, CNCID.Item.Superconductor), "S_Overload" },
                { new Point(3, CNCID.Item.Superconductor), "S_RadialLightning" },
                { new Point(4, CNCID.Item.Superconductor), "S_ChainedLightning" },
                { new Point(5, CNCID.Item.Superconductor), "S_Thunderstorm" },
                { new Point(6, CNCID.Item.Superconductor), "S_ElectricBarrier" },
                { new Point(7, CNCID.Item.Superconductor), "S_ArcaneOverload" },
                { new Point(8, CNCID.Item.Superconductor), "S_Intensity" },
                { new Point(9, CNCID.Item.Superconductor), "S_Activation" },
                { new Point(10, CNCID.Item.Superconductor), "S_Resonance" },
                { new Point(0, CNCID.Item.ArgenteusClaw), "S_SilveryArrow" },
                { new Point(1, CNCID.Item.ArgenteusClaw), "S_Sanctity" },
                { new Point(2, CNCID.Item.ArgenteusClaw), "S_Exorcism" },
                { new Point(0, CNCID.Item.ArgenteusOathKeeper), "S_Judgement" },
                { new Point(1, CNCID.Item.ArgenteusOathKeeper), "S_SilveryShield" },
                { new Point(2, CNCID.Item.ArgenteusOathKeeper), "S_SilverySpark" },
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
                { "S_Fusion", S_Fusion },
                { "S_Overload", S_Overload },
                { "S_RadialLightning", S_RadialLightning },
                { "S_ChainedLightning", S_ChainedLightning },
                { "S_Thunderstorm", S_Thunderstorm },
                { "S_ElectricBarrier", S_ElectricBarrier },
                { "S_ArcaneOverload", S_ArcaneOverload },
                { "S_Intensity", S_Intensity },
                { "S_Activation", S_Activation },
                { "S_Resonance", S_Resonance },
                { "S_SilveryArrow", S_SilveryArrow },
                { "S_Sanctity", S_Sanctity },
                { "S_Exorcism", S_Exorcism },
                { "S_Judgement", S_Judgement },
                { "S_SilveryShield", S_SilveryShield },
                { "S_SilverySpark", S_SilverySpark },
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
                { new Point(1, CNCID.Item.Superconductor), S_Fusion },
                { new Point(2, CNCID.Item.Superconductor), S_Overload },
                { new Point(3, CNCID.Item.Superconductor), S_RadialLightning },
                { new Point(4, CNCID.Item.Superconductor), S_ChainedLightning },
                { new Point(5, CNCID.Item.Superconductor), S_Thunderstorm },
                { new Point(6, CNCID.Item.Superconductor), S_ElectricBarrier },
                { new Point(7, CNCID.Item.Superconductor), S_ArcaneOverload },
                { new Point(8, CNCID.Item.Superconductor), S_Intensity },
                { new Point(9, CNCID.Item.Superconductor), S_Activation },
                { new Point(10, CNCID.Item.Superconductor), S_Resonance },
                { new Point(0, CNCID.Item.ArgenteusClaw), S_SilveryArrow },
                { new Point(1, CNCID.Item.ArgenteusClaw), S_Sanctity },
                { new Point(2, CNCID.Item.ArgenteusClaw), S_Exorcism },
                { new Point(0, CNCID.Item.ArgenteusOathKeeper), S_Judgement },
                { new Point(1, CNCID.Item.ArgenteusOathKeeper), S_SilveryShield },
                { new Point(2, CNCID.Item.ArgenteusOathKeeper), S_SilverySpark },
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
            public const short MaxObject = 5;
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
            //public const short PhantomFlame = 5;
            /// <summary> É­ÁÖÒâÖ¾ </summary>
            public const short SpiritBoss = 6;
            #endregion
            public static Dictionary<string, short> Collection
            {
                get
                {
                    Dictionary<string, short> dict = new Dictionary<string, short>();
                    foreach (var member in typeof(CNCID.NPC).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, (short)member.GetRawConstantValue());
                    return dict;
                }
            }

            public static Dictionary<string, int> LegacyCollection
            {
                get
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (var member in typeof(CNCID.NPC).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, LegacyObject[Collection[member.Name]]);
                    return dict;
                }
            }

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { Mentor, NPCType<Content.NPCs.TownNPCs.Mentor>() },
                { TranquilityPixie, NPCType<Content.NPCs.TranquilityPixie>() },
                { PhantomBat, NPCType<Content.NPCs.PhantomBat>() },
                { PhantomSlime, NPCType<Content.NPCs.PhantomSlime>() },
                //{ PhantomFlame, NPCType<Content.NPCs.PhantomFlame>() },
                { SpiritBoss, NPCType<Content.NPCs.SpiritBoss.SpiritBoss>() },
            };
        }

        public static class Prefix
        {
            public const byte MaxObject = 8;
            #region ID
            /// <summary> æµÊì </summary>
			public const byte Memorable = 1;
            /// <summary> ¾«Í¨ </summary>
            public const byte Proficient = 2; 
            /// <summary> ¼¯ÖÐ </summary>
            public const byte Concentrated = 3; 
            /// <summary> »ØÏì </summary>
            public const byte Reverberant = 4; 
            /// <summary> ¼òÃ÷ </summary>
            public const byte Concise = 5; 
            /// <summary> ¾«Á· </summary>
            public const byte Conversant = 6; 
            /// <summary> Í¸³¹ </summary>
            public const byte Incisive = 7; 
            /// <summary> È«Öª </summary>
            public const byte Omniscient = 8;
            #endregion
            public static Dictionary<string, byte> Collection
            {
                get
                {
                    Dictionary<string, byte> dict = new Dictionary<string, byte>();
                    foreach (var member in typeof(CNCID.Prefix).GetFields())
                        if (member.FieldType == typeof(byte)) dict.Add(member.Name, (byte)member.GetRawConstantValue());
                    return dict;
                }
            }

            public static Dictionary<string, int> LegacyCollection
            {
                get
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (var member in typeof(CNCID.Prefix).GetFields())
                        if (member.FieldType == typeof(byte)) dict.Add(member.Name, LegacyObject[Collection[member.Name]]);
                    return dict;
                }
            }

            public static Dictionary<byte, int> LegacyObject => new Dictionary<byte, int>()
            {
                { Memorable, PrefixType<Content.Prefixes.Memorable>() },
                { Proficient, PrefixType<Content.Prefixes.Proficient>() },
                { Concentrated, PrefixType<Content.Prefixes.Concentrated>() },
                { Reverberant, PrefixType<Content.Prefixes.Reverberant>() },
                { Concise, PrefixType<Content.Prefixes.Concise>() },
                { Conversant, PrefixType<Content.Prefixes.Conversant>() },
                { Incisive, PrefixType<Content.Prefixes.Incisive>() },
                { Omniscient, PrefixType<Content.Prefixes.Omniscient>() },
            };
        }

        public static class Projectile
        {
            public const short MaxObject = 36;
            #region ID
            /// <summary> Ä§·¨·Éµ¯ </summary>
            public const short MentorMissile = 1;
            /// <summary> ÁéÆøÍÅ </summary>
            public const short SpiritMass = 2;
            /// <summary> ÄÜÁ¿Ê¸ </summary>
            public const short EnergyArrow = 3;
            /// <summary> ÄÜÁ¿±¬ÆÆ </summary>
            public const short EnergyExplosion = 4;
            /// <summary> ÁéÆøÊ¸ </summary>
            public const short SpiritArrow = 5;
            /// <summary> Òø°×Í»´Ì£¨Boss£© </summary>
            public const short SilveryDashHostile = 6;
            /// <summary> Ô¤¾¯Ïß </summary>
            public const short WarningLine = 7;
            /// <summary> »ðÇò </summary>
            public const short FireBall = 8;
            /// <summary> Ñ×±¬ </summary>
            public const short FireBallExplosion = 9;
            /// <summary> ±ùÑÀ </summary>
            public const short DentateIce = 10;
            /// <summary> ±ù»¨ </summary>
            public const short DentateIceExplosion = 11;
            /// <summary> Ëª³± </summary>
            public const short FrostWave = 12;
            /// <summary> ÉÁµçµ¯ </summary>
            public const short LightningMissile = 13;
            /// <summary> ÉÁµç±¬ÆÆ </summary>
            public const short LightningExplosion = 14;
            /// <summary> ¹ýÔØÉÁµçµ¯ </summary>
            public const short OverloadedLightningMissile = 15;
            /// <summary> ¹ýÔØ±¬ÆÆ </summary>
            public const short OverloadedLightningExplosion = 16;
            /// <summary> ÉÁµçÁ´ </summary>
            public const short LightningChain = 17;
            /// <summary> Òø°×Ê¸ </summary>
            public const short SilveryArrow = 18;
            /// <summary> ÖÁÊ¥Ê¸ </summary>
            public const short SanctifiedArrow = 19;
            /// <summary> ÖÁÊ¥Éä»÷ </summary>
            public const short SanctifiedArrowShoot = 20;
            /// <summary> ÖÁÊ¥±¬ÆÆ </summary>
            public const short SanctifiedExplosion = 21;
            /// <summary> Ä§·¨ÑÒÊ¯ </summary>
            public const short MagicRock = 22;
            /// <summary> Õðµ´²¨ </summary>
            public const short ShockWave = 23;
            /// <summary> ÆÆÍÁ¼¬´Ì </summary>
            public const short GiantBranch = 24;
            /// <summary> ±¬Õ¨ </summary>
            public const short GenericExplosion = 25;
            /// <summary> Õ¶»÷ </summary>
            public const short GenericSlash = 26;
            /// <summary> Õ¶»÷Ð§¹û </summary>
            public const short GenericSlashHit = 27;
            /// <summary> Õ¶»÷Ð§¹û </summary>
            public const short ShaderSlash = 28;
            /// <summary> Òø¶Ü </summary>
            public const short SilveryField = 30;
            /// <summary> Òø°×Í»´Ì </summary>
            public const short SilveryDash = 31;
            /// <summary> ÌØÊâÕ¶»÷ </summary>
            public const short AnattaSlash = 32;
            /// <summary> ÈÕÔÎ </summary>
            public const short SolarHalo = 33;
            /// <summary> ÈÛÑÒ»ðÉà </summary>
            public const short FireFlow = 34;
            /// <summary> »ÃÏóÖ®ÈÐ </summary>
            public const short PhantomExcalibur = 35;
            /// <summary> Ðé¿ÕÖ®Óü </summary>
            public const short VoidNightsEdge = 36;
            #endregion
            public static Dictionary<string, short> Collection
            {
                get
                {
                    Dictionary<string, short> dict = new Dictionary<string, short>();
                    foreach (var member in typeof(CNCID.Projectile).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, (short)member.GetRawConstantValue());
                    return dict;
                }
            }

            public static Dictionary<string, int> LegacyCollection
            {
                get
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (var member in typeof(CNCID.Projectile).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, LegacyObject[Collection[member.Name]]);
                    return dict;
                }
            }

            public static Dictionary<short, int> LegacyObject => new Dictionary<short, int>()
            {
                { MentorMissile, ProjectileType<Content.Projectiles.MentorMissile>() },
                { SpiritMass, ProjectileType<Content.Projectiles.Hostile.SpiritMass>() },
                { SpiritArrow, ProjectileType<Content.Projectiles.Hostile.SpiritArrow>() },
                { EnergyArrow, ProjectileType<Content.Projectiles.Hostile.EnergyArrow>() },
                { EnergyExplosion, ProjectileType<Content.Projectiles.Hostile.EnergyExplosion>() },
                { WarningLine, ProjectileType<Content.Projectiles.WarningLine>() },
                { SilveryDashHostile, ProjectileType<Content.Projectiles.Hostile.SilveryDashHostile>() },
                { FireBall, ProjectileType<Content.Projectiles.FireGrimoire.FireBall>() },
                { FireBallExplosion, ProjectileType<Content.Projectiles.FireGrimoire.FireBallExplosion>() },
                { DentateIce, ProjectileType<Content.Projectiles.IceTooth.DentateIce>() },
                { DentateIceExplosion, ProjectileType<Content.Projectiles.IceTooth.DentateIceExplosion>() },
                { FrostWave, ProjectileType<Content.Projectiles.IceTooth.FrostWave>() },
                { LightningMissile, ProjectileType<Content.Projectiles.Superconductor.LightningMissile>() },
                { LightningExplosion, ProjectileType<Content.Projectiles.Superconductor.LightningExplosion>() },
                { OverloadedLightningMissile, ProjectileType<Content.Projectiles.Superconductor.OverloadedLightningMissile>() },
                { OverloadedLightningExplosion, ProjectileType<Content.Projectiles.Superconductor.OverloadedLightningExplosion>() },
                { LightningChain, ProjectileType<Content.Projectiles.Superconductor.LightningChain>() },
                { SilveryArrow, ProjectileType<Content.Projectiles.ArgenteusClaw.SilveryArrow>() },
                { SanctifiedArrow, ProjectileType<Content.Projectiles.ArgenteusClaw.SanctifiedArrow>() },
                { SanctifiedArrowShoot, ProjectileType<Content.Projectiles.ArgenteusClaw.SanctifiedArrowShoot>() },
                { SanctifiedExplosion, ProjectileType<Content.Projectiles.ArgenteusClaw.SanctifiedExplosion>() },
                { MagicRock, ProjectileType<Content.Projectiles.TheLightOfAuzzsia.MagicRock>() },
                { ShockWave, ProjectileType<Content.Projectiles.Hostile.ShockWave>() },
                { GiantBranch, ProjectileType<Content.Projectiles.Hostile.GiantBranch>() },
                { GenericExplosion, ProjectileType<Content.Projectiles.GenericExplosion>() },
                { GenericSlash, ProjectileType<Content.Projectiles.GenericSlash>() },
                { GenericSlashHit, ProjectileType<Content.Projectiles.GenericSlashHit>() },
                { ShaderSlash, ProjectileType<Content.Projectiles.ShaderSlash>() },
                { SilveryField, ProjectileType<Content.Projectiles.ArgenteusOathKeeper.SilveryField>() },
                { SilveryDash, ProjectileType<Content.Projectiles.ArgenteusOathKeeper.SilveryDash>() },
                { AnattaSlash, ProjectileType<Content.Projectiles.AnattaSlash>() },
                { SolarHalo, ProjectileType<Content.Projectiles.Remakes.SolarHalo>() },
                { FireFlow, ProjectileType<Content.Projectiles.Remakes.FireFlow>() },
                { PhantomExcalibur, ProjectileType<Content.Projectiles.Remakes.PhantomExcalibur>() },
                { VoidNightsEdge, ProjectileType<Content.Projectiles.Remakes.VoidNightsEdge>() },
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
            public static Dictionary<string, short> Collection
            {
                get
                {
                    Dictionary<string, short> dict = new Dictionary<string, short>();
                    foreach (var member in typeof(CNCID.Tile).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, (short)member.GetRawConstantValue());
                    return dict;
                }
            }

            public static Dictionary<string, int> LegacyCollection
            {
                get
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (var member in typeof(CNCID.Tile).GetFields())
                        if (member.FieldType == typeof(short)) dict.Add(member.Name, LegacyObject[Collection[member.Name]]);
                    return dict;
                }
            }

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