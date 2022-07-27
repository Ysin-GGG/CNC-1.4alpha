
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using CNC.Common.Utils;

namespace CNC.Content.DataStructures
{
    public class TrailShaderData
    {
        public Texture2D imageHeatmap;
        public Texture2D imageTrace;
        public Texture2D imageTrail;
        public SamplerState samplerHeatmap;
        public SamplerState samplerTrace;
        public SamplerState samplerTrail;
        public Color color = new Color(1.0f, 1.0f, 1.0f);
        public Color secondaryColor = new Color(1.0f, 1.0f, 1.0f);
        public float saturation = 1f;
        public float opacity = 1f;
        public bool projectionMatrix;
        public Vector4 shaderSpecificData = Vector4.Zero;
        public Vector2 offset = Vector2.Zero;

        public int width;
        public Color colorBegin;
        public Color colorEnd;

        private float rand;
        private Effect shader;
        private Projectile proj;
        public List<VertexInfo> vertex;
        public List<VertexInfo> triangleList;

        public TrailShaderData(Projectile proj, int width = 16) => this._TrailShaderData((Effect)CNC.Instance.Assets.Request<Effect>("Content/Effects/Trail").Value, proj, null, null, null, width);
        public TrailShaderData(Effect shader, Projectile proj, int width = 16) => this._TrailShaderData(shader, proj, null, null, null, width);
        public TrailShaderData(Projectile proj, Texture2D heatmap, Texture2D trace, Texture2D trail, int width = 16) => this._TrailShaderData((Effect)CNC.Instance.Assets.Request<Effect>("Content/Effects/Trail").Value, proj, heatmap, trace, trail, width);
        public TrailShaderData(Effect shader, Projectile proj, Texture2D heatmap, Texture2D trace, Texture2D trail, int width = 16) => this._TrailShaderData(shader, proj, heatmap, trace, trail, width);
        private void _TrailShaderData(Effect shader, Projectile proj, Texture2D heatmap, Texture2D trace, Texture2D trail, int width = 16)
        {
            this.shader = shader;
            this.proj = proj;
            this.width = width;
            this.imageHeatmap = heatmap;
            this.imageTrace = trace;
            this.imageTrail = trail;
            this.samplerHeatmap = SamplerState.PointWrap;
            this.samplerTrace = SamplerState.PointWrap;
            this.samplerTrail = SamplerState.PointWrap;
            this.projectionMatrix = true;
            this.colorBegin = Color.White;
            this.colorEnd = Color.Transparent;
            this.vertex = new List<VertexInfo>();
            this.triangleList = new List<VertexInfo>();
            this.rand = CNC.Random.NextFloat(-100f, 100f);
        }

        public virtual void Calculate()
        {
            vertex.Clear();
            if (proj != null)
            {
                for (int i = 1; i < proj.oldPos.Length; i++)
                {
                    if (proj.oldPos[i] == Vector2.Zero) break;
                    Vector2 normalVector = proj.oldPos[i - 1] - proj.oldPos[i];
                    normalVector = Vector2.Normalize(new Vector2(-normalVector.Y, normalVector.X));
                    float amount = (float)i / proj.oldPos.Length;
                    Color color = Color.Lerp(colorBegin, colorEnd, amount) * (proj.alpha / 255f);
                    float w = MyMath.Lerp(1f, .05f, amount);

                    vertex.Add(new VertexInfo(proj.oldPos[i] + proj.Size / 2f + normalVector * width, color, new Vector3((float)Math.Sqrt(amount), 1, w)));
                    vertex.Add(new VertexInfo(proj.oldPos[i] + proj.Size / 2f - normalVector * width, color, new Vector3((float)Math.Sqrt(amount), 0, w)));
                }
            }

            triangleList.Clear();
            if (vertex.Count > 2)
            {
                VertexInfo begin = new VertexInfo((vertex[0].position + vertex[1].position) / 2f + Vector2.Normalize(proj.velocity) * width, Color.White, new Vector3(0.0f, 0.5f, 1.0f));
                triangleList.Add(vertex[0]);
                triangleList.Add(vertex[1]);
                triangleList.Add(begin);
                for (int i = 0; i < vertex.Count - 2; i += 2)
                {
                    triangleList.Add(vertex[i + 0]);
                    triangleList.Add(vertex[i + 2]);
                    triangleList.Add(vertex[i + 1]);

                    triangleList.Add(vertex[i + 1]);
                    triangleList.Add(vertex[i + 2]);
                    triangleList.Add(vertex[i + 3]);
                }
                //0 2
                //1 3
            }

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            shader.Parameters["uTransform"].SetValue(model * projection);
            shader.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly + rand);
        }

        public virtual void Draw(SpriteBatch spriteBatch, bool init = true)
        {
            this.Calculate();
            this.Draw(spriteBatch, this.triangleList.ToArray(), init);
        }

        public virtual void Draw(SpriteBatch spriteBatch, VertexInfo[] triangleList, bool init = true)
        {
            if (init)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.WireFrame;
            //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;//显示三角形栅格
            
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            if (imageHeatmap != null)
            {
                Main.graphics.GraphicsDevice.Textures[0] = imageHeatmap;
                Main.graphics.GraphicsDevice.SamplerStates[0] = samplerHeatmap;
            }
            if (imageTrace != null)
            {
                Main.graphics.GraphicsDevice.Textures[1] = imageTrace;
                Main.graphics.GraphicsDevice.SamplerStates[1] = samplerTrace;
            }
            if (imageTrail != null)
            {
                Main.graphics.GraphicsDevice.Textures[2] = imageTrail;
                Main.graphics.GraphicsDevice.SamplerStates[2] = samplerTrail;
            }

            shader.CurrentTechnique.Passes[0].Apply();
            Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList, 0, triangleList.Length / 3);
            Main.graphics.GraphicsDevice.RasterizerState = originalState;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}