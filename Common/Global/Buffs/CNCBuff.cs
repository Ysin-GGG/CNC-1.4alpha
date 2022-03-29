using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;

namespace CNC.Common.Global.Buffs
{
    public abstract class CNCBuff : ModBuff
    {
        private int _damage = 0;
        public int damage { get => _damage; set => _damage = value; }
        public bool pvp { get => Main.pvpBuff[Type]; set => Main.pvpBuff[Type] = value; }
        public bool debuff { get => Main.debuff[Type]; set => Main.debuff[Type] = value; }
        public bool noSave { get => Main.buffNoSave[Type]; set => Main.buffNoSave[Type] = value; }
        public bool noTimeDisplay { get => Main.buffNoTimeDisplay[Type]; set => Main.buffNoTimeDisplay[Type] = value; }
        public bool noClearedByNurse { get => BuffID.Sets.NurseCannotRemoveDebuff[Type]; set => BuffID.Sets.NurseCannotRemoveDebuff[Type] = value; }
        public bool longer { get => BuffID.Sets.LongerExpertDebuff[Type]; set => BuffID.Sets.LongerExpertDebuff[Type] = value; }
        public string killText { get => _killText; set => _killText = value; }
        private string _killText = "";

        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(CNCLocalization.GetText($"Buff.Name.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Buff.Name.{Name}", 7));
            Description.SetDefault(CNCLocalization.GetText($"Buff.Desc.{Name}", 1));
            Description.AddTranslation(7, CNCLocalization.GetText($"Buff.Desc.{Name}", 7));
            ResetStaticDefaults();
        }

        public sealed override void Update(Player target, ref int buffIndex)
        {
            AllPlayer gt = target.GetModPlayer<AllPlayer>();
            //gt.buff[cid] = true;
            Update(target, gt, ref buffIndex);
            if (_damage > 0)
            {
                if (gt.lifeRegen > 0) gt.lifeRegen = gt.lifeRegenCount = 0;
                if (target.lifeRegen > 0) target.lifeRegen = 0;
                //gt.lifeRegen -= 2 * _damage;
                gt.lifeRegenCount += gt.lifeRegen;
                if (gt.lifeRegenCount <= -120 * _damage)
                {
                    gt.lifeRegenCount += 120 * _damage;
                    target.statLife -= _damage;
                    ModifyDamage(target, gt, ref buffIndex);
                    CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height), CombatText.LifeRegen, _damage, false, true);
                }
                if (target.statLife <= 0 && target.whoAmI == Const.me)
                {
                    string tt = _killText;
                    tt = tt.Replace("[player]", target.name);
                    tt = tt.Replace("[Player]", target.name);
                    target.KillMe(PlayerDeathReason.ByCustomReason(tt), damage, target.direction);
                }
                if (Main.netMode == 1) { NetMessage.SendData(16, -1, -1, null, target.whoAmI, 0f, 0f, 0f, 0, 0, 0); }
            }
        }

        public sealed override void Update(NPC target, ref int buffIndex)
        {
            AllNPC gt = target.GetGlobalNPC<AllNPC>();
            //gt.buff[cid] = true;
            Update(target, gt, ref buffIndex);
            if (_damage > 0)
            {
                if (gt.lifeRegen > 0) { gt.lifeRegen = 0; gt.lifeRegenCount = 0; }
                //gt.lifeRegen -= 2 * _damage;
                gt.lifeRegenCount += gt.lifeRegen;
                if (gt.lifeRegenCount <= -120 * _damage)
                {
                    gt.lifeRegenCount += 120 * _damage;
                    int whoAmI = target.whoAmI;
                    if (target.realLife >= 0)
                    {
                        whoAmI = target.realLife;
                    }
                    NPC npc = Main.npc[whoAmI];
                    if (!npc.immortal)
                    {
                        npc.life -= _damage;
                    }
                    ModifyDamage(target, gt, ref buffIndex);
                    CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height), CombatText.LifeRegenNegative, _damage, false, true);
                    if (npc.life <= 0 && !npc.immortal)
                    {
                        npc.life = _damage;
                        if (Main.netMode == 0)
                        {
                            npc.StrikeNPCNoInteraction(9999, 0f, 0, false, false, false);
                        }
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(28, -1, -1, null, whoAmI, 9999f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
            }
        }

        public virtual void ResetStaticDefaults() { }

        public virtual void Update(Player target, AllPlayer gt, ref int index) { }

        public virtual void Update(NPC target, AllNPC gt, ref int index) { }

        public virtual void ModifyDamage(Player target, AllPlayer gt, ref int index) { }

        public virtual void ModifyDamage(NPC target, AllNPC gt, ref int index) { }
    }
}