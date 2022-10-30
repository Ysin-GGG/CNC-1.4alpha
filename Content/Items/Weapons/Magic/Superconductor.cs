
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

        public bool Enable_Fusion => modItem.ExistInscription(CNCID.Inscription.S_Fusion);
        public bool Enable_Overload => modItem.ExistInscription(CNCID.Inscription.S_Overload);
        public bool Enable_RadialLightning => modItem.ExistInscription(CNCID.Inscription.S_RadialLightning) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_RadialLightning);
        public bool Enable_ChainedLightning => modItem.ExistInscription(CNCID.Inscription.S_ChainedLightning) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_ChainedLightning);
        public bool Enable_Thunderstorm => modItem.ExistInscription(CNCID.Inscription.S_Thunderstorm);
        public bool Enable_ElectricBarrier => modItem.ExistInscription(CNCID.Inscription.S_ElectricBarrier);
        public bool Enable_ArcaneOverload => modItem.ExistInscription(CNCID.Inscription.S_ArcaneOverload);
        public bool Enable_Intensity => modItem.ExistInscription(CNCID.Inscription.S_Intensity);
        public bool Enable_Activation => modItem.ExistInscription(CNCID.Inscription.S_Activation);
        public bool Enable_Resonance => modItem.ExistInscription(CNCID.Inscription.S_Resonance);

        public int Damage_RadialLightning => this.JustGetDamage(CNCID.Inscription.S_RadialLightning, 3, -1);
        public int Damage_ChainedLightning => this.JustGetDamage(CNCID.Inscription.S_ChainedLightning, 3, -1);

        public double Probability_Fusion => (double)(float)CNCID.Inscription.S_Fusion.Data[0];
        public double Probability_Thunderstorm => (double)(float)CNCID.Inscription.S_Thunderstorm.Data[0];
        public double Probability_Activation => (double)(float)CNCID.Inscription.S_Activation.Data[0];
        public double Probability_Resonance => (double)(float)CNCID.Inscription.S_Resonance.Data[0];

        private bool consuming = false;

        public override void ResetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.useTime = 53;
            item.useAnimation = 53;
            item.rare = 3;
            item.value = 2000;
            item.damage = 44;
            item.crit = 0;
            item.knockBack = 4f;
            item.useStyle = 5;
            item.mana = 16;
            item.noMelee = true;
            item.autoReuse = true;
            item.DamageType = DamageClass.Magic;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningMissile];
            item.shootSpeed = 16f;
            this.clazz = CNCID.Clazz.Wizard;
            staff = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int crit = player.GetWeaponCrit(item);
            if (player.altFunctionUse == 2)
            {
                type = CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningChain];
                CNCUtils.PlaySound("Superconductor/Chain", player);
                Projectile projectile = CNCUtils.NewProj(source, position - new Vector2(6 * player.direction, 20f), default, type, 0, 0f);
                projectile = CNCUtils.NewProj(source, position - new Vector2(6 * player.direction, 20f), default, type, this.Damage_ChainedLightning, 0f);
                projectile.GetGlobalProjectile<AllProjectile>().data["noDamage"] = false;
                projectile.GetGlobalProjectile<AllProjectile>().data["upgraded"] = this.Enable_Intensity;
                modItem.SetInscriptionCooldown(CNCID.Inscription.S_ChainedLightning, consuming.ToInt(-1), -1, player);
            }
            else
            {
                CNCUtils.PlaySound("Superconductor/Shoot", player);
                int buff = CNCID.Buff.LegacyObject[CNCID.Buff.Surged];
                int buffTime = this.Enable_Resonance ? (int)CNCID.Inscription.S_Resonance.Data[1] : (int)CNCID.Inscription.S_Fusion.Data[1];
                if (player.HasBuff(buff))
                {
                    if (this.Enable_ArcaneOverload) CNCUtils.SetBuff(player, CNCID.Buff.Overloaded, (int)CNCID.Inscription.S_ArcaneOverload.Data[1] * 60);
                    type = CNCID.Projectile.LegacyObject[CNCID.Projectile.OverloadedLightningMissile];
                    Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage * 2, knockback * .75f, crit, -1, 0f, 1f);
                    player.DelBuff(player.FindBuffIndex(buff));
                }
                else
                {
                    double add = MyMath.Min(crit * (float)CNCID.Inscription.S_Fusion.Data[2] * (float)CNCID.Inscription.S_Fusion.Data[3] * 100f, (float)CNCID.Inscription.S_Fusion.Data[4]);
                    Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback, crit, -1, 0f, 1f);
                    projectile.GetGlobalProjectile<AllProjectile>().data["resonance"] = this.Enable_Resonance;
                    projectile.GetGlobalProjectile<AllProjectile>().data["overload"] = this.Enable_Overload;
                    projectile.GetGlobalProjectile<AllProjectile>().data["overloadRequirement"] = (int)CNCID.Inscription.S_Overload.Data[1] - (int)MyMath.Min(crit / ((float)CNCID.Inscription.S_Overload.Data[3] * 100f) * (int)CNCID.Inscription.S_Overload.Data[4], (int)CNCID.Inscription.S_Overload.Data[5]);
                    if (this.Enable_Fusion && CNC.Random.NextBool(this.Probability_Fusion + add)) CNCUtils.SetBuff(player, CNCID.Buff.Surged, buffTime * 60);
                }
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
            => this.Enable_RadialLightning || this.Enable_ChainedLightning;

        public override bool CanUseItem(Player player)
        {
            bool result = base.CanUseItem(player);
            if (player.altFunctionUse == 2)
            {
                consuming = !(this.Enable_Activation && CNC.Random.NextBool(this.Probability_Activation));
                if (this.Enable_RadialLightning && consuming) result = ConsumeSanity(player, (int)CNCID.Inscription.S_RadialLightning.Data[0]);
                if (this.Enable_ChainedLightning && consuming) result = ConsumeSanity(player, (int)CNCID.Inscription.S_ChainedLightning.Data[0]);
            }
            return result;
        }

        public override void HoldItem(Player player)
        {
            if (player.altFunctionUse == 2) player.itemRotation = -MyMath.deg90 * player.direction;
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2) mult = 0f;
        }
    }
}