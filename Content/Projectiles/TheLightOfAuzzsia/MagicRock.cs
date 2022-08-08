using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.TheLightOfAuzzsia
{
    public class MagicRock : CNCProjectile
    {
        private float rot = 0;
        public override void ResetDefaults()
        {
            projectile.width = 42;
            projectile.height = 40;
            projectile.timeLeft = 200;
            projectile.knockBack = 7f;
            projectile.alpha = 32;
            projectile.scale = 0.8f;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = CNCUtils.NewDust(projectile, DustId.PurpleLight, projectile.rotation, 1.2f, projectile.Size*0.5f / 4f, CNCUtils.AccurateColor(Color.White));
                dust.position = projectile.Center;
                dust.velocity *= 0.4f;
                //dust.fadeIn = 0.9f;
                rot += MyMath.deg01;
            }
            CNCUtils.AddLight(projectile.Center, Color.Purple, 1.5f);
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = CNCUtils.NewDust(projectile, DustId.GreyStone, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.AccurateColor(Color.White));
                dust.alpha = 1;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = CNCUtils.NewDust(projectile, DustId.GreyStone, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.AccurateColor(Color.White));
                dust.alpha = 1;
            }
            CNCUtils.PlaySound("TheLightOfAuzzsia/RockHit", projectile, .5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D MagicAura = ModContent.Request<Texture2D>("CNC/Content/Projectiles/TheLightOfAuzzsia/MagicAura").Value;
            Vector2 pos = Main.player[projectile.owner].position-Main.screenPosition;
            Color color = Color.White;
            color.A = 0;
            Main.EntitySpriteDraw(MagicAura, pos, null, color * (1 - projectile.alpha / 255f), rot, MagicAura.Size() / 2, .2f, 0, 1);
            CNCUtils.AddLight(pos, Color.Purple, 2f);
            return true;
        }
    }
}
