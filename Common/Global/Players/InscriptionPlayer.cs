using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using CNC.Content;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Common.Global.Players
{
    public class InscriptionPlayer : ModPlayer
    {
        Player player => Player;
        InscriptionPlayer modPlayer => player.GetModPlayer<InscriptionPlayer>();

        public int Tick => player.GetModPlayer<AllPlayer>().Tick;

        public float cooldownSpeed => (1 + 1 * this.cooldownSpeedAdd) * this.cooldownSpeedMul;
        public float cooldownSpeedAdd = 0f;
        public float cooldownSpeedMul = 1f;
        public float cooldownReduction = 0f;
        public float chantingSpeed => (1 + 1 * this.chantingSpeedAdd) * this.chantingSpeedMul;
        public float chantingSpeedAdd = 0f;
        public float chantingSpeedMul = 1f;
        public float chantingReduction = 0f;
        public float manaReturn = 0f;
        public float attackSpeed = 1f;
        public float spellDamageAdd = 0f;
        public float spellDamageMul = 1f;

        public override void ResetEffects()
        {
            this.cooldownSpeedAdd = 0f;
            this.cooldownSpeedMul = 1f;
            this.cooldownReduction = 0f;
            this.chantingSpeedAdd = 0f;
            this.chantingSpeedMul = 1f;
            this.chantingReduction = 0f;
            this.manaReturn = 0f;
            this.attackSpeed = 1f;
            this.spellDamageAdd = 0f;
            this.spellDamageMul = 1f;
        }

        public override float UseSpeedMultiplier(Item item) => modPlayer.attackSpeed;

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage, ref float flat)
        {
            InscriptionSettings modItem = item.GetGlobalItem<InscriptionSettings>();
            float add = 0;
            float mul = 1;
            for (int i = 0; i < modItem.inscription.Length; i++)
            {
                if (modItem.inscription[i].Equals(CNCID.Inscription.C_Power))
                    add += (float)CNCID.Inscription.C_Power.Data[0];
            }
            damage += add;
            damage *= mul;
        }

        public override float UseTimeMultiplier(Item item)
        {
            InscriptionSettings modItem = item.GetGlobalItem<InscriptionSettings>();
            float add = 0;
            float mul = 1;
            for (int i = 0; i < modItem.inscription.Length; i++)
            {
                if (modItem.inscription[i].Equals(CNCID.Inscription.C_MagicAdept))
                    add += (float)CNCID.Inscription.C_MagicAdept.Data[0];
            }
            return (base.UseTimeMultiplier(item) * (1 / (1 + add))) * mul;
        }

        public override float UseAnimationMultiplier(Item item)
        {
            InscriptionSettings modItem = item.GetGlobalItem<InscriptionSettings>();
            float add = 0;
            float mul = 1;
            for (int i = 0; i < modItem.inscription.Length; i++)
            {
                if (modItem.inscription[i].Equals(CNCID.Inscription.C_MagicAdept))
                    add += (float)CNCID.Inscription.C_MagicAdept.Data[0];
            }
            return (base.UseAnimationMultiplier(item) * (1 / (1 + add))) * mul;
        }
    }
}