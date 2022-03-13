using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.BigProgressBar;
using CNC.Common.Utils;
using CNC.Common.Global.NPCs;

namespace CNC.Content.NPCs.SpiritBoss
{
    public class SpiritCrystal : CNCNPC
    {
        private Color mainColor => new Color(178, 214, 231, 0);
        private float rot = 0;
        private int[] alpha1 = { 0, 1, 0, 0, 255, 0, 50, 5 };
        private int[] scale1 = { 0, 1, 0, 0, 100, 0, 50, 2 };
        private int[] alpha2 = { 0, 1, 0, 255, 510, 0, 0, 3 };
        private int[] scale2 = { 0, 1, 0, 50, 100, 50, 50, 2 };

        public override void ResetStaticDefaults()
        {
            countFrame = 1;
            NPCID.Sets.NeedsExpertScaling[Type] = true;
        }

        public override void ResetDefaults()
        {
            npc.width = 64;
            npc.height = 68;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;
            npc.damage = 0;
            npc.defense = 15;
            npc.lifeMax = 1000;
            npc.alpha = 255;
            npc.npcSlots = 10f;
            npc.noGravity = true;
            npc.dontTakeDamageFromHostiles = true;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.SpawnWithHigherTime(60);
        }

        public override void AI()
        {
            NPC parent = RootNPC;
            if (parent == null || !parent.active || parent.type != CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss]) npc.active = false;
            else if (((SpiritBoss)parent.ModNPC).state == 10)
            {
                if (npc.alpha < 253)
                {
                    npc.alpha += 3;
                    npc.position.Y -= .6f;
                }
                else npc.active = false;
            }
            else if (npc.alpha >= 5)
            {
                npc.alpha -= 5;
                npc.position.Y += 1f;
            }
            Vector2 pos = (npc.position + npc.Size / 2f) / 16f;
            float r = mainColor.R / 255f;
            float g = mainColor.G / 255f;
            float b = mainColor.B / 255f;
            Lighting.AddLight((int)pos.X, (int)pos.Y, r, g, b);
        } 

        public override void ResetBestiary(ref BestiaryDatabase database, ref BestiaryEntry bestiaryEntry)
        {
        }

        public override void ModifyHitBy(Player attacker, Entity entity, ref int damage, ref float knockback, ref bool crit, bool isItem)
        {
            if (((SpiritBoss)RootNPC.ModNPC).rage) damage = (int)(damage * .1f);
        }

        public override bool CheckActive() => !npc.active;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            rot += MyMath.deg01;
            CNCUtils.UpdateEffectVar(ref alpha1);
            CNCUtils.UpdateEffectVar(ref scale1);
            CNCUtils.UpdateEffectVar(ref alpha2);
            CNCUtils.UpdateEffectVar(ref scale2);
            Texture2D Aurora = ModContent.Request<Texture2D>("CNC/Content/NPCs/SpiritBoss/Aurora").Value;
            Texture2D Glow = ModContent.Request<Texture2D>("CNC/Content/NPCs/SpiritBoss/SpiritCrystalGlow").Value;
            Texture2D Light = ModContent.Request<Texture2D>("CNC/Content/NPCs/SpiritBoss/Light").Value;
            Vector2 pos = npc.Center - screenPos + new Vector2(0, 4);
            Main.EntitySpriteDraw(Glow, pos, null, mainColor * (alpha1[0] / 255f) * (1 - npc.alpha / 255f), 0, Glow.Size() / 2, 1 + scale1[0] * .003f, 0, 1);
            Main.EntitySpriteDraw(Light, pos, null, new Color(255, 255, 255, 0) * (alpha2[0] / 255f / 3f) * (1 - npc.alpha / 255f), rot, Light.Size() / 2, .75f + scale2[0] * .001f, 0, 1);
            return true;
        }
    }
}