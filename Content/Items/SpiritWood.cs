using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
    public class SpiritWood : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.scale = 1f;
            item.maxStack = 999;
            item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 15;
            item.consumable = true;
            item.createTile = CNCID.Tile.LegacyObject[CNCID.Tile.SpiritWood];
        }
    }
}