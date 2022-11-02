using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Superconductor
{
    public class Lightning : CNCProjectile
    {
        private NPC targetNPC => data.ContainsKey("targetNPC") ? (NPC)data["targetNPC"] : null;
        private Vector2 targetPosition;
        private List<Vertex>[] lightningVertex = { new List<Vertex>(), new List<Vertex>() };
        private long seed => data.ContainsKey("seed") ? (long)data["seed"] : 0L;
        private PseudoRandom rand = new PseudoRandom();
        private int timer = 0;
        private Color mainColor => Color.LightBlue;

        public override void ResetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 60;
            projectile.alpha = 0;
            projectile.penetrate = 5;
            projectile.extraUpdates = 3;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Magic;
            this.CannotHitSameEnemyAgain = true;
        }

        public override bool? CanHitNPC(NPC target)
            => target == targetNPC ? base.CanHitNPC(target) : false;

        public override void UpdateAI()
        {
            if (targetNPC == null || !targetNPC.active || targetNPC.life < 1)
            {
                projectile.damage = 0;
                projectile.velocity = Vector2.Zero;
            }
            else if (targetNPC.Center.Distance(projectile.Center) < projectile.velocity.Length() * 100f) targetPosition = targetNPC.Center;
            if (timer++ == 0 && seed > 0) rand.SetSeed(seed, true);
            if (projectile.velocity == Vector2.Zero)
            {
                projectile.extraUpdates = 0;
                this.Alpha2 = MyMath.Lerp(0f, 1f, projectile.timeLeft / 40f);
            }
            else
            {
                Vector2 dist = targetPosition - projectile.Center;
                float vel = projectile.velocity.Length();
                projectile.rotation = Utils.AngleLerp(projectile.rotation, dist.ToRotation(), 1f - MyMath.Clamp(dist.Length() / (vel * 10f), 0f, .9f));
                projectile.velocity = new Vector2(vel, 0).RotatedBy(projectile.rotation);
                for (int i = 0; i < this.lightningVertex.Length; i++)
                {
                    Vertex vertex = new Vertex();
                    vertex.position = projectile.Center + new Vector2(rand.NextFloat(8, 16f) * rand.NextBool().ToDirectionInt(1), 0f).RotatedBy(projectile.rotation);
                    vertex.velocity = (projectile.oldVelocity + projectile.velocity) / 2f * .05f;
                    vertex.width = rand.NextFloat(16f, 32f);
                    if (projectile.Center.Distance(targetPosition) < 32f || projectile.velocity == Vector2.Zero)
                    {
                        vertex.width = 8f;
                        vertex.position = targetPosition;
                        projectile.Center = targetPosition;
                        projectile.velocity = Vector2.Zero;
                        projectile.timeLeft = 40;
                    }
                    this.lightningVertex[i].Add(vertex);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Effect ef = CNC.GetEffect("Trail2");
            if (ef != null)
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uColor0"].SetValue(mainColor.ToVector4());
                ef.Parameters["uColor1"].SetValue(mainColor.ToVector4());
                ef.Parameters["uOffset"].SetValue(Vector2.Zero);
                ef.Parameters["uAlpha"].SetValue(Alpha2);
                ef.Parameters["uTime"].SetValue(0f);
                ef.Parameters["uAlphaBlend"].SetValue(true);

                List<VertexInfo>[] vertex = { new List<VertexInfo>(), new List<VertexInfo>() };
                for (int i = 0; i < vertex.Length; i++)
                {
                    for (int j = 0; j < this.lightningVertex[i].Count; j += 2)
                    {
                        if (this.lightningVertex[i].Count <= j + 2) j = this.lightningVertex[i].Count - 1;
                        Vector2 normalDir = Vector2.Normalize(this.lightningVertex[i][j].velocity.RotatedBy(MyMath.deg90));
                        float factor = 1f;
                        Color color = Color.White * factor;
                        vertex[i].Add(new VertexInfo(this.lightningVertex[i][j].position + normalDir * this.lightningVertex[i][j].width * factor * (Alpha2 * .7f + .3f), color, new Vector3(0, 1, 0)));
                        vertex[i].Add(new VertexInfo(this.lightningVertex[i][j].position - normalDir * this.lightningVertex[i][j].width * factor * (Alpha2 * .7f + .3f), color, new Vector3(0, 0, 0)));
                        if (!Main.gamePaused)
                        {
                            var t = this.lightningVertex[i][j];
                            t.position += t.velocity * Vector2.One * MyMath.Lerp(0f, .5f, (float)j / this.lightningVertex[i].Count * 2f);
                            this.lightningVertex[i][j] = t;
                        }
                    }
                }
                var blendState = spriteBatch.GraphicsDevice.BlendState;
                var rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
                var depthStencilState = spriteBatch.GraphicsDevice.DepthStencilState;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                for (int i = 0; i < 3; i++)
                {
                    if (Texture2 != null)
                    {
                        Main.graphics.GraphicsDevice.Textures[i] = Texture2;
                        Main.graphics.GraphicsDevice.SamplerStates[i] = SamplerState.PointWrap;
                    }
                }
                for (int i = 0; i < vertex.Length; i++)
                {
                    if (vertex[i].Count > 2)
                    {
                        ef.CurrentTechnique.Passes[0].Apply();
                        Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex[i].ToArray(), 0, vertex[i].Count - 2);
                    }
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, blendState, SamplerState.PointClamp, depthStencilState, rasterizerState, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }

        private struct Vertex
        {
            public Vector2 position;
            public Vector2 velocity;
            public float width;
        }
    }
}