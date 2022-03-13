using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.NPCs;

namespace CNC.Common.Global.Projectiles
{
    public abstract class CNCProjectile : ModProjectile
    {
        public NPC RootNPC => projectile.GetGlobalProjectile<ProjectileRelation>().RootNPC;
        public NPC ParentNPC => projectile.GetGlobalProjectile<ProjectileRelation>().ParentNPC;
        public NPC[] ChildrenNPC => projectile.GetGlobalProjectile<ProjectileRelation>().ChildrenNPC;
        public Projectile RootProjectile => projectile.GetGlobalProjectile<ProjectileRelation>().RootProjectile;
        public Projectile ParentProjectile => projectile.GetGlobalProjectile<ProjectileRelation>().ParentProjectile;
        public Projectile[] ChildrenProjectile => projectile.GetGlobalProjectile<ProjectileRelation>().ChildrenProjectile;

        public Mod mod => Mod;
        public Projectile projectile => Projectile;
        public Player Target = null;
        public IEntitySource source => projectile.GetProjectileSource_FromThis();
        public float posX { get => projectile.position.X; set => projectile.position.X = value; }
        public float posY { get => projectile.position.Y; set => projectile.position.Y = value; }
        public float velX { get => projectile.velocity.X; set => projectile.velocity.X = value; }
        public float velY { get => projectile.velocity.Y; set => projectile.velocity.Y = value; }
        public int frame { get => projectile.frame; set => projectile.frame = value; }
        public int frameTick { get => projectile.frameCounter; set => projectile.frameCounter = value; }

        public float[] var = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public virtual int countFrame => 1;

        public sealed override void SetStaticDefaults()
        {
            Main.projFrames[Type] = countFrame;
            DisplayName.SetDefault(CNCLocalization.GetText($"Projectile.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Projectile.{Name}", 7));
            ResetStaticDefaults();
        }

        public sealed override void SetDefaults()
        {
            ResetDefaults();
        }

        public void AfterImages(Projectile projectile, Color color, int count = 5, Texture2D image = null)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = image == null ? TextureAssets.Projectile[projectile.type].Value : image;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            int num = projectile.oldPos.Length > count ? count : projectile.oldPos.Length;
            for (int i = 0; i < num; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin;
                Color _color = projectile.GetAlpha(color) * ((num - i) / (float)num);
                Main.EntitySpriteDraw(texture, drawPos, null, _color, projectile.oldRot[i], drawOrigin, projectile.scale, SpriteEffects.None, 0);
            }
        }

        public NPC FindClosestTarget(Projectile p, float range, bool friendly = false)
        {
            NPC target = null;
            float clostest = range;
            for (int i = 0; i < Const.max1; i++)
            {
                NPC npc = Main.npc[i];
                if (npc == null || !npc.active || npc.life <= 0 || npc.friendly != friendly || npc.townNPC != friendly || npc.type == 488) continue;
                float dist = Vector2.Distance(p.position, npc.position);
                bool canHit = Collision.CanHitLine(p.position, p.width, p.height, npc.position, npc.width, npc.height);
                if (dist < clostest && canHit)
                {
                    target = npc;
                    clostest = dist;
                }
            }
            return target;
        }

        public Player FindClosestTarget(Projectile p, float range)
        {
            if (Main.netMode == 0) return Const.Me;
            Player target = null;
            float clostest = range;
            for (int i = 0; i < Const.max1; i++)
            {
                Player player = Main.player[i];
                if (player == null || !player.active || player.statLife <= 0) continue;
                float dist = Vector2.Distance(p.position, player.position);
                bool canHit = Collision.CanHitLine(p.position, p.width, p.height, player.position, player.width, player.height);
                if (dist < clostest && canHit)
                {
                    target = player;
                    clostest = dist;
                }
            }
            return target;
        }

        public void UpdateOldData(Projectile p, Vector2 offset = default)
        {
            if (offset == default) offset = Vector2.Zero;
            for(int i = p.oldPos.Length - 1; i > 0; i--)
                p.oldPos[i] = p.oldPos[i - 1];
            p.oldPos[0] = p.position + offset;
            for (int i = p.oldRot.Length - 1; i > 0; i--)
                p.oldRot[i] = p.oldRot[i - 1];
            p.oldRot[0] = p.rotation;
        }

        protected Projectile AI001_RollingMissile(Projectile p, bool gravity = false, bool updateRot = false, float extraAngle = 0)
        {
            Vector2 realVel = Const._02;
            float angle = MyMath.pi * extraAngle / 180;
            if (p.type == 0 || p.velocity == Const._02) return p;
            p.velocity.Y = gravity ? p.velocity.Y : p.oldVelocity.Y;
            if (updateRot && realVel == Const._02)
            {
                realVel = p.velocity;
                p.rotation = angle + realVel.ToRotation();
            }
            else p.rotation = angle + p.velocity.ToRotation();
            return p;
        }

        protected void Anim001_Loop(Projectile p, int tickPerFrame, int start = 0)
        {
            int[] tick = new int[Main.projFrames[p.type]];
            for (int i = 0; i < tick.Length; i++) tick[i] = tickPerFrame;
            Anim001_Loop(p, tick, start);
        }

        protected void Anim001_Loop(Projectile p, int[] tick, int start = 0)
        {
            int maxFrame = Main.projFrames[p.type];
            int count = tick[p.frame];
            if ((p.frameCounter++ % count) + 1 == count) p.frame++;
            if (p.frame >= maxFrame) p.frame = start;
        }

        public virtual void ResetDefaults() { }
        public virtual void ResetStaticDefaults() { }
    }
}