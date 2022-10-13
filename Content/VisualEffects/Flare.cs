using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content.VisualEffects
{
    public class Flare : VisualEffect
    {
        private const float unit = 32f;
        private const int length = 10;
        private Vector2[] oldPos = new Vector2[length];

        public override void SetDefaults()
        {
            this.deadTime = CNC.Random.NextInt(50, 80);
            this.opacity = 0f;
            this.scale = Vector2.One * CNC.Random.NextFloat(.8f, 1.2f);
            this.rotation = CNC.Random.NextDegree();
            for (int i = 0; i < length; i++) oldPos[i] = this.position;
        }
        
        public override void PostUpdate()
        {
            if (time > deadTime - 20) this.opacity -= .05f;
            if (time < 10) this.opacity += .1f;
            this.scale *= .99f;
            this.velocity *= .975f;
            if (Math.Abs(this.velocity.X) > unit / 2f || Math.Abs(this.velocity.Y) > unit / 2f)
                this.velocity = Vector2.Normalize(this.velocity) * unit;
            this.rotation = this.velocity.ToRotation();
            for (int i = length - 1; i > 0; i--) oldPos[i] = oldPos[i - 1];
            oldPos[0] = this.position;
        }

        public override void Draw(SpriteBatch spriteBatch, DrawLayer layer)
        {
            if (layer == DrawLayer.overProjectlies)
            {
                Texture2D light = CNC.GetTexture("Assets/Effects/Light2");
                Texture2D light2 = CNC.GetTexture("Assets/Effects/Light3");
                Vector2 size = Vector2.One * unit;
                Vector2 scale = size / light.Size() * 2f;
                float max = MyMath.Min(time, length);
                for (int i = (int)max; i > 0; i--)
                {
                    float amount = 1 - i / max;
                    float amount2 = MyMath.Lerp(.75f, 0f, i / max);
                    Vector2 extra = new Vector2(MyMath.Lerp(MyMath.phi, 1f, i / max), 1f);
                    spriteBatch.Draw(light, oldPos[i - 1] - Main.screenPosition, null, color.Flash().NewA(0) * this.opacity, this.rotation, light.Center(), scale * amount2 * this.scale * extra * 1.25f, 0, 0);
                }
                for (int i = (int)max; i > 0; i--)
                {
                    float amount = 1 - i / max;
                    float amount2 = MyMath.Lerp(.75f, 0f, i / max);
                    Vector2 extra = new Vector2(MyMath.Lerp(MyMath.phi, 1f, i / max), 1f);
                    spriteBatch.Draw(light2, oldPos[i - 1] - Main.screenPosition, null, color.Flash() * this.opacity, this.rotation, light2.Center(), scale * amount2 * this.scale * extra, 0, 0);
                    spriteBatch.Draw(light2, oldPos[i - 1] - Main.screenPosition, null, color.Flash() * this.opacity, this.rotation, light2.Center(), scale * amount2 * this.scale * extra, 0, 0);
                }
            }
        }
    }
}