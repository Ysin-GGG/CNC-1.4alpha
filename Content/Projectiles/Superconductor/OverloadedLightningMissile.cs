
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

        const int len = 20;

        private int pointX = 0;
        private int len2 = 0;

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
            if (len != projectile.oldPos.Length)
            {
                Array.Resize(ref projectile.oldPos, len);
                Array.Resize(ref projectile.oldRot, len);
            }
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                projectile.oldRot[i] = 0.0f;
                projectile.oldPos[i].X = 0.0f;
                projectile.oldPos[i].Y = 0.0f;
            }
        }

        public override void AI()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            Anim001_Loop(projectile, 5, 0);
            UpdateOldData(projectile, projectile.velocity);
            if (len2 < len - 1) len2++;
        }

        public override void Kill(int timeLeft)
        {
            CNCUtils.PlaySound("Superconductor/Explosion", projectile.Center, 2f, mod);
            CNCUtils.PlaySound("Superconductor/Chain", projectile.Center, 1f, mod);
            this.NewProjectile(projectile.Center + projectile.oldVelocity, Const._02, CNCID.Projectile.LegacyObject[CNCID.Projectile.OverloadedLightningExplosion], JustGetDamage(CNCID.Inscription.S_ArcaneOverload, 4), projectile.knockBack, CNC.Random.NextFloat(MyMath.pi2));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.LightBlue;
            return true;
        }

        public override bool PreDrawExtras()
        {
            Texture2D tail = ModContent.Request<Texture2D>("CNC/Assets/Effects/Trail2").Value;
            Color color = Color.LightBlue; color.A = 0;
            Drawing.DrawTrailDirect(Main.spriteBatch, tail, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, color, pointX, tail.Height / 2, new Vector2(1f, .1f), new Vector2(1f, .01f), true, 8);
            pointX = (pointX - 8 + tail.Width) % tail.Width;
            return true;
        }
    }
}