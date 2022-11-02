using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.ArgenteusOathKeeper
{
    public class SilveryDash : CNCProjectile
    {
        public override int countFrame => 5;
        private const float time = 10;
        private const int tpf = 2;
        private float rot = 0;

        public override void ResetDefaults()
        {
            projectile.timeLeft = (int)time;
            projectile.knockBack = 2f;
            projectile.alpha = 64;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Melee;
        }

        public override void UpdateAI()
        {
            Anim001_Loop(projectile, tpf);
            projectile.Center = owner.Center;
            projectile.rotation = projectile.velocity.ToRotation();
            owner.SetImmuneTimeForAllTypes(2);
            owner.velocity = projectile.velocity;
            owner.fallStart = (int)(owner.position.Y / 16.0);
            if (projectile.velocity.Y > 0 && !Collision.SolidTiles(owner.oldPosition + projectile.velocity, owner.width, owner.height)) owner.position.Y = MyMath.Max(owner.position.Y, owner.oldPosition.Y + projectile.velocity.Y);
            if (projectile.timeLeft == time) CNCUtils.PlaySound(SoundID.Item28, projectile);
            Color color = CNCID.Colors.SpiritBoss2;
            color.A = 0;
            float max = 8f;
            for (int i = 0; i < max; i++)
            {
                Vector2 pos = projectile.Center + projectile.velocity * (time - projectile.timeLeft) / 2f;
                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                    pos - projectile.velocity * 1.5f * i / max,
                    projectile.velocity * .1f,
                    CNC.Random.NextDegree(),
                    40,
                    Vector2.One,
                    color,
                    new Color(color.R, color.G, color.B, 5)
                    );
                ParticleManager.particle[idx].velocityMul = CNC.Random.NextFloat(.95f, .98f) * Vector2.One;
                ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                ParticleManager.particle[idx].alphaAdd = -.025f;
                ParticleManager.particle[idx].alpha = 1f;

                idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Star1").Value,
                    pos - projectile.velocity * 1.5f * (1 - i / max) - new Vector2(32f, 0f).RotatedBy(rot += MyMath.deg01 * 54f / max) * projectile.direction,
                    projectile.velocity * .1f,
                    CNC.Random.NextDegree(),
                    40,
                    Vector2.One * .75f,
                    color,
                    new Color(color.R, color.G, color.B, 5)
                    );
                ParticleManager.particle[idx].velocityMul = CNC.Random.NextFloat(.95f, .98f) * Vector2.One;
                ParticleManager.particle[idx].rotationAdd = CNC.Random.NextDegree(-16f, 16f);
                ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                ParticleManager.particle[idx].alphaAdd = -.025f;
                ParticleManager.particle[idx].alpha = 1f;
            }
            CNCUtils.AddLight(projectile.position, CNCID.Colors.SpiritBoss, 1.2f);
        }

        public override void OnKill(int timeLeft)
        {
            owner.velocity *= .2f;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 16;
            hitbox.X = (int)projectile.Center.X - size;
            hitbox.Y = (int)projectile.Center.Y - size;
            hitbox.Width = size * 2;
            hitbox.Height = size * 2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < 9; i++) if (targetHitbox.Contains((projectile.Center + projectile.velocity * i).ToPoint())) return true;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void OnHit(Entity target, int damage, float knockback, bool crit)
        {
            var hit = CNCUtils.CreateSlashHit(projectile, Vector2.Zero);
            hit.SetDefaults(mod.Assets.Request<Texture2D>("Assets/Projectiles/SlashHit/SlashHit1").Value, new Vector2(MyMath.phi - 1f, 1f) * .6f, target.Center, 15, projectile.velocity.ToRotation() + MyMath.deg30, 30);
            hit.SetColor(CNCID.Colors.SpiritBoss2, CNCID.Colors.SpiritBoss2, .1f);
            hit = CNCUtils.CreateSlashHit(projectile, Vector2.Zero);
            hit.SetDefaults(mod.Assets.Request<Texture2D>("Assets/Projectiles/SlashHit/SlashHit1").Value, new Vector2(MyMath.phi - 1f, 1f) * .6f, target.Center, 15, projectile.velocity.ToRotation() - MyMath.deg30, 30);
            hit.SetColor(CNCID.Colors.SpiritBoss2, CNCID.Colors.SpiritBoss2, .1f);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = CNCID.Colors.SpiritBoss;
            Texture2D weapon = Terraria.GameContent.TextureAssets.Item[this.owner.HeldItem.type].Value;
            Vector2 origin = new Vector2(0, weapon.Height);
            float alpha = MyMath.sin(projectile.timeLeft / time * MyMath.pi);
            float rotation = projectile.rotation + MyMath.deg45;
            this.owner.itemLocation = new Vector2(this.owner.Center.X, this.owner.position.Y + 20f) + new Vector2(20f, 0).RotatedBy(this.owner.itemRotation = rotation - MyMath.deg45);
            Main.EntitySpriteDraw(weapon, this.owner.itemLocation - Main.screenPosition, null, Color.White * alpha, rotation, origin, 1.5f, 0, 0);
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            lightColor.A = 0;
            Texture2D image = mod.Assets.Request<Texture2D>("Assets/Glow/Refringence").Value;
            Vector2 origin = image.Size() / 2;
            float alpha = MyMath.sin(projectile.timeLeft / time * MyMath.pi);
            Main.EntitySpriteDraw(image, this.owner.Center - Main.screenPosition, null, lightColor * alpha, projectile.rotation, origin, 2f * alpha, 0, 0);
        }
    }
}
