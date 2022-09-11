using Terraria;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class Blessed : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            longer = false;
            debuff = false;
            noSave = false;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            gt.takenDamage -= (float)CNCID.Inscription.S_SilveryVow.Data[2];
        }
    }
}
