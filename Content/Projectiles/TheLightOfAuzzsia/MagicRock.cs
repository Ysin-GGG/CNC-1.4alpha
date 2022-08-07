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
            projectile.aiStyle = 1;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Dust dust=CNCUtils.NewDust(projectile,DustId.Wood, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.AccurateColor(Color.White));
        }

        public override void Kill(int timeLeft)
        {
            Dust dust = CNCUtils.NewDust(projectile, DustId.Wood, projectile.rotation, 1.2f, projectile.Size / 4f, CNCUtils.AccurateColor(Color.White));
        }
    }
}
