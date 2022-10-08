using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.ArgenteusClaw
{
    public class SilveryArrow : CNCProjectile
    {
        public override void ResetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.timeLeft = 600;
            projectile.alpha = 255;
            projectile.light = 0;
            projectile.penetrate = 2;
            projectile.aiStyle = -1;
            projectile.extraUpdates = 1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            CNCUtils.AddLight(projectile.position, Color.Silver);
            AI001_RollingMissile(projectile, projectile.timeLeft < 560, true, 90f);
            projectile.alpha = MyMath.Clamp(projectile.alpha - 15, 0, 255);
            Color color = Color.Silver; color.A = 0;
            int idx = Particle.NewParticle(
                ModContent.Request<Texture2D>("CNC/Assets/Particles/GlowTrail").Value,
                projectile.Center,
                default,
                projectile.velocity.ToRotation(),
                20,
                new Vector2(MyMath.GetZ(velX,velY) / ModContent.Request<Texture2D>("CNC/Assets/Particles/GlowTrail").Value.Width, .1f),
                color,
                new Color(color.R, color.G, color.B, 5)
                );
            ParticleManager.particle[idx].scaleAdd.Y = -.005f;
            ParticleManager.particle[idx].alphaAdd = -.05f;
            ParticleManager.particle[idx].alpha = 1f;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }
    }
}