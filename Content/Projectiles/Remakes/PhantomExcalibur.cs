using System;
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
    public class PhantomExcalibur : CNCProjectile
    {
        private const float capabilityMult = 1.2f;
        private int time = 1200;
        private int timer => time - projectile.timeLeft;
        private Color mainColor = Color.LightGoldenrodYellow;
        private bool attack => slash != null && !slash.fading;
        private int attackDelay = -1;
        private int attackDelayMax = -1;
        private ShaderSlash slash;
        private Vector2 targetPos;
        private float targetRot;
        private Vector2 attackPos;
        private float attackRot;//换成vector2，x为角度y为方向
        private Vector2 oldPosition;
        private float oldRotation;
        private Item oldItem;
        private List<AtkData> atkDatas;
        private EffectTimer eft = new EffectTimer(75, -1, 0, 35, 75, 0, 0, 1, 0);
        private EffectTimer driftPos = new EffectTimer(0, 1, 0, -32, 32, 15, 15, 1, 0);
        private EffectTimer driftRot = new EffectTimer(0, 1, 0, -20, 60, 15, 15, 1, 0);

        public override void ResetDefaults()
        {
            projectile.timeLeft = time;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.scale = 1f;
            projectile.damage = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void OnSpawn()
        {
            oldItem = owner.HeldItem;
            attackPos = projectile.Center;
            attackRot = projectile.rotation;
            atkDatas = new List<AtkData>();
        }

        public override void AI()
        {
            if (owner.HeldItem != oldItem) projectile.timeLeft = 1;
            if (projectile.timeLeft < 2)
            {
                if (this.Alpha2 > 0f)
                {
                    projectile.timeLeft = 2;
                    if (!this.attack) Alpha2 -= .05f;
                }
            }
            else if (this.Alpha2 < 1f) this.Alpha2 += .025f;
            if (timer <= 30)
            {
                if (timer == 30)
                {
                    Player player = owner;
                    Item item = player.HeldItem;
                    float damage = player.GetWeaponDamage(item) * capabilityMult;
                    float knockback = player.GetWeaponKnockback(item) * capabilityMult;
                    float crit = player.GetWeaponCrit(item) * capabilityMult;
                    var source = item.GetSource_ItemUse(item);
                    Vector2 position = player.Center;
                    damage *= 3;
                    knockback *= 1.5f;
                    crit += 20f;
                    slash = CNCUtils.NewProj(source, projectile.Center, projectile.velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.AnattaSlash], (int)damage, knockback, (int)crit, owner.whoAmI, 0f, 1f, 6f, 0).ModProjectile as ShaderSlash;
                    slash.imageWeapon = CNC.GetTexture("Assets/Misc/None");
                    slash.scale *= capabilityMult;
                    slash.bindToOwner = false;
                    slash.projectile.Center = projectile.Center;
                    projectile.velocity = Vector2.Zero;
                }
                else projectile.Center = owner.Center;
            }
            else if (projectile.timeLeft > 2 && owner.itemTime == owner.itemTimeMax)
            {
                int atkData_type = owner.HeldItem.GetGlobalItem<Common.Global.Items.AllItem>().comboCur;
                targetPos = attackPos = owner.Center;
                var p = NewProjectile(1, attackRot, CNCID.Projectile.LegacyObject[CNCID.Projectile.AnattaSlash], 0, attackRot, atkData_type * .1f, 0f).ModProjectile as ShaderSlash;
                atkDatas.Add(new AtkData()
                {
                    velocity = new Vector2(owner.HeldItem.shootSpeed, 0).RotatedBy((Main.MouseWorld - owner.Center).ToRotation()),
                    position = owner.Center,
                    rotation = (p.GetSlashInfo().W + (Main.MouseWorld - owner.Center).ToRotation()) % MyMath.pi2,
                    direction = p.projectile.direction * (p.attackInfo[0].rotation + MyMath.pi2 > p.attackInfo[1].rotation + MyMath.pi2).ToDirectionInt(1) * p.weaponDirection,
                    delay = MyMath.Max(owner.itemTime / 2, 10),
                    type = atkData_type,
                });
                p.projectile.Kill();
            }

            targetRot %= MyMath.pi2;
            projectile.rotation %= MyMath.pi2;
            if (!attack && attackDelay < 0 && atkDatas.Count > 0 && owner.HeldItem == oldItem)
            {
                oldPosition = projectile.Center;
                oldRotation = projectile.rotation;
                attackDelayMax = attackDelay = atkDatas[0].delay;
                targetPos = attackPos = atkDatas[0].position;
                targetRot = atkDatas[0].rotation;
                attackRot = atkDatas[0].velocity.ToRotation();
                projectile.direction = atkDatas[0].direction;
            }

            if (!attack && attackDelay < 0)
            {
                targetPos = owner.Center - new Vector2(owner.direction * 32f, 64f + driftPos.GetNextValue());
                projectile.Center += (targetPos - projectile.Center) * .05f;
                if (projectile.Center.Distance(targetPos) < .1f) projectile.Center = targetPos;
                projectile.direction = (projectile.Center.X > owner.Center.X).ToDirectionInt(1);
                float extraRot = timer < 30 ? 0f : (owner.Center.Y > projectile.Center.Y).ToDirectionInt(1) * MyMath.Lerp(0f, 20f, Math.Abs(owner.Center.Distance(projectile.Center)) / 100f) * MyMath.deg01;
                targetRot = MyMath.deg90 - (MyMath.deg15 + driftRot.GetNextValue() * MyMath.deg01 / 4f + extraRot) * projectile.direction;
                targetRot %= MyMath.pi2;
                projectile.rotation %= MyMath.pi2;
                projectile.rotation = RotLerp(projectile.rotation, targetRot, .025f);
                projectile.rotation %= MyMath.pi2;
            }

            if (attackDelay > 0)
            {
                targetRot %= MyMath.pi2;
                projectile.rotation %= MyMath.pi2;
                float amount = (float)Math.Sqrt(1f - attackDelay-- / (float)attackDelayMax);
                projectile.Center = MyMath.Lerp(oldPosition, owner.Center, amount);
                projectile.rotation = RotLerp(oldRotation, targetRot, amount);
            }
            else if (attackDelay == 0) Attack();
            targetRot %= MyMath.pi2;
            projectile.rotation %= MyMath.pi2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D image = Texture2;
            float alpha = eft.GetNextValue() / 100f;

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            
            Effect effect = CNC.GetEffect("HighLight");
            effect.Parameters["uColor0"].SetValue(Color.Blue.Flash().ToVector4());
            effect.Parameters["uColor1"].SetValue(Color.Magenta.Flash().ToVector4());
            effect.Parameters["uImageSize"].SetValue(image.Size());
            effect.Parameters["uSize"].SetValue(image.Size());
            effect.Parameters["uOffset"].SetValue(new Vector2(.5f) * Main.GlobalTimeWrappedHourly);
            effect.Parameters["uAlpha"].SetValue(this.Alpha2 * alpha);
            effect.Parameters["uFrame"].SetValue(0);
            effect.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.Textures[0] = image;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            Main.graphics.GraphicsDevice.Textures[1] = CNC.GetTexture("Assets/Effects/Trace2");
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            
            if (this.attack)
            {
                Vector4 info = slash.GetSlashInfo();
                projectile.rotation = info.W + slash.projectile.rotation;
                projectile.rotation %= MyMath.pi2;
                Vector2 begin = projectile.Center + (slash.hitboxOffset + info.XY()).RotatedBy(projectile.rotation);
                Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(projectile.rotation);
                Vector2 pos = projectile.Center - Main.screenPosition;
                Vector2 origin = new Vector2(0, image.Height);
                projectile.direction = slash.projectile.direction * (slash.attackInfo[0].rotation + MyMath.pi2 > slash.attackInfo[1].rotation + MyMath.pi2).ToDirectionInt(1) * slash.weaponDirection;
                float rotation2 = projectile.direction * MyMath.deg45;
                float scale = MyMath.Clamp(info.Z / image.Size().Length(), .75f, 1.75f) * capabilityMult;
                if (projectile.direction != 1) { origin.X = image.Width; rotation2 += MyMath.pi; }
                Main.EntitySpriteDraw(image, pos, null, Color.White.Flash() * this.Alpha2 * alpha, projectile.rotation + rotation2, origin, scale, (SpriteEffects)(projectile.direction != 1).ToInt(1), 0);
            }
            else
            {
                Vector2 pos = projectile.Center - Main.screenPosition;
                Vector2 origin = new Vector2(0, image.Height);
                int direction = (projectile.Center.X > owner.Center.X).ToDirectionInt(1);
                float rotation = projectile.rotation;
                float rotation2 = direction * MyMath.deg45;
                float scale = 1.75f * capabilityMult;
                if (direction != 1) { origin.X = image.Width; rotation2 += MyMath.pi; }
                Main.EntitySpriteDraw(image, pos, null, Color.White.Flash() * this.Alpha2 * alpha, rotation + rotation2, origin, scale, (SpriteEffects)(direction != 1).ToInt(1), 0);
                if (attackDelay < 0) projectile.direction = direction;
            }
            Main.graphics.GraphicsDevice.RasterizerState = originalState;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        
        private float RotLerp(float r1, float r2, float progress)
        {
            return Utils.AngleLerp(r1, r2, progress);
            /*
            float pi2 = MyMath.pi2;
            r1 %= pi2;
            r2 %= pi2;
            if (r1 < 0 || r2 < 0) { r1 += pi2; r2 += pi2; }
            r1 %= pi2;
            r2 %= pi2;
            if (Math.Abs(r1 - r2) > Math.Abs(r1 - (r2 + pi2))) r2 += pi2;
            else if (Math.Abs(r1 - r2) > Math.Abs((r1 + pi2) - r2)) r1 += pi2;
            return MyMath.Lerp(r1, r2, progress);
            */
        }

        private void Attack()
        {
            Player player = owner;
            Item item = player.HeldItem;
            Vector2 velocity = new Vector2(item.shootSpeed * 3f, 0f).RotatedBy(attackRot);
            float damage = player.GetWeaponDamage(item) * capabilityMult;
            float knockback = player.GetWeaponKnockback(item) * capabilityMult;
            float crit = player.GetWeaponCrit(item) * capabilityMult;
            var source = item.GetSource_ItemUse(item);
            if (atkDatas[0].type > 1) CNCUtils.NewProj(source, projectile.Center, velocity, ProjectileID.LightBeam, (int)damage, knockback, (int)crit, owner.whoAmI, 0f, 1f);
            damage *= new float[] { 1.1f, .9f, 1.5f }[(int)this.atkDatas[0].type / 2];
            slash = CNCUtils.NewProj(source, projectile.Center, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.AnattaSlash], (int)damage, knockback, (int)crit, owner.whoAmI, 0f, 1f, 5f + atkDatas[0].type * .1f, 0).ModProjectile as ShaderSlash;
            slash.imageWeapon = CNC.GetTexture("Assets/Misc/None");
            slash.scale *= capabilityMult;
            slash.bindToOwner = false;
            slash.projectile.Center = projectile.Center;
            atkDatas.RemoveAt(0);
            attackDelay = -1;
        }

        private struct AtkData
        {
            public Vector2 velocity;
            public Vector2 position;
            public float rotation;
            public int direction;
            public int delay;
            public int type;
        }
    }
}
