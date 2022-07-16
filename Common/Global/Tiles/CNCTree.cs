using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;

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
        public int GrowthFXGore() => Growth;
        public Texture2D GetTexture(string name) => ModContent.Request<Texture2D>(name ?? FullName).Value;
        public Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset) => ModContent.Request<Texture2D>(FullName + "Top").Value;
        public Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame) => ModContent.Request<Texture2D>(FullName + "Branch").Value;

        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override Asset<Texture2D> GetTexture() { return ModContent.Request<Texture2D>(FullName); }

        public override bool CanDropAcorn() => false;

        public override Asset<Texture2D> GetBranchTextures() { return ModContent.Request<Texture2D>(FullName + "Branch"); }

        public override Asset<Texture2D> GetTopTextures() { return ModContent.Request<Texture2D>(FullName + "Top"); }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight) { return; throw new System.NotImplementedException(); }
    }
}