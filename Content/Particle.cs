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
            Vector2 basic = this.position - Main.screenPosition + offset;
            Vector2 scale = new Vector2(MyMath.Max(0f, this.Scale.X), MyMath.Max(0f, this.Scale.Y));
            Vector2 transform = Main.GameViewMatrix.Zoom;
            if (CNC.Me.gravDir == -1) basic.Y += Main.ScreenSize.Y - basic.Y * 2;
            Vector2 pos = basic;// (basic * transform) - Main.ScreenSize.ToVector2() / 2f * (transform - Vector2.One);
            if (PreDraw(spriteBatch, offset))
            {
                if (this.textureGlow != null)
                    spriteBatch.Draw(
                        this.textureGlow,
                        pos,
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
                        pos,
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
            for (int i = 0; i < ParticleManager.maxParticles; i++)
            {
                if (ParticleManager.particle[i] == null || !ParticleManager.particle[i].active)
                {
                    ParticleManager.particle[i] = new Particle();
                    ParticleManager.particle[i].SetDefaults();
                    ParticleManager.particle[i].texture = image;
                    ParticleManager.particle[i].oldPosition = pos;
                    ParticleManager.particle[i].position = pos;
                    ParticleManager.particle[i].velocity = vel;
                    ParticleManager.particle[i].rotation = rot;
                    ParticleManager.particle[i].scale = scale;
                    ParticleManager.particle[i].scale2 = scale;
                    ParticleManager.particle[i].color = color;
                    ParticleManager.particle[i].color2 = color;
                    ParticleManager.particle[i].light = light;
                    ParticleManager.particle[i].light2 = light;
                    ParticleManager.particle[i].noGravity = noGravity;
                    ParticleManager.particle[i].timeLeft = timeLeft;
                    ParticleManager.particle[i].timeLeftMax = timeLeft;
                    ParticleManager.particle[i].extraUpdate = extraUpdate;
                    ParticleManager.particle[i].whoAmI = i;
                    ParticleManager.particle[i].active = true;
                    ParticleManager.particle[i].OnSpawn();
                    return i;
                }
            }
            return ParticleManager.maxParticles;
        }
    }

    public class ParticleManager : ModSystem
    {
        public const int maxParticles = CNC.maxParticles;
        public static Particle[] particle = new Particle[maxParticles + 1];

        public override void PreUpdateDusts() => this.UpdateParticle();
        
        private void UpdateParticle()
        {
            for (int i = 0; i < maxParticles; i++)
            {
                if (ParticleManager.particle[i] == null) ParticleManager.particle[i] = new Particle();
                for (int j = 0; j <= ParticleManager.particle[i].extraUpdate; j++)
                {
                    if (ParticleManager.particle[i].active = ParticleManager.particle[i].timeLeft-- > 0)
                    {
                        if (ParticleManager.particle[i].PreUpdate())
                        {
                            float[] flashSpeed = new float[]
                            {
                                ParticleManager.particle[i].colorFlashSpeed,
                                ParticleManager.particle[i].lightFlashSpeed,
                                ParticleManager.particle[i].alphaFlashSpeed,
                                ParticleManager.particle[i].scaleFlashSpeed,
                            };
                            for(int k = 0; k < 4; k++)
                            {
                                ParticleManager.particle[i].flash[k] = MyMath.Clamp(ParticleManager.particle[i].flash[k] + ParticleManager.particle[i].flashDir[k] * flashSpeed[k], 0f, 1f);
                                if (ParticleManager.particle[i].flash[k] >= 1f) ParticleManager.particle[i].flashDir[k] = -1;
                                if (ParticleManager.particle[i].flash[k] <= 0f) ParticleManager.particle[i].flashDir[k] = 1;
                            }

                            ParticleManager.particle[i].oldPosition = ParticleManager.particle[i].position;
                            ParticleManager.particle[i].position += ParticleManager.particle[i].velocity;
                            ParticleManager.particle[i].rotation += ParticleManager.particle[i].rotationAdd;
                            ParticleManager.particle[i].scale += ParticleManager.particle[i].scaleAdd;
                            ParticleManager.particle[i].scale2 += ParticleManager.particle[i].scaleAdd;
                            ParticleManager.particle[i].scale *= ParticleManager.particle[i].scaleMul;
                            ParticleManager.particle[i].scale2 *= ParticleManager.particle[i].scaleMul;
                            ParticleManager.particle[i].alpha += ParticleManager.particle[i].alphaAdd;
                            ParticleManager.particle[i].alpha2 += ParticleManager.particle[i].alphaAdd;
                            ParticleManager.particle[i].velocity = (ParticleManager.particle[i].velocity + ParticleManager.particle[i].velocityAdd) * ParticleManager.particle[i].velocityMul;
                            //if (ParticleRenderer.particle[i].scale.X < 0) ParticleRenderer.particle[i].scale.X = 0;
                            //if (ParticleRenderer.particle[i].scale.Y < 0) ParticleRenderer.particle[i].scale.Y = 0;
                            if (!ParticleManager.particle[i].noGravity) ParticleManager.particle[i].velocity.Y += MyMath.g * .01f;
                            if (ParticleManager.particle[i].Light != Color.Transparent)
                            {
                                Point pos = (ParticleManager.particle[i].position / 16f).ToPoint();
                                Color light = ParticleManager.particle[i].Light;
                                float r = light.R * light.A / 255f * ParticleManager.particle[i].lightAlpha * (ParticleManager.particle[i].scale.X + ParticleManager.particle[i].scale.Y) / 2f;
                                float g = light.G * light.A / 255f * ParticleManager.particle[i].lightAlpha * (ParticleManager.particle[i].scale.X + ParticleManager.particle[i].scale.Y) / 2f;
                                float b = light.B * light.A / 255f * ParticleManager.particle[i].lightAlpha * (ParticleManager.particle[i].scale.X + ParticleManager.particle[i].scale.Y) / 2f;
                                Lighting.AddLight(pos.X, pos.Y, r, g, b);
                            }
                        }
                        ParticleManager.particle[i].PostUpdate();
                    }
                }
            }
        }
    }
}