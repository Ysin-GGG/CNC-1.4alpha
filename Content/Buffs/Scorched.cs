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
            cid = CNCID.Buff.Scorched;
            longer = false;
            debuff = true;
            noSave = false;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            target.statDefense -= 8;
        }

        public override void Update(NPC target, AllNPC gt, ref int index)
        {
            target.defense -= 8;
        }
    }
}
