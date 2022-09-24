using System;
using System.Timers;
using CNC.Common.Utils;

namespace CNC.Content.DataStructures
{
    public class EffectTimer
    {
        private int type;
        private int delay;
        private int min;
        private int max;
        private int addDelay;
        private int delDelay;
        private int count;
        private Timer timer;
        public int value { get; private set; }
        public double updateInterval
        {
            get => this.timer.Interval;
            set
            {
                if (value > 0 && !this.timer.Enabled) this.timer.Start();
                if (value <= 0 && this.timer.Enabled) this.timer.Stop();
                timer.Interval = MyMath.Max(value, 1);
            }
        }

        /// <summary>  </summary>
        /// <param name="value">初始值</param>
        /// <param name="type">值变化类型（正/负）</param>
        /// <param name="delay">值初始变化延迟</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="addDelay">切换到正变化时的延迟</param>
        /// <param name="delDelay">切换到负变化时的延迟</param>
        /// <param name="count">值变化的大小</param>
        /// <param name="updateInterval">自动刷新的间隔（仅为正数时生效）</param>
        public EffectTimer(int value, int type, int delay, int min, int max, int addDelay, int delDelay, int count, double updateInterval = 1)
        {
            this.value = value;
            this.type = type;
            this.delay = delay;
            this.min = min;
            this.max = max;
            this.addDelay = addDelay;
            this.delDelay = delDelay;
            this.count = count;
            this.timer = new Timer();
            this.updateInterval = updateInterval;
            if (updateInterval > 0)
            {
                timer.AutoReset = true;
                timer.Start();
            }
            timer.Elapsed += this.Update;
        }

        private void Update(object o, ElapsedEventArgs e) => this.Update();
        public void Update()
        {
            if (this.delay-- <= 0)
            {
                if (this.type < 0 && this.value <= this.min)
                {
                    this.type = 1;
                    this.delay = this.addDelay;
                }
                if (this.type > 0 && this.value >= this.max)
                {
                    this.type = -1;
                    this.delay = this.delDelay;
                }
            }
            if (this.delay <= 0) this.value = MyMath.Clamp( this.value + this.type * this.count, this.min, this.max);
        }

        public int GetNextValue()
        {
            int result = this.value;
            this.Update();
            return result;
        }
    }
}