using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.VisualEffects
{
    public class NightsHaze : VisualEffect
    {
        private Color color = Color.Lerp(new Color(128, 0, 255), new Color(48, 0, 112), (float)CNC.Random.NextDouble());
        private int layer = CNC.Random.NextAny((int)DrawLayer.overTiles, (int)DrawLayer.overNPCs, (int)DrawLayer.overProjectlies, (int)DrawLayer.overPlayers);
        public override void SetDefaults()
        {
            this.deadTime = 100;
            this.opacity = 1f;
            this.scale = Vector2.One * CNC.Random.NextFloat(1f, 1.5f);
            this.rotation = CNC.Random.NextDegree();
            this.velocity = new Vector2(CNC.Random.NextFloat(8f, 16f), 0f).RotatedBy(CNC.Random.NextDegree());
            this.velocity.Y *= .75f;
        }
        
        public override void PostUpdate()
        {
            this.opacity -= .01f;
            this.scale *= .99f;
            this.velocity *= .975f;
        }

        public override void Draw(SpriteBatch spriteBatch, DrawLayer layer)
        {
            Texture2D light = CNC.GetTexture("Assets/Effects/Light1");
            Texture2D trace = CNC.GetTexture("Assets/Effects/Trace7");
            Vector2 randPos = CNC.Random.NextVector2(0f, 0f, 1f, 1f);
            if (layer == (DrawLayer)this.layer)
            {
                Effect shader = CNC.GetEffect("Multiply");
                shader.Parameters["uColor"].SetValue(color.Flash().ToVector4());
                shader.Parameters["uImageSize0"].SetValue(light.Size());
                shader.Parameters["uImageSize1"].SetValue(trace.Size() / 2f);
                shader.Parameters["uOffset"].SetValue(Vector2.One * .1f * Main.GlobalTimeWrappedHourly);
                shader.Parameters["uAlpha"].SetValue(this.opacity);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

                Main.graphics.GraphicsDevice.Textures[0] = light;
                Main.graphics.GraphicsDevice.Textures[1] = trace;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

                if (shader != null) shader.CurrentTechnique.Passes[0].Apply();

                spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
                spriteBatch.Draw(light, position - Main.screenPosition, null, color * this.opacity, this.rotation, light.Center(), this.scale, 0, 0);

                spriteBatch.End();
                spriteBatch.Begin();
            }
        }
    }
}