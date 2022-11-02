using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.NPCs;
using CNC.Content.DataStructures;
using System.Collections.Generic;
using Effects = Terraria.Graphics.Effects;

namespace CNC.Content.NPCs.SpiritBoss
{
    [AutoloadBossHead]
    public class SpiritBoss : CNCNPC
    {
        private Color themeColor => CNCID.Colors.SpiritBoss2;

        private int target { get => npc.target; set => npc.target = value; }

        private Texture2D[] bossAnim =
        {
            CNC.GetTexture("Content/NPCs/SpiritBoss/" + "SpiritBoss"),
            CNC.GetTexture("Content/NPCs/SpiritBoss/" + "Channel"),
            CNC.GetTexture("Content/NPCs/SpiritBoss/" + "Attack"),
        };

        /// <summary> 暗淡背景透明度 </summary>
        public float BlackAlpha { get => this.GetData<float>(4); set => this.SetData<float>(MyMath.Clamp(value, 0f, 1f), 4); }
        /// <summary> 移动行为的目标点 </summary>
        private Vector2 NextPosition { get => new Vector2(this.GetData<float>(0), this.GetData<float>(1)); set { this.SetData<float>(value.X, 0); this.SetData<float>(value.Y, 1); } }
        /// <summary> 出生点 </summary>
        private Vector2 SpawnPosition { get => new Vector2(this.GetData<float>(2), this.GetData<float>(3)); set { this.SetData<float>(value.X, 2); this.SetData<float>(value.Y, 3); } }
        /// <summary> 天空滤镜 </summary>
        private bool Sky { set => CNC.SetSky(Name, value); }
        /// <summary> 绘制残影 </summary>
        private bool DrawShadow { get => this.GetData<bool>(0); set => this.SetData<bool>(value, 0); }
        /// <summary> 自动转向 </summary>
        private bool AutoTurnAround { get => this.GetData<bool>(1); set => this.SetData<bool>(value, 1); }
        /// <summary> 透明度变化速度 </summary>
        private int Fade { get => this.GetData<int>(0); set => this.SetData<int>(value, 0); }
        /// <summary> 上一帧的生命值（锁血） </summary>
        private int OldLife { get => this.GetData<int>(1); set => this.SetData<int>(value, 1); }
        /// <summary> 宕机持续时间 </summary>
        private int SleepTime { get => this.GetData<int>(2); set => this.SetData<int>(value, 2); }
        /// <summary> 普通移动状态计时 </summary>
        public int Frame { get => this.GetData<int>(3); set => this.SetData<int>(value, 3); }
        /// <summary> 特殊移动状态计时 </summary>
        public int MovingTime { get => this.GetData<int>(4); set => this.SetData<int>(value, 4); }
        /// <summary> 盾存活时间 </summary>
        public int ShieldTime { get => this.GetData<int>(5); set => this.SetData<int>(value, 5); }
        /// <summary> 本次行动状态计时 </summary>
        private int ActionTime { get => this.GetData<int>(6); set => this.SetData<int>(value, 6); }
        /// <summary> 行动状态手动计时 </summary>
        private int ActionTime2 { get => this.GetData<int>(7); set => this.SetData<int>(value, 7); }
        /// <summary> 下次行动前的冷却 </summary>
        private int ActionCooldown { get => this.GetData<int>(8); set => this.SetData<int>(value, 8); }
        /// <summary> 各个攻击模式的当前冷却 </summary>
        private int[] attackCooldown = new int[8];
        /// <summary> 各个攻击模式的最大冷却 </summary>
        private readonly int[] attackCooldownMax = { 0, 300, 1200, 1200, -1, 1200, 2400, 2400 };
        /// <summary> 各个攻击模式的权重 </summary>
        private readonly int[] weight = { 0, 6, 10, 5, -1, 5, 7, -1 };
        /// <summary> 各个攻击模式的权重（特殊条件下） </summary>
        private readonly int[] weight2 = { 0, 6, 10, 7, 11, 9, 10, 8 };
        /// <summary> 开启二阶段 </summary>
        public bool Rage { get => this.GetData<bool>(2); set => this.SetData<bool>(value, 2); }
        /// <summary> 剩余仆从数目 </summary>
        public int MinionCount { get => this.GetData<int>(9); set => this.SetData<int>(value, 9); }
        /// <summary> 行动编号 </summary>
        public int ActionState { get => this.GetData<int>(10); set => this.SetData<int>(value, 10); }
        /// <summary> 能量盾耐久 </summary>
        public int ShieldStrength { get => this.GetData<int>(11); set => this.SetData<int>(value, 11); }
        /// <summary> 能量盾最大耐久 </summary>
        public int ShieldStrengthMax => 400;
        /// <summary> 环绕箭伤害 </summary>
        public int Damage_Arrow => GetDamage(20, 30, 60);
        /// <summary> 斩击伤害 </summary>
        public int Damage_Slash => GetDamage(20, 40, 60);
        /// <summary> 护盾爆裂伤害 </summary>
        public int Damage_Shield => GetDamage(100, 100, 100);
        /// <summary> 突刺伤害 </summary>
        public int Damage_Dash => GetDamage(30, 50, 70);
        /// <summary> 棘刺伤害 </summary>
        public int Damage_Branch => GetDamage(70, 80, 90);
        /// <summary> 常规背景暗淡程度 </summary>
        public float BlackIntensityCommon => .75f;
        /// <summary> 动画计时器 </summary>
        public float AnimTime = 0;
        /// <summary> 吟唱状态动画 </summary>
        public bool AnimChannel = false;
        /// <summary> 攻击状态动画 </summary>
        public bool AnimAttack = false;

        private int[] shadowFrame = new int[shadowCount];
        private const int shadowCount = 25;

        public override void ResetStaticDefaults()
        {
            countFrame = 18;
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
            NPCID.Sets.NeedsExpertScaling[Type] = true;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void ResetDefaults()
        {
            npc.width = 132;
            npc.height = 112;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;
            npc.damage = 0;
            npc.defense = 15;
            npc.lifeMax = 4300;
            npc.alpha = 255;
            npc.value = 50000;
            npc.npcSlots = 10f;
            npc.boss = true;
            npc.noGravity = true;
            npc.friendly = false;
            npc.noTileCollide = true;
            npc.dontTakeDamageFromHostiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.SpawnWithHigherTime(60);
            npc.BossBar = ModContent.GetInstance<HealthBar>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Biomes.Tranquility>().Type };
            if (npc.oldPos.Length != shadowCount)
            {
                System.Array.Resize(ref npc.oldPos, shadowCount);
                System.Array.Resize(ref npc.oldRot, shadowCount);
            }
            for (int i = 0; i < shadowCount; i++)
            {
                this.shadowFrame[i] = -1;
                npc.oldRot[i] = 0.0f;
                npc.oldPos[i].X = 0.0f;
                npc.oldPos[i].Y = 0.0f;
            }
            if (!Main.dedServ) Music = Options.useModMusic ? MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SpiritBoss") : MusicID.Boss3;
        }

        public override void ModifyFrame(ref bool autoAnim, ref int[] frameTime, ref int frame, int frameHeight)
        {
            autoAnim = false;
        }

        public override void ResetBestiary(ref BestiaryDatabase database, ref BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            npc.dontTakeDamage = true;
            this.ActionState = -1;
            this.SpawnPosition = npc.Center;
            //CNCUtils.LockScreen(npc.Center, 180, target: Target);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            this.DrawBoss_Body(spriteBatch, false);
            if (this.ShieldStrength > 0) this.DrawEffect_Shield(spriteBatch, CNCID.Colors.SpiritBoss, (this.ShieldTime < 4 ? (this.ShieldTime / 2) : (this.ShieldTime - 4) / 3 % 5 + 2));
            if (this.MovingTime < 0)
            {
                if (this.MovingTime < -12) this.DrawEffect_Teleport(spriteBatch, npc.Center, 22 + MovingTime, CNCID.Colors.SpiritBoss);
                if (this.MovingTime > -11) this.DrawEffect_Teleport(spriteBatch, npc.Center, -MovingTime - 1, CNCID.Colors.SpiritBoss);
            }
            return false;
        }
        
        /// <summary> 绘制boss的动画及其自身特效 </summary>
        private void DrawBoss_Body(SpriteBatch spriteBatch, bool attack)
        {
            int type = this.AnimTime < 0 ? 1 : 0;
            int[] frame = { 18, 18, 14 };//每个动作的帧数
            int[] tpf = { 4, 4, 3 };//每帧嘀嗒数
            Texture2D t = bossAnim[type];
            int f = frame[type];
            int height = bossAnim[type].Height / f;
            Rectangle source = new Rectangle(0, f * height, t.Width, height);
            Vector2 origin = source.Size() / 2f;

            npc.width = bossAnim[type].Width / 2;
            npc.height = bossAnim[type].Height / f;

            if (this.AnimTime < 0) 
            {
                int channelAnimFrame = 5;
                int channelAnimTime = channelAnimFrame * tpf[1];
                this.Frame = (int)Math.Abs(this.AnimTime) / tpf[1];
                if (this.Frame >= frame[1] + channelAnimFrame) this.AnimTime += channelAnimTime * 2;//回退动画播放
                if (this.Frame >= frame[1] - channelAnimFrame)
                    this.Frame = this.Frame < frame[1] ? this.Frame : (frame[1] + channelAnimFrame - this.Frame) + (frame[1] - channelAnimFrame - 1);
            }
            else this.Frame = (int)this.AnimTime / tpf[0] % f;
            npc.frame.Y = 0;

            source.Y = this.Frame * height;

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Effect effect = CNC.GetEffect("Purity");
            effect.Parameters["uColor"].SetValue(CNCID.Colors.SpiritBoss.ToVector4());
            effect.Parameters["uAlpha"].SetValue(1f);
            effect.CurrentTechnique.Passes[0].Apply();

            if (this.tick % 2 == 0) this.shadowFrame[0] = this.DrawShadow && this.Alpha2 > 0 ? type : -1;
            int max = 10;
            for (int i = max - 1; i >= 0; i--)
            {
                int type2 = this.shadowFrame[i];
                if (type2 < 0) continue;
                Texture2D t2 = bossAnim[type2];
                int f2 = (int)System.Math.Abs(npc.oldRot[i]) - 1;
                int height2 = t2.Height / frame[type2];
                Rectangle source2 = new Rectangle(0, f2 * height2, t2.Width, height2);
                Vector2 origin2 = source2.Size() / 2f;

                effect.Parameters["uImageSize"].SetValue(t2.Size());
                effect.Parameters["uAlpha"].SetValue((1f - (float)i / max) * Alpha2);
                effect.CurrentTechnique.Passes[0].Apply();

                spriteBatch.Draw(t2, npc.oldPos[i] + npc.Size / 2f - Main.screenPosition, source2, Color.White, 0f, origin2, 1f, (SpriteEffects)(npc.oldRot[i] < 0).ToInt(1), 0);
            }

            effect = CNC.GetEffect("HighLight");
            effect.Parameters["uColor0"].SetValue(CNCUtils.FlashColor(61, 80, 112).ToVector4());
            effect.Parameters["uColor1"].SetValue(CNCUtils.FlashColor(225, 242, 249).ToVector4());
            effect.Parameters["uImageSize"].SetValue(t.Size());
            effect.Parameters["uSize"].SetValue(source.Size());
            effect.Parameters["uOffset"].SetValue(new Vector2(0, .5f) * Main.GlobalTimeWrappedHourly);
            effect.Parameters["uAlpha"].SetValue(this.Alpha2);
            effect.Parameters["uFrame"].SetValue(this.Frame);
            effect.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            Main.graphics.GraphicsDevice.Textures[1] = CNC.GetTexture("Assets/Effects/Trace2");
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

            if (!(SleepTime > 0 && MovingTime < 0))
                spriteBatch.Draw(t, npc.Center - Main.screenPosition, source, this.Rage ? Color.White.Flash().Flash() : Color.White, 0f, origin, 1f, (SpriteEffects)(npc.direction < 0).ToInt(1), 0);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
        }
        
        /// <summary> 绘制传送特效 </summary>
        private void DrawEffect_Teleport(SpriteBatch spriteBatch, Vector2 pos, int frame, Color color)
        {
            Texture2D effect = CNC.GetTexture("Assets/Glow/Teleport");
            Vector2[] scale =
            {
                new Vector2(2f, .75f),
                new Vector2(1.75f, .8f),
                new Vector2(1.5f, .85f),
                new Vector2(1.25f, .9f),
                new Vector2(1f, 1f),
                new Vector2(1f, 1f),
                new Vector2(.875f, 1.125f),
                new Vector2(.75f, 1.25f),
                new Vector2(.625f, 1.375f),
                new Vector2(.5f, 1.5f),
            };
            float[] alpha = { .2f, .4f, .6f, .8f, 1f, 1f, .8f, .6f, .4f, .2f };
            frame = MyMath.Clamp(frame, 0, 9);
            pos -= Main.screenPosition;
            color.A = 0;
            if (frame < 10)
            {
                spriteBatch.Draw(effect, pos, null, color * alpha[frame], 0f, effect.Size() / 2f, scale[frame], 0, 0);
                spriteBatch.Draw(effect, pos, null, color * alpha[frame], 0f, effect.Size() / 2f, scale[frame], 0, 0);
            }
        }
        
        /// <summary> 绘制盾的特效 </summary>
        private void DrawEffect_Shield(SpriteBatch spriteBatch, Color color, int frame)
        {
            int height = Texture2.Height / this.countFrame;
            Vector2 pos = npc.Center - Main.screenPosition;
            Texture2D trace = CNC.GetTexture("Assets/Effects/Trace6");
            Texture2D image = CNC.GetTexture($"Content/NPCs/SpiritBoss/Anim/EnergyShield_{frame}");

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            Effect shader = mod.Assets.Request<Effect>("Content/Effects/ShieldLight").Value;
            shader.Parameters["uColor"].SetValue(Color.White.ToVector3());
            shader.Parameters["uOffset"].SetValue(new Vector2(-.6f, .6f) * Main.GlobalTimeWrappedHourly);
            shader.Parameters["uCenter"].SetValue(new Vector2(.5f, .5f));
            shader.Parameters["uSize"].SetValue(image.Size());
            shader.Parameters["uDist"].SetValue(.1f);
            shader.Parameters["uAlpha"].SetValue(Alpha2 * (ShieldStrength / 3f / ShieldStrengthMax + .375f));
            shader.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.Textures[0] = image;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Main.graphics.GraphicsDevice.Textures[1] = trace;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            spriteBatch.Draw(image, pos, null, color, 0, image.Center(), 1f, 0, 0);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public override void ModifyHitBy(Player attacker, Entity entity, ref int damage, ref float knockback, ref bool crit, bool isItem)
        {
            if (this.ActionState == 4)
            {
                damage = 1;
                this.OldLife = npc.life - damage - crit.ToInt(1);
            }
        }

        /// <summary> 受击检测 </summary>
        public override void HitBy(Player attacker, Entity entity, int damage, float knockback, bool crit, bool isItem) 
            => this.UpdateShield();//每次被击中，执行一次盾伤害吸收的检测

        /// <summary> 击中玩家时的效果 </summary>
        public override void HitPlayer(Player target, ref int damage, ref bool crit)
        {
            //boss不需要这个方法（因为没有碰撞伤害），这个方法是给其他弹幕击中玩家时用的
            damage = 100;
            Vector2 max = Vector2.One * 24f;
            target.velocity = MyMath.Clamp(target.Center - npc.Center, -max, max);
        }

        /// <summary> 死亡判定 </summary>
        public override bool CheckDead()
        {
            if (this.ShieldStrength <= 0 && this.ActionState < -1 && this.ActionTime == -1 || this.ActionState == -2)
            {
                npc.life = 0;
                npc.active = false;
                this.Sky = false;
                this.ShieldTime = 0;
                this.ShieldStrength = 0;
                if (this.ActionState == -3)
                {
                    npc.NPCLoot();
                    CNCUtils.PlaySound(npc.DeathSound.Value, npc);
                }
                return true;
            }
            if (this.ActionState != -3)
            {
                this.FinishAction();
                this.ActionState = -3;
                this.ActionTime = 0;
            }
            npc.life = 1;
            npc.active = true;
            npc.dontCountMe = true;
            npc.dontTakeDamage = true;
            npc.dontTakeDamageFromHostiles = true;
            return false;//若盾存在，则不判定死亡
        }

        /// <summary> 伤害吸收 </summary>
        private void UpdateShield()
        {
            if (this.ShieldStrength > 0)//当盾存在时
            {
                this.ShieldStrength = this.ShieldStrength - (this.OldLife - npc.life);//根据上一帧的生命值与当前生命值，计算盾应该减少的耐久
                npc.life = OldLife + MyMath.Min(this.ShieldStrength, 0);//若盾耐久大于0则锁血，否则按数值正常扣血
                if (this.ShieldStrength < 0) this.ShieldStrength = 0;//盾耐久小于0时，重置为0避免bug
                else npc.active = true;//盾耐久不为0时，npc不判定死亡
                if (this.ShieldStrength <= 0)//爆盾
                {
                    this.ShieldTime = 0;
                    this.ActionCooldown = 0;
                    CNCUtils.PlaySound(SoundID.Item74, npc);
                    CNCUtils.PlaySound(SoundID.Item122, npc);
                    CNCUtils.ScreenShake(npc.Center, 9000f, 5, new Vector2(0, 40), new Vector2(0, 20), .8f);
                    this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.ShockWave], 0, 0).ai[0] = 1;
                    var e = CNCUtils.CreateExplosion(npc);
                    e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/SilverFieldExplosion"), npc.Center, Vector2.One * 320, new int[5] { 2, 2, 3, 3, 3 });
                    e.SetDamage(2, DamageClass.Default, this.Damage_Shield, 0f, 0, 0f, 1f);
                    e.SetColor(CNCID.Colors.SpiritBoss, CNCID.Colors.SpiritBoss, .3f);
                    e.SetAlpha(1f, -.025f, 1f);
                    e.SetScale(1.5f, .0125f, 1f);
                    e.Vanilla_OnHitPlayerEffect += this.HitPlayer;
                }
            }
            else if (this.ActionState == 4) npc.life = this.OldLife;
            else this.OldLife = npc.life;
        }
        
        public override void AI()
        {
            if (!Main.dedServ) Music = Options.useModMusic ? MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SpiritBoss") : MusicID.Boss3;
            if (Effects.SkyManager.Instance["CNC:SpiritBossSky"].IsActive()) (Effects.SkyManager.Instance["CNC:SpiritBossSky"] as Skies.SpiritBossSky).intensity = this.BlackAlpha;
            npc.target = target = npc.FindClosestPlayer();

            //自动转向
            if (this.AutoTurnAround && Target != null) npc.direction = (Target.Center.X > npc.Center.X).ToDirectionInt(1);
            if (this.Fade != 0)
            {
                npc.alpha = MyMath.Clamp(npc.alpha + this.Fade, 0, 255);
                if (npc.alpha == 0 || npc.alpha == 255) this.Fade = 0;
            }

            if (!this.AnimChannel) this.AnimTime++; 
            else this.AnimTime = MyMath.Min(this.AnimTime - 1, 0f);

            //当盾耐久大于0时，为盾的存在时间计时
            if (this.ShieldStrength > 0) this.ShieldTime++;
            //盾的存在时间超过指定数值后，重置相关数据并生成爆炸弹幕
            if (this.ShieldTime >= 25f * 60f && this.MovingTime >= 0 && this.Alpha2 >= .75f && this.ActionState >= 0)
            {
                this.ShieldTime = 0;
                this.ShieldStrength = 0;
                CNCUtils.PlaySound(SoundID.Item74, npc);
                CNCUtils.PlaySound(SoundID.Item122, npc);
                CNCUtils.ScreenShake(npc.Center, 9000f, 5, new Vector2(0, 40), new Vector2(0, 20), .8f);
                this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.ShockWave], 0, 0).ai[0] = 1f;
                var e = CNCUtils.CreateExplosion(npc);
                e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/SilverFieldExplosion"), npc.Center, Vector2.One * 320, new int[5] { 2, 2, 3, 3, 3 });
                e.SetDamage(2, DamageClass.Default, this.Damage_Shield, 0f, 0, 0f, 1f);
                e.SetColor(CNCID.Colors.SpiritBoss, CNCID.Colors.SpiritBoss, .3f);
                e.SetAlpha(1f, -.025f, 1f);
                e.SetScale(1.5f, .0125f, 1f);
                e.Vanilla_OnHitPlayerEffect += this.HitPlayer;
            }

            //每2帧刷新一次残影数据
            if (this.tick % 2 == 0 && !Main.dedServ)
            {
                for (int i = shadowCount - 1; i > 0; i--)
                {
                    this.shadowFrame[i] = this.shadowFrame[i - 1];
                    npc.oldPos[i] = npc.oldPos[i - 1];
                    npc.oldRot[i] = npc.oldRot[i - 1];
                }
                npc.oldRot[0] = (this.Frame + 1) * npc.direction;
                if (this.MovingTime > -12) npc.oldPos[0] = npc.position;
            }

            //每帧都要刷新盾相关的数据
            this.UpdateShield();
            this.ActionTime++;
            if (this.SleepTime == 0)
            {
                if (this.MovingTime == 0)
                {
                    switch (this.ActionState)
                    {
                        case 0:
                            this.AutoTurnAround = true;
                            this.ActionState = this.NextAction();
                            if (this.ActionState > 0)
                            {
                                this.ActionCooldown = 0;
                                this.ActionTime = -1;//进入下个状态前，计时器还会再加1，所以这里设为-1
                            }
                            break;
                        case 1:
                            this.Attack_Rain();
                            break;
                        case 2:
                            this.Attack_Guard();
                            break;
                        case 3:
                            this.Attack_Slash();
                            break;
                        case 4:
                            this.Attack_Vow();
                            break;
                        case 5:
                            this.Attack_Dash();
                            break;
                        case 6:
                            this.Attack_Shield();
                            break;
                        case 7:
                            this.Attack_Branch();
                            break;
                        case -1:
                            this.Anim_Intro();
                            break;
                        case -2:
                            this.Anim_RunAway();
                            break;
                        case -3:
                            this.Anim_Death();
                            break;
                    }
                }
                else
                {
                    if (this.MovingTime > 0) this.Action_MoveQuickly();
                    if (this.MovingTime < 0) this.Action_Teleport();
                }
            }
            else { this.SleepTime--; npc.position = npc.oldPosition; }
            for (int i = 0; i < attackCooldown.Length; i++) if (attackCooldown[i] > 0) attackCooldown[i]--;
        }

        /// <summary> 结束当前行动 </summary>
        private void FinishAction(int extraActionCooldown = 0, int extraAttackCooldown = 0)
        {
            if (this.ActionState >= 0) this.attackCooldown[this.ActionState] = this.attackCooldownMax[this.ActionState] + extraAttackCooldown;
            this.ActionCooldown = 120 + extraActionCooldown;
            this.ActionTime = 0;
            this.ActionState = 0;
            this.AnimChannel = false;
        }

        /// <summary> 决定下个行动 </summary>
        private int NextAction()
        {
            if (!Main.player[target].active || Main.player[target].dead) return -2;
            if (this.ActionCooldown-- > 0) return 0;
            float dist = npc.Center.Distance(Main.player[target].Center);
            int[] weight = this.weight;
            if (dist < 16f * 20f) weight[3] = this.weight2[3];//距离过近，增加斩击权重
            if (dist > 16f * 72f) weight[5] = this.weight2[5];//距离过远，增加突刺权重
            if (!Rage && npc.GetLifePercent() < .5f) weight[4] = this.weight2[4];//达到血线且未进入二阶段，将立誓（进入二阶段）权重设为最高
            if (Rage) weight[7] = this.weight2[7];//二阶段解锁棘刺，即恢复其原本权重
            int maxWeight = 0;//当前可用技能中的最大权重
            List<int> whatCanIdo = new List<int>();
            for (int i = 0; i < this.attackCooldown.Length; i++)
                if (this.attackCooldown[i] == 0 && weight[i] > maxWeight)
                    maxWeight = weight[i];//从可选行动中筛选出最大权重
            for (int i = 0; i < this.attackCooldown.Length; i++)
                if (this.attackCooldown[i] == 0 && weight[i] == maxWeight)
                    whatCanIdo.Add(i);//筛选出权重最大的可选行动
            if (CNC.NotMPClient) npc.netUpdate = true;
            return CNC.Random.NextAny(whatCanIdo);
            
        }

        /// <summary> 箭雨 </summary>
        private void Attack_Rain()
        {
            //移动到目标上方
            if (this.ActionTime == 0)    
            {
                this.NextPosition = Target.Center - new Vector2(CNC.Random.NextFloat(-80f, 80f) + (CNC.Random.NextBool() ? 640f : -640f), 280f);
                this.Action_MoveQuickly(10);
                CNCUtils.PlaySound(SoundID.Item83, npc.Center);
            }
            //释放箭雨
            if (this.ActionTime >= 40)
            {
                this.AutoTurnAround = false;
                float maxVelX = 36f;
                int time = 10;
                if (this.ActionTime == 40) npc.velocity.X = maxVelX / (2 << time) * npc.direction;
                if (this.ActionTime == 40 + time) CNCUtils.PlaySound(SoundID.Item163, npc.Center);
                if (npc.velocity.X * npc.direction < maxVelX) npc.velocity.X = MyMath.Clamp(npc.velocity.X * 2, -maxVelX, maxVelX);
                else if (this.ActionTime % 3 == 0)
                {
                    Projectile p = this.NewProjectile(-new Vector2(this.Rage ? npc.velocity.X * .15f : 0f, 12f), CNCID.Projectile.LegacyObject[CNCID.Projectile.EnergyArrow], this.Damage_Arrow * 2f);
                    p.direction = npc.direction;
                    p.ai[0] = this.Rage.ToInt(1);
                }
            }
            if (this.ActionTime > 100)
            {
                this.NextPosition = SpawnPosition;
                this.Action_Teleport(Rage ? 90 : 30);
                this.FinishAction();
            }
        }

        /// <summary> 环绕箭 </summary>
        private void Attack_Guard()
        {
            //释放环绕箭
            if (this.ActionTime == 0)
            {
                SoundEngine.PlaySound(SoundID.Item165, npc.Center);
                //随机朝向
                int dir = CNC.Random.NextBool().ToDirectionInt(1);
                //上移初速度
                npc.velocity.Y = -8f;
                for (int i = 0; i < 3; i++)
                {
                    Projectile arrow = this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritArrow], this.Damage_Arrow, 0);
                    arrow.localAI[0] = MyMath.pi2 * i / 3f;//extraRotation
                    arrow.localAI[1] = 40f * 16f;//radius
                    arrow.ai[0] = dir;
                    arrow.ai[1] = 0f;//radius2
                }
            }
            //缓慢减速
            else if (npc.velocity.Length() > 1f) npc.velocity *= .95f;
            else
            {
                npc.velocity *= 0f;
                this.FinishAction();
            }
        }

        /// <summary> 斩击 </summary>
        private void Attack_Slash()
        {
            float getSlash() => npc.localAI[0];
            void setSlash(float value) => npc.localAI[0] = value;
            //蓄力
            if (this.ActionTime < 60)
            {
                this.AnimChannel = true;
                //后撤蓄力
                if (this.ActionTime == 0) npc.velocity = -Vector2.Normalize(Target.position - npc.position) * 8f;
                else npc.velocity *= .95f;
                //警告目标玩家
                if (this.ActionTime == 40)
                {
                    Vector2 pos = Target.Center;
                    CNCUtils.PlaySound(SoundID.Item4, pos);
                    Projectiles.GenericExplosion e = CNCUtils.CreateExplosion(npc);
                    e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/MeteorExplosion"), pos, Vector2.One * 120, new int[7] { 1, 1, 1, 1, 2, 2, 2 });
                    e.SetColor(themeColor, themeColor, .3f);
                    e.SetAlpha(1f, -.025f, 1f);
                    e.SetScale(.5f, .025f, 1f);
                    e.SetRotation(0f, 0f, 1f);
                }
                //二阶段时背景暗淡
                if (this.Rage) this.BlackAlpha += .025f;
            }
            //斩击数不足三次时持续进攻
            else if (getSlash() < 3)
            {
                //冲刺
                if (this.ActionTime2 >= (50 - (Rage ? (int)getSlash() * 10 + 5 : 0)) && (getSlash() % 1) < .4f)//0.4本应该写0.5的，但考虑到误差就调小一点
                {
                    Player tar = Main.player[target];
                    this.NextPosition = tar.Center + CNC.Random.NextBool().ToDirectionInt(1) * new Vector2(60, 0);
                    if (Rage) this.NextPosition = tar.Center + npc.direction * new Vector2(60, 0) + tar.velocity * 4f;
                    //二阶段时第一段结束后使用传送而不是高速移动
                    if (Rage && (int)getSlash() == 1) this.Action_Teleport();
                    else
                    {
                        CNCUtils.PlaySound(CNC.Random.NextBool() ? SoundID.Item66 : SoundID.Item92, this.npc);
                        this.Action_MoveQuickly(10);
                    }
                    setSlash(getSlash() + .5f);
                }
                //攻击
                else if ((getSlash() % 1) > .4f)
                {
                    //刀光部分
                    this.ActionTime2 = 0;
                    setSlash((int)getSlash() + 1);
                    CNCUtils.PlaySound(SoundID.Item71, this.npc);
                    CNCUtils.ScreenShake(npc.Center, 196f, 10, new Vector2(0, 40), new Vector2(0, 20), .8f);
                    int num = (int)(getSlash() % 2);
                    var info = new SlashInfo[2][]
                    {
                        new SlashInfo[600],
                        new SlashInfo[600],
                    };
                    int[][] frame =
                    {
                        new int[6] { 3, 4, 3, 0, 0, 0 },
                        new int[6] { 0, 0, 0, 3, 4, 3 },
                    };
                    Vector2[] offset =
                    {
                        new Vector2(-20f, 0f),
                        new Vector2(20f, 0f),
                    };
                    Vector2[] offset2 =
                    {
                        new Vector2(0f, -16f),
                        new Vector2(32f, 0f),
                    };
                    for (int i = 0; i < info[0].Length; i++)
                    {
                        float factor = (float)i / info[0].Length;
                        float rotation = MyMath.deg01 * (factor * 305 - 170);
                        Vector2 pos = new Vector2(11f, 0f).RotatedBy(rotation) * (this.Rage ? 1.1f : .8f);
                        pos.Y *= .3f;
                        info[0][i].position = pos * 16f;
                    }
                    for (int i = 0; i < info[1].Length; i++)
                    {
                        float factor = (float)i / info[1].Length;
                        float rotation = MyMath.deg01 * (90 - factor * 310);
                        Vector2 pos = new Vector2(11f, 0f).RotatedBy(rotation) * (this.Rage ? 1.1f : .8f);
                        if (rotation < MyMath.deg01 * -60) pos.Y *= .7f;
                        pos.Y *= .3f;
                        info[1][i].position = pos * 16f;
                    }

                    if (this.Rage)
                    {
                        Texture2D[] t =
                        {
                            CNC.GetTexture("Assets/Projectiles/Slash/SilverSlashShader"),
                            CNC.GetTexture("Assets/Effects/SlashTop1"),
                            CNC.GetTexture("Assets/Effects/Trace1"),
                            CNC.GetTexture("Assets/Projectiles/SlashHit/SlashHit2"),
                        };
                        Vector2 velocity = new Vector2(npc.direction, 0);
                        var s = CNCUtils.CreateShaderSlash(npc, velocity);
                        s.SetDefaults(npc.Center, velocity, 600, 7, 10, 15);
                        s.SetColor(Color.White, Color.White, Color.White, Color.White, CNCID.Colors.SpiritBoss);
                        s.SetDamage(2, DamageClass.Default, Damage_Slash, 0, 0);
                        s.SetDraw(BlendState.AlphaBlend, 2f);
                        s.SetImage(t[0], t[1], t[2], t[3]);
                        s.SetAttackInfo(info[num]);
                    }
                    else
                    {
                        Vector2 velocity = new Vector2(npc.direction, 0);
                        var e = CNCUtils.CreateSlash(npc, velocity, DustId.CyanShortFx1);
                        e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Slash/SilverSlash"), npc.Center, Vector2.One * .8f, velocity, frame[num], 10);
                        e.SetColor(Color.White, Color.Gold, .5f);
                        e.SetDamage(2, DamageClass.Default, Damage_Slash, 0, 0);
                        e.SetAttackInfo(info[num]);
                        e.SetAfterImages(50, 1, .25f);
                        e.imageOffset = offset[num];
                        e.hitboxOffset = offset2[num];
                    }
                }
                this.ActionTime2++;
            }
            //结束动画
            else if (this.ActionTime2 < 30)
            {
                //二阶段结束攻击后，立即传送回重生点
                if (Rage)
                {
                    if (this.ActionTime2++ == 1)
                    {
                        this.AnimChannel = false;
                        this.NextPosition = this.SpawnPosition;
                        this.Action_Teleport();
                    }
                    else this.BlackAlpha = MyMath.Max(this.BlackAlpha - .025f, this.BlackIntensityCommon);
                }
                //一阶段结束攻击后，后撤拉开距离
                else
                {
                    if (this.ActionTime2++ == 1) npc.velocity = -Vector2.Normalize(Target.position - npc.position) * 16f;
                    else npc.velocity *= .925f;
                }
            }
            //结束动作
            else
            {
                setSlash(0);
                npc.velocity = Vector2.Zero;
                this.ActionTime2 = 0;
                this.FinishAction();
            }
        }

        /// <summary> 濒亡立誓 </summary>
        private void Attack_Vow()
        {
            int beginTime = 120;
            int shakeTime = 120;
            int spawnTime = 180;
            int endTime = 3600;
            int spawnCount = 10;
            int getCount() => (int)npc.localAI[0];
            void setCount(int value) => npc.localAI[0] = value;
            if (this.ActionTime == 0)
            {
                NextPosition = SpawnPosition;
                Action_Teleport(1);
                setCount(spawnCount);
            }
            else if (this.ActionTime == 60)
            {
                CNCUtils.PlaySound(SoundID.Item4, npc);
                CNCUtils.PlaySound(SoundID.Item29, npc);
                CNCUtils.PlaySound("ArgenteusOathKeeper/Blessing", npc);
                var e = CNCUtils.CreateExplosion(npc);
                e.SetDefaults(Mod.Assets.Request<Texture2D>("Content/Projectiles/ArgenteusOathKeeper/Sanctuary").Value, npc.Top, Vector2.One * 170, 23, 2);
                e.SetDamage(0, DamageClass.Default, 0, 0, 0, 0f, 1f);
                e.SetColor(CNCID.Colors.SpiritBoss, CNCID.Colors.SpiritBoss, 1f);
                e.SetAlpha(1f, -.005f, 1f);
                e.SetScale(1.25f);
            }
            if (this.ActionTime == 30)
            {
                this.Sky = true;
                this.Rage = true;
                this.DrawShadow = true;
                this.AutoTurnAround = false;
                this.AnimChannel = true;
            }
            else if (this.ActionTime > 30 && BlackAlpha < .7f) BlackAlpha += .01f;
            if (this.ActionTime >= beginTime)
            {
                if (this.ActionTime % shakeTime == 0)
                {
                    CNCUtils.PlaySound(SoundID.Item122, npc);
                    CNCUtils.ScreenShake(npc.Center, 9000f, 5, new Vector2(0, 40), new Vector2(0, 20), .8f);
                    this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.ShockWave], 0, 0).ai[0] = 1f;
                }
                if (getCount() > 0 && this.MinionCount < 3 && this.ActionTime % spawnTime == 0 && this.ActionTime <= beginTime + spawnTime * spawnCount)
                {
                    setCount(getCount() - 1);
                    this.NewNPC(CNCID.NPC.TranquilityPixie);
                    this.MinionCount++;
                }
            }
            if (this.ActionTime > beginTime + endTime || (this.ActionTime > beginTime + spawnTime * spawnCount && this.MinionCount <= 0 && this.ActionTime % spawnTime == spawnTime - 1))
            {
                setCount(0);
                this.AutoTurnAround = true;
                this.FinishAction();
            }
        }

        /// <summary> 突刺 </summary>
        private void Attack_Dash()
        {
            float getDash() => npc.localAI[0];
            void setDash(float value) => npc.localAI[0] = value;
            int combo = 14;
            if (Rage)
            {
                npc.dontTakeDamage = true;
                this.AutoTurnAround = false;
                this.DrawShadow = false;
                //融入黑暗
                if (this.ActionTime <= 60)
                {
                    this.AnimChannel = true;
                    this.ActionTime2 = 0;
                    this.BlackAlpha = MyMath.Min(this.BlackAlpha + .025f, 1f);
                    this.Alpha2 = MyMath.Max(this.Alpha2 - .025f, 0f);
                }
                //攻击部分
                else if (getDash() < combo)
                {
                    float vel = 128f;
                    //放出预警线
                    if (this.ActionTime2 >= MyMath.Lerp(20f, 5f, (int)getDash() / (combo - 5f)) && (getDash() % 1) <= .4f)
                    {
                        setDash(getDash() + .5f);
                        this.NextPosition = Target.Center;
                        npc.velocity *= 0f;
                        npc.Center = Target.Center + new Vector2(vel * 7.5f, 0f).RotatedBy(CNC.Random.NextDegree());
                        var p = NewProjectile(0f, (NextPosition - npc.Center).ToRotation(), CNCID.Projectile.LegacyObject[CNCID.Projectile.WarningLine], 0f, (NextPosition - npc.Center).ToRotation());
                        p.Center = this.NextPosition;
                        p.timeLeft = MyMath.Max(this.ActionTime2, 25);
                        p.ai[0] = .05f;
                        ((Projectiles.WarningLine)p.ModProjectile).color = this.themeColor;
                    }
                    //按预警线攻击
                    if (this.ActionTime2 >= MyMath.Lerp(40f, 10f, (int)getDash() / (combo - 5f)) && (getDash() % 1) > .4f)
                    {
                        setDash((int)getDash() + 1);
                        this.ActionTime2 = -16;//16是突刺弹幕的存在时长，避免还在突刺时就开始下次攻击的坐标锁定
                        var p = NewProjectile(vel, (this.NextPosition - npc.Center).ToRotation(), CNCID.Projectile.LegacyObject[CNCID.Projectile.SilveryDashHostile], this.Damage_Dash, 0f);
                        p.ai[0] = 1f;
                        CNCUtils.ScreenShake(Target.Center, 144f, 5, Vector2.Normalize(p.velocity) * 30f, Vector2.Normalize(p.velocity) * 15f, .8f);
                        CNCUtils.PlaySound(CNC.Random.NextBool() ? SoundID.Item66 : SoundID.Item92, this.NextPosition);
                        CNCUtils.PlaySound(SoundID.Item28, this.NextPosition);
                    }
                }
                //结束动作，传送回目标玩家附近
                if (this.ActionTime > 60 && this.ActionTime2++ > 60 && getDash() >= combo)
                {
                    setDash(0);
                    npc.velocity = new Vector2(0f, -16f);
                    this.AutoTurnAround = true;
                    this.DrawShadow = Rage;
                    this.ActionTime2 = 0;
                    this.MovingTime = -12;
                    this.NextPosition = Target.Center - CNC.Random.NextVector2(-160f, 80f, 160f, 160f) * 3f;
                    this.Action_Teleport();
                    this.FinishAction();
                }
                //黑暗减淡
                else if (getDash() >= combo && this.ActionTime2 > 0)
                {
                    npc.velocity *= 0f;
                    this.BlackAlpha = MyMath.Max(this.BlackAlpha - .025f, this.BlackIntensityCommon);
                }
            }
            else
            {
                int extraTime = 60;
                float vel = 48f;
                //靠近目标
                if (this.ActionTime == 0)
                {
                    CNCUtils.PlaySound(SoundID.Item83, npc.Center);
                    this.NextPosition = Target.Center + new Vector2(vel * 10f, 0f).RotatedBy(CNC.Random.NextDegree(-30f, 30f)+ MyMath.pi * CNC.Random.NextInt(2));
                    this.Action_MoveQuickly(10);
                }
                //攻击
                if (this.ActionTime == extraTime + 0 || this.ActionTime == extraTime + 50 || this.ActionTime == extraTime + 110 || this.ActionTime == extraTime + 135 || this.ActionTime == extraTime + 160)
                {
                    var p = NewProjectile(vel, (Target.Center - npc.Center).ToRotation(), CNCID.Projectile.LegacyObject[CNCID.Projectile.SilveryDashHostile], this.Damage_Dash, 0f);
                    this.DrawShadow = true;
                    this.AutoTurnAround = false;
                }
                //第二段后的位移
                if (extraTime + 80 <= this.ActionTime && this.ActionTime <= extraTime + 90)
                {
                    if (this.ActionTime == extraTime + 80) npc.velocity -= Vector2.Normalize(Target.Center - npc.Center) * 16f;
                    else if (this.ActionTime == extraTime + 90)
                    {
                        CNCUtils.PlaySound(CNC.Random.NextBool() ? SoundID.Item66 : SoundID.Item92, this.npc);
                        this.NextPosition = Target.Center + new Vector2(CNC.Random.NextInt(48, 96) * CNC.Random.NextBool().ToDirectionInt(1), vel * 10f * CNC.Random.NextBool().ToDirectionInt(1));
                        this.Action_MoveQuickly(7);
                    }
                    else npc.velocity *= .975f;
                }
                //攻击时启用残影
                if (this.ActionTime == extraTime + 20 || this.ActionTime == extraTime + 70 || this.ActionTime == extraTime + 180)
                {
                    this.DrawShadow = this.Rage;
                    this.AutoTurnAround = true;
                }
                //结束动作，传送回重生点
                if (this.ActionTime >= extraTime + 200)
                {
                    this.NextPosition = this.SpawnPosition;
                    this.Action_Teleport(1);
                    this.FinishAction();
                }
            }
        }

        /// <summary> 银盾 </summary>
        private void Attack_Shield()
        {
            this.ShieldTime = 0;
            this.ShieldStrength = this.ShieldStrengthMax;
            CNCUtils.PlaySound(SoundID.Item29, npc);
            CNCUtils.PlaySound(SoundID.Item78, npc);
            CNCUtils.PlaySound(SoundID.Item81, npc);
            this.FinishAction();
            /*
            this.AutoTurnAround = false;
            if (this.ShieldLife >= this.ShieldLifeMax || this.ActionTime > 300)
            {
                FinishAction();
                if (Rage)
                {
                    CNCUtils.PlaySound(SoundID.Item74, npc);
                    var e = CNCUtils.CreateExplosion(npc);
                    e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/SilverFieldExplosion"), npc.Center, Vector2.One * 320, new int[5] { 2, 2, 3, 3, 3 });
                    e.SetDamage(2, DamageClass.Default, 100, 0f, 0, 0f, 1f);
                    e.SetColor(CNCID.Colors.SpiritBoss, CNCID.Colors.SpiritBoss, .3f);
                    e.SetAlpha(1f, -.025f, 1f);
                    e.SetScale(1.5f, .0125f, 1f);
                    e.ModifyHitPlayerEffect += this.HitPlayer;
                }
            }
            else
            {
                this.ShieldLife = (int)MyMath.Min(this.ShieldLife + this.ShieldLifeMax / 180f, this.ShieldLifeMax);
            }
            */
        }

        /// <summary> 棘刺丛生 </summary>
        private void Attack_Branch()
        {
            //传送到目标玩家上方
            if (this.ActionTime == 0)
            {
                this.NextPosition = Target.Center;
                this.floatData[1] -= CNC.Random.NextFloat(128f, 196f);//this.NextPosition.Y -= CNC.Random.NextFloat(128f, 196f);
                this.Action_Teleport(20);
            }
            //向上移动一段距离
            if (this.ActionTime == 60)
            {
                this.floatData[1] -= 320f;//this.NextPosition.Y -= 320f;
                this.Action_MoveQuickly(15);
                this.AnimChannel = true;
                CNCUtils.PlaySound(SoundID.Item83, npc.Center);
            }
            //生成棘刺
            if (this.ActionTime > 100 && this.ActionTime % 8 == 0)
            {
                CNCUtils.PlaySound(SoundID.Item51, npc.Center);
                CNCUtils.PlaySound(SoundID.Item88, npc.Center);
                CNCUtils.PlaySound(CNC.Random.NextBool() ? SoundID.Item69 : SoundID.Item70, npc.Center);
                CNCUtils.ScreenShake(npc.Center, 200f * 16f, 10, new Vector2(0, 60f), new Vector2(0, 15f), .95f);
                Vector2 pos = npc.Bottom;
                pos.X += CNC.Random.NextFloat(-64f, 64f) * 16f;
                //二阶段时，每48帧强制在目标玩家脚下生成
                if (this.Rage && this.ActionTime % 48 == 0) pos.X = Target.Center.X;
                if (this.ActionTime < 270)
                {
                    for (int i = 0; i < 199; i++)
                        if (Collision.SolidTiles(pos, 16, 1)) break;
                        else pos.Y += 16f;
                    this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.GiantBranch], Damage_Branch, 0).Bottom = pos;
                }
            }
            if (this.ActionTime > 300) this.FinishAction();
        }

        /// <summary> 登场动画 </summary>
        private void Anim_Intro()
        {
            if (this.Alpha2 < 1f)
            {
                this.AnimChannel = true;
                if (this.ActionTime > 60) this.Alpha2 += .02f;
                if (this.ActionTime == 60) CNCUtils.PlaySound(SoundID.Item123, npc);
                if (this.ActionTime % 10 == 0) CNCUtils.ScreenShake(10, new Vector2(80, 0), new Vector2(10, 0), .9f);
                Vector2 vel = new Vector2(CNC.Random.NextFloat(4f) + 4f, 0f).RotatedBy(CNC.Random.NextDegree());
                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                    npc.Center + vel * 10f,
                    vel * -.5f,
                    CNC.Random.NextDegree(),
                    30,
                    Vector2.One * 1.5f,
                    themeColor,
                    themeColor.NewA(10)
                    );
                ParticleManager.particle[idx].velocityMul = Vector2.One * .95f;
                ParticleManager.particle[idx].rotationAdd = CNC.Random.NextBool().ToDirectionInt(16);
                ParticleManager.particle[idx].scaleAdd = new Vector2(-.05f);
                ParticleManager.particle[idx].alphaAdd = -.033f;
                ParticleManager.particle[idx].alpha = 1f;
            }
            else
            {
                npc.dontTakeDamage = false;
                this.FinishAction();
            }
        }

        /// <summary> 离开动画 </summary>
        private void Anim_RunAway()
        {
            if (npc.velocity.Y < -16f)
            {
                if (this.ShieldTime <= 0) this.ShieldTime = 1;
                if (this.ShieldStrength <= 0) this.ShieldStrength = 1;
                this.NextPosition = Vector2.Zero;
                this.Action_Teleport(1);
            }
            else
            {
                npc.dontTakeDamage = true;
                if (this.ActionTime == 2) npc.velocity = new Vector2(0f, 8f);
                else npc.velocity.Y -= npc.velocity.Y > 0 ? .25f : .75f;
            }
            /*
            if (this.Alpha2 > 0)
            {
                this.Alpha2 -= .025f;
                npc.target = -1;
                npc.position.Y -= .3f;
                npc.active = true;
                npc.dontCountMe = true;
                npc.dontTakeDamage = true;
                npc.dontTakeDamageFromHostiles = true;
            }
            else
            {
                npc.active = true;
                this.ActionTime = -1;
                this.CheckDead();
                this.FinishAction();
            }
            */
        }

        /// <summary> 死亡动画 </summary>
        private void Anim_Death()
        {
            npc.life = 1;
            npc.active = true;
            npc.dontCountMe = true;
            npc.dontTakeDamage = true;
            npc.dontTakeDamageFromHostiles = true;
            if (this.ActionTime == 1)
            {
                this.NextPosition = this.SpawnPosition;
                this.Action_Teleport(1);
            }
            else if (this.ActionTime > 50)
            {
                this.Sky = true;
                this.Rage = true;
                this.AnimChannel = true;
                this.AutoTurnAround = false;
                if (this.BlackAlpha < 1f) this.BlackAlpha += .01f;
                if (this.ActionTime < 250)
                {
                    this.Alpha2 -= .005f;
                    if (this.ActionTime == 50) CNCUtils.PlaySound(SoundID.Item119, npc);
                    if (this.ActionTime == 200) CNCUtils.PlaySound(SoundID.Item121, npc);//结束前50帧
                    if (this.ActionTime % 15 == 0) CNCUtils.PlaySound(CNC.Random.NextBool() ? SoundID.Item13 : SoundID.Item15, npc);
                    if (this.ActionTime % 10 == 0) CNCUtils.ScreenShake(10, new Vector2(80, 0), new Vector2(10, 0), .9f);
                    Vector2 vel = new Vector2(CNC.Random.NextFloat(4f) + 4f, 0f).RotatedBy(CNC.Random.NextDegree());
                    int idx = Particle.NewParticle(
                        ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                        npc.Center + vel,
                        vel * .5f,
                        CNC.Random.NextDegree(),
                        30,
                        Vector2.One * 1.5f,
                        themeColor,
                        themeColor.NewA(10)
                        );
                    ParticleManager.particle[idx].velocityMul = Vector2.One * .95f;
                    ParticleManager.particle[idx].rotationAdd = CNC.Random.NextBool().ToDirectionInt(16);
                    ParticleManager.particle[idx].scaleAdd = new Vector2(-.05f);
                    ParticleManager.particle[idx].alphaAdd = -.033f;
                    ParticleManager.particle[idx].alpha = 1f;
                }
                else
                {
                    float max = 72f;
                    for (int i = 0; i < max; i++)
                    {
                        float amount = i / max * MyMath.pi2;
                        Vector2 vel = new Vector2(8f, 0f).RotatedBy(amount);
                        int idx = Particle.NewParticle(
                            ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                            npc.Center + vel,
                            vel,
                            amount,
                            50,
                            Vector2.One * 1.5f,
                            themeColor,
                            themeColor.NewA(10)
                            );
                        ParticleManager.particle[idx].velocityMul = Vector2.One * .93f;
                        ParticleManager.particle[idx].rotationAdd = CNC.Random.NextBool().ToDirectionInt(16);
                        ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                        ParticleManager.particle[idx].alphaAdd = -.02f;
                        ParticleManager.particle[idx].alpha = 1f;
                    }
                    CNCUtils.ScreenShake(20, new Vector2(0, 90), new Vector2(0, 20), .95f);
                    this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.ShockWave], 0, 0);
                    this.ShieldStrength = 0;
                    this.ActionTime = -1;
                    this.CheckDead();
                    this.FinishAction();
                }
            }
        }

        /// <summary> 传送（期间无敌） </summary>
        /// <param name="position"> 目标位置 </param>
        /// <param name="time"> 消失多久后出现（特效结束后开始计时） </param>
        private void Action_Teleport(int time = 1)
        {
            if (this.MovingTime < 0) this.MovingTime++;
            else if (time > 0)
            {
                this.MovingTime = -22;
                npc.ai[0] = time;
                npc.dontTakeDamage = true;
                npc.velocity = new Vector2(0, -1f);
            }
            if (this.MovingTime == -12)
            {
                SoundEngine.PlaySound(SoundID.Item115, npc.Center);
                SoundEngine.PlaySound(SoundID.Item67, npc.Center);
                this.DrawShadow = false;
                this.SleepTime = (int)this.npc.ai[0];
                if (this.ActionState == -2)
                {
                    npc.active = false;
                    this.ActionTime = -1;
                    this.ShieldTime = 0;
                    this.ShieldStrength = 0;
                    CNCUtils.PlaySound(SoundID.Item74, npc);
                    CNCUtils.PlaySound(SoundID.Item122, npc);
                    CNCUtils.ScreenShake(npc.Center, 9000f, 5, new Vector2(0, 40), new Vector2(0, 20), .8f);
                    this.NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.ShockWave], 0, 0);
                    var e = CNCUtils.CreateExplosion(npc);
                    e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/SilverFieldExplosion"), npc.Center, Vector2.One * 320, new int[5] { 2, 2, 3, 3, 3 });
                    e.SetDamage(2, DamageClass.Default, 0, 0f, 0, 0f, 1f);
                    e.SetColor(CNCID.Colors.SpiritBoss, CNCID.Colors.SpiritBoss, .3f);
                    e.SetAlpha(1f, -.025f, 1f);
                    e.SetScale(1.5f, .0125f, 1f);
                    this.CheckDead();
                    this.FinishAction();
                }
            }
            if (this.MovingTime == -11)
            {
                this.DrawShadow = Rage;
                npc.Center = this.NextPosition + npc.velocity * 4f;
                npc.velocity *= -1;
                npc.ai[0] = 0f;
                SoundEngine.PlaySound(SoundID.Item115, npc.Center);
                SoundEngine.PlaySound(SoundID.Item67, npc.Center);
            }
            if (this.MovingTime < -12)
            {
                npc.velocity.Y = MyMath.Clamp(npc.velocity.Y * 2f, -16f, -1f);
                this.Fade = 24;
            }
            if (this.MovingTime > -11 && this.MovingTime < 0)
            {
                npc.velocity.Y = MyMath.Clamp(npc.velocity.Y / 2f, 16f, 1f);
                this.Fade = -24;
            }
            if (this.MovingTime == 0)
            {
                npc.dontTakeDamage = false;
                npc.velocity = Vector2.Zero;
                this.Fade = 0;
            }
        }

        /// <summary> 高速移动（期间无敌） </summary>
        /// <param name="position"> 目标位置 </param>
        /// <param name="time"> 移动所需时间（正整数） </param>
        private void Action_MoveQuickly(int time = 0)
        {
            if (this.MovingTime > 0) this.MovingTime--;
            else if (time > 0)
            {
                this.DrawShadow = true;
                this.MovingTime = time;
                npc.dontTakeDamage = true;
                npc.velocity = (this.NextPosition - npc.Center) / time;
            }
            if (this.MovingTime <= 0 && time <= 0)
            {
                this.DrawShadow = this.Rage;
                this.MovingTime = 0;
                npc.dontTakeDamage = false;
                npc.velocity = Vector2.Zero;
            }
        }

        private void Action_MoveRandomly(int style)
        {

        }

        public class HealthBar : ModBossBar
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
                if (npc == null || !npc.active) return false;
                if((SpiritBoss)npc.ModNPC != null)
                {
                    SpiritBoss boss = (SpiritBoss)npc.ModNPC;
                    bossHeadIndex = npc.GetBossHeadTextureIndex();
                    lifePercent = npc.GetLifePercent();
                    shieldPercent = (float)boss.ShieldStrength / (float)boss.ShieldStrengthMax;
                    return true;
                }
                return false;
            }
        }
    }
}