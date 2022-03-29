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
            longer = false;
            debuff = false;
            noSave = true;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            Item item = target.HeldItem;
            if (item.type != CNCID.Item.LegacyObject[CNCID.Item.FireGrimoire] || !item.GetGlobalItem<InscriptionSettings>().HasInscription(CNCID.Inscription.S_Enthusiasm))
            { target.ClearBuff(Type); return; }
            if (target.GetModPlayer<AllPlayer>().Tick % 5 == 0)
            {
                if(target.itemAnimation > 0) target.itemAnimation--;
                if(target.itemTime > 0) target.itemTime--;
            }
            target.moveSpeed += .2f;
        }
    }
}
