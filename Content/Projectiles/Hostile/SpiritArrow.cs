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
    public class SpiritArrow : CNCProjectile
    {
        public override string Texture => "CNC/Content/Projectiles/Hostile/SpiritArrow";
        private ref float extraRotation => ref projectile.localAI[0];
        private ref float radius => ref projectile.localAI[1];
        private ref float radius2 => ref projectile.ai[1];
        private const int time = 900;
        private const int len = 25;

        protected const int dustid1 = DustId.TrailingCyan;
        protected const int dustid2 = DustId.CyanShortFx1;
        protected virtual Color mainColor => CNCID.Colors.SpiritBoss;
        protected virtual Color mainColor2 => CNCID.Colors.SpiritBoss2;
        protected virtual bool update => true;
        protected int attackCD = 0;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).Rage;
        private float rand;
        private int fade = 0;
        private float lightAlpha;
        private float lightRotation;
        private float lightDirection;
        private Vector2 lightScale;
        private EffectTimer lightWidth;
        private EffectTimer lightHeight;
        private EffectTimer lightFlash;

        public override void ResetStaticDefaults()
        {
            DisplayName.SetDefault(CNCLocalization.GetText("Projectile.SpiritArrow", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText("Projectile.SpiritArrow", 7));
        }

        public override void ResetDefaults()
        {
            projectile.timeLeft = time;
            projectile.alpha = 255;
            projectile.light = 0;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            if (len != projectile.oldPos.Length)
            {
                Array.Resize(ref projectile.oldPos, len);
                Array.Resize(ref projectile.oldRot, len);
            }
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                projectile.oldRot[i] = 0.0f;
                projectile.oldPos[i].X = 0.0f;
                projectile.oldPos[i].Y = 0.0f;
            }
            fade = -5;
            attackCD = 0;
        }

        public override void OnSpawn()
        {
            lightAlpha = 0f;
            lightRotation = 1f;
            lightDirection = 1f;
            lightScale = Vector2.Zero;
            lightWidth = new EffectTimer(48, 1, 0, 48, 80, 10, 10, 4, 0);
            lightHeight = new EffectTimer(48, 1, 0, 48, 64, 10, 10, 2, 0);
            lightFlash = new EffectTimer(127, 1, 0, 127, 255, 0, 0, 16, 0);
            rand = CNC.Random.NextFloat(-100f, 100f);
            projectile.oldPosition = projectile.position;
        }

        public override void HitPlayer(Player target, int damage, bool crit)
        {
            CNCUtils.SetBuff(target, CNCID.Buff.StarMark, 60);
            CNCUtils.PlaySound(SoundID.Item27, target);
            float max = 36f;
            for (int i = 0; i < max; i++)
            {
                float amount = i / max * MyMath.pi2;
                Vector2 center = target.Center;
                Vector2 vel = new Vector2(6f * MyMath.cos(amount), 2f * MyMath.sin(amount));
                CNCUtils.NewDust(center + vel, DustId.CyanShortFx1, MyMath.sin(amount) + 2.5f, vel).noGravity = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (ParentNPC.ModNPC is NPCs.SpiritBoss.SpiritBoss)
            {
                var boss = (NPCs.SpiritBoss.SpiritBoss)ParentNPC.ModNPC;
                if (boss.MovingTime < 0 || boss.ActionState == 5) return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void AI()
        {
            Vector2 center = Vector2.Zero;
            if (ParentNPC != null && ParentNPC.active) center = ParentNPC.Center;
            else projectile.Kill();

            AI001_RollingMissile(projectile, false, true, 90f);
            projectile.alpha = MyMath.Clamp(projectile.alpha + fade, 0, 255);
            if (projectile.alpha <= 0 || projectile.alpha >= 255) fade = 0;
            projectile.friendly = false;
            projectile.hostile = true;
            while (attackCD > 0) attackCD--;
            CNCUtils.AddLight(projectile.Center, mainColor, .3f * Alpha2);

            if (ParentNPC != null && 0 <= ParentNPC.target && ParentNPC.target <= 255)
            {
                float dist = ParentNPC.Center.Distance(Main.player[ParentNPC.target].Center) / 16f;
                float amount = 1f;
                if (dist <= 15f) amount = 0f;
                else if (dist <= 35f) amount = .5f;
                if (amount >= 0) this.radius = MyMath.Lerp(10f, 40f, amount) * 16f;
            }
            if (this.radius2 > this.radius) this.radius2 = MyMath.Max(this.radius, this.radius2 - 3f);
            if (this.radius2 < this.radius) this.radius2 = MyMath.Min(this.radius, this.radius2 + 3f);
            projectile.Center = center + new Vector2(radius2, 0).RotatedBy(this.extraRotation + MyMath.deg01 * (time - projectile.timeLeft) * MyMath.Clamp(radius2 / 16f / 32f, 2f, 4f) * (int)projectile.ai[0]);
            projectile.rotation = (projectile.position - projectile.oldPosition).ToRotation() - MyMath.deg90;
            if (ParentNPC.ModNPC is NPCs.SpiritBoss.SpiritBoss)
            {
                var boss = (NPCs.SpiritBoss.SpiritBoss)ParentNPC.ModNPC;/*
                    if (-12 <= boss.movingTime2 && boss.movingTime2 < 0)
                    {
                        for (int i = 0; i < projectile.oldPos.Length; i++)
                        {
                            projectile.oldRot[i] = projectile.rotation;
                            projectile.oldPos[i] = projectile.position;
                        }
                    }
                    else */
                if (boss.MovingTime != 0)
                {
                    for (int i = 0; i < projectile.oldPos.Length; i++)
                    {
                        float rot = MyMath.deg01 * (time - projectile.timeLeft - i);
                        projectile.oldPos[i] = center + new Vector2(radius2, 0).RotatedBy(this.extraRotation + rot * MyMath.Clamp(radius2 / 16f / 32f, 2f, 4f) * (int)projectile.ai[0]);
                        projectile.oldRot[i] = ((i < 1 ? projectile.position : projectile.oldPos[i - 1]) - projectile.oldPos[i]).ToRotation() - MyMath.deg90;
                    }
                    projectile.rotation = (projectile.position - projectile.oldPos[0]).ToRotation() - MyMath.deg90;
                }
                else UpdateOldData(projectile);
                if (boss.MovingTime < 0 || boss.ActionState == 5) { Alpha2 = boss.Alpha2; }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                float x = projectile.position.X;
                float y = projectile.position.Y;
                float[] rand = { CNC.Random.NextDegree() / 2f, CNC.Random.NextFloat(-1, 1), CNC.Random.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(x, y, 9, 9);
                Dust dust = CNCUtils.NewDust(xywh, dustid2, rand[0], CNC.Random.NextFloat(1f, 2f), default, new Color(0, 234, 118));
                dust.position = xywh.XY() + projectile.Size / 2;
                dust.velocity = new Vector2(rand[1], rand[2]) * 8f;
                dust.color = mainColor;
                dust.noGravity = true;
            }
            float max = projectile.oldPos.Length * 2 / 3;
            for (int i = 0; i < max; i++)
            {
                float x = projectile.oldPos[i].X;
                float y = projectile.oldPos[i].Y;
                float[] rand = { CNC.Random.NextDegree() / 2f, CNC.Random.NextFloat(-1, 1), CNC.Random.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(x, y, 9, 9);
                Dust dust = CNCUtils.NewDust(xywh, dustid1, rand[0], CNC.Random.NextFloat(.8f, 1.2f) * (1 - (i / max)), default, mainColor, MyMath.GetXY(projectile.oldRot[i] - MyMath.deg90) * 4f);
                dust.position = xywh.XY() + projectile.Size / 2;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = mainColor * Alpha2;
            Texture2D trailImage = Mod.Assets.Request<Texture2D>("Assets/Effects/Trail/Trail6").Value;
            AfterImages(projectile, mainColor * .75f * Alpha2, len * 2, null, new Vector2(-1f, 1f) / len / 2f / 1.5f, new Vector2(.99f, 1.01f), .01f);

            Texture2D glow = Mod.Assets.Request<Texture2D>("Assets/Glow/Refringence").Value;
            Vector2 pos = projectile.Center - Main.screenPosition;
            Color color = CNCUtils.FlashColor(mainColor2 * Alpha2).NewA(0);
            Main.EntitySpriteDraw(glow, pos, null, color, 0f, glow.Size() / 2f, new Vector2(lightWidth.GetNextValue(), 80f) / 120f, 0, 0);
            Main.EntitySpriteDraw(glow, pos, null, color, MyMath.deg90, glow.Size() / 2f, new Vector2(lightHeight.GetNextValue() / MyMath.phi, 80f) / 120f, 0, 0);

            if (trailImage != null)
            {
                var ef = CNC.GetEffect("Trail2");
                List<VertexInfo> bars = new List<VertexInfo>();
                int width = 32;
                var normalDir = projectile.position - Projectile.oldPos[0];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var color2 = mainColor;
                var w = MathHelper.Lerp(1f, 0.05f, 0);

                bars.Add(new VertexInfo(projectile.position + Projectile.Size / 2f + normalDir * width, color, new Vector3(0, 1, w)));
                bars.Add(new VertexInfo(projectile.position + Projectile.Size / 2f - normalDir * width, color, new Vector3(0, 0, w)));
                for (int i = 1; i < Projectile.oldPos.Length; ++i)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero) break;

                    normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = i / (float)Projectile.oldPos.Length;
                    var factor2 = MyMath.Clamp(factor * 2, 0, 1);
                    color2 = Color.Lerp(CNCID.Colors.SpiritBoss, Color.Transparent, factor);
                    w = MathHelper.Lerp(1f, 0.05f, factor);

                    bars.Add(new VertexInfo(Projectile.oldPos[i - 1] + Projectile.Size / 2f + normalDir * (width + 16 * factor2), color2, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new VertexInfo(Projectile.oldPos[i - 1] + Projectile.Size / 2f - normalDir * (width + 16 * factor2), color2, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }

                List<VertexInfo> triangleList = new List<VertexInfo>();
                if (bars.Count > 2)
                {
                    triangleList.Add(bars[0]);
                    triangleList.Add(bars[1]);
                    triangleList.Add(new VertexInfo((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 3, Color.White, new Vector3(0, 0.5f, 1)));
                    for (int i = 0; i < bars.Count - 2; i += 2)
                    {
                        triangleList.Add(bars[i]);
                        triangleList.Add(bars[i + 2]);
                        triangleList.Add(bars[i + 1]);

                        triangleList.Add(bars[i + 1]);
                        triangleList.Add(bars[i + 2]);
                        triangleList.Add(bars[i + 3]);
                    }
                    RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                    Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                    Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                    ef.Parameters["uTransform"].SetValue(model * projection);
                    ef.Parameters["uColor0"].SetValue(CNCUtils.FlashColor(225, 242, 249).ToVector4());
                    ef.Parameters["uColor1"].SetValue(Color.Blue.ToVector4());// CNCUtils.FlashColor(61, 80, 112).ToVector4());
                    ef.Parameters["uOffset"].SetValue(new Vector2(-1f, 0f));
                    ef.Parameters["uAlpha"].SetValue((1f - ParentNPC.alpha / 255f) * 5f);
                    ef.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly + rand);
                    ef.Parameters["uAlphaBlend"].SetValue(true);
                    var trai = new TrailShaderData(ef, projectile, width)
                    {
                        colorBegin = mainColor,
                        trailAnimAdd = rand,
                        imageHeatmap = Mod.Assets.Request<Texture2D>("Assets/Effects/GlowTrail2").Value,
                        imageTrace = Mod.Assets.Request<Texture2D>("Assets/Effects/Trace3").Value,
                        imageTrail = trailImage,
                    };
                    trai.Draw(Main.spriteBatch, triangleList.ToArray(), true);
                }
            }
            return true;
        }

        public sealed override void PostDraw(Color lightColor)
        {
            Texture2D[] glow =
            {
                    Mod.Assets.Request<Texture2D>("Assets/Glow/Lamp").Value,
                    Mod.Assets.Request<Texture2D>("Assets/Glow/Teleport").Value,
                };
            Vector2 pos = projectile.Center - Main.screenPosition;
            Color color = CNCUtils.FlashColor(mainColor2) * ((255f - projectile.alpha) / 255f); color.A = 0;
            Main.EntitySpriteDraw(glow[1], pos, null, color, 0f, glow[1].Size() / 2f, Vector2.One / 4f, 0, 0);
            if (lightAlpha > 0f)
            {
                Main.EntitySpriteDraw(glow[1], pos, null, color * lightAlpha, lightRotation, glow[1].Size() / 2f, lightScale, 0, 0);
                lightAlpha -= .025f;
                lightRotation += MyMath.deg01 * lightDirection * 2f;
                lightScale += new Vector2(-1f, 1f) / 40f;
            }
            else if (CNC.Random.NextBool(.001))
            {
                lightAlpha = 1f;
                lightRotation = CNC.Random.NextDegree();
                lightDirection = CNC.Random.NextBool().ToDirectionInt();
                lightScale = new Vector2(1f, 0f);
            }
        }
    }
}