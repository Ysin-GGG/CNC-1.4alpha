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
    public class TreeDrawing : GlobalTile//tml快点把modtree写好pls
    {
        private void DrawTree(Point point, Texture2D top, Texture2D branch, WorldGen.GetTreeFoliageDataMethod drawType)
        {
            Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
            Vector2 zero = Vector2.Zero;
            float num2 = 0.1f;
            float num3 = 0.08f;
            int x = point.X;
            int y = point.Y;
            Tile t = Main.tile[x, y];
            if (t != null)
            {
                ushort type = t.TileType;
                short frameX = t.TileFrameX;
                short frameY = t.TileFrameY;
                bool flag1 = t.WallType > (ushort)0;
                bool flag2 = Lighting.Mode == Terraria.Graphics.Light.LightMode.White || Lighting.Mode == Terraria.Graphics.Light.LightMode.Color;
                byte tileColor = t.TileColor;
                Color color = t.TileColor == (byte)31 ? Color.White : Lighting.GetColor(x, y);
                WorldGen.GetTreeFoliageDataMethod foliageDataMethod = new WorldGen.GetTreeFoliageDataMethod(drawType);
                if (frameY >= (short)198 && frameX >= (short)22)
                {
                    int treeFrame = WorldGen.GetTreeFrame(t);
                    int topTextureFrameWidth;
                    int topTextureFrameHeight;
                    int treeStyle = 0;
                    if (frameX == 22)
                    {
                        int xoffset1 = 0;
                        int grassPosX = x + xoffset1;
                        int floorY1 = y;
                        foliageDataMethod(x, y, xoffset1, ref treeFrame, ref treeStyle, out floorY1, out topTextureFrameWidth, out topTextureFrameHeight);
                        PaintSystem.Instance.EmitTreeLeaves(x, y, grassPosX, floorY1, ModContent.GoreType<Content.Tiles.Trees.SpiritTree.SpiritTreeFX>());
                        Texture2D treeTopTexture = top;
                        Vector2 position = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero;
                        float num5 = 0.0f;
                        if (!flag1) num5 = PaintSystem.Instance.GetWindCycle(x, y);
                        position.X += num5 * 2f;
                        position.Y += Math.Abs(num5) * 2f;
                        if (flag2) position += new Vector2(12f * 16f);
                        Main.spriteBatch.Draw(treeTopTexture, position, new Rectangle?(new Rectangle(treeFrame * (topTextureFrameWidth + 2), 0, topTextureFrameWidth, topTextureFrameHeight)), color, num5 * num2, new Vector2((float)(topTextureFrameWidth / 2), (float)topTextureFrameHeight), 1f, 0, 0f);
                    }
                    if (frameX == 44)
                    {
                        int num6 = x;
                        int floorY2 = y;
                        int xoffset2 = 1;
                        foliageDataMethod(x, y, xoffset2, ref treeFrame, ref treeStyle, out floorY2, out topTextureFrameWidth, out topTextureFrameHeight);
                        PaintSystem.Instance.EmitTreeLeaves(x, y, num6 + xoffset2, floorY2, ModContent.GoreType<Content.Tiles.Trees.SpiritTree.SpiritTreeFX>());
                        Vector2 position = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
                        float num8 = 0.0f;
                        if (!flag1) num8 = PaintSystem.Instance.GetWindCycle(x, y);
                        if ((double)num8 > 0.0) position.X += num8;
                        position.X += Math.Abs(num8) * 2f;
                        if (flag2) position += new Vector2(12f * 16f);
                        Main.spriteBatch.Draw(branch, position, new Rectangle?(new Rectangle(0, treeFrame * 42, 40, 40)), color, num8 * num3, new Vector2(40f, 24f), 1f, 0, 0f);
                    }
                    if (frameX == 66)
                    {
                        int num9 = x;
                        int floorY3 = y;
                        int xoffset3 = -1;
                        foliageDataMethod(x, y, xoffset3, ref treeFrame, ref treeStyle, out floorY3, out topTextureFrameWidth, out topTextureFrameHeight);
                        PaintSystem.Instance.EmitTreeLeaves(x, y, num9 + xoffset3, floorY3, ModContent.GoreType<Content.Tiles.Trees.SpiritTree.SpiritTreeFX>());
                        Texture2D treeBranchTexture = branch;
                        Vector2 position = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(0.0f, 18f);
                        float num11 = 0.0f;
                        if (!flag1) num11 = PaintSystem.Instance.GetWindCycle(x, y);
                        if ((double)num11 < 0.0) position.X += num11;
                        position.X -= Math.Abs(num11) * 2f;
                        if(flag2) position += new Vector2(12f * 16f);
                        Main.spriteBatch.Draw(treeBranchTexture, position, new Rectangle?(new Rectangle(42, treeFrame * 42, 40, 40)), color, num11 * num3, new Vector2(0.0f, 30f), 1f, 0, 0f);
                    }
                }
            }
        }

        public override void DrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            bool CheckGrass(int x, int y, int grassType, List<Point> visit)//dfs算法
            {
                visit.Add(new Point(x, y));
                int frameX = Main.tile[x, y].TileFrameX;
                int frameY = Main.tile[x, y].TileFrameY;
                bool flag = 22 <= frameX && frameX < 22 * 3 && 22 * 6 <= frameY && frameY < 22 * 9;
                if (Main.tile[x, y + 1] != null && Main.tile[x, y + 1].TileType == grassType && !flag) return true;
                else if (Main.tile[x, y + 1] != null && Main.tile[x, y + 1].TileType == 5) return CheckGrass(x, y + 1, grassType, visit);
                else if (!visit.Contains(new Point(x + 1, y)) && Main.tile[x + 1, y] != null && Main.tile[x + 1, y].TileType == 5) return CheckGrass(x + 1, y, grassType, visit);
                else if (!visit.Contains(new Point(x - 1, y)) && Main.tile[x - 1, y] != null && Main.tile[x - 1, y].TileType == 5) return CheckGrass(x - 1, y, grassType, visit);
                return false;
            }
            if (type == 5)
            {
                if (CheckGrass(i, j, CNCID.Tile.LegacyObject[CNCID.Tile.TranquilityGrass], new List<Point>()))
                {
                    Texture2D image = ModContent.Request<Texture2D>("CNC/Content/Tiles/Trees/SpiritTree").Value;
                    Texture2D top = ModContent.Request<Texture2D>("CNC/Content/Tiles/Trees/SpiritTreeTop").Value;
                    Texture2D branch = ModContent.Request<Texture2D>("CNC/Content/Tiles/Trees/SpiritTreeBranch").Value;
                    drawData.drawTexture = image;
                    DrawTree(new Point(i, j), top, branch, WorldGen.GetVanityTreeFoliageData);
                }
            }
        }

        public class PaintSystem : ModSystem
        {
            public static PaintSystem Instance = null;
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
                        if (num3 == 2) vector2 += new Vector2(0.0f, 8f);
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
}