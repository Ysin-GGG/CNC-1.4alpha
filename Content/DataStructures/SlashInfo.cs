using Microsoft.Xna.Framework;
using Terraria;

namespace CNC.Content.DataStructures
{
    public struct SlashInfo
    {
        /// <summary> 
        /// 坐标偏移
        /// <para> 坐标系以TR为准 </para>
        /// </summary>
        public Vector2 offset;
        /// <summary> 攻击距离  </summary>
        public float range;
        /// <summary> 攻击角度  </summary>
        public float rotation;
        /// <summary> 
        /// 插值（Lerp）变化的幂 
        /// <para> 取1时，为线性插值 </para>
        /// </summary>
        public float power;

        public Vector2 position
        {
            get => new Vector2(this.range, 0f).RotatedBy(this.rotation);
            set { this.range = value.Length(); this.rotation = value.ToRotation(); }
        }

        public SlashInfo(Vector2 offset, Vector2 pos, float power = 1f)
        {
            this.offset = offset;
            this.rotation = pos.ToRotation();
            this.range = pos.Length();
            this.power = power;
        }

        public SlashInfo(Vector2 offset, float range, float rotation, float power = 1f)
        {
            this.offset = offset;
            this.range = range;
            this.rotation = rotation;
            this.power = power;
        }

    }
}