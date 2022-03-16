using System;
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
    }
}