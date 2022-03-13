using Terraria;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.Items;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class Enthused : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            cid = CNCID.Buff.Enthused;
            longer = false;
            debuff = false;
            noSave = true;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            Item item = target.HeldItem;
            if (item.type != CNCID.Item.LegacyObject[CNCID.Item.FireGrimoire] || item.GetGlobalItem<AllItem>().level < 3)
            { target.ClearBuff(Type); gt.buff[cid] = false; return; }
            target.itemAnimation -= target.itemAnimationMax / 20;
            target.itemTime -= target.itemTimeMax / 20;
            target.moveSpeed *= 1.1f;
        }
    }
}
