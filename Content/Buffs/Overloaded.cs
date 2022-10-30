using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class Overloaded : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            longer = false;
            debuff = false;
            noSave = false;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            target.GetDamage(DamageClass.Magic) += (float)CNCID.Inscription.S_ArcaneOverload.Data[0];
        }
    }
}
