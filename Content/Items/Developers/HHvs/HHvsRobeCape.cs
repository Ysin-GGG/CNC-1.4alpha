using Terraria.ModLoader;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Developers.HHvs
{
    [AutoloadEquip(EquipType.Front, EquipType.Back)]
    internal class HHvsRobeCape : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 9;
            item.vanity = true;
            item.accessory = true;
        }
    }
}
