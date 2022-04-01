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
    public class IceTooth : CNCItem
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();
        public bool FrostWaveEnable => modItem.HasInscription(CNCID.Inscription.S_FrostWave) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_FrostWave);

        public override void ResetDefaults()
        {
            item.width = 54;
            item.height = 54;
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
            if (player.altFunctionUse == 2 && this.FrostWaveEnable)
            {
                CNCUtils.PlaySound("IceTooth/FrostWave", player, .5f);
                Projectile projectile = CNCUtils.NewProj(source, position - new Vector2(6 * player.direction, 52f), velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.FrostWave], Inscription.JustGetDamage(-1, (string)CNCID.Inscription.S_FrostWave.Data[2]), knockback);
                modItem.SetInscriptionCooldown(CNCID.Inscription.S_FrostWave);
            }
            else
            {
                CNCUtils.PlaySound("IceTooth/Shoot", player, 1f);
                Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback);
                if (modItem.HasInscription(CNCID.Inscription.S_Iceflake)) projectile.GetGlobalProjectile<Common.Global.Projectiles.AllProjectile>().data["flake"] = true;
                if (modItem.HasInscription(CNCID.Inscription.S_Glacier)) projectile.GetGlobalProjectile<Common.Global.Projectiles.AllProjectile>().data["frozen"] = true;
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
            => this.FrostWaveEnable;

        public override bool CanUseItem(Player player)
        {
            bool result = base.CanUseItem(player);
            if (player.altFunctionUse == 2)
            {
                int cost = (int)(player.manaCost * 300);
                player.itemRotation = -MyMath.deg90 * player.direction;
                player.itemLocation += new Vector2(0, 6f);
                result = ConsumeSanity(player, (int)CNCID.Inscription.S_FrostWave.Data[0]);
            }
            return result;
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            bool flag = player.altFunctionUse == 2;
            if (flag) mult = 0;
        }

        public override void HoldItem(Player player)
        {
            if (player.altFunctionUse == 2)
                player.itemRotation = -MyMath.deg90 * player.direction;
        }
        
        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(55,55);
        }
    }
}