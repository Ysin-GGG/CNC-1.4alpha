using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

using static Terraria.ModLoader.ModContent;

namespace CNC.Common.Utils
{
    public class MyMath
    {
        public const double G = 9.8061905036721905;
        public const double E = 2.7182818284590451;
        public const double Pi = 3.1415926535897931;
        public const double Pi2 = pi * 2;
        public const double Deg01 = Pi / 180;
        public const double Deg30 = Pi / 6;
        public const double Deg45 = Pi / 4;
        public const double Deg60 = Pi / 3;
        public const double Deg90 = Pi / 2;
        public const float g = 9.8066500f;
        public const float e = 2.7182818f;
        public const float pi = 3.1415927f;
        public const float pi2 = pi * 2;
        public const float deg01 = pi / 180;
        public const float deg30 = pi / 6;
        public const float deg45 = pi / 4;
        public const float deg60 = pi / 3;
        public const float deg90 = pi / 2;
        public static float cos(double degree) => (float)Math.Cos(degree);
        public static float sin(double degree) => (float)Math.Sin(degree);
        public static float tan(double degree) => (float)Math.Tan(degree);
        public static float pow(double x, double a) => (float)Math.Pow(x, a);
        public static float GetZ(double x, double y) => (float)Math.Sqrt(x * x + y * y);
        public static Vector2 GetXY(double degree) => new Vector2(cos(degree), sin(degree));
        public static int Lerp(int value1, int value2, float percentage) => (int)(value1 * (1f - MyMath.Clamp(percentage, 0, 1)) + value2 * MyMath.Clamp(percentage, 0, 1));
        public static float Lerp(float value1, float value2, float percentage) => (float)(value1 * (1f - MyMath.Clamp(percentage, 0, 1)) + value2 * MyMath.Clamp(percentage, 0, 1));
        public static Point Lerp(Point value1, Point value2, float percentage) => MyMath.Lerp(value1, value2, new Vector2(percentage));
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float percentage) => MyMath.Lerp(value1, value2, new Vector2(percentage));
        public static Point Lerp(Point value1, Point value2, Vector2 percentage) => new Point((int)MyMath.Lerp(value1.X, value2.X, percentage.X), (int)MyMath.Lerp(value1.X, value2.X, percentage.Y));
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, Vector2 percentage) => new Vector2((float)MyMath.Lerp(value1.X, value2.X, percentage.X), (float)MyMath.Lerp(value1.X, value2.X, percentage.Y));
        public static int Clamp(int value, int min, int max) => Math.Min(Math.Max(min, value), max);
        public static int Clamp(int value, int min, int max, out int extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static float Clamp(float value, float min, float max) => Math.Min(Math.Max(min, value), max);
        public static float Clamp(float value, float min, float max, out float extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static Point Clamp(Point value, Point min, Point max) => new Point(MyMath.Clamp(value.X, min.X, max.X), MyMath.Clamp(value.Y, min.Y, max.Y));
        public static Point Clamp(Point value, Point min, Point max, out Point extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) => new Vector2(MyMath.Clamp(value.X, min.X, max.X), MyMath.Clamp(value.Y, min.Y, max.Y));
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max, out Vector2 extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
    }
}