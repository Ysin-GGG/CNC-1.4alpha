using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
    public class TranquilityDirt : CNCItem
    {
        public override void ResetDefaults()
        {
            item.CloneDefaults(2);
            item.createTile = CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityDirt];
        }
    }
}