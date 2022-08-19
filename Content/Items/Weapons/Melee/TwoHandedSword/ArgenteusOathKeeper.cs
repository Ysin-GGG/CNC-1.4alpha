using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.BigProgressBar;
using ReLogic.Content;

namespace CNC.Content.Items.Weapons.Melee.TwoHandedSword
{
    class ArgenteusOathKeeper : CNCWeapon.Melee.TwoHandedSword
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();

        public override void ResetDefaults()
        {
            item.width = 74;
            item.height = 76;
            item.useTime = 15;
            item.useAnimation = 15;
            item.scale = 0f;
            item.rare = 2;
            item.value = 20000;
            item.damage = 35;
            item.crit = 5;
            item.knockBack = 8f;
            item.useStyle = 1;
            item.autoReuse = true;
            item.noMelee = true;
            item.UseSound = SoundID.Item1;
            item.DamageType = DamageClass.Melee;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.MagicRock];
            item.shootSpeed = 1f;
            comboCount = 2;
            comboDelay = 10;
            clazz = 13;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num = this.comboCur;
            var info = new Projectiles.GenericSlash.SlashInfo[2][]
            {
                    new Projectiles.GenericSlash.SlashInfo[30],
                    new Projectiles.GenericSlash.SlashInfo[30],
            };
            for (int i = 0; i < info[0].Length; i++)
            {
                float[] range = new float[30] { 70, 60, 50, 40 ,35, 32, 30, 29, 27, 28, 28, 30, 37, 44, 50, 57, 60, 63, 63, 61, 58, 56, 54, 51, 48, 48, 49, 50, 52, 55 };
                float factor = (float)i / info[0].Length;
                info[0][i].offset = Vector2.Zero;
                info[0][i].range = range[i];
                info[0][i].rotation = MyMath.deg01 * (factor * 305 - 170);
            }
            for (int i = 0; i < info[1].Length; i++)
            {
                float factor = (float)i / info[1].Length;
                info[1][i].offset = Vector2.Zero;
                info[1][i].range = 40 + MyMath.sin(factor * MyMath.deg60) * 200;
                info[1][i].rotation = MyMath.deg01 * (90 - factor * 310);
                float lim = .3f;
                if (factor > lim) info[1][i].range = 100 - MyMath.sin(((factor - lim) / (1 - lim)) * MyMath.pi) * 30;
                lim = .4f;
                if (factor > lim) info[1][i].range = 80 - MyMath.sin(((factor - lim) / (1 - lim)) * MyMath.pi) * 50;
            }
            int[][] frame =
            {
                new int[6] { 2, 3, 2, 0, 0, 0 },
                new int[6] { 0, 0, 0, 2, 3, 2 },
            };
            Vector2[] offset =
            {
                new Vector2(-20f, 0f),
                new Vector2(20f, 0f),
            };
            var e = CNCUtils.CreateSlash(player, velocity, DustId.CyanShortFx1);
            e.SetDefaults(Mod.Assets.Request<Texture2D>("Assets/Projectiles/Slash/SilverSlash").Value, player.Center, Vector2.One * .75f, velocity, frame[num], 7);
            e.SetColor(Color.White, Color.Gold, .5f);
            e.SetDamage(1, DamageClass.Melee, damage, knockback, 100);
            e.SetAttackInfo(info[num]);
            e.SetAfterImages(50, 1, .25f);
            e.imageOffset = offset[num];
            return false;
        }
    }
}