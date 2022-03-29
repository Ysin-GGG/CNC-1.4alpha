using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.FireGrimoire
{
    public class FireBallExplosion : CNCProjectile
    {
        public int level => data.ContainsKey("level") ? (int)data["level"] : 0;
        public override int countFrame => 5;
        private const int tpf = 5;//Time Per Frame

        public override void ResetDefaults()
        {
            data["level"] = 0;
            projectile.width = 60;
            projectile.height = 60;
            projectile.timeLeft = 23;
            projectile.knockBack = 2f;
            projectile.alpha = 32;
            projectile.scale = 1f;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Magic;
        }
        
        public override void AI()
        {
            Anim001_Loop(projectile, tpf);
            projectile.scale += .04f;
            projectile.alpha += 8;
            if (frame > 0) projectile.friendly = false; 
            if (frame < 3)
            {
                Dust dust = Dust.NewDustDirect(projectile.position + projectile.velocity, projectile.width, projectile.height, DustId.Fire, 0f, 0f, 100, Color.Red, 3f);
                dust.noGravity = true;
                dust.scale -= .05f;
            }
            Color color = new Color(242, 92, 34); //projectile.scale;
            CNCUtils.AddLight(projectile.position, color, 0.8f);
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 300);
            if (this.level > 0) target.AddBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Scorched], 300);
        }
    }
}