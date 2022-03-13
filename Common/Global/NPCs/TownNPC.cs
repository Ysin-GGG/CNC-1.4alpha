using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CNC.Common.Localization;

namespace CNC.Common.Global.NPCs
{
    public abstract class TownNPC : ModNPC
    {
        public enum Facing { left = -1, right = 1 }
        public NPC npc => NPC;
        public int countFrame { get => Main.npcFrameCount[Type]; set => Main.npcFrameCount[Type] = value; }
        public int attackFrame { get => NPCID.Sets.AttackFrameCount[Type]; set => NPCID.Sets.AttackFrameCount[Type] = value; }
        public int extraFrame { get => NPCID.Sets.ExtraFramesCount[Type]; set => NPCID.Sets.ExtraFramesCount[Type] = value; }
        public int attackType { get => NPCID.Sets.AttackType[Type]; set => NPCID.Sets.AttackType[Type] = value; }
        public int attackTime { get => NPCID.Sets.AttackTime[Type]; set => NPCID.Sets.AttackTime[Type] = value; }

        public sealed override void SetStaticDefaults()
        {
            //NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            //NPCID.Sets.AttackType[Type] = 0;
            //NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            //NPCID.Sets.AttackAverageChance[Type] = 30;
            //NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
            
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.TownNPCBestiaryPriority.Add(Type);
            DisplayName.SetDefault(CNCLocalization.GetText($"TownNPC.{Name}.Name", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"TownNPC.{Name}.Name", 7));
            ResetStaticDefaults();
        }

        public sealed override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.lifeMax = 250;
            npc.aiStyle = 7;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            ResetDefaults();
        }

        public sealed override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(CNCLocalization.GetText($"TownNPC.{Name}.Bestiary"))
            });
            ResetBestiary(ref database, ref bestiaryEntry);
        }

        public sealed override string TownNPCName()
        {
            List<string> nameList = new List<string>();
            int idx = 1;
            string key = $"Mods.CNC.TownNPCName.{Name}." + idx.ToString();
            string value = CNCLocalization.GetText(key);
            while (key != value)
            {
                nameList.Add(value);
                idx++;
                key = $"Mods.CNC.TownNPCName.{Name}." + idx.ToString();
                value = CNCLocalization.GetText(key);
            }
            NPCName(ref nameList);
            string[] name = nameList.ToArray();
            return name[WorldGen.genRand.Next(name.Length)];
        }

        public sealed override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            NPCChat(ref chat);
            return chat;
        }

        public sealed override void SetChatButtons(ref string button, ref string button2) {
            button = Language.GetTextValue("LegacyInterface.28");
            ChatButton(ref button, ref button2);
        }

        public sealed override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            shop = firstButton;
            OnClickChatButton(firstButton, ref shop);
        }

        public sealed override void TownNPCAttackStrength(ref int damage, ref float knockback) => AtkStrength(ref damage, ref knockback);

        public sealed override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) => AtkCooldown(ref cooldown, ref randExtraCooldown);

        public sealed override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) => AtkProjSpeed(ref multiplier, ref gravityCorrection, ref randomOffset);

        public sealed override bool CanGoToStatue(bool toKingStatue) => true;

        public sealed override void OnGoToStatue(bool toKingStatue) => StatueTeleport();

        // Create a square of pixels around the NPC on teleport.
        public void StatueTeleport()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 position = Main.rand.NextVector2Square(-20, 21);
                if (Math.Abs(position.X) > Math.Abs(position.Y))
                {
                    position.X = Math.Sign(position.X) * 20;
                }
                else
                {
                    position.Y = Math.Sign(position.Y) * 20;
                }
            }
        }
        public virtual void ResetDefaults() { }
        public virtual void ResetStaticDefaults() { }
        public virtual void ResetBestiary(ref BestiaryDatabase database, ref BestiaryEntry bestiaryEntry) { }
        public virtual void NPCName(ref List<string> name) { }
        public virtual void NPCChat(ref WeightedRandom<string> chat) { }
        public virtual void ChatButton(ref string left, ref string right) { }
        public virtual void OnClickChatButton(bool firstButton, ref bool shop) { }
        public virtual void AtkStrength(ref int dmg, ref float kb) { }
        public virtual void AtkCooldown(ref int CD, ref int randExtraCD) { }
        public virtual void AtkProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randOffset) { }
    }
}