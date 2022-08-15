using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public class GiantBranch : CNCProjectile
    {
        private const int dustid1 = DustId.BrownDirt;
        private const int dustid2 = DustId.LightGreyStone;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).rage;
        public override int countFrame => 9; 

        public override void ResetDefaults()
        {
            projectile.width = 86;
            projectile.height = 194;
            projectile.timeLeft = 40;
            projectile.alpha = 0;
            projectile.light = 0;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
            projectile.position = projectile.oldPosition;
            for (int i = 0; i < 10; i++)
            {
                float factor = i / 10f;
                Vector2[] pos = {
                    MyMath.Lerp(projectile.TopLeft, projectile.TopRight, factor),
                    MyMath.Lerp(projectile.TopRight, projectile.BottomRight, factor),
                    MyMath.Lerp(projectile.BottomRight, projectile.BottomLeft, factor),
                    MyMath.Lerp(projectile.BottomLeft, projectile.TopLeft, factor),
                };
                foreach(var p in pos) CNCUtils.NewDust(p, DustId.CyanFx).noGravity = true;
            }
            if (projectile.hostile)
            {
                projectile.alpha = 0;
                if(frameTick++ > 2)
                {
                    if (frame++ >= countFrame) frame--;
                    frameTick = 0;
                }
            }
            else 
            {
                if (projectile.timeLeft == 1)
                {
                    projectile.hostile = true;
                    projectile.timeLeft = 20;
                }
                else for (int i = 0; i < 5; i++)
                    {
                        Vector4 xywh = new Vector4(projectile.BottomLeft.X, projectile.BottomLeft.Y - 20, projectile.width, 20);
                        CNCUtils.NewDust(xywh, dustid1, 0f, 1f, default, default(Color));
                        CNCUtils.NewDust(xywh, dustid2, 0f, 1f, default, default(Color));
                    }
            }
        }

        public override void HitPlayer(Player target, int damage, bool crit)
        {
            target.velocity.Y = MyMath.Max(target.velocity.Y - 24f, -24f);
            target.extraFall = 50;
        }
    }
}