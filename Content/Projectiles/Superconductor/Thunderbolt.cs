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
    public class Thunderbolt : CNCProjectile
    {
        private Vector2 targetPosition => data.ContainsKey("targetPosition") ? (Vector2)data["targetPosition"] : Vector2.Zero;
        private long seed => data.ContainsKey("seed") ? (long)data["seed"] : 0l;
        private PseudoRandom rand = new PseudoRandom();
        //private List<Vertex> thunderVertex = new List<Vertex>();
        private List<Vertex>[] thunderVertex = { new List<Vertex>(), new List<Vertex>(), new List<Vertex>() };
        private int timer;
        private Color mainColor => Color.LightBlue;

        public override void ResetDefaults()
        {
            projectile.alpha = 0;
            projectile.width = 32;
            projectile.height = 96;
            projectile.timeLeft = 30;
            projectile.penetrate = -1;
            projectile.extraUpdates = 2;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
            this.CannotHitSameEnemyAgain = true;
        }

        public override void UpdateAI()
        {
            if (timer++ == 0)
            {
                if (seed > 0) rand.SetSeed(seed, true);
                projectile.position -= new Vector2(0f, projectile.height * 15f);
                projectile.velocity = new Vector2(0f, projectile.height);
            }
            if (projectile.velocity != Vector2.Zero)
            {
                for (int i = 0; i < this.thunderVertex.Length; i++)
                {
                    Vertex vertex = new Vertex();
                    vertex.position = projectile.Center + new Vector2(rand.NextFloat(8, i == 0 ? 16 : 32), 0f) * rand.NextBool().ToDirectionInt(1);
                    vertex.velocity = i == 0 ? projectile.velocity * .05f : projectile.velocity.RotatedBy(rand.NextDegree(-15f, 15f)) * .05f;
                    vertex.width = i == 0 ? 32f : rand.NextFloat(12f, 24f);
                    if (vertex.position.Y > targetPosition.Y)
                    {
                        vertex.width = 8f;
                        vertex.position.Y = targetPosition.Y;
                        projectile.Center = targetPosition;
                        projectile.velocity = Vector2.Zero;
                        projectile.timeLeft = 40;
                        for (int j = 0; j < this.thunderVertex.Length; j++) this.thunderVertex[i].Add(vertex);
                        break;
                    }
                    this.thunderVertex[i].Add(vertex);
                }
            }
            else
            {
                projectile.extraUpdates = 0;
                this.Alpha2 = MyMath.Lerp(0f, 1f, projectile.timeLeft / 40f);
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

                List<VertexInfo>[] vertex = { new List<VertexInfo>(), new List<VertexInfo>(), new List<VertexInfo>() };
                for (int i = 0; i < vertex.Length; i++)
                {
                    for (int j = 0; j < this.thunderVertex[i].Count; j += 2)
                    {
                        if (this.thunderVertex[i].Count <= j + 2) j = this.thunderVertex[i].Count - 1;
                        Vector2 normalDir = Vector2.Normalize(this.thunderVertex[i][j].velocity.RotatedBy(MyMath.deg90));
                        float factor = MyMath.Lerp(0f, 1f, 3f * j / this.thunderVertex[i].Count);
                        Color color = Color.White * factor;
                        vertex[i].Add(new VertexInfo(this.thunderVertex[i][j].position + normalDir * this.thunderVertex[i][j].width * factor * (Alpha2 * .7f + .3f), color, new Vector3(0, 1, 0)));
                        vertex[i].Add(new VertexInfo(this.thunderVertex[i][j].position - normalDir * this.thunderVertex[i][j].width * factor * (Alpha2 * .7f + .3f), color, new Vector3(0, 0, 0)));
                        if (!Main.gamePaused)
                        {
                            var t = this.thunderVertex[i][j];
                            t.position += t.velocity * Vector2.One * MyMath.Lerp(0f, .5f, MyMath.sin(MyMath.pi * j / this.thunderVertex[i].Count));
                            this.thunderVertex[i][j] = t;
                        }
                    }
                }
                var blendState = spriteBatch.GraphicsDevice.BlendState;
                var rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
                var depthStencilState = spriteBatch.GraphicsDevice.DepthStencilState;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                
                for (int i = 0; i < vertex.Length; i++)
                    if (vertex[i].Count > 2)
                    {
                        Texture2D tex = i == 0 ? Texture2 : CNC.GetTexture("Assets/Effects/GlowTrail2");
                        for (int j = 0; j < 3; j++)
                        {
                            if (tex != null)
                            {
                                Main.graphics.GraphicsDevice.Textures[j] = tex;
                                Main.graphics.GraphicsDevice.SamplerStates[j] = SamplerState.PointWrap;
                            }
                        }
                        ef.CurrentTechnique.Passes[0].Apply();
                        Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex[i].ToArray(), 0, vertex[i].Count - 2);
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