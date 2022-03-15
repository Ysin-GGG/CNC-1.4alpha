using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.NPCs;

namespace CNC.Content.NPCs.SpiritBoss
{
    [AutoloadBossHead]
    public class SpiritBoss : CNCNPC
    {
        private Color mainColor => new Color(178, 214, 231, 0);
        private int preState = 0;
        public int state = 0;
        private int stateTick = 0;
        private float rot = 0;
        private float hurtMul = 1.2f;
        private float atkMul = 1f;
        private float[] lifeRegen = { 0f, 0f };
        private int[] lifeRegenTime = { 0, 0 };
        private int[] children = new int[3] { -1, -1, -1 };
        private int[] atkDamage = new int[3] { 30, 20, 25 };
        private int[] cd = new int[2] { 0, 0 };
        private float cure = 0;
        private int count = 0;
        private int[] alpha1 = { 0, 1, 0, 0, 100, 50, 0, 2 };
        private int[] scale1 = { 0, 1, 0, 0, 100, 50, 0, 2 };
        private int[] alpha2 = { 100, -1, 0, 25, 100, 0, 0, 1 };
        private int[] alpha3 = { 0, 1, 0, 0, 75, 0, 0, 1 };

        private int target { get => npc.target; set => npc.target = value; }

        public bool rage = false;

        public override void ResetStaticDefaults()
        {
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    BuffID.Confused,
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "CNC/Content/NPCs/SpiritBoss/Preview",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void ResetDefaults()
        {
            npc.width = 110;
            npc.height = 118;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;
            npc.damage = 30;
            npc.defense = 15;
            npc.lifeMax = 4300;
            npc.alpha = 255;
            npc.value = 50000;
            npc.boss = true;
            npc.noGravity = true;
            npc.dontTakeDamageFromHostiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.SpawnWithHigherTime(30);
            npc.BossBar = ModContent.GetInstance<SpiritBossBossBar>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Biomes.Tranquility>().Type }; 
            if (!Main.dedServ) Music = Options.useModMusic ? MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SpiritBoss") : MusicID.Boss3;
        }

        public override void ResetBestiary(ref BestiaryDatabase database, ref BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D Aura = ModContent.Request<Texture2D>("CNC/Content/NPCs/SpiritBoss/Aura").Value;
            Texture2D Glow = ModContent.Request<Texture2D>("CNC/Content/NPCs/SpiritBoss/Glow").Value;
            Texture2D Hoop = ModContent.Request<Texture2D>("CNC/Content/NPCs/SpiritBoss/Hoop").Value;
            Texture2D Rune = ModContent.Request<Texture2D>("CNC/Content/NPCs/SpiritBoss/Rune").Value;
            Vector2 pos = npc.Center - screenPos;
            Color color = Color.White;
            Color color2 = color; color.A = 0;
            CNCUtils.UpdateEffectVar(ref alpha1);
            CNCUtils.UpdateEffectVar(ref alpha2);
            Main.EntitySpriteDraw(Aura, pos, null, color * (1 - npc.alpha / 255f), rot, Aura.Size() / 2, 1, 0, 1);
            Main.EntitySpriteDraw(Aura, pos, null, color * (1 - npc.alpha / 255f) * (alpha1[0] / 100f) * .25f, rot, Aura.Size() / 2, 1 + (alpha1[0] / 100f) * .2f, 0, 1);
            Main.EntitySpriteDraw(Rune, pos, null, color * (1 - npc.alpha / 255f) * (alpha2[0] / 100f), rot, Rune.Size() / 2, 1, 0, 1);
            Main.EntitySpriteDraw(Glow, pos, null, Color.Lerp(color2, mainColor, alpha3[0] / 100f) * (1 - npc.alpha / 255f) * (alpha2[0] / 100f), 0, Glow.Size() / 2, 1, 0, 1);
            if (state == 3) Main.EntitySpriteDraw(Hoop, pos, null, mainColor * (1 - stateTick / 120f), rot / 2f, Hoop.Size() / 2, stateTick / 90f, 0, 0);
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == 2) return;
            if (NPC.life <= 0) for (int i = 1; i <= 2; i++)
                Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-3, 4), Main.rand.Next(-6, 7)), Mod.Find<ModGore>($"SpiritBossGore{i}").Type);
        }

        public override void ModifyHit(Entity target, ref int damage, ref float knockback, ref bool crit) => damage = rage ? 99999 : (int)(damage * atkMul);

        public override void ModifyHitBy(Player attacker, Entity entity, ref int damage, ref float knockback, ref bool crit, bool isItem) => damage = (int)(damage * hurtMul);

        public override void AI()
        {
            if (!Main.dedServ) Music = Options.useModMusic ? MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SpiritBoss") : MusicID.Boss3;
            target = Const.me;
            stateTick++;
            rot += MyMath.deg01;
            count = 0;
            for (int i = 0; i < 3; i++)
            {
                if (children[i] < 0) continue;
                NPC crystal = Main.npc[children[i]];
                if (crystal.active && crystal.life > 0) count++;
                else children[i] = -1;
            }
            hurtMul = 1.20f - count * .2f;
            atkMul = 1.75f - count * .25f;
            atkDamage = new int[] {
                (int)(30 * atkMul * (rage ? 50 : 1)),
                (int)(20 * atkMul * (rage ? 50 : 1)),
                (int)(25 * atkMul * (rage ? 50 : 1)),
            };
            float life = npc.GetLifePercent();
            if (life <= .5f && cd[0] == 0)
            {
                lifeRegen[0] = npc.lifeMax * .01f * count / 60;
                lifeRegenTime[0] = 60 * 5;
                cd[0] = 60 * 40;
            }
            if (life <= .1f && cd[1] == 0)
            {
                lifeRegen[1] = 30;
                if (Main.expertMode) lifeRegen[1] += 10;
                if (Main.masterMode) lifeRegen[1] += 10;
                lifeRegen[1] += (count * 20) / 60f;
                lifeRegenTime[1] = 60 * 10;
                cd[1] = -1;
            }
            if (lifeRegenTime[0]-- > 0) cure += lifeRegen[0];
            if (lifeRegenTime[1]-- > 0 && life < .1f) cure += lifeRegen[1];//Îó²î´ýÐÞÕý
            if(cure >= 1) { npc.life += (int)cure; cure -= (int)cure; }
            if (cd[0] > 0) cd[0]--;
            if (cd[1] > 0) cd[1]--;
            Vector2 pos = (npc.position + npc.Size / 2f) / 16f;
            float r = mainColor.R / 255f;
            float g = mainColor.G / 255f;
            float b = mainColor.B / 255f;
            Lighting.AddLight((int)pos.X, (int)pos.Y, r, g, b);
            
            var mp = Main.player[target].GetModPlayer<Common.Global.Players.AllPlayer>();
            if (rage = mp.EquippedQuartzHead && ((float)Main.player[target].statLifeMax2 / Main.player[target].statLifeMax) > 50)
                hurtMul = .1f;

            RunAway();
            Intro();
            Normal();
            Attack_QuickShoot();
            Attack_ConcentratedShoot();
            Attack_SummonMinions();
            Attack_RadialShoot();
            Attack_MeteorShower();
            Attack_BranchGrowth();
        }

        private void Intro()
        {
            if (state != 0) return;
            if (npc.alpha <= 0)
            {
                state = 1;
                stateTick = -90;
            }
            else
            {
                if(npc.alpha == 255)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float arc = MyMath.deg01;
                        Vector2 pos = npc.Center - MyMath.GetXY(arc * i * 90) * 320f;
                        children[i] = NewNPC(pos, CNCID.NPC.LegacyObject[CNCID.NPC.SpiritCrystal]).whoAmI;
                    }
                }
                npc.alpha -= 5;
                npc.position.Y += .5f;
                float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(npc.position, npc.width, npc.height);
                Dust dust = CNCUtils.NewDust(xywh, DustId.TrailingCyan, rand[0], Main.rand.NextFloat(1f, 2f), default, mainColor, new Vector2(0, Main.rand.NextFloat(1f, 2f)));
            }
        }

        private void Normal()
        {
            if(state != 1) return;
            if (stateTick >= 0)
            {
                state = Main.rand.Next(2, 6);
                stateTick = 0;
                //state = 5; preState = 1;
            }
        }

        private void Attack_QuickShoot()
        {
            if (state != 2) return;
            if (stateTick == 0)
            {
                if (preState == state)
                {
                    state = 1;
                    return;
                }
                preState = state;
            }
            if (stateTick < 120)
            {
                float rot2 = (Main.player[target].Center - npc.Center).ToRotation();
                if (stateTick % (rage ? 5 : 10) == 0) this.NewProjectile(24, rot2, CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritArrowV1], atkDamage[0]);
                if (stateTick % 60 == 0) SoundEngine.PlaySound(SoundID.Item165, npc.Center);
            }
            if (stateTick >= 120)
            {
                state = 1;
                stateTick = -180;
            }
        }

        private void Attack_ConcentratedShoot()
        {
            if (state != 3) return;
            if (stateTick == 0)
            {
                if (preState == state)
                {
                    state = 1;
                    return;
                }
                preState = state;
                SoundEngine.PlaySound(SoundID.Item162, npc.Center);
            }
            if (stateTick >= 60 && stateTick % (rage ? 2 : 5) == 0)
            {
                float rot = Main.rand.NextFloat(MyMath.pi * 2);
                Vector2 pos = npc.Center + MyMath.GetXY(rot) * Main.rand.Next(stateTick, stateTick + 10) * 4;
                this.NewProjectile(Vector2.Zero, CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritArrowV2], atkDamage[1]).oldPosition = pos;
            }
            if (stateTick >= 120)
            {
                state = 1;
                stateTick = -270;
            }
        }

        private void Attack_SummonMinions()
        {
            if (state != 4) return;
            int type = CNCID.NPC.LegacyObject[CNCID.NPC.TranquilityPixie];
            int count = 0;
            for (int i = 0; i < ChildrenNPC.Length; i++)
                count += (ChildrenNPC[i].type == type && ChildrenNPC[i].active) ? 1 : 0;
            if (count > 3 || rage)
            {
                state = 1;
                stateTick = -1;
                return;
            }
            if (stateTick == 0)
            {
                if (preState == state)
                {
                    state = 1;
                    return;
                }
                preState = state;
            }
            for (int i = 0; i < 3; i++)
            {
                float arc = MyMath.pi / 180;
                Vector2 pos = npc.Center + MyMath.GetXY(arc * i * 120) * 160f;
                NewNPC(pos, type);
                stateTick = -200;
            }
            state = 1;
        }

        private void Attack_RadialShoot()
        {
            if (state != 5) return;
            if (stateTick == 0)
            {
                if (preState == state)
                {
                    state = 1;
                    return;
                }
                preState = state;
            }
            if (stateTick % 18 == 17 && rage)
            {
                if (stateTick % 36 == 17) SoundEngine.PlaySound(SoundID.Item163, npc.Center);
                for (int i = 0; i < 10; i++)
                {
                    float deg = MyMath.deg01 * 36 * i + rot;
                    Vector2 pos = npc.Center;
                    Vector2 vel = MyMath.GetXY(deg) * 16f;
                    this.NewProjectile(vel, CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritArrowV3], atkDamage[2]).ai[0] = target + 1;
                }
            }
            else if (stateTick % 36 == 35)
            {
                SoundEngine.PlaySound(SoundID.Item163, npc.Center);
                for (int i = 0; i < 5; i++)
                {
                    float deg = MyMath.deg01 * 72 * i + rot;
                    Vector2 pos = npc.Center;
                    Vector2 vel = MyMath.GetXY(deg) * 16f;
                    if (stateTick == 71) vel *= .5f;
                    CNCUtils.NewProj(source, pos, vel, CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritArrowV3], atkDamage[2], 0, -1, target + 1);
                }
            }
            if (stateTick >= (rage ? 90 : 108))
            {
                state = 1;
                stateTick = -360;
            }
        }

        private void Attack_MeteorShower()
        {
            if (state != 6) return;
            if (stateTick == 0)
            {
                if (preState == state)
                {
                    state = 1;
                    return;
                }
                preState = state;
            }
            if (stateTick >= 0)
            {
                state = 1;
                stateTick = 0;
            }
        }

        private void Attack_BranchGrowth()
        {
            if (state != 7) return;
            if (stateTick == 0)
            {
                if (preState == state)
                {
                    state = 1;
                    return;
                }
                preState = state;
            }
            if (stateTick >= 0)
            {
                state = 1;
                stateTick = 0;
            }
        }

        private void RunAway()
        {
            if (state == 0) return;
            if (!Main.player[target].active || Main.player[target].dead || state == 10)
            {
                state = 10;
                npc.target = -1;
                if (npc.alpha == 255) npc.active = false;
                npc.alpha += 3;
                npc.position.Y -= .3f;
                float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(npc.position, npc.width, npc.height);
                Dust dust = CNCUtils.NewDust(xywh, DustId.TrailingCyan, rand[0], Main.rand.NextFloat(1f, 2f), default, mainColor);
            }
        }
    }

    public class SpiritBossBossBar : ModBossBar
    {
        private int bossHeadIndex = -1;

        public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
        {
            if (bossHeadIndex != -1) return TextureAssets.NpcHeadBoss[bossHeadIndex];
            return null;
        }
        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float lifePercent, ref float shieldPercent)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];
            if (!npc.active) return false;
            bossHeadIndex = npc.GetBossHeadTextureIndex();
            lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);
            return true;
        }
    }
}