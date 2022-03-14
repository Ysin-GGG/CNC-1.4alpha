using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.NPCs;

namespace CNC.Content.NPCs.TownNPCs
{
	[AutoloadHead]//载入npc在小地图上的头像贴图
	public class Mentor : TownNPC
	{
        public override void ResetStaticDefaults()
        {
            countFrame = 25;
            attackFrame = 1;
            extraFrame = 0;
            attackType = 2;
            attackTime = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.DangerDetectRange[Type] = 640; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.HatOffsetY[Type] = 0;

            npc.Happiness.SetBiomeAffection<Biomes.Tranquility>(AffectionLevel.Love);
            npc.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
            npc.Happiness.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike);
            npc.Happiness.SetNPCAffection(54, AffectionLevel.Like);
            npc.Happiness.SetNPCAffection(228, AffectionLevel.Dislike);
            npc.Happiness.SetNPCAffection(108, AffectionLevel.Hate);
        }

		public override void ResetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 23;
            npc.defense = 15;
            AnimationType = NPCID.Wizard;
        }

        public override void NPCChat(ref WeightedRandom<string> chat)
        {
            for (int i = 1; i < 10; i++)
               if (CNCLocalization.TextExist($"TownNPCChat.{Name}." + i.ToString()))
                    chat.Add(CNCLocalization.GetText($"TownNPCChat.{Name}." + i.ToString()));
        }

        public override void TownNPCAttackMagic(ref float auraLightMultiplier) => auraLightMultiplier = .5f;

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = CNCID.Projectile.LegacyObject[CNCID.Projectile.MentorMissile];
            attackDelay = 30;
        }

        public override void AtkStrength(ref int dmg, ref float kb)
        {
            dmg = 11;
            kb = .5f;
        }

        public override void AtkCooldown(ref int CD, ref int randExtraCD)
        {
            CD = 45;
            randExtraCD = 45;
        }
    }
}