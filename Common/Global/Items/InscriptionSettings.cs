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
                clone.level = this.level;
                clone.levelMax = this.levelMax;
                clone._inscription1 = this._inscription1;
                clone._inscription2 = this._inscription2;
            }
            return clone;
        }

        public override void SetDefaults(Item item)
        {
            if (item.ModItem != null && item.ModItem.Mod == mod && CNCID.Item.LegacyObjectReturn.ContainsKey(item.type))
            {
                short type = CNCID.Item.LegacyObjectReturn[item.type];
                InscriptionSettings itemInscription = item.GetGlobalItem<InscriptionSettings>();
                itemInscription._inscription1 = new List<Inscription>();
                switch (type)
                {
                    case CNCID.Item.FireGrimoire:
                        break;
                    case CNCID.Item.IceTooth:
                        itemInscription.level = 1;
                        itemInscription.levelMax = 5;
                        itemInscription._inscription1.Add(CNCID.Inscription.S_Gelidity);
                        itemInscription._inscription2 = new List<Inscription[]>
                        {
                            new Inscription[]
                            {
                                CNCID.Inscription.S_Gelidity,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Chanter,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.Assimilation,
                                CNCID.Inscription.Economization,
                                CNCID.Inscription.Arcana,
                                CNCID.Inscription.S_Iceflake,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Chanter,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.Assimilation,
                                CNCID.Inscription.Economization,
                                CNCID.Inscription.Arcana,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Chanter,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.Assimilation,
                                CNCID.Inscription.Economization,
                                CNCID.Inscription.Arcana,
                                CNCID.Inscription.S_Glacier,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Chanter,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.Assimilation,
                                CNCID.Inscription.Economization,
                                CNCID.Inscription.Arcana,
                            },
                        };
                        break;
                    case CNCID.Item.Superconductor:
                        break;
                    case CNCID.Item.ArgenteusClaw:
                        itemInscription.level = 1;
                        itemInscription.levelMax = 5;
                        itemInscription._inscription1.Add(CNCID.Inscription.S_SilveryArrow);
                        itemInscription._inscription2 = new List<Inscription[]>
                        {
                            new Inscription[]
                            {
                                CNCID.Inscription.S_SilveryArrow,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Dexterity,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.EnchantedBowstring,
                                CNCID.Inscription.MeleeAdept,
                                CNCID.Inscription.EagleEye,
                                CNCID.Inscription.S_Sanctity,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Dexterity,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.EnchantedBowstring,
                                CNCID.Inscription.MeleeAdept,
                                CNCID.Inscription.EagleEye,
                                CNCID.Inscription.Penetration,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Dexterity,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.EnchantedBowstring,
                                CNCID.Inscription.MeleeAdept,
                                CNCID.Inscription.EagleEye,
                                CNCID.Inscription.S_Exorcism,
                            },
                            new Inscription[]
                            {
                                CNCID.Inscription.Dexterity,
                                CNCID.Inscription.Power,
                                CNCID.Inscription.EnchantedBowstring,
                                CNCID.Inscription.MeleeAdept,
                                CNCID.Inscription.EagleEye,
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
                for (int i = 1; i <= itemInscription.inscription.Length; i++)
                {
                    Inscription inscription = itemInscription.inscription[i - 1];
                    string name = inscription.displayName;
                    string desc = inscription.GetTooltip();
                    TooltipLine tt = new TooltipLine(mod, name, desc) { overrideColor = Color.White };
                    if (Main.LocalPlayer.GetModPlayer<InscriptionSystem>().showDetail) tt.text = $"<{inscription.GetTooltip()}>";
                    else tt.text = CNCLocalization.GetText("Misc.Inscription", inscription.displayName);
                    tooltips.Add(tt);
                }
            }
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

        public class Inscription
        {
            public short type;
            public short style;//专有铭文类型，0则通用
            public string name => _name;//铭文内部文件名
            private string _name;
            public string displayName;//铭文名
            public string description;//铭文描述
            public int spellCooldown;
            public int spellCooldownMax;
            public object[] args => _args;
            private object[]  _args = new object[99];

            public Inscription() => this.SetDefaults(0, 0);

            public Inscription(short type, short style = 0) => this.SetDefaults(type, style);

            public void SetDefaults(short type, short style)
            {
                this.type = type;
                this.style = style;
                this._name = "Inscription." + CNCID.Inscription.Collection[new Point(type, style)];
                string[] tt = CNCLocalization.GetText(_name.Replace("Inscription.S_", "S_Inscription.")).Split('|');
                this._name = CNCID.Inscription.Collection[new Point(type, style)];
                this.spellCooldown = 0;
                this.spellCooldownMax = 0;
                switch (style)
                {
                    case 0:
                        if (type == 0) _args = new object[] { 0.20f };
                        if (type == 1) _args = new object[] { 0.25f };
                        if (type == 2) _args = new object[] { 0.20f };
                        if (type == 3) _args = new object[] { 0.13f };
                        if (type == 4) _args = new object[] { 0.20f };
                        if (type == 5) _args = new object[] { 0.25f };
                        if (type == 6) _args = new object[] { 0.20f };
                        if (type == 7) _args = new object[] { 0.20f };
                        if (type == 8) _args = new object[] { 0.25f };
                        if (type == 9) _args = new object[] { 0.20f };
                        if (type == 10) _args = new object[] { 0.15f, 0.35f };
                        if (type == 11) _args = new object[] { 0.10f };
                        if (type == 12) _args = new object[] { 0.25f };
                        if (type == 13) _args = new object[] { 0.20f };
                        if (type == 14) _args = new object[] { 20 };
                        if (type == 15) _args = new object[] { };
                        if (type == 16) _args = new object[] { 0.40f };
                        if (type == 17) _args = new object[] { 1 };
                        if (type == 18) _args = new object[] { 0.30f };
                        if (type == 19) _args = new object[] { 0.15f, };
                        if (type == 20) _args = new object[] { 0.30f, 0.30f };
                        if (type == 21) _args = new object[] { 0.27f };
                        if (type == 22) _args = new object[] { 0.17f };
                        if (type == 23) _args = new object[] { 0.10f, 0.50f };
                        if (type == 24) _args = new object[] { 0.35f };
                        if (type == 25) _args = new object[] { 0.20f };
                        break;
                    case CNCID.Item.FireGrimoire:
                        if (type == 0) _args = new object[] { };
                        if (type == 1) _args = new object[] { "[damage=36]" };
                        if (type == 2) _args = new object[] { 8 };
                        if (type == 3) _args = new object[] { 0.02f };
                        break;
                    case CNCID.Item.IceTooth:
                        if (type == 0) _args = new object[] { };
                        if (type == 1) _args = new object[] { "[damage=14]" };
                        if (type == 2) _args = new object[] { };
                        break;
                    case CNCID.Item.Superconductor:
                        if (type == 0) _args = new object[] { };
                        if (type == 1) _args = new object[] { "[damage=36]" };
                        if (type == 2) _args = new object[] { 0.03f, 10, 4, "[damage=40]" };
                        if (type == 3) _args = new object[] { 0.05f };
                        if (type == 4) _args = new object[] { 1, 5 };
                        if (type == 5) _args = new object[] { 5 };
                        break;
                    case CNCID.Item.ArgenteusClaw:
                        if (type == 0) _args = new object[] { };
                        if (type == 1) _args = new object[] { 0.45f, "[damage=2f]" };
                        if (type == 2) _args = new object[] { 3, "[damage=3f]" };
                        break;
                    case CNCID.Item.SilveryTilia:
                        if (type == 0) _args = new object[] { };
                        if (type == 1) _args = new object[] { "[damage=12]" };
                        if (type == 2) _args = new object[] { 30, 15, "[damage=80]", "[damage=100]" };
                        if (type == 3) _args = new object[] { 6, 8, 0.15f, 0.15f, 10 };
                        break;
                    case CNCID.Item.StarlightOfEzcia:
                        if (type == 0) _args = new object[] { "[damage=50]" };
                        if (type == 1) _args = new object[] { };
                        if (type == 2) _args = new object[] { "[damage=30]" };
                        break;
                    case CNCID.Item.EmberOfNean:
                        if (type == 0) _args = new object[] { 0.10f, 1 };
                        if (type == 1) _args = new object[] { 6, 6 };
                        if (type == 2) _args = new object[] { 5, 5, 0.10f };
                        break;
                    case CNCID.Item.CursedKnife:
                        if (type == 0) _args = new object[] { 10, 8, 8, 2.00f };
                        if (type == 1) _args = new object[] { 3, 0.10f, 4 };
                        if (type == 2) _args = new object[] { 6 };
                        break;
                }
                this.displayName = tt[0];
                this.description = tt[1];
            }

            private string GetDesc(object[] args)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is float arg1) args[i] = ((int)(arg1 * 100f)).ToString() + '%';
                    else if (args[i] is string arg2) args[i] = arg2;
                    else if (args[i] is int arg3) args[i] = arg3.ToString();
                    else args[i] = "";
                }
                return string.Format(this.description, args);
            }

            public string GetTooltip(object[] args = default)
            {
                string tt = GetDesc(args == default ? this.args : args);
                return tt;
            }

            public override bool Equals(object obj)
            {
                if (obj is Inscription other) return this.type == other.type && this.style == other.style;
                else return base.Equals(obj);
            }

            public override int GetHashCode() => new { type, style, }.GetHashCode();
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