using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public class AnnulusOfPneuma : CNCProjectile
    {
        private const int dustid1 = DustId.TrailingCyan;
        private const int dustid2 = DustId.CyanShortFx1;
        private Color mainColor => CNCID.Colors.SpiritBoss;
        private Color mainColor2 => CNCID.Colors.SpiritBoss2;
        protected int attackCD = 0;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).rage;
        private float rand;
        private Vector2[][] effectPos;

        public override void ResetDefaults()
        {
            projectile.timeLeft = 360;
            projectile.alpha = 0;
            projectile.light = 0;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            effectPos = new Vector2[3][]
            {
                new Vector2[20],
                new Vector2[20],
                new Vector2[20],
            };
            for (int i = 0; i < effectPos.Length; i++)
                for (int j = 0; j < effectPos[i].Length; j++)
                    effectPos[i][j] = projectile.Center;
            rand = CNC.Random.NextFloat(-100f, 100f);
        }

        public override void AI()
        {
            float z = (float)Math.Sqrt(velX * velX + velY * velY);
            projectile.rotation += MyMath.deg01 * z * projectile.direction;
            if (z < 8f) projectile.velocity += Vector2.Normalize(projectile.velocity) * .1f;

            for (int i = 0; i < effectPos.Length; i++)
                for (int j = effectPos[i].Length - 1; j > 0; j--)
                    effectPos[i][j] = effectPos[i][j - 1];
            effectPos[0][0] = projectile.Center + new Vector2(100f, 0f).RotatedBy(projectile.rotation + MyMath.deg01 * 70);
            effectPos[1][0] = projectile.Center + new Vector2(100f, 0f).RotatedBy(projectile.rotation + MyMath.deg01 * 188);
            effectPos[2][0] = projectile.Center + new Vector2(100f, 0f).RotatedBy(projectile.rotation + MyMath.deg01 * 315);
            if(projectile.timeLeft % 10 == 0 && z >= 8f)
            {
                Vector2 pos = effectPos[CNC.Random.NextInt(3)][0];
                float rot = (FindClosestTarget(projectile, 600f).Center - pos).ToRotation();
                this.NewProjectile(pos, 24 * MyMath.GetXY(rot), CNCID.Projectile.LegacyObject[CNCID.Projectile.SpiritArrowV1], 0, 0, 0);
                CNCUtils.PlaySound(SoundID.Item28, projectile);
            }
        }

        public override bool PreDraw(ref Color lightColor) => false;

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, mainColor2, projectile.rotation, texture.Size() / 2f, .375f, 0, 0);
            for (int j = 0; j < effectPos.Length; j++)
            {
                var ef = Mod.Assets.Request<Effect>("Content/Effects/Trail").Value;
                List<VertexInfo> bars = new List<VertexInfo>();
                int width = 32;
                var normalDir = effectPos[j][0] - effectPos[j][1];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var color = mainColor;
                var w = MathHelper.Lerp(1f, 0.05f, 0);
                
                for (int i = 1; i < effectPos[j].Length; ++i)
                {
                    if (effectPos[j][i] == Vector2.Zero) break;

                    normalDir = effectPos[j][i - 1] - effectPos[j][i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = i / (float)effectPos[j].Length;
                    var factor2 = MyMath.Clamp(factor * 2, 0, 1);
                    color = Color.Lerp(mainColor, Color.Transparent, factor);
                    w = MathHelper.Lerp(1f, 0.05f, factor);

                    bars.Add(new VertexInfo(effectPos[j][i - 1] + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new VertexInfo(effectPos[j][i - 1] - normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }

                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly + rand);
                var trai = new TrailShaderData(ef, projectile, width)
                {
                    colorBegin = mainColor,
                    trailAnimAdd = rand,
                    imageHeatmap = Mod.Assets.Request<Texture2D>("Assets/Effects/Heatmap/Blue2").Value,
                    imageTrace = Mod.Assets.Request<Texture2D>("Assets/Effects/Trace2").Value,
                    imageTrail = Mod.Assets.Request<Texture2D>("Assets/Effects/FormalTrail6").Value,
                };
                trai.Draw(Main.spriteBatch, bars);
            }
        }
    }
}