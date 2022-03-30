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
        private bool shake => this.shakeTime > 0;
        private int shakeTime;
        private Vector2 shakeValue;
        private Vector2 shakeDirection;
        private Vector2 shakeFrequency;
        private Vector2 shakeAmplitude;
        private Vector2 shakeAmplitudeMul;

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
            this.UpdateShakeData();
            this.shakeAmplitudeMul = Vector2.One;
            if (this.shake) Main.CurrentPan = this.shakeValue;
            if (this.shakeTime > 0) this.shakeTime--;
        }

        public void ScreenShake(int time, float frequency, float amplitude, bool vertical = true)
            => ScreenShake(time, new Vector2(frequency) * new Vector2((!vertical).ToInt(), (vertical).ToInt()), new Vector2(amplitude) * new Vector2((!vertical).ToInt(), (vertical).ToInt()));

        public void ScreenShake(int time, Vector2 frequency, Vector2 amplitude)
        {
            this.shakeTime = time;
            this.shakeDirection = new Vector2(amplitude.X < 0 ? -1 : 1, amplitude.Y < 0 ? -1 : 1);
            this.shakeFrequency = frequency;
            this.shakeAmplitude = amplitude * this.shakeAmplitudeMul;
            this.shakeValue = Vector2.Zero;
        }

        private void UpdateShakeData()
        {
            Vector2 amplitude = new Vector2(Math.Abs(this.shakeAmplitude.X), Math.Abs(this.shakeAmplitude.Y));
            Vector2 shakeSpeed = this.shakeAmplitude / this.shakeFrequency / 2;
            this.shakeValue += this.shakeDirection * shakeSpeed;
            if (this.shakeValue.X >= amplitude.X) { this.shakeDirection.X = -1; this.shakeValue.X = amplitude.X; }
            if (this.shakeValue.X <= amplitude.X) { this.shakeDirection.X = 1; this.shakeValue.X = -amplitude.X; }
            if (this.shakeValue.Y >= amplitude.Y) { this.shakeDirection.Y = -1; this.shakeValue.Y = amplitude.Y; }
            if (this.shakeValue.Y <= amplitude.Y) { this.shakeDirection.Y = 1; this.shakeValue.Y = -amplitude.Y; }
        }

        public override void PreUpdate()
        {
            if (Tick % 5 == 0 && modPlayer.EquippedQuartzHead && modPlayer.IsModDeveloper)
            {
                int idx = Content.Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Content/Dusts/Star").Value,
                    Main.MouseWorld,
                    new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)),
                    0,
                    100,
                    Vector2.One,
                    new Color(178, 214, 231, 0),
                    new Color(178, 214, 231, 10)
                    );
                Content.ParticleRenderer.particle[idx].color = CNCID.Colors.Expert;
                Content.ParticleRenderer.particle[idx].light = CNCID.Colors.Expert;
                Content.ParticleRenderer.particle[idx].scaleAdd = new Vector2(-Main.rand.NextFloat(.065f, .015f));
                Content.ParticleRenderer.particle[idx].rotationAdd = MyMath.deg01 * Main.rand.Next(1, 16);
                Content.ParticleRenderer.particle[idx].color.A = 0;
                Content.ParticleRenderer.particle[idx].light.A = 10;
            }
        }
    }
}