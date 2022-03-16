using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public abstract class SpiritArrow : CNCProjectile
    {
        public override string Texture => "CNC/Content/Projectiles/Hostile/SpiritArrow";
        protected Projectile p => projectile;
        protected const int dustid1 = DustId.TrailingCyan;
        protected const int dustid2 = DustId.CyanShortFx1;
        protected virtual Color mainColor => new Color(178, 214, 231);
        protected virtual int len => 30;
        protected virtual bool update => true;
        protected int attackCD = 0;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).rage;

        public override void ResetStaticDefaults()
        {
            DisplayName.SetDefault(CNCLocalization.GetText("Projectile.SpiritArrow", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText("Projectile.SpiritArrow", 7));
        }

        public override void ResetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.timeLeft = 360;
            projectile.alpha = 255;
            projectile.light = 0;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            if (len != projectile.oldPos.Length)
            {
                Array.Resize(ref projectile.oldPos, len);
                Array.Resize(ref projectile.oldRot, len);
            }
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                projectile.oldRot[i] = 0.0f;
                projectile.oldPos[i].X = 0.0f;
                projectile.oldPos[i].Y = 0.0f;
            }
            attackCD = 0;
        }

        public override void AI()
        {
            AI001_RollingMissile(p, false, true, 90f);
            if (update) UpdateOldData(p);
            p.alpha = p.alpha > 5 ? p.alpha - 5 : 0;
            projectile.friendly = false;
            projectile.hostile = true;
            while (attackCD > 0) attackCD--;
            Vector2 pos = (p.position + p.Size / 2f) / 16f;
            float r = mainColor.R / 255f;
            float g = mainColor.G / 255f;
            float b = mainColor.B / 255f;
            Lighting.AddLight((int)pos.X, (int)pos.Y, r, g, b);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 24;
            Point pos = p.position.ToPoint() + new Point(size, size);
            hitbox = new Rectangle(pos.X, pos.Y, p.width - size * 2, p.height - size * 2);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (rage)
            {
                target.statMana -= (int)(target.statMana * .1f);
                target.immuneTime = 5;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                float x = p.position.X;
                float y = p.position.Y;
                float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(x, y, 9, 9);
                Dust dust = CNCUtils.NewDust(xywh, dustid2, rand[0], Main.rand.NextFloat(2f, 3f), default, new Color(0, 234, 118));
                dust.position = xywh.XY() + p.Size / 2;
                dust.velocity = new Vector2(rand[1], rand[2]) * 8f;
                dust.color = mainColor;
                dust.noGravity = true;
            }
            for (int i = 0; i < p.oldPos.Length; i++)
            {
                float x = p.oldPos[i].X;
                float y = p.oldPos[i].Y;
                float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(x, y, 9, 9);
                Dust dust = CNCUtils.NewDust(xywh, dustid1, rand[0], Main.rand.NextFloat(1f, 2f), default, mainColor, MyMath.GetXY(p.oldRot[i] - MyMath.deg90) * 4f);
                dust.position = xywh.XY() + p.Size / 2;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            AfterImages(projectile, mainColor * .75f * (255 - p.alpha / 255f), len);
            return true;
        }

        public class V1 : SpiritArrow
        {
            protected override int len => 15 + (int)(15 * precent);
            float precent = 0f;
            Vector2 maxVel = Const._02;

            public override void AI()
            {
                base.AI();
                projectile.extraUpdates = 1;
                precent = precent + .01f > 1 ? 1 : precent + .01f;
                if (maxVel == Const._02) maxVel = p.velocity * .75f;
                else p.velocity = maxVel * precent;
            }
        }

        public class V2 : SpiritArrow
        {
            protected override bool update => p.timeLeft <= 300;
            float precent = .01f;
            Vector2 maxVel = Const._02;

            public override void AI()
            {
                base.AI();
                projectile.extraUpdates = 1;
                if (!update)
                {
                    Player target = FindClosestTarget(p, 800);
                    if (target.active)
                    {
                        Vector2 pos1 = target.Center;
                        Vector2 pos2 = p.position + new Vector2(32);
                        Vector2 distVec2 = pos1 - pos2;
                        maxVel = p.velocity = MyMath.GetXY(distVec2.ToRotation()) * 24f * .75f;
                    }
                    if (precent > 0) p.timeLeft = 480;
                    precent = 0;
                    p.position = p.oldPosition;
                }
                else
                {
                    precent = precent + .01f > 1 ? 1 : precent + .01f;
                    p.velocity = maxVel * precent;
                }
                p.hostile = update;
            }

            public override bool PreDraw(ref Color lightColor)
            {
                lightColor = Color.White;
                if (update) AfterImages(projectile, mainColor * .75f * (255 - p.alpha / 255f), len);
                return true;
            }
        }

        public class V3 : SpiritArrow
        {
            public override void AI()
            {
                base.AI();
                if (p.ai[0] > 0 && Main.player[(int)p.ai[0] - 1].active && p.timeLeft < 240)
                {
                    Player target = Main.player[(int)p.ai[0] - 1];
                    Vector2 pos1 = target.Center;
                    Vector2 pos2 = p.position + new Vector2(32);
                    Vector2 distVec2 = pos1 - pos2;
                    float velMax = 12f;
                    float rot = distVec2.ToRotation();
                    Vector2 vel = MyMath.GetXY(rot) * velMax / 12f;
                    p.velocity += vel / 2;
                    if (Math.Abs(p.velocity.X) > velMax) { p.velocity.X = (p.velocity.X < 0 ? -1 : 1) * velMax; }
                    if (Math.Abs(p.velocity.Y) > velMax) { p.velocity.Y = (p.velocity.Y < 0 ? -1 : 1) * velMax; }
                }
                if (len2 < 16) len2++;
            }
            
            private int pointX = 0;
            private int len2 = 0;

            public override bool PreDrawExtras()
            {
                Texture2D slash1 = Terraria.ModLoader.ModContent.Request<Texture2D>("CNC/Assets/Effects/Slash1").Value;
                Texture2D slash2 = Terraria.ModLoader.ModContent.Request<Texture2D>("CNC/Assets/Effects/Slash2").Value;
                Color _color = mainColor; _color.A = 0;
                //Drawing.DrawTrailDirect(Main.spriteBatch, slash1, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, slash1.Height / 2, new Vector2(1f, .10f), new Vector2(1f, .10f), true, 16);
                //Drawing.DrawTrailDirect(Main.spriteBatch, slash2, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, slash2.Height / 2, new Vector2(1f, .10f), new Vector2(1f, .10f), true, 16);
                //pointX = (pointX - 15 + slash1.Width) % slash1.Width;
                return true;
            }
        }
    }
}