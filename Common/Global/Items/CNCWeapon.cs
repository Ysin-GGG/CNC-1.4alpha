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
            public sealed override void HoldItem(Player player) => this.NewHoldItem(player);
            public sealed override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) => this.NewModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
            public sealed override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) => this.NewModifyHitPvp(player, target, ref damage, ref crit);
            public sealed override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => this.NewOnHitNPC(player, target, damage, knockBack, crit);
            public sealed override void OnHitPvp(Player player, Player target, int damage, bool crit) => this.NewOnHitPvp(player, target, damage, crit);

            protected virtual bool? NewUseItem(Player player) => base.UseItem(player);
            protected virtual void NewHoldItem(Player player) { }
            protected virtual void NewUpdateInventory(Player player) { }
            protected virtual void NewModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }
            protected virtual void NewModifyHitPvp(Player player, Player target, ref int damage, ref bool crit) { }
            protected virtual void NewOnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) { }
            protected virtual void NewOnHitPvp(Player player, Player target, int damage, bool crit) { }

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
            public abstract class Longbow : Ranged
            {

            }

            public abstract class Shortbow : Ranged
            {

            }

            public abstract class Crossbow : Ranged
            {

            }

            public abstract class Arbalest : Ranged
            {

            }

            public abstract class Pistol : Ranged
            {

            }

            public abstract class Heavyweapons : Ranged
            {

            }

            public abstract class Blowgun : Ranged
            {

            }
        }

        public abstract class Magic : CNCWeapon
        {
            public abstract class Magicwand : Magic
            {

            }

            public abstract class Sceptre : Magic
            {

            }

            public abstract class Grimoire : Magic
            {

            }

            public abstract class Arcaneitems : Magic
            {

            }
        }

        public abstract class Raid : CNCWeapon
        {
            public abstract class Knife : Raid
            {
                public static void AddBleeding(NPC target, int value)
                {
                    int max = target.GetGlobalNPC<AllNPC>().bleeding.Length;
                    CNCUtils.SetBuff(target, CNCID.Buff.Bleeding, max * 60);
                    for (int i = 0; i < max; i++) target.GetGlobalNPC<AllNPC>().bleeding[i] += value / max;
                }
            }

            public abstract class Spear : Raid
            {

            }

            public abstract class Rapier : Raid
            {

            }

            public abstract class Darts : Raid
            {

            }

        }
    }
}