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

        public float cooldownReductionAdd = 0f;
        public float cooldownReductionMul = 1f;
        public float cooldownSpeedAdd = 0f;
        public float cooldownSpeedMul = 1f;
        public float chantingReduction = 0f;
        public float chantingSpeedAdd = 0f;
        public float chantingSpeedMul = 1f;
        public float manaReturn = 0f;
        public float attackSpeed = 0f;
        public float skillDamageAdd = 0f;
        public float skillDamageMul = 1f;

        public override void ResetEffects()
        {
            this.cooldownReductionAdd = 0f;
            this.cooldownReductionMul = 1f;
            this.cooldownSpeedAdd = 0f;
            this.cooldownSpeedMul = 1f;
            this.chantingReduction = 0f;
            this.chantingSpeedAdd = 0f;
            this.chantingSpeedMul = 1f;
            this.manaReturn = 0f;
            this.attackSpeed = 0f;
            this.skillDamageAdd = 0f;
            this.skillDamageMul = 1f;
        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            reduce += item.GetGlobalItem<InscriptionSettings>().manaCostAdd;
            mult *= item.GetGlobalItem<InscriptionSettings>().manaCostMul;
        }

        private float UseItemSpeed(Item item, float value) => value / (1 + modPlayer.attackSpeed + item.GetGlobalItem<InscriptionSettings>().attackSpeed);
        public override float UseTimeMultiplier(Item item) => this.UseItemSpeed(item, base.UseTimeMultiplier(item));
        public override float UseAnimationMultiplier(Item item) => this.UseItemSpeed(item, base.UseAnimationMultiplier(item));
    }
}