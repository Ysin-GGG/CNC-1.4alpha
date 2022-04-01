using Terraria.ModLoader;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Developers.HHvs
{
    [AutoloadEquip(EquipType.Legs)]
    public class HHvsLegs : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.rare = 9;
            item.vanity = true;
        }
    }
}