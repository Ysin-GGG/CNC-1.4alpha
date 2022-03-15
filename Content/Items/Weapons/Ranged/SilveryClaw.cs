using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Ranged
{
	public class SilveryClaw : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 20;
            item.height = 68;
            item.useTime = 54;
            item.useAnimation = 54;
            item.rare = 2;
            item.damage = 47;
            item.crit = 4;
            item.knockBack = 2f;
            item.useStyle = 5;
            item.value = 50000;
            item.noMelee = true;
            item.useAmmo = AmmoID.Arrow;
            item.DamageType = DamageClass.Ranged;
            clazz = 21;
        }
        /*
        public override bool Projectile(Player owner, ref Vector2 pos, ref Vector2 vel, ref int id, ref int dmg, ref float kb)
        {
            int proj1 = CNC.GetType(1, 11);
            int proj2 = CNC.GetType(1, 12);
            g = item.GetGlobalItem<GItem>();
            if (id == 1) { id = proj1; }
            if (g.level > 1 && Main.rand.Next(20) < 9) { id = proj2; dmg *= 2; }
            if (id == proj2) { CNC.PlayMySnd("SilveryClaw/Shoot2", pos); }
            else { CNC.PlayMySnd("SilveryClaw/Shoot1", pos, 2f); }
            if (id == proj2) { Method.NewProj(pos + vel / 4f, vel, CNC.GetType(1, 14), 0, 0); }
            if (g.level > 2 && id == proj2)
            {
                Method.NewProj(pos, vel, proj2, dmg, kb, item.owner, 0, 1);
                id = CNC.GetType(1, 0);
            }
            Projectile p = Method.NewProj(pos, vel, id, dmg, kb, item.owner);
            id = CNC.GetType(1, 0);
            return base.Projectile(owner, ref pos, ref vel, ref id, ref dmg, ref kb);
        }
        */
        public override Vector2? HoldoutOffset() => Vector2.Zero;
    }
}