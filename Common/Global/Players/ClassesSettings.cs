using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.GameContent;
using Terraria.DataStructures;
using CNC.Common.Utils;
using Terraria.ModLoader.IO;

using static CNC.Common.Utils.CNCID.Clazz;

namespace CNC.Common.Global.Players
{
    public class ClassesSettings : ModPlayer
    {
        Player player => Player;
        ClassesSettings modPlayer => player.GetModPlayer<ClassesSettings>();

        public static ModKeybind ClassKey;
        private int tick => player.GetModPlayer<AllPlayer>().Tick;
        public float atkSpeed = 0f;
        private float useTime = 0f;
        private float animTime = 0f;
        public float dealtDamage = 0f;
        public float takenDamage = 0f;
        public float takenDamageFromEnemies = 0f;

        public float block;// 当前格挡值
        public float blockMax;// 最大格挡值
        public float blockCooldown;// 当前格挡值补充冷却
        public float blockSpeed = 1;// 格挡值补充速度/举盾速度（时间单位：tick/帧）
        public float blockSpeedAdd;// 格挡值补充速度加成/举盾速度加成（加）
        public float blockSpeedMul;// 格挡值补充速度加成/举盾速度加成（乘）
        public float blockExhaustAdd;// 格挡消耗倍率（加）
        public float blockExhaustMul;// 格挡消耗倍率（乘）
        public float blockDamageReductionAdd;// 格挡减伤倍率（加）
        public float blockDamageReductionMul;// 格挡减伤倍率（乘）

        public bool[] IsClazz => new bool[]
        {
            this.clazz == CNCID.Clazz.Barbarian,
            this.clazz == CNCID.Clazz.Defender,
            this.clazz == CNCID.Clazz.Crusader,
            this.clazz == CNCID.Clazz.Ranger,
            this.clazz == CNCID.Clazz.Gunman,
            this.clazz == CNCID.Clazz.Thrower,
            this.clazz == CNCID.Clazz.Musketeer,
            this.clazz == CNCID.Clazz.Cleric,
            this.clazz == CNCID.Clazz.Wizard,
            this.clazz == CNCID.Clazz.SpellBreaker,
            this.clazz == CNCID.Clazz.Druid,
            this.clazz == CNCID.Clazz.Shaman,
            this.clazz == CNCID.Clazz.Warlock,
            this.clazz == CNCID.Clazz.Monk,
            this.clazz == CNCID.Clazz.Assassin,
        };

        private static object[] defaults => new object[]
        {
            (byte)0,
            0f, 30f, 1f, 0f, 1f, false, 0, 0,
            0f, 240f, 0f, 1f, 0f, 1f, 0,
            30, 30, 30, false, 0f, 0f, 0f, 1f, 1f, 0f, 1f,
            0f, 50f, 1f / 120f, 0f, 1f,
            0f, 900f, false, 0, 1f, 0f, 1f, 1f, 0f, 1f,
        };
        private string[] defaultTags => new string[]
        {
            "class",

            "rage",
            "rageMax",
            "rageSpeed",
            "rageSpeedAdd",
            "rageSpeedMul",
            "raged",
            "rageWeak",
            "rageCD",

            "power",
            "powerMax",
            "powerProgress",
            "powerSpeed",
            "powerSpeedAdd",
            "powerSpeedMul",
            "powerCD",

            "durability",
            "durabilityOld",
            "durabilityMax",
            "durabilityReload",
            "durabilityReloadTime",
            "durabilityReloadTimeMax",
            "durabilityReloadTimeAdd",
            "durabilityReloadTimeMul",
            "durabilityReloadSpeed",
            "durabilityReloadSpeedAdd",
            "durabilityReloadSpeedMul",

            "blessing",
            "blessingMax",
            "blessingSpeed",
            "blessingSpeedAdd",
            "blessingSpeedMul",

            "phantom",
            "phantomMax",
            "phantomState",
            "phantomInvincible",
            "phantomSpeed",
            "phantomSpeedAdd",
            "phantomSpeedMul",
            "phantomDelSpeed",
            "phantomDelSpeedAdd",
            "phantomDelSpeedMul",
        };

        public short[] stateType => _stateType.ToArray();
        public int[] stateTime => _stateTime;
        private List<short> _stateType = new List<short>();
        private int[] _stateTime = new int[CNCID.State.MaxObject];

        public int[] state => new int[CNCID.State.MaxObject]
        {
            this.clazz == CNCID.Clazz.Barbarian ? this.state_Wrath : 0,
            this.clazz == CNCID.Clazz.Barbarian ? this.state_Cowardice : 0,
            (this.clazz == CNCID.Clazz.Crusader || this.clazz == CNCID.Clazz.Cleric) ? this.state_Blasphemy : 0,
            this.clazz == CNCID.Clazz.Ranger ? this.state_Fluster : 0,
            this.clazz == CNCID.Clazz.Musketeer ? this.state_Restoring : 0,
            this.clazz == CNCID.Clazz.Assassin ? this.state_Wraith : 0,
        };

        public int state_Wrath;
        public int state_Cowardice;
        public int state_Blasphemy;
        public int state_Fluster;
        public int state_Restoring;
        public int state_Wraith;

        public byte clazz = (byte)defaults[0];

        public float clazz_Rage = (float)defaults[1];
        public float clazz_RageMax = (float)defaults[2];
        public float clazz_RageSpeed = (float)defaults[3];
        public float clazz_RageSpeedAdd = (float)defaults[4];
        public float clazz_RageSpeedMul = (float)defaults[5];
        public bool clazz_Raged = (bool)defaults[6];
        public int clazz_RageWeak = (int)defaults[7];
        public int clazz_RageCD = (int)defaults[8];

        public float clazz_Power = (float)defaults[9];
        public float clazz_PowerMax = (float)defaults[10];
        public float clazz_PowerProgress = (float)defaults[11];
        public float clazz_PowerSpeed = (float)defaults[12];
        public float clazz_PowerSpeedAdd = (float)defaults[13];
        public float clazz_PowerSpeedMul = (float)defaults[14];
        public int clazz_PowerCD = (int)defaults[15];

        public int clazz_Durability = (int)defaults[16];
        public int clazz_DurabilityOld = (int)defaults[17];
        public int clazz_DurabilityMax = (int)defaults[18];
        public bool clazz_DurabilityReload = (bool)defaults[19];
        public float clazz_DurabilityReloadTime = (float)defaults[20];
        public float clazz_DurabilityReloadTimeMax = (float)defaults[21];
        public float clazz_DurabilityReloadTimeAdd = (float)defaults[22];
        public float clazz_DurabilityReloadTimeMul = (float)defaults[23];
        public float clazz_DurabilityReloadSpeed = (float)defaults[24];
        public float clazz_DurabilityReloadSpeedAdd = (float)defaults[25];
        public float clazz_DurabilityReloadSpeedMul = (float)defaults[26];

        public float clazz_Blessing = (float)defaults[27];
        public float clazz_BlessingMax = (float)defaults[28];
        public float clazz_BlessingSpeed = (float)defaults[29];
        public float clazz_BlessingSpeedAdd = (float)defaults[30];
        public float clazz_BlessingSpeedMul = (float)defaults[31];

        public float clazz_Phantom = (float)defaults[32];
        public float clazz_PhantomMax = (float)defaults[33];
        public bool clazz_PhantomState = (bool)defaults[34];
        public int clazz_PhantomInvincible = (int)defaults[35];
        public float clazz_PhantomSpeed = (float)defaults[36];
        public float clazz_PhantomSpeedAdd = (float)defaults[37];
        public float clazz_PhantomSpeedMul = (float)defaults[38];
        public float clazz_PhantomDelSpeed = (float)defaults[39];
        public float clazz_PhantomDelSpeedAdd = (float)defaults[40];
        public float clazz_PhantomDelSpeedMul = (float)defaults[41];
        
        private void GetTag(TagCompound tag, int index, ref int target)
            => target = tag.ContainsKey(defaultTags[index]) ? tag.GetInt(defaultTags[index]) : (int)defaults[index];

        private void GetTag(TagCompound tag, int index, ref float target)
            => target = tag.ContainsKey(defaultTags[index]) ? tag.GetFloat(defaultTags[index]) : (float)defaults[index];

        private void GetTag(TagCompound tag, int index, ref bool target)
            => target = tag.ContainsKey(defaultTags[index]) ? tag.GetBool(defaultTags[index]) : (bool)defaults[index];

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag[defaultTags[0]] = this.clazz;

            tag[defaultTags[1]] = this.clazz_Rage;
            tag[defaultTags[2]] = this.clazz_RageMax;
            tag[defaultTags[3]] = this.clazz_RageSpeed;
            tag[defaultTags[4]] = this.clazz_RageSpeedAdd;
            tag[defaultTags[5]] = this.clazz_RageSpeedMul;
            tag[defaultTags[6]] = this.clazz_Raged;
            tag[defaultTags[7]] = this.clazz_RageWeak;
            tag[defaultTags[8]] = this.clazz_RageCD;

            tag[defaultTags[9]] = this.clazz_Power;
            tag[defaultTags[10]] = this.clazz_PowerMax;
            tag[defaultTags[11]] = this.clazz_PowerProgress;
            tag[defaultTags[12]] = this.clazz_PowerSpeed;
            tag[defaultTags[13]] = this.clazz_PowerSpeedAdd;
            tag[defaultTags[14]] = this.clazz_PowerSpeedMul;
            tag[defaultTags[15]] = this.clazz_PowerCD;
            
            tag[defaultTags[16]] = this.clazz_Durability;
            tag[defaultTags[17]] = this.clazz_DurabilityOld;
            tag[defaultTags[18]] = this.clazz_DurabilityMax;
            tag[defaultTags[19]] = this.clazz_DurabilityReload;
            tag[defaultTags[20]] = this.clazz_DurabilityReloadTime;
            tag[defaultTags[21]] = this.clazz_DurabilityReloadTimeMax;
            tag[defaultTags[22]] = this.clazz_DurabilityReloadTimeAdd;
            tag[defaultTags[23]] = this.clazz_DurabilityReloadTimeMul;
            tag[defaultTags[24]] = this.clazz_DurabilityReloadSpeed;
            tag[defaultTags[25]] = this.clazz_DurabilityReloadSpeedAdd;
            tag[defaultTags[26]] = this.clazz_DurabilityReloadSpeedMul;

            tag[defaultTags[27]] = this.clazz_Blessing;
            tag[defaultTags[28]] = this.clazz_BlessingMax;
            tag[defaultTags[29]] = this.clazz_BlessingSpeed;
            tag[defaultTags[30]] = this.clazz_BlessingSpeedAdd;
            tag[defaultTags[31]] = this.clazz_BlessingSpeedMul;

            tag[defaultTags[32]] = this.clazz_Phantom;
            tag[defaultTags[33]] = this.clazz_PhantomMax;
            tag[defaultTags[34]] = this.clazz_PhantomState;
            tag[defaultTags[35]] = this.clazz_PhantomInvincible;
            tag[defaultTags[36]] = this.clazz_PhantomSpeed;
            tag[defaultTags[37]] = this.clazz_PhantomSpeedAdd;
            tag[defaultTags[38]] = this.clazz_PhantomSpeedMul;
            tag[defaultTags[39]] = this.clazz_PhantomDelSpeed;
            tag[defaultTags[40]] = this.clazz_PhantomDelSpeedAdd;
            tag[defaultTags[41]] = this.clazz_PhantomDelSpeedMul;
        }
        
        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);

            this.clazz = tag.ContainsKey(defaultTags[0]) ? tag.Get<byte>(defaultTags[0]) : (byte)defaults[0];
            this.GetTag(tag, 1, ref this.clazz_Rage);
            this.GetTag(tag, 2, ref this.clazz_RageMax);
            this.GetTag(tag, 3, ref this.clazz_RageSpeed);
            this.GetTag(tag, 4, ref this.clazz_RageSpeedAdd);
            this.GetTag(tag, 5, ref this.clazz_RageSpeedMul);
            this.GetTag(tag, 6, ref this.clazz_Raged);
            this.GetTag(tag, 7, ref this.clazz_RageWeak);
            this.GetTag(tag, 8, ref this.clazz_RageCD);
            
            this.GetTag(tag, 9, ref this.clazz_Power);
            this.GetTag(tag, 10, ref this.clazz_PowerMax);
            this.GetTag(tag, 11, ref this.clazz_PowerProgress);
            this.GetTag(tag, 12, ref this.clazz_PowerSpeed);
            this.GetTag(tag, 13, ref this.clazz_PowerSpeedAdd);
            this.GetTag(tag, 14, ref this.clazz_PowerSpeedMul);
            this.GetTag(tag, 15, ref this.clazz_PowerCD);
            
            this.GetTag(tag, 16, ref this.clazz_Durability);
            this.GetTag(tag, 17, ref this.clazz_DurabilityOld);
            this.GetTag(tag, 18, ref this.clazz_DurabilityMax);
            this.GetTag(tag, 19, ref this.clazz_DurabilityReload);
            this.GetTag(tag, 20, ref this.clazz_DurabilityReloadTime);
            this.GetTag(tag, 21, ref this.clazz_DurabilityReloadTimeMax);
            this.GetTag(tag, 22, ref this.clazz_DurabilityReloadTimeAdd);
            this.GetTag(tag, 23, ref this.clazz_DurabilityReloadTimeMul);
            this.GetTag(tag, 24, ref this.clazz_DurabilityReloadSpeed);
            this.GetTag(tag, 25, ref this.clazz_DurabilityReloadSpeedAdd);
            this.GetTag(tag, 26, ref this.clazz_DurabilityReloadSpeedMul);

            this.GetTag(tag, 27, ref this.clazz_Blessing);
            this.GetTag(tag, 28, ref this.clazz_BlessingMax);
            this.GetTag(tag, 29, ref this.clazz_BlessingSpeed);
            this.GetTag(tag, 30, ref this.clazz_BlessingSpeedAdd);
            this.GetTag(tag, 31, ref this.clazz_BlessingSpeedMul);

            this.GetTag(tag, 32, ref this.clazz_Phantom);
            this.GetTag(tag, 33, ref this.clazz_PhantomMax);
            this.GetTag(tag, 34, ref this.clazz_PhantomState);
            this.GetTag(tag, 35, ref this.clazz_PhantomInvincible);
            this.GetTag(tag, 36, ref this.clazz_PhantomSpeed);
            this.GetTag(tag, 37, ref this.clazz_PhantomSpeedAdd);
            this.GetTag(tag, 38, ref this.clazz_PhantomSpeedMul);
            this.GetTag(tag, 39, ref this.clazz_PhantomDelSpeed);
            this.GetTag(tag, 40, ref this.clazz_PhantomDelSpeedAdd);
            this.GetTag(tag, 41, ref this.clazz_PhantomDelSpeedMul);
        }

        public override void ResetEffects()
        {
            this.atkSpeed = 0f;
            this.dealtDamage = 0f;
            this.takenDamage = 0f;
            this.takenDamageFromEnemies = 0f;

            this.state_Wrath = this.clazz_Raged ? (int)this.clazz_Rage : 0;
            this.state_Cowardice = this.clazz_RageWeak > 0 ? this.clazz_RageWeak : 0;
            this.state_Blasphemy = this.clazz_Blessing / this.clazz_BlessingMax < .2f ? 2 : 0;
            this.state_Fluster = 0;
            for (int i = 0; i < Main.maxNPCs && this.state_Fluster == 0; i++)
                if (Main.npc[i].active)
                    if (Main.npc[i].type != 488 && !Main.npc[i].townNPC && !Main.npc[i].friendly)
                        if (Vector2.Distance(Main.npc[i].Center, player.Center) < 160.0)//range
                            this.state_Fluster = 2;
            this.state_Restoring = this.clazz_DurabilityReloadTime > 0 ? (int)this.clazz_DurabilityReloadTime : 0;
            this.state_Wraith = this.clazz_PhantomState ? (int)this.clazz_Phantom : 0;
            
            for (short i = 1; i < CNCID.State.MaxObject; i++)
            {
                bool flag = false;
                if (i == CNCID.State.Wrath) flag = true;
                if (i == CNCID.State.Restoring) flag = true;
                if (i == CNCID.State.Wraith) flag = true;
                int value = flag ? 0 : this.state[i - 1];
                if (value > 0) { if (!this._stateType.Contains(i)) this._stateType.Add(i); }
                else if (this._stateType.Contains(i)) this._stateType.Remove(i);
            }
            for (short i = 1; i < CNCID.State.MaxObject; i++)
            {
                if (this._stateType.Contains(i)) this._stateTime[i - 1] = this.state[i - 1];
                else this.state[i - 1] = 0;
            }
            this.ResetClassVar(false);
            this.Update();
        }

        public void Update()
        {
            this.UpdateClassVar(CNCID.Clazz.Barbarian, 1);
            this.UpdateClassVar(CNCID.Clazz.Barbarian, 2);
            this.UpdateClassVar(CNCID.Clazz.Crusader, 0);
            this.UpdateClassVar(CNCID.Clazz.Cleric, 0);
            this.UpdateClassVar(CNCID.Clazz.Ranger, 0);
            this.UpdateClassVar(CNCID.Clazz.Musketeer, 0);
            this.UpdateClassVar(CNCID.Clazz.Musketeer, 2);
            this.UpdateClassVar(CNCID.Clazz.Assassin, 0);
            this.UpdateClassVar(CNCID.Clazz.Assassin, 1);

            if (modPlayer.clazz == CNCID.Clazz.Barbarian)
            {
                if (modPlayer.clazz_Raged)
                {
                    modPlayer.atkSpeed += .2f;
                    player.moveSpeed += .2f;
                    player.GetDamage(DamageClass.Generic) += .1f;
                }
                if (modPlayer.clazz_RageWeak > 0)
                {
                    modPlayer.atkSpeed -= .2f;
                    player.moveSpeed -= .2f;
                    player.GetDamage(DamageClass.Generic) -= .1f;
                }
            }
            if (modPlayer.clazz == CNCID.Clazz.Crusader || modPlayer.clazz == CNCID.Clazz.Cleric)
            {
                if (modPlayer.state_Blasphemy > 0)
                {
                    player.moveSpeed -= .2f;
                    modPlayer.takenDamage += .1f;
                }
            }

            if (modPlayer.clazz == CNCID.Clazz.Ranger)
            {
            }

            if (modPlayer.clazz == CNCID.Clazz.Musketeer)
            {
                if (modPlayer.clazz_DurabilityReload)
                {
                    player.moveSpeed -= .35f;
                    modPlayer.takenDamage += .2f;
                }
                else player.moveSpeed += .1f;
            }

            if (modPlayer.clazz == CNCID.Clazz.Assassin)
            {
                if (modPlayer.clazz_PhantomState)
                {
                    player.moveSpeed += .4f;
                    //player.GetDamage<StabbedDamage>() += .5f;
                }
                if (modPlayer.clazz_PhantomInvincible > 0) modPlayer.takenDamageFromEnemies = -9999f;
            }
            
            if (player.itemTime > 0)
            {
                int num;
                modPlayer.useTime += modPlayer.atkSpeed * player.itemTimeMax / 60f;
                if (modPlayer.useTime >= 1f)
                {
                    num = (int)Math.Floor(modPlayer.useTime);
                    player.itemTime -= num;
                    modPlayer.useTime -= num;
                    if (player.itemTime <= 0) player.itemTime = 0;
                    if (modPlayer.useTime <= 0) modPlayer.useTime = 0;
                }
                if (modPlayer.useTime <= -1f)
                {
                    num = 1;// (int)Math.Ceiling(modPlayer.useTime);
                    player.itemTime += num;
                    modPlayer.useTime += num;
                }
            }
            if (player.itemAnimation > 0)
            {
                int num;
                modPlayer.animTime += modPlayer.atkSpeed * player.itemAnimationMax / 60f;
                if (modPlayer.animTime >= 1f)
                {
                    num = (int)Math.Floor(modPlayer.animTime);
                    player.itemAnimation -= num;
                    modPlayer.animTime -= num;
                    if (player.itemAnimation <= 0) player.itemAnimation = 0;
                    if (modPlayer.animTime <= 0) modPlayer.animTime = 0;
                }
                if (modPlayer.animTime <= -1f)
                {
                    num = 1;// (int)Math.Ceiling(modPlayer.animTime);
                    player.itemAnimation += num;
                    modPlayer.animTime += num;
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ClassKey.JustPressed)
            {
                if (modPlayer.clazz == CNCID.Clazz.Musketeer)
                {
                    if (modPlayer.clazz_DurabilityReload && ((float)modPlayer.clazz_Durability / modPlayer.clazz_DurabilityMax) >= .35f)
                    {
                        modPlayer.clazz_DurabilityReload = false;
                    }
                    else if (!modPlayer.clazz_DurabilityReload && modPlayer.clazz_Durability < modPlayer.clazz_DurabilityMax)
                    {
                        modPlayer.clazz_DurabilityReload = true;
                        SoundEngine.PlaySound(40, player.position, 1);
                    }
                }
                if (modPlayer.clazz == CNCID.Clazz.Assassin && modPlayer.clazz_Phantom >= modPlayer.clazz_PhantomMax)
                {
                    modPlayer.clazz_PhantomState = true;
                    modPlayer.clazz_PhantomInvincible = 120;
                }
            }
        }

        public void ResetClassVar(bool all = false, Player player = null)
        {
            if (player == null)
            {
                if (all) this.clazz_Rage = (float)defaults[1];
                this.clazz_RageMax = (float)defaults[2];
                this.clazz_RageSpeed = (float)defaults[3];
                this.clazz_RageSpeedAdd = (float)defaults[4];
                this.clazz_RageSpeedMul = (float)defaults[5];
                if (all) this.clazz_Raged = (bool)defaults[6];
                if (all) this.clazz_RageWeak = (int)defaults[7];
                if (all) this.clazz_RageCD = (int)defaults[8];

                if (all) this.clazz_Power = (float)defaults[9];
                this.clazz_PowerMax = (float)defaults[10];
                if (all) this.clazz_PowerProgress = (float)defaults[11];
                this.clazz_PowerSpeed = (float)defaults[12];
                this.clazz_PowerSpeedAdd = (float)defaults[13];
                this.clazz_PowerSpeedMul = (float)defaults[14];
                if (all) this.clazz_PowerCD = (int)defaults[15];

                if (all) this.clazz_Durability = (int)defaults[16];
                if (all) this.clazz_DurabilityOld = (int)defaults[17];
                this.clazz_DurabilityMax = (int)defaults[18];
                if (all) this.clazz_DurabilityReload = (bool)defaults[19];
                if (all) this.clazz_DurabilityReloadTime = (float)defaults[20];
                if (all) this.clazz_DurabilityReloadTimeMax = (float)defaults[21];
                this.clazz_DurabilityReloadTimeAdd = (float)defaults[22];
                this.clazz_DurabilityReloadTimeMul = (float)defaults[23];
                this.clazz_DurabilityReloadSpeed = (float)defaults[24];
                this.clazz_DurabilityReloadSpeedAdd = (float)defaults[25];
                this.clazz_DurabilityReloadSpeedMul = (float)defaults[26];

                if (all) this.clazz_Blessing = (float)defaults[27];
                this.clazz_BlessingMax = (float)defaults[28];
                this.clazz_BlessingSpeed = (float)defaults[29];
                this.clazz_BlessingSpeedAdd = (float)defaults[30];
                this.clazz_BlessingSpeedMul = (float)defaults[31];

                if (all) this.clazz_Phantom = (float)defaults[32];
                this.clazz_PhantomMax = (float)defaults[33];
                if (all) this.clazz_PhantomState = (bool)defaults[34];
                if (all) this.clazz_PhantomInvincible = (int)defaults[35];
                this.clazz_PhantomSpeed = (float)defaults[36];
                this.clazz_PhantomSpeedAdd = (float)defaults[37];
                this.clazz_PhantomSpeedMul = (float)defaults[38];
                this.clazz_PhantomDelSpeed = (float)defaults[39];
                this.clazz_PhantomDelSpeedAdd = (float)defaults[40];
                this.clazz_PhantomDelSpeedMul = (float)defaults[41];
            }
            else
            {
                ClassesSettings modP = player.GetModPlayer<ClassesSettings>();
                if (all) modP.clazz_Rage = (float)defaults[1];
                modP.clazz_RageMax = (float)defaults[2];
                modP.clazz_RageSpeed = (float)defaults[3];
                modP.clazz_RageSpeedAdd = (float)defaults[4];
                modP.clazz_RageSpeedMul = (float)defaults[5];
                if (all) modP.clazz_Raged = (bool)defaults[6];
                if (all) modP.clazz_RageWeak = (int)defaults[7];
                if (all) modP.clazz_RageCD = (int)defaults[8];

                if (all) modP.clazz_Power = (float)defaults[9];
                modP.clazz_PowerMax = (float)defaults[10];
                if (all) modP.clazz_PowerProgress = (float)defaults[11];
                modP.clazz_PowerSpeed = (float)defaults[12];
                modP.clazz_PowerSpeedAdd = (float)defaults[13];
                modP.clazz_PowerSpeedMul = (float)defaults[14];
                if (all) modP.clazz_PowerCD = (int)defaults[15];

                if (all) modP.clazz_Durability = (int)defaults[16];
                if (all) modP.clazz_DurabilityOld = (int)defaults[17];
                modP.clazz_DurabilityMax = (int)defaults[18];
                if (all) modP.clazz_DurabilityReload = (bool)defaults[19];
                if (all) modP.clazz_DurabilityReloadTime = (float)defaults[20];
                if (all) modP.clazz_DurabilityReloadTimeMax = (float)defaults[21];
                modP.clazz_DurabilityReloadTimeAdd = (float)defaults[22];
                modP.clazz_DurabilityReloadTimeMul = (float)defaults[23];
                modP.clazz_DurabilityReloadSpeed = (float)defaults[24];
                modP.clazz_DurabilityReloadSpeedAdd = (float)defaults[25];
                modP.clazz_DurabilityReloadSpeedMul = (float)defaults[26];

                if (all) modP.clazz_Blessing = (float)defaults[27];
                modP.clazz_BlessingMax = (float)defaults[28];
                modP.clazz_BlessingSpeed = (float)defaults[29];
                modP.clazz_BlessingSpeedAdd = (float)defaults[30];
                modP.clazz_BlessingSpeedMul = (float)defaults[31];

                if (all) modP.clazz_Phantom = (float)defaults[32];
                modP.clazz_PhantomMax = (float)defaults[33];
                if (all) modP.clazz_PhantomState = (bool)defaults[34];
                if (all) modP.clazz_PhantomInvincible = (int)defaults[35];
                modP.clazz_PhantomSpeed = (float)defaults[36];
                modP.clazz_PhantomSpeedAdd = (float)defaults[37];
                modP.clazz_PhantomSpeedMul = (float)defaults[38];
                modP.clazz_PhantomDelSpeed = (float)defaults[39];
                modP.clazz_PhantomDelSpeedAdd = (float)defaults[40];
                modP.clazz_PhantomDelSpeedMul = (float)defaults[41];
            }
        }

        private void UpdateClassVar(byte clazz, int style = 0)
        {
            if (modPlayer.clazz != clazz) return;
            float max;
            float value;
            float add;
            float mul;
            if (clazz == CNCID.Clazz.Barbarian)
            {
                max = modPlayer.clazz_RageMax;
                value = modPlayer.clazz_RageSpeed;
                add = modPlayer.clazz_RageSpeedAdd;
                mul = modPlayer.clazz_RageSpeedMul;
                if (style == 0)//常规：攒气
                {
                    if (modPlayer.clazz_RageCD > 0) return;
                    modPlayer.clazz_RageCD = 24;
                    modPlayer.clazz_Rage += (value + value * add) * mul;
                    if (modPlayer.clazz_Rage > max) modPlayer.clazz_Rage = max;
                    if (!modPlayer.clazz_Raged && modPlayer.clazz_Rage >= max) modPlayer.clazz_Raged = true;
                }
                if (style == 1)//狂怒
                {
                    if (!modPlayer.clazz_Raged) return;
                    modPlayer.clazz_Rage -= 3 / 60f;
                    if (modPlayer.clazz_Rage < 5)
                    {
                        modPlayer.clazz_RageCD = 300;
                        modPlayer.clazz_RageWeak = 300;
                        modPlayer.clazz_Raged = false;
                    }
                }
                if (style == 2)//泄气/冷却
                {
                    modPlayer.clazz_RageCD--;
                    modPlayer.clazz_RageWeak--;
                    if (modPlayer.clazz_RageWeak <= 0) modPlayer.clazz_RageWeak = 0;
                    if (modPlayer.clazz_RageWeak > 0 && modPlayer.clazz_RageCD <= 0) modPlayer.clazz_RageCD = 1;
                }
            }
            if (clazz == CNCID.Clazz.Crusader || clazz == CNCID.Clazz.Cleric)
            {
                max = modPlayer.clazz_BlessingMax;
                value = modPlayer.clazz_BlessingSpeed;
                add = modPlayer.clazz_BlessingSpeedAdd;
                mul = modPlayer.clazz_BlessingSpeedMul;
                if (style == 0)//常规
                {
                    modPlayer.clazz_Blessing += (value + value * add) * mul;
                    if (modPlayer.clazz_Blessing > max) modPlayer.clazz_Blessing = max;
                }
            }
            if (clazz == CNCID.Clazz.Ranger)
            {
                max = modPlayer.clazz_PowerMax;
                value = modPlayer.clazz_PowerSpeed;
                add = modPlayer.clazz_PowerSpeedAdd;
                mul = modPlayer.clazz_PowerSpeedMul;
                if (style == 0)//常规
                {
                    modPlayer.clazz_Power += (value + value * add) * mul;
                    if (modPlayer.clazz_Power > max) modPlayer.clazz_Power = max;
                    if (player.itemAnimation == 0) modPlayer.clazz_PowerProgress = modPlayer.clazz_Power / max;
                }
                if (style == 1)//消耗
                {
                    modPlayer.clazz_Power = 0;
                    modPlayer.clazz_PowerCD = 0;
                }
            }
            if (clazz == CNCID.Clazz.Musketeer)
            {
                if (style == 0)//常规
                {
                    if (!modPlayer.clazz_DurabilityReload && modPlayer.clazz_Durability <= 0) modPlayer.clazz_DurabilityReload = true;
                    if (modPlayer.clazz_DurabilityReload) return;
                    modPlayer.clazz_DurabilityOld = modPlayer.clazz_Durability;
                    modPlayer.clazz_DurabilityReloadTimeMax = modPlayer.clazz_DurabilityReloadTime;
                }
                if (style == 1)//开枪
                {
                    max = modPlayer.clazz_DurabilityReloadTimeMax;
                    value = player.itemAnimationMax;
                    add = modPlayer.clazz_DurabilityReloadTimeAdd;
                    mul = modPlayer.clazz_DurabilityReloadTimeMul;
                    modPlayer.clazz_Durability--;
                    modPlayer.clazz_DurabilityReloadTime += (value + value * add) * mul;
                    modPlayer.clazz_DurabilityReload = false;
                    if (modPlayer.clazz_Durability <= 0)
                    {
                        modPlayer.clazz_Durability = 0;
                        modPlayer.clazz_DurabilityReload = true;
                        SoundEngine.PlaySound(40, player.position, 1);
                    }
                    modPlayer.clazz_DurabilityOld = modPlayer.clazz_Durability;
                    modPlayer.clazz_DurabilityReloadTimeMax = modPlayer.clazz_DurabilityReloadTime;
                }
                if (style == 2)//填充
                {
                    if (!modPlayer.clazz_DurabilityReload) return;
                    max = modPlayer.clazz_DurabilityReloadTimeMax;
                    value = modPlayer.clazz_DurabilityReloadSpeed;
                    add = modPlayer.clazz_DurabilityReloadSpeedAdd;
                    mul = modPlayer.clazz_DurabilityReloadSpeedMul;
                    float tickPerAmmo = max / (modPlayer.clazz_DurabilityMax - modPlayer.clazz_DurabilityOld);
                    modPlayer.clazz_DurabilityReloadTime -= (value + value * add) * mul;
                    if (max - modPlayer.clazz_DurabilityReloadTime >= (modPlayer.clazz_Durability - modPlayer.clazz_DurabilityOld + 1) * tickPerAmmo)
                    {
                        modPlayer.clazz_Durability++;
                    }
                    if (modPlayer.clazz_Durability >= modPlayer.clazz_DurabilityMax)
                    {
                        modPlayer.clazz_DurabilityReload = false;
                        modPlayer.clazz_Durability = modPlayer.clazz_DurabilityMax;
                        modPlayer.clazz_DurabilityReloadTime = 0;
                        modPlayer.clazz_DurabilityReloadTimeMax = 0;
                        SoundEngine.PlaySound(40, player.position, 1);
                    }
                }
            }
            if (clazz == CNCID.Clazz.Assassin)
            {
                if (style == 0)//常规
                {
                    if (modPlayer.clazz_PhantomState) return;
                    max = modPlayer.clazz_PhantomMax;
                    value = modPlayer.clazz_PhantomSpeed;
                    add = modPlayer.clazz_PhantomSpeedAdd;
                    mul = modPlayer.clazz_PhantomSpeedMul;
                    modPlayer.clazz_Phantom += (value + value * add) * mul;
                    if (modPlayer.clazz_Phantom >= max) modPlayer.clazz_Phantom = max;
                }
                if (style == 1)//消耗
                {
                    if (!modPlayer.clazz_PhantomState) return;
                    max = modPlayer.clazz_PhantomMax;
                    value = modPlayer.clazz_PhantomDelSpeed;
                    add = modPlayer.clazz_PhantomDelSpeedAdd;
                    mul = modPlayer.clazz_PhantomDelSpeedMul;
                    modPlayer.clazz_Phantom -= (value + value * add) * mul;
                    modPlayer.clazz_PhantomInvincible--;
                    if (modPlayer.clazz_PhantomInvincible <= 0)
                    {
                        modPlayer.clazz_PhantomInvincible = 0;
                    }
                    if (modPlayer.clazz_Phantom <= 0)
                    {
                        modPlayer.clazz_Phantom = 0;
                        modPlayer.clazz_PhantomState = false;
                    }
                }
                if (style == 2)//清空
                {
                    if (!modPlayer.clazz_PhantomState || modPlayer.clazz_PhantomInvincible > 0) return;
                    modPlayer.clazz_Phantom = 0;
                    modPlayer.clazz_PhantomState = false;
                }
            }
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (modPlayer.clazz == CNCID.Clazz.Ranger)
            {
                if (item.useAmmo == AmmoID.Arrow) UpdateClassVar(CNCID.Clazz.Ranger, 1);
            }
            if (modPlayer.clazz == CNCID.Clazz.Musketeer)
            {
                if (item.useAmmo == AmmoID.Bullet) UpdateClassVar(CNCID.Clazz.Musketeer, 1);
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }

        public override bool CanUseItem(Item item)
        {
            if (modPlayer.clazz == CNCID.Clazz.Musketeer)
            {
                if (base.CanUseItem(item))
                    if (item.useAmmo == AmmoID.Bullet)
                        if (modPlayer.clazz_DurabilityReload)
                            if (((float)modPlayer.clazz_Durability / modPlayer.clazz_DurabilityMax) < .35f)
                                return false;
            }
            return base.CanUseItem(item);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            bool flag = base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
            damage += (int)(damage * modPlayer.takenDamage);
            if (damage < 1) damage = 1;
            return flag;
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            this.UpdateClassVar(CNCID.Clazz.Barbarian, 0);
            this.UpdateClassVar(CNCID.Clazz.Assassin, 2);
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            this.UpdateClassVar(CNCID.Clazz.Barbarian, 0);
            this.UpdateClassVar(CNCID.Clazz.Assassin, 2);
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if(target.type != 488) this.UpdateClassVar(CNCID.Clazz.Barbarian, 0);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (target.type != 488) this.UpdateClassVar(CNCID.Clazz.Barbarian, 0);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            damage += (int)(damage * modPlayer.takenDamageFromEnemies);
            if (damage < 1) damage = 1;
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (proj.hostile) damage += (int)(damage * modPlayer.takenDamageFromEnemies);
            if (damage < 1) damage = 1;
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            damage += (int)(damage * modPlayer.dealtDamage);
            if (damage < 1) damage = 1;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (modPlayer.clazz == CNCID.Clazz.Musketeer)
            {
                if (target.GetLifePercent() > .75f && proj.GetGlobalProjectile<AmmoType>().bullet) damage += (int)(damage * .25f);
            }
            damage += (int)(damage * modPlayer.dealtDamage);
            if (damage < 1) damage = 1;
        }

        public override void ModifyWeaponCrit(Item item, ref int crit)
        {
            if (modPlayer.clazz == CNCID.Clazz.Ranger)
            {
                if (item.useAmmo == AmmoID.Arrow)
                {
                    if (modPlayer.state_Fluster > 0) crit -= 10;
                    crit += (int)(modPlayer.clazz_PowerProgress * 30);
                }
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage, ref float flat)
        {
            if (modPlayer.clazz == CNCID.Clazz.Ranger)
            {
                if (item.useAmmo == AmmoID.Arrow)
                {
                    if (modPlayer.state_Fluster > 0) damage -= .3f;
                    damage += modPlayer.clazz_PowerProgress * .6f;
                }
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            //DrawShadow(drawInfo, Color.Red);
        }

        public void DrawShadow(PlayerDrawSet drawInfo, Color color)
        {
            if (!drawInfo.drawPlayer.ShouldNotDraw)
            {
                Player player = drawInfo.drawPlayer;
                int max = player.shadowPos.Length;
                if (max > player.shadowOrigin.Length) max = player.shadowOrigin.Length;
                if (max > player.shadowRotation.Length) max = player.shadowRotation.Length;
                for (int i = 0; i < max; i++)
                {
                    Texture2D image = null;
                    if (0 <= player.legs && player.legs < TextureAssets.ArmorLeg.Length) image = TextureAssets.ArmorLeg[player.legs].Value;
                    if (image != null)
                    {
                        Vector2 basePos = player.shadowPos[i] - Main.screenPosition;
                        Vector2 origin = player.legFrame.Center.ToVector2();//player.shadowOrigin[i];//drawInfo.legVect;
                        Vector2 pos = basePos + player.legPosition + origin;// - new Vector2(player.legFrame.Width / 2, player.legFrame.Height + 4);
                        //Color color = Color.Red;// drawInfo.colorArmorLegs;
                        DrawData legs = new DrawData(image, pos, player.legFrame, color, player.shadowRotation[i], origin, 1.1f * Main.mouseTextColor / 255f, drawInfo.playerEffect, 0);
                        legs.shader = drawInfo.cLegs;
                        drawInfo.DrawDataCache.Add(legs);
                    }
                }
            }
        }

        class AmmoType : GlobalProjectile
        {
            public bool arrow;
            public bool bullet;
            private List<int> arrowList;
            private List<int> bulletList;

            public override bool CloneNewInstances => true;
            public override bool InstancePerEntity => true;

            public override void Load()
            {
                this.arrowList = new List<int>();
                this.bulletList = new List<int>();
                for (int i = 0; i < ItemLoader.ItemCount; i++)
                {
                    if (ItemLoader.GetItem(i) == null) continue;
                    if (ItemLoader.GetItem(i).Item.ammo == AmmoID.Arrow)
                        this.arrowList.Add(ItemLoader.GetItem(i).Item.shoot);
                    if (ItemLoader.GetItem(i).Item.ammo == AmmoID.Bullet)
                        this.bulletList.Add(ItemLoader.GetItem(i).Item.shoot);
                }
                for (int i = 0; i < Main.maxItemTypes; i++)
                {
                    Item item = new Item();
                    item.SetDefaults(i);
                    if (item.ammo == AmmoID.Arrow) this.arrowList.Add(item.shoot);
                    if (item.ammo == AmmoID.Bullet) this.bulletList.Add(item.shoot);
                }
            }

            public override void Unload()
            {
                if (this.arrowList != null)
                {
                    this.arrowList.Clear();
                    this.arrowList = null;
                }
                if (this.bulletList != null)
                {
                    this.bulletList.Clear();
                    this.bulletList = null;
                }
            }

            public override void SetDefaults(Projectile projectile)
            {
                projectile.GetGlobalProjectile<AmmoType>().arrow = this.arrowList.Contains(projectile.type);
                projectile.GetGlobalProjectile<AmmoType>().bullet = this.bulletList.Contains(projectile.type);
            }
        }
    }
}