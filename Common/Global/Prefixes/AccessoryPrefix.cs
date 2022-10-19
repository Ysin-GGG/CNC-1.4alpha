using System;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;

namespace CNC.Common.Global.Prefixes
{
    public abstract class AccessoryPrefix : CNCPrefix
    {
        public sealed override PrefixCategory Category => PrefixCategory.Accessory;

        public override bool CanRoll(Item item) => true;

        public override float Rarity => 0f;
        public virtual float Value => Rarity;

        public override void ModifyValue(ref float valueMult)
            => valueMult *= 1f + .05f * this.Rarity;

        public override float RollChance(Item item)
        {
            int count = 19;//tr”–19∏ˆ Œ∆∑–ﬁ Œ”Ô
            int max = PrefixLoader.PrefixCount;
            for (int i = 0; i < max; i++) if (PrefixLoader.GetPrefix(i) != null && PrefixLoader.GetPrefix(i).Category == this.Category) count++;
            return 100f / count;
        }
    }
}
