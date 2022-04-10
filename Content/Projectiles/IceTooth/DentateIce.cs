
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.IceTooth
{
    public class DentateIce : CNCProjectile
    {
        public bool flake => data.ContainsKey("flake") ? (bool)data["flake"] : false;
        public bool frozen => data.ContainsKey("frozen") ? (bool)data["frozen"] : false;
        NPC npc = null;

        public override int countFrame => 5;

        public override void ResetDefaults()
        {
            data["add"] = 0f;
            data["mul"] = 1f;
            data["flake"] = false;
            data["frozen"] = false;
            projectile.width = 20;
            projectile.height = 10;
            projectile.timeLeft = 600;
            projectile.knockBack = 2f;
            projectile.alpha = 64;
            projectile.scale = 1.15f;
            projectile.penetrate = 1;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void OnSpawn()
        {
            projectile.frame = Main.rand.Next(0, 4);
        }

        public override void AI()
        {
            AI001_RollingMissile(projectile, false, true, 90f);
            Dust dust = Dust.NewDustDirect(projectile.position, 14, 14, DustID.IceTorch, 0f, 0f, 100, Color.LightBlue, 1f);
            dust.noGravity = true;
            CNCUtils.AddLight(projectile.position, new Color(0, 170, 255), .2f);
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int buff1 = CNCID.Buff.LegacyObject[CNCID.Buff.Gelid];
            int buff2 = CNCID.Buff.LegacyObject[CNCID.Buff.Glacier];
            projectile.velocity = projectile.oldVelocity;
            if (this.flake)
            {
                projectile.friendly = false;
                projectile.tileCollide = false;
                projectile.damage = 0;
                npc = target;
                for (int i = 0; i < 16; i++)
                {
                    Vector2 dustPos = new Vector2(projectile.Center.X - projectile.width, projectile.Center.Y);
                    Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.IceTorch, projectile.oldVelocity.X / -2f, projectile.oldVelocity.Y * -2f, 100, Color.LightBlue, 1.5f);
                }
                if (target.HasBuff(buff1) && !target.HasBuff(buff2) && Main.rand.Next(10) == 0 && this.frozen) target.AddBuff(buff2, 90);
                else target.AddBuff(buff1, 300);
            }
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (this.flake) NewProjectile(0f, 0f, 0, projectile.damage, projectile.knockBack);
            return base.OnTileCollide(oldVelocity);
        }
        
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, projectile.Center);
            for (int i = 0; i < 16; i++)
            {
                Vector2 dustPos = new Vector2(projectile.Center.X - projectile.width, projectile.Center.Y);
                Dust.NewDust(dustPos, projectile.width, projectile.height, DustID.IceTorch, projectile.oldVelocity.X / -2f, projectile.oldVelocity.Y * 2f, 100, Color.LightBlue, 1.5f);
            }
            if (this.flake)
            {
                int type = CNCID.Projectile.LegacyObject[CNCID.Projectile.DentateIceExplosion];
                Projectile proj = this.NewProjectile(0f, 0f, type, Inscription.JustGetDamage(projectile.damage, (string)CNCID.Inscription.S_Iceflake.Data[0], (float)data["add"], (float)data["mul"]), projectile.knockBack, Main.rand.NextFloat(0, MyMath.pi * 2));
                proj.GetGlobalProjectile<AllProjectile>().data["target"] = npc != null ? npc.whoAmI : -1;
                proj.GetGlobalProjectile<AllProjectile>().data["frozen"] = this.frozen;
                proj.owner = this.projectile.owner;
            }
        }
    }
}