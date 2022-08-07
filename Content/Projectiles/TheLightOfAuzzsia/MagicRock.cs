using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Players;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.TheLightOfAuzzsia
{
    public class MagicRock : CNCProjectile
    {
        public override void ResetDefaults()
        {
            projectile.width = 42;
            projectile.height = 40;
            projectile.timeLeft = 600;
            projectile.knockBack = 7f;
            projectile.alpha = 32;
            projectile.scale = 0.8f;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            CNCUtils.AddLight(projectile.Center, Color.Purple, 1f);
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = CNCUtils.NewDust(projectile, DustId.GreyStone, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.AccurateColor(Color.White));
                dust.alpha = 1;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = CNCUtils.NewDust(projectile, DustId.GreyStone, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.AccurateColor(Color.White));
                dust.alpha = 1;
            }
            CNCUtils.PlaySound("TheLightOfAuzzsia/RockHit", projectile, .5f);
        }
    }
}
