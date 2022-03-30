
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.IceTooth
{
    public class FrostWave : CNCProjectile
    {
        public Vector2 center => data.ContainsKey("center") ? (Vector2)data["center"] : -Vector2.One;
        private int stage = 0;

        public override void ResetDefaults()
        {
            data["center"] = -Vector2.One;
            stage = 0;
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 5;
            projectile.knockBack = 0f;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int buff1 = CNCID.Buff.LegacyObject[CNCID.Buff.Gelid];
            int buff2 = CNCID.Buff.LegacyObject[CNCID.Buff.Glacier];
        }

        public override void AI()
        {
            if (this.stage == 0)//Äý¾Û
            {
                projectile.scale = 0f;
                if (projectile.timeLeft <= 1)
                {
                    projectile.timeLeft = 15;
                    this.stage = 1;
                }
            }
            if (this.stage == 1)//±¬·¢
            {
                for (int i = 0; i < 3; i++)
                {
                    int idx = Particle.NewParticle(
                                 ModContent.Request<Texture2D>("CNC/Assets/Effects/Snowflake" + Main.rand.Next(1, 5).ToString()).Value,
                                 projectile.Center,
                                 new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)) * 6f,
                                 MyMath.deg01 * Main.rand.Next(359),
                                 500,
                                 Vector2.One * .1f,
                                 new Color(112, 196, 255, 0), //new Color(178, 214, 231, 0);
                                 new Color(0, 170, 255, 5)
                                 );
                    ParticleRenderer.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.001f, .002f));
                    ParticleRenderer.particle[idx].rotationAdd = Main.rand.NextFloat(0, MyMath.deg30);
                    ParticleRenderer.particle[idx].velocityMul = new Vector2(.975f);
                }
                projectile.friendly = true;
                projectile.scale = (15 - projectile.timeLeft) / 15f;
                projectile.Size = Vector2.One * 14 * 16 * projectile.scale * 2;
                if (projectile.timeLeft <= 1)
                {
                    projectile.timeLeft = 180;
                    this.stage = 2;
                }
            }
            if (this.stage == 2)//¹Ì¶¨
            {
                projectile.friendly = false;
                projectile.scale = 1f;
                if(projectile.timeLeft == 60) SoundEngine.PlaySound(SoundID.Item27, projectile.Center);
                if (projectile.timeLeft <= 1)
                {
                    SoundEngine.PlaySound(SoundID.Item107, projectile.Center);
                    CNCUtils.PlaySound("IceTooth/Hit", projectile.position);
                    for (int i = 0; i < 60; i++)
                    {
                        int idx = Particle.NewParticle(
                                     ModContent.Request<Texture2D>("CNC/Assets/Effects/Snowflake" + Main.rand.Next(1, 5).ToString()).Value,
                                     projectile.Center + new Vector2(projectile.width / 2f, 0).RotatedBy(i / 60f * MyMath.pi * 2),
                                     new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, 1f)),
                                     Main.rand.NextFloat(MyMath.pi2),
                                     500,
                                     Vector2.One * .09f,
                                     new Color(112, 196, 255, 0),
                                     new Color(0, 170, 255, 5),
                                     false
                                     );
                        ParticleRenderer.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0005f, .0010f));
                        ParticleRenderer.particle[idx].rotationAdd = Main.rand.NextFloat(0, MyMath.deg30 / 10f);
                    }
                    for (int i = 0; i < 90; i++)
                    {
                        float rot = MyMath.deg01;
                        Vector2[] offset = new Vector2[]
                        {
                            MyMath.GetXY(-37f * rot) * .73f,
                            MyMath.GetXY(-84f * rot) * .45f,
                            MyMath.GetXY(-116f * rot) * .62f,
                            MyMath.GetXY(-162f * rot) * .42f,
                            MyMath.GetXY(-200f * rot) * .58f,
                            MyMath.GetXY(-232f * rot) * .80f,
                            MyMath.GetXY(-285f * rot) * .38f,
                            MyMath.GetXY(-311f * rot) * .63f,
                            MyMath.GetXY(-340f * rot) * .52f,
                        };
                        int idx = Particle.NewParticle(
                                     ModContent.Request<Texture2D>("CNC/Assets/Effects/Snowflake" + Main.rand.Next(1, 5).ToString()).Value,
                                     projectile.Center + projectile.width / 2f * offset[i / 10] * (i % 10 + 1) / 10f,
                                     Vector2.Zero,
                                     Main.rand.NextFloat(MyMath.pi2),
                                     500,
                                     Vector2.One * (.06f * (10 - (i % 10 + 1)) / 10f + .02f),
                                     new Color(112, 196, 255, 0),
                                     new Color(0, 170, 255, 5),
                                     true
                                     );
                        ParticleRenderer.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0005f, .0009f));
                        ParticleRenderer.particle[idx].rotationAdd = Main.rand.NextFloat(0, MyMath.deg30 / 10f);
                        ParticleRenderer.particle[idx].velocityAdd = new Vector2(Main.rand.NextFloat(-.03f, .03f), Main.rand.NextFloat(-.03f, .03f));
                    }
                }
            }
            projectile.Center = (Vector2)data["center"];
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool PreDrawExtras()
        {
            Texture2D image1 = ModContent.Request<Texture2D>("CNC/Content/Projectiles/IceTooth/IceAura").Value;
            Texture2D image2 = ModContent.Request<Texture2D>("CNC/Content/Projectiles/IceTooth/IceCrack").Value;
            Texture2D image3 = ModContent.Request<Texture2D>("CNC/Content/Projectiles/IceTooth/FrostWave").Value;

            Vector2 size1 = new Vector2(500) * projectile.scale;
            Vector2 size2 = new Vector2(320) * projectile.scale;
            Vector2 size3 = new Vector2(500) * projectile.scale;
            Vector2 pos1 = projectile.Center - size1 / 2 - Main.screenPosition;
            Vector2 pos2 = projectile.Center - size2 / 2 - Main.screenPosition;
            Vector2 pos3 = projectile.Center - size3 / 2 - Main.screenPosition;
            Rectangle rect1 = new Rectangle((int)pos1.X, (int)pos1.Y, (int)size1.X, (int)size1.Y);
            Rectangle rect2 = new Rectangle((int)pos2.X, (int)pos2.Y, (int)size2.X, (int)size2.Y);
            Rectangle rect3 = new Rectangle((int)pos3.X, (int)pos3.Y, (int)size3.X, (int)size3.Y);
            Color color = new Color(112, 196, 255, 0);
            if (stage == 2) Main.spriteBatch.Draw(image1, rect1, color);
            if (stage == 2 && projectile.timeLeft < 60) Main.spriteBatch.Draw(image2, rect2, color);
            if (stage == 1) Main.spriteBatch.Draw(image3, rect3, color);
            return false;
        }
    }
}