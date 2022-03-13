using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using CNC.Common.Utils;

namespace CNC.Common.Global.Players
{
    public class AllPlayer : ModPlayer
    {
        Player player => Player;
        AllPlayer modPlayer => player.GetModPlayer<AllPlayer>();
        public int overloaded_count;

        public int lifeRegen;
        public int lifeRegenCount;
        
        public bool ZoneTranquility => zone[0];

        public int Tick => _tick;
        private int _tick = 0;
        public int[] spellCD = new int[999];
        public float[] CDReduction = new float[9];
        public float manaReturn = 0;
        public bool[] buff = new bool[CNCID.Buff.MaxObject];
        public bool[] zone = new bool[1];
        public bool IsModDeveloper => name.Contains(player.name);
        public bool EquippedQuartzHead => player.armor[0].type == ModContent.ItemType<Content.Items.Developers.Quartz.QuartzBlock>();
        public string[] developers => name.ToArray();

        private List<string> name = new List<string>()
        {
            "Quartz", "quartz",
            "HHvs",
        };

        public override void ResetEffects()
        {
            manaReturn = 0;
            _tick = (_tick + 1) % 60;
            for (int i = 0; i < spellCD.Length; i++) spellCD[i]--;
            for (int i = 0; i < CDReduction.Length; i++) CDReduction[i] = 1f;
            for (int i = 0; i < buff.Length; i++) buff[i] = false;
        }
    }
}