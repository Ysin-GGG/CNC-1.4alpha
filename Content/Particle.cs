using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content
{
    public class Particle
    {
        public Color color;
        public Color color2;
        public Color light;
        public Color light2;
        public float lightAlpha;
        public Vector2 scale;
        public Vector2 scale2;
        public Vector2 scaleAdd;
        public Vector2 scaleMul;
        public Vector2 scaleGlow;
        public Vector2 oldPosition;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 velocityAdd;
        public Vector2 velocityMul;
        public Texture2D texture;
        public Texture2D textureGlow;
        public float oldRotation;
        public float rotation;
        public float rotationAdd;
        public float alpha;
        public float alpha2;
        public float alphaAdd;
        public float colorFlashSpeed;
        public float lightFlashSpeed;
        public float alphaFlashSpeed;
        public float scaleFlashSpeed;
        public bool active;
        public bool noGravity;
        public int timeLeft;
        public int timeLeftMax;
        public int extraUpdate;
        public int whoAmI;
        public string tag;
        internal float[] flash;
        internal int[] flashDir;

        public Color Color
        {
            get => Color.Lerp(this.color, this.color2, this.flash[0]);
            set
            {
                Color origin = Color.Lerp(this.color, this.color2, this.flash[0]);
                Color newColor = new Color((byte)((float)value.R / (float)origin.R), (byte)((float)value.G / (float)origin.G), (byte)((float)value.B / (float)origin.B), (byte)((float)value.A / (float)origin.A));
                this.color.R = (byte)((float)this.color.R * (float)newColor.R);
                this.color.G = (byte)((float)this.color.G * (float)newColor.G);
                this.color.B = (byte)((float)this.color.B * (float)newColor.B);
                this.color.A = (byte)((float)this.color.A * (float)newColor.A);
                this.color2.R = (byte)((float)this.color2.R * (float)newColor.R);
                this.color2.G = (byte)((float)this.color2.G * (float)newColor.G);
                this.color2.B = (byte)((float)this.color2.B * (float)newColor.B);
                this.color2.A = (byte)((float)this.color2.A * (float)newColor.A);
            }
        }
        public Color Light
        {
            get => Color.Lerp(this.light, this.light2, this.flash[1]);
            set
            {
                Color origin = Color.Lerp(this.light, this.light2, this.flash[1]);
                Color newLight = new Color((byte)((float)value.R / (float)origin.R), (byte)((float)value.G / (float)origin.G), (byte)((float)value.B / (float)origin.B), (byte)((float)value.A / (float)origin.A));
                this.light.R = (byte)((float)this.light.R * (float)newLight.R);
                this.light.G = (byte)((float)this.light.G * (float)newLight.G);
                this.light.B = (byte)((float)this.light.B * (float)newLight.B);
                this.light.A = (byte)((float)this.light.A * (float)newLight.A);
                this.light2.R = (byte)((float)this.light2.R * (float)newLight.R);
                this.light2.G = (byte)((float)this.light2.G * (float)newLight.G);
                this.light2.B = (byte)((float)this.light2.B * (float)newLight.B);
                this.light2.A = (byte)((float)this.light2.A * (float)newLight.A);
            }
        }
        public float Alpha
        {
            get => MyMath.Lerp(this.alpha, this.alpha2, this.flash[2]);
            set
            {
                float newAlpha = value / MyMath.Lerp(this.alpha, this.alpha2, this.flash[2]);
                this.alpha = this.alpha * newAlpha;
                this.alpha2 = this.alpha2 * newAlpha;
            }
        }
        public Vector2 Scale
        {
            get => MyMath.Lerp(this.scale, this.scale2, this.flash[3]);
            set
            {
                Vector2 newScale = value / MyMath.Lerp(this.scale, this.scale2, this.flash[3]);
                this.scale.X = this.scale.X * newScale.X;
                this.scale.Y = this.scale.Y * newScale.Y;
                this.scale2.X = this.scale2.X * newScale.X;
                this.scale2.Y = this.scale2.Y * newScale.Y;
            }
        }

        public Particle() => this.SetDefaults();
        public void SetDefaults()
        {
            this.color = Color.White;
            this.color2 = Color.White;
            this.light = Color.Transparent;
            this.light2 = Color.Transparent;
            this.lightAlpha = .1f;
            this.scale = Vector2.One;
            this.scale2 = Vector2.One;
            this.scaleAdd = Vector2.Zero;
            this.scaleMul = Vector2.One;
            this.scaleGlow = Vector2.One;
            this.oldPosition = Vector2.Zero;
            this.position = Vector2.Zero;
            this.velocity = Vector2.Zero;
            this.velocityAdd = Vector2.Zero;
            this.velocityMul = Vector2.One;
            this.texture = null;
            this.textureGlow = null;
            this.rotation = 0f;
            this.rotationAdd = 0f;
            this.alpha = 1f;
            this.alpha2 = 1f;
            this.alphaAdd = 0f;
            this.active = false;
            this.noGravity = false;
            this.timeLeft = 0;
            this.timeLeftMax = 0;
            this.extraUpdate = 0;
            this.whoAmI = -1;
            this.tag = null;
            this.colorFlashSpeed = 0f;
            this.lightFlashSpeed = 0f;
            this.alphaFlashSpeed = 0f;
            this.scaleFlashSpeed = 0f;
            this.flash = new float[] { 0f, 0f, 0f, 0f };
            this.flashDir = new int[] { 1, 1, 1, 1 };
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset = default)
        {
            if (!this.active) return;
            float alpha = MyMath.Clamp(this.Alpha, 0, 1);
            Vector2 basic = this.position - Main.screenPosition;// + offset;
            Vector2 scale = new Vector2(MyMath.Max(0f, this.Scale.X), MyMath.Max(0f, this.Scale.Y));
            Vector2 scale2 = Main.GameViewMatrix.Zoom / Main.UIScale;
            if (PreDraw(spriteBatch, offset))
            {
                if (this.textureGlow != null)
                    spriteBatch.Draw(
                        this.textureGlow,
                        (basic * scale2) - Main.ScreenSize.ToVector2() / 2f * (scale2 - Vector2.One),
                        null,
                        this.Color * alpha,
                        this.rotation,
                        this.textureGlow.Size() / 2,
                        scale * this.scaleGlow,
                        0,
                        0);
                if (this.texture != null)
                    spriteBatch.Draw(
                        this.texture,
                        basic,
                        null,
                        this.Color * alpha,
                        this.rotation,
                        this.texture.Size() / 2,
                        scale,
                        0,
                        0);
            }
            PostDraw(spriteBatch, offset);
        }

        public virtual void OnSpawn() { }

        public virtual bool PreUpdate() => true;
        public virtual void PostUpdate() { }

        public virtual bool PreDraw(SpriteBatch spriteBatch, Vector2 offset = default) => true;
        public virtual void PostDraw(SpriteBatch spriteBatch, Vector2 offset = default) { }

        public static int NewParticle(Texture2D image, Vector2 pos, Vector2 vel, float rot, int timeLeft, Vector2 scale, Color color, Color light = default, bool noGravity = true, int extraUpdate = 0)
        {
            for (int i = 0; i < ParticleRenderer.maxParticles; i++)
            {
                if (ParticleRenderer.particle[i] == null || !ParticleRenderer.particle[i].active)
                {
                    ParticleRenderer.particle[i] = new Particle();

                    ParticleRenderer.particle[i].SetDefaults();
                    ParticleRenderer.particle[i].texture = image;
                    ParticleRenderer.particle[i].oldPosition = pos;
                    ParticleRenderer.particle[i].position = pos;
                    ParticleRenderer.particle[i].velocity = vel;
                    ParticleRenderer.particle[i].rotation = rot;
                    ParticleRenderer.particle[i].scale = scale;
                    ParticleRenderer.particle[i].scale2 = scale;
                    ParticleRenderer.particle[i].color = color;
                    ParticleRenderer.particle[i].color2 = color;
                    ParticleRenderer.particle[i].light = light;
                    ParticleRenderer.particle[i].light2 = light;
                    ParticleRenderer.particle[i].noGravity = noGravity;
                    ParticleRenderer.particle[i].timeLeft = timeLeft;
                    ParticleRenderer.particle[i].timeLeftMax = timeLeft;
                    ParticleRenderer.particle[i].extraUpdate = extraUpdate;
                    ParticleRenderer.particle[i].whoAmI = i;
                    ParticleRenderer.particle[i].active = true;
                    ParticleRenderer.particle[i].OnSpawn();
                    return i;
                }
            }
            return ParticleRenderer.maxParticles;
        }
    }

    public class ParticleRenderer : ModSystem
    {
        public const int maxParticles = CNC.maxParticle;
        public static Particle[] particle = new Particle[maxParticles + 1];
        public static bool drawn = false;

        public override void PreUpdateDusts() => this.UpdateParticle();

        public override void UpdateUI(GameTime gameTime) => ParticleRenderer.drawn = false;

        private void UpdateParticle()
        {
            for (int i = 0; i < maxParticles; i++)
            {
                if (ParticleRenderer.particle[i] == null) ParticleRenderer.particle[i] = new Particle();
                for (int j = 0; j <= ParticleRenderer.particle[i].extraUpdate; j++)
                {
                    if (ParticleRenderer.particle[i].active = ParticleRenderer.particle[i].timeLeft-- > 0)
                    {
                        if (ParticleRenderer.particle[i].PreUpdate())
                        {
                            float[] flashSpeed = new float[]
                            {
                                ParticleRenderer.particle[i].colorFlashSpeed,
                                ParticleRenderer.particle[i].lightFlashSpeed,
                                ParticleRenderer.particle[i].alphaFlashSpeed,
                                ParticleRenderer.particle[i].scaleFlashSpeed,
                            };
                            for(int k = 0; k < 4; k++)
                            {
                                ParticleRenderer.particle[i].flash[k] = MyMath.Clamp(ParticleRenderer.particle[i].flash[k] + ParticleRenderer.particle[i].flashDir[k] * flashSpeed[k], 0f, 1f);
                                if (ParticleRenderer.particle[i].flash[k] >= 1f) ParticleRenderer.particle[i].flashDir[k] = -1;
                                if (ParticleRenderer.particle[i].flash[k] <= 0f) ParticleRenderer.particle[i].flashDir[k] = 1;
                            }

                            ParticleRenderer.particle[i].oldPosition = ParticleRenderer.particle[i].position;
                            ParticleRenderer.particle[i].position += ParticleRenderer.particle[i].velocity;
                            ParticleRenderer.particle[i].rotation += ParticleRenderer.particle[i].rotationAdd;
                            ParticleRenderer.particle[i].scale += ParticleRenderer.particle[i].scaleAdd;
                            ParticleRenderer.particle[i].scale2 += ParticleRenderer.particle[i].scaleAdd;
                            ParticleRenderer.particle[i].scale *= ParticleRenderer.particle[i].scaleMul;
                            ParticleRenderer.particle[i].scale2 *= ParticleRenderer.particle[i].scaleMul;
                            ParticleRenderer.particle[i].alpha += ParticleRenderer.particle[i].alphaAdd;
                            ParticleRenderer.particle[i].alpha2 += ParticleRenderer.particle[i].alphaAdd;
                            ParticleRenderer.particle[i].velocity = (ParticleRenderer.particle[i].velocity + ParticleRenderer.particle[i].velocityAdd) * ParticleRenderer.particle[i].velocityMul;
                            //if (ParticleRenderer.particle[i].scale.X < 0) ParticleRenderer.particle[i].scale.X = 0;
                            //if (ParticleRenderer.particle[i].scale.Y < 0) ParticleRenderer.particle[i].scale.Y = 0;
                            if (!ParticleRenderer.particle[i].noGravity) ParticleRenderer.particle[i].velocity.Y += MyMath.g * .01f;
                            if (ParticleRenderer.particle[i].Light != Color.Transparent)
                            {
                                Point pos = (ParticleRenderer.particle[i].position / 16f).ToPoint();
                                Color light = ParticleRenderer.particle[i].Light;
                                float r = light.R * light.A / 255f * ParticleRenderer.particle[i].lightAlpha * (ParticleRenderer.particle[i].scale.X + ParticleRenderer.particle[i].scale.Y) / 2f;
                                float g = light.G * light.A / 255f * ParticleRenderer.particle[i].lightAlpha * (ParticleRenderer.particle[i].scale.X + ParticleRenderer.particle[i].scale.Y) / 2f;
                                float b = light.B * light.A / 255f * ParticleRenderer.particle[i].lightAlpha * (ParticleRenderer.particle[i].scale.X + ParticleRenderer.particle[i].scale.Y) / 2f;
                                Lighting.AddLight(pos.X, pos.Y, r, g, b);
                            }
                        }
                        ParticleRenderer.particle[i].PostUpdate();
                    }
                }
            }
        }
    }
}