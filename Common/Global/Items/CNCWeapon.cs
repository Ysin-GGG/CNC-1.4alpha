using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;
using CNC.Common.Global.Projectiles;

namespace CNC.Common.Global.Items
{
    public abstract class CNCWeapon : CNCItem
    {
        public virtual bool NoAssistant => false;
        public InscriptionSettings insp => this.inscriptions;

        public sealed override bool CanUseItem(Player player)
        {
            bool flag = this.NewCanUseItem(player);
            if (flag)
            {
                int mouse = 0;
                float castTime = 0;
                float castTimeMax = 0;
                bool allowRelease = true;
                bool cannotBeCanceled = false;
                if (!player.All().Casting && this.OnCast(player, ref mouse, ref castTime, ref castTimeMax, ref allowRelease, ref cannotBeCanceled))
                {
                    if ((mouse == 1 || mouse == 2) && castTimeMax > 0)
                        player.All().SetCasting(mouse, castTime, castTimeMax, allowRelease, cannotBeCanceled);
                }
                if (player.All().Casting) return false;
            }
            return flag;
        }

        public sealed override void HoldItem(Player player)
        {
            if (player.IsMe() && player.All().castTimeMax > 0)
            {
                SoundStyle? useSound = SoundID.Item4;
                if (ModifyCasting(player, player.All().MouseToCast, player.All().CastingAccepted, ref useSound, ref player.All().castTime, ref player.All().castTimeMax))
                {
                    if(player.All().CastingAccept && useSound != null) CNCUtils.PlaySound(useSound.Value, player);
                    float damage = player.GetWeaponDamage(item);
                    float knockback = player.GetWeaponKnockback(item);
                    float crit = player.GetWeaponCrit(item);
                    int type = item.shoot;
                    IEntitySource source = item.GetSource_ItemUse(item);
                    Vector2 position = player.Center;
                    Vector2 velocity = new Vector2(item.shootSpeed, 0f).RotatedBy((Main.MouseWorld - player.Center).ToRotation());
                    if (CastingShoot(player, ref position, ref velocity, ref type, ref damage, ref knockback, ref crit))
                    {
                        CNCUtils.NewProj(source, position, velocity, item.shoot, (int)damage, knockback, (int)crit, player.whoAmI, 0f, 1f, 0f, 0f);
                        CNCUtils.PlaySound(item.UseSound.Value, player);
                    }
                    player.All().ResetCasting(false);
                    player.ApplyItemTime(item);
                    player.ApplyItemAnimation(item);
                }
            }
            this.NewHoldItem(player);
        }

        /// <summary> 修改充能属性 </summary>
        /// <param name="player">当前玩家</param>
        /// <param name="mouse">触发充能的鼠标键（左键=1，右键=2）</param>
        /// <param name="accepted">自然地完成充能</param>
        /// <param name="acceptedSound">充能完成时的音效（默认SoundID.Item4）</param>
        /// <param name="castTime">当前充能时长</param>
        /// <param name="castTimeMax">当前充能时长原始需求（即无缩减的充能时长需求）</param>
        /// <returns>是否执行充能完成后的动作（默认返回accepted的值）</returns>
        public virtual bool ModifyCasting(Player player, int mouse, bool accepted, ref SoundStyle? acceptedSound, ref float castTime, ref float castTimeMax) => accepted;

        /// <summary> 充能预备 </summary>
        /// <param name="player">当前玩家</param>
        /// <param name="mouse">触发充能的鼠标键（左键=1，右键=2，需要选择其一才能充能）</param>
        /// <param name="castTime">充能的起始时间（默认0）</param>
        /// <param name="castTimeMax">充能时长需求（需要大于0才能充能）</param>
        /// <param name="allowRelease">充能时是否不需要按紧（默认true）</param>
        /// <param name="cannotBeCanceled">充能不受被击影响（默认false）</param>
        /// <returns>是否开始充能（默认在castTimeMax大于0时返回true，反之false）</returns>
        public virtual bool OnCast(Player player, ref int mouse, ref float castTime, ref float castTimeMax, ref bool allowReleasem, ref bool cannotBeCanceled) => castTimeMax > 0;

        /// <summary> 充能完成后的弹幕发射 </summary>
        /// <param name="player">该弹幕归属的玩家</param>
        /// <param name="position">生成坐标（默认玩家中心，即Player.Center）</param>
        /// <param name="velocity">发射速度（速度大小默认为item.shootSpeed，方向默认为(Main.MouseWorld - player.Center).ToRotation()）</param>
        /// <param name="type">发射的弹幕类型（默认item.shoot）</param>
        /// <param name="damage">弹幕伤害（默认player.GetWeaponDamage(item)）</param>
        /// <param name="knockback">弹幕击退率（默认player.GetWeaponKnockback(item)）</param>
        /// <param name="crit">弹幕暴击率（默认player.GetWeaponCrit(item)）</param>
        /// <returns>是否发射默认弹幕（默认返回true）</returns>
        public virtual bool CastingShoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref float damage, ref float knockback, ref float crit) => true;

        protected virtual void NewHoldItem(Player player) { }
        protected virtual bool NewCanUseItem(Player player) => base.CanUseItem(player);

        public abstract class Melee : CNCWeapon
        {
            public int comboCur { get => allItem.comboCur; set => allItem.comboCur = value; }
            public int comboNext { get => allItem.comboNext; set => allItem.comboNext = value; }
            public int comboCount { get => allItem.comboCount; set => allItem.comboCount = value; }
            public int comboTime { get => allItem.comboTime; set => allItem.comboTime = value; }
            public int comboDelay { get => allItem.comboDelay; set => allItem.comboDelay = value; }

            /// <summary> 连段修改（重写自UseItem） </summary>
            /// <param name="player">玩家</param>
            /// <param name="curCombo">当前连段</param>
            /// <param name="nextCombo">下一连段</param>
            /// <param name="comboTime">连段间隔时长</param>
            public virtual bool? ModifyCombo(Player player, int curCombo, ref int nextCombo, ref int comboTime) => base.UseItem(player);

            public sealed override bool? UseItem(Player player)
            {
                bool? flag = this.NewUseItem(player);
                int next = (this.comboCur + 1) % comboCount;
                int time = this.comboDelay;
                flag = this.ModifyCombo(player, this.comboCur, ref next, ref time);
                this.comboNext = next % comboCount;
                this.comboTime = time;
                return flag;
            }
            public sealed override void UpdateInventory(Player player) => this.NewUpdateInventory(player);
            public sealed override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) => this.NewModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
            public sealed override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) => this.NewModifyHitPvp(player, target, ref damage, ref crit);
            public sealed override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => this.NewOnHitNPC(player, target, damage, knockBack, crit);
            public sealed override void OnHitPvp(Player player, Player target, int damage, bool crit) => this.NewOnHitPvp(player, target, damage, crit);

            protected virtual bool? NewUseItem(Player player) => base.UseItem(player);
            protected virtual void NewUpdateInventory(Player player) { }
            protected virtual void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
            protected virtual void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
            protected virtual void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
            protected virtual void NewOnHitPvp(Player player, Player target, int damage, bool crit) { }

            /// <summary> 长剑 </summary>
            public abstract class LongSword : Melee
            {
                #region Default
                public new virtual bool? UseItem(Player player) => base.NewUseItem(player);
                public new virtual void UpdateInventory(Player player) { }
                public new virtual void HoldItem(Player player) { }
                public new virtual void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
                public new virtual void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
                public new virtual void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
                public new virtual void OnHitPvp(Player player, Player target, int damage, bool crit) { }

                protected sealed override bool? NewUseItem(Player player) => this.UseItem(player);
                protected sealed override void NewHoldItem(Player player) => this.HoldItem(player);
                protected sealed override void NewUpdateInventory(Player player) => this.UpdateInventory(player);
                protected sealed override void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) => this.NewModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
                protected sealed override void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) => this.NewModifyHitPvp(player, target, ref damage, ref crit);
                protected sealed override void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => this.NewOnHitNPC(player, target, damage, knockBack, crit);
                protected sealed override void NewOnHitPvp(Player player, Player target, int damage, bool crit) => this.NewOnHitPvp(player, target, damage, crit);
                #endregion
            }

            /// <summary> 椎 </summary>
            public abstract class Hammer : Melee
            {
                public virtual float DizzyStrikeChance => .1f;
                public virtual int DizzyStrikeBuffTime => 60;

                protected sealed override void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
                {
                    if (CNC.Random.NextBool()) CNCUtils.SetBuff(target, CNCID.Buff.Dizzy, this.DizzyStrikeBuffTime);
                    this.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
                }

                protected sealed override void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
                {
                    if (CNC.Random.NextBool()) CNCUtils.SetBuff(target, CNCID.Buff.Dizzy, this.DizzyStrikeBuffTime);
                    this.ModifyHitPvp(player, target, ref damage, ref crit);
                }
                #region Default
                public new virtual bool? UseItem(Player player) => base.NewUseItem(player);
                public new virtual void UpdateInventory(Player player) { }
                public new virtual void HoldItem(Player player) { }
                public new virtual void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
                public new virtual void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
                public new virtual void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
                public new virtual void OnHitPvp(Player player, Player target, int damage, bool crit) { }

                protected sealed override bool? NewUseItem(Player player) => this.UseItem(player);
                protected sealed override void NewHoldItem(Player player) => this.HoldItem(player);
                protected sealed override void NewUpdateInventory(Player player) => this.UpdateInventory(player);
                protected sealed override void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => this.NewOnHitNPC(player, target, damage, knockBack, crit);
                protected sealed override void NewOnHitPvp(Player player, Player target, int damage, bool crit) => this.NewOnHitPvp(player, target, damage, crit);
                #endregion
            }

            /// <summary> 战锤 </summary>
            public abstract class BattleHammer : Melee
            {
                public override bool NoAssistant => true;
                public virtual float DizzyStrikeChance => .1f;
                public virtual int DizzyStrikeBuffTime => 60;
                public virtual float BluntImpactExtraDamage => .2f;

                protected sealed override void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
                {
                    damage += (int)(target.defense * BluntImpactExtraDamage);
                    if (CNC.Random.NextBool()) CNCUtils.SetBuff(target, CNCID.Buff.Dizzy, this.DizzyStrikeBuffTime);
                    this.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
                }

                protected sealed override void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
                {
                    damage += (int)(target.statDefense * BluntImpactExtraDamage);
                    if (CNC.Random.NextBool()) CNCUtils.SetBuff(target, CNCID.Buff.Dizzy, this.DizzyStrikeBuffTime);
                    this.ModifyHitPvp(player, target, ref damage, ref crit);
                }
                #region Default
                public new virtual bool? UseItem(Player player) => base.NewUseItem(player);
                public new virtual void UpdateInventory(Player player) { }
                public new virtual void HoldItem(Player player) { }
                public new virtual void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
                public new virtual void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
                public new virtual void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
                public new virtual void OnHitPvp(Player player, Player target, int damage, bool crit) { }

                protected sealed override bool? NewUseItem(Player player) => this.UseItem(player);
                protected sealed override void NewHoldItem(Player player) => this.HoldItem(player);
                protected sealed override void NewUpdateInventory(Player player) => this.UpdateInventory(player);
                protected sealed override void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => this.NewOnHitNPC(player, target, damage, knockBack, crit);
                protected sealed override void NewOnHitPvp(Player player, Player target, int damage, bool crit) => this.NewOnHitPvp(player, target, damage, crit);
                #endregion
            }

            /// <summary> 战斧 </summary>
            public abstract class BattleAxe : Melee
            {
                public override bool NoAssistant => true;
                public virtual float DecapitationMaxLifePercent => .5f;
                public virtual float DecapitationExtraDamage => .5f;

                protected sealed override void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
                {
                    if (target.GetLifePercent() < this.DecapitationMaxLifePercent) damage += (int)(damage * DecapitationExtraDamage);
                    this.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
                }

                protected sealed override void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
                {
                    if (target.statLife < target.statLifeMax2 * this.DecapitationMaxLifePercent) damage += (int)(damage * DecapitationExtraDamage);
                    this.ModifyHitPvp(player, target, ref damage, ref crit);
                }
                #region Default
                public new virtual bool? UseItem(Player player) => base.NewUseItem(player);
                public new virtual void UpdateInventory(Player player) { }
                public new virtual void HoldItem(Player player) { }
                public new virtual void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
                public new virtual void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
                public new virtual void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
                public new virtual void OnHitPvp(Player player, Player target, int damage, bool crit) { }

                protected sealed override bool? NewUseItem(Player player) => this.UseItem(player);
                protected sealed override void NewHoldItem(Player player) => this.HoldItem(player);
                protected sealed override void NewUpdateInventory(Player player) => this.UpdateInventory(player);
                protected sealed override void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => this.NewOnHitNPC(player, target, damage, knockBack, crit);
                protected sealed override void NewOnHitPvp(Player player, Player target, int damage, bool crit) => this.NewOnHitPvp(player, target, damage, crit);
                #endregion
            }

            /// <summary> 双手剑 </summary>
            public abstract class TwoHandedSword : Melee
            {
                public override bool NoAssistant => true;
                public int MonsterHunterCooldown { get; set; }
                public bool MonsterHunter => MonsterHunterCooldown <= 0;
                public virtual int MonsterHunterCooldownMax => 60;
                public virtual float MonsterHunterExtraDamage => .04f;
                public virtual float Heaviness => .1f;

                protected sealed override void NewHoldItem(Player player)
                {
                    player.moveSpeed -= Heaviness;
                    this.HoldItem(player);
                }

                protected sealed override void NewUpdateInventory(Player player)
                {
                    if (!this.MonsterHunter) this.MonsterHunterCooldown--;
                    this.UpdateInventory(player);
                }

                protected sealed override void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
                {
                    this.OnHitNPC(player, target, damage, knockBack, crit);
                    if (this.MonsterHunter && target.type != 488 && crit)
                    {
                        float quarter = target.boss ? .25f : 1f;
                        CNCUtils.TrueDamage(target, (int)(target.lifeMax * this.MonsterHunterExtraDamage * quarter), 500);
                        this.MonsterHunterCooldown = this.MonsterHunterCooldownMax;
                    }
                }

                #region Default
                public new virtual bool? UseItem(Player player) => base.NewUseItem(player);
                public new virtual void UpdateInventory(Player player) { }
                public new virtual void HoldItem(Player player) { }
                public new virtual void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
                public new virtual void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
                public new virtual void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
                public new virtual void OnHitPvp(Player player, Player target, int damage, bool crit) { }

                protected sealed override bool? NewUseItem(Player player) => this.UseItem(player);
                protected sealed override void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) => this.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
                protected sealed override void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) => this.ModifyHitPvp(player, target, ref damage, ref crit);
                protected sealed override void NewOnHitPvp(Player player, Player target, int damage, bool crit) => this.OnHitPvp(player, target, damage, crit);
                #endregion
            }
        }

        public abstract class Ranged : CNCWeapon
        {
            /// <summary> 长弓 </summary>
            public abstract class Longbow : Ranged
            {

            }

            /// <summary> 短弓 </summary>
            public abstract class Shortbow : Ranged
            {

            }

            /// <summary> 十字弩 </summary>
            public abstract class Crossbow : Ranged
            {

            }

            /// <summary> 劲弩 </summary>
            public abstract class Arbalest : Ranged
            {

            }

            /// <summary> 手枪 </summary>
            public abstract class Pistol : Ranged
            {

            }

            /// <summary> 吹箭筒 </summary>
            public abstract class Blowgun : Ranged
            {

            }

            /// <summary> 重武器 </summary>
            public abstract class HeavyWeapon : Ranged
            {

            }
        }

        public abstract class Magic : CNCWeapon
        {
            public sealed override void UpdateInventory(Player player) => this.NewUpdateInventory(player);
            public sealed override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) => this.NewModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
            public sealed override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) => this.NewModifyHitPvp(player, target, ref damage, ref crit);
            public sealed override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => this.NewOnHitNPC(player, target, damage, knockBack, crit);
            public sealed override void OnHitPvp(Player player, Player target, int damage, bool crit) => this.NewOnHitPvp(player, target, damage, crit);

            protected virtual bool? NewUseItem(Player player) => base.UseItem(player);
            protected virtual void NewUpdateInventory(Player player) { }
            protected virtual void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
            protected virtual void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
            protected virtual void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
            protected virtual void NewOnHitPvp(Player player, Player target, int damage, bool crit) { }

            /// <summary> 魔杖 </summary>
            public abstract class MagicWand : Magic
            {

            }

            /// <summary> 法杖 </summary>
            public abstract class Sceptre : Magic
            {
                #region Default
                public new virtual bool? UseItem(Player player) => base.NewUseItem(player);
                public new virtual bool CanUseItem(Player player) => base.NewCanUseItem(player);
                public new virtual void UpdateInventory(Player player) { }
                public new virtual void HoldItem(Player player) { }
                public new virtual void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
                public new virtual void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
                public new virtual void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
                public new virtual void OnHitPvp(Player player, Player target, int damage, bool crit) { }

                protected sealed override bool? NewUseItem(Player player) => this.UseItem(player);
                protected sealed override bool NewCanUseItem(Player player) => this.CanUseItem(player);
                protected sealed override void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) => this.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
                protected sealed override void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) => this.ModifyHitPvp(player, target, ref damage, ref crit);
                protected sealed override void NewOnHitPvp(Player player, Player target, int damage, bool crit) => this.OnHitPvp(player, target, damage, crit);
                #endregion
            }

            /// <summary> 魔典 </summary>
            public abstract class Grimoire : Magic
            {

            }

            /// <summary> 法器 </summary>
            public abstract class ArcaneItem : Magic
            {

            }
        }
        
        public abstract class Raid : CNCWeapon
        {
            /// <summary> 匕首 </summary>
            public abstract class Knife : Raid
            {
                public static void AddBleeding(NPC target, int value)
                {
                    int max = target.GetGlobalNPC<AllNPC>().bleeding.Length;
                    CNCUtils.SetBuff(target, CNCID.Buff.Bleeding, max * 60);
                    for (int i = 0; i < max; i++) target.GetGlobalNPC<AllNPC>().bleeding[i] += value / max;
                }
            }

            /// <summary> 轻型矛 </summary>
            public abstract class Spear : Raid
            {

            }

            /// <summary> 刺剑 </summary>
            public abstract class Rapier : Raid
            {

            }

            /// <summary> 飞镖 </summary>
            public abstract class Darts : Raid
            {

            }

        }
    }
}