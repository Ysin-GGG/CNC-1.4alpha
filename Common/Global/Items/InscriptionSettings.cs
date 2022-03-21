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

        public bool active = false;
        public int level = -1;
        public int levelMax = -1;
        public Inscription[] inscription => _inscription1.ToArray();//已有铭文
        public Inscription[][] inscriptionList => _inscription2.ToArray();//各级待定铭文
        public bool CanUpgrade => level >= 0 && levelMax >= 0;
        public bool CanUpgrade2 => CanUpgrade && levelMax > level;
        public Mod mod => Mod;

        public override bool InstancePerEntity => true;

        public InscriptionSettings()
        {
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

        public override void SetDefaults(Item item)
        {
            List<Inscription[]> AddInscriptionList(List<Inscription[]> origin, int begin, int end, Inscription inscription)
            {
                for (; begin <= end; begin++)
                {
                    List<Inscription> member = new List<Inscription>();
                    member.AddRange(origin[begin]);
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
                List<Inscription> list1 = itemInscription._inscription1 = new List<Inscription>();
                List<Inscription[]> list2 = itemInscription._inscription2 = new List<Inscription[]>()
                {
                    new Inscription[0],
                    new Inscription[0],
                    new Inscription[0],
                    new Inscription[0],
                    new Inscription[0],
                    new Inscription[0],
                    new Inscription[0],
                    new Inscription[0],
                };
                switch (type)
                {
                    case CNCID.Item.FireGrimoire:
                        level = 0;
                        levelMax = 4;
                        list1.Add(CNCID.Inscription.S_Fireball);
                        list1.Add(CNCID.Inscription.S_Incinerating);
                        list2 = AddInscriptionList(list2, 1, 4, CNCID.Inscription.MagicAdept);
                        list2 = AddInscriptionList(list2, 1, 4, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 4, CNCID.Inscription.Assimilation);
                        list2 = AddInscriptionList(list2, 1, 4, CNCID.Inscription.Economization);
                        list2 = AddInscriptionList(list2, 0, 0, CNCID.Inscription.S_Fireball);
                        list2 = AddInscriptionList(list2, 0, 0, CNCID.Inscription.S_Incinerating);
                        list2 = AddInscriptionList(list2, 2, 2, CNCID.Inscription.S_Scorching);
                        list2 = AddInscriptionList(list2, 4, 4, CNCID.Inscription.S_Enthusiasm);
                        break;
                    case CNCID.Item.IceTooth:
                        level = 0;
                        levelMax = 6;
                        list1.Add(CNCID.Inscription.S_Gelidity);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.MagicAdept);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Assimilation);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Economization);
                        list2 = AddInscriptionList(list2, 2, 6, CNCID.Inscription.Arcana);
                        list2 = AddInscriptionList(list2, 0, 0, CNCID.Inscription.S_Gelidity);
                        list2 = AddInscriptionList(list2, 2, 2, CNCID.Inscription.S_Glacier);
                        list2 = AddInscriptionList(list2, 4, 4, CNCID.Inscription.S_Iceflake);
                        list2 = AddInscriptionList(list2, 4, 4, CNCID.Inscription.S_Shattering);
                        list2 = AddInscriptionList(list2, 6, 6, CNCID.Inscription.S_FrostWave);
                        break;
                    case CNCID.Item.Superconductor:
                        level = 0;
                        levelMax = 6;
                        list1.Add(CNCID.Inscription.S_Charging);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.MagicAdept);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Assimilation);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Economization);
                        list2 = AddInscriptionList(list2, 2, 6, CNCID.Inscription.Arcana);
                        list2 = AddInscriptionList(list2, 0, 0, CNCID.Inscription.S_Charging);
                        list2 = AddInscriptionList(list2, 1, 1, CNCID.Inscription.S_SpellLightningChain);
                        list2 = AddInscriptionList(list2, 2, 2, CNCID.Inscription.S_ArcaneOverload);
                        list2 = AddInscriptionList(list2, 4, 4, CNCID.Inscription.S_SpellReconstruction);
                        list2 = AddInscriptionList(list2, 6, 6, CNCID.Inscription.S_SpellAugmentated);
                        list2 = AddInscriptionList(list2, 6, 6, CNCID.Inscription.S_StableSpell);
                        break;
                    case CNCID.Item.ArgenteusClaw:
                        level = 0;
                        levelMax = 6;
                        list1.Add(CNCID.Inscription.S_SilveryArrow);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Dexterity);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.EnchantedBowstring);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.MeleeAdept);
                        list2 = AddInscriptionList(list2, 2, 6, CNCID.Inscription.EagleEye);
                        list2 = AddInscriptionList(list2, 3, 3, CNCID.Inscription.Penetration);
                        list2 = AddInscriptionList(list2, 0, 0, CNCID.Inscription.S_SilveryArrow);
                        list2 = AddInscriptionList(list2, 2, 2, CNCID.Inscription.S_Sanctity);
                        list2 = AddInscriptionList(list2, 4, 4, CNCID.Inscription.S_Exorcism);
                        break;
                    case CNCID.Item.SilveryTilia:
                        level = 0;
                        levelMax = 6;
                        list1.Add(CNCID.Inscription.S_SilveryFlame);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Dexterity);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.MeleeAdept);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Force);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Destroyer);
                        list2 = AddInscriptionList(list2, 0, 0, CNCID.Inscription.S_SilveryFlame);
                        list2 = AddInscriptionList(list2, 1, 1, CNCID.Inscription.S_Judgement);
                        list2 = AddInscriptionList(list2, 2, 2, CNCID.Inscription.S_SilveryShield);
                        list2 = AddInscriptionList(list2, 5, 5, CNCID.Inscription.S_SilveryVow);
                        break;
                    case CNCID.Item.StarlightOfEzcia:
                        level = 0;
                        levelMax = 3;
                        list1.Add(CNCID.Inscription.S_Rock);
                        list2 = AddInscriptionList(list2, 1, 3, CNCID.Inscription.Chanter);
                        list2 = AddInscriptionList(list2, 1, 3, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 3, CNCID.Inscription.Assimilation);
                        list2 = AddInscriptionList(list2, 1, 3, CNCID.Inscription.Economization);
                        list2 = AddInscriptionList(list2, 0, 0, CNCID.Inscription.S_Rock);
                        list2 = AddInscriptionList(list2, 1, 1, CNCID.Inscription.S_ActiveChant);
                        list2 = AddInscriptionList(list2, 3, 3, CNCID.Inscription.S_Fragmentation);
                        break;
                    case CNCID.Item.EmberOfNean:
                        level = 0;
                        levelMax = 5;
                        list1.Add(CNCID.Inscription.S_DizzyStrike);
                        list2 = AddInscriptionList(list2, 1, 5, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 5, CNCID.Inscription.Dexterity);
                        list2 = AddInscriptionList(list2, 1, 5, CNCID.Inscription.MeleeAdept);
                        list2 = AddInscriptionList(list2, 1, 5, CNCID.Inscription.Force);
                        list2 = AddInscriptionList(list2, 1, 5, CNCID.Inscription.Shockwave);
                        list2 = AddInscriptionList(list2, 1, 1, CNCID.Inscription.Smiting);
                        list2 = AddInscriptionList(list2, 2, 2, CNCID.Inscription.S_EmberPrayer);
                        list2 = AddInscriptionList(list2, 3, 3, CNCID.Inscription.S_EmberOath);
                        break;
                    case CNCID.Item.CursedKnife:
                        level = 0;
                        levelMax = 6;
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Power);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.RaidAdept);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Murder);
                        list2 = AddInscriptionList(list2, 1, 6, CNCID.Inscription.Assassination);
                        list2 = AddInscriptionList(list2, 1, 1, CNCID.Inscription.S_TameHead);
                        list2 = AddInscriptionList(list2, 2, 2, CNCID.Inscription.S_SoulScorching);
                        list2 = AddInscriptionList(list2, 3, 3, CNCID.Inscription.S_IndulgentGhost);
                        break;
                    case CNCID.Item.SpiritNachrichter:
                        break;
                    case CNCID.Item.MysticalLight:
                        break;
                    case CNCID.Item.ShortKnife:
                        break;
                    case CNCID.Item.WandOfSpiritSparking:
                        break;
                }
                itemInscription.level = level;
                itemInscription.levelMax = levelMax;
                itemInscription._inscription1 = list1;
                itemInscription._inscription2 = list2;
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
                        if (type == 1) _args = new object[] { };
                        if (type == 2) _args = new object[] { "[damage=14]" };
                        if (type == 3) _args = new object[] { 0.40f };
                        if (type == 4) _args = new object[] { 20, 9, 3 };
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