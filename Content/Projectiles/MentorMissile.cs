using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles
{
    public class MentorMissile : CNCProjectile
    {
        public override int countFrame => 13;

        const int dustid1 = DustId.CyanShortFx;
        const int dustid2 = DustId.CyanShortFx1;
        Color color = new Color(101, 145, 230);
        public int stage { get => _stage; set => _stage = value; }
        public int _stage = 0;
        float rot = 0;
        int len => 20;

        private int pointX = 0;
        private int len2 = 0;

        public override bool PreDrawExtras()
        {
            if (frame > 4)
            {
                Texture2D tail = Terraria.ModLoader.ModContent.Request<Texture2D>("CNC/Assets/Effects/Trail2").Value;
                Texture2D glow = Terraria.ModLoader.ModContent.Request<Texture2D>("CNC/Assets/Effects/GlowTrail1").Value;
                Color _color = color; _color.A = 0;
                Drawing.DrawTrail(Main.spriteBatch, tail, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, tail.Height / 2, new Vector2(1f, .10f), new Vector2(1f, .01f), true, 16);
                //Drawing.DrawTrail(Main.spriteBatch, glow, len2, projectile.oldPos, projectile.oldRot, projectile.Size / 2, MyMath.deg90, _color, pointX, glow.Height / 2, new Vector2(1f, .05f), new Vector2(1f, .01f), true, 16);
                pointX = (pointX - 15 + tail.Width) % tail.Width;

            }
            return true;
        }

        public override void ResetDefaults()
        {
            projectile.damage = 23;
            projectile.knockBack = 1.5f;
            projectile.width = 48;
            projectile.height = 48;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = .2f;
            projectile.scale = 1;
            projectile.penetrate = 1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            stage = 0;
            rot = 0;
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
        }

        public override void AI()
        {
            Anim001_Loop(projectile, 5, 9);
            if (_stage == 0)
            {
                NPC npc = FindClosestTarget(projectile, 800, false);
                rot = (npc.Center - projectile.Center).ToRotation();
                if (frame == 4) projectile.timeLeft = 0;
            }
            else
            {
                if (frame < 4) frame = 4;
                if (len2 < len - 5) len2++;
                projectile.Size = new Vector2(14f);
                projectile.friendly = true;
                if(frame > 6) projectile.tileCollide = true;
                /*
                for (int i = 0; i < 8; i++)
                {
                    Vector4 xywh = new Vector4(projectile.Center.X, projectile.Center.Y, projectile.width - 4f, projectile.height - 4f);
                    Dust dust = CNCUtils.NewDust(xywh, dustid2, Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(.9f, 1.6f), new Vector2(4), color);
                    dust.noGravity = true;
                }
                */
            }
            AI001_RollingMissile(projectile, true, true, 90);
            UpdateOldData(projectile, projectile.velocity);
            Vector2 pos = (projectile.Center + projectile.Size / 2f) / 16f;
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;
            //Lighting.AddLight((int)pos.X, (int)pos.Y, r, g, b);
        }
        /*
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int[] size = new int[] { 48, 48, 48, 48, 28, 20, 10, 14, 14, 14, 14, 14, 14 };
            hitbox.Width = size[projectile.frame];
            hitbox.Height = size[projectile.frame];
            hitbox.X = (int)(projectile.Center.X - hitbox.Size().X / 2);
            hitbox.Y = (int)(projectile.Center.X - hitbox.Size().Y / 2);
        }
        */
        public override void Kill(int timeLeft)
        {
            if (_stage == 0)
            {
                SoundEngine.PlaySound(SoundID.Item20, projectile.position);
                Vector2 pos = projectile.Center + new Vector2(0, 14);
                Vector2 vel = MyMath.GetXY(rot) * 16f;
                int type = CNCID.Projectile.LegacyCollection[Name];
                int dmg = projectile.damage;
                float kb = projectile.knockBack;
                int owner = projectile.owner;
                MentorMissile child = (MentorMissile)CNCUtils.NewProj(source, pos, vel, type, dmg, kb, owner).ModProjectile;
                child.stage = 1;
            }
            else for (int i = 0; i < 15; i++)
            {
                float[] rand = { Main.rand.NextFloat(MyMath.pi), Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1) };
                Vector4 xywh = new Vector4(projectile.Center.X, projectile.Center.Y, 14, 14);
                Dust dust = CNCUtils.NewDust(xywh, dustid2, rand[0], Main.rand.NextFloat(3f, 4f), default, color);
                dust.position = xywh.XY();
                dust.velocity = -projectile.velocity / 4f + new Vector2(rand[1], rand[2]) * 4f;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(
                Terraria.GameContent.TextureAssets.Projectile[Type].Value,
                projectile.Center - Main.screenPosition,
                new Rectangle(0, projectile.frame * 88, 48, 88),
                lightColor,
                projectile.rotation,
                new Vector2(24f),
                1,0,0
                );
            return base.PreDraw(ref lightColor);
        }
    }
}
