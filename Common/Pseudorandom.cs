using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace CNC.Common
{
    /// <summary> 伪随机数生成器（线形同余法） </summary>
    public class Pseudorandom
    {
        /// <summary> 初始种子 </summary>
        public long OriginalSeed => this.originalSeed;
        /// <summary> 当前种子 </summary>
        public long Seed => this.seed;
        private long originalSeed;
        private long seed;
        private MyRandom rand;
        private Dictionary<string, MyRandom> rands;

        /// <summary> 伪随机数生成器（线形同余法） </summary>
        public Pseudorandom()
        {
            this.rands = new Dictionary<string, MyRandom>();
            this.originalSeed = this.seed = System.DateTime.Now.Ticks;
            this.rand = new MyRandom((int)(this.seed % ((long)int.MaxValue + (long)1)));
            this.rands.Add(this.seed.ToString(), rand);
        }

        /// <summary> 伪随机数生成器（线形同余法） </summary>
        /// <param name="seed"> 初始种子 </param>
        public Pseudorandom(long seed)
        {
            this.seed = seed;
            this.rand = new MyRandom((int)(this.seed % ((long)int.MaxValue + (long)1)));
        }

        /// <summary> 重置种子为初始种子 </summary>
        /// <param name="reset"> 重置上次使用这个种子的随机数生成器 </param>
        public void SetSeed(bool reset = false) => this.SetSeed(this.originalSeed, reset);

        /// <summary> 设置新种子 </summary>
        /// <param name="newSeed"> 种子 </param>
        /// <param name="reset"> 重置上次使用这个种子的随机数生成器 </param>
        public void SetSeed(long newSeed, bool reset = false)
        {
            this.seed = newSeed;
            string key = this.seed.ToString();
            if (!this.rands.ContainsKey(key))
            {
                this.rand = new MyRandom((int)(this.seed % ((long)int.MaxValue + (long)1)));
                this.rands.Add(key, this.rand);
            }
            else if (this.rands.ContainsKey(key) && reset) this.rands[key] = this.rand = new MyRandom((int)(this.seed % ((long)int.MaxValue + (long)1)));
            else this.rand = this.rands[key];
        }

        /// <summary> 
        /// 随机取整数
        /// <para> 当max≥0时：从[0, max)中取一个整数 </para>
        /// <para> 当max＜0时：从[max, 0)中取一个整数 </para>
        /// </summary>
        /// <param name="max"> 最大整数 </param>
        public int NextInt(int max = int.MaxValue) => max < 0 ? this.NextInt(max, 0) : this.NextInt(0, max);

        /// <summary> 
        /// 随机取整数
        /// <para> 从[min, max)中取一个整数 </para>
        /// </summary>
        /// <param name="max"> 最小整数 </param>
        /// <param name="max"> 最大整数 </param>
        public int NextInt(int min, int max)
        {
            if (max < min) throw new ArgumentOutOfRangeException($"最大值不得小于最小值：{max} < {min}");
            if (min < 0)
            {
                min *= -1;
                int positive = this.rand.Next(0, max);
                int negative = this.rand.Next(1, min + 1);
                int random = (int)((double)min / (min + max) * 100);
                return this.NextBool(random) ? -negative : positive;
            }
            else return this.rand.Next(0, max);
        }

        /// <summary> 
        /// 随机取真假
        /// <para> max * 100% 的几率取true </para>
        /// </summary>
        /// <param name="max"> 整型概率 </param>
        public bool NextBool(long max = 50) => this.NextBool(0, max);

        /// <summary> 
        /// 随机取真假
        /// <para> (max - min) * 100% 的几率取true </para>
        /// </summary>
        /// <param name="min"> 整型最小概率 </param>
        /// <param name="max"> 整型最大概率 </param>
        public bool NextBool(long min, long max)
        {
            double num = this.rand.NextDouble();
            if (max > 100) max = 100;
            if (max < 0) max = 0;
            if (min > 100) min = 100;
            if (min < 0) min = 0;
            return min / 100f < num && num < max / 100f;
        }

        /// <summary> 
        /// 随机取真假
        /// <para> max * 100% 的几率取true </para>
        /// </summary>
        /// <param name="max"> 双精度概率 </param>
        public bool NextBool(double max) => this.NextBool(0, max);

        /// <summary> 
        /// 随机取真假
        /// <para> (max - min) * 100% 的几率取true </para>
        /// </summary>
        /// <param name="min"> 双精度最小概率 </param>
        /// <param name="max"> 双精度最大概率 </param>
        public bool NextBool(double min, double max)
        {
            double num = this.rand.NextDouble();
            if (max > 1) max = 1;
            if (max < 0) max = 0;
            if (min > 1) min = 1;
            if (min < 0) min = 0;
            return min < num && num < max;
        }

        /// <summary> 
        /// 随机取单精度浮点数
        /// <para> 当max≥0时：从[0, max)中取一个单精度浮点数 </para>
        /// <para> 当max＜0时：从[max, 0)中取一个单精度浮点数 </para>
        /// </summary>
        public float NextFloat(float max = float.MaxValue) => max < 0 ? this.NextFloat(max, 0) : this.NextFloat(0, max);

        /// <summary> 
        /// 随机取单精度浮点数
        /// <para> 从[min, max)中取一个单精度浮点数 </para>
        /// </summary>
        /// <param name="min"> 最小单精度浮点数 </param>
        /// <param name="max"> 最大单精度浮点数 </param>
        public float NextFloat(float min, float max)
        {
            if (max < min) throw new ArgumentOutOfRangeException($"最大值不得小于最小值：{max} < {min}");
            if (min < 0)
            {
                min *= -1;
                float positive = (float)(this.rand.NextDouble() * max);
                float negative = (float)(this.rand.NextDouble() * min);
                return NextBool(min / (min + max)) ? -negative : positive;
            }
            else return (float)(this.rand.NextDouble() * max);
        }

        /// <summary> 
        /// 随机取双精度浮点数
        /// <para> 当max≥0时：从[0, max)中取一个双精度浮点数 </para>
        /// <para> 当max＜0时：从[max, 0)中取一个双精度浮点数 </para>
        /// </summary>
        public double NextDouble(double max = double.MaxValue) => max < 0 ? this.NextDouble(max, 0) : this.NextDouble(0, max);

        /// <summary> 
        /// 随机取双精度浮点数
        /// <para> 从[min, max)中取一个双精度浮点数 </para>
        /// </summary>
        /// <param name="min"> 最小双精度浮点数 </param>
        /// <param name="max"> 最大双精度浮点数 </param>
        public double NextDouble(double min, double max)
        {
            if (max < min) throw new ArgumentOutOfRangeException($"最大值不得小于最小值：{max} < {min}");
            if (min < 0)
            {
                min *= -1;
                double positive = this.rand.NextDouble() * max;
                double negative = this.rand.NextDouble() * min;
                return NextBool(min / (min + max)) ? -negative : positive;
            }
            else return this.rand.NextDouble() * max;
        }

        //待完善
        public Vector2 NextVector2(float max) => this.NextVector2(new Vector2(max));
        public Vector2 NextVector2(float maxX, float maxY) => this.NextVector2(new Vector2(maxX, maxY));
        public Vector2 NextVector2(Vector2 max) => new Vector2(this.NextFloat(max.X), this.NextFloat(max.Y));
        public Vector2 NextVector2(float minX, float minY, float maxX, float maxY) => this.NextVector2(new Vector2(minX, minY), new Vector2(maxX, maxY));
        public Vector2 NextVector2(Vector2 min, Vector2 max) => new Vector2(this.NextFloat(min.X, max.X), this.NextFloat(min.Y, max.Y));
        /*
        public Point NextPoint(int max) { }
        public Point NextPoint(int maxX, int maxY) { }
        public Point NextPoint(Point max) { }
        public Point NextPoint(int minX, int minY, int maxX, int maxY) { }
        public Point NextPoint(Point min, Point max) { }

        public short NextShort(short max) { }
        public short NextShort(short min, short max) { }
        public ushort NextUnsignedShort(ushort max) { }
        public ushort NextUnsignedShort(ushort min, ushort max) { }

        public byte NextByte(byte max) { }
        public byte NextByte(byte min, byte max) { }
        public sbyte NextSignedByte(sbyte max) { }
        public sbyte NextSignedByte(sbyte min, sbyte max) { }
        
        public uint NextUnsignedInt(uint max) { }
        public uint NextUnsignedInt(uint min, uint max) { }

        public long NextLong(long max) { }
        public long NextLong(long min, long max) { }
        public ulong NextUnsignedLong(ulong max) { }
        public ulong NextUnsignedLong(ulong min, ulong max) { }
        */
    }
    /// <summary> C# System.Random类的源码（稍有修改） </summary>
    [Serializable()] internal class MyRandom
    {
        //
        // Private Constants 
        //
        private const int MBIG = Int32.MaxValue;
        private const int MSEED = 161803398;
        private const int MZ = 0;


        //
        // Member Variables
        //
        private int inext, inextp;
        private int[] SeedArray = new int[56];

        /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.Random"]/*' />
        public MyRandom()
          : this(Environment.TickCount) {
        }

        /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.Random1"]/*' />
        public MyRandom(int Seed) {
            int ii;
            int mj, mk;

            //Initialize our Seed array.
            //This algorithm comes from Numerical Recipes in C (2nd Ed.)
            mj = MSEED - Math.Abs(Seed);
            SeedArray[55] = mj;
            mk = 1;
            for (int i = 1; i < 55; i++) {  //Apparently the range [1..55] is special (Knuth) and 
                                            //so we're wasting the 0'th position.
                ii = (21 * i) % 55;
                SeedArray[ii] = mk;
                mk = mj - mk;
                if (mk < 0) mk += MBIG;
                mj = SeedArray[ii];
            }
            for (int k = 1; k < 5; k++) {
                for (int i = 1; i < 56; i++) {
                    SeedArray[i] -= SeedArray[1 + (i + 30) % 55];
                    if (SeedArray[i] < 0) SeedArray[i] += MBIG;
                }
            }
            inext = 0;
            inextp = 21;
            Seed = 1;
        }

        //
        // Package Private Methods
        //

        /**//*====================================Sample====================================
        **Action: Return a new random number [0..1) and reSeed the Seed array.
        **Returns: A double [0..1)
        **Arguments: None
        **Exceptions: None
        ==============================================================================*/
            /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.Sample"]/*' />
        protected virtual double Sample() {
            int retVal;
            int locINext = inext;
            int locINextp = inextp;

            if (++locINext >= 56) locINext = 1;
            if (++locINextp >= 56) locINextp = 1;

            retVal = SeedArray[locINext] - SeedArray[locINextp];

            if (retVal < 0) retVal += MBIG;

            SeedArray[locINext] = retVal;

            inext = locINext;
            inextp = locINextp;

            //Including this division at the end gives us significantly improved
            //random number distribution.
            return (retVal * (1.0 / MBIG));
        }

        //
        // Public Instance Methods
        // 


        /**//*=====================================Next=====================================
        **Returns: An int [0.._int4.MaxValue)
        **Arguments: None
        **Exceptions: None.
        ==============================================================================*/
            /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.Next"]/*' />
        public virtual int Next() {
            return (int)(Sample() * Int32.MaxValue);
        }

        /**//*=====================================Next=====================================
        **Returns: An int [minvalue..maxvalue)
        **Arguments: minValue -- the least legal value for the Random number.
        **           maxValue -- the greatest legal return value.
        **Exceptions: None.
        ==============================================================================*/
            /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.Next1"]/*' />
        public virtual int Next(int minValue, int maxValue) {
            if (minValue > maxValue) {
                throw new ArgumentOutOfRangeException($"最大值不得小于最小值：{maxValue} < {minValue}");
                //throw new ArgumentOutOfRangeException("minValue", String.Format(Environment.GetResourceString("Argument_MinMaxValue"), "minValue", "maxValue"));
            }

            int range = (maxValue - minValue);

            //This is the case where we flipped around (e.g. MaxValue-MinValue);
            if (range < 0) {
                long longRange = (long)maxValue - (long)minValue;
                return (int)(((long)(Sample() * ((double)longRange))) + minValue);
            }

            return ((int)(Sample() * (range))) + minValue;
        }


        /**//*=====================================Next=====================================
        **Returns: An int [0..maxValue)
        **Arguments: maxValue -- the greatest legal return value.
        **Exceptions: None.
        ==============================================================================*/
            /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.Next2"]/*' />
        public virtual int Next(int maxValue) {
            if (maxValue < 0) {
                throw new ArgumentOutOfRangeException($"最大值不得小于最小值：{maxValue} < 0");
                //throw new ArgumentOutOfRangeException("maxValue", String.Format(Environment.GetResourceString("ArgumentOutOfRange_MustBePositive"), "maxValue"));
            }
            return (int)(Sample() * maxValue);
        }


        /**//*=====================================Next=====================================
        **Returns: A double [0..1)
        **Arguments: None
        **Exceptions: None
        ==============================================================================*/
            /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.NextDouble"]/*' />
        public virtual double NextDouble() {
            return Sample();
        }


        /**//*==================================NextBytes===================================
        **Action:  Fills the byte array with random bytes [0..0x7f].  The entire array is filled.
        **Returns:Void
        **Arugments:  buffer -- the array to be filled.
        **Exceptions: None
        ==============================================================================*/
            /**//// <include file='doc\Random.uex' path='docs/doc[@for="Random.NextBytes"]/*' />
        public virtual void NextBytes(byte[] buffer) {
            if (buffer == null) throw new ArgumentNullException("buffer");
            for (int i = 0; i < buffer.Length; i++) {
                buffer[i] = (byte)(Sample() * (Byte.MaxValue + 1));
            }
        }
    }
}