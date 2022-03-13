using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.ObjectData;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CNC.Common.Utils;
using CNC.Common.Localization;

namespace CNC.Common.Global.Tiles
{
    public abstract class CNCOre : CNCTile
    {
        public short Value
        {
            get => Main.tileOreFinderPriority[Type];
            set => Main.tileOreFinderPriority[Type] = value;
        }

        public int ShineTick
        {
            get => Main.tileShine[Type];
            set
            {
                Main.tileShine[Type] = value;
                Main.tileShine2[Type] = value > 0;
            }
        }

        public override void SetStaticDefaults()
        {
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
            ShineTick = 900;
            Value = 100;
			SoundType = SoundID.Tink;
			SoundStyle = 1;
            base.SetStaticDefaults();
        }
	}
}