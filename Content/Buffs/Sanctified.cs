using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using CNC.Common.Utils;
using CNC.Common.Global.Buffs;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;

namespace CNC.Content.Buffs
{
    public class Sanctified : CNCBuff
    {
        public override void ResetStaticDefaults()
        {
            noClearedByNurse = true;
            longer = false;
            debuff = true;
            noSave = true;
            noTimeDisplay = true;
            pvp = false;
            //killText = CNC.ReadJson("DeathText.Sanctified");
            damage = 6;
        }

        public override void Update(Player target, AllPlayer gt, ref int index)
        {
            //gt.Sanctified = true;
            gt.lifeRegen -= 30;
            //if (CNC.Random.NextBool((target.width + target.height) / 2)) Effect(target, 1);
        }

        public override void Update(NPC target, AllNPC gt, ref int index)
        {
            //gt.Sanctified = true;
            gt.lifeRegen -= 30;
            //if (CNC.Random.NextBool((target.width + target.height) / 2)) Effect(target, 1);
        }

        public override void ModifyDamage(Player target, AllPlayer gt, ref int index)
        {
            Effect(target);
        }

        public override void ModifyDamage(NPC target, AllNPC gt, ref int index)
        {
            Effect(target);
        }

        private void Effect(Entity target, int type = 0)
        {
            Color color = Color.PaleGoldenrod; color.A = 0;
            if (type == 1)
            {
                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                    target.position + CNC.Random.NextVector2(target.Size),
                    CNC.Random.NextVector2(-3f, 0, 3f, 0),
                    CNC.Random.NextDegree(),
                    20,
                    Vector2.One,
                    color
                    );
                ParticleRenderer.particle[idx].velocityMul = CNC.Random.NextVector2(.9f, 1f, .95f, 1f);
                ParticleRenderer.particle[idx].velocityAdd = CNC.Random.NextVector2(0, -1f, 0, -.5f);
                ParticleRenderer.particle[idx].rotationAdd = CNC.Random.NextDegree(-8f, 8f);
                ParticleRenderer.particle[idx].scaleAdd = Vector2.One * CNC.Random.NextFloat(-.05f);
            }
            else
            {
                float randRot = CNC.Random.NextDegree();
                for (int i = 0; i < 32; i++)
                {
                    float spd = i % 8 + 1f;
                    int idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                        target.Center,
                        new Vector2(0, (i / 8 % 2 == 0 ? MyMath.phi : 1f) * spd * 1.25f).RotatedBy(MyMath.deg90 * (i / 8) + randRot),
                        CNC.Random.NextDegree(),
                        20,
                        Vector2.One,
                        color
                        );
                    ParticleRenderer.particle[idx].velocityMul = Vector2.One * (((1 - spd / 8f) * .08f) + .92f);
                    ParticleRenderer.particle[idx].rotationAdd = CNC.Random.NextDegree(-8f, 8f);
                    ParticleRenderer.particle[idx].scaleAdd = Vector2.One * CNC.Random.NextFloat(spd * -.005f);
                    ParticleRenderer.particle[idx].alphaAdd = -.05f;
                    ParticleRenderer.particle[idx].alpha = 1f;
                }
            }
        }
    }
}