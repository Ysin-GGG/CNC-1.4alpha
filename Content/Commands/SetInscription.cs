
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
    public class SetInscription : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "inscription";

        public override string Usage
            => "/inscription [name]";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Item i = caller.Player.HeldItem;
            InscriptionSettings ai = i.GetGlobalItem<InscriptionSettings>();
            int index = -1;
            if (!ai.CanUpgrade)
            { throw new UsageException(CNCLocalization.GetText("Command.LvSet.NoLevel", i.Name)); }/*
            if (!int.TryParse(args[0], out index))
            { throw new UsageException(CNCLocalization.GetText("Command.LvSet.NotPositiveInt", args[0])); }
            else { index = int.Parse(args[0]); }*/
            ai.SetInscription(index, args[0]);
        }
    }
}