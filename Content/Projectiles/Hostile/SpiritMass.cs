using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public class SpiritMass : CNCProjectile
    {
        Projectile p => projectile;
        const int dustid1 = DustId.CyanShortFx;
        const int dustid2 = DustId.CyanShortFx1;

        public override void ResetDefaults()
        {
            projectile.width = 9;
            projectile.height = 9;
            projectile.timeLeft = 180;
            projectile.alpha = 0;
            projectile.light = 0;
            projectile.scale = 0;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
        }

        public override void UpdateAI()
        {
            AI001_RollingMissile(p, false, true, 90f);
            for (int i = 0; i < 8; i++)
            {
                if (p.timeLeft > 175) { break; }
                float x = p.position.X;
                float y = p.position.Y;
                Vector4 xywh = new Vector4(x, y, 15, 15);
                Dust dust = CNCUtils.NewDust(xywh, dustid1, Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(1.2f, 2.4f), new Vector2(6), default(Color));
                dust.position = xywh.XY();
                dust.velocity = -p.velocity / 2;
                dust.noGravity = true;
                dust.noLight = true;
            }
            if (p.ai[0] > 0 && Main.player[(int)(p.ai[0] - 1)].active)
            {
                Player target = Main.player[(int)(p.ai[0] - 1)];
                Vector2 pos1 = target.Center;
                Vector2 pos2 = p.Center;
                Vector2 distVec2 = pos1 - pos2;
                if (Vector2.Distance(pos1, pos2) > 128f)
                {
                    pos1.Y = target.position.Y;
                    distVec2 = pos1 - pos2;
                }
                float velMax = 6f;
                float rot = distVec2.ToRotation();
                Vector2 vel = new Vector2(MyMath.cos(rot), MyMath.sin(rot)) * velMax / 8f;
                p.velocity += vel / 2;
                if (Math.Abs(p.velocity.X) > velMax) { p.velocity.X = (p.velocity.X < 0 ? -1 : 1) * velMax; }
                if (Math.Abs(p.velocity.Y) > velMax) { p.velocity.Y = (p.velocity.Y < 0 ? -1 : 1) * velMax; }
            }
            Vector2 pos = (p.position + p.Size / 2f) / 16f;
            Color color = new Color(0, 234, 118);
            CNCUtils.AddLight(p.Center, color);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                float x = p.position.X;
                float y = p.position.Y;
                float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1,1) };
                Vector4 xywh = new Vector4(x, y, 9, 9);
                Dust dust = CNCUtils.NewDust(xywh, dustid2, rand[0], Main.rand.NextFloat(2f, 3f), default, new Color(0, 234, 118));
                dust.position = xywh.XY();
                dust.velocity = new Vector2(rand[1], rand[2]) * 8f;
                dust.noGravity = true;
            }
        }
    }
}
