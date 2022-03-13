using Terraria.ModLoader;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Developers.HHvs
{
    [AutoloadEquip(EquipType.Body)]
    internal class HHvsRobe : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 9;
            item.vanity = true;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
			robes = true;
            equipSlot = mod.GetEquipSlot("CNC/Content/Items/Developers/HHvs/HHvsRobe_Legs", EquipType.Legs);
        }
	}
}
