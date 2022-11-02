using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Content.DataStructures;

namespace CNC.Content.Skies
{
    public class TranquilitySky : BiomeSky
    {
        private Star[] star;
        private Meteor[] meteor;
        private Firefly[] firefly;

        public override void SetDefaults()
        {
            this.skyColor = Color.White;
            this.tileColor = Color.Black;
            this.cloudAlpha = 1f;
            this.opacityIn = .015f;
            this.opacityOut = .025f;
            this.intensity = .9f;
            this.skyImage = CNC.GetTexture("Content/Skies/TranquilitySky");
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (this.visible && maxDepth >= 3E+38f && minDepth < 3E+38f)
                spriteBatch.Draw(this.skyImage, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), this.skyColor * this.opacity);
            for (int i = 0; i < this.star.Length; i++)
                if (this.star[i].active && maxDepth >= 3E+38f && minDepth < 3E+38f)
                    this.star[i].Draw(spriteBatch, this.opacity);
            for (int i = 0; i < this.meteor.Length; i++)
                if (this.meteor[i].active && minDepth < this.meteor[i].depth && this.meteor[i].depth <= maxDepth)
                    this.meteor[i].Draw(spriteBatch, this.opacity);
            for (int i = 0; i < this.firefly.Length; i++)
                if (this.firefly[i].active && minDepth < this.firefly[i].depth && this.firefly[i].depth <= maxDepth)
                    this.firefly[i].Draw(spriteBatch, this.opacity);
        }

        public override void OnActivate()
        {
            this.star = new Star[300];
            for (int i = 0; i < this.star.Length; i++)
            {
                this.star[i] = new Star();
                this.star[i].active = false;
                this.star[i].opacity = 0f;
                this.star[i].rotation = 0f;
                this.star[i].position = Vector2.Zero;
                this.star[i].scale = 0f;
            }
            this.meteor = new Meteor[30];
            for (int i = 0; i < this.meteor.Length; i++)
            {
                this.meteor[i] = new Meteor();
                this.meteor[i].oldPos = new List<Vector2>();
                this.meteor[i].active = false;
                this.meteor[i].depth = 0f;
                this.meteor[i].opacity = 0f;
                this.meteor[i].position = Vector2.Zero;
                this.meteor[i].velocity = Vector2.Zero;
                this.meteor[i].scale = 0f;
            }
            this.firefly = new Firefly[200];
            for (int i = 0; i < this.firefly.Length; i++)
            {
                this.firefly[i] = new Firefly();
                this.firefly[i].active = false;
                this.firefly[i].depth = 0f;
                this.firefly[i].opacity = 0f;
                this.firefly[i].position = Vector2.Zero;
                this.firefly[i].velocity = Vector2.Zero;
                this.firefly[i].scale = Vector2.Zero;
                this.firefly[i].gradient = new EffectTimer(0, 1, 0, 0, 300, 50, 50, 1, 1);
            }
        }

        public override void OnDeactivate()
        {
            for (int i = 0; i < this.meteor.Length; i++)
                this.meteor[i].time = 0;
            for (int i = 0; i < this.meteor.Length; i++)
                this.meteor[i].time = 0;
            for (int i = 0; i < this.firefly.Length; i++)
                this.firefly[i].time = 0;
        }

        public override void Update()
        {
            Main.ColorOfTheSkies = Color.Lerp(Main.ColorOfTheSkies, Color.Black, this.opacity / 2f);
            for (int i = 0; i < this.star.Length; i++)
            {
                if (this.star[i].active) this.star[i].Update();
                else if (CNC.Random.NextBool(5))
                {
                    this.star[i].active = true;
                    this.star[i].time = 300 + CNC.Random.NextInt(200);
                    this.star[i].rotation = CNC.Random.NextDegree();
                    this.star[i].position = CNC.Random.NextVector2(Main.ScreenSize.ToVector2());
                    this.star[i].scale = CNC.Random.NextFloat(.75f, 1f);
                    this.star[i].gradient = new EffectTimer(0, 1, 0, 0, CNC.Random.NextInt(150, 400), 50, 50, 1, 1);
                }
            }
            for (int i = 0; i < this.meteor.Length; i++)
            {
                if (this.meteor[i].active) this.meteor[i].Update();
                else
                {
                    this.meteor[i].active = true;
                    this.meteor[i].time = 300 + CNC.Random.NextInt(200);
                    this.meteor[i].depth = (float)CNC.Random.NextDouble() * 8f + 1.5f;
                    this.meteor[i].position.X = CNC.Me.Center.X + CNC.Random.NextInt(Main.screenWidth);
                    this.meteor[i].position.Y = CNC.Random.NextInt((int)Main.worldSurface) * 16f - 1000f;
                    this.meteor[i].velocity = new Vector2(CNC.Random.NextFloat(12f, 13f), 0f).RotatedBy(120 * MyMath.deg01);
                    this.meteor[i].scale = CNC.Random.NextFloat(.8f, 1.2f) * MyMath.Lerp(.35f, .5f, this.meteor[i].depth / 10f);
                }
            }
            for (int i = 0; i < this.firefly.Length; i++)
            {
                if (this.firefly[i].active) this.firefly[i].Update();
                else
                {
                    this.firefly[i].active = true;
                    this.firefly[i].time = 300 + CNC.Random.NextInt(600);
                    this.firefly[i].depth = (float)CNC.Random.NextDouble() * 8f + 1.5f;
                    this.firefly[i].position.X = CNC.Me.Center.X + CNC.Random.NextInt(Main.screenWidth * 2) - Main.screenWidth;
                    this.firefly[i].position.Y = CNC.Random.NextInt((int)Main.worldSurface) * 16f + 1000f;
                    this.firefly[i].velocity = CNC.Random.NextVector2(1f, -1.5f, 1.5f, -1f);
                    this.firefly[i].scale = new Vector2(1f + CNC.Random.NextFloat(.3f), 1f) * .15f;
                    this.firefly[i].gradient = new EffectTimer(0, 1, 0, 0, 300, 50, 50, 1, 1);
                }
            }
        }

        public override Color OnTileColor(Color inColor)
            => Color.Lerp(inColor, Color.Black, this.opacity * this.intensity * .5f);

        private struct Star
        {
            public bool active;
            public int time;
            public float scale;
            public float opacity;
            public float rotation;
            public Vector2 position;
            public EffectTimer gradient;

            public void Update()
            {
                if (this.time-- > 0) opacity += .005f;
                else if (this.opacity <= 0f) this.active = false;
                else this.opacity -= .005f;
                this.opacity = MyMath.Clamp(this.opacity, 0f, 1f);
            }

            public void Draw(SpriteBatch spriteBatch, float alpha)
            {
                Texture2D tex1 = CNC.GetTexture("Assets/Particles/Star1");
                Vector2 pos = this.position;
                Color color = Color.Lerp(Color.White, CNCID.Colors.SpiritBoss, gradient.GetNextValue() / 300f).NewA(0);
                spriteBatch.Draw(tex1, pos, null, color * this.opacity * alpha, this.rotation, tex1.Center(), this.scale, 0, 0f);
            }
        }
        
        private struct Meteor
        {
            public bool active;
            public int time;
            public float depth;
            public float scale;
            public float opacity;
            public Vector2 position;
            public Vector2 velocity;
            public List<Vector2> oldPos;

            private float lineAlpha;
            private float lightAlpha;
            private float lightRotation;
            private float lightDirection;
            private Vector2 lightScale;

            public void Update()
            {
                oldPos.Insert(0, position);
                if (oldPos.Count > 30) oldPos.RemoveAt(30);
                this.position += this.velocity;
                if (this.time-- > 0) opacity += .025f;
                else if (this.opacity <= 0f) this.active = false;
                else this.opacity -= .025f;
                this.opacity = MyMath.Clamp(this.opacity, 0f, 1f);
            }

            public void Draw(SpriteBatch spriteBatch, float alpha)
            {
                Vector2 pos = this.position - Main.screenPosition;
                Color color = CNCUtils.FlashColor(CNCID.Colors.SpiritBoss * this.opacity * alpha).NewA(0);
                Texture2D[] glow =
                {
                    CNC.GetTexture("Assets/Glow/Lamp"),
                    CNC.GetTexture("Assets/Glow/Teleport"),
                    CNC.GetTexture("Assets/Effects/Metero1"),
                };
                Main.EntitySpriteDraw(glow[0], pos, null, color, 0f, glow[0].Center(), this.scale / 3f, 0, 0);
                spriteBatch.Draw(glow[2], pos, null, color * .75f, this.velocity.ToRotation(), new Vector2(glow[2].Width - 24f, glow[2].Height / 2f), new Vector2(MyMath.Clamp(this.velocity.Length() / 10f, .75f, 1.25f), .25f) * this.scale, SpriteEffects.FlipHorizontally, 0f);

                if (this.lightAlpha > 0f)
                {
                    Main.EntitySpriteDraw(glow[1], pos, null, color * lightAlpha, lightRotation, glow[1].Center(), lightScale * this.scale, 0, 0);
                    this.lightAlpha -= .025f;
                    this.lightRotation += MyMath.deg01 * lightDirection * 2f;
                    this.lightScale += new Vector2(-1f, 1f) / 40f;
                }
                else if (CNC.Random.NextBool(.01))
                {
                    this.lightAlpha = 1f;
                    this.lightRotation = CNC.Random.NextDegree();
                    this.lightDirection = CNC.Random.NextBool().ToDirectionInt();
                    this.lightScale = new Vector2(1f, 0f);
                }
            }
        }

        private struct Firefly
        {
            public bool active;
            public int time;
            public float depth;
            public float opacity;
            public Vector2 scale;
            public Vector2 position;
            public Vector2 velocity;
            public EffectTimer gradient;

            public void Update()
            {
                this.position += this.velocity;
                if (this.time-- > 0) opacity += .005f;
                else if (this.opacity <= 0f) this.active = false;
                else this.opacity -= .005f;
                this.opacity = MyMath.Clamp(this.opacity, 0f, 1f);
            }

            public void Draw(SpriteBatch spriteBatch, float alpha)
            {
                Texture2D tex1 = CNC.GetTexture("Assets/Particles/Light1");
                Vector2 pos = this.position - Main.screenPosition;
                Color color = Color.Lerp(Color.Yellow, Color.Green, gradient.GetNextValue() / 300f).NewA(0);
                spriteBatch.Draw(tex1, pos, null, color * this.opacity * alpha, this.velocity.ToRotation(), tex1.Center(), this.scale, 0, 0f);
            }
        }
    }
}