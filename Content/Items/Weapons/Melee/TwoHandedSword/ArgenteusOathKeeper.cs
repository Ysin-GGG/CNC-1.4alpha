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

namespace CNC.Content.Items.Weapons.Melee.TwoHandedSword
{
    class ArgenteusOathKeeper : CNCItem
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();

        public override void ResetDefaults()
        {
            item.width = 74;
            item.height = 76;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 2;
            item.value = 20000;
            item.damage = 35;
            item.crit = 5;
            item.knockBack = 8f;
            item.useStyle = 1;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.DamageType = DamageClass.Melee;
            //item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.MagicRock];
            //item.shootSpeed = 20f;
            clazz = 13;
            //staff = false;
            //CNCStaff = true;
        }
    }
}