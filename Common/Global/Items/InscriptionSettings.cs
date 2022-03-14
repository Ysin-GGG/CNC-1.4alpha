using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Localization;
using System.Collections.ObjectModel;

namespace CNC.Common.Global.Items
{
    public class InscriptionSettings : GlobalItem
    {
        private List<Inscription> _inscription1 = new List<Inscription>();
        private List<Inscription[]> _inscription2 = new List<Inscription[]>();

        public int level = 0;
        public int levelMax = 0;
        public Inscription[] inscription => _inscription1.ToArray();
        public Inscription[][] inscriptionList => _inscription2.ToArray();
        public bool CanUpgrade => level > 0 && levelMax > 0;
        public bool CanUpgrade2 => CanUpgrade && levelMax > level;
        public Mod mod => Mod;

        public override bool InstancePerEntity => true;

        public InscriptionSettings()
        {
            level = 0;
            levelMax = 0;
            _inscription1 = new List<Inscription>();
            _inscription2 = new List<Inscription[]>();
    }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            InscriptionSettings clone = (InscriptionSettings)base.Clone(item, itemClone);
            if (clone != null)
            {
                clone.level = level;
                clone.levelMax = levelMax;
            }
            return clone;
        }

        public override void SetDefaults(Item item)
        {
            if (item.ModItem != null && item.ModItem.Mod == mod && CNCID.Item.LegacyObjectReturn.ContainsKey(item.type))
            {
                short type = CNCID.Item.LegacyObjectReturn[item.type];
                InscriptionSettings itemInscription = item.GetGlobalItem<InscriptionSettings>();
                switch (type)
                {
                    case CNCID.Item.FireGrimoire:
                        break;
                    case CNCID.Item.IceTooth:
                        itemInscription.level = 1;
                        itemInscription.levelMax = 5;
                        itemInscription._inscription2 = new List<Inscription[]>
                        {
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            }
                        };
                        break;
                    case CNCID.Item.Superconductor:
                        break;
                    case CNCID.Item.SilveryClaw:
                        itemInscription.level = 1;
                        itemInscription.levelMax = 5;
                        itemInscription._inscription2 = new List<Inscription[]>
                        {
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            },
                            new Inscription[]
                            {

                            }
                        };
                        break;
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            InscriptionSettings itemInscription = item.GetGlobalItem<InscriptionSettings>();
            if (itemInscription.CanUpgrade)
            {
                for (int i = 0; i < itemInscription.inscription.Length; i++)
                {
                    Inscription inscription = itemInscription.inscription[i];
                    string name = inscription.displayName;
                    string desc = inscription.GetTooltip();
                    TooltipLine tt = new TooltipLine(mod, name, desc) { overrideColor = Color.White };
                    tooltips.Add(tt);
                }
            }
        }

        public override void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
        {
        }

        public class Inscription
        {
            public short type;
            public short style;//专有铭文类型，0则通用
            public string displayName;//铭文名
            public string description;//铭文描述
            public int spellCooldown;
            public int spellCooldownMax;

            public Inscription()
            {
                this.type = 0;
                this.style = 0;
                this.displayName = "";
                this.description = "";
                SetDefaults(type, style);
            }

            public Inscription(string keyName)
            {
                this.type = type;
                this.style = style;
                this.displayName = "";
                this.description = "";
                SetDefaults(type, style);
            }

            public Inscription(short type, short style = 0)
            {
                this.type = type;
                this.style = style;
                this.displayName = "";
                this.description = "";
                SetDefaults(type, style);
            }

            public void SetDefaults(short type, short style = 0)
            {
            }

            public string GetTooltip()
            {
                string tt = "";
                if (this.displayName != "") tt += this.displayName;
                if (tt != "") tt += CNCLocalization.isChinese ? "：" : ": ";
                if (this.description != "") tt += this.description;
                return tt;
            }
        }

        private class InscriptionSystem : ModSystem
        {
            public override void PreUpdateItems()
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Item item = Main.player[i].HeldItem;
                    if(item != null && item.type != 0)
                    {
                        InscriptionSettings itemSettings = item.GetGlobalItem<InscriptionSettings>();
                        if (itemSettings.CanUpgrade) ;
                    }
                }
            }
        }
    }
}