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
    }
}