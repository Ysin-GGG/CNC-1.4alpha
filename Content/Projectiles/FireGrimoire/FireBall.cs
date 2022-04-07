using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using System.Collections.Generic;

namespace CNC.Content.Projectiles.FireGrimoire
{
    public class FireBall : CNCProjectile
    {
        public int level => data.ContainsKey("level") ? (int)data["level"] : 0;
        public override int countFrame => 7;
        private const int tpf = 4;//Time Per Frame
        private const int lenMax = 17;
        private int len = 0;
        private int pointX = 0;

        public override void ResetDefaults()
        {
            data["level"] = 0;
            projectile.width = 14;
            projectile.height = 14;
            projectile.timeLeft = 600;
            projectile.knockBack = 2f;
            projectile.alpha = 0;
            projectile.scale = 1.5f;
            projectile.penetrate = 1;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ignoreWater = false;
            projectile.DamageType = DamageClass.Magic;
            if (projectile.oldPos.Length != lenMax)
            {
                Array.Resize(ref projectile.oldPos, lenMax);
                Array.Resize(ref projectile.oldRot, lenMax);
            }
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                projectile.oldRot[i] = 0.0f;
                projectile.oldPos[i].X = 0.0f;
                projectile.oldPos[i].Y = 0.0f;
            }
        }

        public override void AI()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            Anim001_Loop(projectile, tpf);
            UpdateOldData(projectile, projectile.velocity);
            projectile.velocity = projectile.oldVelocity;
            if (len < lenMax) len++;
            if (projectile.timeLeft < 597)
            {
                CNCUtils.AddLight(projectile.position, new Color(242, 92, 34), .8f);
                for (int i = 0; i < 2; i++)
                {
                    int idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Light1").Value,
                        projectile.Center + new Vector2(Main.rand.NextFloat(-1f, 1f) * Main.rand.NextFloat(-1f, 1f)) * projectile.Size * projectile.scale / 2f,
                        -new Vector2(projectile.velocity.X, 6f) * Main.rand.NextFloat(.05f, .1f),
                        0f,
                        100,
                        new Vector2(Main.rand.NextFloat(.03f, .09f)),
                        new Color(242, 92, 34, 0),
                        new Color(242, 92, 34, 25)
                        );
                    ParticleRenderer.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0003f, .0010f));
                    ParticleRenderer.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
                }
            }
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 300);
            if (this.level > 0) target.AddBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Scorched], 300);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < len; i++)
            {
                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Light1").Value,
                    projectile.oldPos[i] + projectile.Size / 2f + new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * projectile.Size * projectile.scale / 2f,
                    -new Vector2(projectile.velocity.X, 6f) * Main.rand.NextFloat(.05f, .1f),
                    0f,
                    100,
                    new Vector2(Main.rand.NextFloat(.06f, .12f)) * ((len - i) / (len + 1f) * .5f + .5f),
                    new Color(242, 92, 34, 0),
                    new Color(242, 92, 34, 25)
                    );
                ParticleRenderer.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0005f, .0012f));
                ParticleRenderer.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
            }
            Projectile proj = this.NewProjectile(0f, 0f, CNCID.Projectile.LegacyObject[CNCID.Projectile.FireBallExplosion], Inscription.JustGetDamage(projectile.damage, (string)CNCID.Inscription.S_Incinerating.Data[0]), projectile.knockBack, MyMath.deg01 * Main.rand.Next(0, 359));
            proj.GetGlobalProjectile<AllProjectile>().data["level"] = level;
            proj.owner = this.projectile.owner;
            SoundEngine.PlaySound(SoundID.Item100, projectile.position);
        }

        public override bool PreDrawExtras()
        {
            Texture2D glowTrail = ModContent.Request<Texture2D>("CNC/Assets/Effects/GlowTrail1").Value;
            Texture2D trail = ModContent.Request<Texture2D>("CNC/Assets/Effects/Trail3").Value;
            Color color = new Color(242, 92, 34, 0);
            //Main.spriteBatch.Draw(light, projectile.Center - light.Size() / 2f * .5f - Main.screenPosition, null, color, 0, default, .5f, 0, 0f);
            Drawing.DrawTrailDirect(Main.spriteBatch, glowTrail, len, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, color, pointX, glowTrail.Height / 2, new Vector2(1, .05f), new Vector2(1, .05f), true, 16, 1);
            Drawing.DrawTrailDirect(Main.spriteBatch, trail, Math.Max(0, len - 2), projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, color, pointX, trail.Height / 2, new Vector2(1, .19f), new Vector2(1, .05f), true, 16, 2);
            //Drawing.DrawTrail(Main.spriteBatch, trail, Math.Max(0, len - 2), projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, color, pointX, trail.Height / 2, new Vector2(.15f), 16);
            pointX = (pointX - 16 + trail.Width) % trail.Width;
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }
    }
}