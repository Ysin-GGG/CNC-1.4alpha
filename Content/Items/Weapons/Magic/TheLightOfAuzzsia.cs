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
            item.damage = 80;
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

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CNCUtils.PlaySound("TheLightOfAuzzsia/Shoot1", player, .5f);
            CNCUtils.NewProj(source, position, velocity, type, damage, knockback);
            return false;
        }
    }
}
