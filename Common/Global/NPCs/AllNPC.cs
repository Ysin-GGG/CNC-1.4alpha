
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
        public int sanctifiedLevel = 0;
        public int lastFrame = 0;

        public int lifeRegenCount;
        public int lifeRegen;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            for (int i = 0; i < buff.Length; i++) buff[i] = false;

            if (lifeRegen > 0) { lifeRegenCount = 0; }
            lifeRegen = 0;
            if (sanctifiedLevel < 0) sanctifiedLevel = MyMath.Clamp(sanctifiedLevel + 1, -2, 0);
        }

        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.GetGlobalNPC<AllNPC>().buff[CNCID.Buff.Glacier] && !npc.boss) npc.frameCounter = 0;
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.GetGlobalNPC<AllNPC>().buff[CNCID.Buff.Glacier] && !npc.boss)
            {
                npc.position = npc.oldPosition.ToPoint().ToVector2();
                npc.velocity = npc.oldVelocity.ToPoint().ToVector2();
                npc.CheckActive();
                npc.checkDead();
                return false;
            }
            return base.PreAI(npc);
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.GetGlobalNPC<AllNPC>().buff[CNCID.Buff.Glacier] && !npc.boss)
            {
                Texture2D frozen = Terraria.GameContent.TextureAssets.Frozen.Value;
                float scale = 1f;
                Vector2 size = npc.frame.Size();
                if (size.X > frozen.Width * scale - 16f) scale = size.X / (frozen.Width * scale - 16f);
                if (size.Y > frozen.Height * scale - 16f) scale = size.Y / (frozen.Height * scale - 16f);
                Vector2 pos = npc.Center - frozen.Size() * scale / 2f - Main.screenPosition;
                Main.spriteBatch.Draw(frozen, pos, null, Color.White * .5f, 0, Vector2.Zero, scale, 0, 0);
            }
        }
    }
}