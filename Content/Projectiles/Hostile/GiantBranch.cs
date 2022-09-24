using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public class GiantBranch : CNCProjectile
    {
        private const int dustid1 = DustId.BrownDirt;
        private const int dustid2 = DustId.LightGreyStone;
        private bool rand;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).Rage;
        public override int countFrame => 9; 

        public override void ResetDefaults()
        {
            projectile.width = 90;
            projectile.height = 194;
            projectile.timeLeft = 40;
            projectile.alpha = 0;
            projectile.light = 0;
            projectile.scale = 1;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D image = rand ? CNC.GetTexture(Texture + 2) : Texture2;
            int height = image.Height / this.countFrame;
            Rectangle source = new Rectangle(0, this.frame * height, image.Width, height);
            Vector2 pos = projectile.Bottom - Main.screenPosition + new Vector2(0f, 8f);
            Vector2 origin = new Vector2(image.Width / 2f, height);
            Main.EntitySpriteDraw(image, pos, source, lightColor * Alpha2, 0f, origin, new Vector2(1f, projectile.scale), (SpriteEffects)(int)projectile.ai[1], 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float distX = Math.Abs(targetHitbox.Center.X - projectile.Center.X);
            float distY = Math.Abs(projectile.Bottom.Y - targetHitbox.Bottom);
            return targetHitbox.Bottom < projectile.Bottom.Y && distX < projectile.width / 2f && distY < projectile.height * projectile.scale - distX;
        }

        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
            if(!projectile.hostile && this.frame == 0 && this.Alpha1 == 0)
            {
                if (projectile.timeLeft == 1)
                {
                    rand = CNC.Random.NextBool();
                    projectile.ai[1] = CNC.Random.NextInt(2);
                    projectile.scale += CNC.Random.NextFloat(.75f);
                    projectile.timeLeft = 40;
                    projectile.hostile = true;
                }
                else for (int i = 0; i < 1; i++)
                    {
                        Vector4 xywh = new Vector4(projectile.BottomLeft.X, projectile.BottomLeft.Y - 20, projectile.width, 20);
                        CNCUtils.NewDust(xywh, dustid1, 0f, 1f, default, default(Color));
                        CNCUtils.NewDust(xywh, dustid2, 0f, 1f, default, default(Color));
                    }
            }
            else
            {
                projectile.timeLeft = 2;
                if (projectile.hostile)
                {
                    projectile.alpha = 0;
                    if (frameTick++ > 1)
                    {
                        if (frame++ >= countFrame) frame--;
                        if (frame == countFrame - 1) projectile.hostile = false;
                        frameTick = 0;
                    }
                }
                else
                {
                    if (frameTick++ > 1)
                    {
                        if (frame-- > countFrame) frame--;
                        frameTick = 0;
                        if (frame == 0)
                        {
                            if (Alpha1 < 1) Alpha1 += .025f;
                            else projectile.Kill();
                        } 
                    }
                }
            }
        }

        public override void HitPlayer(Player target, int damage, bool crit)
        {
            target.velocity.Y = MyMath.Max(target.velocity.Y - 16f, -16f);
        }
    }
}