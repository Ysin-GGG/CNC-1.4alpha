using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Magic
{
    class TheLightOfAuzzsia : CNCItem
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();

        public override void ResetDefaults()
        {
            item.width = 48;
            item.height = 50;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 3;
            item.value = 20000;
            item.damage = 18;
            item.crit = 5;
            item.knockBack = 20f;
            item.useStyle = 5;
            item.mana = 10;
            item.noMelee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item20;
            item.DamageType = DamageClass.Magic;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.MagicRock];
            item.shootSpeed = 20f;
            clazz = 32;
            staff = false;
            CNCStaff = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CNCUtils.PlaySound("TheLightOfAuzzsia/Shoot1", player, .5f);
            Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback);
            return false;
        }
    }
}
