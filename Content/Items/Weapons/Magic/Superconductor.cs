
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CNC.Common.Utils;
using CNC.Common.Global.Items;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Items.Weapons.Magic
{
    public class Superconductor : CNCItem
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();

        public bool LightningChainEnable => modItem.HasInscription(CNCID.Inscription.S_SpellLightningChain) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_SpellLightningChain);
        private bool LightningChainUpgrade => modItem.HasInscription(CNCID.Inscription.S_SpellAugmentated);
        private bool LightningChainUpgrade2 => modItem.HasInscription(CNCID.Inscription.S_SpellReconstruction);
        private int LightningChainDamage => this.JustGetDamage(CNCID.Inscription.S_SpellLightningChain, 2);
        public bool OverloadEnable => modItem.HasInscription(CNCID.Inscription.S_ArcaneOverload) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_ArcaneOverload);
        private bool OverloadUpgrade => modItem.HasInscription(CNCID.Inscription.S_StableSpell) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_StableSpell);
        private int OverloadDamage => this.JustGetDamage(CNCID.Inscription.S_ArcaneOverload, 3);
        
        public override void ResetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 3;
            item.value = 2000;
            item.damage = 18;
            item.crit = 5;
            item.knockBack = 3f;
            item.useStyle = 5;
            item.mana = 9;
            item.noMelee = true;
            item.autoReuse = true;
            item.DamageType = DamageClass.Magic;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningMissile];
            item.shootSpeed = 16f;
            this.clazz = CNCID.Clazz.Wizard;
            this.CNCStaff = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2 && this.LightningChainEnable)
            {
                type = CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningChain];
                CNCUtils.PlaySound("Superconductor/Chain", player);
                Projectile projectile = CNCUtils.NewProj(source, position - new Vector2(6 * player.direction, -10f), default, type, 0, 0f);
                projectile = CNCUtils.NewProj(source, position - new Vector2(6 * player.direction, -10f), default, type, this.LightningChainDamage, 0f);
                projectile.GetGlobalProjectile<AllProjectile>().data["noDamage"] = false;
                projectile.GetGlobalProjectile<AllProjectile>().data["upgraded"] = this.LightningChainUpgrade;
                bool noCooldown = this.LightningChainUpgrade2 && CNC.Random.NextBool((float)CNCID.Inscription.S_SpellReconstruction.Data[0]);
                modItem.SetInscriptionCooldown(CNCID.Inscription.S_SpellLightningChain, (noCooldown ? 0 : -1), -1, player);
            }
            else
            {
                CNCUtils.PlaySound("Superconductor/Shoot", player);
                if (this.OverloadEnable && player.HasBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Overloaded]))
                {
                    type = CNCID.Projectile.LegacyObject[CNCID.Projectile.OverloadedLightningMissile];
                    Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback, -1, modItem.skillDamageAdd, modItem.skillDamageMul);
                    if (player.HasBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Overloaded])) player.DelBuff(player.FindBuffIndex(CNCID.Buff.LegacyObject[CNCID.Buff.Overloaded]));
                }
                else
                {
                    Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback, -1);
                    projectile.GetGlobalProjectile<AllProjectile>().data["overload"] = this.OverloadEnable;
                    projectile.GetGlobalProjectile<AllProjectile>().data["overloadUpgraded"] = this.OverloadUpgrade;
                }
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
            => this.LightningChainEnable;

        public override bool CanUseItem(Player player)
        {
            bool result = base.CanUseItem(player);
            if (player.altFunctionUse == 2 && this.LightningChainEnable)
                result = ConsumeSanity(player, (int)CNCID.Inscription.S_SpellLightningChain.Data[0]);
            return result;
        }

        public override void HoldItem(Player player)
        {
            if (player.altFunctionUse == 2)
                player.itemRotation = -MyMath.deg45 * player.direction;
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2) mult = 0f;
        }
    }
}