using Terraria.ModLoader;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Developers.HHvs
{
    [AutoloadEquip(EquipType.Head)]
    public class HHvsHat : CNCItem
	{
        public override void ResetDefaults()
        {
			item.width = 32;
            item.height = 20;
            item.rare = 9;
            item.vanity = true;
		}
    }
}