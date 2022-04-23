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
        
        /// <summary> 绘制宽度为1的纹理线条 </summary>
        /// <param name="image">纹理</param>
        /// <param name="pos1">点1</param>
        /// <param name="pos2">点2</param>
        /// <param name="source">纹理截取</param>
        /// <param name="color">颜色</param>
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D image, Vector2 pos1, Vector2 pos2, Rectangle? source, Color color, float extraRot = 0, bool drawVertically = false, SpriteEffects effect = 0)
        {
            int dist = (int)pos1.Distance(pos2);
            float rot = (pos2 - pos1).ToRotation() + extraRot - (drawVertically ? MyMath.deg90 : 0f);
            Rectangle drawRect = new Rectangle((int)pos1.X, (int)pos1.Y, drawVertically ? 1 : dist, drawVertically ? dist : 1);
            spriteBatch.Draw(image, drawRect, source, color, rot, default, effect, 0);
        }

        public static void DrawQuadrilateral(SpriteBatch spriteBatch, Texture2D image, Vector2[] vertex, Rectangle? source, Color color, float imageRot = 0f, bool cut = false, SpriteEffects drawType = 0)
        {
            Vector2 floor(Vector2 vector) => vector.ToPoint().ToVector2();
            Vector2 ceiling(Vector2 vector) => new Vector2((float)Math.Ceiling(vector.X), (float)Math.Ceiling(vector.Y));
            float rot = imageRot % MyMath.pi / 2;

            Vector2 lt = floor(vertex[0]);
            Vector2 rt = floor(vertex[1]);
            Vector2 rb = floor(vertex[2]);
            Vector2 lb = floor(vertex[3]);
            float left = MyMath.Min(lt.X, rt.X, rb.X, lb.X);
            float top = MyMath.Min(lt.Y, rt.Y, rb.Y, lb.Y);
            float right = MyMath.Max(lt.X, rt.X, rb.X, lb.X);
            float bottom = MyMath.Max(lt.Y, rt.Y, rb.Y, lb.Y);
            float width = right - left;
            float height = bottom - top;
            Vector2 center = new Vector2(left + width / 2f, top + height / 2f);

            Vector2 lt2 = floor(center + MyMath.GetXY((lt - center).ToRotation() - imageRot) * Vector2.Distance(lt, center));
            Vector2 rt2 = floor(center + MyMath.GetXY((rt - center).ToRotation() - imageRot) * Vector2.Distance(rt, center));
            Vector2 rb2 = floor(center + MyMath.GetXY((rb - center).ToRotation() - imageRot) * Vector2.Distance(rb, center));
            Vector2 lb2 = floor(center + MyMath.GetXY((lb - center).ToRotation() - imageRot) * Vector2.Distance(lb, center));

            float left2 = MyMath.Min(lt2.X, rt2.X, rb2.X, lb2.X);
            float top2 = MyMath.Min(lt2.Y, rt2.Y, rb2.Y, lb2.Y);
            float right2 = MyMath.Max(lt2.X, rt2.X, rb2.X, lb2.X);
            float bottom2 = MyMath.Max(lt2.Y, rt2.Y, rb2.Y, lb2.Y);
            float width2 = right2 - left2;
            float height2 = bottom2 - top2;

            Vector2 center2 = new Vector2(left2 + width2 / 2f, top2 + height2 / 2f);

            //if (width2 < height2)
            {
                float[] len1 = {//上边缘
                    MyMath.Max(lt2.X - lb2.X, 0f),
                    MyMath.Max(rt2.X - lt2.X, 0f),
                    MyMath.Max(rb2.X - rt2.X, 0f),
                };
                float[] len2 = {//下边缘
                    MyMath.Max(lb2.X - lt2.X, 0f),
                    MyMath.Max(rb2.X - lb2.X, 0f),
                    MyMath.Max(rt2.X - rb2.X, 0f),
                };
                for (int i = 0; i < width2 - 1; i++)
                {
                    Vector2 pos1 = new Vector2();
                    Vector2 pos2 = new Vector2();
                    float precent1 = 0f;
                    float precent2 = 0f;
                    if (i < len1[0])
                    {
                        precent1 = i / len1[0];
                        pos1 = MyMath.Lerp(lb2, lt2, precent1);
                    }
                    else if (i < len1[0] + len1[1])
                    {
                        precent1 = (i - len1[0]) / len1[1];
                        pos1 = MyMath.Lerp(lt2, rt2, precent1);
                    }
                    else
                    {
                        precent1 = (i - len1[0] - len1[1]) / len1[2];
                        pos1 = MyMath.Lerp(rt2, rb2, precent1);
                    }
                    pos1 = ceiling((center2) + (MyMath.GetXY((pos1 - center2).ToRotation() + imageRot) * Vector2.Distance(pos1, center2)));
                    if (i < len2[0])
                    {
                        precent2 = i / len2[0];
                        pos2 = MyMath.Lerp(lt2, lb2, precent2);
                    }
                    else if (i < len2[0] + len2[1])
                    {
                        precent2 = (i - len2[0]) / len2[1];
                        pos2 = MyMath.Lerp(lb2, rb2, precent2);
                    }
                    else
                    {
                        precent2 = (i - len2[0] - len2[1]) / len2[2];
                        pos2 = MyMath.Lerp(rb2, rt2, precent2);
                    }
                    pos2 = ceiling((center2) + (MyMath.GetXY((pos2 - center2).ToRotation() + imageRot) * Vector2.Distance(pos2, center2)));
                    int wid = source == null ? image.Width : ((Rectangle)source).Width;
                    int hei = source == null ? image.Height : ((Rectangle)source).Height;
                    int pointX = (int)((source == null ? ((int)(i / width * wid)) : ((Rectangle)source).X) + (((int)drawType == 1) ? (int)(i / width * wid) : (wid - (int)(i / width * wid))));
                    Rectangle rect = new Rectangle(pointX, 0, 1, hei);
                    int FlipVertically = (int)drawType / 2 * 2;
                    if (cut)
                    {
                        if (FlipVertically > 0)
                        {

                        }
                        else
                        {
                            rect.Y = (int)((pos1.Y - top) / height * hei);
                            rect.Height = (int)Math.Ceiling(pos1.Distance(pos2) / height * hei);
                        }
                    }
                    Drawing.DrawLine(spriteBatch, image, pos1, pos2, rect, color, 0, true, (SpriteEffects)((int)drawType % 2));
                }
                Drawing.DrawLine(spriteBatch, image, lt2, rt2, null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, rt2, rb2, null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, rb2, lb2, null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, lb2, lt2, null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(left, top), new Vector2(right, top), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(right, top), new Vector2(right, bottom), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(right, bottom), new Vector2(left, bottom), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(left, bottom), new Vector2(left, top), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(left2, top2), new Vector2(right2, top2), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(right2, top2), new Vector2(right2, bottom2), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(right2, bottom2), new Vector2(left2, bottom2), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
                Drawing.DrawLine(spriteBatch, image, new Vector2(left2, bottom2), new Vector2(left2, top2), null, color, 0, true, (SpriteEffects)((int)drawType % 2));
            }

            /*
            Rectangle Box = new Rectangle((int)left, (int)top, (int)width, (int)height);
            //（最难点）纹理旋转未完成
            //积分原理绘制
            //if (width < height)
            {
                float posX = left;
                float[] len1 = {
                    MyMath.Max(lt.X - lb.X, 0f),
                    MyMath.Max(rt.X - lt.X, 0f),
                    MyMath.Max(rb.X - rt.X, 0f),
                };
                float[] len2 = {
                    MyMath.Max(lb.X - lt.X, 0f),
                    MyMath.Max(rb.X - lb.X, 0f),
                    MyMath.Max(rt.X - rb.X, 0f),
                };
                for (int i = 0; i < width; i++)
                {
                    Vector2 pos = new Vector2(posX + i, 0);
                    float precent1 = 0f;
                    float precent2 = 0f;
                    float len = 0f;
                    if (i < len1[0])
                    {
                        precent1 = i / len1[0];
                        pos.Y += MyMath.Lerp(lb.Y, lt.Y, precent1);
                    }
                    else if (i < len1[0] + len1[1])
                    {
                        precent1 = (i - len1[0]) / len1[1];
                        pos.Y += MyMath.Lerp(lt.Y, rt.Y, precent1);
                    }
                    else
                    {
                        precent1 = (i - len1[0] - len1[1]) / len1[2];
                        pos.Y += MyMath.Lerp(rt.Y, rb.Y, precent1);
                    }
                    if (i < len2[0])
                    {
                        precent2 = i / len2[0];
                        len = MyMath.Lerp(lt.Y, lb.Y, precent2) - pos.Y;
                    }
                    else if (i < len2[0] + len2[1])
                    {
                        precent2 = (i - len2[0]) / len2[1];
                        len = MyMath.Lerp(lb.Y, rb.Y, precent2) - pos.Y;
                    }
                    else
                    {
                        precent2 = (i - len2[0] - len2[1]) / len2[2];
                        len = MyMath.Lerp(rb.Y, rt.Y, precent2) - pos.Y;
                    }
                    int wid = source == null ? image.Width : ((Rectangle)source).Width;
                    int hei = source == null ? image.Height : ((Rectangle)source).Height;
                    int pointX = (int)((source == null ? 0 : ((Rectangle)source).X) + (((int)drawType == 1) ? (wid - (int)(i / width * wid)) : (int)(i / width * wid)) % wid);
                    Rectangle rect = new Rectangle(pointX, 0, 1, hei);
                    int FlipVertically = (int)drawType / 2 * 2;
                    if (imageRot >= MyMath.pi / 2) { FlipVertically = FlipVertically == 1 ? 0 : 1; }
                    if (cut)
                    {
                        if (FlipVertically > 0)
                        {

                        }
                        else
                        {
                            rect.Y = (int)((pos.Y - top) / height * hei);
                            rect.Height = (int)Math.Ceiling(len / height * hei);
                        }
                        spriteBatch.Draw(image, new Rectangle((int)pos.X, (int)pos.Y, 1, (int)Math.Ceiling(len)), rect, color, 0, default, (SpriteEffects)FlipVertically, 0);
                        //spriteBatch.Draw(image, pos, rect, color, 0, default, new Vector2(1f, height / hei), 0, 0);
                    }
                    else spriteBatch.Draw(image, new Rectangle((int)pos.X, (int)pos.Y, 1, (int)Math.Ceiling(len)), rect, color, 0, default, (SpriteEffects)FlipVertically, 0);
                    //spriteBatch.Draw(image, pos, rect, color, 0, default, new Vector2(1f, len / hei), 0, 0);
                }
            }
            */
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

        public static void DrawTrail(SpriteBatch spriteBatch, Texture2D image, int max, Vector2[] oldPos, float[] oldRot, Vector2 extraPos, float extraRot, Color color, int pointX, int originY, Vector2 scaleY, int width = 1)
        {
            for (int i = 0; i < max - 1; i++)
            {
                int origin2 = image.Height - originY;
                Vector2 pos1 = oldPos[i] - Main.screenPosition;
                Vector2 pos2 = oldPos[i + 1] - Main.screenPosition;
                float rot1 = oldRot[i] + extraRot;
                float rot2 = oldRot[i + 1] + extraRot;
                Vector2[] vertex = {
                    pos1 + extraPos - new Vector2(-(float)Math.Sin(rot1), (float)Math.Cos(rot1)) * originY * MyMath.Lerp(scaleY.X, scaleY.Y, (float)i / (max - 1)),
                    pos2 + extraPos - new Vector2(-(float)Math.Sin(rot1), (float)Math.Cos(rot1)) * originY * MyMath.Lerp(scaleY.X, scaleY.Y, (float)i / (max - 1)),
                    pos2 + extraPos + new Vector2(-(float)Math.Sin(rot2), (float)Math.Cos(rot2)) * origin2 * MyMath.Lerp(scaleY.X, scaleY.Y, (float)i / (max - 1)),
                    pos1 + extraPos + new Vector2(-(float)Math.Sin(rot2), (float)Math.Cos(rot2)) * origin2 * MyMath.Lerp(scaleY.X, scaleY.Y, (float)i / (max - 1)),
                };
                Rectangle rect = new Rectangle((pointX + i * width) % image.Width, 0, width, image.Height);
                DrawQuadrilateral(spriteBatch, image, vertex, rect, color, rot1, false, SpriteEffects.FlipHorizontally);
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