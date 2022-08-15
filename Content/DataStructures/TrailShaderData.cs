
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
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
        public bool useTransform;
        public bool useTime;
        public Vector4 shaderSpecificData = Vector4.Zero;
        public Vector2 offset = Vector2.Zero;

        public int width;
        public float width2;
        public float trailAnimAdd;
        public float trailAnimMult;
        public Color colorBegin;
        public Color colorEnd;
        
        private Effect shader;
        private Projectile proj;
        public List<VertexInfo> vertex;
        public List<VertexInfo> triangleList;

        public TrailShaderData(Projectile proj, int width = 16) => this._TrailShaderData(null, proj, null, null, null, width);
        public TrailShaderData(Effect shader, Projectile proj, int width = 16) => this._TrailShaderData(shader, proj, null, null, null, width);
        public TrailShaderData(Projectile proj, Texture2D heatmap, Texture2D trace, Texture2D trail, int width = 16) => this._TrailShaderData(null, proj, heatmap, trace, trail, width);
        public TrailShaderData(Effect shader, Projectile proj, Texture2D heatmap, Texture2D trace, Texture2D trail, int width = 16) => this._TrailShaderData(shader, proj, heatmap, trace, trail, width);
        private void _TrailShaderData(Effect shader, Projectile proj, Texture2D heatmap, Texture2D trace, Texture2D trail, int width = 16)
        {
            this.proj = proj;
            this.width = width;
            this.width2 = width / 2;
            this.trailAnimAdd = 0f;
            this.trailAnimMult = 1f;
            this.imageHeatmap = heatmap;
            this.imageTrace = trace;
            this.imageTrail = trail;
            this.samplerHeatmap = SamplerState.PointWrap;
            this.samplerTrace = SamplerState.PointWrap;
            this.samplerTrail = SamplerState.PointWrap;
            this.useTransform = shader == null;
            this.useTime = shader == null;
            this.colorBegin = Color.White;
            this.colorEnd = Color.Transparent;
            this.vertex = new List<VertexInfo>();
            this.triangleList = new List<VertexInfo>();
            this.shader = shader ?? ModContent.Request<Effect>("CNC/Content/Effects/Trail").Value;
        }

        public static List<VertexInfo> GetBars(Projectile projectile, float width, bool reverse = false, float amountAdd = 0f, float amountMult = 1f, int len = -1)
        {
            /*
            List<VertexInfo> bars = new List<VertexInfo>();
            int max = len < 0 ? projectile.oldPos.Length : len;
            for (int i = 1; i < max; i++)
            {
                if (projectile.oldPos[i] == Vector2.Zero) break;
                Vector2 normalVector = projectile.oldPos[i - 1] - projectile.oldPos[i];
                normalVector = Vector2.Normalize(new Vector2(-normalVector.Y, normalVector.X));
                float amount = (float)i / max;
                float w = MyMath.Lerp(1f, .05f, amount);
                bars.Add(new VertexInfo(projectile.oldPos[i - 1] + projectile.Size / 2f + normalVector * width * (amount + amountAdd) * amountMult, Color.White, new Vector3((float)Math.Sqrt(amount), 1, w)));
                bars.Add(new VertexInfo(projectile.oldPos[i - 1] + projectile.Size / 2f - normalVector * width * (amount + amountAdd) * amountMult, Color.White, new Vector3((float)Math.Sqrt(amount), 0, w)));
            }
            */
            return GetBars(projectile, width, float.MinValue, float.MaxValue, reverse, amountAdd, amountMult, len);
        }

        public static List<VertexInfo> GetBars(Projectile projectile, float width, float amountMin, float amountMax, bool reverse = false, float amountAdd = 0f, float amountMult = 1f, int len = -1, bool sqrt = true)
        {
            List<VertexInfo> bars = new List<VertexInfo>();
            int max = len < 0 ? projectile.oldPos.Length : len;
            for (int i = 1; i < max; i++)
            {
                if (projectile.oldPos[i] == Vector2.Zero) break;
                Vector2 normalVector = projectile.oldPos[i - 1] - projectile.oldPos[i];
                normalVector = Vector2.Normalize(new Vector2(-normalVector.Y, normalVector.X));
                float amount = reverse ? ((float)i / max) : (1 - (float)i / max);
                float w = MyMath.Lerp(1f, .05f, amount);
                bars.Add(new VertexInfo(projectile.oldPos[i - 1] + projectile.Size / 2f + normalVector * width * MyMath.Clamp((amount + amountAdd) * amountMult, amountMin, amountMax), Color.White, new Vector3(sqrt ? (float)Math.Sqrt(amount) : amount, 1, w)));
                bars.Add(new VertexInfo(projectile.oldPos[i - 1] + projectile.Size / 2f - normalVector * width * MyMath.Clamp((amount + amountAdd) * amountMult, amountMin, amountMax), Color.White, new Vector3(sqrt ? (float)Math.Sqrt(amount) : amount, 0, w)));
            }
            return bars;
        }

        /// <summary> 计算生成三角形列 </summary>
        /// <param name="bars">直线列表</param>
        /// <param name="vel">速度矢量</param>
        /// <param name="mult">倍数</param>
        public static List<VertexInfo> GetTriangleList(List<VertexInfo> bars, Vector2 vel, float mult)
            => GetTriangleList(bars, Vector2.Normalize(vel) * mult);

        /// <summary> 计算生成三角形列 </summary>
        /// <param name="bars">直线列表</param>
        /// <param name="beginOffset">拖尾头部坐标偏移</param>
        public static List<VertexInfo> GetTriangleList(List<VertexInfo> bars, Vector2 beginOffset)
        {
            if (bars.Count > 2)
            {
                List<VertexInfo> triangleList = new List<VertexInfo>();
                triangleList.Add(bars[0]);
                triangleList.Add(bars[1]);
                triangleList.Add(new VertexInfo((bars[0].position + bars[1].position) * 0.5f + beginOffset, Color.White, new Vector3(0.0f, 0.5f, 1.0f)));
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }
                return triangleList;
            }
            return null;
        }

        public virtual void Calculate(List<VertexInfo> vertex = null, Vector2 firstPos = default)
        {
            if (vertex == null)
            {
                this.vertex.Clear();
                vertex = this.vertex;
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
            }

            if (vertex != null && vertex.Count > 2)
            {
                VertexInfo begin = new VertexInfo(firstPos == default ? (vertex[0].position + vertex[1].position) / 2f + Vector2.Normalize(proj.velocity) * width2 : firstPos, Color.White, new Vector3(0.0f, 0.5f, 1.0f));
                triangleList.Clear();
                //↓第一个三角形比较特殊，因为是拖尾的头部
                triangleList.Add(vertex[0]);
                triangleList.Add(vertex[1]);
                triangleList.Add(begin);
                for (int i = 0; i < vertex.Count - 2; i += 2)
                {
                    //↓第二个三角形
                    triangleList.Add(vertex[i + 0]);
                    triangleList.Add(vertex[i + 2]);
                    triangleList.Add(vertex[i + 1]);
                    //↓第三个三角形
                    triangleList.Add(vertex[i + 1]);
                    triangleList.Add(vertex[i + 2]);
                    triangleList.Add(vertex[i + 3]);
                    //以此类推生成...
                }
            }

            if (useTransform)
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                shader.Parameters["uTransform"].SetValue(model * projection);
            }
            if (useTime) shader.Parameters["uTime"].SetValue(trailAnimMult * (trailAnimAdd - Main.GlobalTimeWrappedHourly));
        }

        /// <summary> 根据每帧的直线生成三角形列后绘制拖尾 </summary>
        /// <param name="bars">直线列表</param>
        /// <param name="firstPos">拖尾的头部坐标</param>
        /// <param name="init">将SpriteBatch初始化</param>
        public virtual void Draw(SpriteBatch spriteBatch, List<VertexInfo> vertex = null, Vector2 firstPos = default, bool init = true)
        {
            this.Calculate(vertex, default);
            this.Draw(spriteBatch, this.triangleList.ToArray(), init);
        }

        /// <summary> 根据三角形列直接绘制拖尾 </summary>
        /// <param name="triangleList">三角形列</param>
        /// <param name="init">将SpriteBatch初始化</param>
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

            if (shader != null) shader.CurrentTechnique.Passes[0].Apply();//引用着色器pass通道
            Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList, 0, triangleList.Length / 3);//依据三角形列绘制拖尾
            Main.graphics.GraphicsDevice.RasterizerState = originalState;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
/*
        public virtual void Calculate(List<VertexInfo> vertex = null, Vector2 firstPos = default)
        {
            if (this.shader == null) this.shader = ModContent.Request<Effect>("CNC/Content/Effects/Trail").Value;
            if (vertex == null)
            {
                vertex = new List<VertexInfo>();
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
                this.vertex = vertex;
            }

            if (vertex.Count > 2)
            {
                VertexInfo begin = new VertexInfo(firstPos == default ? (vertex[0].position + vertex[1].position) / 2f + Vector2.Normalize(proj.velocity) * width2 : firstPos, Color.White, new Vector3(0.0f, 0.5f, 1.0f));
                triangleList.Clear();
                //↓第一个三角形比较特殊，因为是拖尾的头部
                triangleList.Add(vertex[0]);
                triangleList.Add(vertex[1]);
                triangleList.Add(begin);
                for (int i = 0; i < vertex.Count - 2; i += 2)
                {
                    //↓第二个三角形
                    triangleList.Add(vertex[i + 0]);
                    triangleList.Add(vertex[i + 2]);
                    triangleList.Add(vertex[i + 1]);
                    //↓第三个三角形
                    triangleList.Add(vertex[i + 1]);
                    triangleList.Add(vertex[i + 2]);
                    triangleList.Add(vertex[i + 3]);
                    //以此类推生成...
                }
                //三角形顶点顺序如下
                //0 2
                //1 3
                if (useTransform)
                {
                    Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                    Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                    shader.Parameters["uTransform"].SetValue(model * projection);
                }
                if (useTime) shader.Parameters["uTime"].SetValue(trailAnimMult * (trailAnimAdd - Main.GlobalTimeWrappedHourly));
            }

        }

        /// <summary> 根据每帧的直线生成三角形列后绘制拖尾 </summary>
        /// <param name="bars">直线列表</param>
        /// <param name="firstPos">拖尾的头部坐标</param>
        /// <param name="init">将SpriteBatch初始化</param>
        public virtual void Draw(SpriteBatch spriteBatch, List<VertexInfo> bars = null, Vector2 firstPos = default, bool init = true)
        {
            this.Calculate(vertex, default);
            this.Draw(spriteBatch, this.triangleList.ToArray(), init);
        }

        /// <summary> 根据三角形列直接绘制拖尾 </summary>
        /// <param name="triangleList">三角形列</param>
        /// <param name="init">将SpriteBatch初始化</param>
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

            Main.graphics.GraphicsDevice.RasterizerState = originalState;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
*/