
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

        public override void UpdateAI()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            Anim001_Loop(projectile, 5, 0);
            CNCUtils.AddLight(projectile.Center, Color.DarkCyan, .3f);
            if (len2 < len - 1) len2++;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < len2; i++)
            {
                Vector2 pos = projectile.oldPos[i] + projectile.Size / 2f + new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * projectile.Size * projectile.scale / 2f;
                Dust dust = CNCUtils.NewDust(projectile, DustId.ElectricCyan, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.FlashColor(Color.DarkCyan));
                dust.position = pos;
                dust.noGravity = true;
            }
            CNCUtils.PlaySound("Superconductor/Explosion", projectile.Center, 2f, mod);
            CNCUtils.PlaySound("Superconductor/Chain", projectile.Center, 1f, mod);
            Projectile p = this.NewProjectile(projectile.Center + projectile.oldVelocity, Const._02, CNCID.Projectile.LegacyObject[CNCID.Projectile.OverloadedLightningExplosion], projectile.damage, projectile.knockBack, CNC.Random.NextFloat(MyMath.pi2));
            (p.ModProjectile as CNCProjectile).Projectile_OnHitNPCEffect += this.Projectile_OnHitNPCEffect;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.LightBlue;
            Main.EntitySpriteDraw(
                Terraria.GameContent.TextureAssets.Projectile[Type].Value,
                projectile.Center - Main.screenPosition,
                new Rectangle(0, projectile.frame * 25, 13, 25),
                lightColor,
                projectile.rotation,
                new Vector2(6.5f, 12.5f),
                projectile.scale,
                0, 0
                );
            return false;
        }

        public override bool PreDrawExtras()
        {
            Texture2D[] image =
            {
                CNC.GetTexture("Assets/Effects/GlowTrail2"),
                CNC.GetTexture("Assets/Effects/Trace6"),
                CNC.GetTexture("Assets/Effects/Trail/Trail2"),
            };
            Color[] color =
            {
                Color.LightBlue,
                Color.LightCyan,
                Color.White,
                Color.White,
            };
            return Drawing.DrawTrail(Main.spriteBatch, projectile, image, color, 12, 0, true);
        }
    }
}