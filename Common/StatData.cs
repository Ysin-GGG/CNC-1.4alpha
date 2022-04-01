using System;
using System.Collections;

namespace CNC.Common
{
    public class StatData : IEnumerable
    {
        public string name = "";
        public float additive = 0f;
        public float multiplicative = 1f;
        private static StatData instance = null;

        public StatData(float add = 0f, float mult = 1f, string name = "")
        {
            this.name = name;
            this.additive = add;
            this.multiplicative = mult;
            instance = this;
        }

        public int GetValue(int value) => GetValue(this, value); //(int)this.GetValue((float)value);
        public float GetValue(float value) => GetValue(this, value); //(value + value * this.additive) * this.multiplicative;

        public static int GetValue(StatData statData, int value) => (int)StatData.GetValue(statData, (float)value);
        public static float GetValue(StatData statData, float value) => (value + value * statData.additive) * statData.multiplicative;

        private static StatData GetInstance(float add = 0f, float mult = 1f, string name = "")
        {
            if (instance == null) instance = new StatData();
            instance.name = name;
            instance.additive = add;
            instance.multiplicative = mult;
            return instance;
        }

        public static StatData operator +(StatData statData, float add) => StatData.GetInstance(statData.additive + add, statData.multiplicative, statData.name);
        public static StatData operator +(float add, StatData statData) => StatData.GetInstance(statData.additive + add, statData.multiplicative, statData.name);
        public static StatData operator -(StatData statData, float sub) => StatData.GetInstance(statData.additive - sub, statData.multiplicative, statData.name);
        public static StatData operator -(float sub, StatData statData) => StatData.GetInstance(statData.additive - sub, statData.multiplicative, statData.name);
        public static StatData operator *(StatData statData, float mul) => StatData.GetInstance(statData.additive, statData.multiplicative * mul, statData.name);
        public static StatData operator *(float mul, StatData statData) => StatData.GetInstance(statData.additive, statData.multiplicative * mul, statData.name);
        public static StatData operator /(StatData statData, float div) => StatData.GetInstance(statData.additive, div == 0 ? 0 : statData.multiplicative / div, statData.name);
        public static StatData operator /(float div, StatData statData) => StatData.GetInstance(statData.additive, div == 0 ? 0 : statData.multiplicative / div, statData.name);
        public static bool operator ==(StatData statData1, StatData statData2) => statData1.name == statData2.name && statData1.additive == statData2.additive && statData1.multiplicative == statData2.multiplicative;
        public static bool operator !=(StatData statData1, StatData statData2) => !(statData1 == statData2);
        
        public override bool Equals(object obj)
        {
            if (obj is StatData other) return this.name == other.name && this.additive == other.additive && this.multiplicative == other.multiplicative;
            return base.Equals(obj);
        }

        public override string ToString() => "{" + this.name + ": additive = " + this.additive + ", multiplicative = " + this.multiplicative + "}";
        public override int GetHashCode() => new { this.name, this.additive, this.multiplicative }.GetHashCode();
        public IEnumerator GetEnumerator() => new Iterator(this.additive, this.multiplicative, this.name);

        private class Iterator : IEnumerator
        {
            private string name { get; set; }
            private float additive { get; set; }
            private float multiplicative { get; set; }
            private int position = -1;

            public Iterator(float add = 0f, float mult = 1f, string name = "")
            {
                this.name = name;
                this.additive = add;
                this.multiplicative = mult;
            }

            public object Current
            {
                get
                {
                    if (this.name == null) throw new InvalidOperationException("×Ö·û´®²»ÄÜÎª¿Õ(null)");
                    return this;
                }
            }

            public bool MoveNext()
            {
                if (this.position != -1) this.position--;
                return this.position > -1;
            }

            public void Reset() => this.position = -1;
        }
    }
}