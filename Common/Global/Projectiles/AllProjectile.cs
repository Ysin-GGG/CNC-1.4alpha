using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Global.NPCs;

namespace CNC.Common.Global.Projectiles
{
    public class AllProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        public Dictionary<string, object> data = new Dictionary<string, object>();//modµ¯Ä»ÁÙÊ±Êý¾Ý

        public override void SetDefaults(Projectile projectile)
        {
            data = new Dictionary<string, object>();
        }
    }
}