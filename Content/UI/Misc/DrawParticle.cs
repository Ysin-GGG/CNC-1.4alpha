using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.UI.Misc
{
    public class DrawParticle : UIState
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
                if (ParticleRenderer.particle[i] != null && ParticleRenderer.particle[i].active)
                {
                    Vector2 basic = ParticleRenderer.particle[i].position - Main.screenPosition;
                    Vector2 scale = Main.GameViewMatrix.Zoom / Main.UIScale;
                    ParticleRenderer.particle[i].Draw(spriteBatch, ((basic / Main.UIScale) - basic));
                }
        }
    }
}