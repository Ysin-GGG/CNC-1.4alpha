
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using CNC.Common.Utils;

namespace CNC.Common.Global.NPCs
{
    public class AllNPC : GlobalNPC
    {
        public bool[] buff = new bool[CNCID.Buff.MaxObject];
        public int SanctifiedLevel;
        public int lastFrame = 0;

        public int lifeRegenCount;
        public int lifeRegen;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            for (int i = 0; i < buff.Length; i++) buff[i] = false;

            if (lifeRegen > 0) { lifeRegenCount = 0; }
            lifeRegen = 0;
        }

        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.GetGlobalNPC<AllNPC>().buff[CNCID.Buff.Glacier]) npc.frameCounter = 0;
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.GetGlobalNPC<AllNPC>().buff[CNCID.Buff.Glacier])
            {
                npc.position = npc.oldPosition;
                //npc.velocity.X = 0;
                //npc.noGravity = false;
                //npc.color = new Color(96, 129, 192);
                return false;
            }
            return base.PreAI(npc);
        }
    }
}