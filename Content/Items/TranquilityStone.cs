using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
    public class TranquilityStone : CNCItem
    {
        public override void ResetDefaults()
        {
            item.CloneDefaults(3);
            item.createTile = CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityStone];
        }
    }
}