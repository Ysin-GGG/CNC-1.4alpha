using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Developers.Quartz
{
    public class Anatta : CNCItem
    {
        public override void ResetDefaults()
        {
            item.useTime = 20;
            item.useAnimation = 20;
            item.scale = 0f;
            item.rare = 9;
            item.value = 0;
            item.damage = 35;
            item.crit = 15;
            item.knockBack = 8f;
            item.useStyle = 1;
            item.noMelee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.DamageType = DamageClass.Melee;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.AnattaSlash];
            item.shootSpeed = 16f;
        }
    }
}