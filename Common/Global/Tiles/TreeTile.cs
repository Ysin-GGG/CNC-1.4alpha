using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.ObjectData;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CNC.Common.Utils;
using CNC.Common.Localization;

namespace CNC.Common.Global.Tiles
{
    public class TreeTile : GlobalTile
    {
        private void DrawTree(Point point, Texture2D top, Texture2D branch)
        {
            Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
            Vector2 zero = Vector2.Zero;
            float num2 = 0.1f;
            float num3 = 0.08f;
            int x = point.X;
            int y1 = point.Y;
            Tile t = Main.tile[x, y1];
            if (t != null)
            {
                ushort type = t.TileType;
                short frameX = t.TileFrameX;
                short frameY1 = t.TileFrameY;
                bool flag1 = t.WallType > (ushort)0;
                bool flag2 = true;
                byte tileColor = t.TileColor;
                Color color = t.TileColor == (byte)31 ? Color.White : Lighting.GetColor(x, y1);
                WorldGen.GetTreeFoliageDataMethod foliageDataMethod = new WorldGen.GetTreeFoliageDataMethod(WorldGen.GetVanityTreeFoliageData);
                if (flag2 && frameY1 >= (short)198 && frameX >= (short)22)
                {
                    int treeFrame = WorldGen.GetTreeFrame(t);
                    if (frameX == 22)
                    {
                        int treeStyle1 = 0;
                        int topTextureFrameWidth1 = 114;
                        int topTextureFrameHeight1 = 96;
                        int xoffset1 = 0;
                        int grassPosX = x + xoffset1;
                        int floorY1 = y1;
                        foliageDataMethod(x, y1, xoffset1, ref treeFrame, ref treeStyle1, out floorY1, out topTextureFrameWidth1, out topTextureFrameHeight1);
                        TreeDrawing.Instance.EmitTreeLeaves(x, y1, grassPosX, floorY1, ModContent.GoreType<Content.Tiles.Trees.SpiritTree.SpiritTreeFX>());
                        Texture2D treeTopTexture = top;
                        Vector2 vector2;
                        Vector2 position = vector2 = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y1 * 16 - (int)unscaledPosition.Y + 16)) + zero;
                        float num5 = 0.0f;
                        if (!flag1) num5 = TreeDrawing.Instance.GetWindCycle(x, y1);
                        if (Main.windSpeedCurrent < 0 && num5 > 0) num5 *= -1;
                        position.X += num5 * 2f;
                        position.Y += Math.Abs(num5) * 2f;
                        position += new Vector2(12f * 16f);
                        Main.spriteBatch.Draw(treeTopTexture, position, new Rectangle?(new Rectangle(treeFrame * (topTextureFrameWidth1 + 2), 0, topTextureFrameWidth1, topTextureFrameHeight1)), color, num5 * num2, new Vector2((float)(topTextureFrameWidth1 / 2), (float)topTextureFrameHeight1), 1f, SpriteEffects.None, 0.0f);
                    }
                    if (frameX == 44)
                    {
                        int treeStyle2 = 0;
                        int num6 = x;
                        int floorY2 = y1;
                        int xoffset2 = 1;
                        int topTextureFrameWidth2;
                        int topTextureFrameHeight2;
                        foliageDataMethod(x, y1, xoffset2, ref treeFrame, ref treeStyle2, out floorY2, out topTextureFrameWidth2, out topTextureFrameHeight2);
                        TreeDrawing.Instance.EmitTreeLeaves(x, y1, num6 + xoffset2, floorY2, ModContent.GoreType<Content.Tiles.Trees.SpiritTree.SpiritTreeFX>());
                        Texture2D treeBranchTexture = branch;
                        Vector2 position = new Vector2((float)(x * 16), (float)(y1 * 16)) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
                        float num8 = 0.0f;
                        if (!flag1) num8 = TreeDrawing.Instance.GetWindCycle(x, y1);
                        if (Main.windSpeedCurrent < 0 && num8 > 0) num8 *= -1;
                        if ((double)num8 > 0.0) position.X += num8;
                        position.X += Math.Abs(num8) * 2f;
                        position += new Vector2(12f * 16f);
                        Main.spriteBatch.Draw(treeBranchTexture, position, new Rectangle?(new Rectangle(0, treeFrame * 42, 40, 40)), color, num8 * num3, new Vector2(40f, 24f), 1f, SpriteEffects.None, 0.0f);
                    }
                    if (frameX == 66)
                    {
                        int treeStyle3 = 0;
                        int num9 = x;
                        int floorY3 = y1;
                        int xoffset3 = -1;
                        int topTextureFrameWidth3;
                        int topTextureFrameHeight3;
                        foliageDataMethod(x, y1, xoffset3, ref treeFrame, ref treeStyle3, out floorY3, out topTextureFrameWidth3, out topTextureFrameHeight3);
                        TreeDrawing.Instance.EmitTreeLeaves(x, y1, num9 + xoffset3, floorY3, ModContent.GoreType<Content.Tiles.Trees.SpiritTree.SpiritTreeFX>());
                        Texture2D treeBranchTexture = branch;
                        Vector2 position = new Vector2((float)(x * 16), (float)(y1 * 16)) - unscaledPosition.Floor() + zero + new Vector2(0.0f, 18f);
                        float num11 = 0.0f;
                        if (!flag1) num11 = TreeDrawing.Instance.GetWindCycle(x, y1);
                        if (Main.windSpeedCurrent < 0 && num11 > 0) num11 *= -1;
                        if ((double)num11 < 0.0) position.X += num11;
                        position.X -= Math.Abs(num11) * 2f;
                        position += new Vector2(12f * 16f);
                        Main.spriteBatch.Draw(treeBranchTexture, position, new Rectangle?(new Rectangle(42, treeFrame * 42, 40, 40)), color, num11 * num3, new Vector2(0.0f, 30f), 1f, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }

        public override void DrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (type == 5)
            {
                List<Point> vst = new List<Point>();
                bool dfs(int x, int y)
                {
                    vst.Add(new Point(x, y));
                    int frameX = Main.tile[x, y].TileFrameX;
                    int frameY = Main.tile[x, y].TileFrameY;
                    bool flag = 22 <= frameX && frameX < 22 * 3 && 22 * 6 <= frameY && frameY < 22 * 9;
                    if (Main.tile[x, y + 1] != null && Main.tile[x, y + 1].TileType == CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGrass] && !flag) return true;
                    else if (Main.tile[x, y + 1] != null && Main.tile[x, y + 1].TileType == 5) return dfs(x, y + 1);
                    else if (!vst.Contains(new Point(x + 1, y)) && Main.tile[x + 1, y] != null && Main.tile[x + 1, y].TileType == 5) return dfs(x + 1, y);
                    else if (!vst.Contains(new Point(x - 1, y)) && Main.tile[x - 1, y] != null && Main.tile[x - 1, y].TileType == 5) return dfs(x - 1, y);
                    return false;
                }
                if (dfs(i, j))
                {
                    Texture2D image = ModContent.Request<Texture2D>("CNC/Content/Tiles/Trees/SpiritTree").Value;
                    Texture2D top = ModContent.Request<Texture2D>("CNC/Content/Tiles/Trees/SpiritTreeTop").Value;
                    Texture2D branch = ModContent.Request<Texture2D>("CNC/Content/Tiles/Trees/SpiritTreeBranch").Value;
                    drawData.drawTexture = image;
                    DrawTree(new Point(i, j), top, branch);
                }
            }
        }
    }

    public class TreeDrawing : ModSystem
    {
        public static TreeDrawing Instance = null;
        public double treeWindCounter => _treeWindCounter;
        public UnifiedRandom rand => _rand;
        private int _leafFrequency = 100000;
        private UnifiedRandom _rand;
        private double _treeWindCounter;
        private bool _isActiveAndNotPaused => !Main.gamePaused && Main.instance.IsActive;

        public override void Load()
        {
            Instance = this;
            this._rand = new UnifiedRandom();
        }

        public override void PostDrawTiles()
        {
            if (_isActiveAndNotPaused) Update();
        }
        
        public void Update()
        {
            double lerpValue = (double)Terraria.Utils.GetLerpValue(0.08f, 1.2f, Math.Abs(Main.WindForVisuals), true);
            _treeWindCounter += 1.0 / 240.0 + 1.0 / 240.0 * lerpValue * 2.0;
            float num = Math.Abs(Main.WindForVisuals);
            this._leafFrequency = (double)num > 0.100000001490116 ? ((double)num > 0.200000002980232 ? ((double)num > 0.300000011920929 ? ((double)num > 0.400000005960464 ? ((double)num > 0.5 ? ((double)num > 0.600000023841858 ? ((double)num > 0.699999988079071 ? ((double)num > 0.800000011920929 ? ((double)num > 0.899999976158142 ? ((double)num > 1.0 ? ((double)num > 1.10000002384186 ? 10 : 20) : 30) : 40) : 50) : 75) : 130) : 200) : 300) : 450) : 1000) : 2000;
            this._leafFrequency *= 7;
        }
        
        public float GetWindCycle(int x, int y)
        {
            if (!Main.SettingsEnabled_TilesSwayInWind)
                return 0.0f;
            float num1 = (float)((double)x * 0.5 + (double)(y / 100) * 0.5);
            float num2 = (float)Math.Cos(treeWindCounter * 6.28318548202515 + (double)num1) * 0.5f;
            return (double)y < Main.worldSurface ? (num2 + Main.WindForVisuals) * Terraria.Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), true) : 0.0f;
        }
        
        public void EmitTreeLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY, int leafType)
        {
            if (!_isActiveAndNotPaused) return;
            int treeHeight = grassPosY - tilePosY;
            Tile topTile = Main.tile[tilePosX, tilePosY];
            if (topTile.LiquidType > (byte)0) return;
            bool flag1 = true;
            int maxValue = this._leafFrequency;
            bool flag2 = (uint)(tilePosX - grassPosX) > 0U;
            if (flag1) maxValue /= 2;
            if ((double)tilePosY > Main.worldSurface) maxValue = 10000;
            if (flag2) maxValue *= 3;
            if (this._rand.Next(maxValue) != 0) return;
            Vector2 vector2 = new Vector2((float)(tilePosX * 16 + 8), (float)(tilePosY * 16 + 8));
            if (flag2)
            {
                int num2 = tilePosX - grassPosX;
                int num3 = 0;
                vector2.X += (float)(num2 * 12);
                if (topTile.TileFrameY == (short)220) num3 = 1;
                else if (topTile.TileFrameY == (short)242) num3 = 2;
                if (topTile.TileFrameX == (short)66)
                {
                    if(num3 == 2) vector2 += new Vector2(0.0f, 8f);
                    else vector2 += new Vector2(0.0f, -6f);
                }
                else
                {
                    if (num3 == 0) vector2 += new Vector2(0.0f, 4f);
                    if (num3 == 1) vector2 += new Vector2(2f, -6f);
                    if (num3 == 2) vector2 += new Vector2(6f, -6f);
                }
            }
            else
            {
                vector2 += new Vector2(-16f, -16f);
                if (flag1) vector2.Y -= (float)(Main.rand.Next(0, 28) * 4);
            }
            if (WorldGen.SolidTile(vector2.ToTileCoordinates())) return;
            Gore.NewGoreDirect(vector2, Terraria.Utils.RandomVector2(Main.rand, -2, 2), leafType, (float)(0.699999988079071 + (double)Main.rand.NextFloat() * 0.600000023841858));
        }
    }
}