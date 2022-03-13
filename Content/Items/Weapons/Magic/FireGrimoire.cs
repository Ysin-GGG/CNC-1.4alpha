
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CNC.Common.Localization;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Magic
{
    public class FireGrimoire : CNCItem
    {
        public override string[] attributeKeys => new string[] {
            "Fireball",
            "Incinerating",
            "Scorching",
            "Enthusiasm",
        };

        public override void ResetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 3;
            item.value = 10000;
            item.damage = 36;
            item.crit = 20;
            item.knockBack = 2f;
            item.useStyle = 5;
            item.mana = 8;
            item.noMelee = true;
            item.autoReuse = true;
            item.DamageType = DamageClass.Magic;
            item.UseSound = SoundID.DD2_BetsyFireballShot;
            level = 1;
            levelMax = 3;
            clazz = 32;
        }

        public override void Tooltips(List<TooltipLine> t)
        {
            string name = item.Name;
            t.Add(new TooltipLine(mod, "", GetAttribute(attributeKeys[0])) { overrideColor = level > 0 ? u : n });
            t.Add(new TooltipLine(mod, "", GetAttribute(attributeKeys[1], item, 1)) { overrideColor = level > 0 ? u : n });
            t.Add(new TooltipLine(mod, "", GetAttribute(attributeKeys[2])) { overrideColor = level > 1 ? u : n });
            t.Add(new TooltipLine(mod, "", GetAttribute(attributeKeys[3])) { overrideColor = level > 2 ? u : n });
        }
        /*
        public override bool Projectile(Player owner, ref Vector2 pos, ref Vector2 vel, ref int id, ref int dmg, ref float kb)
        {
            id = 0;
            int lv = item.GetGlobalItem<GItem>().level;
            int proj = CNC.GetType(1, 6);
            if (Main.rand.Next(49) < 1 && lv > 2) { owner.AddBuff(CNC.GetType(2, 4), 300); }
            Method.NewProj(pos, vel, proj, dmg, kb, -1, lv > 1 ? lv : 0f);
            return base.Projectile(owner, ref pos, ref vel, ref id, ref dmg, ref kb);
        }
        */

        public override Vector2? HoldoutOffset() => Vector2.One;
    }
}