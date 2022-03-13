using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
    public class SpiritSapling : CNCItem
    {
        public override int researchAmount => 10;
        public override void ResetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.maxStack = 99;
            item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 15;
            item.consumable = true;
            item.createTile = CNCID.Tile.LegacyObject[CNCID.Tile.SpiritSapling];
        }
    }
}