using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Magic
{
    public class FireGrimoire : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 24;
            item.height = 30;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 3;
            item.value = 10000;
            item.damage = 36;
            item.crit = 20;
            item.knockBack = 2f;
            item.useStyle = 5;
            item.mana = 8;
            item.noMelee = true;
            item.autoReuse = true;
            item.DamageType = DamageClass.Magic;
            item.UseSound = SoundID.DD2_BetsyFireballShot;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.FireBall];
            item.shootSpeed = 16f;
            staff = false;
            CNCStaff = true;
            clazz = 32;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile projectile = CNCUtils.NewProj(source, position, velocity, type, damage, knockback);
            if (item.GetGlobalItem<InscriptionSettings>().ExistInscription(CNCID.Inscription.S_Scorching))
                projectile.GetGlobalProjectile<Common.Global.Projectiles.AllProjectile>().data["level"] = 1;
            if (item.GetGlobalItem<InscriptionSettings>().ExistInscription(CNCID.Inscription.S_Enthusiasm) && CNC.Random.NextBool((float)CNCID.Inscription.S_Enthusiasm.Data[0]))
                player.AddBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Enthused], 300);
            return false;
        }
    }
}