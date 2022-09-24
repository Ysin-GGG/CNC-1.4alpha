using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.ID;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.ArgenteusOathKeeper
{
    public class SilveryField : CNCProjectile
    {
        public override int countFrame => 7;
        private const float time = 300;
        private const int tpf = 3;
        private float rot = 0;

        public override void ResetDefaults()
        {
            projectile.timeLeft = (int)time;
            projectile.knockBack = 2f;
            projectile.alpha = 64;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Anim001_Loop(projectile, tpf, 2);
            projectile.Bottom = owner.Bottom;
            CNCUtils.AddLight(projectile.position, CNCID.Colors.SpiritBoss, 1.2f);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override void Kill(int timeLeft)
        {
            CNCUtils.ScreenShake(owner.Center, 160f, 0, new Vector2(0, 40), new Vector2(0, 20), .9f);
            //CNCUtils.PlaySound(SoundID.Item62, projectile);
            CNCUtils.PlaySound(SoundID.Item74, projectile);
            var e = CNCUtils.CreateExplosion(this.projectile);
            e.SetDefaults(Mod.Assets.Request<Texture2D>("Assets/Projectiles/Explosion/SilverFieldExplosion").Value, projectile.Center, Vector2.One * 160, new int[5] { 2, 2, 3, 3, 3 });
            e.SetDamage(1, projectile.DamageType, projectile.damage, projectile.knockBack, projectile.CritChance, 0f, 1f);
            e.SetColor(Lighting.GetColor((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f)), CNCID.Colors.SpiritBoss, .3f);
            e.SetAlpha(1f, -.025f, 1f);
            e.SetScale(1f, .0125f, 1f);
            e.projectile.Bottom = owner.Bottom;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = CNCID.Colors.SpiritBoss;
            int height = Texture2.Height / this.countFrame;
            Vector2 pos = owner.Bottom - Main.screenPosition;
            Vector2 origin = new Vector2(Texture2.Width / 2f, height);
            Rectangle source = new Rectangle(0, this.frame * height, Texture2.Width, height);
            Texture2D trace = mod.Assets.Request<Texture2D>("Assets/Effects/Trace6").Value;
            Texture2D image = mod.Assets.Request<Texture2D>(Texture.Substring(4) + "_" + this.frame).Value;
            pos.Y += 10f;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            Effect shader = CNC.GetEffect("ShieldLight");
            shader.Parameters["uColor"].SetValue(lightColor.ToVector3());
            shader.Parameters["uOffset"].SetValue(new Vector2(-.85f, .85f) * Main.GlobalTimeWrappedHourly);
            shader.Parameters["uCenter"].SetValue(new Vector2(.5f, .75f));
            shader.Parameters["uSize"].SetValue(source.Size());
            shader.Parameters["uDist"].SetValue(.5f);
            shader.Parameters["uAlpha"].SetValue(.4f);

            //shader.Parameters["uImageSize"].SetValue(Texture2.Size());
            //shader.Parameters["uFrame"].SetValue(this.frame);
            shader.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.Textures[0] = image;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Main.graphics.GraphicsDevice.Textures[1] = trace;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            Main.spriteBatch.Draw(image, pos, null, lightColor, 0, origin, 1f, 0, 0);
            //Main.spriteBatch.Draw(Texture2, pos, source, lightColor, 0, origin, 1f, 0, 0);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            
            return false;
        }
    }
}
