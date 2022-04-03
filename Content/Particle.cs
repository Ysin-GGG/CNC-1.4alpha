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
        public Color light;
        public float lightAlpha;
        public Vector2 scale;
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
        public float alphaAdd;
        public bool active;
        public bool noGravity;
        public int timeLeft;
        public int timeLeftMax;
        public int extraUpdate;
        public int whoAmI;
        public string tag;

        public Particle() => this.SetDefaults();

        public void SetDefaults()
        {
            this.color = Color.White;
            this.light = Color.Transparent;
            this.lightAlpha = .1f;
            this.scale = Vector2.One;
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
            this.alphaAdd = 0f;
            this.active = false;
            this.noGravity = false;
            this.timeLeft = 0;
            this.timeLeftMax = 0;
            this.extraUpdate = 0;
            this.whoAmI = -1;
            this.tag = null;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset = default)
        {
            if (!this.active) return;
            float alpha = MyMath.Clamp(this.alpha, 0, 1);
            if (PreDraw(spriteBatch, offset))
            {
                if (this.textureGlow != null)
                    spriteBatch.Draw(
                        this.textureGlow,
                        this.position + offset - Main.screenPosition,
                        null,
                        this.color * alpha,
                        this.rotation,
                        this.textureGlow.Size() / 2,
                        this.scale * this.scaleGlow,
                        0,
                        0);
                if (this.texture != null)
                    spriteBatch.Draw(
                        this.texture,
                        this.position + offset - Main.screenPosition,
                        null,
                        this.color * alpha,
                        this.rotation,
                        this.texture.Size() / 2,
                        this.scale,
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
                    ParticleRenderer.particle[i].color = color;
                    ParticleRenderer.particle[i].light = light;
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
        public const int maxParticles = 10000;
        public static Particle[] particle = new Particle[maxParticles + 1];

        public override void PreUpdateDusts() => this.UpdateParticle();

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
                            ParticleRenderer.particle[i].oldPosition = ParticleRenderer.particle[i].position;
                            ParticleRenderer.particle[i].position += ParticleRenderer.particle[i].velocity;
                            ParticleRenderer.particle[i].rotation += ParticleRenderer.particle[i].rotationAdd;
                            ParticleRenderer.particle[i].scale += ParticleRenderer.particle[i].scaleAdd;
                            ParticleRenderer.particle[i].scale *= ParticleRenderer.particle[i].scaleMul;
                            ParticleRenderer.particle[i].alpha += ParticleRenderer.particle[i].alphaAdd;
                            ParticleRenderer.particle[i].velocity = (ParticleRenderer.particle[i].velocity + ParticleRenderer.particle[i].velocityAdd) * ParticleRenderer.particle[i].velocityMul;
                            if (ParticleRenderer.particle[i].scale.X < 0) ParticleRenderer.particle[i].scale.X = 0;
                            if (ParticleRenderer.particle[i].scale.Y < 0) ParticleRenderer.particle[i].scale.Y = 0;
                            if (!ParticleRenderer.particle[i].noGravity) ParticleRenderer.particle[i].velocity.Y += MyMath.g * .01f;
                            if (ParticleRenderer.particle[i].light != Color.Transparent)
                            {
                                Point pos = (ParticleRenderer.particle[i].position / 16f).ToPoint();
                                Color light = ParticleRenderer.particle[i].light;
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