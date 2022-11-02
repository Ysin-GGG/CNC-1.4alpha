
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.ArgenteusClaw
{
    public class SanctifiedArrowShoot : CNCProjectile
    {
        public override int countFrame => 3;

        public override void ResetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.timeLeft = 15;
        }

        public override void OnSpawn()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            //projectile.position += projectile.velocity;
            projectile.velocity *= 0f;
        }

        public override void UpdateAI()
        {
            Anim001_Loop(projectile, 5);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }
    }
}
