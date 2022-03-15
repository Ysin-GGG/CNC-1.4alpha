
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Magic
{
    public class IceTooth : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 48;
            item.height = 48;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 3;
            item.value = 20000;
            item.damage = 18;
            item.crit = 5;
            item.knockBack = 5f;
            item.useStyle = 5;
            item.mana = 10;
            item.noMelee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item20;
            item.DamageType = DamageClass.Magic;
            clazz = 32;
            staff = true;
        }
        /*
        public override bool Projectile(Player owner, ref Vector2 pos, ref Vector2 vel, ref int id, ref int dmg, ref float kb)
        {
            id = 0;
            GItem g = item.GetGlobalItem<GItem>();
            Main.PlaySound(SoundLoader.customSoundType, -1, -1, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/IcyTooth/Shoot"), 1f);
            Method.NewProj(pos, vel, proj, dmg, kb, -1, g.level);
            return base.Projectile(owner, ref pos, ref vel, ref id, ref dmg, ref kb);
        }
        */
    }
}