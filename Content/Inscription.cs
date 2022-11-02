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
        public int cooldown;
        public int defCooldown;//预设冷却
        public object[] Data { get; private set; } = new object[99];

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
                this.cooldown = 0;
                this.defCooldown = 0;
                this.Data = new object[0];
                return;
            }
            this.type = type;
            this.style = style;
            this._name = "Inscription." + CNCID.Inscription.Collection[new Point(type, style)];
            this._name = this._name.Replace("Inscription.C_", "Inscription.Common.");
            this._name = this._name.Replace("Inscription.S_", "Inscription.Special.");
            string[] tt = CNCLocalization.GetText(_name).Split('|');
            this._name = CNCID.Inscription.Collection[new Point(type, style)];
            this.cooldown = 0;
            this.defCooldown = 0;
            this.displayName = tt[0];
            this.description = tt[1];
            switch (style)
            {
                case 0:
                    if (type == 0) { this.Data = new object[] { 0.20f }; }
                    if (type == 1) { this.Data = new object[] { 0.25f }; }
                    if (type == 2) { this.Data = new object[] { 0.20f }; }
                    if (type == 3) { this.Data = new object[] { 0.13f }; }
                    if (type == 4) { this.Data = new object[] { 0.20f }; }
                    if (type == 5) { this.Data = new object[] { 0.25f }; }
                    if (type == 6) { this.Data = new object[] { 0.20f }; }
                    if (type == 7) { this.Data = new object[] { 0.20f }; }
                    if (type == 8) { this.Data = new object[] { 0.25f }; }
                    if (type == 9) { this.Data = new object[] { 0.20f }; }
                    if (type == 10) { this.Data = new object[] { 0.15f, 0.35f }; }
                    if (type == 11) { this.Data = new object[] { 0.10f }; }
                    if (type == 12) { this.Data = new object[] { 0.25f }; }
                    if (type == 13) { this.Data = new object[] { 0.20f }; }
                    if (type == 14) { this.Data = new object[] { 20 }; }
                    if (type == 15) { this.Data = new object[] { }; }
                    if (type == 16) { this.Data = new object[] { 0.40f }; }
                    if (type == 17) { this.Data = new object[] { 1 }; }
                    if (type == 18) { this.Data = new object[] { 0.30f }; }
                    if (type == 19) { this.Data = new object[] { 0.15f }; }
                    if (type == 20) { this.Data = new object[] { 0.30f, 0.30f }; }
                    if (type == 21) { this.Data = new object[] { 0.27f }; }
                    if (type == 22) { this.Data = new object[] { 0.17f }; }
                    if (type == 23) { this.Data = new object[] { 0.10f, 0.50f }; }
                    if (type == 24) { this.Data = new object[] { 0.35f }; }
                    if (type == 25) { this.Data = new object[] { 0.20f }; }
                    break;
                case CNCID.Item.FireGrimoire:
                    if (type == 0) { this.Data = new object[] { }; }
                    if (type == 1) { this.Data = new object[] { "[damage3=36]" }; }
                    if (type == 2) { this.Data = new object[] { 8 }; }
                    if (type == 3) { this.Data = new object[] { 0.05f }; }
                    break;
                case CNCID.Item.IceTooth:
                    if (type == 0) { this.Data = new object[] { }; }
                    if (type == 1) { this.Data = new object[] { }; }
                    if (type == 2) { this.Data = new object[] { "[damage3=30]" }; }
                    if (type == 3) { this.Data = new object[] { 0.40f }; }
                    if (type == 4) { this.Data = new object[] { 10, 14, "[damage3=80]", 15, 10 }; this.defCooldown = 600; }
                    break;
                case CNCID.Item.Superconductor:
                    if (type == 0) { this.Data = new object[] { }; }
                    if (type == 1) { this.Data = new object[] { 0.100f, 5, 0.010f, 0.006f, 0.350f }; }
                    if (type == 2) { this.Data = new object[] { 1, 8, 5, 0.07f, 1, 6 }; }
                    if (type == 3) { this.Data = new object[] { 4, 4, 45, "[damage3=240]" }; this.defCooldown = 300; }
                    if (type == 4) { this.Data = new object[] { 3, 4, 45, "[damage3=310]" }; this.defCooldown = 240; }
                    if (type == 5) { this.Data = new object[] { 0.10f, "[damage3=105]", 0.010f, 0.004f, 0.200f }; }
                    if (type == 6) { this.Data = new object[] { 5, 50, 1 }; }
                    if (type == 7) { this.Data = new object[] { 0.500f, 3 }; }
                    if (type == 8) { this.Data = new object[] { 1, 30, 5, 0.300f }; }
                    if (type == 9) { this.Data = new object[] { 0.400f }; }
                    if (type == 10) { this.Data = new object[] { 0.150f, 2, 0.010f, 0.006f, 0.350f }; }
                    break;
                case CNCID.Item.ArgenteusClaw:
                    if (type == 0) { this.Data = new object[] { }; }
                    if (type == 1) { this.Data = new object[] { 0.45f, "[damage2=200f]" }; }
                    if (type == 2) { this.Data = new object[] { 3, "[damage2=300f]" }; }
                    break;
                case CNCID.Item.ArgenteusOathKeeper:
                    if (type == 0) { this.Data = new object[] { 0.60f, "[damage3=90]" }; }
                    if (type == 1) { this.Data = new object[] { 20, 5, 100, "[damage3=200]" }; }
                    if (type == 2) { this.Data = new object[] { 2, "[damage1=150f]" }; this.defCooldown = 120; }
                    if (type == 3) { this.Data = new object[] { 6, 8, 0.15f, 0.15f, 10 }; this.defCooldown = 1200; }
                    break;
                case CNCID.Item.TheLightOfAuzzsia:
                    if (type == 0) { this.Data = new object[] { "[damage3=50]" }; }
                    if (type == 1) { this.Data = new object[] { }; }
                    if (type == 2) { this.Data = new object[] { "[damage3=30]" }; }
                    break;
                case CNCID.Item.EmberOfNien:
                    if (type == 0) { this.Data = new object[] { 0.10f, 1 }; }
                    if (type == 1) { this.Data = new object[] { 6, 6 }; }
                    if (type == 2) { this.Data = new object[] { 5, 5, 0.10f }; }
                    break;
                case CNCID.Item.FragmentarySoulDagger:
                    if (type == 0) { this.Data = new object[] { 10, 8, 8, 2.00f }; }
                    if (type == 1) { this.Data = new object[] { 3, 0.10f, 4 }; }
                    if (type == 2) { this.Data = new object[] { 6 }; }
                    break;
            }
        }

        /// <summary> 获取铭文描述 </summary>
        /// <param name="args">数据参数</param>
        private string GetDesc(object[] args)
        {
            try
            {
                if (args == null || args.Length <= 0) return this.description;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is float arg1) args[i] = ((float)Math.Round(arg1 * 100f, 1)).ToString() + '%';
                    else if (args[i] is int arg2) args[i] = arg2.ToString();
                    else if (args[i] is string arg3) args[i] = arg3;
                }
                return string.Format(this.description, args);
            }
            catch
            {
                throw new Exception($"Invalid inscription: type = {type}, style = {style}");
            }
        }

        /// <summary> 设置冷却时间 </summary>
        /// <param name="newCooldown">冷却</param>
        /// <param name="newCooldownMax">新预设冷却</param>
        /// <param name="add">追加倍数</param>
        /// <param name="mul">追乘倍数</param>
        public void SetCooldown(int newCooldown = -1, int newCooldownMax = -1, float add = 0f, float mul = 1f)
        {
            if (newCooldownMax >= 0) this.defCooldown = newCooldownMax;
            if (newCooldown < 0) newCooldown = this.defCooldown;
            this.cooldown = (int)((newCooldown  / (1 + add)) * mul);
        }

        /// <summary> 铭文描述转物品描述 </summary>
        /// <param name="data">数据参数</param>
        public string GetTooltip(object[] data = default)
        {
            string tt = GetDesc(data == default ? this.Data : data);
            return tt;
        }

        /// <summary> 将文本的伤害表达式转成具体数字 </summary>
        /// <param name="text">要读取的文本</param>
        /// <param name="item">目标物品</param>
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
                    CNCUtils.FlashColor(0  ,0  ,0  ,0,true),
                    CNCUtils.FlashColor(217,60 ,60 ,0,true),
                    CNCUtils.FlashColor(67 ,230,57 ,0,true),
                    CNCUtils.FlashColor(53 ,157,255,0,true),
                    CNCUtils.FlashColor(224,44 ,230,0,true),
                    CNCUtils.FlashColor(255,255,255,0,true),
                    CNCUtils.FlashColor(255,196,85 ,0,true),
                    CNCUtils.FlashColor(179,175,50 ,0,true),
                    CNCUtils.FlashColor(139,29 ,242,0,true),
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
            float basicDamage = (damage + (damage * (ip.skillDamageAdd + i.skillDamageAdd))) * ip.skillDamageMul * i.skillDamageMul;
            float extraDamage = basicDamage * (damageAdd[0] + damageAdd[type] - 2) * (damageMul[0] * damageMul[type]);
            text = text.Substring(0, begin) + ColorDamage((int)basicDamage, (int)extraDamage, damageColor[type]) + text.Substring(end + 1);
        }

        /// <summary> 将文本的伤害表达式转成具体数字 </summary>
        /// <param name="text">要读取的文本</param>
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

        /// <summary> 获取伤害值 </summary>
        /// <param name="originalDamage">百分比伤害的基础伤害值</param>
        /// <param name="arg">伤害表达式</param>
        /// <param name="item">提供加成的物品</param>
        public static int JustGetDamage(int originalDamage, string arg, Item item)
            => JustGetDamage(originalDamage, arg, item.GetGlobalItem<InscriptionSettings>().skillDamageAdd, item.GetGlobalItem<InscriptionSettings>().skillDamageMul);

        /// <summary> 获取伤害值 </summary>
        /// <param name="originalDamage">百分比伤害的基础伤害值</param>
        /// <param name="arg">伤害表达式</param>
        /// <param name="add">追加倍数</param>
        /// <param name="mul">追乘倍数</param>
        public static int JustGetDamage(int originalDamage, string arg, float add = 0f, float mul = 1f)
        {
            Player p = Main.LocalPlayer;
            InscriptionPlayer ip = p.GetModPlayer<InscriptionPlayer>();
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
                    CNCUtils.FlashColor(0  ,0  ,0  ,0,true),
                    CNCUtils.FlashColor(217,60 ,60 ,0,true),
                    CNCUtils.FlashColor(67 ,230,57 ,0,true),
                    CNCUtils.FlashColor(53 ,157,255,0,true),
                    CNCUtils.FlashColor(224,44 ,230,0,true),
                    CNCUtils.FlashColor(255,255,255,0,true),
                    CNCUtils.FlashColor(255,196,85 ,0,true),
                    CNCUtils.FlashColor(179,175,50 ,0,true),
                    CNCUtils.FlashColor(139,29 ,242,0,true),
            };
            int type = 0;
            arg = arg.Replace("[Damage", "[damage");
            if (arg.IndexOf("[damage") < 0) return 0;
            int _begin = arg.IndexOf("[damage");
            int _end = _begin + arg.Substring(_begin).IndexOf("=");
            for (int j = _begin + "[damage".Length; j < _end; j++) if ('0' <= arg[j] && arg[j] <= '9') type = type * 10 + arg[j] - '0';
            int damage = 0;
            int begin = arg.IndexOf('=');
            int end = arg.IndexOf(']');
            for (int j = begin; j < end; j++) if ('0' <= arg[j] && arg[j] <= '9') damage = damage * 10 + arg[j] - '0';
            if (arg[end - 1] == 'f') damage = (int)(originalDamage * (damage * .01f));
            float basicDamage = (damage + (damage * (ip.skillDamageAdd + add))) * ip.skillDamageMul * mul;
            float extraDamage = basicDamage * (damageAdd[0] + damageAdd[type] - 2) * (damageMul[0] * damageMul[type]);
            return (int)(basicDamage + extraDamage);
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

        public override bool Equals(object obj)
        {
            if (obj is Inscription other) return this.type == other.type && this.style == other.style;
            return base.Equals(obj);
        }

        public override int GetHashCode() => new { type, style }.GetHashCode();
        
        public static bool operator ==(Inscription i1, Inscription i2) => i1.Equals(i2);
        public static bool operator !=(Inscription i1, Inscription i2) => !i1.Equals(i2);
    }
}