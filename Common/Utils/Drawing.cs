using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CNC.Common.Utils
{
    public class Drawing
    {
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D image, Vector2 pos, float len, float rot, Color color, Vector2 point, int width = 1, float scaleY = 1f)
        {
            if (point.X + width <= image.Width)
            {
                Rectangle rect = new Rectangle((int)point.X, (int)point.Y, width, 1);
                spriteBatch.Draw(image, pos, rect, color, rot, Vector2.Zero, new Vector2((len + 1f) / width, scaleY), 0, 0);
                return;
            }
            for (int i = 0; i < width; i++)
            {
                Rectangle rect = new Rectangle((int)(point.X + i) % image.Width, (int)point.Y, 1, 1);
                spriteBatch.Draw(image, pos, rect, color, rot, Vector2.Zero, new Vector2((len + 1f) / width, scaleY), 0, 0);
                pos += MyMath.GetXY(rot) * 1f / width * (len + 1f);
            }
        }

        public static void DrawQuadrilateral(SpriteBatch spriteBatch, Texture2D image, Vector2[] vertex, int pointX, Color color)//待优化
        {
            /*
            0 →2
            ↓↘↓ 
            1 →3
            */
            for (int i = 1; i < image.Height; i++)
            {
                Vector2 pos1 = vertex[0] + (vertex[1] - vertex[0]) * ((float)i / image.Height);
                Vector2 pos2 = vertex[2] + (vertex[3] - vertex[2]) * ((float)i / image.Height);
                //DrawLine(image, pos1, pos2, color, new Vector2(pointX, i));
            }
        }

        public static Vector2 DrawRA_Trapezoid(SpriteBatch spriteBatch, Texture2D image, Vector2 pos, float len1, float len2, float rot, Color color, int pointX, int width = 1, float scale = 1f)
          => DrawRA_Trapezoid(spriteBatch, image, pos, len1, len2, rot, color, pointX, width, new Vector2(scale));

        public static Vector2 DrawRA_Trapezoid(SpriteBatch spriteBatch, Texture2D image, Vector2 pos, float len1, float len2, float rot, Color color, int pointX, int width = 1, Vector2? scale = null)
        {
            Vector2 _pos = new Vector2();
            Vector2 _scale = scale == null ? Vector2.One : (Vector2)scale;
            float cnt = image.Height;
            for (int i = 0; i < cnt; i++)
            {
                _pos = pos + new Vector2((float)(-i * Math.Sin(rot)), (float)(i * Math.Cos(rot))) * _scale.Y;
                DrawLine(spriteBatch, image, _pos, (len1 + (len2 - len1) * (i / cnt)) * _scale.X, rot, color, new Vector2(pointX, i), width, _scale.Y);
            }
            return pos + new Vector2((float)(len1 * Math.Cos(rot) * _scale.X), (float)(len1 * Math.Sin(rot) * _scale.X));
        }

        public static void DrawRing(SpriteBatch spriteBatch, Texture2D image, Vector2 position, float radiusMax, float radiusMin, Rectangle? source, Color color, float imageRot = 0f) { }

        public static void DrawTriangle(SpriteBatch spriteBatch, Texture2D image, Vector2[] vertex, Rectangle? source, Color color, float imageRot = 0f) { }

        public static void DrawQuadrilateral(SpriteBatch spriteBatch, Texture2D image, Vector2[] vertex, Rectangle? source, Color color, float imageRot = 0f)
        {
            Vector2 floor(Vector2 vector) => vector.ToPoint().ToVector2();
            Vector2 lt = floor(vertex[0]);
            Vector2 rt = floor(vertex[1]);
            Vector2 rb = floor(vertex[2]);
            Vector2 lb = floor(vertex[3]);
            float rot = imageRot;

            if (rb.X == lt.X || lb.X == rt.X) return;
            float k1 = (rb.Y - lt.Y) / (rb.X - lt.X);
            float k2 = (lb.Y - rt.Y) / (lb.X - rt.X);
            float b1 = (lt.Y - lt.X * k1);
            float b2 = (lb.Y - lb.X * k2);
            if (k1 == k2) return;
            Vector2 center = new Vector2((b2 - b1) / (k1 - k2), (b1 - b2) / (k2 - k1));

            Vector2 lt2 = floor(center + MyMath.GetXY((lt - center).ToRotation() - rot) * center.Distance(lt));
            Vector2 rt2 = floor(center + MyMath.GetXY((rt - center).ToRotation() - rot) * center.Distance(rt));
            Vector2 rb2 = floor(center + MyMath.GetXY((rb - center).ToRotation() - rot) * center.Distance(rb));
            Vector2 lb2 = floor(center + MyMath.GetXY((lb - center).ToRotation() - rot) * center.Distance(lb));

            float right = Math.Max(Math.Max(lt2.X, rt2.X), Math.Max(lb2.X, rb2.X));
            float left = Math.Min(Math.Min(lt2.X, rt2.X), Math.Min(lb2.X, rb2.X));
            float top = Math.Min(Math.Min(lt2.Y, rt2.Y), Math.Min(lb2.Y, rb2.Y));
            float bottom = Math.Max(Math.Max(lt2.Y, rt2.Y), Math.Max(lb2.Y, rb2.Y));

            Vector2 lt3 = floor(new Vector2(left, top));
            Vector2 rt3 = floor(new Vector2(right, top));
            Vector2 rb3 = floor(new Vector2(right, bottom));
            Vector2 lb3 = floor(new Vector2(left, bottom));

            Vector2 lt4 = floor(center + MyMath.GetXY((lt3 - center).ToRotation() - rot) * center.Distance(lt3));
            Vector2 rt4 = floor(center + MyMath.GetXY((rt3 - center).ToRotation() - rot) * center.Distance(rt3));
            Vector2 rb4 = floor(center + MyMath.GetXY((rb3 - center).ToRotation() - rot) * center.Distance(rb3));
            Vector2 lb4 = floor(center + MyMath.GetXY((lb3 - center).ToRotation() - rot) * center.Distance(lb3));

            float width = right - left;
            float height = bottom - top;
            /*爷不干了，有空再补
            Main.NewText($"{lt}|{rt}|{rb}|{lb}");
            Main.NewText($"{lt2}|{rt2}|{rb2}|{lb2}");
            Main.NewText($"{lt3}|{rt3}|{rb3}|{lb3}");
            Main.NewText($"{lt4}|{rt4}|{rb4}|{lb4}");
            */
            for (int i = 0; i < width; i++)
            {
                int _pointX = (int)(i * image.Width / width) % image.Width;
                if (source != null) _pointX = (int)(((Rectangle)source).X + i * ((Rectangle)source).Width / width) % image.Width;
                Vector2 pos = floor(lt4) + floor((rt4 - lt4) * i / width);
                Vector2 scale = new Vector2(1f, height / image.Height);
                Rectangle rect = new Rectangle(_pointX, 0, 1, source == null ? image.Height : ((Rectangle)source).Height);
                Vector2 offset = Vector2.Zero;
                float height2 = height;
                spriteBatch.Draw(image, new Rectangle((int)(pos.X + offset.X), (int)(pos.Y + offset.Y), 1, (int)height2), rect, color, rot, Vector2.Zero, 0, 0);
            }
        }

        public static void DrawRA_Trapezoid(SpriteBatch spriteBatch, Texture2D image, Vector2[] vertex, Color color, float rot, int pointX, int width = 1, float scaleY = 1f)
        {
            Vector2 floor(Vector2 vector) => vector.ToPoint().ToVector2();
            Vector2 lt = vertex[0]; 
            Vector2 rt = vertex[1];
            Vector2 rb = vertex[2];
            Vector2 lb = vertex[3];
            float len1 = (rt.Distance(lt));
            float len2 = (rb.Distance(lb));
            if (true || len1 > len2)
            {
                float rot1 = (rt - lt).ToRotation();
                for (int i = 0; i < len1; i++)
                {
                    int _pointX = pointX + (int)(i * width / len1);
                    Vector2 pos = (floor(lt) + (floor(rt) - floor(lt)) * (i / len1));
                    Vector2 pos2 = (floor(lb) + (floor(rb) - floor(lb)) * (i / len2));
                    spriteBatch.Draw(image, pos, new Rectangle(_pointX, 0, 1, image.Height), color, rot1, Vector2.Zero, new Vector2(1, pos2.Distance(pos) / image.Height), 0, 0);
                }
            }
            else
            {
                float rot1 = (rt - lt).ToRotation();
                float rot2 = (rb - lb).ToRotation();
                len1 = rb.X - lb.X;
                for (int i = 0; i < len2; i++)
                {
                    int _pointX = pointX + (int)(i * width / len2);
                    Vector2 dist = (-new Vector2(-(float)Math.Sin(rot), (float)Math.Cos(rot)) * image.Height * scaleY);
                    Vector2 pos = (floor(lt) + (floor(rt) - floor(lt)) * (i / len1));
                    Vector2 pos2 = (floor(lb) + (floor(rb) - floor(lb)) * (i / len2));
                    spriteBatch.Draw(image, pos2, new Rectangle(_pointX, 0, 1, image.Height), color, rot1 - rot2 + rot, Vector2.Zero, new Vector2(1, scaleY), 0, 0);
                }
            }
        }

        public static void DrawTrail(SpriteBatch spriteBatch, Texture2D image, int max, Vector2[] oldPos, float[] oldRot, Vector2 extraPos, float extraRot, Color color, int pointX, int originY, float scaleY, int width = 1)
        {
            for (int i = 0; i < max - 1; i++)
            {
                int origin2 = image.Height - originY;
                Vector2 pos1 = oldPos[i] - Main.screenPosition;
                Vector2 pos2 = oldPos[i + 1] - Main.screenPosition;
                float rot1 = oldRot[i] + extraRot;
                float rot2 = oldRot[i + 1] + extraRot;
                Vector2[] vertex = {
                    pos1 + extraPos - new Vector2(-(float)Math.Sin(rot1), (float)Math.Cos(rot1)) * originY * scaleY,
                    pos2 + extraPos - new Vector2(-(float)Math.Sin(rot1), (float)Math.Cos(rot1)) * originY * scaleY,
                    pos2 + extraPos + new Vector2(-(float)Math.Sin(rot2), (float)Math.Cos(rot2)) * origin2 * scaleY,
                    pos1 + extraPos + new Vector2(-(float)Math.Sin(rot2), (float)Math.Cos(rot2)) * origin2 * scaleY,
                };
                DrawRA_Trapezoid(spriteBatch, image, vertex, color, oldRot[i], (pointX + i * width), width, scaleY);
            }
        }

        /// <summary> 简易拖尾：相当吃性能慎用！ </summary>
        public static void DrawTrailDirect(SpriteBatch spriteBatch, Texture2D image, Vector2[] oldPos, float[] oldRot, Vector2 extraPos, float extraRot, Color color, int pointX, int originY, Vector2 scale, Vector2? scale2 = null, bool fade = true, int width = 1, int bright = 1)
        {
            int max = oldPos.Length < oldRot.Length ? oldPos.Length : oldRot.Length;
            DrawTrailDirect(spriteBatch, image, max, oldPos, oldRot, extraPos, extraRot, color, pointX, originY, scale, scale2, fade, width, bright);
        }

        /// <summary> 简易拖尾：相当吃性能慎用！ </summary>
        public static void DrawTrailDirect(SpriteBatch spriteBatch, Texture2D image, int max, Vector2[] oldPos, float[] oldRot, Vector2 extraPos, float extraRot, Color color, int pointX, int originY, Vector2 scale, Vector2? scale2 = null, bool fade = true, int width = 1, int bright = 1)
        {
            DrawTrailDirect(spriteBatch, image, max, oldPos, oldRot, extraPos, extraRot, new Color[1] { color }, pointX, originY, scale, scale2, fade, width, bright);
        }

        /// <summary> 简易拖尾：相当吃性能慎用！ </summary>
        public static void DrawTrailDirect(SpriteBatch spriteBatch, Texture2D image, int max, Vector2[] oldPos, float[] oldRot, Vector2 extraPos, float extraRot, Color[] color, int pointX, int originY, Vector2 scale, Vector2? scale2 = null, bool fade = true, int width = 1, int bright = 1)
        {
            if (max <= 1) return;
            Vector2[] pos1 = new Vector2[max];
            Vector2[] pos2 = new Vector2[max];
            float[] len1 = new float[max];
            float[] len2 = new float[max];
            Vector2[] _scale = new Vector2[max];
            int origin2 = image.Height - originY;
            float rot = oldRot[0] + extraRot;
            if (rot < 0) rot = MyMath.pi * 2 - ((-rot) % MyMath.pi * 2);
            _scale[0] = scale;
            pos1[0] = oldPos[0] + extraPos - new Vector2(-(float)(Math.Sin(rot) * originY), (float)(Math.Cos(rot) * originY)) * _scale[0].Y - Main.screenPosition;
            pos2[0] = oldPos[0] + extraPos + new Vector2(-(float)(Math.Sin(rot) * origin2), (float)(Math.Cos(rot) * origin2)) * _scale[0].Y - Main.screenPosition;
            for (int i = 1; i < max; i++)
            {
                rot = oldRot[i] + extraRot;
                Vector2 _scale2 = scale2 == null ? scale : (Vector2)scale2;
                _scale[i] = scale - ((scale - _scale2) * ((float)i / max));
                if (rot < 0) rot = MyMath.pi * 2 - ((-rot) % MyMath.pi * 2);
                pos1[i] = oldPos[i] + extraPos - new Vector2(-(float)(Math.Sin(rot) * originY), (float)(Math.Cos(rot) * originY)) * _scale[i].Y - Main.screenPosition;
                pos2[i] = oldPos[i] + extraPos + new Vector2(-(float)(Math.Sin(rot) * origin2), (float)(Math.Cos(rot) * origin2)) * _scale[i].Y - Main.screenPosition;
                len1[i - 1] = pos1[i - 1].Distance(pos1[i]) - 1f;
                len2[i - 1] = pos2[i - 1].Distance(pos2[i]) - 1f;
                pos1[i] -= new Vector2((float)(len1[i - 1] * Math.Cos(rot)), (float)(len1[i - 1] * Math.Sin(rot))) * (1 - scale.X);
                pos2[i] -= new Vector2((float)(len2[i - 1] * Math.Cos(rot)), (float)(len2[i - 1] * Math.Sin(rot))) * (1 - scale.X);
            }
            for (int i = 0; i < max - 1; i++)
            {
                float alpha = fade ? (float)(max - i) / max : 1;
                int idx = i % color.Length;
                rot = oldRot[i] + extraRot;
                if (rot < 0) rot = MyMath.pi * 2 - ((-rot) % MyMath.pi * 2);
                for(int j = 0; j < bright; j++) DrawRA_Trapezoid(spriteBatch, image, pos1[i], len1[i], len2[i], rot, color[idx] * alpha, (pointX + i * width) % image.Width, width, _scale[i]);
            }
        }

        public static void DrawChain(SpriteBatch spriteBatch, Texture2D image, Rectangle box, Color color = default)
        {

        }

        public static void DrawBox(SpriteBatch spriteBatch, Texture2D image, Rectangle box, Color color = default, float corner = 16f)
            => DrawBox(spriteBatch, image, box, color, corner);

        public static void DrawBox(SpriteBatch spriteBatch, Texture2D image, Rectangle box, Color color = default, Vector2 corner = default)
        {
            Rectangle _rect = box;
            int mw = (int)corner.X;
            int mh = (int)corner.Y;
            int _mw = mw * 2;
            int _mh = mh * 2;
            int _w = image.Width;
            int _h = image.Height;
            int w = _rect.Width < mw ? mw : _rect.Width;
            int h = _rect.Height < mh ? mh : _rect.Height;
            int x = _rect.X;
            int y = _rect.Y;
            int xw = x + w;
            int yh = y + h;
            Rectangle[] dRect = new Rectangle[9]
            {
                new Rectangle(x, y, mw, mh),
                new Rectangle(x + mw, y, w - _mw, mh),
                new Rectangle(x + w - mw, y, mw, mh),
                new Rectangle(x, y + mh, mw, h - _mh),
                new Rectangle(x + mw, y + mh, w - _mw, h - _mh),
                new Rectangle(x + w - mw, y + mh, mw, h - _mh),
                new Rectangle(x, y + h - mh, mw, mh),
                new Rectangle(x + mw, y + h - mh, w - _mw, mh),
                new Rectangle(x + w - mw, y + h - mh, mw, mh),
            };
            Rectangle?[] sRect = new Rectangle?[9]
            {
                new Rectangle(0, 0, mw, mh),
                new Rectangle(mw, 0, _w - _mw, mh),
                new Rectangle(_w - mw, 0, mw, mh),
                new Rectangle(0, mh, mw, _h - _mh),
                new Rectangle(mw, mh, _w - _mw, _h - _mh),
                new Rectangle(_w - mw, mh, mw, _h - _mh),
                new Rectangle(0, _h - mh, mw, mh),
                new Rectangle(mw, _h - mh, _w - _mw, mh),
                new Rectangle(_w - mw, _h - mh, mw, mh),
            };
            for (int i = 0; i < 9; i++) { spriteBatch.Draw(image, dRect[i], sRect[i], color); }
        }

        public static void DrawString(string text)
        {

        }

        public class DrawDataSimple
        {
            List<Texture2D> textures;
            int[][] range;
            int drawType;
            int width;
            int height;

            public enum DrawType
            {
                None,//（不混合）就是来源混合数为1，目标混合数为0。即后来的图像覆盖原有的图像。
                Additive,//（添加混合）是将来源的混合参数和目标的混合参数都设置成1，这样混合白色会没有变化，其他的颜色看上去会更加明亮，黑色会更加明亮如图1，DNF的魔法阵周遍是完全的黑色所以想过是直接显示了底层的ELLY图片。这要求美工作出的图片必须符合要求。
                AlphaBlend,//（透明度混合）是将来源的Alpha作为来源混合参数，将1-Alpha作为目标混合参数。
            }

            public DrawDataSimple(int width, int height)
            {
                this.textures = new List<Texture2D>();
                this.range = new int[height][];
                for (int i = 0; i < height; i++) this.range[i] = new int[width];
                this.width = width;
                this.height = height;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                
            }
        }
    }
}