using Terraria;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.NPCs;
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
    }
}
