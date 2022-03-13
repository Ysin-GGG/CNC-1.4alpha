
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
    public class Lvset : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "Lvset";

        public override string Usage
            => "/lvset " + CNCLocalization.GetText("Command.LvSet.Value");

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Item i = caller.Player.HeldItem;
            AllItem ai = i.GetGlobalItem<AllItem>();
            int level = 0;
            if (!ai.levelExist)
            { throw new UsageException(CNCLocalization.GetText("Command.LvSet.NoLevel", i.Name)); }
            if (!int.TryParse(args[0], out level))
            { throw new UsageException(CNCLocalization.GetText("Command.LvSet.NotPositiveInt", args[0])); }
            else
            { level = int.Parse(args[0]); }
            if (level < 1)
            { throw new UsageException(CNCLocalization.GetText("Command.LvSet.NotPositiveInt", level)); }
            else if (level > ai.levelMax)
            { throw new UsageException(CNCLocalization.GetText("Command.LvSet.OutOfMaxLevel", level, arg1: ai.levelMax)); }
            else
            {
                Main.NewText(CNCLocalization.GetText("Command.LvSet.Success", ai.level, arg1: level), Colors.RarityGreen);
                ai.level = level;
            }
        }
    }

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
            SoundEngine.PlaySound(int.Parse(args[0]), caller.Player.position, int.Parse(args[1]));
        }
    }

    public class spawn : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "spawn";

        public override string Usage
            => "/spawn Type";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), (int)caller.Player.position.X, (int)caller.Player.position.Y, Common.Utils.CNCID.NPC.LegacyObject[short.Parse(args[0])]);
        }
    }
}