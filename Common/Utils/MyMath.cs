using System;
using System.Linq;
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
        public static float log(double x, double a) => (float)Math.Pow(x, a);
        public static float GetZ(double x, double y) => (float)Math.Sqrt(x * x + y * y);
        public static Vector2 GetXY(double degree) => new Vector2(MyMath.cos(degree), MyMath.sin(degree));
        public static int Lerp(int value1, int value2, float percentage) => (int)(value1 * (1f - MyMath.Clamp(percentage, 0, 1)) + value2 * MyMath.Clamp(percentage, 0, 1));
        public static float Lerp(float value1, float value2, float percentage) => (float)(value1 * (1f - MyMath.Clamp(percentage, 0, 1)) + value2 * MyMath.Clamp(percentage, 0, 1));
        public static Point Lerp(Point value1, Point value2, float percentage) => MyMath.Lerp(value1, value2, new Vector2(percentage));
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float percentage) => MyMath.Lerp(value1, value2, new Vector2(percentage));
        public static Point Lerp(Point value1, Point value2, Vector2 percentage) => new Point((int)MyMath.Lerp(value1.X, value2.X, percentage.X), (int)MyMath.Lerp(value1.X, value2.X, percentage.Y));
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, Vector2 percentage) => new Vector2((float)MyMath.Lerp(value1.X, value2.X, percentage.X), (float)MyMath.Lerp(value1.X, value2.X, percentage.Y));
        public static int Clamp(int value, int min, int max) => MyMath.Min(MyMath.Max(min, value), max);
        public static int Clamp(int value, int min, int max, out int extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static float Clamp(float value, float min, float max) => MyMath.Min(MyMath.Max(min, value), max);
        public static float Clamp(float value, float min, float max, out float extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static Point Clamp(Point value, Point min, Point max) => new Point(MyMath.Clamp(value.X, min.X, max.X), MyMath.Clamp(value.Y, min.Y, max.Y));
        public static Point Clamp(Point value, Point min, Point max, out Point extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) => new Vector2(MyMath.Clamp(value.X, min.X, max.X), MyMath.Clamp(value.Y, min.Y, max.Y));
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max, out Vector2 extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        
        public static byte Max(byte value1, byte value2, byte value3, byte value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static byte Max(byte value1, byte value2, byte value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static byte Max(byte value1, byte value2) => (value1 > value2) ? value1 : value2;
        public static byte Max(byte[] values) => values.Max();
        public static sbyte Max(sbyte value1, sbyte value2, sbyte value3, sbyte value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static sbyte Max(sbyte value1, sbyte value2, sbyte value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static sbyte Max(sbyte value1, sbyte value2) => (value1 > value2) ? value1 : value2;
        public static sbyte Max(sbyte[] values) => values.Max();
        public static short Max(short value1, short value2, short value3, short value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static short Max(short value1, short value2, short value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static short Max(short value1, short value2) => (value1 > value2) ? value1 : value2;
        public static short Max(short[] values) => values.Max();
        public static ushort Max(ushort value1, ushort value2, ushort value3, ushort value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static ushort Max(ushort value1, ushort value2, ushort value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static ushort Max(ushort value1, ushort value2) => (value1 > value2) ? value1 : value2;
        public static ushort Max(ushort[] values) => values.Max();
        public static int Max(int value1, int value2, int value3, int value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static int Max(int value1, int value2, int value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static int Max(int value1, int value2) => (value1 > value2) ? value1 : value2;
        public static int Max(int[] values) => values.Max();
        public static uint Max(uint value1, uint value2, uint value3, uint value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static uint Max(uint value1, uint value2, uint value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static uint Max(uint value1, uint value2) => (value1 > value2) ? value1 : value2;
        public static uint Max(uint[] values) => values.Max();
        public static long Max(long value1, long value2, long value3, long value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static long Max(long value1, long value2, long value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static long Max(long value1, long value2) => (value1 > value2) ? value1 : value2;
        public static long Max(long[] values) => values.Max();
        public static ulong Max(ulong value1, ulong value2, ulong value3, ulong value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static ulong Max(ulong value1, ulong value2, ulong value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static ulong Max(ulong value1, ulong value2) => (value1 > value2) ? value1 : value2;
        public static ulong Max(ulong[] values) => values.Max();
        public static float Max(float value1, float value2, float value3, float value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static float Max(float value1, float value2, float value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static float Max(float value1, float value2) => (value1 > value2) ? value1 : value2;
        public static float Max(float[] values) => values.Max();
        public static double Max(double value1, double value2, double value3, double value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static double Max(double value1, double value2, double value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static double Max(double value1, double value2) => (value1 > value2) ? value1 : value2;
        public static double Max(double[] values) => values.Max();
        public static char Max(char value1, char value2, char value3, char value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static char Max(char value1, char value2, char value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static char Max(char value1, char value2) => (value1 > value2) ? value1 : value2;
        public static char Max(char[] values) => values.Max();
        public static decimal Max(decimal value1, decimal value2, decimal value3, decimal value4) => (((value1 > value2) ? value1 : value2) > ((value3 > value4) ? value3 : value4)) ? ((value1 > value2) ? value1 : value2) : ((value3 > value4) ? value3 : value4);
        public static decimal Max(decimal value1, decimal value2, decimal value3) => (((value1 > value2) ? value1 : value2) > value3) ? ((value1 > value2) ? value1 : value2) : value3;
        public static decimal Max(decimal value1, decimal value2) => (value1 > value2) ? value1 : value2;
        public static decimal Max(decimal[] values) => values.Max();

        public static byte Min(byte value1, byte value2, byte value3, byte value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static byte Min(byte value1, byte value2, byte value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static byte Min(byte value1, byte value2) => (value1 < value2) ? value1 : value2;
        public static byte Min(byte[] values) => values.Min();
        public static sbyte Min(sbyte value1, sbyte value2, sbyte value3, sbyte value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static sbyte Min(sbyte value1, sbyte value2, sbyte value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static sbyte Min(sbyte value1, sbyte value2) => (value1 < value2) ? value1 : value2;
        public static sbyte Min(sbyte[] values) => values.Min();
        public static short Min(short value1, short value2, short value3, short value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static short Min(short value1, short value2, short value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static short Min(short value1, short value2) => (value1 < value2) ? value1 : value2;
        public static short Min(short[] values) => values.Min();
        public static ushort Min(ushort value1, ushort value2, ushort value3, ushort value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static ushort Min(ushort value1, ushort value2, ushort value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static ushort Min(ushort value1, ushort value2) => (value1 < value2) ? value1 : value2;
        public static ushort Min(ushort[] values) => values.Min();
        public static int Min(int value1, int value2, int value3, int value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static int Min(int value1, int value2, int value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static int Min(int value1, int value2) => (value1 < value2) ? value1 : value2;
        public static int Min(int[] values) => values.Min();
        public static uint Min(uint value1, uint value2, uint value3, uint value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static uint Min(uint value1, uint value2, uint value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static uint Min(uint value1, uint value2) => (value1 < value2) ? value1 : value2;
        public static uint Min(uint[] values) => values.Min();
        public static long Min(long value1, long value2, long value3, long value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static long Min(long value1, long value2, long value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static long Min(long value1, long value2) => (value1 < value2) ? value1 : value2;
        public static long Min(long[] values) => values.Min();
        public static ulong Min(ulong value1, ulong value2, ulong value3, ulong value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static ulong Min(ulong value1, ulong value2, ulong value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static ulong Min(ulong value1, ulong value2) => (value1 < value2) ? value1 : value2;
        public static ulong Min(ulong[] values) => values.Min();
        public static float Min(float value1, float value2, float value3, float value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static float Min(float value1, float value2, float value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static float Min(float value1, float value2) => (value1 < value2) ? value1 : value2;
        public static float Min(float[] values) => values.Min();
        public static double Min(double value1, double value2, double value3, double value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static double Min(double value1, double value2, double value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static double Min(double value1, double value2) => (value1 < value2) ? value1 : value2;
        public static double Min(double[] values) => values.Min();
        public static char Min(char value1, char value2, char value3, char value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static char Min(char value1, char value2, char value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static char Min(char value1, char value2) => (value1 < value2) ? value1 : value2;
        public static char Min(char[] values) => values.Min();
        public static decimal Min(decimal value1, decimal value2, decimal value3, decimal value4) => (((value1 < value2) ? value1 : value2) < ((value3 < value4) ? value3 : value4)) ? ((value1 < value2) ? value1 : value2) : ((value3 < value4) ? value3 : value4);
        public static decimal Min(decimal value1, decimal value2, decimal value3) => (((value1 < value2) ? value1 : value2) < value3) ? ((value1 < value2) ? value1 : value2) : value3;
        public static decimal Min(decimal value1, decimal value2) => (value1 < value2) ? value1 : value2;
        public static decimal Min(decimal[] values) => values.Min();
        public static byte Sum(byte[] values, byte basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (byte)(basic + values[i]);
            return basic;
        }
        public static sbyte Sum(sbyte[] values, sbyte basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (sbyte)(basic + values[i]);
            return basic;
        }
        public static short Sum(short[] values, short basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (short)(basic + values[i]);
            return basic;
        }
        public static ushort Sum(ushort[] values, ushort basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (ushort)(basic + values[i]);
            return basic;
        }
        public static int Sum(int[] values, int basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (int)(basic + values[i]);
            return basic;
        }
        public static uint Sum(uint[] values, uint basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (uint)(basic + values[i]);
            return basic;
        }
        public static long Sum(long[] values, long basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (long)(basic + values[i]);
            return basic;
        }
        public static ulong Sum(ulong[] values, ulong basic = 0)
        {
            for (int i = 0; i < values.Length; i++) basic = (ulong)(basic + values[i]);
            return basic;
        }
        public static Point Sum(Point[] values, Point basic = default)
        {
            for (int i = 0; i < values.Length; i++) basic = (Point)(basic + values[i]);
            return basic;
        }
        public static Vector2 Sum(Vector2[] values, Vector2 basic = default)
        {
            for (int i = 0; i < values.Length; i++) basic = (Vector2)(basic + values[i]);
            return basic;
        }
    }
}