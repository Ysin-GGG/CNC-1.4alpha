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
    public class ArgenteusOathKeeper : CNCWeapon.Melee.TwoHandedSword
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();
        public bool JudgementEnable => modItem.HasInscription(CNCID.Inscription.S_Judgement);
        public bool SilveryShieldEnable => modItem.HasInscription(CNCID.Inscription.S_SilveryShield);
        public bool SilverySparkEnable => modItem.HasInscription(CNCID.Inscription.S_SilverySpark) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_SilverySpark);
        public bool SilveryVowEnable => modItem.HasInscription(CNCID.Inscription.S_SilveryVow) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_SilveryVow);
        public int hit = 0;
        public int hitCd = 0;

        public override void ResetDefaults()
        {
            item.width = 74;
            item.height = 76;
            item.useTime = 50;
            item.useAnimation = 50;
            item.scale = 0f;
            item.rare = 2;
            item.value = 20000;
            item.damage = 35;
            item.crit = 5;
            item.knockBack = 8f;
            item.useStyle = 4;
            item.noMelee = true;
            item.UseSound = SoundID.Item1;
            item.DamageType = DamageClass.Melee;
            item.shoot = CNCID.Projectile.LegacyObject[CNCID.Projectile.SilveryDash];
            item.shootSpeed = 32f;
            comboCount = 2;
            comboDelay = 60;
            clazz = 13;
        }

        public override bool AltFunctionUse(Player player)
            => this.SilveryVowEnable && this.ConsumeBlessing(player, (int)CNCID.Inscription.S_SilveryVow.Data[0]);

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse == 2)
            {
                item.scale = 1f;
                player.itemRotation = -MyMath.deg45 * player.direction;
            }
            else item.scale = 0f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (this.JudgementEnable && target.type != 488 && CNC.Random.NextBool((float)CNCID.Inscription.S_Judgement.Data[0] + (player.HasBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Blessed]) ? (float)CNCID.Inscription.S_SilveryVow.Data[3] : 0f)))
            {
                CNCUtils.PlaySound($"{Name}/{CNCID.Inscription.S_Judgement.Name.Substring(CNCID.Inscription.S_Judgement.Name.LastIndexOf('_') + 1)}", player);
                CNCUtils.TrueDamage(target, Inscription.JustGetDamage(90, (string)CNCID.Inscription.S_Judgement.Data[1], item) - target.defense / 2, int.MaxValue, true, CNCID.Colors.Magic);
                for (int i = 0; i < 32; i++)
                {
                    float spd = i % 8 + 1f;
                    int idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                        target.Center,
                        new Vector2(0, (i / 8 % 2 == 0 ? MyMath.phi : 1f) * spd * .75f).RotatedBy(MyMath.deg90 * (i / 8)),
                        CNC.Random.NextDegree(),
                        20,
                        Vector2.One * .5f,
                        CNCID.Colors.SpiritBoss2
                        );
                    ParticleRenderer.particle[idx].velocityMul = Vector2.One * (((1 - spd / 8f) * .08f) + .92f);
                    ParticleRenderer.particle[idx].rotationAdd = CNC.Random.NextDegree(-8f, 8f);
                    ParticleRenderer.particle[idx].scaleAdd = Vector2.One * CNC.Random.NextFloat(spd * -.005f);
                    ParticleRenderer.particle[idx].alphaAdd = -.05f;
                    ParticleRenderer.particle[idx].alpha = 1f;
                }
            }
            if (this.SilveryShieldEnable && target.type != 488 && hitCd-- > 0)
            {
                if (++hit % 5  == 0) CombatText.NewText(player.Hitbox, Color.Cyan, hit == 20 ? "Silver Field!" : hit + " Hit", false, hit != 20);
                if (hit >= 20)
                {
                    hit = 0;
                    int dmg = Inscription.JustGetDamage(200, (string)CNCID.Inscription.S_SilveryShield.Data[3], 0f, 1f);
                    CNCUtils.PlaySound(SoundID.Item29, player);
                    CNCUtils.PlaySound(SoundID.Item78, player);
                    CNCUtils.PlaySound(SoundID.Item81, player);
                    CNCUtils.NewProj(player.GetSource_FromThis(), player.Bottom, Vector2.Zero, CNCID.Projectile.LegacyObject[CNCID.Projectile.SilveryField], dmg, 20f, player.GetWeaponCrit(item), -1, 0f, 1f);
                }
            }
        }
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                modItem.SetInscriptionCooldown(CNCID.Inscription.S_SilveryVow, -1, -1, null);
                CNCUtils.SetBuff(player, CNCID.Buff.Blessed, 60 * (int)CNCID.Inscription.S_SilveryVow.Data[1]);
                CNCUtils.PlaySound(SoundID.Item29, player);
                CNCUtils.PlaySound(Name + "/Blessing", player);
                var e = CNCUtils.CreateExplosion(player);
                e.SetDefaults(Mod.Assets.Request<Texture2D>("Content/Projectiles/ArgenteusOathKeeper/Sanctuary").Value, player.Center, Vector2.One * 170, 23, 2);
                e.SetDamage(0, item.DamageType, 0, 0, 0, 0f, 1f);
                e.SetColor(CNCID.Colors.SpiritBoss, CNCID.Colors.SpiritBoss, 1f);
                e.SetAlpha(1f, -.005f, 1f);
                e.SetScale(1.25f);
                e.projectile.Bottom = player.Bottom;
                return false;
            }
            if (!this.SilverySparkEnable)
            {
                int num = this.comboCur;
                var info = new Projectiles.GenericSlash.SlashInfo[2][]
                {
                    new Projectiles.GenericSlash.SlashInfo[60],
                    new Projectiles.GenericSlash.SlashInfo[60],
                };
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
                Vector2[] offset2 =
                {
                    new Vector2(0f, 8f),
                    new Vector2(32f, 0f),
                };
                for (int i = 0; i < info[0].Length; i++)
                {
                    float factor = (float)i / info[0].Length;
                    float rotation = MyMath.deg01 * (factor * 305 - 170);
                    Vector2 pos = new Vector2(8f, 0f).RotatedBy(rotation);
                    pos.Y *= .3f;
                    info[0][i].position = pos * 16f;
                }
                for (int i = 0; i < info[1].Length; i++)
                {
                    float factor = (float)i / info[1].Length;
                    float rotation = MyMath.deg01 * (90 - factor * 310);
                    Vector2 pos = new Vector2(8f, 0f).RotatedBy(rotation);
                    if (rotation < MyMath.deg01 * -60) pos.Y *= .7f;
                    pos.Y *= .3f;
                    info[1][i].position = pos * 16f;
                }
                var e = CNCUtils.CreateSlash(player, velocity, -1);
                e.OnHitNPCEffect += this.NewOnHitNPC;
                e.SetDefaults(Mod.Assets.Request<Texture2D>("Assets/Projectiles/Slash/SilverSlash").Value, player.Center, Vector2.One * .75f, velocity, frame[num], 7);
                e.SetColor(Color.White, Color.Gold, .5f);
                e.SetDamage(1, DamageClass.Melee, damage, knockback, player.GetWeaponCrit(item));
                e.SetAttackInfo(info[num]);
                e.SetAfterImages(50, 1, .25f);
                e.imageOffset = offset[num];
                e.hitboxOffset = offset2[num];
            }
            else
            {
                this.comboNext = 0;
                player.itemTime /= 2;
                player.itemAnimation /= 2;
                int dmg = Inscription.JustGetDamage(damage, (string)CNCID.Inscription.S_SilverySpark.Data[1], 0f, 1f);
                ((Common.Global.Projectiles.CNCProjectile)CNCUtils.NewProj(player.GetSource_FromThis(), position, velocity, type, dmg, knockback, player.GetWeaponCrit(item), -1, 0f, 1f).ModProjectile).OnHitNPCEffect += this.NewOnHitNPC;
            }
            modItem.SetInscriptionCooldown(CNCID.Inscription.S_SilverySpark, -1, -1, null);
            hitCd = 1;
            return false;
        }
    }
}