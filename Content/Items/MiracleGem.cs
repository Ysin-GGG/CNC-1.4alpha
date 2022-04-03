using Terraria.ID;
using Terraria.ModLoader;
using CNC.Common.Global.Items;

namespace CNC.Content.Items
{
	public class MiracleGem : CNCItem
	{
		public override void ResetDefaults() {
			item.width = 20;
            item.height = 22;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 4;
            item.value = 250000;
            item.rare = 8;
            item.noMelee = true;
            item.mountType = ModContent.MountType<Mounts.MiracleFox>();
            item.UseSound = SoundID.Item79;
        }
	}
}
