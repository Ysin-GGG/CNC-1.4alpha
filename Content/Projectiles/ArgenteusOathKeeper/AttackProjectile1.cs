using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.ArgenteusOathKeeper
{
    public abstract class AttackProjectile1 : CNCProjectile
    {
        public int level => data.ContainsKey("level") ? (int)data["level"] : 0;
        public override int countFrame => 2;
        private const int tpf = 5;//Time Per Frame

        public override void ResetDefaults()
        {
            data["level"] = 0;
            projectile.width = 38;
            projectile.height = 208;
            projectile.timeLeft = 15;
            projectile.knockBack = 2f;
            projectile.alpha = 32;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.aiStyle = 1;
            projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
            Anim001_Loop(projectile, tpf);
            Color color = new Color(225, 242, 249); //projectile.scale;
            CNCUtils.AddLight(projectile.position, color, 1.2f);
        }
    }
}
