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
    public abstract class SpiritArrow : CNCProjectile
    {
        public override string Texture => "CNC/Content/Projectiles/Hostile/SpiritArrow";
        protected Projectile p => projectile;
        protected const int dustid1 = DustId.TrailingCyan;
        protected const int dustid2 = DustId.CyanShortFx1;
        protected virtual Color mainColor => new Color(178, 214, 231);
        protected virtual int len => 30;
        protected virtual bool update => true;
        protected int attackCD = 0;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).rage;
        private float rand;
        private int fade = 0;
        //private bool fadeIn = true;
        //private bool fadeOut = false;
        protected bool drawTrail = false;

        public override void ResetStaticDefaults()
        {
            DisplayName.SetDefault(CNCLocalization.GetText("Projectile.SpiritArrow", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText("Projectile.SpiritArrow", 7));
        }

        public override void ResetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.timeLeft = 360;
            projectile.alpha = 255;
            projectile.light = 0;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            rand = CNC.Random.NextFloat(-100f, 100f);
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

        public override void AI()
        {
            AI001_RollingMissile(p, false, true, 90f);
            if (update) UpdateOldData(p);
            p.alpha = MyMath.Clamp(p.alpha + fade, 0, 255);
            if (p.alpha <= 0 || p.alpha >= 255) fade = 0;
            /*
            if (fadeIn) p.alpha = MyMath.Clamp(p.alpha - 5, 0, 255);
            if (fadeOut) p.alpha = MyMath.Clamp(p.alpha + 5, 0, 255);
            if (fadeIn && p.alpha <= 0) fadeIn = false;
            if (fadeOut && p.alpha >= 255) fadeOut = false;
            */
            projectile.friendly = false;
            projectile.hostile = true;
            while (attackCD > 0) attackCD--;
            Vector2 pos = (p.position + p.Size / 2f) / 16f;
            float r = mainColor.R / 255f;
            float g = mainColor.G / 255f;
            float b = mainColor.B / 255f;
            Lighting.AddLight((int)pos.X, (int)pos.Y, r, g, b);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 24;
            Point pos = p.position.ToPoint() + new Point(size, size);
            hitbox = new Rectangle(pos.X, pos.Y, p.width - size * 2, p.height - size * 2);
        }

        public override void HitPlayer(Player target, int damage, bool crit)
        {
            if (rage)
            {
                target.statMana -= (int)(target.statMana * .1f);
                target.immuneTime = 5;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                float x = p.position.X;
                float y = p.position.Y;
                float[] rand = { CNC.Random.NextDegree() / 2f, CNC.Random.NextFloat(-1, 1), CNC.Random.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(x, y, 9, 9);
                Dust dust = CNCUtils.NewDust(xywh, dustid2, rand[0], CNC.Random.NextFloat(1f, 2f), default, new Color(0, 234, 118));
                dust.position = xywh.XY() + p.Size / 2;
                dust.velocity = new Vector2(rand[1], rand[2]) * 8f;
                dust.color = mainColor;
                dust.noGravity = true;
            }
            for (int i = 0; i < p.oldPos.Length; i++)
            {
                float x = p.oldPos[i].X;
                float y = p.oldPos[i].Y;
                float[] rand = { CNC.Random.NextDegree() / 2f, CNC.Random.NextFloat(-1, 1), CNC.Random.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(x, y, 9, 9);
                Dust dust = CNCUtils.NewDust(xywh, dustid1, rand[0], CNC.Random.NextFloat(.8f, 1.2f), default, mainColor, MyMath.GetXY(p.oldRot[i] - MyMath.deg90) * 4f);
                dust.position = xywh.XY() + p.Size / 2;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            AfterImages(projectile, mainColor * .75f * (255 - p.alpha / 255f), len);
            return true;
        }
        
        public override void PostDraw(Color lightColor)
        {
            var ef = (Effect)Mod.Assets.Request<Effect>("Content/Effects/Trail").Value;
            List<VertexInfo> bars = new List<VertexInfo>();
            int width = 32;
            var normalDir = projectile.position - Projectile.oldPos[0];
            normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

            var color = mainColor;
            var w = MathHelper.Lerp(1f, 0.05f, 0);

            bars.Add(new VertexInfo(projectile.position + Projectile.Size / 2f + normalDir * width, color, new Vector3(0, 1, w)));
            bars.Add(new VertexInfo(projectile.position + Projectile.Size / 2f - normalDir * width, color, new Vector3(0, 0, w)));
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) break;

                normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                color = Color.Lerp(mainColor, Color.Transparent, factor);
                w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new VertexInfo(Projectile.oldPos[i - 1] + Projectile.Size / 2f + normalDir * (width + 32 * factor), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new VertexInfo(Projectile.oldPos[i - 1] + Projectile.Size / 2f - normalDir * (width + 32 * factor), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
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
                ef.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly + rand);
                var trai = new TrailShaderData(ef, projectile, width)
                {
                    colorBegin = mainColor,
                    imageHeatmap = Mod.Assets.Request<Texture2D>("Assets/Effects/Heatmap/Blue2").Value,
                    imageTrace = Mod.Assets.Request<Texture2D>("Assets/Effects/Trace1").Value,
                    imageTrail = Mod.Assets.Request<Texture2D>("Assets/Effects/FormalTrail6").Value,
                };
                trai.Draw(Main.spriteBatch, triangleList.ToArray(), true);
            }
        }

        public class V1 : SpiritArrow
        {
            protected override int len => 15 + (int)(15 * precent);
            float precent = 0f;
            Vector2 maxVel = Const._02;

            public override void AI()
            {
                base.AI();
                projectile.extraUpdates = 1;
                precent = precent + .01f > 1 ? 1 : precent + .01f;
                if (maxVel == Const._02) maxVel = p.velocity * .75f;
                else p.velocity = maxVel * precent;
            }
        }

        public class V2 : SpiritArrow
        {
            protected override bool update => p.timeLeft <= 300;
            float precent = .01f;
            Vector2 maxVel = Const._02;

            public override void AI()
            {
                base.AI();
                projectile.extraUpdates = 1;
                if (!update)
                {
                    Player target = FindClosestTarget(p, 800);
                    if (target.active)
                    {
                        Vector2 pos1 = target.Center;
                        Vector2 pos2 = p.position + new Vector2(32);
                        Vector2 distVec2 = pos1 - pos2;
                        maxVel = p.velocity = MyMath.GetXY(distVec2.ToRotation()) * 24f * .75f;
                    }
                    if (precent > 0) p.timeLeft = 480;
                    precent = 0;
                    p.position = p.oldPosition;
                }
                else
                {
                    precent = precent + .01f > 1 ? 1 : precent + .01f;
                    p.velocity = maxVel * precent;
                }
                p.hostile = update;
            }

            public override bool PreDraw(ref Color lightColor)
            {
                lightColor = Color.White;
                if (update) AfterImages(projectile, mainColor * .75f * (255 - p.alpha / 255f), len);
                return true;
            }
        }

        public class V3 : SpiritArrow
        {
            public override void AI()
            {
                base.AI();
                if (p.ai[0] > 0 && Main.player[(int)p.ai[0] - 1].active && p.timeLeft < 240)
                {
                    Player target = Main.player[(int)p.ai[0] - 1];
                    Vector2 pos1 = target.Center;
                    Vector2 pos2 = p.position + new Vector2(32);
                    Vector2 distVec2 = pos1 - pos2;
                    float velMax = 12f;
                    float rot = distVec2.ToRotation();
                    Vector2 vel = MyMath.GetXY(rot) * velMax / 12f;
                    p.velocity += vel / 2;
                    if (Math.Abs(p.velocity.X) > velMax) { p.velocity.X = (p.velocity.X < 0 ? -1 : 1) * velMax; }
                    if (Math.Abs(p.velocity.Y) > velMax) { p.velocity.Y = (p.velocity.Y < 0 ? -1 : 1) * velMax; }
                }
                if (len2 < 16) len2++;
            }

            private int pointX = 0;
            private int len2 = 0;

            public override bool PreDrawExtras()
            {
                Texture2D slash1 = Terraria.ModLoader.ModContent.Request<Texture2D>("CNC/Assets/Effects/Slash1").Value;
                Texture2D slash2 = Terraria.ModLoader.ModContent.Request<Texture2D>("CNC/Assets/Effects/Slash2").Value;
                Color _color = mainColor; _color.A = 0;
                //Drawing.DrawTrail(Main.spriteBatch, slash1, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, slash1.Height / 2, .1f, 16);
                //Drawing.DrawTrail(Main.spriteBatch, slash2, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, slash2.Height / 2, .1f, 16);
                //Drawing.DrawTrailDirect(Main.spriteBatch, slash1, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, slash1.Height / 2, new Vector2(1f, .10f), new Vector2(1f, .10f), true, 16);
                //Drawing.DrawTrailDirect(Main.spriteBatch, slash2, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, slash2.Height / 2, new Vector2(1f, .10f), new Vector2(1f, .10f), true, 16);
                //pointX = (pointX - 15 + slash1.Width) % slash1.Width;
                return true;
            }
        }

        public class V4 : SpiritArrow
        {
            public int state;
            public int stateTime { get => this.p.frameCounter; set => this.p.frameCounter = value; }

            public int[] stateOrder = new int[] { 64, 200, 100, 9999 };

            public float rot = 0;
            protected override bool update => state > 2;

            public override void AI()
            {
                base.AI();
                p.hostile = state == 3;
                if (state == 0)
                {
                    p.alpha = 255;
                    p.velocity.Y = MyMath.Min(p.velocity.Y + MyMath.g * .025f, 24f);
                    for (int i = 0; i < 2; i++)
                    {
                        float x = p.position.X;
                        float y = p.position.Y;
                        float[] rand = { CNC.Random.NextDegree() / 2f, CNC.Random.NextFloat(-1, 1), CNC.Random.NextFloat(-1, 1) };
                        Vector4 xywh = new Vector4(x, y, 9, 9);
                        Dust dust = CNCUtils.NewDust(xywh, dustid2, rand[0], CNC.Random.NextFloat(1.5f, 2.5f), default, new Color(0, 234, 118));
                        dust.position = xywh.XY() + p.Size / 2;
                        dust.velocity = Vector2.Normalize( p.velocity) * 4f;
                        dust.color = mainColor;
                        dust.noGravity = true;
                    }
                }
                if (state == 1)
                {
                    fade = -24;
                    p.velocity = Vector2.Zero;
                    p.rotation = 0f;
                }
                if (state == 2)
                {
                    fade = 24;
                    p.velocity.Y = MyMath.Clamp(p.velocity.Y * 2f, -16f, -1f);
                }
                if (state == 3)
                {
                    fade = -24;
                    if (p.velocity.Y < 16f) p.velocity *= 1.1f;
                    p.tileCollide = true;
                }
                if (++stateTime >= stateOrder[state])
                {
                    stateTime = 0;
                    state++;
                    if (state == 3)
                    {
                        p.velocity = new Vector2(0, 4f).RotatedBy(CNC.Random.NextDegree(-15f, 15f));
                        p.timeLeft = 300;
                    }
                }
                rot += MyMath.deg01 * 5;
            }

            public override bool PreDraw(ref Color lightColor)
            {
                lightColor = Color.White;
                if (update) AfterImages(projectile, mainColor * .75f * (255 - p.alpha / 255f), len);
                return true;
            }

            public override bool PreDrawExtras()
            {
                if (state == 1)
                {
                    const int frameCount = 7;
                    const int tickPerFrame = 2;
                    int frame = stateTime / tickPerFrame;
                    Texture2D effect = mod.Assets.Request<Texture2D>("Content/NPCs/SpiritBoss/Spawn").Value;
                    Texture2D image = mod.Assets.Request<Texture2D>("Content/Projectiles/Hostile/SpiritArrow").Value;
                    int height = effect.Height / frameCount;
                    Vector2 pos = this.p.Center - Main.screenPosition;
                    Rectangle source = new Rectangle(0, height * frame, effect.Width, height);
                    Color color = mainColor;
                    if (frame < frameCount) Main.EntitySpriteDraw(effect, pos, source, color, 0f, new Vector2(effect.Width, height) / 2f, .5f, 0, 0);
                    Main.EntitySpriteDraw(image, pos, null, color * (1 - p.alpha / 255f), rot, image.Size() / 2f, 1f, 0, 0);
                }
                if (state == 2)
                {
                    const int tickPerFrame = 1;
                    int frame = stateTime / tickPerFrame;
                    Texture2D effect = mod.Assets.Request<Texture2D>("Content/NPCs/SpiritBoss/Teleport").Value;
                    Vector2 pos = this.p.Center - Main.screenPosition;
                    Vector2[] scale =
                    {
                        new Vector2(2f, .75f),
                        new Vector2(1.75f, .8f),
                        new Vector2(1.5f, .85f),
                        new Vector2(1.25f, .9f),
                        new Vector2(1f, 1f),
                        new Vector2(1f, 1f),
                        new Vector2(.875f, 1.125f),
                        new Vector2(.75f, 1.25f),
                        new Vector2(.625f, 1.375f),
                        new Vector2(.5f, 1.5f),
                    };
                    float[] alpha = { .2f, .4f, .6f, .8f, 1f, 1f, .8f, .6f, .4f, .2f };
                    Color color = CNCUtils.AccurateColor(mainColor);
                    color.A = 0;
                    if (frame < 10)
                    {
                        Main.EntitySpriteDraw(effect, pos, null, color * alpha[frame], 0f, effect.Size() / 2f, scale[frame], 0, 0);
                        Main.EntitySpriteDraw(effect, pos, null, color * alpha[frame], 0f, effect.Size() / 2f, scale[frame], 0, 0);
                    }
                }
                return true;
            }

            public override void Kill(int timeLeft)
            {
                base.Kill(timeLeft);
                SoundEngine.PlaySound(SoundID.Item62, this.p.position);//14
                //SoundEngine.PlaySound(SoundID.Item25, this.p.position);
            }
        }
    }
}