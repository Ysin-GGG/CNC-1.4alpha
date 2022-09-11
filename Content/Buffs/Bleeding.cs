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
    public class Bleeding : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            noClearedByNurse = true;
            longer = false;
            debuff = true;
            noSave = true;
            noTimeDisplay = true;
            pvp = false;
            //killText = CNC.ReadJson("DeathText.Sanctified");
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            foreach (int dmg in gt.bleeding) if (dmg > 0) { target.buffTime[index] = 2; break; }
            if (gt.Tick % 60 == 0)
            {
                List<int> bleedingDamage = new List<int>();
                for (int i = 0; i < gt.bleeding.Length; i++)
                {
                    if (gt.bleeding[i] > 0) bleedingDamage.Add(gt.bleeding[i]);
                    gt.bleeding[i] = 0;
                }
                int num = CNC.Random.NextInt(bleedingDamage.Count);
                if (bleedingDamage[num] > 0) CNCUtils.TrueDamage(target, bleedingDamage[num], 30000, true, Color.Red);
                bleedingDamage[num] = 0;
                for (int i = 0; i < bleedingDamage.Count; i++) gt.bleeding[i] = bleedingDamage[i];
            }
        }

        public override void Update(NPC target, AllNPC gt, ref int index)
        {
            foreach (int dmg in gt.bleeding) if (dmg > 0) { target.buffTime[index] = 2; break; }
            if (gt.Tick % 60 == 0)
            {
                List<int> bleedingDamage = new List<int>();
                for (int i = 0; i < gt.bleeding.Length; i++)
                {
                    if (gt.bleeding[i] > 0) bleedingDamage.Add(gt.bleeding[i]);
                    gt.bleeding[i] = 0;
                }
                int num = CNC.Random.NextInt(bleedingDamage.Count);
                if(bleedingDamage[num] > 0) CNCUtils.TrueDamage(target, bleedingDamage[num], 30000, true, Color.Red);
                bleedingDamage[num] = 0;
                for (int i = 0; i < bleedingDamage.Count; i++) gt.bleeding[i] = bleedingDamage[i];
            }
        }
    }
}