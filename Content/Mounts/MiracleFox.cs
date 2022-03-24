using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Localization;
using Terraria.DataStructures;

namespace CNC.Content.Mounts
{
    public class MiracleFox : ModMount
    {
        private int frameCount = 0;
        public override void SetStaticDefaults()
        {
            // Movement
            MountData.jumpHeight = 15; //跳跃高度（格）
            MountData.acceleration = 0.5f; //加速度
            MountData.jumpSpeed = 6.8f; //跳跃初速度
            MountData.blockExtraJumps = false; //二段跳
            MountData.constantJump = true; //按紧跳跃键自动跳跃
            MountData.heightBoost = 34; //骑乘时人物高度升高
            MountData.fallDamage = 0.2f; // 摔落伤害倍率
            MountData.runSpeed = 6f; //奔跑速度
            MountData.dashSpeed = 8f; //冲刺速度需求
            MountData.flightTimeMax = 0; //飞行时长
            MountData.xOffset = 0;
            MountData.yOffset = -14;
            MountData.lightColor = new Vector3(.5f);
            MountData.spawnDust = ModContent.DustType<Dusts.Star>();

            // Misc
            MountData.fatigueMax = 0;//坐骑疲劳
            MountData.buff = CNCID.Buff.LegacyCollection[Name]; //坐骑buff
            // Effects
            //MountData.spawnDust = ModContent.DustType<Dusts.Sparkle>(); //坐骑粒子

            MountData.totalFrames = 23; // 总帧数
            int[] dist = new int[MountData.totalFrames];
            for (int i = 0; i < dist.Length; i++) dist[i] = 28;
            dist[0] += 4;
            dist[2] -= 2;
            dist[4] += 2;
            dist[5] += 2;
            dist[13] -= 2;
            dist[14] -= 2;
            dist[15] -= 2;
            dist[16] -= 2;
            dist[17] -= 2;
            dist[18] -= 2;
            dist[22] += 2;

            MountData.playerYOffsets = dist; // Fills an array with values for less repeating code
            MountData.playerHeadOffset = 34;
            MountData.bodyFrame = 3;
            // Standing
            MountData.standingFrameCount = 16;
            MountData.standingFrameDelay = 6;
            MountData.standingFrameStart = 6;
            // Running
            MountData.runningFrameCount = 6;
            MountData.runningFrameDelay = 24;
            MountData.runningFrameStart = 0;
            // Dashing
            MountData.dashingFrameCount = 6;
            MountData.dashingFrameDelay = 200;
            MountData.dashingFrameStart = 0;
            // In-air
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 1;
            MountData.inAirFrameStart = 22;
            // Idle
            MountData.idleFrameCount = 16;
            MountData.idleFrameDelay = 6;
            MountData.idleFrameStart = 6;
            MountData.idleFrameLoop = true;
            // Swim
            MountData.swimFrameCount = MountData.inAirFrameCount;
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;

            if (!Main.dedServ)
            {
                MountData.backTextureExtraGlow = MountData.backTextureGlow = MountData.backTexture;
                MountData.backTextureExtra = Asset<Texture2D>.Empty;
                MountData.frontTexture = Asset<Texture2D>.Empty;
                MountData.frontTextureExtra = Asset<Texture2D>.Empty;
                MountData.textureWidth = MountData.backTexture.Width() + 20;
                MountData.textureHeight = MountData.backTexture.Height();
            }
        }

        public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
        {
            drawColor = Lighting.GetColor((int)(drawPlayer.Center.X / 16f), (int)(drawPlayer.Center.Y / 16f));
            glowColor = drawColor;
            return base.Draw(playerDrawData, drawType, drawPlayer, ref texture, ref glowTexture, ref drawPosition, ref frame, ref drawColor, ref glowColor, ref rotation, ref spriteEffects, ref drawOrigin, ref drawScale, shadow);
        }

        public override void UpdateEffects(Player player)
        {
            if (Math.Abs(player.velocity.X) >= 5f)
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDustPerfect(new Vector2(player.Center.X, player.position.X + Main.rand.Next(0,player.height)), MountData.spawnDust, new Vector2(-Main.rand.NextFloat(0, player.velocity.X / 2), 0));
            }
        }

        public override void SetMount(Player player, ref bool skipDust)
        {
            // mount code bypasses the normal mount spawning dust and replaces it with our own visual.
            if (!Main.dedServ)
            {
                int num = 16;
                for (int i = 0; i < num; i++)
                {
                    Dust.NewDustPerfect(player.Center + new Vector2(80, 0).RotatedBy(i * MyMath.pi * 2 / num), MountData.spawnDust, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)));
                }
                skipDust = true;
            }
        }
        /*
        public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
        {
            drawColor = Lighting.GetColor((int)(drawPlayer.Center.X / 16f), (int)(drawPlayer.Center.Y / 16f));
            /*
            Vector2 _drawOrigin = new Vector2(0);
            int count = 10;
            int num = drawPlayer.shadowPos.Length > count ? count : drawPlayer.shadowPos.Length;
            for (int i = 0; i < num; i++)
            {
                Vector2 drawPos = (drawPlayer.shadowPos[i] - Main.screenPosition) + drawOrigin + new Vector2(0, drawPlayer.gfxOffY);
                Color _color = (drawColor * ((num - i) / (float)num)) * .25f;
                Main.EntitySpriteDraw(glowTexture, drawPos, frame, _color, 0, drawOrigin, 1, (SpriteEffects)(drawPlayer.direction == -1).ToInt(), 0);
            }
            return base.Draw(playerDrawData, drawType, drawPlayer, ref texture, ref glowTexture, ref drawPosition, ref frame, ref drawColor, ref glowColor, ref rotation, ref spriteEffects, ref drawOrigin, ref drawScale, shadow);
        }
        */
        /*
        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {

            if (Math.Abs(velocity.X) >= MountData.dashSpeed && state != 2)
            {
                Mount mount = mountedPlayer.mount;
                int delay = MountData.dashingFrameDelay;
                mount._frameCounter = 0;
                if (frameCount % delay + 1 == delay) frameCount++;
                else mount._frame = (mount._frame + 1) % MountData.dashingFrameCount;
                return false;
            }
            else frameCount = 0;
            return true;
        }
        */
    }

    public class MiracleFoxBuff : Common.Global.Buffs.MountBuff
    {
        public override string mountName => "MiracleFox";
    }
}