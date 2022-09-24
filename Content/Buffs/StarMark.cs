using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class StarMark : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            noClearedByNurse = true;
            longer = false;
            debuff = true;
            noSave = true;
            pvp = false;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            float spd = .3f;
            float lim = spd * 10f;
            target.dashTime = 0;
            target.moveSpeed *= spd;
            target.maxRunSpeed *= spd;
            target.accRunSpeed *= spd;
            target.wingAccRunSpeed *= spd;
            target.jumpSpeedBoost = spd - 3f;
            if (target.velocity.X < -lim) target.velocity.X++;
            if (target.velocity.X > lim) target.velocity.X--;
            if (target.velocity.Y < -lim) target.velocity.Y++;
            {
                Vector2 center = target.Center;
                float max = 3f;
                int dustid1 = DustId.CyanShortFx1;
                for (int i = 0; i < max; i++)
                {
                    float amount = (i / max * MyMath.pi2 + gt.Tick * MyMath.deg01 * 6 + MyMath.pi2) % MyMath.pi2;
                    Vector2 vel = new Vector2(6f * MyMath.cos(amount), 2f * MyMath.sin(amount));
                    CNCUtils.NewDust(center + vel * 5f, dustid1, MyMath.sin(amount) + 1).noGravity = true;
                }
            }
        }
    }
}