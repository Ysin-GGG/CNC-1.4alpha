using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Magic
{
    public class IceTooth : CNCItem
    {
        public bool FrostWaveEnable => item.GetGlobalItem<InscriptionSettings>().HasInscription(CNCID.Inscription.S_FrostWave);

        public override void ResetDefaults()
        {
            item.width = 48;
            item.height = 48;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 3;
            item.value = 20000;
            item.damage = 18;
            item.crit = 5;
            item.knockBack = 5f;
            item.useStyle = 5;
            item.mana = 10;
            item.noMelee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item20;
            item.DamageType = DamageClass.Magic;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.DentateIce];
            item.shootSpeed = 16f;
            clazz = 32;
            staff = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2 && this.FrostWaveEnable)
            {
                CNCUtils.PlaySound("IceTooth/FrostWave", player, 1f);
                Projectile projectile = CNCUtils.NewProj(source, position, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.FrostWave], damage, knockback);
            }
            else
            {
                CNCUtils.PlaySound("IceTooth/Shoot", player, 1f);
                Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback);
                if (item.GetGlobalItem<InscriptionSettings>().HasInscription(CNCID.Inscription.S_Iceflake)) projectile.GetGlobalProjectile<Common.Global.Projectiles.AllProjectile>().data["level"] = 1;
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
            => this.FrostWaveEnable;// && CheckCD(player, 2);

        public override bool CanUseItem(Player player)
        {
            bool flag = player.altFunctionUse == 2;
            item.mana = flag ? 30 : 9;
            item.useTime = flag ? 15 : 30;
            item.useAnimation = flag ? 15 : 30;
            item.useStyle = flag ? 4 : 5;
            if (flag) player.itemRotation = -90f * player.direction;
            return base.CanUseItem(player);
        }

        public override void HoldItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                var num1 = 45f * (player.Center.X - Main.MouseWorld.X < 0 ? -1 : 1);
                var num2 = 45f * -player.direction;
                player.itemRotation = player.itemTime > 0 ? num2 : num1;//细节待优化:使用时有转身延迟
            }
        }
    }
}