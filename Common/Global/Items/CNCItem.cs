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

namespace CNC.Common.Global.Items
{
    public abstract class CNCItem : ModItem
    {
        public AllItem allItem => item.GetGlobalItem<AllItem>();
        public Item item => Item;
        public Mod mod => Mod;

        protected Color u => new Color(255, 255, 255, 255);
        protected Color n => new Color(64, 64, 64, 255);
        
        public byte clazz { get => allItem.clazz; set => allItem.clazz = value; }

        public bool staff { get => Item.staff[Type]; set => Item.staff[Type] = value; }
        public virtual string[] attributeKeys => new string[0];
        public virtual int countFrame => 1;
        public virtual int ticksPerFrame => 30;
        public virtual int researchAmount => 1;

        public sealed override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new Terraria.DataStructures.DrawAnimationVertical(ticksPerFrame, countFrame));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = researchAmount;
            DisplayName.SetDefault(CNCLocalization.GetText($"Item.Name.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Item.Name.{Name}", 7));
            if (CNCLocalization.GetText($"Item.Tooltip.{Name}") != "\0" && CNCLocalization.GetText($"Item.Tooltip.{Name}") != CNCLocalization.ModKey + $"Item.Tooltip.{Name}")
            {
                Tooltip.SetDefault(CNCLocalization.GetText($"Item.Tooltip.{Name}", 1));
                Tooltip.AddTranslation(7, CNCLocalization.GetText($"Item.Tooltip.{Name}", 7));
            }
            ResetStaticDefaults();
        }

        public sealed override void SetDefaults() => ResetDefaults();

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips) => Tooltips(tooltips);

        public override void AddRecipes()
        {
            //CreateRecipe().Register();
        }

        public virtual void ResetDefaults() { }
        public virtual void ResetStaticDefaults() { }
        public virtual void Tooltips(List<TooltipLine> tooltips) { }
        /*
        protected string GetAttribute(string name)
            => CNCLocalization.GetText($"Attribute.Name.{name}") + (CNCLocalization.isChinese ? "：" : ": ") + CNCLocalization.GetText($"Attribute.Desc.{name}");
        
        protected string GetAttribute(string name, Item item, int level)
        {
            string[] Split(string _str, string _key)
            {
                List<string> res = new List<string>();
                int left = 0;
                int right = _str.IndexOf(_key);
                while (right >= 0)
                {
                    res.Add(_str.Substring(left, right - left));
                    left = right + _key.Length;
                    right = _str.IndexOf(_key, left + 1);
                }
                if (right < 0) res.Add(_str.Substring(left));
                return res.ToArray();
            }
            float GetDamage(string _str)
            {
                float res = _str.Length == 0 ? -1f : 0;
                for (int i = 0; i < _str.Length && res >= 0; i++)
                    if ('0' <= _str[i] && _str[i] <= '9') res = res * 10 + _str[i] - '0';
                    else res = -1f;
                return res;
            }
            string ColorToHex(Color _color)
            {
                string res = "";
                int[] num = new int[8];
                byte[] ARGB = new byte[4] { _color.A, _color.R, _color.G, _color.B };
                string[] hex = new string[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
                for (int i = 0; i < 8; i++) num[i] = Math.Abs(ARGB[i / 2] / (int)Math.Pow(16, (i + 1) % 2) % 16);
                for (int i = 0; i < 8; i++) res += hex[num[i]];
                return res;
            }
            string ColorDamageText(Item _item, float _damage, float _percentage = 1f)
            {
                int dmgType = _item.DamageType.Type - 1;
                string color = ColorToHex(DamageClassColor(dmgType));
                Player p = Main.LocalPlayer;
                AllItem ai = _item.GetGlobalItem<AllItem>();
                float[] playerDamageAdd = new float[]
                {
                    p.GetDamage(DamageClass.Generic).Additive,
                    p.GetDamage(DamageClass.Melee).Additive,
                    p.GetDamage(DamageClass.Ranged).Additive,
                    p.GetDamage(DamageClass.Magic).Additive,
                    p.GetDamage(DamageClass.Summon).Additive,
                    p.GetDamage(DamageClass.Throwing).Additive,
                };
                float[] playerDamageMult = new float[]
                {
                    p.GetDamage(DamageClass.Generic).Multiplicative,
                    p.GetDamage(DamageClass.Melee).Multiplicative,
                    p.GetDamage(DamageClass.Ranged).Multiplicative,
                    p.GetDamage(DamageClass.Magic).Multiplicative,
                    p.GetDamage(DamageClass.Summon).Multiplicative,
                    p.GetDamage(DamageClass.Throwing).Multiplicative,
                };
                if (ai.levelExist && ai.level < level) color = ColorToHex(n);
                int weaponDamage = (int)_damage;
                float dmgMul = (playerDamageAdd[dmgType] - 1f) * playerDamageMult[dmgType];
                float extraDmg = (p.GetWeaponDamage(item) - _damage) * _percentage;
                float finalDmg = _damage * _percentage + extraDmg;
                string damageText = string.Format(CNCLocalization.isChinese ? "（{0}{1}）" : "({0}{1})", (extraDmg < 0 ? "" : "+"), (int)extraDmg);
                string colorText = string.Format($"[c/{color}:{"{0}"}]", (int)finalDmg);
                return colorText + damageText;
            }

            string text = GetAttribute(name);
            if (item == null || text == CNCLocalization.ModKey + $"Attribute.Name.{name}" + (CNCLocalization.language == 7 ? "：" : ": ") + CNCLocalization.ModKey  + $"Attribute.Name.{name}")
            { return text; }

            text.Replace("[Damage=", "[damage=");
            string key = "[damage=";
            string postfix1 = "]";
            string postfix2 = "f]";
            string[] lines = Split(text, key);
            string _text = lines[0];
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                int idx1 = line.IndexOf(postfix1);
                int idx2 = line.IndexOf(postfix2);
                if (idx1 >= 0 && GetDamage(line.Substring(0, idx1)) >= 0)
                {
                    int dmg = (int)GetDamage(line.Substring(0, idx1));
                    _text += ColorDamageText(item, dmg) + line.Substring(idx1 + postfix1.Length);
                }
                else if (idx2 >= 0 && GetDamage(line.Substring(0, idx2)) >= 0)
                {
                    float dmg = GetDamage(line.Substring(0, idx2));
                    _text += ColorDamageText(item, item.OriginalDamage, dmg) + line.Substring(idx1 + postfix1.Length);
                }
                else _text += key + line;
            }
            return _text;
        }
        */
        protected Color DamageClassColor(int damageType, bool crit = false, bool flash = true)
        {
            Color[] color = new Color[]
            {
                CNCUtils.AccurateColor(0  ,0  ,0  ,0,flash),
                CNCUtils.AccurateColor(217,60 ,60 ,0,flash),
                CNCUtils.AccurateColor(67 ,230,57 ,0,flash),
                CNCUtils.AccurateColor(53 ,157,255,0,flash),
                CNCUtils.AccurateColor(224,44 ,230,0,flash),
                CNCUtils.AccurateColor(255,255,255,0,flash),
                CNCUtils.AccurateColor(255,196,85 ,0,flash),
                CNCUtils.AccurateColor(179,175,50 ,0,flash),
                CNCUtils.AccurateColor(139,29 ,242,0,flash),
            };
            Color[] critColor = new Color[]
            {
                CNCUtils.AccurateColor(0  ,0  ,0  ,0,flash),
                CNCUtils.AccurateColor(163,29 ,29 ,0,flash),
                CNCUtils.AccurateColor(31 ,171,27 ,0,flash),
                CNCUtils.AccurateColor(61 ,128,192,0,flash),
                CNCUtils.AccurateColor(224,44 ,230,0,flash),
                CNCUtils.AccurateColor(255,255,255,0,flash),
                CNCUtils.AccurateColor(255,196,85 ,0,flash),
                CNCUtils.AccurateColor(179,175,50 ,0,flash),
                CNCUtils.AccurateColor(103,43 ,159,0,flash),
            };
            return crit ? critColor[damageType] : color[damageType];
        }
    }
}