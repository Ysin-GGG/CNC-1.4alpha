using Microsoft.Xna.Framework;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;

namespace CNC.Content.Tiles.Tranquility
{
    public class SpiritWood : CNCTile
    {
        public override void ResetStaticDefaults()
        {
            Clone(30);
            MapColor = new Color(209, 144, 70);
            DustType = DustID.t_LivingWood;
            ItemDrop = CNCID.Item.LegacyObject[CNCID.Item.SpiritWood];
        }
    }
}