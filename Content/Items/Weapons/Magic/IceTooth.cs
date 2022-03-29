using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Magic
{
    public class IceTooth : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 48;
            item.height = 48;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 3;
            item.value = 20000;
            item.damage = 18;
            item.crit = 5;
            item.knockBack = 5f;
            item.useStyle = 5;
            item.mana = 10;
            item.noMelee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item20;
            item.DamageType = DamageClass.Magic;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.DentateIce];
            item.shootSpeed = 16f;
            clazz = 32;
            staff = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CNCUtils.PlaySound("IceTooth/Shoot", player, 1f);
            Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback);
            if (item.GetGlobalItem<InscriptionSettings>().HasInscription(CNCID.Inscription.S_Iceflake)) projectile.GetGlobalProjectile<Common.Global.Projectiles.AllProjectile>().data["level"] = 1;
            return false;
        }
    }
}