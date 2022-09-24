
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
    public class LightningExplosion : CNCProjectile
    {
        public override int countFrame => 6;

        public override void ResetDefaults()
        {
            projectile.width = 76;
            projectile.height = 72;
            projectile.timeLeft = 24;
            projectile.knockBack = 0f;
            projectile.alpha = 0;
            projectile.scale = 1;
            projectile.light = 0.5f;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Anim001_Loop(projectile, 4);
            projectile.scale += .01f;
            if (projectile.timeLeft > 14)
            {
                Dust dust = CNCUtils.NewDust(projectile, DustId.ElectricCyan, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.FlashColor(Color.DarkCyan));
                dust.noGravity = true;
                CNCUtils.AddLight(projectile.Center, dust.color, .4f);
            }
        }
    }
}