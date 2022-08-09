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
            projectile.scale = 0.8f;
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

        public override void AI()
        {
            UpdateOldData(projectile);
            CNCUtils.NewDust(projectile.Center + new Vector2(0, projectile.height / 2f).RotatedBy(projectile.rotation * projectile.direction), DustId.PurpleLight, 1f, projectile.velocity * .1f);
            CNCUtils.NewDust(projectile.Center + new Vector2(0, -projectile.height / 2f).RotatedBy(projectile.rotation * projectile.direction), DustId.PurpleLight, 1f, projectile.velocity * .1f);
            CNCUtils.NewDust(projectile.Center + new Vector2(0, projectile.height / 2f * projectile.direction).RotatedBy(projectile.velocity.ToRotation()), DustId.PurpleLight, 1f, projectile.velocity.RotatedBy(MyMath.deg90) * .1f * projectile.direction);
            CNCUtils.NewDust(projectile.Center + new Vector2(0, -projectile.height / 2f * projectile.direction).RotatedBy(projectile.velocity.ToRotation()), DustId.PurpleLight, 1f, projectile.velocity.RotatedBy(-MyMath.deg90) * .1f * projectile.direction);
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
                Vector2 vel = new Vector2(6f * MyMath.cos(amount), 18f * MyMath.sin(amount)).RotatedBy(rot);
                Dust dust = CNCUtils.NewDust(projectile.Center, DustId.GreyStone, 1.5f, vel);
                dust.noGravity = true;
                dust.alpha = 0;
            }
            CNCUtils.PlaySound("TheLightOfAuzzsia/RockHit", projectile, .5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D MagicAura = Mod.Assets.Request<Texture2D>("Content/Projectiles/TheLightOfAuzzsia/MagicAura").Value;
            Vector2 pos = Main.player[projectile.owner].position - Main.screenPosition;
            Color color = Color.White;
            lightColor = color;
            color.A = 0;
            Main.EntitySpriteDraw(MagicAura, pos, null, color * (1 - projectile.alpha / 255f), projectile.rotation, MagicAura.Size() / 2, .2f, 0, 1);
            CNCUtils.AddLight(pos, Color.Purple, 2f);
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Effect ef = Mod.Assets.Request<Effect>("Content/Effects/Trail").Value;
            List<VertexInfo> bars = TrailShaderData.GetBars(projectile, 64, 2f, 3f, true, 1f, 2.5f, -1);
            if (bars.Count > 2)
            {
                ef.Parameters["uTransform"].SetValue(transform);
                ef.Parameters["uTime"].SetValue(rand - Main.GlobalTimeWrappedHourly);
                var trai = new TrailShaderData(ef, projectile, 64)
                {
                    width2 = 32f,
                    imageHeatmap = Mod.Assets.Request<Texture2D>("Assets/Effects/Heatmap/Grey2").Value,
                    imageTrace = Mod.Assets.Request<Texture2D>("Assets/Effects/GlowTrail1").Value,
                    imageTrail = Mod.Assets.Request<Texture2D>("Assets/Effects/Trace2").Value,
                };
                trai.Draw(Main.spriteBatch, bars);
            }
        }
    }
}
