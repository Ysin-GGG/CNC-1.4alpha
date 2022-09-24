using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Global.NPCs;

namespace CNC.Common.Global.Projectiles
{
    public class AllProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        //public override bool CloneNewInstances => true;

        public Dictionary<string, object> data = new Dictionary<string, object>();//modµ¯Ä»ÁÙÊ±Êý¾Ý
        public float damageAdd { get => (float)this.data["damageAdd"]; set => this.data["damageAdd"] = value; }
        public float damageMult { get => (float)this.data["damageMul"]; set => this.data["damageMul"] = value; }
        
        public int[] intData = new int[CNC.maxProjectileData];
        public bool[] boolData = new bool[CNC.maxProjectileData];
        public float[] floatData = new float[CNC.maxProjectileData];
        public double[] doubleData = new double[CNC.maxProjectileData];
        public string[] stringData = new string[CNC.maxProjectileData];

        public override void SetDefaults(Projectile projectile)
        {
            this.data = new Dictionary<string, object>();
            this.damageAdd = 0f;
            this.damageMult = 1f;
        }
    }
}