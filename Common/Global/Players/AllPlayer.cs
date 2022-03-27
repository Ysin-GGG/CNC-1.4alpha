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