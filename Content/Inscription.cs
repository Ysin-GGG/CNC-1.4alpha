using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Items;
using CNC.Common.Global.Players;

namespace CNC.Content
{
    public class Inscription
    {
        public short type;
        public short style;//专有铭文类型，0则通用
        public string Name => _name;//铭文内部文件名
        private string _name;
        public string displayName;//铭文名
        public string description;//铭文描述
        public int spellCooldown;
        public int spellCooldownMax;
        public object[] Args => _args;
        private object[] _args = new object[99];

        public Inscription() => this.SetDefaults(0, -1);

        public Inscription(short type, short style = 0) => this.SetDefaults(type, style);

        public void SetDefaults(short type, short style)
        {
            if (!CNCID.Inscription.Collection.ContainsKey(new Point(type, style)))
            {
                this.type = 0;
                this.style = -1;
                this._name = "None";
                this.displayName = "";
                this.description = "";
                this.spellCooldown = 0;
                this.spellCooldownMax = 0;
                this._args = new object[99];
                return;
            }
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
                    if (type == 1) _args = new object[] { "[damage3=36]" };
                    if (type == 2) _args = new object[] { 8 };
                    if (type == 3) _args = new object[] { 0.02f };
                    break;
                case CNCID.Item.IceTooth:
                    if (type == 0) _args = new object[] { };
                    if (type == 1) _args = new object[] { };
                    if (type == 2) _args = new object[] { "[damage3=14]" };
                    if (type == 3) _args = new object[] { 0.40f };
                    if (type == 4) _args = new object[] { 20, 9, 3 };
                    break;
                case CNCID.Item.Superconductor:
                    if (type == 0) _args = new object[] { };
                    if (type == 1) _args = new object[] { "[damage3=36]" };
                    if (type == 2) _args = new object[] { 0.03f, 10, 4, "[damage3=40]" };
                    if (type == 3) _args = new object[] { 0.05f };
                    if (type == 4) _args = new object[] { 1, 5 };
                    if (type == 5) _args = new object[] { 5 };
                    break;
                case CNCID.Item.ArgenteusClaw:
                    if (type == 0) _args = new object[] { };
                    if (type == 1) _args = new object[] { 0.45f, "[damage2=200f]" };
                    if (type == 2) _args = new object[] { 3, "[damage2=300f]" };
                    break;
                case CNCID.Item.ArgenteusTree:
                    if (type == 0) _args = new object[] { };
                    if (type == 1) _args = new object[] { "[damage1=12]" };
                    if (type == 2) _args = new object[] { 30, 15, "[damage1=80]", "[damage1=100]" };
                    if (type == 3) _args = new object[] { 6, 8, 0.15f, 0.15f, 10 };
                    break;
                case CNCID.Item.TheLightOfAuzzsia:
                    if (type == 0) _args = new object[] { "[damage3=50]" };
                    if (type == 1) _args = new object[] { };
                    if (type == 2) _args = new object[] { "[damage3=30]" };
                    break;
                case CNCID.Item.EmberOfNien:
                    if (type == 0) _args = new object[] { 0.10f, 1 };
                    if (type == 1) _args = new object[] { 6, 6 };
                    if (type == 2) _args = new object[] { 5, 5, 0.10f };
                    break;
                case CNCID.Item.FragmentarySoulDagger:
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
                else if (args[i] is int arg2) args[i] = arg2.ToString();
                else if (args[i] is string arg3) args[i] = arg3;
                else args[i] = "";
            }
            return string.Format(this.description, args);
        }

        public string GetTooltip(object[] args = default)
        {
            string tt = GetDesc(args == default ? this.Args : args);
            return tt;
        }

        public override bool Equals(object obj)
        {
            if (obj is Inscription other) return this.type == other.type && this.style == other.style;
            return base.Equals(obj);
        }

        public override int GetHashCode() => new { type, style, }.GetHashCode();

        public static void GetFirstDamage(ref string text, Item item)
        {
            Player p = Main.LocalPlayer;
            InscriptionPlayer ip = p.GetModPlayer<InscriptionPlayer>();
            InscriptionSettings i = item.GetGlobalItem<InscriptionSettings>();
            float[] damageAdd = new float[]
            {
                    p.GetDamage(DamageClass.Generic).Additive,
                    p.GetDamage(DamageClass.Melee).Additive,
                    p.GetDamage(DamageClass.Ranged).Additive,
                    p.GetDamage(DamageClass.Magic).Additive,
                    p.GetDamage(DamageClass.Summon).Additive,
                    p.GetDamage(DamageClass.Throwing).Additive,
            };
            float[] damageMul = new float[]
            {
                    p.GetDamage(DamageClass.Generic).Multiplicative,
                    p.GetDamage(DamageClass.Melee).Multiplicative,
                    p.GetDamage(DamageClass.Ranged).Multiplicative,
                    p.GetDamage(DamageClass.Magic).Multiplicative,
                    p.GetDamage(DamageClass.Summon).Multiplicative,
                    p.GetDamage(DamageClass.Throwing).Multiplicative,
            };
            Color[] damageColor = new Color[]
            {
                    CNCUtils.AccurateColor(0  ,0  ,0  ,0,true),
                    CNCUtils.AccurateColor(217,60 ,60 ,0,true),
                    CNCUtils.AccurateColor(67 ,230,57 ,0,true),
                    CNCUtils.AccurateColor(53 ,157,255,0,true),
                    CNCUtils.AccurateColor(224,44 ,230,0,true),
                    CNCUtils.AccurateColor(255,255,255,0,true),
                    CNCUtils.AccurateColor(255,196,85 ,0,true),
                    CNCUtils.AccurateColor(179,175,50 ,0,true),
                    CNCUtils.AccurateColor(139,29 ,242,0,true),
            };
            int type = 0;
            text = text.Replace("[Damage", "[damage");
            if (text.IndexOf("[damage") < 0) return;
            int _begin = text.IndexOf("[damage");
            int _end = _begin + text.Substring(_begin).IndexOf("=");
            for (int j = _begin + "[damage".Length; j < _end; j++) if ('0' <= text[j] && text[j] <= '9') type = type * 10 + text[j] - '0';
            if (text.IndexOf($"[damage{type}=") < 0) return;
            int begin = text.IndexOf($"[damage{type}=");
            if (text.Substring(begin).IndexOf("]") < 0) return;
            int end = begin + text.Substring(begin).IndexOf("]");
            int index = $"[damage{type}=".Length;
            int damage = 0;
            for (int j = begin + index; j < end; j++) if ('0' <= text[j] && text[j] <= '9') damage = damage * 10 + text[j] - '0';
            if (text[end - 1] == 'f') damage = (int)(item.OriginalDamage * (damage * .01f));
            float basicDamage = (damage + (damage * ip.spellDamageAdd)) * ip.spellDamageMul;
            float extraDamage = basicDamage * (damageAdd[0] + damageAdd[type] - 2) * (damageMul[0] * damageMul[type]);
            text = text.Substring(0, begin) + ColorDamage((int)basicDamage, (int)extraDamage, damageColor[type]) + text.Substring(end + 1);
        }

        public static void GetFirstDamageCommon(ref string text)
        {
            int type = 0;
            text = text.Replace("[Damage", "[damage");
            if (text.IndexOf("[damage") < 0) return;
            int _begin = text.IndexOf("[damage");
            int _end = _begin + text.Substring(_begin).IndexOf("=");
            for (int j = _begin + "[damage".Length; j < _end; j++) if ('0' <= text[j] && text[j] <= '9') type = type * 10 + text[j] - '0';
            if (text.IndexOf($"[damage{type}=") < 0) return;
            int begin = text.IndexOf($"[damage{type}=");
            if (text.Substring(begin).IndexOf("]") < 0) return;
            int end = begin + text.Substring(begin).IndexOf("]");
            int index = $"[damage{type}=".Length;
            int damage = 0;
            string extra = "";
            for (int j = begin + index; j < end; j++) if ('0' <= text[j] && text[j] <= '9') damage = damage * 10 + text[j] - '0';
            if (text[end - 1] == 'f') extra = "%";
            text = text.Substring(0, begin) + damage + extra + text.Substring(end + 1);
        }

        public static string ColorDamage(int basicDamage, int extraDamage, Color color)
        {
            string colorStr = "";
            int[] num = new int[8];
            byte[] ARGB = new byte[4] { color.A, color.R, color.G, color.B };
            char[] hex = new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            for (int i = 0; i < 8; i++) num[i] = Math.Abs(ARGB[i / 2] / (int)Math.Pow(16, (i + 1) % 2) % 16);
            for (int i = 0; i < 8; i++) colorStr += hex[num[i]];
            string damageText = string.Format("({0}{1})", (extraDamage < 0 ? "-" : "+"), (int)extraDamage);
            string colorText = string.Format($"[c/{colorStr}:{"{0}"}]", (int)basicDamage);
            return colorText + damageText;
        }
    }
}