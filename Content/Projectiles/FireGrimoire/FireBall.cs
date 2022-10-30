using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
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
        private const int lenMax = 15;
        private int len = 0;
        private int pointX = 0;

        public override void ResetDefaults()
        {
            data["level"] = 0;
            projectile.width = 14;
            projectile.height = 14;
            projectile.timeLeft = 600;
            projectile.knockBack = 2f;
            projectile.alpha = 255;
            projectile.scale = 1.5f;
            projectile.penetrate = 1;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ignoreWater = false;
            projectile.DamageType = DamageClass.Magic;
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = lenMax;
            
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
            projectile.velocity = Vector2.Normalize(projectile.oldVelocity) * MyMath.Lerp(8f, 20f, this.Alpha2);
            if (this.Alpha2 < 1f) this.Alpha2 = MyMath.Clamp(this.Alpha2 + .05f, 0f, 1f);
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
                    ParticleManager.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0003f, .0010f));
                    ParticleManager.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
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
                ParticleManager.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0005f, .0012f));
                ParticleManager.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
            }
            Projectile proj = this.NewProjectile(0f, 0f, CNCID.Projectile.LegacyObject[CNCID.Projectile.FireBallExplosion], Inscription.JustGetDamage(projectile.damage, (string)CNCID.Inscription.S_Incinerating.Data[0]), projectile.knockBack, CNC.Random.NextFloat(MyMath.pi2), 0f, 1f, 0f, 0f);
            proj.GetGlobalProjectile<AllProjectile>().data["level"] = level;
            proj.owner = this.projectile.owner;
            SoundEngine.PlaySound(SoundID.Item100, projectile.position);
        }

        public override bool PreDrawExtras()
        {
            Texture2D[] image =
            {
                CNC.GetTexture("Assets/Effects/GlowTrail2"),
                CNC.GetTexture("Assets/Effects/Trace7"),
                CNC.GetTexture("Assets/Effects/Trail/Trail3"),
            };
            Texture2D[] image2 =
            {
                CNC.GetTexture("Assets/Effects/GlowTrail2"),
                CNC.GetTexture("Assets/Effects/Trace7"),
                CNC.GetTexture("Assets/Effects/Trail/Trail5"),
            };
            Color[] color =
            {
                Color.Red,
                new Color(242, 92, 34),
                Color.White,
                Color.White,
            };
            Drawing.DrawTrail(Main.spriteBatch, projectile, image, color, 20, 8, false, .5d, 2d, 1d);
            Drawing.DrawTrail(Main.spriteBatch, projectile, image2, color, 56, 56, true, 1d, 2d, 0d);
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }
    }
}