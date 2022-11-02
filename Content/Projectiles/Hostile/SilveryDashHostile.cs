using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public class SilveryDashHostile : CNCProjectile
    {
        public override int countFrame => 5;
        private float time = 45;
        private int tpf = 9;
        private float rot = 0;

        public bool rage { get => projectile.ai[0] != 0; set => projectile.ai[0] = value.ToInt(0); }

        public override void ResetDefaults()
        {
            projectile.timeLeft = (int)time;
            projectile.knockBack = 2f;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Default;
        }

        public override void UpdateAI()
        {
            if (rage)
            {
                projectile.extraUpdates = 2;
                Anim001_Loop(projectile, tpf);
                projectile.Center = ParentNPC.Center;
                projectile.rotation = projectile.velocity.ToRotation();
                ParentNPC.velocity = projectile.velocity;
                Color color = CNCID.Colors.SpiritBoss.NewA(0);
                Vector2 pos = projectile.Center + projectile.velocity;
                float max = 8f;
                for (int i = 0; i < max; i++)
                {
                    int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                    pos - projectile.velocity * i / max,
                    projectile.velocity * .1f,
                    CNC.Random.NextDegree(),
                    40,
                    Vector2.One,
                    color,
                    new Color(color.R, color.G, color.B, 5)
                    );
                    ParticleManager.particle[idx].velocityMul = CNC.Random.NextFloat(.95f, .98f) * Vector2.One;
                    ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                    ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                    ParticleManager.particle[idx].alphaAdd = -.025f;
                    ParticleManager.particle[idx].alpha = 1f;
                }
            }
            else
            {
                if (tpf > 4) tpf = 4;
                if (time > 20) time = 20;
                if (projectile.timeLeft > time) projectile.timeLeft = (int)time;
                //projectile.extraUpdates = 1;
                Anim001_Loop(projectile, tpf);
                projectile.Center = ParentNPC.Center;
                projectile.rotation = projectile.velocity.ToRotation();
                ParentNPC.velocity = projectile.velocity;
                if (projectile.timeLeft == time) CNCUtils.PlaySound(SoundID.Item28, projectile);
                Color color = CNCID.Colors.SpiritBoss.NewA(0);
                float max = 8f;
                for (int i = 0; i < max; i++)
                {
                    Vector2 pos = projectile.Center + projectile.velocity * (time - projectile.timeLeft) / 2f;
                    int idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                        pos - projectile.velocity * 1.5f * i / max,
                        projectile.velocity * .1f,
                        CNC.Random.NextDegree(),
                        40,
                        Vector2.One,
                        color,
                        new Color(color.R, color.G, color.B, 5)
                        );
                    ParticleManager.particle[idx].velocityMul = CNC.Random.NextFloat(.95f, .98f) * Vector2.One;
                    ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                    ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                    ParticleManager.particle[idx].alphaAdd = -.025f;
                    ParticleManager.particle[idx].alpha = 1f;

                    idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                        pos - projectile.velocity * 1.5f * (1 - i / max) - new Vector2(32f, 0f).RotatedBy(rot += MyMath.deg01 * 54f / max) * projectile.direction,
                        projectile.velocity * .1f,
                        CNC.Random.NextDegree(),
                        40,
                        Vector2.One * .75f,
                        color,
                        new Color(color.R, color.G, color.B, 5)
                        );
                    ParticleManager.particle[idx].velocityMul = CNC.Random.NextFloat(.95f, .98f) * Vector2.One;
                    ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                    ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                    ParticleManager.particle[idx].alphaAdd = -.025f;
                    ParticleManager.particle[idx].alpha = 1f;
                }
                CNCUtils.AddLight(projectile.position, CNCID.Colors.SpiritBoss, 1.2f);
            }
        }

        public override bool ModifyKill(int timeLeft)
        {
            if (CNC.NotMPClient) ParentNPC.velocity *= 0f;
            return base.ModifyKill(timeLeft);
        }

        public override void OnKill(int timeLeft)
        {
            if (CNC.NotMPClient) ParentNPC.velocity *= 0f;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 16;
            hitbox.X = (int)projectile.Center.X - size;
            hitbox.Y = (int)projectile.Center.Y - size;
            hitbox.Width = size * 2;
            hitbox.Height = size * 2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < 9; i++) if (targetHitbox.Contains((projectile.Center + projectile.velocity * i / 2f).ToPoint())) return true;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void OnHit(Entity target, int damage, float knockback, bool crit)
        {
            var hit = CNCUtils.CreateSlashHit(projectile, Vector2.Zero);
            hit.SetDefaults(mod.Assets.Request<Texture2D>("Assets/Projectiles/SlashHit/SlashHit1").Value, new Vector2(MyMath.phi - 1f, 1f) * .6f, target.Center, 15, projectile.velocity.ToRotation() + MyMath.deg30, 30);
            hit.SetColor(CNCID.Colors.SpiritBoss2, CNCID.Colors.SpiritBoss2, .1f);
            hit = CNCUtils.CreateSlashHit(projectile, Vector2.Zero);
            hit.SetDefaults(mod.Assets.Request<Texture2D>("Assets/Projectiles/SlashHit/SlashHit1").Value, new Vector2(MyMath.phi - 1f, 1f) * .6f, target.Center, 15, projectile.velocity.ToRotation() - MyMath.deg30, 30);
            hit.SetColor(CNCID.Colors.SpiritBoss2, CNCID.Colors.SpiritBoss2, .1f);

            CNCUtils.PlaySound("ArgenteusClaw/Hit", projectile.position, 2f);
            CNCUtils.ScreenShake(projectile.Center, 144f, 5, Vector2.Normalize(projectile.velocity) * 30f, Vector2.Normalize(projectile.velocity) * 15f, .8f);

            target.velocity = projectile.velocity / 4f;
            target.velocity.X = MyMath.Clamp(target.velocity.X, -16f, 16f);
            target.velocity.Y = MyMath.Clamp(target.velocity.Y, -16f, 16f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = CNCID.Colors.SpiritBoss;
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            lightColor.A = 0;
            Texture2D image = mod.Assets.Request<Texture2D>("Assets/Glow/Refringence").Value;
            Vector2 origin = image.Size() / 2;
            float alpha = MyMath.sin(projectile.timeLeft / time * MyMath.pi);
            Main.EntitySpriteDraw(image, this.ParentNPC.Center - Main.screenPosition, null, lightColor * alpha, projectile.rotation, origin, (2f + projectile.ai[0] * 2) * alpha, 0, 0);
        }
    }
}
