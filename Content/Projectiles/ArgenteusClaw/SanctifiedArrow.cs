
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.ArgenteusClaw
{
    public class SanctifiedArrow : CNCProjectile
    {
        Vector2 dist;
        NPC npc;

        private int[] alpha = { 0, 1, 0, 0, 50, 0, 25, 5 };
        private int[] scale = { 0, 1, 0, 50, 100, 5, 5, 2 };

        public bool explosion => data.ContainsKey("explosion") ? (bool)this.data["explosion"] : false;

        public override void ResetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.timeLeft = 600;
            projectile.knockBack = 8f;
            projectile.alpha = 0;
            projectile.light = 0;
            projectile.penetrate = -1;
            projectile.aiStyle = 1;
            projectile.extraUpdates = 2;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            npc = null;
            dist = Const._02;
        }

        public override void HitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target != null && target.active)
            {
                CNCUtils.PlaySound("ArgenteusClaw/Hit", projectile.position, 2f);
                projectile.timeLeft = 1200;
                projectile.hostile =
                projectile.friendly =
                projectile.tileCollide = false;
                int _dmg = (crit ? 2 : 1) * damage;
                dist = (npc = (NPC)target).position - projectile.position;
                AllNPC modNPC = npc.GetGlobalNPC<AllNPC>();
                if (this.explosion)
                {
                    if (++modNPC.sanctifiedLevel > 2 || npc.life <= _dmg)
                    {
                        if (npc.life > _dmg) damage *= 2;
                        modNPC.sanctifiedLevel = -5;
                        this.NewProjectile(target.Center, Const._02, CNCID.Projectile.LegacyObject[CNCID.Projectile.SanctifiedExplosion], 0, 0);
                    }
                }
            }
        }

        public override void AI()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            Color color = Color.PaleGoldenrod;
            CNCUtils.AddLight(projectile.position, color);
            color.A = 0;
            if (npc != null)
            {
                AllNPC modNPC = npc.GetGlobalNPC<AllNPC>();
                npc.AddBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Sanctified], 1);
                if (modNPC.sanctifiedLevel < 0 || !npc.active) { projectile.Kill(); }
                projectile.position = npc.position - dist;

                if (projectile.timeLeft % 6 == 0)
                {
                    int idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                        projectile.Center,
                        CNC.Random.NextVector2(-3f, -2f, 3f, -1f),
                        CNC.Random.NextDegree(),
                        20,
                        Vector2.One,
                        color
                        );
                    ParticleManager.particle[idx].velocityMul = CNC.Random.NextVector2(.9f, 1f, .95f, 1f);
                    ParticleManager.particle[idx].velocityAdd = CNC.Random.NextVector2(0, -1.2f, 0, -1f);
                    ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-8f, 8f);
                    ParticleManager.particle[idx].scaleAdd = Vector2.One * CNC.Random.NextFloat(-.05f);
                }
            }
            else
            {
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
                ParticleManager.particle[idx].scaleAdd.Y = -.005f;
                ParticleManager.particle[idx].alphaAdd = -.05f;
                ParticleManager.particle[idx].alpha = 1f;

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
                ParticleManager.particle[idx].velocityMul = CNC.Random.NextVector2(.95f, .95f, .98f, .98f);
                ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                ParticleManager.particle[idx].alphaAdd = -.025f;
                ParticleManager.particle[idx].alpha = 1f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            if (npc != null) npc.GetGlobalNPC<AllNPC>().sanctifiedLevel--;
            for (int i = 0; i < 15; i++)
            {
                Color color = Color.PaleGoldenrod; color.A = 0;
                Vector2 vel = new Vector2(Main.rand.NextFloat(2f, 4.5f) * projectile.direction, Main.rand.NextFloat(-7f, .5f));

                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                    projectile.Center,
                    vel,
                    CNC.Random.NextDegree(),
                    100,
                    new Vector2(1f),
                    color,
                    new Color(color.R, color.G, color.B, 5),
                    false
                    );
                ParticleManager.particle[idx].lightAlpha = .2f;
                ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                ParticleManager.particle[idx].scaleAdd = new Vector2(-.01f);
                ParticleManager.particle[idx].alphaAdd = -.005f;
                ParticleManager.particle[idx].alpha = 1f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        public override bool PreDrawExtras()
        {
            Color color = Color.PaleGoldenrod; color.A = 0;
            Texture2D star = ModContent.Request<Texture2D>("CNC/Assets/Particles/Star4").Value;
            Vector2 pos = projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(star, pos, null, color * (alpha[0] / 100f), 0, star.Size() / 2f, new Vector2(scale[0] / 200f * MyMath.phi, scale[0] / 200f), 0, 0);
            
            CNCUtils.UpdateEffectVar(ref alpha);
            CNCUtils.UpdateEffectVar(ref scale);
            return true;
        }
    }
}
