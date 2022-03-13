using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
    public class TranquilityGem : CNCItem
    {
        public override int researchAmount => 15;
        public override void ResetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 999;
            item.value = 5000;
            item.useTime = 15;
            item.useAnimation = 15;
            //item.createTile = CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGemSapling];
        }
    }
}