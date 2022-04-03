using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
	public class DestineScroll : CNCItem
	{
        public override int researchAmount => 5;

        public override void ResetDefaults() {
			item.width = 34;
            item.height = 34;
            item.value = 250000;
            item.rare = -11;
            item.maxStack = 6;
        }
	}
}
