
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Superconductor
{
    public class LightningChain : CNCProjectile
    {
        NPC[] ignoreNPC = { null, null, null, null, null };
        NPC currentTarget = null;
        bool foundTarget = false;
        float deviation = 0f;
        float direction = 0f;
        int chance = 0;
        float canHit = 0;
        const int time = 12 * 60;

        bool demo => data.ContainsKey("noDamage") ? (bool)data["noDamage"] : true;
        bool upgraded => data.ContainsKey("upgraded") ? (bool)data["upgraded"] : false;

        public override void ResetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.timeLeft = time;
            projectile.alpha = 255;
            projectile.penetrate = 5;
            projectile.extraUpdates = 60;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.DamageType = DamageClass.Magic;
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            this.NewProjectile(projectile.Center - projectile.oldVelocity, Const._02, CNCID.Projectile.LegacyObject[CNCID.Projectile.LightningExplosion], 0, 0f, CNC.Random.NextFloat(MyMath.pi2));
            for (int i = 0; i < ignoreNPC.Length; i++) { if (ignoreNPC[i] == null || ignoreNPC[i] == target) { ignoreNPC[i] = target; break; } }
            projectile.timeLeft = (int)(12 * 60);
            projectile.velocity = Const._02;
            projectile.friendly = false;
            foundTarget = false;
            currentTarget = null;
            //CNCUtils.PlaySound("Superconductor/Chain", projectile.Center, 1f, Main.rand.NextFloat(.1f, .3f), mod);
            if (target.netID == 488) { projectile.penetrate++; }
            //SpellDamage(p, damage, crit);
        }

        public override void AI()
        {
            if (demo)//ÌØÐ§
            {
                Player owner = Main.player[projectile.owner];
                projectile.penetrate = -1;
                projectile.extraUpdates = 120;
                projectile.tileCollide = false;
                projectile.rotation = 0f;
                projectile.velocity = new Vector2(MyMath.sin(projectile.rotation), -MyMath.cos(projectile.rotation)) * 3f;
                float num = Main.rand.NextFloat(Main.rand.NextFloat(3, 4), Main.rand.NextFloat(5, 6)) / 12;
                bool update = CNC.Random.NextBool(20 + chance);
                chance = update ? 0 : chance + 10;
                if (direction == 0) { direction = CNC.Random.NextBool() ? 1 : -1 * Main.rand.NextFloat(0.1f, 3); projectile.position = owner.Center + new Vector2(owner.direction * -5f, -50f); }
                if (((direction > 0 && direction * deviation > num) || (direction < 0 && (-direction) * deviation < -num)) && update) { direction = (direction < 0) ? 1 : -1 * Main.rand.NextFloat(0.1f, 2f); }
                deviation += direction * Main.rand.NextFloat(4f, 5f);
                float x = (float)(deviation * Math.Cos(projectile.rotation));
                float y = (float)(deviation * Math.Sin(projectile.rotation));
                Vector2 dev = new Vector2(x, y);
                projectile.position += dev * .1f;
            }
            else
            {
                if (canHit == 0) canHit = (int)CNCID.Inscription.S_SpellLightningChain.Data[1] + ((bool)data["upgraded"] ? (int)CNCID.Inscription.S_SpellAugmentated.Data[0] : 0);
                if (projectile.penetrate > canHit) { projectile.penetrate = (int)Math.Floor(canHit); }
                NPC target = foundTarget ? currentTarget : FindTargetAI();
                projectile.velocity = foundTarget ? projectile.velocity : Const._02;
                projectile.knockBack = foundTarget ? 0f : 3f;
                Player owner = Main.player[projectile.owner];
                for (int i = 0; i < ignoreNPC.Length; i++)
                { if (ignoreNPC[i] == null || ignoreNPC[i] == target) { ignoreNPC[i] = target; break; } }
                if (foundTarget && target != null)
                {
                    projectile.tileCollide = true;
                    projectile.friendly = false;
                    projectile.friendly = Vector2.Distance(target.Center, projectile.Center) < 8f;
                    projectile.rotation = MyMath.pi / 2 + (target.Center - projectile.Center).ToRotation();
                    projectile.velocity = new Vector2(MyMath.sin(projectile.rotation), -MyMath.cos(projectile.rotation)) * 2f;
                    float num = Main.rand.NextFloat(Main.rand.NextFloat(3, 4), Main.rand.NextFloat(5, 6)) / 12;
                    bool update = CNC.Random.NextBool(20 + chance);
                    chance = update ? 0 : chance + 10;
                    if (direction == 0) { direction = CNC.Random.NextBool() ? 1 : -1 * Main.rand.NextFloat(0.1f, 3); projectile.position = owner.Center + new Vector2(owner.direction * -5f, -50f); }
                    if (((direction > 0 && direction * deviation > num) || (direction < 0 && (-direction) * deviation < -num)) && update) { direction = (direction < 0) ? 1 : -1 * Main.rand.NextFloat(0.1f, 2f); }
                    deviation += direction * Main.rand.NextFloat(4f, 5f);
                    float x = (float)(deviation * Math.Cos(projectile.rotation));
                    float y = (float)(deviation * Math.Sin(projectile.rotation));
                    Vector2 dev = new Vector2(x, y);
                    projectile.Center += dev * .1f;
                }
                else { projectile.Kill(); return; }
            }
            Color color = Color.LightBlue;
            color.A = 0;
            int idx = Particle.NewParticle(
                ModContent.Request<Texture2D>("CNC/Assets/Particles/Light2").Value,
                projectile.Center,
                Const._02,
                0,
                30,
                new Vector2(.175f),
                color
                );
            ParticleRenderer.particle[idx].scaleAdd = -new Vector2(.0002f);
            ParticleRenderer.particle[idx].alpha = .75f;
            ParticleRenderer.particle[idx].alphaAdd = -.025f;
            idx = Particle.NewParticle(
                ModContent.Request<Texture2D>("CNC/Assets/Particles/Light2").Value,
                projectile.Center,
                Const._02,
                0,
                30,
                new Vector2(.05f),
                color
                );
            ParticleRenderer.particle[idx].scaleAdd = -new Vector2(.0001f);
            ParticleRenderer.particle[idx].alpha = .4f;
            ParticleRenderer.particle[idx].alphaAdd = -.013f;
        }

        private NPC FindTargetAI()
        {
            if (currentTarget != null) { return currentTarget; }
            NPC target = null;
            float range = 16f * (24f + (upgraded ? (int)CNCID.Inscription.S_SpellAugmentated.Data[1] : 0f));
            float dist = 0;
            float closestDist = range;
            bool closer = false;
            bool inRange = false;
            bool lineOfSight = false;
            for (int i = 0; i < Const.max1; i++)
            {
                NPC npc = Main.npc[i];
                if (!foundTarget && npc.CanBeChasedBy())
                {
                    lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                    dist = Vector2.Distance(npc.Center, projectile.Center);
                    inRange = dist < range;
                    closer = dist < closestDist;
                    if (!inRange || !closer || !lineOfSight || npc.netID == 488) { continue; }
                    for (int j = 0; j < ignoreNPC.Length; j++)
                    {
                        if (npc != ignoreNPC[j] && ignoreNPC[j] == null)
                        {
                            closestDist = dist;
                            target = npc;
                        }
                        else if (npc == ignoreNPC[j]) { break; }
                    }
                }
            }
            currentTarget = target;
            foundTarget = target != null;
            return target;
        }
    }
}