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
        public bool CanUpgrade2 => CanUpgrade && levelMax >= level;
        public Mod mod => Mod;

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
            List<Inscription[]> AddToInscriptionList(List<Inscription[]> origin, int begin, int end, Inscription inscription)
            {
                if (end > maxInscriptionList) end = maxInscriptionList;
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
                        levelMax = 4;
                        list = AddToInscriptionList(list, 1, 4, CNCID.Inscription.MagicAdept);
                        list = AddToInscriptionList(list, 1, 4, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 4, CNCID.Inscription.Assimilation);
                        list = AddToInscriptionList(list, 1, 4, CNCID.Inscription.Economization);
                        list = AddToInscriptionList(list, 0, 0, CNCID.Inscription.S_Fireball);
                        list = AddToInscriptionList(list, 0, 0, CNCID.Inscription.S_Incinerating);
                        list = AddToInscriptionList(list, 2, 2, CNCID.Inscription.S_Scorching);
                        list = AddToInscriptionList(list, 4, 4, CNCID.Inscription.S_Enthusiasm);
                        break;
                    case CNCID.Item.IceTooth:
                        levelMax = 6;
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.MagicAdept);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Assimilation);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Economization);
                        list = AddToInscriptionList(list, 2, 6, CNCID.Inscription.Arcana);
                        list = AddToInscriptionList(list, 0, 0, CNCID.Inscription.S_Gelidity);
                        list = AddToInscriptionList(list, 2, 2, CNCID.Inscription.S_Glacier);
                        list = AddToInscriptionList(list, 4, 4, CNCID.Inscription.S_Iceflake);
                        list = AddToInscriptionList(list, 4, 4, CNCID.Inscription.S_Shattering);
                        list = AddToInscriptionList(list, 6, 6, CNCID.Inscription.S_FrostWave);
                        break;
                    case CNCID.Item.Superconductor:
                        levelMax = 6;
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.MagicAdept);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Assimilation);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Economization);
                        list = AddToInscriptionList(list, 2, 6, CNCID.Inscription.Arcana);
                        list = AddToInscriptionList(list, 0, 0, CNCID.Inscription.S_Charging);
                        list = AddToInscriptionList(list, 1, 1, CNCID.Inscription.S_SpellLightningChain);
                        list = AddToInscriptionList(list, 2, 2, CNCID.Inscription.S_ArcaneOverload);
                        list = AddToInscriptionList(list, 4, 4, CNCID.Inscription.S_SpellReconstruction);
                        list = AddToInscriptionList(list, 6, 6, CNCID.Inscription.S_SpellAugmentated);
                        list = AddToInscriptionList(list, 6, 6, CNCID.Inscription.S_StableSpell);
                        break;
                    case CNCID.Item.ArgenteusClaw:
                        levelMax = 6;
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Dexterity);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.EnchantedBowstring);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.MeleeAdept);
                        list = AddToInscriptionList(list, 2, 6, CNCID.Inscription.EagleEye);
                        list = AddToInscriptionList(list, 3, 3, CNCID.Inscription.Penetration);
                        list = AddToInscriptionList(list, 0, 0, CNCID.Inscription.S_SilveryArrow);
                        list = AddToInscriptionList(list, 2, 2, CNCID.Inscription.S_Sanctity);
                        list = AddToInscriptionList(list, 4, 4, CNCID.Inscription.S_Exorcism);
                        break;
                    case CNCID.Item.ArgenteusTree:
                        levelMax = 6;
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Dexterity);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.MeleeAdept);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Force);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Destroyer);
                        list = AddToInscriptionList(list, 0, 0, CNCID.Inscription.S_SilveryFlame);
                        list = AddToInscriptionList(list, 1, 1, CNCID.Inscription.S_Judgement);
                        list = AddToInscriptionList(list, 2, 2, CNCID.Inscription.S_SilveryShield);
                        list = AddToInscriptionList(list, 5, 5, CNCID.Inscription.S_SilveryVow);
                        break;
                    case CNCID.Item.TheLightOfAuzzsia:
                        levelMax = 3;
                        list = AddToInscriptionList(list, 1, 3, CNCID.Inscription.Chanter);
                        list = AddToInscriptionList(list, 1, 3, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 3, CNCID.Inscription.Assimilation);
                        list = AddToInscriptionList(list, 1, 3, CNCID.Inscription.Economization);
                        list = AddToInscriptionList(list, 0, 0, CNCID.Inscription.S_Rock);
                        list = AddToInscriptionList(list, 1, 1, CNCID.Inscription.S_ActiveChant);
                        list = AddToInscriptionList(list, 3, 3, CNCID.Inscription.S_Fragmentation);
                        break;
                    case CNCID.Item.EmberOfNien:
                        levelMax = 5;
                        list = AddToInscriptionList(list, 1, 5, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 5, CNCID.Inscription.Dexterity);
                        list = AddToInscriptionList(list, 1, 5, CNCID.Inscription.MeleeAdept);
                        list = AddToInscriptionList(list, 1, 5, CNCID.Inscription.Force);
                        list = AddToInscriptionList(list, 1, 5, CNCID.Inscription.Shockwave);
                        list = AddToInscriptionList(list, 1, 1, CNCID.Inscription.Smiting);
                        list = AddToInscriptionList(list, 2, 2, CNCID.Inscription.S_EmberPrayer);
                        list = AddToInscriptionList(list, 3, 3, CNCID.Inscription.S_EmberOath);
                        break;
                    case CNCID.Item.FragmentarySoulDagger:
                        levelMax = 6;
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Power);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.RaidAdept);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Murder);
                        list = AddToInscriptionList(list, 1, 6, CNCID.Inscription.Assassination);
                        list = AddToInscriptionList(list, 1, 1, CNCID.Inscription.S_TameHead);
                        list = AddToInscriptionList(list, 2, 2, CNCID.Inscription.S_SoulScorching);
                        list = AddToInscriptionList(list, 3, 3, CNCID.Inscription.S_IndulgentGhost);
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