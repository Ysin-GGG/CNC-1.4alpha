using Terraria;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class Glacier : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            longer = false;
            debuff = true;
            noClearedByNurse = true;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            gt.buff[CNCID.Buff.Glacier] = true;
            target.jump = 0;
            target.moveSpeed = 0;
            target.velocity.X = 0;
            target.headVelocity.X = 0;
            target.headVelocity.Y = 0;
            target.headFrameCounter = 0;
            target.bodyVelocity.X = 0;
            target.bodyVelocity.Y = 0;
            target.bodyFrameCounter = 0;
            target.legVelocity.X = 0;
            target.legVelocity.Y = 0;
            target.legFrameCounter = 0;
            target.wingFrameCounter = 0;
            target.controlUp = false;
            target.controlDown = false;
            target.controlLeft = false;
            target.controlRight = false;
            target.controlJump = false;
            target.controlQuickHeal = false;
            target.controlQuickMana = false;
            target.controlThrow = false;
            target.controlTorch = false;
            target.controlSmart = false;
            target.controlInv = false;
            target.controlHook = false;
            target.controlMount = false;
            target.controlUseItem = false;
            target.controlUseTile = false;
            target.position.X = target.oldPosition.X;
            target.velocity.Y += .4f;
        }

        public override void Update(NPC target, AllNPC gt, ref int index)
        {
            if (!target.boss)
            {
                target.velocity.X = 0;
                target.noGravity = false;
                target.color = new Microsoft.Xna.Framework.Color(96, 129, 192);
                //target.aiStyle = -1;
            }
        }
    }
}
