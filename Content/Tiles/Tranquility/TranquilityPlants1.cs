using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using CNC.Common.Utils;
using CNC.Common.Global.Tiles;

namespace CNC.Content.Tiles.Tranquility
{
    public abstract class TranquilityPlants : CNCTile
    {
        public override string Texture => "CNC/Content/Tiles/Tranquility/" + Name;

        public override void ResetStaticDefaults()
        {
            MapColor = new Color(100, 218, 157);
            DustType = DustID.Grass;
            SoundType = SoundID.Grass;
        }

        public class TranquilityPlants1 : TranquilityPlants
        {
            public override void ResetStaticDefaults()
            {
                base.ResetStaticDefaults();
                Clone(3);
            }
        }

        public class TranquilityPlants2 : TranquilityPlants
        {
            public override void ResetStaticDefaults()
            {
                base.ResetStaticDefaults();
                Clone(74);
            }
        }

        public class TranquilityVines : TranquilityPlants
        {
            public override void ResetStaticDefaults()
            {
                base.ResetStaticDefaults();
                Clone(52);
                tile.CopyFrom(GetStyle());
                tile.AnchorTop = new Terraria.DataStructures.AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, tile.Width, 0);
                addTile();
            }
        }
    }
}