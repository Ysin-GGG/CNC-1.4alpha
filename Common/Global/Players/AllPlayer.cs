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
        private bool shake => this.shakeTime > 0 && this.shakeValue == Vector2.Zero;
        private int shakeTime;
        /// <summary> 震动数值 </summary>
        private Vector2 shakeValue;
        /// <summary> 震动方向 </summary>
        private Vector2 shakeDirection;
        /// <summary> 震动频率 </summary>
        private Vector2 shakeFrequency;
        /// <summary> 震动幅度 </summary>
        private Vector2 shakeAmplitude;
        /// <summary> 震动幅度变化倍数（取值区间为(0, 1)） </summary>
        private Vector2 shakeAmplitudeMult;
        private Vector2 instance;

        public bool ZoneTranquility => zone[0];

        public int Tick => _tick;
        private int _tick = 0;
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
            for (int i = 0; i < CDReduction.Length; i++) CDReduction[i] = 1f;
            for (int i = 0; i < buff.Length; i++) buff[i] = false;
            this.shakeAmplitudeMult = Vector2.One;

        }

        public void ScreenShake(int time, float frequency, float amplitude, bool vertical = true)
            => ScreenShake(time, frequency * new Vector2((!vertical).ToInt(), (vertical).ToInt()), amplitude * new Vector2((!vertical).ToInt(), (vertical).ToInt()));

        /// <summary> 屏幕震动 </summary>
        /// <param name="time"> 震动时长 </param>
        /// <param name="frequency"> 震动频率 </param>
        /// <param name="amplitude"> 震动幅度 </param>
        public void ScreenShake(int time, Vector2 frequency, Vector2 amplitude)
        {
            this.shakeTime = time;
            this.shakeDirection = new Vector2(amplitude.X < 0 ? -1 : 1, amplitude.Y < 0 ? -1 : 1);
            this.shakeFrequency = frequency;
            this.shakeAmplitude = amplitude * this.shakeAmplitudeMult;
            this.shakeValue = Vector2.Zero;
        }

        private void UpdateShakeData()
        {
        }

        public override void ModifyScreenPosition()
        {
            if (this.shake) Main.screenPosition += this.shakeValue;
        }

        public override void PostUpdate()
        {
            if (this.shake)
            {
                Vector2 amplitude = new Vector2(Math.Abs(this.shakeAmplitude.X), Math.Abs(this.shakeAmplitude.Y));
                Vector2 shakeSpeed = this.shakeAmplitude / this.shakeFrequency / 2f;
                this.shakeValue += this.shakeDirection * shakeSpeed;
                this.shakeTime = MyMath.Max(this.shakeTime - 1, 0);
                if (this.shakeValue.X >= amplitude.X) { this.shakeDirection.X = -1; this.shakeValue.X = amplitude.X; }
                if (this.shakeValue.X <= amplitude.X) { this.shakeDirection.X = 1; this.shakeValue.X = -amplitude.X; }
                if (this.shakeValue.Y >= amplitude.Y) { this.shakeDirection.Y = -1; this.shakeValue.Y = amplitude.Y; }
                if (this.shakeValue.Y <= amplitude.Y) { this.shakeDirection.Y = 1; this.shakeValue.Y = -amplitude.Y; }
            }
        }
    }
}