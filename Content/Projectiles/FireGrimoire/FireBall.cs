using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using System.Collections.Generic;

namespace CNC.Content.Projectiles.FireGrimoire
{
    public class FireBall : CNCProjectile
    {
        public int level => data.ContainsKey("level") ? (int)data["level"] : 0;
        public override int countFrame => 7;
        private const int tpf = 4;//Time Per Frame
        private const int lenMax = 17;
        private int len = 0;
        private int pointX = 0;

        public override void ResetDefaults()
        {
            data["level"] = 0;
            projectile.width = 10;
            projectile.height = 10;
            projectile.timeLeft = 600;
            projectile.knockBack = 2f;
            projectile.alpha = 0;
            projectile.scale = 1.5f;
            projectile.penetrate = 1;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ignoreWater = false;
            projectile.DamageType = DamageClass.Magic;
            if (projectile.oldPos.Length != lenMax)
            {
                Array.Resize(ref projectile.oldPos, lenMax);
                Array.Resize(ref projectile.oldRot, lenMax);
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
            Anim001_Loop(projectile, tpf);
            UpdateOldData(projectile, projectile.velocity);
            projectile.velocity = projectile.oldVelocity;
            if (len < lenMax) len++;
            if (projectile.timeLeft < 597)
            {
                Color color = new Color(242, 92, 34);// * projectile.scale;
                CNCUtils.AddLight(projectile.position, color, .8f);
                Dust dust = Dust.NewDustDirect(projectile.position, (int)(projectile.width * projectile.scale), (int)(projectile.height * projectile.scale), DustId.Fire, 0f, 0f, 100, Color.Red, 2f);
                dust.noGravity = true;
                dust.velocity *= 2;
            }
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 300);
            if (this.level > 0) target.AddBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Scorched], 300);
        }

        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < len; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.oldPos[i], (int)(projectile.width * projectile.scale), (int)(projectile.height * projectile.scale), DustId.Fire, 0f, 0f, 100, Color.Red, 2f - i / len);
                dust.noGravity = true;
                dust.velocity = projectile.velocity / 8f;
            }
            Projectile proj = this.NewProjectile(0f, 0f, CNCID.Projectile.LegacyObject[CNCID.Projectile.FireBallExplosion], Inscription.JustGetDamage(projectile.damage, (string)CNCID.Inscription.S_Incinerating.Data[0]), projectile.knockBack, MyMath.deg01 * Main.rand.Next(0, 359));
            proj.GetGlobalProjectile<AllProjectile>().data["level"] = level;
            proj.owner = this.projectile.owner;
            SoundEngine.PlaySound(SoundID.Item100, projectile.position);
        }

        public override bool PreDrawExtras()
        {
            Texture2D glowTrail = ModContent.Request<Texture2D>("CNC/Assets/Effects/GlowTrail1").Value;
            Texture2D trail = ModContent.Request<Texture2D>("CNC/Assets/Effects/Trail3").Value;
            Color color = new Color(242, 92, 34, 0);
            Drawing.DrawTrailDirect(Main.spriteBatch, glowTrail, len, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, color, pointX, glowTrail.Height / 2, new Vector2(1, .05f), new Vector2(1, .05f), true, 16, 1);
            Drawing.DrawTrailDirect(Main.spriteBatch, trail, Math.Max(0, len - 2), projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, color, pointX, trail.Height / 2, new Vector2(1, .19f), new Vector2(1, .05f), true, 16, 2);
            pointX = (pointX - 15 + trail.Width) % trail.Width;
            return true;
        }
    }
}