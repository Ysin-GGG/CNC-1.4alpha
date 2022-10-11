using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using CNC.Content.DataStructures;
using System.Collections.Generic;

namespace CNC.Content.Projectiles.Remakes
{
    public class SolarHalo : CNCProjectile
    {
        private const int time = 200;
        private Vector2 scale;
        private int timer => time - projectile.timeLeft;

        public override void ResetDefaults()
        {
            projectile.timeLeft = time;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.scale = 1f;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.velocity = Vector2.Zero;
            scale = new Vector2(1f, .4f) * .1f;
        }

        public override void AI()
        {
            if (timer < 25)
            {
                if (Alpha2 < 1f)
                {
                    Alpha2 += .04f;
                    if(Alpha2 <= .5f) projectile.Center = owner.Center;
                }
                if (scale.X < 1f) scale += new Vector2(1f, .4f) * .04f;
            }
            if (35 <= timer && timer < 60)
            {
                if (scale.Y < 1f) scale.Y += .04f;
            }
            if (projectile.timeLeft < 30)
            {
                scale -= Vector2.One * .005f;
                if (Alpha2 > 0f) Alpha2 -= .0334f;
            }
            if (scale.X * scale.Y >= 1f && CNC.Random.NextBool(25))
            {
                Vector2 pos = projectile.Center + new Vector2(512 + 16 - CNC.Random.NextInt(116), 0f).RotatedBy(CNC.Random.NextDegree());
                Color color = CNCID.Colors.Expert;
                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Glow/Cross").Value,
                    pos,
                    Vector2.Zero,
                    0f,
                    60,
                    new Vector2(1f, 1f / MyMath.phi) * CNC.Random.NextFloat(.25f, .35f),
                    color.NewA(0),
                    color.NewA(5)
                    );
                ParticleManager.particle[idx].scaleAdd = new Vector2(-.002f);
                ParticleManager.particle[idx].alphaAdd = -.018f;
                ParticleManager.particle[idx].alpha = .5f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float dist = targetHitbox.Center.ToVector2().Distance(projectile.Center);
            float range = 100f;
            float extraRange = 16f;
            bool full = scale.X * scale.Y >= 1f;
            bool inRange = (512f - range - extraRange) <= dist && dist <= 512f + extraRange;
            return full && inRange;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(Texture2, projectile.Center - Main.screenPosition, null, Color.White.Flash().NewA(0) * this.Alpha2 * this.Alpha2 * .15f, projectile.rotation, Texture2.Center(), projectile.scale * scale, 0, 0);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
