using Terraria;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class Scorched : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            longer = false;
            debuff = true;
            noSave = false;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            target.statDefense -= (int)CNCID.Inscription.S_Scorching.Args[0];
        }

        public override void Update(NPC target, AllNPC gt, ref int index)
        {
            target.defense -= (int)CNCID.Inscription.S_Scorching.Args[0];
        }
    }
}
