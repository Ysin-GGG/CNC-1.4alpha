using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.DataStructures
{
    /// <summary> 常时变化数据 </summary>
    public struct VariagleStatus
    {
        /// <summary> 值 </summary>
        public float value;
        /// <summary> 递增值（受mult2, mult3两个递乘值影响） </summary>
        public float additive1;
        /// <summary> 递增值（不受递乘值影响） </summary>
        public float additive2;
        /// <summary> 递乘值（影响value） </summary>
        public float multiplicative1;
        /// <summary> 递乘值（影响add1） </summary>
        public float multiplicative2;
        /// <summary> 递乘值（影响value和add1） </summary>
        public float multiplicative3;

        /// <summary> 常时变化数据 </summary>
        /// <param name="value">初始值</param>
        /// <param name="add1">递增值（受mult2, mult3两个递乘值影响）</param>
        /// <param name="add2">递增值（不受递乘值影响）</param>
        /// <param name="mult1">递乘值（影响value）</param>
        /// <param name="mult2">递乘值（影响add1）</param>
        /// <param name="mult3">递乘值（影响value和add1）</param>
        public VariagleStatus(float value, float add1, float add2, float mult1, float mult2, float mult3)
        {
            this.value = value;
            this.additive1 = add1;
            this.additive2 = add2;
            this.multiplicative1 = mult1;
            this.multiplicative2 = mult2;
            this.multiplicative3 = mult3;
        }

        /// <summary> 获取下一个值 </summary>
        /// <para> 下一个值的递推公式：nextValue = (value * mult1 + add1 * mult2) * mult3 + add2 </para>
        public float GetNextValue()
        {
            float result = value;
            this.value = (this.value * this.multiplicative1 + this.additive1 * this.multiplicative2) * this.multiplicative3 + this.additive2;
            return result;
        }
    }
}