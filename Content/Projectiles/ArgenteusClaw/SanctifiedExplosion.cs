
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using System.Collections.Generic;

namespace CNC.Content.Projectiles.ArgenteusClaw
{
    public class SanctifiedExplosion : CNCProjectile
    {
        public override int countFrame => 7;
        private int[] alpha = { 100, 1, 0, 10, 100, 20, 20, 2 };

        public override void ResetDefaults()
        {
            projectile.scale = 1f;
            projectile.alpha = 64;
            projectile.width = 120;
            projectile.height = 120;
            projectile.timeLeft = 28;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void OnSpawn()
        {
            projectile.scale = 1f;
            projectile.alpha = 64;
            projectile.rotation = CNC.Random.NextDegree();
            projectile.velocity = Vector2.Zero;
            CNCUtils.PlaySound("ArgenteusClaw/Explode", projectile.position, 2f);
        }

        public override void UpdateAI()
        {
            Anim001_Loop(projectile, 4);
            projectile.scale += .03f;
            if (projectile.timeLeft < 8) { projectile.alpha = 64 + (8 - projectile.timeLeft) * 24; }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool PreDrawExtras()
        {
            CNCUtils.UpdateEffectVar(ref alpha);
            Color color = Color.PaleGoldenrod; color.A = 0;
            Texture2D star = ModContent.Request<Texture2D>("CNC/Assets/Particles/Star4").Value;
            Texture2D light = ModContent.Request<Texture2D>("CNC/Assets/Particles/Light4").Value;
            Vector2 pos = projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(star, pos, null, color * (projectile.timeLeft / 28f) * (alpha[0] / 100f), projectile.rotation * projectile.rotation, star.Size() / 2f, new Vector2((projectile.scale - .75f) * MyMath.phi, projectile.scale - .75f), 0, 0);
            //if (projectile.timeLeft >= 27) Main.EntitySpriteDraw(light, pos, null, new Color(255, 255, 255, 0), 0, light.Size() / 2f, 99f, 0, 0);
            return true;
        }
    }
}
