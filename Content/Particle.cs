using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public Vector2 oldPosition;
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D texture;
        public Texture2D textureGlow;
        public float oldRotation;
        public float rotation;
        public float rotationAdd;
        public bool active;
        public bool noGravity;
        public int timeLeft;
        public int timeLeftMax;
        public int extraUpdate;
        public int whoAmI;

        public Particle() => this.SetDefaults();

        public void SetDefaults()
        {
            this.color = Color.White;
            this.light = Color.Transparent;
            this.lightAlpha = .1f;
            this.scale = Vector2.One;
            this.scaleAdd = Vector2.Zero;
            this.scaleMul = Vector2.One;
            this.oldPosition = Vector2.Zero;
            this.position = Vector2.Zero;
            this.velocity = Vector2.Zero;
            this.texture = null;
            this.textureGlow = null;
            this.rotation = 0f;
            this.rotationAdd = 0f;
            this.active = false;
            this.noGravity = false;
            this.timeLeft = 0;
            this.timeLeftMax = 0;
            this.extraUpdate = 0;
            this.whoAmI = -1;
        }

        public virtual void OnSpawn() { }

        public virtual void Update() { }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset = default)
        {
            if (!this.active) return;
            if (this.textureGlow != null)
                spriteBatch.Draw(
                    this.textureGlow,
                    this.position + offset - Main.screenPosition,
                    null,
                    this.color,
                    this.rotation,
                    this.texture.Size() / 2,
                    this.scale,
                    0,
                    0);
            if (this.texture != null)
                spriteBatch.Draw(
                    this.texture,
                    this.position + offset - Main.screenPosition,
                    null,
                    this.color,
                    this.rotation,
                    this.texture.Size() / 2,
                    this.scale,
                    0,
                    0);
        }

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
        public const int maxParticles = 1000;
        public static Particle[] particle = new Particle[maxParticles + 1];

        public override void PreUpdateDusts() => this.UpdateParticle();

        private void UpdateParticle()
        {
            for (int i = 0; i < maxParticles; i++)
            {
                if (particle[i] == null) particle[i] = new Particle();
                for (int j = 0; j <= particle[i].extraUpdate; j++) 
                if (particle[i].active = particle[i].timeLeft-- > 0)
                {
                    particle[i].oldPosition = particle[i].position;
                    particle[i].position += particle[i].velocity;
                    particle[i].rotation += particle[i].rotationAdd;
                    particle[i].scale += particle[i].scaleAdd;
                    particle[i].scale *= particle[i].scaleMul;
                    if (particle[i].scale.X < 0) particle[i].scale.X = 0;
                    if (particle[i].scale.Y < 0) particle[i].scale.Y = 0;
                    if (!particle[i].noGravity) particle[i].velocity.Y += .0980f;
                    if (particle[i].light != Color.Transparent)
                    {
                        Point pos = (particle[i].position / 16f).ToPoint();
                        Color light = particle[i].light;
                        float r = light.R * light.A / 255f * particle[i].lightAlpha * (particle[i].scale.X + particle[i].scale.Y) / 2f;
                        float g = light.G * light.A / 255f * particle[i].lightAlpha * (particle[i].scale.X + particle[i].scale.Y) / 2f;
                        float b = light.B * light.A / 255f * particle[i].lightAlpha * (particle[i].scale.X + particle[i].scale.Y) / 2f;
                        Lighting.AddLight(pos.X, pos.Y, r, g, b);
                    }
                    particle[i].Update();
                }
            }
        }
    }
}