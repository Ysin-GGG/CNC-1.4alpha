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
    }
}