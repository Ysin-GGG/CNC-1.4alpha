using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using CNC.Content;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Players;

namespace CNC.Common.Global.Items
{
    public class InscriptionSettings : GlobalItem
    {
        private List<Inscription> _inscription1 = new List<Inscription>();
        private List<Inscription[]> _inscription2 = new List<Inscription[]>();

        public const int maxInscriptionList = 9;
        public Inscription[] inscription => _inscription1.ToArray();//已有铭文
        public Inscription[][] inscriptionList => _inscription2.ToArray();//各级待定铭文
        public bool CanUpgrade => level >= 0 && levelMax >= 0;
        public bool CanUpgrade2 => CanUpgrade && levelMax > level;
        public Mod mod => Mod;

        public bool active = false;
        public int level = -1;
        public int levelMax = -1;
        public int innateness = 0;
        public float attackSpeed = 0f;
        public float cooldownSpeedAdd = 0f;
        public float cooldownSpeedMul = 1f;
        public float cooldownReductionAdd = 0f;
        public float cooldownReductionMul = 1f;
        public float chantingSpeedAdd = 0f;
        public float chantingSpeedMul = 1f;
        public float chantingReductionAdd = 0f;
        public float chantingReductionMul = 0f;
        public float skillDamageAdd = 0f;
        public float skillDamageMul = 1f;
        public int weaponCritAdd = 0;
        public float weaponDamageAdd = 0f;
        public float weaponDamageMul = 1f;
        public float dealtDamageAdd = 0f;
        public float dealtDamageMul = 0f;
        public float manaCostAdd = 0f;
        public float manaCostMul = 1f;
        public float manaReturn = 0f;

        public StatData weaponDamage = new StatData(0, 1, "WeaponDamage");
        public StatData dealtDamage = new StatData(0, 1, "DealtDamage");

        public Dictionary<string, StatData> data = new Dictionary<string, StatData>();

        private string[] defaultTags => new string[]
        {
            "level",
            "inscriptionType",
            "inscriptionStyle",
        };

        public override bool InstancePerEntity => true;

        public InscriptionSettings()
        {
            this._inscription1 = new List<Inscription>();
            this._inscription2 = new List<Inscription[]>();
            this.active = false;
            this.level = -1;
            this.levelMax = -1;
            this.innateness = 0;
            this.ResetEffects();
        }

        public void ResetEffects(Item item = null)
        {
            if (item == null)
            {
                this.attackSpeed = 0f;
                this.cooldownSpeedAdd = 0f;
                this.cooldownSpeedMul = 1f;
                this.cooldownReductionAdd = 0f;
                this.cooldownReductionMul = 1f;
                this.chantingSpeedAdd = 0f;
                this.chantingSpeedMul = 1f;
                this.chantingReductionAdd = 0f;
                this.chantingReductionMul = 0f;
                this.skillDamageAdd = 0f;
                this.skillDamageMul = 1f;
                this.weaponCritAdd = 0;
                this.weaponDamageAdd = 0f;
                this.weaponDamageMul = 1f;
                this.dealtDamageAdd = 0f;
                this.dealtDamageMul = 0f;
                this.manaCostAdd = 0f;
                this.manaCostMul = 1f;
                this.manaReturn = 0f;
            }
            else
            {
                InscriptionSettings modItem = item.GetGlobalItem<InscriptionSettings>();
                modItem.attackSpeed = 0f;
                modItem.cooldownSpeedAdd = 0f;
                modItem.cooldownSpeedMul = 1f;
                modItem.cooldownReductionAdd = 0f;
                modItem.cooldownReductionMul = 1f;
                modItem.chantingSpeedAdd = 0f;
                modItem.chantingSpeedMul = 1f;
                modItem.chantingReductionAdd = 0f;
                modItem.chantingReductionMul = 0f;
                modItem.skillDamageAdd = 0f;
                modItem.skillDamageMul = 1f;
                modItem.weaponCritAdd = 0;
                modItem.weaponDamageAdd = 0f;
                modItem.weaponDamageMul = 1f;
                modItem.dealtDamageAdd = 0f;
                modItem.dealtDamageMul = 0f;
                modItem.manaCostAdd = 0;
                modItem.manaCostMul = 0;
                modItem.manaReturn = 0;
            }
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            InscriptionSettings clone = (InscriptionSettings)base.Clone(item, itemClone);
            if (clone != null)
            {
                clone.level = this.level;
                clone.levelMax = this.levelMax;
                clone._inscription1 = this._inscription1;
                clone._inscription2 = this._inscription2;
            }
            return clone;
        }
        
        public override void SaveData(Item item, TagCompound tag)
        {
            int[] type = new int[this.inscription.Length];
            int[] style = new int[this.inscription.Length];
            for (int i = 0; i < this.inscription.Length; i++)
            {
                type[i] = this.inscription[i].type;
                style[i] = this.inscription[i].style;
            }
            tag[defaultTags[0]] = this.level;
            tag[defaultTags[1]] = type;
            tag[defaultTags[2]] = style;
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            item.GetGlobalItem<InscriptionSettings>().level = tag.GetInt(defaultTags[0]);
            List<Inscription> list = new List<Inscription>();
            int[] type = tag.GetIntArray(defaultTags[1]);
            int[] style = tag.GetIntArray(defaultTags[2]);
            for (int i = 0; i < type.Length; i++) list.Add(new Inscription((short)type[i], (short)style[i]));
            item.GetGlobalItem<InscriptionSettings>()._inscription1 = list;
        }

        public bool ExistInscription(Inscription inscription) => this._inscription1.Contains(inscription);

        /// <summary> 为铭文施加冷却 </summary>
        /// <param name="inscription">铭文类型</param>
        /// <param name="newCooldown">铭文冷却时长</param>
        /// <param name="newCooldownMax">铭文原始冷却时长</param>
        /// <param name="player">要提供冷却缩减的玩家</param>
        public void SetInscriptionCooldown(Inscription inscription, int newCooldown = -1, int newCooldownMax = -1, Player player = null)
        {
            float add = 0; 
            float mul = 1;
            if (player != null)
            {
                add = player.GetModPlayer<Players.InscriptionPlayer>().cooldownReductionAdd;
                mul = player.GetModPlayer<Players.InscriptionPlayer>().cooldownReductionMul;
            }
            for (int i = 0; i < this._inscription1.Count; i++)
                if (this._inscription1[i] == inscription)
                    this._inscription1[i].SetCooldown(newCooldown, newCooldownMax, this.cooldownReductionAdd + add, this.cooldownReductionMul * mul);
        }

        /// <summary> 铭文是否冷却中 </summary>
        /// <param name="inscription">铭文类型</param>
        public bool GetInscriptionCooldown(Inscription inscription)
        {
            bool flag = false;
            for (int i = 0; i < this._inscription1.Count; i++)
                if (this._inscription1[i].type == inscription.type && this._inscription1[i].style == inscription.style)
                    flag = this._inscription1[i].cooldown <= 0;
            return flag;
        }

        public void SetInscription(int index, Inscription newInscription)
        {
            if (newInscription != null)
            {
                if (index < 0) this._inscription1.Add(newInscription);
                else this._inscription1[index] = newInscription;
            }
            else
            {
                if (index < 0) this._inscription1.RemoveAt(_inscription1.Count - 1);
                else this._inscription1.RemoveAt(index);
            }
        }

        public void SetInscription(int index, string name)
        {
            if (CNCID.Inscription.LegacyCollection.ContainsKey(name))
            {
                Inscription newInscription = CNCID.Inscription.LegacyCollection[name];
                if (index < 0) this._inscription1.Add(newInscription);
                else this._inscription1[index] = newInscription;
            }
            else
            {
                if (index < 0) this._inscription1.RemoveAt(_inscription1.Count - 1);
                else this._inscription1.RemoveAt(index);
            }
        }

        public void SetInscription(int index, short type, short style)
        {
            if (CNCID.Inscription.Collection.ContainsKey(new Point(type, style)))
            {
                Inscription newInscription = new Inscription(type, style);
                if (index < 0) this._inscription1.Add(newInscription);
                else this._inscription1[index] = newInscription;
            }
            else
            {
                if (index < 0) this._inscription1.RemoveAt(_inscription1.Count - 1);
                else this._inscription1.RemoveAt(index);
            }
        }

        /// <summary> 物品可选铭文表预设 </summary>
        /// <param name="item">当前物品</param>
        public override void SetDefaults(Item item)
        {
            List<Inscription[]> AddToInscriptionList(ref List<Inscription[]> origin, ref int levelMax, int begin, int end, Inscription inscription)
            {
                if (end > maxInscriptionList) end = maxInscriptionList;
                if (end >= levelMax) levelMax = end + 1;
                for (; begin <= end; begin++)
                {
                    List<Inscription> member = new List<Inscription>(origin[begin]);
                    member.Add(inscription);
                    origin[begin] = member.ToArray();
                }
                return origin;
            }

            if (item.ModItem != null && item.ModItem.Mod == mod && item.type > 0 && CNCID.Item.LegacyObject.ContainsValue(item.type))
            {
                short type = CNCID.Item.LegacyObject.FirstOrDefault(q => q.Value == item.type).Key;
                InscriptionSettings itemInscription = item.GetGlobalItem<InscriptionSettings>();
                int level = itemInscription.level = -1;
                int levelMax = itemInscription.levelMax = -1;
                List<Inscription[]> list = itemInscription._inscription2 = new List<Inscription[]>();
                for (int i = 0; i < maxInscriptionList; i++) itemInscription._inscription2.Add(new Inscription[0]);
                switch (type)
                {
                    case CNCID.Item.FireGrimoire:
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.C_MagicAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.C_Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.C_Economization);
                        AddToInscriptionList(ref list, ref levelMax, 3, 4, CNCID.Inscription.C_Arcane);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.C_Reiki);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Fireball);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Incinerating);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_Scorching);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Enthusiasm);
                        break;
                    case CNCID.Item.IceTooth:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_MagicAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Economization);
                        AddToInscriptionList(ref list, ref levelMax, 2, 6, CNCID.Inscription.C_Arcane);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_SkillPower);
                        AddToInscriptionList(ref list, ref levelMax, 5, 6, CNCID.Inscription.C_Charging);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Gelidity);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_Glacier);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Iceflake);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Shattering);
                        AddToInscriptionList(ref list, ref levelMax, 6, 6, CNCID.Inscription.S_FrostWave);
                        break;
                    case CNCID.Item.Superconductor:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_MagicAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Economization);
                        AddToInscriptionList(ref list, ref levelMax, 2, 6, CNCID.Inscription.C_Arcane);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Charging);
                        AddToInscriptionList(ref list, ref levelMax, 3, 6, CNCID.Inscription.C_SkillPower);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.C_Reiki);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Charging);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.S_SpellLightningChain);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_ArcaneOverload);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_SpellReconstruction);
                        AddToInscriptionList(ref list, ref levelMax, 6, 6, CNCID.Inscription.S_SpellAugmentated);
                        AddToInscriptionList(ref list, ref levelMax, 6, 6, CNCID.Inscription.S_StableSpell);
                        break;
                    case CNCID.Item.ArgenteusClaw:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Dexterity);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_EnchantedBowstring);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_MeleeAdept);
                        AddToInscriptionList(ref list, ref levelMax, 2, 6, CNCID.Inscription.C_EagleEye);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.C_Penetration);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_SilveryArrow);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_Sanctity);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Exorcism);
                        break;
                    case CNCID.Item.ArgenteusOathKeeper:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Dexterity);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_MeleeAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Force);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Destroyer);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_SkillPower);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Charging);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.S_Judgement);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_SilveryShield);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_SilverySpark);
                        AddToInscriptionList(ref list, ref levelMax, 5, 5, CNCID.Inscription.S_SilveryVow);
                        break;
                    case CNCID.Item.TheLightOfAuzzsia:
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.C_Chanter);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.C_Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.C_Economization);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.C_SkillPower);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.C_Arcane);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.C_Reiki);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Rock);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.S_ActiveChant);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.S_Fragmentation);
                        break;
                    case CNCID.Item.EmberOfNien:
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.C_Dexterity);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.C_MeleeAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.C_Force);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.C_Shockwave);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.C_Smiting);
                        AddToInscriptionList(ref list, ref levelMax, 2, 5, CNCID.Inscription.C_Charging);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_EmberPrayer);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.S_EmberOath);
                        break;
                    case CNCID.Item.FragmentarySoulDagger:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_RaidAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Murder);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.C_Assassination);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.S_TameHead);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_SoulScorching);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.S_IndulgentGhost);
                        break;
                    case CNCID.Item.SpiritIronGreatsword:
                        break;
                    case CNCID.Item.MysticalLight:
                        break;
                    case CNCID.Item.ShortDagger:
                        break;
                    case CNCID.Item.SpiritSparkingWand:
                        break;
                }
                itemInscription.innateness = list[0].Length;
                itemInscription.level = (levelMax > 0).ToInt();
                itemInscription.levelMax = levelMax;
                itemInscription._inscription1 = new List<Inscription>(list[0]);
                itemInscription._inscription2 = list;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            InscriptionSettings itemInscription = item.GetGlobalItem<InscriptionSettings>();
            if (itemInscription.CanUpgrade)
            {
                for (int i = 1; i <= itemInscription.inscription.Length; i++)
                {
                    Inscription inscription = itemInscription.inscription[i - 1];
                    string name = inscription.displayName;
                    string desc = "";
                    string desc2 = inscription.GetTooltip();
                    while (desc != desc2) { desc = desc2; Inscription.GetFirstDamage(ref desc2, item); }
                    TooltipLine tt = new TooltipLine(mod, name, desc) { OverrideColor = Color.White };
                    if (Main.LocalPlayer.GetModPlayer<InscriptionSystem>().showDetail) tt.Text = $"<{desc}>";
                    else tt.Text = CNCLocalization.GetText("Misc.Inscription", name);
                    tooltips.Add(tt);
                }
            }
        }

        public override void RightClick(Item item, Player player)
        {
        }

        public override void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
        {
        }

        public override void UpdateInventory(Item item, Player player)
        {
            InscriptionSettings modItem = item.GetGlobalItem<InscriptionSettings>();
            if (modItem.CanUpgrade)
            {
                modItem.ResetEffects();
                for (int i = 0; i < modItem.inscription.Length; i++)
                {
                    Inscription inscription = modItem.inscription[i];
                    if (modItem.inscription[i].cooldown > 0) modItem.inscription[i].cooldown--;
                    if (inscription == CNCID.Inscription.C_SkillPower) modItem.skillDamageAdd += (float)CNCID.Inscription.C_SkillPower.Data[0];
                    if (inscription == CNCID.Inscription.C_Charging) modItem.cooldownReductionAdd += (float)CNCID.Inscription.C_Charging.Data[0];
                    if (inscription == CNCID.Inscription.C_Power) modItem.weaponDamageAdd += (float)CNCID.Inscription.C_Power.Data[0];
                    if (inscription == CNCID.Inscription.C_Arcane) modItem.weaponCritAdd += (int)((float)CNCID.Inscription.C_Arcane.Data[0] * 100f);
                    if (inscription == CNCID.Inscription.C_Destroyer) modItem.weaponCritAdd += (int)((float)CNCID.Inscription.C_Destroyer.Data[0] * 100f);
                    if (inscription == CNCID.Inscription.C_Murder) modItem.weaponCritAdd += (int)((float)CNCID.Inscription.C_Murder.Data[0] * 100f);
                    if (inscription == CNCID.Inscription.C_EagleEye) modItem.weaponCritAdd += (int)((float)CNCID.Inscription.C_EagleEye.Data[0] * 100f);
                    if (inscription == CNCID.Inscription.C_MagicAdept) modItem.attackSpeed += (float)CNCID.Inscription.C_MagicAdept.Data[0];
                    if (inscription == CNCID.Inscription.C_MeleeAdept) modItem.attackSpeed += (float)CNCID.Inscription.C_MeleeAdept.Data[0];
                    if (inscription == CNCID.Inscription.C_RaidAdept) modItem.attackSpeed += (float)CNCID.Inscription.C_RaidAdept.Data[0];
                    if (inscription == CNCID.Inscription.C_Reiki) modItem.manaReturn += (float)CNCID.Inscription.C_Reiki.Data[0];
                    if (inscription == CNCID.Inscription.C_Economization) modItem.manaCostAdd += (float)CNCID.Inscription.C_Economization.Data[0];
                    if (inscription == CNCID.Inscription.C_EnchantedBowstring) modItem.manaCostAdd += (float)CNCID.Inscription.C_EnchantedBowstring.Data[0];
                }
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            InscriptionSettings modItem = item.GetGlobalItem<InscriptionSettings>();
            for (int i = 0; i < modItem.inscription.Length; i++)
            {
                Inscription inscription = modItem.inscription[i];
                if (inscription == CNCID.Inscription.C_EnchantedBowstring) player.GetModPlayer<ClassesSettings>().clazz_PowerSpeedAdd += (float)CNCID.Inscription.C_EnchantedBowstring.Data[0];
            }
        }

        public override void OnConsumeMana(Item item, Player player, int manaConsumed)
        {
            int mana = (int)(manaConsumed * item.GetGlobalItem<InscriptionSettings>().manaReturn);
            if (mana > 0)
            {
                player.statMana += mana;
                player.ManaEffect(mana);
            }
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            reduce += item.GetGlobalItem<InscriptionSettings>().manaCostAdd;
            mult *= item.GetGlobalItem<InscriptionSettings>().manaCostMul;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            damage += item.GetGlobalItem<InscriptionSettings>().weaponDamageAdd;
            damage *= item.GetGlobalItem<InscriptionSettings>().weaponDamageMul;
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit) 
            => crit += item.GetGlobalItem<InscriptionSettings>().weaponCritAdd;

        public class InscriptionSystem : ModPlayer
        {
            public static ModKeybind InscriptionKey;
            public bool showDetail = false;
            Player player => Player;
            InscriptionSystem modPlayer => player.GetModPlayer<InscriptionSystem>();

            public override void ProcessTriggers(TriggersSet triggersSet)
            {
                if (showDetail = InscriptionKey.Current)
                {
                    //showDetail = !showDetail;
                }
            }
        }
    }
}