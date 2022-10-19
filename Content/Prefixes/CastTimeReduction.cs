using Terraria;
using CNC.Common.Global.Prefixes;

namespace CNC.Content.Prefixes
{
    public class Concise : AccessoryPrefix
    {
        public override float RollChance(Item item) => base.RollChance(item) / 2f;
        public override float Value => -base.Value;
        public override float Rarity => base.Rarity + 1f;
    }
    public class Conversant : Concise { public override float Rarity => base.Rarity + 1f; }
    public class Incisive : Conversant { public override float Rarity => base.Rarity + 1f; }
    public class Omniscient : Incisive { public override float Rarity => base.Rarity + 1f; }
}
