using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.BigProgressBar;
using ReLogic.Content;

namespace CNC.Content.Items.Weapons.Magic
{
    public class TheLightOfAuzzsia : CNCItem
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();

        public override void ResetDefaults()
        {
            item.width = 48;
            item.height = 50;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 2;
            item.value = 20000;
            item.damage = 120;
            item.crit = 5;
            item.knockBack = 20f;
            item.useStyle = 5;
            item.mana = 10;
            item.noMelee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item20;
            item.DamageType = DamageClass.Magic;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.MagicRock];
            item.shootSpeed = 12f;
            clazz = 32;
            staff = false;
            CNCStaff = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (!player.All().casting) player.All().SetCasting(1, 0, 60, false);
            if (player.All().casting && player.All().castTime >= player.All().castTimeMax) return false;
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (player.All().castTimeMax > 0 && player.All().leftCastTime > player.All().castTimeMax)
            {
                float damage = player.GetWeaponDamage(item);
                float knockback = player.GetWeaponKnockback(item);
                float crit = player.GetWeaponCrit(item);
                float type = 0f;
                IEntitySource source = item.GetSource_ItemUse(item);
                Vector2 position = player.Center;
                Vector2 velocity = new Vector2(item.shootSpeed, 0f).RotatedBy((Main.MouseWorld - player.Center).ToRotation());
                CNCUtils.PlaySound(SoundID.Item20, player);
                CNCUtils.NewProj(source, position, velocity, item.shoot, (int)damage, knockback, (int)crit, player.whoAmI, 0f, 1f, 0f, 0f);
                player.All().ResetCasting(false);
                ApplyUse(player, item, item.useTime);
            }
        }

        private void ApplyUse(Player player, Item item, int time) => player.itemAnimationMax = player.itemAnimation = player.itemTimeMax = player.itemTime = (int)(time / player.GetWeaponAttackSpeed(item));

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
