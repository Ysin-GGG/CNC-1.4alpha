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
        private Color mainColor2 => Color.Lerp(new Color(64, 192, 64), new Color(0, 128, 0), et.GetNextValue() * .005f);
        private int time = 250;
        private int timer => time - projectile.timeLeft;
        private const float defRange = 320f;
        private const float defPower = 1.25f;
        private Vector2 scale = Vector2.Zero;
        private float alpha = 0f;
        private EffectTimer et = new EffectTimer(80, 1, 0, 80, 180, 10, 10, 1, 0);
        private EffectTimer et2 = new EffectTimer(75, 1, 0, 75, 100, 10, 10, 1, 0);
        private Vector4[] light = new Vector4[15];
        private List<NPC> haveHit = new List<NPC>();

        public override void ResetDefaults()
        {
            projectile.timeLeft = time;
            projectile.Size = Vector2.One * defRange * .8f;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.scale = 0f;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.noEnchantmentVisuals = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 25;
        }

        public override void OnSpawn()
        {
            CNCUtils.PlaySound("Remakes/TrueNightsEdge/Spawn", projectile.Center, .25f, 1f);
            CNCUtils.PlaySound("Remakes/TrueNightsEdge/Explode", projectile.Center, .35f, 1.25f);
            CNCUtils.ScreenShake(10, new Vector2(0, 60f), new Vector2(0, 40f), .9f, default, owner);
            projectile.ai[0] = (projectile.velocity.X < 0).ToDirectionInt(1);
            projectile.velocity = Vector2.Zero;
        }

        public override void HitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = 1;
            knockback = -15f;
            crit = false;
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
                if(timer == 25) CNCUtils.PlaySound("Remakes/TrueNightsEdge/Void", projectile.Center, .25f, 1.25f);
                if(timer == 40) CNCUtils.PlaySound("Remakes/TrueNightsEdge/Void2", projectile.Center, .5f, 1f);
                if (Alpha2 < 1f) Alpha2 += .02f;
                if (projectile.scale < 1f)
                {
                    projectile.scale += .02f;
                    scale += new Vector2(1f, -.5f) * .005f;
                    alpha -= .02f;
                }
                if (timer % 3 == 0 && CNC.Random.NextBool()) VisualEffect.NewVisualEffect<VisualEffects.Electrospark>(projectile.Center, new Vector2(CNC.Random.NextFloat(12f, 24f), 0f).RotatedBy(CNC.Random.NextDegree()), 0f).color = mainColor2;
                Swirl();
            }
            else if (projectile.timeLeft <= 25)
            {
                Vector2 pos = projectile.Center + new Vector2(CNC.Random.NextFloat(120f, 160f), 0f).RotatedBy(CNC.Random.NextDegree()) * this.scale;
                Vector2 vel = new Vector2(0f, CNC.Random.NextFloat(8f, 16f) * CNC.Random.NextBool().ToDirectionInt(1)).RotatedBy(CNC.Random.NextDegree());
                VisualEffect ef = VisualEffect.NewVisualEffect<VisualEffects.Electrospark>(pos, vel, 0f);
                ef.color = mainColor2;
                ef.deadTime += 20;
                if (Alpha2 > 0f) Alpha2 = MyMath.Lerp(0f, 1f, projectile.timeLeft / 25f);
                if (projectile.scale < 1.5f) projectile.scale += .02f;
                if (projectile.timeLeft == 25)
                {
                    projectile.friendly = false;
                    CNCUtils.PlaySound(SoundID.Item122, projectile, .8f, 1f);
                    //CNCUtils.PlaySound("Remakes/TrueNightsEdge/Explode2", projectile.Center, .2f, 1.25f);
                    CNCUtils.PlaySound("Remakes/TrueNightsEdge/Explode3", projectile.Center, 1f, 1f);
                    CNCUtils.ScreenShake(projectile.Center, defRange * 10f, 5, new Vector2(0, 60f), new Vector2(0, 20f), .9f, default);
                    this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.ShockWave], 0, 0f, 0f, 0f, 1f, 2f, 0f).ai[0] = 2f;
                    var e = CNCUtils.CreateExplosion(projectile);
                    e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/SilverFieldExplosion"), projectile.Center, Vector2.One * defRange * 1.125f, 1, 5);
                    e.SetDamage(1, projectile.DamageType, projectile.damage, 15f, projectile.CritChance, 0f, 1f);
                    e.SetColor(Color.Transparent, Color.Transparent, 0f);
                    e.SetAlpha(0f, 0f, 0f);
                    e.SetScale(1f, 0f, 0f);
                    for (int i = 0; i < 20; i++)
                    {
                        VisualEffect.NewVisualEffect<VisualEffects.NightsHaze>(projectile.Center, new Vector2(CNC.Random.NextFloat(6f, 12f), 0f).RotatedBy(CNC.Random.NextDegree()), 0f).color = Color.Lerp(mainColor, mainColor, (float)(CNC.Random.NextDouble() * .5 + .25));
                        VisualEffect.NewVisualEffect<VisualEffects.Flare>(projectile.Center, new Vector2(CNC.Random.NextFloat(12f, 16f), 0f).RotatedBy(CNC.Random.NextDegree()), 0f).color = mainColor;
                    }
                    for (int i = 0; i < light.Length; i++)
                    {
                        Vector2 scale = new Vector2(CNC.Random.NextFloat(.5f, 1f), CNC.Random.NextFloat(2f, 4f));
                        light[i] = new Vector4(scale * 7.5f, CNC.Random.NextDegree(), 0f);
                    }
                    for (int i = 0; i < haveHit.Count; i++)
                        if (haveHit[i] != null && haveHit[i].active)
                        {
                            haveHit[i].rotation = 0f;
                            haveHit[i].noGravity = false;
                        }
                }
                if(projectile.timeLeft == 24)
                {
                    for (int i = 0; i < Main.maxGore; i++)
                    {
                        if (Main.gore[i] != null && Main.gore[i].active)
                        {
                            float dist = Main.gore[i].position.Distance(projectile.Center);
                            float size = new Vector2(Main.gore[i].Width, Main.gore[i].Height).Length() * Main.gore[i].scale;
                            float range = defRange * .25f;
                            if (size < range && dist < range) Main.gore[i].active = false;
                        }
                    }
                }
                CNC.SetFilter("Absorption", false);
            }
            else
            {
                Vector2 offScreenRange = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
                Vector2 realScreenSize = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / Main.GameViewMatrix.Zoom;
                Vector2 screenCenter = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f;
                Vector2 realScreenPos = Main.screenPosition + screenCenter * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);

                Vector2 size = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
                Vector2 screen = realScreenPos - offScreenRange;
                Vector2 targetPos = projectile.Center - offScreenRange;
                Effect ef = CNC.Shader["Absorption"];
                ef.Parameters["uScreenResolution"].SetValue(realScreenSize);
                ef.Parameters["uScreenPosition"].SetValue(screen);
                ef.Parameters["uTargetPosition"].SetValue((targetPos - screen) / size);
                ef.Parameters["uImageOffset"].SetValue(Vector2.One * .1f);
                ef.Parameters["uIntensity"].SetValue(16f);
                ef.Parameters["uProgress"].SetValue(1f);
                ef.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
                ef.Parameters["uDist"].SetValue(defRange);

                //CNC.SetFilter("Absorption", true).GetShader().UseImage(CNC.GetTexture("Assets/Effects/Trace3"));

                if (timer % 10 == 0) CNCUtils.ScreenShake(projectile.Center, defRange * 3f, 5, new Vector2(0, 20f), new Vector2(0, 10f), .9f, default);
                if (timer % 3 == 0)
                {
                    Vector2 vel = new Vector2(CNC.Random.NextFloat(12f, 16f), 0f).RotatedBy(CNC.Random.NextDegree());
                    if (projectile.timeLeft > 100 && timer % 6 == 0)
                    {
                        VisualEffect effect = VisualEffect.NewVisualEffect<VisualEffects.Flare>(projectile.Center + vel * 30f, -vel, 0f);
                        effect.color = mainColor;
                        effect.deadTime = 65;
                    }
                    if (CNC.Random.NextBool()) VisualEffect.NewVisualEffect<VisualEffects.Electrospark>(projectile.Center, new Vector2(CNC.Random.NextFloat(12f, 24f), 0f).RotatedBy(CNC.Random.NextDegree()), 0f).color = mainColor2;
                }
                projectile.friendly = true;
                Swirl();
            }
            if (timer >= 25 && projectile.timeLeft >= 25)
            {
                for (int i = 0; i < 4; i++)
                {
                    float rot = i * MyMath.deg90 + MyMath.deg01 * 3f * projectile.timeLeft * projectile.ai[0];
                    Vector2 vel = new Vector2(1f, 0f).RotatedBy(rot);
                    float amount = MyMath.Lerp(1.25f, .75f, timer / (float)time);
                    int idx = Particle.NewParticle(
                        CNC.GetTexture("Assets/Particles/Light1"),
                        projectile.Center,
                        vel * MyMath.Lerp(1f, 6f, projectile.scale) * amount * et2.GetNextValue() * .01f,
                        rot,
                        30,
                        Vector2.One * projectile.scale * amount * .25f,
                        mainColor2.NewA(0) * amount,
                        mainColor2.NewA(3) * amount
                        );
                    ParticleManager.particle[idx].velocityMul = new Vector2(.99f);
                    ParticleManager.particle[idx].scaleAdd = -ParticleManager.particle[idx].scale / ParticleManager.particle[idx].timeLeftMax * 1.5f;
                    ParticleManager.particle[idx].alphaAdd = -1f / ParticleManager.particle[idx].timeLeftMax;
                    ParticleManager.particle[idx].alpha = 1f;

                    rot = i * MyMath.deg90 - MyMath.deg01 * 3f * projectile.timeLeft * projectile.ai[0] + MyMath.deg45;
                    vel = new Vector2(1f, 0f).RotatedBy(rot);
                    idx = Particle.NewParticle(
                        CNC.GetTexture("Assets/Particles/Light1"),
                        projectile.Center,
                        vel * MyMath.Lerp(1f, 4f, projectile.scale),
                        rot,
                        40,
                        Vector2.One * projectile.scale * .375f,
                        mainColor.NewA(0),
                        mainColor.NewA(3)
                        );
                    ParticleManager.particle[idx].velocityMul = new Vector2(.975f);
                    ParticleManager.particle[idx].scaleAdd = ParticleManager.particle[idx].scale / ParticleManager.particle[idx].timeLeftMax;
                    ParticleManager.particle[idx].scale = Vector2.Zero;
                    ParticleManager.particle[idx].alphaAdd = -1f / ParticleManager.particle[idx].timeLeftMax; ;
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
            Texture2D l = CNC.GetTexture("Assets/Glow/Refringence");
            Rectangle half = new Rectangle(0, 0, l.Width / 2, l.Height);
            Vector2 pos = projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(t, pos, null, mainColor.Flash().NewA(0) * alpha, MyMath.deg30 * projectile.ai[0], t.Center(), scale * MyMath.phi, 0, 0);
            Main.EntitySpriteDraw(t, pos, null, mainColor.Flash().NewA(0) * alpha, MyMath.deg60 * 2 * projectile.ai[0], t.Center(), scale * .75f, 0, 0);

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
            
            var ef = CNC.GetEffect("Trail2");
            ef.Parameters["uTransform"].SetValue(model * projection);
            ef.Parameters["uColor0"].SetValue(Color.White.ToVector4());
            ef.Parameters["uColor1"].SetValue(Color.White.ToVector4());
            ef.Parameters["uOffset"].SetValue(new Vector2(-1, 1) * 256f / Main.ScreenSize.ToVector2());
            ef.Parameters["uAlpha"].SetValue(this.Alpha2 * 3);
            ef.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 3f);
            
            float width = CNC.GetTexture(Texture + 2).Width * projectile.scale * MyMath.Lerp(.15f, .02f, timer / (float)time);
            List<VertexInfo> vertex = new List<VertexInfo>();
            for (int i = 0; i <= 360; ++i)
            {
                var factor = i / 360f;
                vertex.Add(new VertexInfo(projectile.Center, mainColor2, new Vector3(factor, 0, 0)));
                vertex.Add(new VertexInfo(projectile.Center + new Vector2(width, 0f).RotatedBy(MyMath.pi2 * factor), mainColor, new Vector3(factor, 1, 0)));
            }

            if (vertex.Count > 2)
            {
                Texture2D[] t2 =
                {
                        CNC.GetTexture("Assets/Effects/Trace1"),
                        CNC.GetTexture(Texture + 2),
                        CNC.GetTexture("Assets/Effects/Trace7"),
                    };

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                for (int i = 0; i < 3; i++)
                {
                    Main.graphics.GraphicsDevice.Textures[i] = t2[i];
                    Main.graphics.GraphicsDevice.SamplerStates[i] = SamplerState.PointWrap;
                }
                ef.CurrentTechnique.Passes[0].Apply();

                Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex.ToArray(), 0, vertex.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            
            Main.EntitySpriteDraw(Texture2, pos, null, mainColor.Flash().NewA(0) * Alpha2, 0f, Texture2.Center(), projectile.scale * MyMath.Lerp(.375f, .25f, timer / (float)time), 0, 0);
            if (projectile.timeLeft <= 25)
            {
                float amount = 1 - projectile.timeLeft / 25f;
                float amount2 = projectile.timeLeft / 25f;
                for (int i = 0; i < light.Length; i++)
                    Main.EntitySpriteDraw(l, pos, half, mainColor2.NewA(0) * Alpha2 * amount, light[i].Z + time * MyMath.deg01, l.Center(), projectile.scale * light[i].XY() * MyMath.Lerp(.5f, .2f, amount2) * amount2, 0, 0);
            }
            Main.EntitySpriteDraw(Texture2, pos, null, Color.Lerp(new Color(0, 255, 0), Color.White, timer / (float)time).Flash().NewA(0) * Alpha2 * .5f, 0f, Texture2.Center(), projectile.scale * MyMath.Lerp(.6f, .001f, timer / (float)time / 1.2f) / MyMath.phi, 0, 0);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (timer < 30) overPlayers.Add(index); 
            else behindNPCs.Add(index);
        }

        private void Swirl()
        {
            void swirl(Entity e)
            {
                float dist = e.Center.Distance(projectile.Center);
                float range = defRange * projectile.scale * 1.5f;
                float power = defPower * projectile.scale * 1.2f;
                if (dist < range)
                {
                    e.velocity *= (dist / range) * .05f + .95f;
                    e.velocity -= new Vector2(MyMath.Lerp(power, 0f, dist / range), 0f).RotatedBy((e.Center - projectile.Center).ToRotation());
                    if (e is NPC n)
                    {
                        if (n.rotation == 0 && !n.noGravity && !haveHit.Contains(n)) haveHit.Add(n);
                        n.rotation -= projectile.direction * MyMath.deg15 * MyMath.Lerp(1f, 0f, dist / range);
                        n.noGravity = true;
                    }
                }
            }
            int max = MyMath.Max(Main.maxNPCs, Main.maxItems, Main.maxGore);
            for (int i = 0; i < max; i++)
            {
                if (i < Main.maxNPCs && Main.npc[i] != null && Main.npc[i].active && !Main.npc[i].townNPC && !Main.npc[i].friendly && !Main.npc[i].boss && Main.npc[i].type != 488)
                    swirl(Main.npc[i]);
                if (i < Main.maxItems && Main.item[i] != null && Main.item[i].active)
                    swirl(Main.item[i]);
                if (i < Main.maxGore && Main.gore[i] != null && Main.gore[i].active)
                {
                    float dist = Main.gore[i].position.Distance(projectile.Center);
                    float range = defRange * projectile.scale * 1.5f;
                    float power = defPower * projectile.scale * 1.2f;
                    if (dist < range)
                    {
                        Main.gore[i].velocity *= (dist / range) * .05f + .95f;
                        Main.gore[i].velocity -= new Vector2(MyMath.Lerp(power, 0f, dist / range), 0f).RotatedBy((Main.gore[i].position - projectile.Center).ToRotation());
                    }
                    if (dist < range * .1f)
                    {
                        if (new Vector2(Main.gore[i].Width, Main.gore[i].Height).Length() * Main.gore[i].scale < 16f) Main.gore[i].active = false;
                        else Main.gore[i].scale *= .98f;
                    }
                }
            }
        }
    }
}
