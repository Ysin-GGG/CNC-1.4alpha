using System;
using Microsoft.Xna.Framework;

namespace CNC.Content.DataStructures
{
    public class ScreenShake
    {
        /// <summary> 震动时长 </summary>
        public int time;
        /// <summary> 震动数值 </summary>
        public Vector2 value;
        /// <summary> 震动方向 </summary>
        public Vector2 direction;
        /// <summary> 震动频率（屏幕上下震动一次的周期） </summary>
        public Vector2 frequency;
        /// <summary> 震动幅度 </summary>
        public Vector2 amplitude;
        /// <summary> 震动幅度等差变化 </summary>
        public Vector2 amplitudeAdd;
        /// <summary> 震动幅度等比变化 </summary>
        public Vector2 amplitudeMult;
        
        public ScreenShake(int time, float frequency, float amplitude, float amplitudeAdd, float amplitudeMult, bool vertical = true)
        {
            Vector2 normal = new Vector2(vertical ? 0f : 1f, vertical ? 1f : 0f);
            this.time = time;
            this.direction = normal * Math.Sign(amplitude);
            this.frequency = normal * frequency;
            this.amplitude = normal * amplitude;
            this.amplitudeAdd = normal * amplitudeAdd;
            this.amplitudeMult = normal * amplitudeMult;
        }

        public void Update()
        {
            if (this.time-- > 0)
            {
                float speed;
            }
        }
    }
}