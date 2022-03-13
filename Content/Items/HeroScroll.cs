using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
	public class HeroScroll : CNCItem
	{
        public override int researchAmount => 5;

        public override void ResetDefaults() {
			item.width = 40;
            item.height = 36;
            item.value = 250000;
            item.rare = -11;
            item.maxStack = 11;
        }
	}
}
