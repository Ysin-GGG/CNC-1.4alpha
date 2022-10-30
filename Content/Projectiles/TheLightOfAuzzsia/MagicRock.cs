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
using Terraria.DataStructures;

namespace CNC.Content.Projectiles.TheLightOfAuzzsia
{
    public class MagicRock : CNCProjectile
    {
        private float rand;
        private Matrix transform => Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix * Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);

        public override void ResetDefaults()
        {
            projectile.width = 42;
            projectile.height = 40;
            projectile.timeLeft = 600;
            projectile.knockBack = 7f;
            projectile.alpha = 32;
            projectile.scale = 0.7f;
            projectile.penetrate = 1;
            projectile.extraUpdates = 1;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
            Array.Resize(ref projectile.oldPos, 20);
            Array.Resize(ref projectile.oldRot, 20);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                projectile.oldRot[i] = 0f;
                projectile.oldPos[i].X = 0f;
                projectile.oldPos[i].Y = 0f;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            OnSpawn();
        }

        public override void OnSpawn()
        {
            rand = CNC.Random.NextDegree();
            projectile.rotation = rand;
            projectile.oldPosition = projectile.position;
        }

        public override void AI()
        {
            UpdateOldData(projectile);
            CNCUtils.NewDust(projectile.Center + new Vector2(0, projectile.height / 2f).RotatedBy(projectile.rotation * projectile.direction), DustId.PurpleShortFx, 1.2f, projectile.velocity * .5f);
            CNCUtils.NewDust(projectile.Center + new Vector2(0, -projectile.height / 2f).RotatedBy(projectile.rotation * projectile.direction), DustId.PurpleShortFx, 1.2f, projectile.velocity * .5f);
            CNCUtils.NewDust(projectile.Center + new Vector2(0, projectile.height / 2f * projectile.direction).RotatedBy(projectile.velocity.ToRotation()), DustId.GreyStone, 1f, projectile.velocity.RotatedBy(MyMath.deg90) * .1f * projectile.direction).noGravity = true;
            CNCUtils.NewDust(projectile.Center + new Vector2(0, -projectile.height / 2f * projectile.direction).RotatedBy(projectile.velocity.ToRotation()), DustId.GreyStone, 1f, projectile.velocity.RotatedBy(-MyMath.deg90) * .1f * projectile.direction).noGravity = true;
            float dist = projectile.position.Distance(projectile.oldPosition);
            if (dist >= 12f)
            {
                for (int i = 0; i < dist / 12f; i++)
                {
                    Vector2 pos = projectile.Center + (projectile.position - projectile.oldPosition) * (i + 1) / (dist / 12f + 1);
                    CNCUtils.NewDust(pos + new Vector2(0, projectile.height / 2f * projectile.direction).RotatedBy(projectile.velocity.ToRotation()), DustId.GreyStone, 1f, projectile.velocity.RotatedBy(MyMath.deg90) * .1f * projectile.direction).noGravity = true;
                    CNCUtils.NewDust(pos + new Vector2(0, -projectile.height / 2f * projectile.direction).RotatedBy(projectile.velocity.ToRotation()), DustId.GreyStone, 1f, projectile.velocity.RotatedBy(-MyMath.deg90) * .1f * projectile.direction).noGravity = true;
                }
            }
            projectile.rotation += MyMath.deg30 / 6 * projectile.direction;
            if (projectile.timeLeft > 540) projectile.velocity.X *= 1.0025f; 
            else projectile.velocity.Y += MyMath.g * .01f;
            CNCUtils.AddLight(projectile.Center, Color.Purple, 1.5f);
        }

        public override void Kill(int timeLeft)
        {
            float rot = projectile.velocity.ToRotation() + CNC.Random.NextDegree(-16f, 16f);
            float max = 64f;
            for (int i = 0; i < 8; i++)
            {
                Dust dust = CNCUtils.NewDust(projectile, DustId.GreyStone, rot, 1.2f, projectile.Size / 4f, Color.White);
                dust.alpha = 0;
            }
            for (int i = 0; i < max; i++)
            {
                float amount = i / max * MyMath.pi2;
                Vector2 vel = new Vector2(6f * MyMath.cos(amount), 18f * MyMath.sin(amount)).RotatedBy((projectile.oldPosition - projectile.position).ToRotation());
                Dust dust = CNCUtils.NewDust(projectile.Center, DustId.GreyStone, 1.5f, vel);
                dust.noGravity = true;
                dust.alpha = 0;
            }
            for (int i = 0; i < max; i++)
            {
                Vector2 vel = (projectile.oldPosition - projectile.position).RotatedBy(CNC.Random.NextDegree(-16f, 16f)) * CNC.Random.NextFloat(.25f, .75f);
                if (vel.X > 8f || vel.Y > 8f) vel *= 8f / (vel.X > 8f ? vel.X : vel.Y);
                Dust dust = CNCUtils.NewDust(projectile, DustId.GreyStone, 1.25f, vel * projectile.scale);
                dust.noGravity = false;
                dust.alpha = 0;
            }
            CNCUtils.ScreenShake(projectile.Center, 480f, 0, new Vector2(0, 20), new Vector2(0, 10), .9f);
            CNCUtils.AddLight(projectile.position, Color.Purple, .6f);
            CNCUtils.PlaySound("TheLightOfAuzzsia/RockHit", projectile, .5f);
            GenericExplosion e = CNCUtils.CreateExplosion(projectile);
            e.SetDefaults(Mod.Assets.Request<Texture2D>("Content/Projectiles/TheLightOfAuzzsia/MagicRockExplosion").Value, projectile.Center, Vector2.One * 120, new int[12] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 });
            e.SetDamage(0, projectile.DamageType, projectile.damage, projectile.knockBack, projectile.CritChance, 0f, 1f);
            e.SetColor(Lighting.GetColor((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f)), Color.MediumPurple, .3f);
            e.SetAlpha(1f, -.025f, 1f);
            e.SetScale(1f, .0375f, 1f);
            e.SetRotation((projectile.position - projectile.oldPosition).ToRotation() - MyMath.deg90, 0f, 1f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            var ef = CNC.GetEffect("Trail");
            List<VertexInfo> bars = TrailShaderData.GetBars(projectile, 64, 3f * projectile.scale, 4f * projectile.scale, true, 1f, 2.5f, -1, false);
            if (bars.Count > 2)
            {
                ef.Parameters["uTransform"].SetValue(transform);
                ef.Parameters["uTime"].SetValue(0f);
                var trai = new TrailShaderData(ef, projectile, 64)
                {
                    width2 = 32f,
                    imageHeatmap = Mod.Assets.Request<Texture2D>("Assets/Effects/Heatmap/Grey2").Value,
                    imageTrace = Mod.Assets.Request<Texture2D>("Assets/Effects/Metero1").Value,
                    imageTrail = Mod.Assets.Request<Texture2D>("Assets/Effects/Metero1").Value,
                };
                trai.Draw(Main.spriteBatch, bars);
            }
        }
    }
}
