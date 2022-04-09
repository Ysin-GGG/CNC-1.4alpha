
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Superconductor
{
    public class OverloadedLightningMissile : CNCProjectile
    {
        public override int countFrame => 5;

        public override void ResetDefaults()
        {
            projectile.width = 13;
            projectile.height = 13;
            projectile.timeLeft = 300;
            projectile.knockBack = 2f;
            projectile.alpha = 0;
            projectile.scale = 2f;
            projectile.aiStyle = 1;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            Anim001_Loop(projectile, 5, 0);
            for (int i = 0; i < 2; i++)
            {
                Dust dust = CNCUtils.NewDust(projectile, 226, projectile.rotation, .8f, projectile.Size / 2f, CNCUtils.AccurateColor(Color.LightBlue));
                dust.velocity /= 2f;
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            CNCUtils.PlaySound("Superconductor/Explosion", projectile.Center, 2f, mod);
            CNCUtils.PlaySound("Superconductor/Chain", projectile.Center, 1f, mod);
            this.NewProjectile(projectile.Center + projectile.oldVelocity, Const._02, CNCID.Projectile.LegacyObject[CNCID.Projectile.OverloadedLightningExplosion], JustGetDamage(CNCID.Inscription.S_ArcaneOverload, 4), projectile.knockBack, CNC.Random.NextFloat(MyMath.pi2));
         }
    }
}