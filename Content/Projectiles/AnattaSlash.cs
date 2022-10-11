using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Content.DataStructures;
using Terraria.DataStructures;

namespace CNC.Content.Projectiles
{
    public class AnattaSlash : ShaderSlash
    {
        private float lightAlpha;
        private float lightRotation;
        private float lightDirection;
        private Vector2 lightScale;
        private Vector2 lightScaleTotal;
        private EffectTimer lightWidth;
        private EffectTimer lightHeight;
        private EffectTimer lightFlash;

        public override void ResetDefaults()
        {
            base.ResetDefaults();
            projectile.DamageType = DamageClass.Melee;
        }

        public override void OnSpawn(IEntitySource source) => this.OnSpawn();

        public override void OnSpawn()
        {
            lightAlpha = 0f;
            lightRotation = 1f;
            lightDirection = 1f;
            lightScale = Vector2.Zero;
            lightScaleTotal = Vector2.One;
            lightWidth = new EffectTimer(48, 1, 0, 48, 80, 10, 10, 4, 0);
            lightHeight = new EffectTimer(48, 1, 0, 48, 64, 10, 10, 2, 0);
            lightFlash = new EffectTimer(127, 1, 0, 127, 255, 0, 0, 16, 0);
            var info = new SlashInfo[300];
            var t = new Texture2D[4];
            var blendStyle = BlendState.Additive;
            var opacity = 1f;
            var time = 10;
            var time2 = 5;
            var dir = -1;
            var style = 2;
            var fadeMult = .5d;
            switch (projectile.ai[0])
            {
                case 8f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (105 - factor * 210);
                        Vector2 pos = new Vector2(16f, 0f).RotatedBy(rotation);
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/TrueNightsEdge"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 10;
                    time2 = 20;
                    opacity = 4f;
                    style = 1;
                    dir = 1;
                    fadeMult = .1d;
                    CNCUtils.PlaySound(SoundID.Item71, projectile);
                    CNCUtils.ScreenShake(0, new Vector2(0, 40f), new Vector2(0, 20f), .9f, default, owner);
                    break;
                case 6f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (factor * 210 - 75);
                        Vector2 pos = new Vector2(16f, 0f).RotatedBy(rotation);
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/TrueExcalibur"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 10;
                    time2 = 20;
                    opacity = 2f;
                    style = 1;
                    dir = 1;
                    fadeMult = .05d;
                    CNCUtils.PlaySound(SoundID.Item71, projectile);
                    CNCUtils.ScreenShake(0, new Vector2(0, 40f), new Vector2(0, 20f), .9f, default, owner);
                    break;
                case 5.4f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (factor * 450 + 145);
                        Vector2 pos = new Vector2(16f, 0f).RotatedBy(rotation);
                        pos.Y *= .8f;
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/TrueExcalibur"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 20;
                    time2 = 20;
                    opacity = 2f;
                    style = 2;
                    dir = 1;
                    CNCUtils.PlaySound(SoundID.Item71, projectile);
                    CNCUtils.ScreenShake(0, new Vector2(0, 20f), new Vector2(0, 15f), .9f, default, owner);
                    break;
                case 5.3f:
                case 5.2f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        int dir2 = (projectile.ai[0] == 5.2f).ToDirectionInt(1);
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (135 - factor * 270) * dir2;
                        Vector2 pos = new Vector2(10f, 0f).RotatedBy(rotation);
                        pos.Y *= .5f;
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash2"),
                        CNC.GetTexture("Assets/Effects/SlashTop3"),
                        CNC.GetTexture("Assets/Projectiles/Slash/TrueExcalibur"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 8;
                    time2 = 10;
                    opacity = 2f;
                    style = 1;
                    dir = 1;
                    CNCUtils.PlaySound(SoundID.Item60, projectile);
                    break;
                case 5.1f:
                case 5f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        int dir2 = (projectile.ai[0] == 5f).ToDirectionInt(1);
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (135 - factor * 270) * dir2;
                        Vector2 pos = new Vector2(12f, 0f).RotatedBy(rotation + dir2 * MyMath.deg15);
                        pos.Y *= .75f;
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop4"),
                        CNC.GetTexture("Assets/Projectiles/Slash/TrueExcalibur"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 15;
                    time2 = 15;
                    opacity = 2f;
                    style = 2;
                    dir = 1;
                    CNCUtils.PlaySound("Slash1", projectile, .3f);
                    CNCUtils.ScreenShake(0, new Vector2(0, 20f), new Vector2(0, 10f), .8f, default, owner);
                    break;
                case 4f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (factor * 270 - 45);
                        Vector2 pos = new Vector2(16f, 0f).RotatedBy(rotation);
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/FieryGreatsword"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 10;
                    time2 = 15;
                    opacity = 2f;
                    style = 1;
                    dir = 1;
                    CNCUtils.PlaySound(SoundID.DD2_BetsyFireballShot, owner);
                    CNCUtils.ScreenShake(0, new Vector2(0, 40f), new Vector2(0, 20f), .9f, default, owner);
                    break;
                case 3.3f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (135 - factor * 240);
                        Vector2 pos = new Vector2(10f, 0f).RotatedBy(rotation);
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/FieryGreatsword"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 10;
                    time2 = 20;
                    opacity = 1.5f;
                    style = 1;
                    dir = 1;
                    CNCUtils.PlaySound(SoundID.Item45, projectile, 1f, 2f);
                    CNCUtils.ScreenShake(0, new Vector2(0, 20f), new Vector2(0, 10f), .9f, default, owner);
                    break;
                case 3.2f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (factor * 360 + 90);
                        Vector2 pos = new Vector2(12f, 0f).RotatedBy(rotation);
                        pos.Y *= .4f;
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/FieryGreatsword"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 15;
                    time2 = 15;
                    opacity = 1f;
                    dir = 1;
                    break;
                case 3.1f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (factor * 270 - 100);
                        Vector2 pos = new Vector2(12f, 0f).RotatedBy(rotation);
                        pos.Y *= .4f;
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/FieryGreatsword"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 15;
                    time2 = 15;
                    opacity = 1f;
                    dir = 1;
                    break;
                case 3f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (180 - factor * 270);
                        Vector2 pos = new Vector2(10f, 0f).RotatedBy(rotation);
                        pos.Y *= .3f;
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Projectiles/Slash/FieryGreatsword"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 15;
                    time2 = 15;
                    opacity = 1f;
                    dir = -1;
                    break;
                case 2f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (225 - factor * 450);
                        Vector2 pos = new Vector2(20f, 0f).RotatedBy(rotation);
                        pos.Y *= .4f;
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        Texture2,
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit3"),
                    };
                    time = 11;
                    time2 = 10;
                    dir = 1;
                    opacity = 3f;
                    CNCUtils.PlaySound(SoundID.Item71, projectile);
                    CNCUtils.ScreenShake(0, new Vector2(0, 40f), new Vector2(0, 20f), .9f, default, owner);
                    break;
                case 1f:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (180 - factor * 240);
                        Vector2 pos = new Vector2(12f, 0f).RotatedBy(rotation);
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        Texture2,
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit3"),
                    };
                    time = 16;
                    time2 = 5;
                    lightScaleTotal *= .5f;
                    opacity = 2f;
                    CNCUtils.PlaySound(SoundID.Item60, owner);
                    break;
                default:
                    for (int i = 0; i < info.Length; i++)
                    {
                        float factor = (float)i / info.Length;
                        float rotation = MyMath.deg01 * (180 - factor * 240);
                        Vector2 pos = new Vector2(12f, 0f).RotatedBy(rotation);
                        info[i].position = pos * 16f;
                    }
                    t = new Texture2D[]
                    {
                        CNC.GetTexture("Assets/Effects/Slash4"),
                        CNC.GetTexture("Assets/Effects/SlashTop6"),
                        CNC.GetTexture("Assets/Effects/Trace1"),
                        CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit1"),
                    };
                    time = 15;
                    time2 = 10;
                    lightScaleTotal *= .5f;
                    break;
            }
            SetDefaults(owner.Center, projectile.velocity, info.Length, time, time2, 10);
            SetDraw(blendStyle, opacity, 1f, style, dir, fadeMult);
            SetImage(t[0], t[1], t[2], t[3]);
            SetAttackInfo(info);
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.HitNPC(target, damage, knockback, crit);
            switch (projectile.ai[0])
            {
                case 4f:
                    target.AddBuff(BuffID.OnFire, 180);
                    if (target.type != 488 && !target.boss && ((target.position.X > projectile.Center.X).ToDirectionInt(1) == projectile.direction)) target.velocity.Y -= (1f - target.knockBackResist) * 16f;
                    break;
                case 3.3f:
                case 3.2f:
                case 3.1f:
                case 3f:
                    if (CNC.Random.NextBool()) target.AddBuff(BuffID.OnFire, 180);
                    CNCUtils.ScreenShake(0, new Vector2(0, 10f), new Vector2(0, 5f), .9f, default, owner);
                    break;
            }
        }

        public override void AI()
        {
            base.AI();
            Color c1 = Color.White;
            Color c2 = CNCID.Colors.Expert;
            float type = projectile.ai[0];
            switch (type)
            {
                case 8f:
                    c1 = Color.Lerp(new Color(128, 255, 128), Color.DarkGreen, .5f);
                    c2 = Color.DarkViolet;
                    SetColor(c2, Color.Lerp(c1, c2, .3f), new Color(128, 255, 128), new Color(0, 255, 0), c2);
                    break;
                case 6f:
                    c2 = Color.LightGoldenrodYellow;
                    SetColor(c2, c1, c2, c1, c2);
                    break;
                case 5.4f:
                case 5.3f:
                case 5.2f:
                case 5.1f:
                case 5f:
                    c2 = Color.LightGoldenrodYellow;
                    SetColor(c2, c1, c2, c1, c2);
                    if (!fading && projectile.timeLeft % projectile.extraUpdates == 0)
                    {
                        Vector4 info = GetSlashInfo();
                        float rot = info.W + projectile.rotation;
                        Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                        Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                        Color color = c2;
                    }
                    break;
                case 4f:
                    c2 = new Color(242, 92, 34);
                    SetColor(c2, c2, c2, c1, c2);
                    if (!fading && projectile.timeLeft % projectile.extraUpdates == 0)
                    {
                        Vector4 info = GetSlashInfo();
                        float rot = info.W + projectile.rotation;
                        Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                        Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                        Color color = c2;
                        for (int i = 5; i < 10; i++)
                        {
                            float amount = i / 10f;
                            Vector2 pos = MyMath.Lerp(begin, end, amount) + CNC.Random.NextVector2(-16f, -16f, 16f, 16f);
                            Vector2 vel = new Vector2(0f, CNC.Random.NextFloat(6f) * -projectile.direction * owner.gravDir).RotatedBy(rot);
                            int idx = Particle.NewParticle(
                                CNC.GetTexture("Assets/Particles/Light1"),
                                pos + vel,
                                vel * .5f,
                                0f,
                                150,
                                new Vector2(Main.rand.NextFloat(.05f, .15f)),
                                color.NewA(0),
                                color.NewA(15)
                            );
                            ParticleManager.particle[idx].velocityMul = new Vector2(.98f, .975f);
                            ParticleManager.particle[idx].velocityAdd = new Vector2(0f, -.05f);
                            ParticleManager.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0003f, .0010f));
                            ParticleManager.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
                        }
                    }
                    if (!fading && ((int)projectile.timeLeft % (this.timeLeft / 10) == 1))
                    {
                        Vector4 info = GetSlashInfo();
                        float rot = info.W + projectile.rotation;
                        Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                        Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                        //NewProjectile((begin + end) / 2f, new Vector2(8f, 0f).RotatedBy(rot), CNCID.Projectile.LegacyObject[CNCID.Projectile.FireFlow], projectile.damage, rot, 0f, 0f, 1f, 1f, 0f);
                        CNCUtils.NewProj(source, (begin + end) / 2f, new Vector2(4f, 0f).RotatedBy(rot), CNCID.Projectile.LegacyObject[CNCID.Projectile.FireFlow], projectile.damage, 0f, ai0: 1f);
                    }
                    break;
                case 3.3f:
                case 3.2f:
                case 3.1f:
                case 3f:
                    c2 = new Color(242, 92, 34);
                    SetColor(c2, c2, c2, c1, c2);
                    if (!fading && projectile.timeLeft % projectile.extraUpdates == 0)
                    {
                        Vector4 info = GetSlashInfo();
                        float rot = info.W + projectile.rotation;
                        Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                        Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                        Color color = c2;
                        for (int i = 5; i < 10; i++)
                        {
                            float amount = i / 10f;
                            Vector2 pos = MyMath.Lerp(begin, end, amount) + CNC.Random.NextVector2(-16f, -16f, 16f, 16f);
                            Vector2 vel = new Vector2(0f, CNC.Random.NextFloat(6f) * projectile.direction * owner.gravDir).RotatedBy(rot);
                            CNCUtils.NewDust(pos, DustId.Fire, CNC.Random.NextFloat(1f, 2f), vel, c2).noGravity = true;
                        }
                    }
                    double time = this.fading ? 1.0d : 1.0d - (double)projectile.timeLeft / this.timeLeft;
                    if (type == 3.3f && !fading && ((int)projectile.timeLeft % (this.timeLeft / 6) == 1))
                    {
                        Vector4 info = GetSlashInfo();
                        float rot = info.W + projectile.rotation;
                        Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                        Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                        rot = (end - begin).ToRotation();
                        //NewProjectile((begin + end) / 2f, new Vector2(6f, 0f).RotatedBy(rot), CNCID.Projectile.LegacyObject[CNCID.Projectile.FireFlow], projectile.damage, rot, 0f, 0f, 1f, 0f, 0f);
                        CNCUtils.NewProj(source, (begin + end) / 2f, new Vector2(6f, 0f).RotatedBy(rot), CNCID.Projectile.LegacyObject[CNCID.Projectile.FireFlow], projectile.damage / 2, 0f, ai0: 0f);
                    }
                    break;
                case 2f:
                    SetColor(c1, c1, c2, c1, c1);
                    if (!fading && projectile.timeLeft % (projectile.extraUpdates / 2) == 0)
                    {
                        Vector4 info = GetSlashInfo();
                        float rot = info.W + projectile.rotation;
                        Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                        Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                        Color color = CNCID.Colors.Expert.NewA(0);
                        for (int i = 5; i < 10; i++)
                        {
                            float amount = i / 10f;
                            Vector2 pos = MyMath.Lerp(begin, end, amount) + CNC.Random.NextVector2(-16f, -16f, 16f, 16f);
                            Vector2 vel = new Vector2(0f, CNC.Random.NextFloat(6f) * projectile.direction * owner.gravDir).RotatedBy(rot);
                            int idx = Particle.NewParticle(
                                ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                                pos + vel,
                                vel * .5f,
                                CNC.Random.NextDegree(),
                                30,
                                Vector2.One * CNC.Random.NextFloat(1.5f, 2f),
                                color,
                                color.NewA(5)
                                );
                            ParticleManager.particle[idx].velocityMul = Vector2.One * .95f;
                            ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(8f, 16f) * CNC.Random.NextBool().ToDirectionInt(1);
                            ParticleManager.particle[idx].scaleAdd = new Vector2(-.05f);
                            ParticleManager.particle[idx].alphaAdd = -.033f;
                            ParticleManager.particle[idx].alpha = 1f;
                        }
                    }
                    break;
                case 1f:
                    SetColor(c1, c2, c2, c1, c1);
                    if (!fading && projectile.timeLeft % projectile.extraUpdates == 0)
                    {
                        Vector4 info = GetSlashInfo();
                        float rot = info.W + projectile.rotation;
                        Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                        Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                        Color color = CNCID.Colors.Expert.NewA(0);
                        for (int i = 5; i < 10; i++)
                        {
                            float amount = i / 10f;
                            Vector2 pos = MyMath.Lerp(begin, end, amount) + CNC.Random.NextVector2(-16f, -16f, 16f, 16f);
                            Vector2 vel = new Vector2(0f, CNC.Random.NextFloat(6f) * projectile.direction * owner.gravDir).RotatedBy(rot);
                            int idx = Particle.NewParticle(
                                ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                                pos + vel,
                                vel,
                                CNC.Random.NextDegree(),
                                30,
                                Vector2.One * CNC.Random.NextFloat(1f, 1.5f),
                                color,
                                color.NewA(5)
                                );
                            ParticleManager.particle[idx].velocityMul = Vector2.One * .975f;
                            ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(8f, 16f) * CNC.Random.NextBool().ToDirectionInt(1);
                            ParticleManager.particle[idx].scaleAdd = new Vector2(-.05f);
                            ParticleManager.particle[idx].alphaAdd = -.033f;
                            ParticleManager.particle[idx].alpha = 1f;
                        }
                    }
                    break;
                default:
                    SetColor(c1, c1, c1, c1, c1);
                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            bool flag = base.PreDraw(ref lightColor);
            double time = this.fading ? 1.0d : 1.0d - (double)projectile.timeLeft / this.timeLeft;
            float alpha = MyMath.Lerp(1f, 0f, (float)(Math.Abs(time - .5f) * Math.Abs(time - .5f)) * 4f);
            Vector4 info = this.GetSlashInfo();
            float rotation = info.W + projectile.rotation;
            Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rotation);
            Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rotation);
            Vector2 pos = end - Main.screenPosition;
            Texture2D glow = Mod.Assets.Request<Texture2D>("Assets/Glow/Refringence").Value;
            Color color = this.colorSlashTop.Flash().NewA(0) * Alpha2 * alpha;
            Main.EntitySpriteDraw(glow, pos, null, color, 0f, glow.Size() / 2f, new Vector2(lightWidth.GetNextValue(), 80f) / 120f * lightScaleTotal, 0, 0);
            Main.EntitySpriteDraw(glow, pos, null, color, MyMath.deg90, glow.Size() / 2f, new Vector2(lightHeight.GetNextValue() / MyMath.phi, 80f) / 120f * lightScaleTotal, 0, 0);
            return flag;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D[] glow =
            {
                Mod.Assets.Request<Texture2D>("Assets/Glow/Lamp").Value,
                Mod.Assets.Request<Texture2D>("Assets/Glow/Teleport").Value,
            };
            bool flag = base.PreDraw(ref lightColor);
            double time = this.fading ? 1.0d : 1.0d - (double)projectile.timeLeft / this.timeLeft;
            float alpha = MyMath.Lerp(1f, 0f, (float)(Math.Abs(time - .5f) * Math.Abs(time - .5f)) * 4f);
            Vector4 info = this.GetSlashInfo();
            float rotation = info.W + projectile.rotation;
            Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rotation);
            Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rotation);
            Vector2 pos = end - Main.screenPosition;
            Color color = this.colorSlashTop.Flash().NewA(0) * Alpha2 * alpha;
            Main.EntitySpriteDraw(glow[1], pos, null, color, 0f, glow[1].Size() / 2f, Vector2.One / 4f * lightScaleTotal, 0, 0);
            float div = projectile.extraUpdates + 1;
            if (lightAlpha > 0f)
            {
                Main.EntitySpriteDraw(glow[1], pos, null, color * lightAlpha, lightRotation, glow[1].Size() / 2f, lightScale * lightScaleTotal, 0, 0);
                lightAlpha -= .025f / div;
                lightRotation += MyMath.deg01 * lightDirection * 2f / div;
                lightScale += new Vector2(-1f, 1f) / 40f / div;
            }
            else if (CNC.Random.NextBool(.001 / div))
            {
                lightAlpha = 1f;
                lightRotation = CNC.Random.NextDegree();
                lightDirection = CNC.Random.NextBool().ToDirectionInt();
                lightScale = new Vector2(1f, 0f);
            }
        }
    }
}
