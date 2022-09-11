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

        public int lifeRegen;
        public int lifeRegenCount;
        /// <summary> 震动数值 </summary>
        public Vector2 shakeValue;
        /// <summary> 震动方向 </summary>
        private Vector2 shakeDirection;
        /// <summary> 震动频率 </summary>
        private Vector2 shakeFrequency;
        /// <summary> 震动幅度 </summary>
        private Vector2 shakeAmplitude;
        /// <summary> 震动持续时长 </summary>
        private int shakeTime;
        /// <summary> 震动衰减程度 </summary>
        public float shakeCur;
        /// <summary> 震动衰减速度 </summary>
        private float shakeAttenuation;
        private Vector2 instance;

        public bool ZoneTranquility => zone[0];

        public int Tick { get; private set; } = 0;

        public float[] CDReduction = new float[9];
        public float manaReturn = 0;
        public float dealtDamage { get => player.GetModPlayer<ClassesSettings>().dealtDamage; set => player.GetModPlayer<ClassesSettings>().dealtDamage = value; }
        public float takenDamage { get => player.GetModPlayer<ClassesSettings>().takenDamage; set => player.GetModPlayer<ClassesSettings>().takenDamage = value; }
        public float takenDamageFromEnemies { get => player.GetModPlayer<ClassesSettings>().takenDamageFromEnemies; set => player.GetModPlayer<ClassesSettings>().takenDamageFromEnemies = value; }
        public bool[] buff = new bool[CNCID.Buff.MaxObject];
        public bool[] zone = new bool[1];
        public bool IsModDeveloper => name.Contains(player.name);
        public bool EquippedQuartzHead => player.armor[0].type == ModContent.ItemType<Content.Items.Developers.Quartz.QuartzBlock>();
        public string[] developers => name.ToArray();
        public int[] bleeding = new int[6];

        private List<string> name = new List<string>()
        {
            "Quartz", "quartz",
            "HHvs",
        };

        public override void ResetEffects()
        {
            manaReturn = 0;
            Tick = (Tick + 1) % 60;
            for (int i = 0; i < CDReduction.Length; i++) CDReduction[i] = 1f;
            for (int i = 0; i < buff.Length; i++) buff[i] = false;

        }

        /// <summary> 屏幕震动 </summary>
        /// <param name="time"> 震动时长 </param>
        /// <param name="frequency"> 震动速率 </param>
        /// <param name="amplitude"> 震动幅度 </param>
        /// <param name="attenuation"> 震动衰减速度 </param>
        /// <param name="startOffset"> 初始震动偏移 </param>
        public void ScreenShake(int time, Vector2 frequency, Vector2 amplitude, float attenuation, Vector2 startOffset = default)
        {
            this.shakeTime = time;
            this.shakeDirection = new Vector2(amplitude.X < 0 ? -1 : 1, amplitude.Y < 0 ? -1 : 1);
            this.shakeFrequency = frequency;
            this.shakeValue = startOffset;
            this.shakeAmplitude = amplitude;
            this.shakeAttenuation = attenuation;
            this.shakeCur = 1f;
        }

        private void UpdateShakeData()
        {
            Vector2 amplitude = new Vector2(Math.Abs(this.shakeAmplitude.X), Math.Abs(this.shakeAmplitude.Y));
            if (this.shakeTime-- <= 0) this.shakeCur *= this.shakeCur < .01f ? 0f : this.shakeAttenuation;
            if (this.shakeValue.X <= -amplitude.X) this.shakeDirection.X = 1;
            if (this.shakeValue.X >= amplitude.X) this.shakeDirection.X = -1;
            if (this.shakeValue.Y <= -amplitude.Y) this.shakeDirection.Y = 1;
            if (this.shakeValue.Y >= amplitude.Y) this.shakeDirection.Y = -1;
            this.shakeValue.X = MyMath.Clamp(this.shakeValue.X + this.shakeDirection.X * this.shakeFrequency.X, -amplitude.X, amplitude.X);
            this.shakeValue.Y = MyMath.Clamp(this.shakeValue.Y + this.shakeDirection.Y * this.shakeFrequency.Y, -amplitude.Y, amplitude.Y);
        }

        public override void ModifyScreenPosition()
        {
            if (this.shakeCur > 0) Main.screenPosition += this.shakeValue * this.shakeCur;
        }

        public override void PostUpdate()
        {
            if (this.shakeCur > 0) this.UpdateShakeData();

        }
    }
}