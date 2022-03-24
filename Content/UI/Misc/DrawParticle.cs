using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.UI.Misc
{
    public class DrawParticleBehind : UIState
    {
        public static bool Visible = true;

        public override void Initialize()
        {
            Size = Vector2.One;
            Offset = Vector2.Zero;
            Visible = true;
            CanDrag = false;
            CanOutScreen = true;
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ParticleRenderer.maxParticles; i++)
                if (ParticleRenderer.particle[i] != null)
                    if (ParticleRenderer.particle[i].behindPlayer)
                        ParticleRenderer.particle[i].Draw(spriteBatch, (ParticleRenderer.particle[i].position - Main.screenPosition) / Main.UIScale - (ParticleRenderer.particle[i].position - Main.screenPosition));
        }
    }

    public class DrawParticleOver : UIState
    {
        public static bool Visible = true;

        public override void Initialize()
        {
            Size = Vector2.One;
            Offset = Vector2.Zero;
            Visible = true;
            CanDrag = false;
            CanOutScreen = true;
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ParticleRenderer.maxParticles; i++)
                if (ParticleRenderer.particle[i] != null)
                    if (!ParticleRenderer.particle[i].behindPlayer)
                        ParticleRenderer.particle[i].Draw(spriteBatch, (ParticleRenderer.particle[i].position - Main.screenPosition) / Main.UIScale - (ParticleRenderer.particle[i].position - Main.screenPosition));
        }
    }
}