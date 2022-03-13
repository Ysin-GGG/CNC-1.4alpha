using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CNC.Common.Localization;
using CNC.Common.Global.NPCs;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.NPCs
{
    public class PhantomBat : CNCNPC
    {
        int var = 0;

        public override void ResetStaticDefaults()
        {
            countFrame = 5;
        }

        public override void ResetDefaults()
        {
            npc.width = 44;
            npc.height = 30;
            npc.aiStyle = 14;
            npc.knockBackResist = .2f;
            npc.damage = 9;
            npc.defense = 3;
            npc.lifeMax = 25;
            npc.value = 25f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            AnimationType = 49;
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
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] { BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground });
        }
    }
}
