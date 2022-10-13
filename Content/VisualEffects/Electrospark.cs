using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Content.DataStructures;

namespace CNC.Content.VisualEffects
{
    public class Electrospark : VisualEffect
    {
        private const int length = 15;
        private Vector2[] oldPos = new Vector2[length];

        public override void SetDefaults()
        {
            this.deadTime = CNC.Random.NextInt(50, 80);
            this.opacity = 1f;
            this.scale = Vector2.One * CNC.Random.NextFloat(.8f, 1.2f);
        }
        
        public override void PostUpdate()
        {
            position -= velocity;
            this.opacity = 1 - (float)time / deadTime;
            this.scale *= .99f;
            Vector2 pos = this.position;
            if (time <= length) this.velocity *= .975f;
            else this.velocity *= .985f;
            if (time % 3 == 0) for (int i = 0; i < length; i++)
                {
                    oldPos[i] = pos + new Vector2(CNC.Random.NextFloat(2f, MyMath.Max(2f, velocity.Length())), 0).RotatedBy(CNC.Random.NextInt(-60, 60) * MyMath.deg01 + velocity.ToRotation());
                    pos += velocity;
                }
            position += Vector2.Normalize(velocity) * .5f;
        }

        public override void Draw(SpriteBatch spriteBatch, DrawLayer layer)
        {
            if (layer == DrawLayer.overProjectlies)
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                var ef = CNC.GetEffect("Trail2");
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uColor0"].SetValue(Color.White.ToVector4());
                ef.Parameters["uColor1"].SetValue(Color.White.ToVector4());
                ef.Parameters["uAlpha"].SetValue(this.opacity);
                ef.Parameters["uTime"].SetValue(0);

                int width = 2;
                int max = MyMath.Min(time, length);
                Vector2 normalDir = this.position - this.oldPos[0];
                List<VertexInfo> vertex = new List<VertexInfo>();
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                vertex.Add(new VertexInfo(this.position - normalDir * width, color, new Vector3(0, 0, 1)));
                vertex.Add(new VertexInfo(this.position + normalDir * width, color, new Vector3(0, 1, 0)));
                for (int i = 1; i < length; ++i)
                {
                    if (this.oldPos[i] == Vector2.Zero) break;

                    normalDir = this.oldPos[i - 1] - this.oldPos[i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = i / (float)(length - 1f);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);

                    vertex.Add(new VertexInfo(this.oldPos[i - 1] - normalDir * width, color, new Vector3(factor, 0, w)));
                    vertex.Add(new VertexInfo(this.oldPos[i - 1] + normalDir * width, color, new Vector3(factor, 1, 0)));
                }
                
                if (vertex.Count > 2)
                {
                    Texture2D[] t =
                    {
                        CNC.GetTexture("Assets/Effects/Light2"),
                        CNC.GetTexture("Assets/Effects/Light2"),
                        CNC.GetTexture("Assets/Effects/Trace1"),
                    };

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    for (int i = 0; i < 3; i++) Main.graphics.GraphicsDevice.Textures[i] = t[i];
                    ef.CurrentTechnique.Passes[0].Apply();

                    Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex.ToArray(), 0, vertex.Count - 2);//依据三角形列绘制拖尾

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
        }
    }
}