using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Localization;

namespace CNC.Common.Global.Prefixes
{
    public abstract class CNCPrefix : ModPrefix
	{
		public override PrefixCategory Category => PrefixCategory.Custom;

        public override bool CanRoll(Item item) => true;

        public override float RollChance(Item item) => 5f;

        public sealed override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault(CNCLocalization.GetText($"Prefix.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Prefix.{Name}", 7));
            this.ResetStaticDefaults();
        }

        public virtual float Rarity => 1f;

        public virtual void ResetStaticDefaults() { }
    }
}
