using System;
using System.Collections.Generic;
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
        public int Damage_Thunderstorm => this.JustGetDamage(CNCID.Inscription.S_Thunderstorm, 1, -1);

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
            this.staff = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int crit = player.GetWeaponCrit(item);
            if (player.altFunctionUse == 2)
            {
                CNCUtils.PlaySound("Superconductor/Thunder", player.Center, .2f, 1f);
                CNCUtils.ScreenShake(player.Center, 960f, 0, new Vector2(0, 40f), new Vector2(0, 16f), .95f, default);
                double add = MyMath.Min(crit * (float)CNCID.Inscription.S_Resonance.Data[2] * (float)CNCID.Inscription.S_Resonance.Data[3] * 100f, (float)CNCID.Inscription.S_Resonance.Data[4]);
                if (this.Enable_Resonance && CNC.Random.NextBool(Probability_Resonance + add)) CNCUtils.SetBuff(player, CNCID.Buff.Surged, (int)CNCID.Inscription.S_Resonance.Data[1] * 60);
                if (Enable_ChainedLightning)
                {
                    type = CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningChain];
                    CNCUtils.PlaySound("Superconductor/Chain", player);
                    Projectile projectile = CNCUtils.NewProj(source, position - new Vector2(6 * player.direction, 20f), default, type, 0, 0f);
                    projectile = CNCUtils.NewProj(source, position - new Vector2(6 * player.direction, 20f), default, type, this.Damage_ChainedLightning, 0f);
                    projectile.GetGlobalProjectile<AllProjectile>().data["noDamage"] = false;
                    projectile.GetGlobalProjectile<AllProjectile>().data["upgraded"] = this.Enable_Intensity;
                    projectile.As<CNCProjectile>().Projectile_ModifyHitNPCEffect += this.SpellHitEffect;
                    modItem.SetInscriptionCooldown(CNCID.Inscription.S_ChainedLightning, consuming.ToInt(-1), -1, player);
                }
                else
                {
                    position -= new Vector2(6 * player.direction, 20f);
                    Projectile thunder = CNCUtils.NewProj(player.GetSource_ItemUse(item), position, Vector2.One, CNCID.Projectile.LegacyObject[CNCID.Projectile.Thunderbolt], 0, 0f, -1, 0, 1, 0f, 0f);
                    thunder.CritChance = crit;
                    thunder.GetGlobalProjectile<AllProjectile>().data["seed"] = DateTime.Now.Ticks;
                    thunder.GetGlobalProjectile<AllProjectile>().data["targetPosition"] = position;
                    if (this.Enable_RadialLightning) thunder.As<CNCProjectile>().Projectile_UpdateEffect += this.SummonRadialLightning;
                    if (this.Enable_ChainedLightning) thunder.As<CNCProjectile>().Projectile_UpdateEffect += this.SummonChainedLightning;
                    modItem.SetInscriptionCooldown(CNCID.Inscription.S_RadialLightning, consuming.ToInt(-1), -1, player);
                }
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
                    Projectile projectile = CNCUtils.NewProj(source, position, velocity * 1.5f, type, damage * 2, knockback * .75f, crit, -1, 0f, 1f);
                    projectile.velocity = projectile.oldVelocity = velocity * 1.5f;
                    (projectile.ModProjectile as CNCProjectile).Projectile_OnHitNPCEffect += SummonThunder;
                    player.DelBuff(player.FindBuffIndex(buff));
                }
                else
                {
                    double add = MyMath.Min(crit * (float)CNCID.Inscription.S_Fusion.Data[2] * (float)CNCID.Inscription.S_Fusion.Data[3] * 100f, (float)CNCID.Inscription.S_Fusion.Data[4]);
                    Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback, crit, -1, 0f, 1f);
                    (projectile.ModProjectile as CNCProjectile).Projectile_OnHitNPCEffect += SummonThunder;
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

        private void SummonThunder(Projectile projectile, NPC target, int damage, float knockBack, bool crit)
        {
            double add = MyMath.Min(projectile.CritChance * (float)CNCID.Inscription.S_Thunderstorm.Data[2] * (float)CNCID.Inscription.S_Thunderstorm.Data[3] * 100f, (float)CNCID.Inscription.S_Thunderstorm.Data[4]);
            if (this.Enable_Thunderstorm && CNC.Random.NextBool(Probability_Thunderstorm + add))
            {
                add = MyMath.Min(projectile.CritChance * (float)CNCID.Inscription.S_Resonance.Data[2] * (float)CNCID.Inscription.S_Resonance.Data[3] * 100f, (float)CNCID.Inscription.S_Resonance.Data[4]);
                if (this.Enable_Resonance && CNC.Random.NextBool(Probability_Resonance + add)) CNCUtils.SetBuff(Main.player[projectile.owner], CNCID.Buff.Surged, (int)CNCID.Inscription.S_Resonance.Data[1] * 60);
                CNCUtils.PlaySound("Superconductor/Thunder", projectile.Center, .2f, 1f);
                CNCUtils.ScreenShake(projectile.Center, 960f, 0, new Vector2(0, 40f), new Vector2(0, 16f), .95f, default);
                Projectile thunder = (projectile.ModProjectile as CNCProjectile).NewProjectile(target.Bottom, Vector2.Zero, CNCID.Projectile.LegacyObject[CNCID.Projectile.Thunderbolt], this.Damage_Thunderstorm, 0, 0, 0, 1, 0f, 0f);
                thunder.CritChance = projectile.CritChance;
                thunder.GetGlobalProjectile<AllProjectile>().data["seed"] = DateTime.Now.Ticks;
                thunder.GetGlobalProjectile<AllProjectile>().data["targetPosition"] = target.Bottom;
                thunder.As<CNCProjectile>().Projectile_ModifyHitNPCEffect += this.SpellHitEffect;
            }
        }

        private void SummonRadialLightning(Projectile projectile)
        {
            if (projectile.velocity == Vector2.Zero && projectile.oldVelocity != Vector2.Zero)
            {
                Vector2 position = projectile.Center;
                List<NPC> whoIcanHit = new List<NPC>();
                int count = this.Enable_Intensity ? (int)CNCID.Inscription.S_Intensity.Data[2] : (int)CNCID.Inscription.S_RadialLightning.Data[1];
                float range = ((int)CNCID.Inscription.S_RadialLightning.Data[2] + (this.Enable_Intensity ? (int)CNCID.Inscription.S_Intensity.Data[1] : 0)) * 16f;
                for (int i = 0; i < Main.maxNPCs; i++)//找出范围内的所有敌人
                {
                    bool flag = true;
                    if (flag) flag &= Main.npc[i] != null && Main.npc[i].active;
                    if (flag) flag &= Main.npc[i].Center.Distance(position) <= range;
                    if (flag) flag &= Main.npc[i].type != 488;
                    if (flag) flag &= !Main.npc[i].friendly;
                    if (flag) flag &= !Main.npc[i].dontTakeDamage;
                    if (flag) whoIcanHit.Add(Main.npc[i]);
                }
                if (whoIcanHit.Count > 0)
                {
                    CNCUtils.PlaySound("Superconductor/Chain", position);
                    whoIcanHit.Sort((a, b) => a.Center.Distance(position).CompareTo(b.Center.Distance(position)));//按距离排序，近者优先
                    if (whoIcanHit.Count > count) whoIcanHit.RemoveRange(count, whoIcanHit.Count - count);//保留前几个最近的敌人
                    int targetCount = whoIcanHit.Count;
                    int[][] hitCount = { new int[targetCount], new int[targetCount] };//当前击中次数/最大击中次数
                    for (int i = 0; i < count; i++) hitCount[1][i % targetCount]++;//计算最大击中次数
                    for (int i = 0; i < count; i++)
                    {
                        int idx = i % targetCount;//队列头成员
                        NPC target = whoIcanHit[idx];
                        Vector2 velocity = Vector2.Normalize(target.Center - position) * 32f;
                        int dir = (velocity.X > 0).ToDirectionInt(1) * (velocity.Y > 0).ToDirectionInt(1);//象限
                        float angle = dir * MyMath.deg45 * MyMath.Clamp((hitCount[1][idx] - 1) / 2, 1f, 1.5f) + CNC.Random.NextDegree(-15f, 15f);
                        float rotation = Utils.AngleLerp(velocity.ToRotation() - angle, velocity.ToRotation() + angle, (float)hitCount[0][idx]++ / hitCount[1][idx]);
                        Projectile lightning = (projectile.ModProjectile as CNCProjectile).NewProjectile(position, velocity.RotatedBy(rotation), CNCID.Projectile.LegacyObject[CNCID.Projectile.Lightning], this.Damage_RadialLightning, 0f, rotation, 0, 1, 0f, 0f);
                        lightning.rotation = rotation;
                        lightning.CritChance = projectile.CritChance;
                        lightning.GetGlobalProjectile<AllProjectile>().data["seed"] = DateTime.Now.Ticks + i;
                        lightning.GetGlobalProjectile<AllProjectile>().data["targetNPC"] = target;
                        lightning.GetGlobalProjectile<AllProjectile>().data["targetPosition"] = target.Bottom;
                        lightning.As<CNCProjectile>().Projectile_ModifyHitNPCEffect += this.SpellHitEffect;
                    }
                }
            }
        }

        private void SummonChainedLightning(Projectile projectile)
        {
        }

        private void SpellHitEffect(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            (projectile.ModProjectile as CNCProjectile).NewProjectile(target.Center, Vector2.Zero, CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningExplosion], 0, 0f, CNC.Random.NextFloat(MyMath.pi2));
            CNC.CombatTextColor = crit ? CNCID.Colors.MagicCrit : CNCID.Colors.Magic;
        }
    }
}