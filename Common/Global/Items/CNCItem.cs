using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Players;

namespace CNC.Common.Global.Items
{
    public abstract class CNCItem : ModItem
    {
        public AllItem allItem => item.GetGlobalItem<AllItem>();
        public Item item => Item;
        public Mod mod => Mod;

        protected Color u => new Color(255, 255, 255, 255);
        protected Color n => new Color(64, 64, 64, 255);
        
        public byte clazz { get => allItem.clazz; set => allItem.clazz = value; }
        public bool staff { get => Item.staff[Type]; set => Item.staff[Type] = value; }

        public bool ConsumeSanity(Player player, int amount, float ignoreCost = 0, bool? convert = null)
        {
            ignoreCost = MyMath.Clamp(ignoreCost, 0, 1);
            ClassesSettings modPlayer = player.GetModPlayer<ClassesSettings>();
            float value = modPlayer.clazz_Sanity;
            float max = modPlayer.clazz_SanityMax;
            float add = modPlayer.clazz_SanityCostAdd;
            float mul = modPlayer.clazz_SanityCostMul;
            float damage = 0;
            float newValue = MyMath.Clamp(value - ((amount + amount * MyMath.Lerp(add, 0, ignoreCost)) * MyMath.Lerp(mul, 1, ignoreCost)), 0, max, out damage);
            damage *= -1;
            if ((convert ?? true) || (bool)convert)//前面的true要改成判定有无某个饰品
            {
                if (modPlayer.clazz_SanityLethal || (!modPlayer.clazz_SanityLethal && damage < player.statLife))
                {
                    player.statLife -= (int)(damage * modPlayer.clazz_SanityRatioToLife);
                    player.lifeRegenTime = 0;
                    modPlayer.clazz_Sanity = newValue;
                    if (damage > 0) CombatText.NewText(player.getRect(), CombatText.LifeRegen, (int)(damage * modPlayer.clazz_SanityRatioToLife), false, true);
                    if (player.statLife <= 0 && player.whoAmI == Const.me)
                    {
                        string tt = CNCLocalization.GetText("DeathText.LostSanity");
                        tt = tt.Replace("[player]", player.name);
                        tt = tt.Replace("[Player]", player.name);
                        player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(tt), int.MaxValue, player.direction);
                    }
                    return true;
                }
                return false;
            }
            else if (amount <= value)
            {
                if (damage <= 0) modPlayer.clazz_Sanity = newValue;
                return damage <= 0;
            }
            return false;
        }

        public virtual string[] attributeKeys => new string[0];
        public virtual int countFrame => 1;
        public virtual int ticksPerFrame => 30;
        public virtual int researchAmount => 1;

        public sealed override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new Terraria.DataStructures.DrawAnimationVertical(ticksPerFrame, countFrame));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = researchAmount;
            DisplayName.SetDefault(CNCLocalization.GetText($"Item.Name.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Item.Name.{Name}", 7));
            if (CNCLocalization.GetText($"Item.Tooltip.{Name}") != "\0" && CNCLocalization.GetText($"Item.Tooltip.{Name}") != CNCLocalization.ModKey + $"Item.Tooltip.{Name}")
            {
                Tooltip.SetDefault(CNCLocalization.GetText($"Item.Tooltip.{Name}", 1));
                Tooltip.AddTranslation(7, CNCLocalization.GetText($"Item.Tooltip.{Name}", 7));
            }
            ResetStaticDefaults();
        }

        public sealed override void SetDefaults() => ResetDefaults();

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips) => Tooltips(tooltips);

        public override void AddRecipes() { }
        public virtual void ResetDefaults() { }
        public virtual void ResetStaticDefaults() { }
        public virtual void Tooltips(List<TooltipLine> tooltips) { }
        public bool CNCStaff;
        public override Vector2? HoldoutOffset()
        {
            Vector2? result = base.HoldoutOffset();
            float x = 0;
            if (this.CNCStaff)
            {
                x = (-item.width * 1 / 2) + (item.width * 1 / 2 * 1 / 3);
                result = new Vector2(x, 0);
            }
            return result;
        }
    }
}