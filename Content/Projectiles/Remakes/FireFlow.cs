using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using CNC.Content.DataStructures;
using System.Collections.Generic;

namespace CNC.Content.Projectiles.Remakes
{
    public class FireFlow : CNCProjectile
    {
        private int time = 60;
        private int timer => time - projectile.timeLeft;
        private Color mainColor = new Color(242, 92, 34);
        private bool upgrade = false;
        public override int countFrame => 4;

        public override void ResetDefaults()
        {
            projectile.timeLeft = time;
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.scale = 1f;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
        }

        public override void OnSpawn()
        {
            if (projectile.ai[0] != 0f)
            {
                upgrade = true;
                time = (int)(time * 1.5f);
                projectile.timeLeft = time;
                projectile.damage /= 2;
                projectile.penetrate = 1;
                projectile.extraUpdates = 1;
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                projectile.netUpdate2 = true;
                projectile.ai[0] = 0f;
            }
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (CNC.Random.NextBool() || projectile.ai[0] != 0f) target.AddBuff(BuffID.OnFire, 180);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(time);
            writer.Write(upgrade);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            time = reader.ReadInt32();
            upgrade = reader.ReadBoolean();
        }

        public override void AI()
        {
            if (projectile.ai[0] != 0f) OnSpawn();
            if (timer < 25 && Alpha2 < 1f) Alpha2 += .04f;
            if (projectile.timeLeft < 30 && Alpha2 > 0f) Alpha2 -= .0334f;
            float extra = MyMath.sin(projectile.timeLeft % 10 / 10f * MyMath.pi2);
            AI001_RollingMissile(projectile, false, true, 90 + extra * 15f);
            Anim001_Loop(projectile, 4, 0);
            projectile.position += new Vector2(0f, extra * 4f).RotatedBy(projectile.velocity.ToRotation());
            CNCUtils.AddLight(projectile.position, mainColor, .8f);
            if (!upgrade) CNCUtils.NewDust(projectile.Center, DustId.Fire, CNC.Random.NextFloat(1f, 2f), projectile.velocity * .2f, mainColor.Flash()).noGravity = true;
            else
            {
                int idx = Particle.NewParticle(
                    ModContent.Request<Texture2D>("CNC/Assets/Particles/Light1").Value,
                    projectile.Center + new Vector2(Main.rand.NextFloat(-1f, 1f) * Main.rand.NextFloat(-1f, 1f)) * projectile.Size * projectile.scale / 2f,
                    -new Vector2(projectile.velocity.X, 6f) * Main.rand.NextFloat(.05f, .1f),
                    0f,
                    100,
                    new Vector2(Main.rand.NextFloat(.03f, .09f)),
                    mainColor.NewA(0),
                    mainColor.NewA(25)
                    );
                ParticleManager.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0003f, .0010f));
                ParticleManager.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = mainColor;
            Texture2D t = Texture2;
            Rectangle? source = new Rectangle(0, projectile.frame * t.Height / countFrame, t.Width, t.Height / countFrame);
            Vector2 pos = projectile.Center - Main.screenPosition;
            Vector2 origin = source.Value.Size() / 2f;
            if (!upgrade)
            {
                t = ModContent.Request<Texture2D>("Terraria/Images/Projectile_666").Value;
                source = null;
                origin = t.Center();
                lightColor = Color.White;
            }
            Main.EntitySpriteDraw(t, pos, source, lightColor.Flash() * this.Alpha2, projectile.rotation, origin, projectile.scale, 0, 0);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if(upgrade)
            {
                CNCUtils.PlaySound(SoundID.Item14, projectile);
                GenericExplosion e = CNCUtils.CreateExplosion(projectile);
                e.VanillaOnHitNPCEffect += OnHitNPC;
                e.SetDefaults(CNC.GetTexture("Assets/Projectiles/Explosion/FireFlowExplosion"), projectile.Center, Vector2.One * 49, new int[7] { 1, 1, 1, 2, 2, 2, 2 });
                e.SetDamage(ProjDamageStyle.Friendly, projectile.DamageType, projectile.damage, 0f, projectile.CritChance, 0f, 1f);
                e.SetColor(mainColor, mainColor, .3f);
                e.SetAlpha(1f, -.025f, 1f);
                e.SetScale(.5f, .025f, 1f);
                e.SetRotation(CNC.Random.NextDegree(), 0f, 1f);
            }
        }
    }
}
