using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CNC.Common.Global.Tiles
{
    public abstract class CNCTree : ModTree
    {
        public virtual int Dust => -1;
        public virtual int Drop => -1;
        public virtual int Growth => -1;
        public virtual string FullName => "";
        public override int CreateDust() => Dust;
        public override int DropWood() => Drop;
        public sealed override int GrowthFXGore() => Growth;
        public sealed override Texture2D GetTexture() => ModContent.Request<Texture2D>(FullName).Value;
        public sealed override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset) => ModContent.Request<Texture2D>(FullName + "Top").Value;
        public sealed override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame) => ModContent.Request<Texture2D>(FullName + "Branch").Value;
    }
}