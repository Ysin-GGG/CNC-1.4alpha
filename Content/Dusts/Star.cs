using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.Dusts
{
    public class Star : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            int idx = Particle.NewParticle(
                ModContent.Request<Texture2D>("CNC/Content/Dusts/Star").Value,
                dust.position,
                dust.velocity,
                dust.rotation,
                100,
                Vector2.One,
                new Color(255, 255, 255, 0),
                new Color(255, 255, 255, 5)
                );
            ParticleRenderer.particle[idx].scaleAdd = new Vector2(-Main.rand.NextFloat(.05f, .01f));
            ParticleRenderer.particle[idx].rotationAdd = Common.Utils.MyMath.deg01 * Main.rand.Next(1, 16);
            dust.scale *= 0;
            dust.active = false;
        }
    }
}