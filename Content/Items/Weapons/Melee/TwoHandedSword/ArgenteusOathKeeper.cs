using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Weapons.Melee.TwoHandedSword
{
    public class ArgenteusOathKeeper : CNCWeapon.Melee.TwoHandedSword
    {
        public InscriptionSettings modItem => item.GetGlobalItem<InscriptionSettings>();
        public bool JudgementEnable => modItem.ExistInscription(CNCID.Inscription.S_Judgement);
        public bool SilveryShieldEnable => modItem.ExistInscription(CNCID.Inscription.S_SilveryShield);
        public bool SilverySparkEnable => modItem.ExistInscription(CNCID.Inscription.S_SilverySpark) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_SilverySpark);
        public bool SilveryVowEnable => modItem.ExistInscription(CNCID.Inscription.S_SilveryVow) && modItem.GetInscriptionCooldown(CNCID.Inscription.S_SilveryVow);
        public int hit = 0;
        public int hitCd = 0;

        public override void ResetDefaults()
        {
            item.width = 74;
            item.height = 76;
            item.useTime = 35;
            item.useAnimation = 35;
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
            comboDelay = 45;
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

        public override bool? UseItem(Player player)
        {
            if (modItem.ExistInscription(CNCID.Inscription.S_SilveryVow) && player.HasBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Blessed]))
            {
                item.autoReuse = true;
                item.useTime = 21;//打六折
                item.useAnimation = 21;
            }
            else
            {
                item.autoReuse = false;
                item.useTime = 35;
                item.useAnimation = 35;
            }
            return base.UseItem(player);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (this.JudgementEnable && target.type != 488 && CNC.Random.NextBool((float)CNCID.Inscription.S_Judgement.Data[0] + (player.HasBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Blessed]) ? (float)CNCID.Inscription.S_SilveryVow.Data[3] : 0f)))
            {
                damage = Inscription.JustGetDamage(90, (string)CNCID.Inscription.S_Judgement.Data[1], item) - target.defense / 2;
                CNCUtils.PlaySound($"{Name}/{CNCID.Inscription.S_Judgement.Name.Substring(CNCID.Inscription.S_Judgement.Name.LastIndexOf('_') + 1)}", player);
                CNCUtils.TrueDamage(target, damage, int.MaxValue, true, CNCID.Colors.Magic);
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
                    ParticleManager.particle[idx].velocityMul = Vector2.One * (((1 - spd / 8f) * .08f) + .92f);
                    ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-8f, 8f);
                    ParticleManager.particle[idx].scaleAdd = Vector2.One * CNC.Random.NextFloat(spd * -.005f);
                    ParticleManager.particle[idx].alphaAdd = -.05f;
                    ParticleManager.particle[idx].alpha = 1f;
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
                bool upgrade = player.HasBuff(CNCID.Buff.LegacyObject[CNCID.Buff.Blessed]);
                int num = this.comboCur;
                var info = new SlashInfo[1200];
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
                if (num == 0)
                for (int i = 0; i < info.Length; i++)
                {
                    float factor = (float)i / info.Length;
                    float rotation = MyMath.deg01 * (factor * 305 - 170);
                    Vector2 pos = new Vector2(9.5f, 0f).RotatedBy(rotation) * (upgrade ? 1.5f : 1f);
                    pos.Y *= .3f;
                    info[i].position = pos * 16f;
                }
                else
                for (int i = 0; i < info.Length; i++)
                {
                    float factor = (float)i / info.Length;
                    float rotation = MyMath.deg01 * (90 - factor * 310);
                    Vector2 pos = new Vector2(9.5f, 0f).RotatedBy(rotation) * (upgrade ? 1.5f : 1f);
                    if (rotation < MyMath.deg01 * -60) pos.Y *= .7f;
                    pos.Y *= .3f;
                    info[i].position = pos * 16f;
                }
                if (upgrade)
                {
                    Texture2D[] t =
                    {
                        CNC.GetTexture("Assets/Projectiles/Slash/SilverSlashShader"),
                        CNC.GetTexture("Assets/Effects/SlashTop1"),
                        CNC.GetTexture("Assets/Effects/Trace1"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    var s = CNCUtils.CreateShaderSlash(player, velocity.RotatedBy((CNC.Random.NextFloat(30f) - 15f) * MyMath.deg01));
                    s.OnHitNPCEffect += this.NewOnHitNPC;
                    s.SetDefaults(player.Center, velocity, 1200, 8, 10, 7);
                    s.SetColor(Color.White, Color.White, Color.White, Color.White, CNCID.Colors.SpiritBoss * .5f);
                    s.SetDamage(1, DamageClass.Melee, damage, knockback, player.GetWeaponCrit(item));
                    s.SetDraw(BlendState.AlphaBlend, 3f, 1f);
                    s.SetImage(t[0], t[1], t[2], t[3]);
                    s.SetAttackInfo(info);
                }
                else
                {
                    var e = CNCUtils.CreateSlash(player, velocity, -1);
                    e.OnHitNPCEffect += this.NewOnHitNPC;
                    e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Slash/SilverSlash"), player.Center, Vector2.One * .85f, velocity, frame[num], 7);
                    e.SetColor(Color.White, Color.Gold, .5f);
                    e.SetDamage(1, DamageClass.Melee, damage, knockback, player.GetWeaponCrit(item));
                    e.SetAttackInfo(info);
                    e.SetAfterImages(50, 1, .25f);
                    e.imageSlashHit = CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit2");
                    e.imageOffset = offset[num];
                    e.hitboxOffset = offset2[num];
                }
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