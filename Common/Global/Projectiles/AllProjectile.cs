using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Global.NPCs;
using CNC.Common.Utils;

namespace CNC.Common.Global.Projectiles
{
    public class AllProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        //public override bool CloneNewInstances => true;

        public Dictionary<string, object> data = new Dictionary<string, object>();//mod弹幕临时数据
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

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            damage = (int)((double)damage * .5 + (double)damage * (double)MyMath.Max(.5 * (1 - Math.Tanh((double)damage / (double)MyMath.Max(500.0, (double)target.statLifeMax))), .1));
            if (crit) damage = damage * 3 / 2;
        }

        public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            //pvp伤害修正
            damage = (int)((double)damage * .5 + (double)damage * (double)MyMath.Max(.5 * (1 - Math.Tanh((double)damage / (double)MyMath.Max(500.0, (double)target.statLifeMax))), .1));
            if (crit) damage = damage * 3 / 2;
        }
    }
}