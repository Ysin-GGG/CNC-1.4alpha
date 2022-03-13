using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Players;

namespace CNC.Common.Global.Buffs
{
    public abstract class MountBuff : ModBuff
    {
        public virtual string mountName => Name;
        public virtual int mount => CNCID.Buff.LegacyCollection[mountName];

        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(CNCLocalization.GetText($"Buff.Name.{mountName}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Buff.Name.{mountName}", 7));
            Description.SetDefault(CNCLocalization.GetText($"Buff.Desc.{mountName}", 1));
            Description.AddTranslation(7, CNCLocalization.GetText($"Buff.Desc.{mountName}", 7));
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            ResetStaticDefaults();
        }

        public sealed override void Update(Player player, ref int buffIndex)
        {
            if(mount > 0)
            {
                player.GetModPlayer<AllPlayer>().buff[CNCID.Buff.Collection[mountName]] = true;
                player.mount.SetMount(mount, player);
                player.buffTime[buffIndex] = 1;
            }
        }

        public virtual void ResetStaticDefaults() { }
    }
}