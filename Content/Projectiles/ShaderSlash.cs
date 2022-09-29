using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Global.Projectiles;
using CNC.Content.DataStructures;
using System.Collections.Generic;

namespace CNC.Content.Projectiles
{
    public class ShaderSlash : CNCProjectile
    {
        public sealed override int countFrame => 1;

        public Texture2D imageSlash;
        public Texture2D imageSlashTop;
        public Texture2D imageSlashTrace;
        public Texture2D imageSlashHit;
        public Texture2D imageWeapon;
        public Color colorSlash;
        public Color colorSlashTop;
        public Color colorSlashTrace;
        public Color colorHead;
        public Color colorTail;
        public SlashInfo[] attackInfo;
        public int timeLeft;
        public int slashLength;
        public Vector4[] slashPosition;
        public Vector2 direction;
        public Vector2 hitboxOffset;
        public BlendState blendStyle = BlendState.Additive;
        public float opacity = 1f;
        public float fadeSpeed;
        public float traceChangeSpeed = -.2f;
        private bool fading = false;

        public ShaderSlash SetDefaults(Vector2 position, Vector2 direction, int slashLength, int time, int fadeTime, int precision = 1)
        {
            this.direction = direction;
            this.slashLength = slashLength * precision;
            this.timeLeft = time * precision;
            this.projectile.timeLeft = time * precision;
            this.projectile.extraUpdates = precision - 1;
            this.projectile.Center = position;
            this.projectile.ai[1] = this.projectile.timeLeft;
            this.fadeSpeed = 1f / (fadeTime * precision);
            this.Initialize();
            return this;
        }

        public ShaderSlash SetImage(Texture2D slash, Texture2D slashTop, Texture2D slashTrace, Texture2D slashHit = null, Texture2D weapon = null)
        {
            this.imageSlash = slash;
            this.imageSlashTop = slashTop;
            this.imageSlashTrace = slashTrace;
            this.imageSlashHit = slashHit;
            this.imageWeapon = weapon;
            return this;
        }

        public ShaderSlash SetColor(Color slash, Color slashTop, Color slashTrace, Color head, Color tail)
        {
            this.colorSlash = slash;
            this.colorSlashTop = slashTop;
            this.colorSlashTrace = slashTrace;
            this.colorHead = head;
            this.colorTail = tail;
            return this;
        }

        /// <summary> 设定伤害项 </summary>
        /// <param name="style">友好/敌对型</param>
        /// <param name="type">伤害类型</param>
        /// <param name="damage">伤害值</param>
        /// <param name="knockback">击退</param>
        /// <param name="crit">暴击率</param>
        /// <param name="add">追加伤害</param>
        /// <param name="mult">伤害倍率</param>
        public ShaderSlash SetDamage(int style, DamageClass type, int damage, float knockback, int crit, float add = 0f, float mult = 1f)
            => this.SetDamage((ProjDamageStyle)style, type, damage, knockback, crit, add, mult);
        /// <summary> 设定伤害项 </summary>
        /// <param name="style">友好/敌对型</param>
        /// <param name="type">伤害类型</param>
        /// <param name="damage">伤害值</param>
        /// <param name="knockback">击退</param>
        /// <param name="crit">暴击率</param>
        /// <param name="add">追加伤害</param>
        /// <param name="mult">伤害倍率</param>
        public ShaderSlash SetDamage(ProjDamageStyle style, DamageClass type, int damage, float knockback, int crit, float add = 0f, float mult = 1f)
        {
            this.DamageStyle = style;
            this.projectile.DamageType = type;
            this.projectile.damage = (int)((damage + add) * mult);
            this.projectile.knockBack = knockback;
            this.projectile.CritChance = crit;
            return this;
        }

        public ShaderSlash SetAttackInfo(SlashInfo[] info)
        {
            this.attackInfo = info;
            return this;
        }

        public ShaderSlash SetDraw(BlendState style, float opacity = 1f, float traceChangeSpeed = -.2f)
        {
            this.blendStyle = style;
            this.opacity = opacity;
            this.traceChangeSpeed = traceChangeSpeed;
            return this;
        }

        public override void ResetDefaults()
        {
            projectile.timeLeft = 50;
            projectile.width = 4;
            projectile.height = 4;
            projectile.alpha = 0;
            projectile.light = 0f;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            this.UpdateData();
            projectile.velocity = Vector2.Zero;
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            if(projectile.friendly) projectile.Center = owner.Center;
            if (!this.fading && projectile.timeLeft <= 1) this.fading = true;
            if (this.fading)
            {
                if (this.Alpha1 < 1f)
                {
                    projectile.friendly = false;
                    projectile.timeLeft = 2;
                    this.Alpha1 = MyMath.Min(this.Alpha1 + this.fadeSpeed, 1f);
                }
            }
            if (!this.fading) projectile.ai[1]--;
            else projectile.ai[1] -= fadeSpeed * slashLength * .25f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector4 info = this.GetSlashInfo();
            float rot = projectile.rotation;
            Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
            Vector2 end = begin + info.ZW().RotatedBy(rot);
            Vector2 dist = end - begin;
            bool inRange = false;
            if (targetHitbox.Contains(begin.ToPoint()) || targetHitbox.Contains(end.ToPoint())) inRange = true;
            else
            {
                float[] pos = { targetHitbox.X, targetHitbox.Y, targetHitbox.X + targetHitbox.Width, targetHitbox.Y + targetHitbox.Height };
                bool containsX1 = MyMath.Min(begin.X, end.X, pos[0]) != pos[0] && MyMath.Max(begin.X, end.X, pos[0]) != pos[0];
                bool containsX2 = MyMath.Min(begin.X, end.X, pos[2]) != pos[2] && MyMath.Max(begin.X, end.X, pos[2]) != pos[2];
                bool containsY1 = MyMath.Min(begin.Y, end.Y, pos[1]) != pos[1] && MyMath.Max(begin.Y, end.Y, pos[1]) != pos[1];
                bool containsY2 = MyMath.Min(begin.Y, end.Y, pos[3]) != pos[3] && MyMath.Max(begin.Y, end.Y, pos[3]) != pos[3];
                if (containsY1)
                {
                    float dy = pos[1] - begin.Y;
                    float x = begin.X + dist.X * (dy / dist.Y);
                    if (pos[0] <= x && x <= pos[2]) inRange = true;
                }
                else if (containsY2)
                {
                    float dy = pos[3] - begin.Y;
                    float x = begin.X + dist.X * (dy / dist.Y);
                    if (pos[0] <= x && x <= pos[2]) inRange = true;
                }
                else if (containsX1)
                {
                    float dx = pos[0] - begin.X;
                    float y = begin.Y + dist.Y * (dx / dist.X);
                    if (pos[1] <= y && y <= pos[3]) inRange = true;
                }
                else if (containsX2)
                {
                    float dx = pos[2] - begin.X;
                    float y = begin.Y + dist.Y * (dx / dist.X);
                    if (pos[1] <= y && y <= pos[3]) inRange = true;
                }
            }
            return !this.fading && inRange;
        }

        public override void HitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (this.imageSlashHit != null)
            {
                float rot = this.GetSlashInfo().W + MyMath.deg60 * 2 - CNC.Random.NextDegree(60f);
                int dir = (this.attackInfo[1].rotation > this.attackInfo[0].rotation).ToDirectionInt();
                var hit = CNCUtils.CreateSlashHit(projectile, Vector2.Zero);
                hit.SetDefaults(this.imageSlashHit, new Vector2(MyMath.phi - 1f, 1f) * .5f, target.Center, -10, rot * dir);
                hit.SetColor(this.colorSlash, this.colorSlash, .1f);
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            this.DrawSlash(Main.spriteBatch);
            this.DrawWeapon(Main.spriteBatch);
            return false;
        }

        private void DrawWeapon(SpriteBatch spriteBatch)
        {
            Texture2D image = this.imageWeapon ?? Terraria.GameContent.TextureAssets.Item[owner.HeldItem.type].Value;
            Vector4 info = this.GetSlashInfo();
            Vector2 pos = owner.Center - Main.screenPosition;
            Vector2 origin = new Vector2(0, image.Height);
            int direction = (this.attackInfo[1].rotation > this.attackInfo[0].rotation).ToInt(1);
            int direction2 = projectile.direction * (this.attackInfo[0].rotation > this.attackInfo[1].rotation).ToDirectionInt(1);
            float alpha = 1f;
            float rotation = info.ZW().ToRotation() + projectile.rotation;
            float rotation2 = projectile.direction * (this.attackInfo[0].rotation > this.attackInfo[1].rotation).ToDirectionInt(1) * MyMath.deg45;
            float scale = MyMath.Clamp(info.ZW().Length() / image.Size().Length(), .75f, 1.75f);
            if (direction2 != 1) { origin.X = image.Width; rotation2 += MyMath.pi; }
            Main.EntitySpriteDraw(image, pos, null, Color.White * this.Alpha2 * alpha, rotation + rotation2, origin, scale, (SpriteEffects)(direction2 != 1).ToInt(1), 0);
        }

        private void DrawSlash(SpriteBatch spriteBatch)
        {
            List<VertexInfo> vertex = new List<VertexInfo>();
            float max = MyMath.Lerp(0f, this.slashLength, (float)(this.timeLeft - projectile.timeLeft) / (float)this.slashLength);
            for (int i = 0; i < max; i++)
            {
                if (slashPosition[i].XY() == Vector2.Zero || slashPosition[i].ZW() == Vector2.Zero) break;
                float amount = i / max;
                float w = MyMath.Lerp(1f, 0f, amount);
                Color color = Color.Lerp(this.colorHead, this.colorTail, amount) * w * Alpha2;
                vertex.Add(new VertexInfo(slashPosition[i].XY(), color, new Vector3(amount, 0f, w)));
                vertex.Add(new VertexInfo(slashPosition[i].ZW(), color, new Vector3(amount, 1f, 0f)));
            }
            if (vertex.Count > 2)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, this.blendStyle, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                if (imageSlash != null)
                {
                    Main.graphics.GraphicsDevice.Textures[0] = imageSlash;
                    Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                }
                if (imageSlashTop != null)
                {
                    Main.graphics.GraphicsDevice.Textures[1] = imageSlashTop;
                    Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                }
                if (imageSlashTrace != null)
                {
                    Main.graphics.GraphicsDevice.Textures[2] = imageSlashTrace;
                    Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                }

                Matrix projection = Matrix.CreateOrthographicOffCenter(0f, Main.screenWidth, Main.screenHeight, 0f, 0f, 1f);
                Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0f)) * Main.GameViewMatrix.ZoomMatrix;
                
                Effect shader = CNC.GetEffect("Slash");
                shader.Parameters["uTransform"].SetValue(model * projection);
                shader.Parameters["uColor"].SetValue(Color.White.ToVector4());
                shader.Parameters["uColor0"].SetValue(colorSlash.ToVector4());
                shader.Parameters["uColor1"].SetValue(colorSlashTop.ToVector4());
                shader.Parameters["uColor2"].SetValue(colorSlashTrace.ToVector4());
                shader.Parameters["uAlpha"].SetValue(this.Alpha2 * this.opacity);
                shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * this.traceChangeSpeed);
                shader.CurrentTechnique.Passes[0].Apply();
                
                Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex.ToArray(), 0, vertex.Count - 2);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }

        private void UpdateData()
        {
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            for (int i = 0; i < slashLength; i++)
            {
                double time = ((double)projectile.ai[1] + i) / this.timeLeft;
                Vector4 info = this.GetSlashInfo(time);
                float rot = projectile.rotation;
                Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                Vector2 end = begin + info.ZW().RotatedBy(rot);
                slashPosition[i] = new Vector4(begin.X, begin.Y, end.X, end.Y);
            }
        }

        private void Initialize()
        {
            int len = slashLength;
            this.slashPosition = new Vector4[len];
            while (len-- > 0)
            {
                this.slashPosition[len] = new Vector4(projectile.position, projectile.position.X, projectile.position.Y);
            }
        }

        /// <summary> 获取当前帧的斩击信息 <para> Vector4的XYZW分别对应：横坐标偏移，纵坐标偏移，攻击距离，攻击角度）</para> </summary>
        public virtual Vector4 GetSlashInfo() => this.GetSlashInfo(this.fading ? 0.0d : (double)projectile.timeLeft / this.timeLeft);
        /// <summary> 获取当前帧的斩击信息 <para> Vector4的XYZW分别对应：横坐标偏移，纵坐标偏移，攻击距离，攻击角度）</para> </summary>
        public virtual Vector4 GetSlashInfo(double amount)
        {
            double clamp(double v, double min, double max) => MyMath.Min(MyMath.Max(min, v), max);
            Vector2 lerp(Vector2 v1, Vector2 v2, double percentage)
            {
                Vector2 v = new Vector2();
                percentage = clamp(percentage, 0.0d, 1.0d);
                v.X = (float)(v1.X * (1.0d - percentage) + v2.X * percentage);
                v.Y = (float)(v1.Y * (1.0d - percentage) + v2.Y * percentage);
                return v;
            }
            amount = clamp(amount, 0.0d, 1.0f);
            int count = this.attackInfo.Length;
            double eps = 1e5d;
            double len = eps / count;
            double amount2 = amount * eps % len / len;
            int index = MyMath.Clamp((int)(amount * count) + 1, 1, count - 1);
            double amount3 = Math.Pow(amount2, this.attackInfo[index].power);
            Vector2 offset = lerp(this.attackInfo[index - 1].offset, this.attackInfo[index].offset, amount3);
            Vector2 position = lerp(this.attackInfo[index - 1].position, this.attackInfo[index].position, amount3);
            return new Vector4(offset, position.X, position.Y);
        }
    }
}
