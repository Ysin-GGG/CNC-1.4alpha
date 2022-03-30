
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.IceTooth
{
    public class DentateIceExplosion : CNCProjectile
    {
        public int level => data.ContainsKey("level") ? (int)data["level"] : 0;
        public int target => data.ContainsKey("target") ? (int)data["target"] : -1;
        private int stage = 0;
        private const int tpf = 3;//Tick Per Frame

        public override int countFrame => 7;

        public override void ResetDefaults()
        {
            data["level"] = 0;
            data["target"] = -1;
            projectile.width = 58;
            projectile.height = 50;
            projectile.timeLeft = 45;
            projectile.knockBack = 0f;
            projectile.alpha = 0;
            projectile.scale = 1f;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int buff1 = CNCID.Buff.LegacyObject[CNCID.Buff.Gelid];
            int buff2 = CNCID.Buff.LegacyObject[CNCID.Buff.Glacier];
            if (target.HasBuff(buff1) && Main.rand.Next(9) == 0 && level > 1)
            {
                target.AddBuff(buff1, 0);
                target.AddBuff(buff2, 90);
            }
            else target.AddBuff(buff1, 300);
        }

        public override void AI()
        {
            if (this.stage == 0)
            {
                if (this.target >= 0) projectile.Center = Main.npc[target].Center;
                for (int i = 0; i < 3; i++)
                {
                    int idx = Dust.NewDust(projectile.Center, 1, 1, DustID.IceTorch, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 100, Color.LightBlue, 1.5f);
                    Main.dust[idx].noGravity = true;
                    Main.dust[idx].velocity *= 3f;
                }
                projectile.alpha = 255;
                projectile.scale = 0f;
                if (projectile.timeLeft <= 1 || this.target < 0 || (this.target >= 0 && !Main.npc[this.target].active))
                {
                    projectile.friendly = true;
                    projectile.timeLeft = tpf * countFrame;
                    projectile.alpha = 0;
                    projectile.scale = 1.2f;
                    this.stage = 1;
                    SoundEngine.PlaySound(SoundID.Item107, projectile.Center);
                    CNCUtils.PlaySound("IceTooth/Hit", projectile.position);
                }
            }
            else
            {
                Anim001_Loop(projectile, tpf);
                projectile.friendly = false;
                projectile.scale += .03f;
                if (projectile.timeLeft < 8) projectile.alpha = 64 + (8 - projectile.timeLeft) * 24;
                Dust dust = Dust.NewDustDirect(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.IceTorch, 1f * (Main.rand.Next(2) - 1), 1f * (Main.rand.Next(2) - 1), 128, Color.LightBlue, 2f);
                dust.noGravity = true;
                dust.velocity *= 8f;
                dust.scale -= .01f;
                /*Ñ©»¨Á£×Ó
                int idx = Particle.NewParticle(
                             ModContent.Request<Texture2D>("CNC/Assets/Effects/Snowflake" + Main.rand.Next(1, 5).ToString()).Value,
                             projectile.Center,
                             new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, 1f)),
                             MyMath.deg01 * Main.rand.Next(359),
                             50,
                             Vector2.One * .09f,
                             new Color(112, 196, 255, 0),
                             new Color(0, 170, 255, 5),
                             false
                             );
                ParticleRenderer.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0015f, .0025f));
                ParticleRenderer.particle[idx].rotationAdd = Main.rand.NextFloat(0, MyMath.deg30 / 10f);
                */
            }
        }
    }
}