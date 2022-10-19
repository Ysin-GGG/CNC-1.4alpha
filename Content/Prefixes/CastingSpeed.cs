using Terraria;
using CNC.Common.Global.Prefixes;

namespace CNC.Content.Prefixes
{
    public class Memorable : AccessoryPrefix
    {
        public override float RollChance(Item item) => base.RollChance(item) / 2f;
        public override float Rarity => base.Rarity + 1f;
    }
    public class Proficient : Memorable { public override float Rarity => base.Rarity + 1f; }
    public class Concentrated : Proficient { public override float Rarity => base.Rarity + 1f; }
    public class Reverberant : Concentrated { public override float Rarity => base.Rarity + 1f; }
}
