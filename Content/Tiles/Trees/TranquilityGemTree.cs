using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;

namespace CNC.Content.Tiles.Trees
{
	public class TranquilityGemTree : CNCTree
    {
        public override int Dust => 1;
        public override int Drop => 1;
        //public override int Growth => ModContent.GoreType<SpiritTreeFX>();
        public override string FullName => "CNC/Content/Tiles/Trees/TranquilityGemTree";
        public override bool CanDropAcorn() => true;

        //public class TranquilityGemTreeFX : ModGore { }
    }
} 