
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Players;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Superconductor
{
    public class LightningMissile : CNCProjectile
    {
        public override int countFrame => 5;
        public bool overload => this.data.ContainsKey("overload") ? (bool)this.data["overload"] : false;
        public bool overloadUpgraded => (this.overload && this.data.ContainsKey("overloadUpgraded")) ? (bool)this.data["overloadUpgraded"] : false;

        public override void ResetDefaults()
        {
            projectile.width = 13;
            projectile.height = 13;
            projectile.timeLeft = 300;
            projectile.knockBack = 2f;
            projectile.alpha = 32;
            projectile.scale = 1.5f;
            projectile.aiStyle = 1;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player p = Main.player[projectile.owner];
            InscriptionPlayer gp = p.GetModPlayer<InscriptionPlayer>();
            if (this.overload)
            {
                int time = (int)(overloadUpgraded ? CNCID.Inscription.S_StableSpell.Data[0] : CNCID.Inscription.S_ArcaneOverload.Data[2]) * 60;
                gp.overloadLevelCounter = time;
                if (++gp.overloadLevel >= 10)
                {
                    p.AddBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Overloaded], time);
                    gp.overloadLevelCounter = 0;
                }
            }
        }

        public override void AI()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            Anim001_Loop(projectile, 5, 0);
            Vector4 xywh = new Vector4(projectile.Center.X, projectile.Center.Y, 1, 1);
            Dust dust = CNCUtils.NewDust(xywh, 226, projectile.rotation, .8f, projectile.Size / 2f, CNCUtils.FlashColor(Color.LightBlue));
            dust.position = projectile.position + CNC.Random.NextVector2(new Vector2(12f));
            dust.velocity *= .2f;
            dust.noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        public override void Kill(int timeLeft)
        {
            CNCUtils.PlaySound("Superconductor/Explosion", projectile.position, 1.2f, 1f, mod);
            this.NewProjectile(projectile.Center + projectile.oldVelocity, Const._02, CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningExplosion], 0, 0, CNC.Random.NextDegree());
         }
    }
}