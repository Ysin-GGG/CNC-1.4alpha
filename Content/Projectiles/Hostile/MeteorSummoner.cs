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
    public class MeteorSummoner : CNCProjectile
    {
        private const int dustid1 = DustId.TrailingCyan;
        private const int dustid2 = DustId.CyanFx;
        private Color mainColor => CNCID.Colors.SpiritBoss;
        private Color mainColor2 => CNCID.Colors.SpiritBoss2;
        protected int attackCD = 0;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).rage;
        internal static Vector2 targetOffset => new Vector2(0f, -400f);
        private float rot = 0f;
        private bool attack = false;
        private bool dying = false;
        private int width = 128;

        public override void ResetDefaults()
        {
            projectile.timeLeft = 420;
            projectile.alpha = 255;
            projectile.light = 0;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (dying || attack)
            {
                for (int i = 0; i < 3; i++)
                {
                    float[] rot2 = { 70, 188, 315 };
                    float amount = i / 3f;
                    float rot3 = (rot * 8 + rot2[i]) * MyMath.deg01;
                    Texture2D image = Mod.Assets.Request<Texture2D>("Content/Projectiles/Hostile/AnnulusOfPneuma").Value;
                    float scale = 3f * width / image.Width;
                    Vector2 center = projectile.Center;
                    Vector2 offset = new Vector2(3f * MyMath.sin(rot3), 1f * MyMath.cos(rot3)) * scale * 65f;
                    Dust dust = CNCUtils.NewDust(center + offset, dustid2);
                    dust.noGravity = true;
                    dust.color = mainColor2;
                }
            }
            if (dying)
            {
                projectile.velocity.X *= .95f;
                projectile.velocity.Y = MyMath.Max(projectile.velocity.Y - .375f, -24f);
            }
            else if (attack)
            {
                if (projectile.ai[0] > 0 && Main.player[(int)projectile.ai[0] - 1].active)
                {
                    Player target = Main.player[(int)projectile.ai[0] - 1];
                    Vector2 pos1 = target.Center + targetOffset;
                    Vector2 pos2 = projectile.Center;
                    Vector2 distVec2 = pos1 - pos2;
                    float velMax = 6f;
                    float rot = distVec2.ToRotation();
                    Vector2 vel = MyMath.GetXY(rot) * velMax / 12f;
                    projectile.velocity += vel / 2;
                    if (Math.Abs(projectile.velocity.X) > velMax) { projectile.velocity.X = (projectile.velocity.X < 0 ? -1 : 1) * velMax; }
                    if (Math.Abs(projectile.velocity.Y) > velMax * 2 / 3) { projectile.velocity.Y = (projectile.velocity.Y < 0 ? -1 : 1) * velMax * 2 / 3; }
                }
                float eps = 728f;
                var factor = CNC.Random.NextFloat(eps) / eps;
                Vector2 center = projectile.Center;
                Vector2 offset = new Vector2(3f * MyMath.cos(factor * MyMath.pi2), 1f * MyMath.sin(factor * MyMath.pi2)) * CNC.Random.NextInt(width / 2);
                if (projectile.timeLeft > 0 && projectile.timeLeft % 10 == 0 && rot >= 12.5f)
                {
                    float max = 36f;
                    for (int i = 0; i < max; i++)
                    {
                        float amount = i / max * MyMath.pi2;
                        Vector2 vel = new Vector2(6f * MyMath.cos(amount), 2f * MyMath.sin(amount));
                        CNCUtils.NewDust(center + offset, dustid1, .5f, vel * 2 / 3).noGravity = true;
                        CNCUtils.NewDust(center + offset, dustid1, .5f, vel / 2).noGravity = true;
                    }
                    this.NewProjectile(center + offset, new Vector2(0, 4f), CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritArrowV5], 0, 0, 0);
                    if (projectile.timeLeft % 20 == 10) CNCUtils.PlaySound(SoundID.Item28, projectile);
                }
                if (projectile.timeLeft == 1)
                {
                    dying = true;
                    projectile.timeLeft = 120;
                }
            }
            else
            {
                projectile.alpha = MyMath.Max(projectile.alpha - 4, 0);
                attack = projectile.alpha <= 0;
                projectile.timeLeft = 420;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (attack)
            {
                rot += rot < 25f ? MyMath.Lerp(.1f, .5f, rot / 25f) : .5f;
                var ef = (Effect)Mod.Assets.Request<Effect>("Content/Effects/Trail").Value;
                List<VertexInfo> bars = new List<VertexInfo>();
                float eps = 728f;
                for (int i = 0; i <= eps; i++)
                {
                    var factor = i / eps;
                    Vector2 center = projectile.Center;
                    Vector2 offset = new Vector2(3f * MyMath.sin(factor * MyMath.pi2), 1f * MyMath.cos(factor * MyMath.pi2)) * width;
                    bars.Add(new VertexInfo(center - offset, mainColor2, new Vector3(factor, 1, 1)));
                    bars.Add(new VertexInfo(center, mainColor2, new Vector3(factor, 0, 1)));
                }

                if (bars.Count > 2)
                {
                    List<VertexInfo> triangleList = new List<VertexInfo>();
                    for (int i = 0; i < bars.Count - 2; i += 2)
                    {
                        triangleList.Add(bars[i]);
                        triangleList.Add(bars[i + 2]);
                        triangleList.Add(bars[i + 1]);

                        triangleList.Add(bars[i + 1]);
                        triangleList.Add(bars[i + 2]);
                        triangleList.Add(bars[i + 3]);
                    }

                    Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                    Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                    ef.Parameters["uTransform"].SetValue(model * projection);
                    ef.Parameters["uTime"].SetValue(rot / 55f);
                    var trail = new TrailShaderData(ef, projectile, 64)
                    {
                        imageHeatmap = Mod.Assets.Request<Texture2D>("Assets/Effects/Heatmap/SpiritBoss").Value,
                        imageTrace = Mod.Assets.Request<Texture2D>("Assets/Effects/Trace1").Value,
                        imageTrail = Mod.Assets.Request<Texture2D>("Content/Projectiles/Hostile/MeteorSummoner").Value,
                    };
                    trail.Draw(Main.spriteBatch, triangleList.ToArray());
                }
            }
            else
            {
                rot = 0f;
                float dist = 420f;
                float factor = projectile.alpha / 255f;
                float factor2 = 1 - factor;
                float max = 7f;
                Texture2D image = Mod.Assets.Request<Texture2D>("Content/Projectiles/Hostile/AnnulusOfPneuma").Value;
                for (int i = 0; i < max; i++)
                {
                    float factor3 = 1 - (float)Math.Sqrt(factor2);
                    float factor4 = i / max * MyMath.pi2 + projectile.alpha * MyMath.deg01;
                    Vector2 pos = projectile.Center - Main.screenPosition;
                    Vector2 offset = (new Vector2(1f * MyMath.cos(factor4), 1f / 3f * MyMath.sin(factor4)) * factor3) * dist;
                    Vector2 origin = image.Size() / 2f;
                    Vector2 scale = new Vector2(3f, 1f) * 2f * width / image.Size();
                    Main.EntitySpriteDraw(image, pos + offset, null, mainColor2 * factor2 * (1 / max), 0, origin, scale, 0, 0);
                }
            }
            return false;
        }
    }
}