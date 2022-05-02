
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.DataStructures;
using CNC.Common.Localization;
using CNC.Common.Global.Items;

namespace CNC.Content.Commands
{
    public class Spawn : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "Spawn";

        public override string Usage
            => "/spawn Type";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)caller.Player.position.X, (int)caller.Player.position.Y, Common.Utils.CNCID.NPC.LegacyObject[short.Parse(args[0])]);
        }
    }
}