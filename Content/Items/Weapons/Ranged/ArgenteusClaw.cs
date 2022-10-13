using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Items.Weapons.Ranged
{
	public class ArgenteusClaw : CNCItem
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();
        public bool SanctityEnable => modItem.ExistInscription(CNCID.Inscription.S_Sanctity);
        public bool ExorcismEnable => modItem.ExistInscription(CNCID.Inscription.S_Exorcism);

        public override void ResetDefaults()
        {
            item.width = 20;
            item.height = 68;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 2;
            item.damage = 47;
            item.crit = 4;
            item.knockBack = 2f;
            item.useStyle = 5;
            item.value = 50000;
            item.noMelee = true;
            item.autoReuse = true;
            item.useAmmo = AmmoID.Arrow;
            item.DamageType = DamageClass.Ranged;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.SilveryArrow];
            item.shootSpeed = 12f;
            clazz = 21;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj1 = CNCID.Projectile.LegacyObject[CNCID.Projectile.SilveryArrow];
            int proj2 = CNCID.Projectile.LegacyObject[CNCID.Projectile.SanctifiedArrow]; 
            if (type == 1) { type = proj1; }
            if (this.SanctityEnable && CNC.Random.NextBool(45)) { type = proj2; damage *= 2; }
            if (type == proj2) { CNCUtils.PlaySound("ArgenteusClaw/Shoot2", player, 2f, this.mod); }
            else { CNCUtils.PlaySound("ArgenteusClaw/Shoot1", player, this.mod); }
            //if (type == proj2) { CNCUtils.NewProj(source, position, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.SanctifiedArrowShoot], 0, 0); }
            if (this.ExorcismEnable && type == proj2)
            {
                Projectile p2 = CNCUtils.NewProj(source, position, velocity, proj2, damage, knockback, player.whoAmI, 0f, 1f, 0f, 0f);
                p2.GetGlobalProjectile<AllProjectile>().data["explosion"] = true;
                type = 0;// CNCID.Projectile.LegacyObject[CNCID.Projectile.SanctifiedArrowShoot];
            }
            Projectile p = CNCUtils.NewProj(source, player.Center, velocity, type, damage, knockback, player.whoAmI, 0f, 1f, 0f, 0f);
            p.direction = player.direction;
            p.Center = player.Center + velocity;
            return false;
        }

        public override Vector2? HoldoutOffset() => Vector2.Zero;
    }
}