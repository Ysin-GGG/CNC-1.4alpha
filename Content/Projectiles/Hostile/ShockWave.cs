
using Terraria;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using System.Collections.Generic;

namespace CNC.Content.Projectiles.Hostile
{
    public class ShockWave : CNCProjectile
    {
        public const int time = 90;
        private float maxRange = 32f;
        private float maxDist = 800f;
        private float maxStrength = 80f;

        public override void ResetDefaults()
        {
            projectile.scale = 0f;
            projectile.alpha = 64;
            projectile.width = 120;
            projectile.height = 120;
            projectile.timeLeft = time;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;

            Vector2 offScreenRange = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            Vector2 realScreenSize = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / Main.GameViewMatrix.Zoom;
            Vector2 screenCenter = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f;
            Vector2 realScreenPos = Main.screenPosition + screenCenter * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);

            Vector2 size = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
            Vector2 screen = realScreenPos - offScreenRange;
            Vector2 targetPos = projectile.Center - offScreenRange;

            float amount = 1 - (float)projectile.timeLeft / time;
            float strength = MyMath.Lerp(maxStrength, 0f, amount);
            float range = MyMath.Lerp(1f, maxRange, MyMath.sin(MyMath.pi * amount));
            float dist = MyMath.Lerp(0f, maxDist, amount);

            if (projectile.ai[0] == 1f)
            {
                foreach (Player player in Main.player)
                {
                    if (player != null && player.active)
                    {
                        Vector2 offset = player.Center - projectile.Center;
                        float dist2 = offset.Length();
                        float minRange = dist - range;
                        float maxRange = dist + range;
                        if (minRange <= dist2 && dist2 <= maxRange)
                        {
                            float intensity = (1 - (dist - dist2) / range) * (strength / maxStrength);
                            player.velocity = Vector2.Normalize(offset) * intensity * 16f;
                            if (player.velocity.Length() > 16f) player.velocity = Vector2.Normalize(player.velocity) * 16f;
                            Vector2 shake = Vector2.Normalize(offset) * 8f;
                            if (shake.X < 0) shake.X *= -1;
                            if (shake.Y < 0) shake.Y *= -1;
                            CNCUtils.ScreenShake(30, shake * 2, shake, .8f, default(Vector2), player);
                        }
                    }
                }
            }

            if (projectile.ai[0] == 2f)
            {
                projectile.extraUpdates = 2;
                range = MyMath.Lerp(48f, 80f, MyMath.sin(MyMath.pi * amount));
                dist = MyMath.Lerp(60f, 600f, amount);
            }

            Effect ef = CNC.Shader["ShockWave"];
            ef.Parameters["uScreenResolution"].SetValue(realScreenSize);
            ef.Parameters["uPosition"].SetValue((targetPos - screen) / size);
            ef.Parameters["uStrength"].SetValue(strength);
            ef.Parameters["uRange"].SetValue(range);
            ef.Parameters["uDist"].SetValue(dist);

            CNC.SetFilter("ShockWave", true);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            CNC.SetFilter("ShockWave", false);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
