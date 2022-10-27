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
using Terraria.Audio;

namespace CNC.Content.Items.Weapons.Magic.Sceptre
{
    public class TheLightOfAuzzsia : CNCWeapon.Magic.Sceptre
    {
        public override void ResetDefaults()
        {
            item.width = 48;
            item.height = 50;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 2;
            item.value = 20000;
            item.damage = 100;
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

        public override bool OnCast(Player player, ref int mouse, ref float castTime, ref float castTimeMax, ref bool allowRelease, ref bool cannotBecanceled)
        {
            mouse = 1;
            castTimeMax = 120;
            return base.OnCast(player, ref mouse, ref castTime, ref castTimeMax, ref allowRelease, ref cannotBecanceled);
        }
    }
}
