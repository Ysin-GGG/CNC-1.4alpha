
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
    public class Sound : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "Sound";

        public override string Usage
            => "/sound Type Style";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            //SoundEngine.PlaySound(int.Parse(args[0]), caller.Player.position, int.Parse(args[1]));
        }
    }
}