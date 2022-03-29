using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;
using CNC.Content;
using CNC.Common.Utils;
using CNC.Common.Localization;
using System.Collections.ObjectModel;
using Terraria.ModLoader.IO;

namespace CNC.Common.Global.Items
{
    public class InscriptionSettings : GlobalItem
    {
        private List<Inscription> _inscription1 = new List<Inscription>();
        private List<Inscription[]> _inscription2 = new List<Inscription[]>();
        public const int maxInscriptionList = 9;

        public bool active = false;
        public int innateness = 0;
        public int level = -1;
        public int levelMax = -1;
        public Inscription[] inscription => _inscription1.ToArray();//已有铭文
        public Inscription[][] inscriptionList => _inscription2.ToArray();//各级待定铭文
        public bool CanUpgrade => level >= 0 && levelMax >= 0;
        public bool CanUpgrade2 => CanUpgrade && levelMax > level;
        public Mod mod => Mod;

        private string[] defaultTags => new string[]
        {
            "level",
            "inscriptionType",
            "inscriptionStyle",
        };

        public override bool InstancePerEntity => true;

        public InscriptionSettings()
        {
            this.innateness = 0;
            this.level = -1;
            this.levelMax = -1;
            this.active = false;
            this._inscription1 = new List<Inscription>();
            this._inscription2 = new List<Inscription[]>();
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

        public bool HasInscription(Inscription inscription) => this._inscription1.Contains(inscription);

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

            if (item.ModItem != null && item.ModItem.Mod == mod && item.type > 0 && CNCID.Item.LegacyObjectReturn.ContainsKey(item.type))
            {
                short type = CNCID.Item.LegacyObjectReturn[item.type];
                InscriptionSettings itemInscription = item.GetGlobalItem<InscriptionSettings>();
                int level = itemInscription.level = -1;
                int levelMax = itemInscription.levelMax = -1;
                List<Inscription[]> list = itemInscription._inscription2 = new List<Inscription[]>();
                for (int i = 0; i < maxInscriptionList; i++) itemInscription._inscription2.Add(new Inscription[0]);
                switch (type)
                {
                    case CNCID.Item.FireGrimoire:
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.MagicAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 4, CNCID.Inscription.Economization);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Fireball);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Incinerating);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_Scorching);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Enthusiasm);
                        AddToInscriptionList(ref list, ref levelMax, 3, 4, CNCID.Inscription.Arcane);
                        AddToInscriptionList(ref list, ref levelMax, 3, 4, CNCID.Inscription.Reiki);
                        break;
                    case CNCID.Item.IceTooth:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.MagicAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Economization);
                        AddToInscriptionList(ref list, ref levelMax, 2, 6, CNCID.Inscription.Arcana);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Gelidity);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_Glacier);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Iceflake);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Shattering);
                        AddToInscriptionList(ref list, ref levelMax, 6, 6, CNCID.Inscription.S_FrostWave);
                        break;
                    case CNCID.Item.Superconductor:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.MagicAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Economization);
                        AddToInscriptionList(ref list, ref levelMax, 2, 6, CNCID.Inscription.Arcana);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Charging);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.S_SpellLightningChain);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_ArcaneOverload);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_SpellReconstruction);
                        AddToInscriptionList(ref list, ref levelMax, 6, 6, CNCID.Inscription.S_SpellAugmentated);
                        AddToInscriptionList(ref list, ref levelMax, 6, 6, CNCID.Inscription.S_StableSpell);
                        break;
                    case CNCID.Item.ArgenteusClaw:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Dexterity);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.EnchantedBowstring);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.MeleeAdept);
                        AddToInscriptionList(ref list, ref levelMax, 2, 6, CNCID.Inscription.EagleEye);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.Penetration);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_SilveryArrow);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_Sanctity);
                        AddToInscriptionList(ref list, ref levelMax, 4, 4, CNCID.Inscription.S_Exorcism);
                        break;
                    case CNCID.Item.ArgenteusTree:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Dexterity);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.MeleeAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Force);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Destroyer);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_SilveryFlame);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.S_Judgement);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_SilveryShield);
                        AddToInscriptionList(ref list, ref levelMax, 5, 5, CNCID.Inscription.S_SilveryVow);
                        break;
                    case CNCID.Item.TheLightOfAuzzsia:
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.Chanter);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.Assimilation);
                        AddToInscriptionList(ref list, ref levelMax, 1, 3, CNCID.Inscription.Economization);
                        AddToInscriptionList(ref list, ref levelMax, 0, 0, CNCID.Inscription.S_Rock);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.S_ActiveChant);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.S_Fragmentation);
                        break;
                    case CNCID.Item.EmberOfNien:
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.Dexterity);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.MeleeAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.Force);
                        AddToInscriptionList(ref list, ref levelMax, 1, 5, CNCID.Inscription.Shockwave);
                        AddToInscriptionList(ref list, ref levelMax, 1, 1, CNCID.Inscription.Smiting);
                        AddToInscriptionList(ref list, ref levelMax, 2, 2, CNCID.Inscription.S_EmberPrayer);
                        AddToInscriptionList(ref list, ref levelMax, 3, 3, CNCID.Inscription.S_EmberOath);
                        break;
                    case CNCID.Item.FragmentarySoulDagger:
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Power);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.RaidAdept);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Murder);
                        AddToInscriptionList(ref list, ref levelMax, 1, 6, CNCID.Inscription.Assassination);
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
                itemInscription.level = (list[0].Length > 0).ToInt();
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
                    TooltipLine tt = new TooltipLine(mod, name, desc) { overrideColor = Color.White };
                    if (Main.LocalPlayer.GetModPlayer<InscriptionSystem>().showDetail) tt.text = $"<{desc}>";
                    else tt.text = CNCLocalization.GetText("Misc.Inscription", name);
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
            InscriptionSettings itemSettings = item.GetGlobalItem<InscriptionSettings>();
            if (itemSettings.CanUpgrade)
                for (int i = 0; i < itemSettings.inscription.Length; i++)
                    if (itemSettings.inscription[i].spellCooldown > 0)
                        itemSettings.inscription[i].spellCooldown--;
        }

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