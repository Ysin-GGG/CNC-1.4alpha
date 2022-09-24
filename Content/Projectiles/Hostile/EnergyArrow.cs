
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public class EnergyArrow : CNCProjectile
    {
        private float alpha => 1f - projectile.alpha / 255f;
        private float lineAlpha = 0f;
        private float lightAlpha;
        private float lightRotation;
        private float lightDirection;
        private Vector2 lightScale;
        public bool rage { get => projectile.ai[0] != 0; set => projectile.ai[0] = value ? 3 : 0; }

        public override void ResetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.timeLeft = 500;
            projectile.knockBack = 8f;
            projectile.alpha = 255;
            projectile.light = 0;
            projectile.penetrate = -1;
            projectile.aiStyle = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Color color = CNCID.Colors.SpiritBoss;
            CNCUtils.AddLight(projectile.position, color);
            color.A = 0;
            frameTick++;
            if (alpha < 1f)
            {
                projectile.alpha = MyMath.Max(projectile.alpha - 32, 0);
            }
            if (projectile.timeLeft > 350)
            {
                if (rage)
                {
                    if (400 < projectile.timeLeft && projectile.timeLeft <= 450) lineAlpha = MyMath.Min(lineAlpha + .04f, 1f);
                    else lineAlpha = MyMath.Max(lineAlpha - .03f, 0f);
                    if (frameTick <= 50) projectile.rotation = projectile.velocity.ToRotation() - MyMath.deg90 + MyMath.pi * (1 - MyMath.cos(MyMath.pi * frameTick / 50f)) * projectile.direction;
                    else projectile.extraUpdates = 2;
                }
                else
                {
                    if (projectile.timeLeft > 400) lineAlpha = MyMath.Min(lineAlpha + .04f, 1f);
                    else lineAlpha = MyMath.Max(lineAlpha - .03f, 0f);
                    projectile.rotation = projectile.velocity.ToRotation() - MyMath.deg90;
                    projectile.extraUpdates = 1;
                }
                projectile.velocity *= .95f;
                if (projectile.timeLeft % 6 == 0)
                {
                    int idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                        projectile.Center,
                        CNC.Random.NextVector2(-3f, -2f, 3f, -1f) - projectile.velocity,
                        CNC.Random.NextDegree(),
                        20,
                        Vector2.One,
                        color
                        );
                    ParticleRenderer.particle[idx].velocityMul = CNC.Random.NextVector2(.9f, 1f, .95f, 1f);
                    ParticleRenderer.particle[idx].velocityAdd = CNC.Random.NextVector2(0, -1.2f, 0, -1f);
                    ParticleRenderer.particle[idx].rotationAdd = CNC.Random.NextDegree(-8f, 8f);
                    ParticleRenderer.particle[idx].scaleAdd = Vector2.One * CNC.Random.NextFloat(-.05f);
                }
            }
            else
            {
                if (projectile.timeLeft == 350) CNCUtils.PlaySound(SoundID.Item66, projectile.Center);
                projectile.extraUpdates = 2 + (int)projectile.ai[0];
                projectile.hostile = true;
                projectile.velocity = -new Vector2(0, 16f).RotatedBy(this.projectile.rotation);
                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/GlowTrail").Value,
                    projectile.Center,
                    default,
                    projectile.velocity.ToRotation(),
                    20,
                    new Vector2(MyMath.GetZ(velX, velY) / ModContent.Request<Texture2D>("CNC/Assets/Particles/GlowTrail").Value.Width, .1f),
                    color,
                    new Color(color.R, color.G, color.B, 5)
                    );
                ParticleRenderer.particle[idx].scaleAdd.Y = -.005f;
                ParticleRenderer.particle[idx].alphaAdd = -.05f;
                ParticleRenderer.particle[idx].alpha = 1f;

                idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                    projectile.Center,
                    projectile.velocity,
                    CNC.Random.NextDegree(),
                    40,
                    Vector2.One,
                    color,
                    new Color(color.R, color.G, color.B, 5)
                    );
                ParticleRenderer.particle[idx].velocityMul = CNC.Random.NextVector2(.95f, .95f, .98f, .98f);
                ParticleRenderer.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                ParticleRenderer.particle[idx].scaleAdd = new Vector2(-.025f);
                ParticleRenderer.particle[idx].alphaAdd = -.025f;
                ParticleRenderer.particle[idx].alpha = 1f;
            }
        }

        public override void HitPlayer(Player target, int damage, bool crit)
        {
            CNCUtils.PlaySound("ArgenteusClaw/Hit", projectile.position, 2f);
            CNCUtils.ScreenShake(projectile.Center, 144f, 5, Vector2.Normalize(projectile.velocity) * 30f, Vector2.Normalize(projectile.velocity) * 15f, .8f);
            CNCUtils.SetBuff(target, CNCID.Buff.StarMark, 300);
            float max = 36f;
            for (int i = 0; i < max; i++)
            {
                float amount = i / max * MyMath.pi2;
                Vector2 center = target.Center;
                Vector2 vel = new Vector2(6f * MyMath.cos(amount), 2f * MyMath.sin(amount));
                CNCUtils.NewDust(center + vel, DustId.CyanShortFx1, MyMath.sin(amount) + 2.5f, vel).noGravity = true;
            }
            if (rage)
            {
                this.NewProjectile(target.Center, Const._02, CNCID.Projectile.LegacyObject[CNCID.Projectile.EnergyExplosion], 0, 0);
                target.velocity = target.position - projectile.position + projectile.velocity;
                if (target.velocity.Y < 0) target.velocity.Y *= -1;
                target.velocity.X = MyMath.Clamp(target.velocity.X, -16f, 16f);
                target.velocity.Y = MyMath.Clamp(target.velocity.Y, 0, 8f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = CNCID.Colors.SpiritBoss * alpha;
            Color themeColor = lightColor;
            Texture2D line = CNC.GetTexture("Assets/Glow/WarningLine");
            Vector2 pos = projectile.Center - Main.screenPosition;
            if (this.lineAlpha > 0f) Main.EntitySpriteDraw(line, pos, null, themeColor * lineAlpha, projectile.rotation, line.Bottom(), new Vector2(1f, 3f), 0, 0);
            return true;
        }

        public override bool PreDrawExtras()
        {
            Vector2 pos = projectile.Center - Main.screenPosition;
            Color color = CNCUtils.FlashColor(CNCID.Colors.SpiritBoss * alpha).NewA(0);
            Texture2D[] glow =
            {
                Mod.Assets.Request<Texture2D>("Assets/Glow/Lamp").Value,
                Mod.Assets.Request<Texture2D>("Assets/Glow/Teleport").Value,
            };
            Main.EntitySpriteDraw(glow[0], pos, null, color, 0f, glow[0].Size() / 2f, Vector2.One / 4f, 0, 0);
            if (this.lightAlpha > 0f)
            {
                Main.EntitySpriteDraw(glow[1], pos, null, color * lightAlpha, lightRotation, glow[1].Size() / 2f, lightScale, 0, 0);
                this.lightAlpha -= .025f;
                this.lightRotation += MyMath.deg01 * lightDirection * 2f;
                this.lightScale += new Vector2(-1f, 1f) / 40f;
            }
            else if (CNC.Random.NextBool(.001))
            {
                this.lightAlpha = 1f;
                this.lightRotation = CNC.Random.NextDegree();
                this.lightDirection = CNC.Random.NextBool().ToDirectionInt();
                this.lightScale = new Vector2(1f, 0f);
            }
            return true;
        }
    }
}
