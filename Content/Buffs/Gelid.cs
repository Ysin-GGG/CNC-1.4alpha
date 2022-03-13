using Terraria;
using CNC.Common.Utils;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class Gelid : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            cid = CNCID.Buff.Gelid;
            longer = false;
            debuff = true;
            noSave = false;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            target.moveSpeed *= .5f;
        }

        public override void Update(NPC target, AllNPC gt, ref int index)
        {
            target.position.X -= (target.position.X - target.oldPosition.X) / 2;
        }
    }
}
