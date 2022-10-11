using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using CNC.Content.DataStructures;
using System.Collections.Generic;

namespace CNC.Content.Projectiles.Remakes
{
    public class VoidNightsEdge : CNCProjectile
    {
        private Color mainColor => Color.Lerp(new Color(96, 0, 192), new Color(48, 0, 112), et.GetNextValue() * .005f);
        private int time = 250;
        private int timer => time - projectile.timeLeft;
        private const float defRange = 320f;
        private const float defPower = 1.25f;
        private Vector2 scale = Vector2.Zero;
        private float alpha = 0f;
        private EffectTimer et = new EffectTimer(80, 1, 0, 80, 180, 10, 10, 1, 0);
        private EffectTimer et2 = new EffectTimer(750, 1, 0, 750, 1000, 10, 10, 5, 0);

        public override void ResetDefaults()
        {
            projectile.timeLeft = time;
            projectile.Size = Vector2.One * defRange * .8f;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.scale = 0f;
            projectile.damage = 100;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void OnSpawn()
        {
            CNCUtils.PlaySound("Remakes/TrueNightsEdge/Spawn", projectile.Center, .25f, 1f);
            CNCUtils.PlaySound("Remakes/TrueNightsEdge/Explode", projectile.Center, .25f, 1.25f);
            CNCUtils.ScreenShake(10, new Vector2(0, 60f), new Vector2(0, 40f), .9f, default, owner);
        }

        public override void AI()
        {
            if (timer < 25)
            {
                scale += Vector2.One * .04f;
                alpha += .04f;
            }
            else if (timer < 75)
            {
                if(timer == 25)
                {
                    CNCUtils.PlaySound("Remakes/TrueNightsEdge/Void", projectile.Center, .25f, 1.25f);
                    CNCUtils.PlaySound("Remakes/TrueNightsEdge/Void3", projectile.Center, .25f, 1f);
                }
                if (Alpha2 < 1f) Alpha2 += .02f;
                if (projectile.scale < 1f)
                {
                    projectile.scale += .02f;
                    scale += new Vector2(1f, -.5f) * .01f;
                    alpha -= .02f;
                }
                Swirl();
            }
            else if (projectile.timeLeft <= 25)
            {
                if (Alpha2 > 0f) Alpha2 = MyMath.Lerp(0f, 1f, projectile.timeLeft / 25f);
                if (projectile.scale < 1.5f) projectile.scale += .02f;
                if (projectile.timeLeft == 25)
                {
                    projectile.friendly = false;
                    CNCUtils.PlaySound(SoundID.Item122, projectile);
                    CNCUtils.PlaySound("Remakes/TrueNightsEdge/Explode", projectile.Center, .25f, 1.25f);
                    CNCUtils.PlaySound("Remakes/TrueNightsEdge/Explode2", projectile.Center, .25f, 5f);
                    CNCUtils.PlaySound("Remakes/TrueNightsEdge/Explode3", projectile.Center, .25f, 1f);
                    CNCUtils.PlaySound("Remakes/TrueNightsEdge/Lightning", projectile.Center, .25f, 1f);
                    CNCUtils.ScreenShake(projectile.Center, defRange * 10f, 5, new Vector2(0, 60f), new Vector2(0, 20f), .9f, default);
                    this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.ShockWave], 0, 0f, 0f, 0f, 1f, 2f, 0f).ai[0] = 2f;
                    var e = CNCUtils.CreateExplosion(projectile);
                    e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/SilverFieldExplosion"), projectile.Center, Vector2.One * defRange * 1.125f, 1, 5);
                    e.SetDamage(1, projectile.DamageType, projectile.damage * 10, 5f, projectile.CritChance, 0f, 1f);
                    e.SetColor(Color.Transparent, Color.Transparent, 0f);
                    e.SetAlpha(0f, 0f, 0f);
                    e.SetScale(1f, 0f, 0f);
                    for(int i = 0; i < 20; i++)
                    VisualEffect.NewVisualEffect<VisualEffects.NightsHaze>(projectile.Center, Vector2.Zero, 0f);
                }
            }
            else
            {
                if (projectile.timeLeft % 10 == 0) CNCUtils.ScreenShake(projectile.Center, defRange * 3f, 5, new Vector2(0, 20f), new Vector2(0, 10f), .9f, default);
                projectile.friendly = projectile.timeLeft >= 35;
                Swirl();
            }
            if(timer >= 25 && projectile.timeLeft >= 25)
            {
                for (int i = 0; i < 4; i++)
                {
                    float rot = i * MyMath.deg90 + MyMath.deg01 * 3f * projectile.timeLeft;
                    Vector2 vel = new Vector2(1f, 0f).RotatedBy(rot);
                    int idx = Particle.NewParticle(
                        CNC.GetTexture("Assets/Particles/Light1"),
                        projectile.Center,
                        vel * MyMath.Lerp(1f, 8f, projectile.scale) * et2.GetNextValue() * .001f,
                        rot,
                        40,
                        Vector2.One * projectile.scale * .25f,
                        mainColor.NewA(0),
                        mainColor.NewA(3)
                        );
                    ParticleManager.particle[idx].velocityMul = new Vector2(.98f);
                    ParticleManager.particle[idx].scaleAdd = new Vector2(-.0075f);
                    ParticleManager.particle[idx].alphaAdd = -.033f;
                    ParticleManager.particle[idx].alpha = 1f;
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool? flag = base.Colliding(projHitbox, targetHitbox);
            bool inRange = targetHitbox.Center.ToVector2().Distance(projectile.Center) < defRange * projectile.scale / 2f;
            return inRange;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            Texture2D t = CNC.GetTexture("Assets/Glow/Teleport");
            Main.EntitySpriteDraw(t, projectile.Center - Main.screenPosition, null, mainColor.Flash().NewA(0) * alpha, MyMath.deg30, t.Center(), scale, 0, 0);
            Main.EntitySpriteDraw(t, projectile.Center - Main.screenPosition, null, mainColor.Flash().NewA(0) * alpha, MyMath.deg60 * 2, t.Center(), scale / MyMath.phi, 0, 0);
            Main.EntitySpriteDraw(Texture2, projectile.Center - Main.screenPosition, null, mainColor.Flash().NewA(0) * Alpha2, 0f, Texture2.Center(), projectile.scale * .5f, 0, 0);
            Main.EntitySpriteDraw(Texture2, projectile.Center - Main.screenPosition, null, Color.White.Flash().NewA(0) * Alpha2 * .5f, 0f, Texture2.Center(), projectile.scale * .5f / MyMath.phi, 0, 0);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (timer < 30) overPlayers.Add(index); 
            else behindNPCs.Add(index);
        }

        private void Swirl()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if(Main.npc[i] != null && Main.npc[i].active && !Main.npc[i].townNPC && !Main.npc[i].friendly && !Main.npc[i].boss && Main.npc[i].type != 488)
                {
                    float dist = Main.npc[i].Distance(projectile.Center);
                    float range = defRange * projectile.scale * 1.5f;
                    float power = defPower * projectile.scale * 1.2f;
                    if (dist < range)
                    {
                        Main.npc[i].velocity *= (dist / range) * .05f + .95f;
                        Main.npc[i].velocity -= new Vector2(MyMath.Lerp(power, 0f, dist / range), 0f).RotatedBy((Main.npc[i].Center - projectile.Center).ToRotation());
                    }
                }
            }
            for (int i = 0; i < Main.maxItems; i++)
            {
                if (Main.item[i] != null && Main.item[i].active)
                {
                    float dist = Main.item[i].Distance(projectile.Center);
                    float range = defRange * projectile.scale * 1.5f;
                    float power = defPower * projectile.scale * 1.2f;
                    if (dist < range)
                    {
                        Main.item[i].velocity *= (dist / range) * .05f + .95f;
                        Main.item[i].velocity -= new Vector2(MyMath.Lerp(power, 0f, dist / range), 0f).RotatedBy((Main.item[i].Center - projectile.Center).ToRotation());
                    }
                }
            }
        }
    }
}
