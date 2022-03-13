
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CNC.Common.Utils;
using CNC.Common.Global.NPCs;
using Terraria.ModLoader;

namespace CNC.Content.NPCs
{
    public class TranquilityPixie : CNCNPC
    {
        //重新优化：回避凸起地AI，蓄力攻击聚气特效，发射弹幕特效（k），弹幕追踪方向，帧图错误修正
        
        const int dustid = DustId.CyanShortFx1;

        const int damage = 25;
        const int StandFrame = 12;
        const int AttackFrame = 12;
        const int DeathFrame = 3;
        int var = 0;
        int spawn = 255;

        State state { get => (State)var1; set => var1 = (int)value; }
        double time { get => (double)var2; set => var2 = (float)value; }
        int targetID { get => (int)var3; set => var3 = (int)value; }

        private enum State : byte
        {
            NoTarget = 1,
            ToAttack = 2,
            Attacking = 3,
            Death = 4
        }

        public override void ResetStaticDefaults()
        {
            countFrame = 27;
        }

        public override void ResetDefaults()
        {
            npc.width = 60;
            npc.height = 108;
            npc.lifeMax = 130;
            npc.damage = 35;
            npc.defense = 10;
            npc.value = 500f;
            npc.noGravity = true;
            npc.alpha = (int)(var4 = 255);
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            var1 = var2 = 0;
            var3 = -1;
            spawn = 255;
        }

        public override void ResetBestiary(ref BestiaryDatabase database, ref BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] { BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface });
        }

        public override void AI()
        {
            float tick = 2f - 1f * npc.GetLifePercent();//蓄力速度
            float attackDelay = 60f * (Main.expertMode ? 2 : 3);//最大蓄力时长
            float velMax = 3f / (state == State.Attacking ? 2 : 1);//移动速度
            if (var1 != 1 && var1 != 2 && var1 != 3 && var1 != 4) { state = State.NoTarget; }//状态数值不在范围内时，重置为漫游状态
            if (state != State.Death)
            {
                if (!npc.HasPlayerTarget) { state = State.NoTarget; }//没有目标时，重置为漫游状态
                else if (state != State.ToAttack && state != State.Attacking) { state = State.ToAttack; }
            }
            if (state == State.NoTarget)//当没有目标时
            {
                time = 0f;//清空蓄力
                npc.velocity.X = Math.Abs(npc.velocity.X) < 0.2f ? 0 : npc.velocity.Y * 0.8f;//减速
                npc.velocity.Y = npc.velocity.Y > velMax ? velMax : npc.velocity.Y + 0.2f;//缓慢降落
            }
            if (state == State.ToAttack || state == State.Attacking)//当有目标时
            {
                if (npc.HasPlayerTarget && Main.player[targetID] != null)//检测目标是否存活
                {
                    Player player = Main.player[targetID];
                    if (!player.active || player.dead || player.respawnTimer > 0)//若目标玩家已死亡或不存在
                    {
                        state = State.NoTarget;//回到漫游状态
                        targetID = -1;
                    }
                    else //否则追击目标玩家
                    {
                        npc.direction = player.position.X - npc.position.X > 0 ? 1 : -1;
                        Vector2 pos1 = player.Center;
                        Vector2 pos2 = npc.Center;
                        Vector2 distVec2 = pos1 - pos2;
                        float rot = distVec2.ToRotation();
                        Vector2 vel = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot)) * velMax / 8f;
                        npc.velocity += vel;
                        if (Math.Abs(npc.velocity.X) > velMax) { npc.velocity.X = (npc.velocity.X < 0 ? -1 : 1) * velMax; }
                        if (Math.Abs(npc.velocity.Y) > velMax / 2) { npc.velocity.Y = (npc.velocity.Y < 0 ? -1 : 1) * velMax / 2; }
                        if (state == State.ToAttack)//蓄力阶段
                        {
                            if (time >= attackDelay) { state = State.Attacking; }//蓄力完成时，进入攻击状态并清空蓄力
                            time = time >= attackDelay ? 0 : time + tick;//否则进行蓄力
                        }
                    }
                }
            }
            FindTarget();//寻找最近的目标玩家
            if (Main.rand.Next(0, 30) == 0) CNCUtils.NewDust(npc, dustid, Main.rand.NextFloat(MathHelper.Pi), 1.2f, default, default(Color));
            Vector2 pos = (npc.position + npc.Size / 2f) / 16f;
            Color color = new Color(0, 234, 118);
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;
            Lighting.AddLight((int)pos.X, (int)pos.Y, r, g, b);
            if (spawn > 0) npc.alpha = (spawn -= 5);
        }

        public override void ModifyFrame(ref bool autoAnim, ref int[] frameTime, ref int frame, int frameHeight)
        {
            autoAnim = false;
            double counter = npc.frameCounter;
            double counterMax = state != State.Attacking ? 7 : 2;
            bool nextFrame = counter >= counterMax;
            if (state == State.ToAttack && frame >= StandFrame - 1 && nextFrame)//若进入攻击状态，但非攻击动画
            { frame = StandFrame; npc.frameCounter = 0; nextFrame = false; }//进入攻击动画第一帧
            if (state == State.Attacking && frame == StandFrame + 8 - 1 && npc.frameCounter == 0 && npc.HasPlayerTarget)//在攻击动画的特定帧发起攻击
            {
                for (int i = 0; i < 30; i++)//发射特效
                {
                    float x = npc.Center.X + (16f + npc.velocity.X * 4f) * npc.direction;
                    float y = npc.Center.Y;
                    float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(1f), Main.rand.NextFloat(-1, 1) };
                    Vector4 xywh = new Vector4(x, y, 9, 9);
                    int id = dustid;
                    Dust dust = CNCUtils.NewDust(xywh, id, rand[0], Main.rand.NextFloat(1.8f, 2.6f) * 1.2f, default, default(Color));
                    dust.alpha = 0;
                    dust.position = new Vector2(x, y);
                    dust.velocity = new Vector2(rand[1] * npc.direction, rand[2]) * 10f;
                    dust.noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item20, npc.position);
                float rot = (Main.player[targetID].position - npc.Center).ToRotation();
                Vector2 pos = npc.Center + new Vector2(16f + npc.velocity.X * 4f, 0f) * npc.direction;
                Vector2 vel = new Vector2(MyMath.cos(rot), MyMath.sin(rot)) * 8f;
                int proj = CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritMass];
                int projDmg = (int)(damage * 1.2f);
                CNCUtils.NewProj(npc.GetSpawnSource_ForProjectile(), pos, vel, proj, projDmg, 0f, -1, targetID + 1);//发射弹幕
            }
            if (state == State.Attacking && frame >= StandFrame + AttackFrame - 1)//结束攻击动画并退出攻击状态
            {
                state = State.ToAttack;
                time = 0;
                npc.frameCounter = 0;
            }
            if (nextFrame)
            {
                frame++;
                npc.frameCounter = 0;
                nextFrame = false;
                /*
                if (state == State.Attacking && frame >= StandFrame && frame < StandFrame + AttackFrame - 1)
                {
                    int alpha = npc.alpha;
                    if (alpha == 0) { npc.alpha = 1; }
                    else if (alpha == 1) { npc.alpha = 127; }
                    else if (alpha == 127) { npc.alpha = 128; }
                    else if (alpha == 128) { npc.alpha = 0; }
                }
                else
                {
                    var = (var + 8) % 512;
                    if (var < 256) npc.alpha = var;
                    else npc.alpha = 512 - var;
                    npc.alpha = (int)(npc.alpha * .75);
                }
                */
                var = (var + 16) % 512;
                if (var < 256) npc.alpha = var;
                else npc.alpha = 512 - var;
                npc.alpha = (int)(npc.alpha * .6);
            }
            else { npc.frameCounter++; }
            if (var4 > 0) npc.alpha = (int)var4--;
            if (state != State.Death)
            {
                if (frame >= StandFrame + AttackFrame && state == State.Attacking) { frame = 0; }//完成发射弹幕动画，回到常规动画第一帧
                if (frame >= StandFrame && state != State.Attacking) { frame = 0; }//防止非发射弹幕状态下，播放发射弹幕动画
                if (frame >= StandFrame + AttackFrame && state != State.Death) { frame = 0; }//防止非消逝状态下，播放消逝动画
            }
            else if (frame < StandFrame + AttackFrame) { frame = StandFrame + AttackFrame; }
            if (frame >= StandFrame + AttackFrame + DeathFrame) { frame--; npc.alpha = 255; CheckDead(); }
        }
        
        public virtual void HitBy(Player attacker, Entity entity, int damage, float knockback, bool crit)
        {
            npc.velocity.X = npc.direction * -knockback;
            npc.velocity.Y = -knockback / 3f;
            for(int i = 0; i < 3; i++)
            { CNCUtils.NewDust(npc, dustid, Main.rand.NextFloat(MyMath.pi), 3f, default, npc.velocity); }
        }
        
        public override bool CheckDead()
        {
            if (state != State.Death)
            {
                state = State.Death;
                npc.velocity = Vector2.Zero;
                npc.active = true;
                npc.life = 1;
                npc.dontCountMe = true;
                npc.dontTakeDamage = true;
                npc.dontTakeDamageFromHostiles = true;
                npc.NPCLoot();
                for (int i = 0; i < 15; i++)
                {
                    float x = npc.Center.X;
                    float y = npc.Center.Y;
                    float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1) };
                    Vector4 xywh = new Vector4(x, y, 9, 9);
                    int id = DustId.CyanFx;
                    Dust dust = CNCUtils.NewDust(xywh, id, rand[0], Main.rand.NextFloat(1.8f, 2.6f), default, new Color(0, 234, 118));
                    dust.position = npc.Center;
                    dust.velocity = new Vector2(rand[1], rand[2]) * 8f;
                    dust.noGravity = true;
                }
                return false;
            }
            else
            {
                npc.active = false;
                npc.life = 0;
                return true;
            }
        }

        private void FindTarget(bool refindTarget = false)
        {
            if (state != State.NoTarget) { if (refindTarget) { state = State.NoTarget; } else return; }
            const float range = 16f * 48f;
            Player target = null;
            float dist = 0;
            float closestDist = range;
            bool lineOfSight = false;
            bool inRange = false;
            bool closer = false;
            if (Main.netMode == 0)//单人模式下，只有自己一个玩家，此处只需判定自己与该npc的条件即可（节省计算）
            {
                if (Const.Me.active && !Const.Me.dead)//若玩家存活，则进行判定
                {
                    target = Const.Me;
                    lineOfSight = Collision.CanHitLine(target.position, target.width, target.height, npc.position, npc.width, npc.height);
                    dist = Vector2.Distance(target.Center, npc.Center);
                    inRange = dist < range;
                    npc.target = targetID = inRange ? Const.me : -1;
                }
                else { npc.target = targetID = -1; }//否则清空目标编号
            }
            else
            {
                for (int i = 0; i < Const.max2; i++)
                {
                    if (!Main.player[i].active || Main.player[i].dead || Main.player[i] == null) { continue; }
                    Player target2 = Main.player[i];
                    lineOfSight = Collision.CanHitLine(npc.position, npc.width, npc.height, target.position, target.width, target.height);//与目标间有无物块阻拦
                    dist = Vector2.Distance(target.Center, npc.Center);//目标距离
                    inRange = dist < range;//目标是否在范围内
                    closer = dist < closestDist;//与该目标是否比原目标更近
                    if (!inRange || !closer || !lineOfSight) { continue; }
                    {
                        closestDist = dist;//记录与新目标的距离
                        target = target2;//原目标改为新目标
                    }
                }
                if (target.active && target.dead && target != null)//若最终目标存活
                { npc.target = targetID = target.whoAmI; }//记录目标编号
                else { targetID = -1; }//否则清空目标编号
            }
        }
    }
}