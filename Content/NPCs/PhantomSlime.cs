using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CNC.Common.Localization;
using CNC.Common.Global.NPCs;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.NPCs
{
    public class PhantomSlime : CNCNPC
    {
        int var = 0;

        public override void ResetStaticDefaults()
        {
            countFrame = 2;
        }

        public override void ResetDefaults()
        {
            npc.width = 32;
            npc.height = 24;
            npc.aiStyle = 1;
            npc.knockBackResist = .1f;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 55;
            npc.value = 5f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            AnimationType = 1;
            var = 0;
        }

        public override void AI()
        {
            base.AI();
            var = (var + 8) % 512;
            if (var < 256) npc.alpha = var;
            else npc.alpha = 512 - var;
            npc.alpha = (int)(npc.alpha * .75);
        }

        public override void ResetBestiary(ref BestiaryDatabase database, ref BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] { BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface });
        }
    }
}
