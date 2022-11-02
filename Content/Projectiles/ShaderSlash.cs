using System;
using System.IO;
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
        public Color colorSlashHit;
        public Color colorHead;
        public Color colorTail;
        public SlashInfo[] attackInfo;
        public int timeLeft;
        public int slashLength;
        public Vector2 direction;
        public Vector2 hitboxOffset;
        public BlendState blendStyle = BlendState.Additive;
        public float opacity = 1f;
        public float scale = 1f;
        public bool fading = false;
        public bool bindToOwner = true;
        public double fadeSpeed;
        public double fadeSpeedMult = 1f;
        public int weaponDirection = 1;
        protected int slashStyle = 1;
        protected float traceChangeSpeed = -.2f;
        private int timer = 0;
        private double leftTimer = 0;
        private List<NPC> haveHit = new List<NPC>();

        public ShaderSlash SetDefaults(Vector2 position, Vector2 direction, int slashLength, int time, int fadeTime, int precision = 1)
        {
            this.direction = direction;
            this.slashLength = slashLength;
            this.timeLeft = time * precision;
            this.projectile.timeLeft = time * precision;
            this.projectile.extraUpdates = precision - 1;
            this.projectile.Center = position;
            this.projectile.rotation = direction.ToRotation();
            this.leftTimer = this.projectile.timeLeft;
            this.fadeSpeed = 1f / (fadeTime * precision);
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
            this.colorSlashHit = slash;
            this.colorHead = head;
            this.colorTail = tail;
            return this;
        }

        public ShaderSlash SetColor(Color slash, Color slashTop, Color slashTrace, Color head, Color tail, Color hit)
        {
            this.colorSlash = slash;
            this.colorSlashTop = slashTop;
            this.colorSlashTrace = slashTrace;
            this.colorSlashHit = hit;
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

        public ShaderSlash SetDraw(BlendState style, float opacity = 1f, float traceChangeSpeed = -.2f, int slashStyle = 1, int weaponDir = 1, double fadeSpeedMult = .5f)
        {
            this.blendStyle = style;
            this.opacity = opacity;
            this.traceChangeSpeed = traceChangeSpeed;
            this.slashStyle = slashStyle;
            this.weaponDirection = weaponDir;
            this.fadeSpeedMult = fadeSpeedMult;
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
            projectile.noEnchantmentVisuals = true;
            this.CannotHitSameEnemyAgain = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            this.OnSpawn();
        }

        public override void UpdateAI()
        {
            projectile.velocity = Vector2.Zero;
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            if (projectile.friendly && bindToOwner) projectile.Center = owner.Center;
            if (!this.fading && projectile.timeLeft <= 1) this.fading = true;
            if (this.fading)
            {
                if (this.Alpha1 < 1f)
                {
                    projectile.friendly = false;
                    projectile.timeLeft = 2;
                    this.Alpha1 = (float)MyMath.Min(this.Alpha1 + this.fadeSpeed, 1d);
                }
            }
            if (!this.fading)
            {
                leftTimer--;
                if (this.attackInfo == null) return;
                Vector4 info = this.GetSlashInfo();
                float rot = info.W + projectile.rotation;
                Vector2 begin = projectile.position + (this.hitboxOffset + info.XY()).RotatedBy(rot);
                Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                float len = (end - begin).Length() / 16f;
                //以下代码逻辑来自tr源码
                //Terraria.ItemCheck_CutTiles(Item sItem, Microsoft.Xna.Framework.Rectangle itemRectangle, List< ushort > ignoreList)
                for (int i = 0; i <= len; i++)
                {
                    int x = (int)(MyMath.Lerp(begin, end, i / len) / 16f).X;
                    int y = (int)(MyMath.Lerp(begin, end, i / len) / 16f).Y;
                    
                    if (Main.tile[x, y] != null && Main.tileCut[(int)Main.tile[x, y].TileType])
                    {
                        WorldGen.KillTile(x, y);
                        if (CNC.IsMPClient) NetMessage.SendData(17, number2: ((float)x), number3: ((float)y));
                    }
                }
                //Terraria.Projectiles.UpdateEnchantmentVisuals()
                if (!projectile.npcProj)
                {
                    Vector2 position = MyMath.Lerp(begin, end, (float)CNC.Random.NextDouble() * .75f + .25f);
                    Vector2 size = Vector2.One * 16f;
                    //寒霜套
                    if (Main.player[projectile.owner].frostBurn && CNC.Random.NextBool(1.0 / 3))//CNC.Random.NextBool(1.0 / (2 * (1 + projectile.extraUpdates))))
                    {
                        int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 135, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, Scale: 2f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity *= 0.7f;
                        Main.dust[index].velocity.Y -= 0.5f;
                    }
                    //岩浆石
                    if (Main.player[projectile.owner].magmaStone && !CNC.Random.NextBool(1.0 / 3))
                    {
                        int index = Dust.NewDust(new Vector2(position.X - 4f, position.Y - 4f), (int)size.X + 8, (int)size.Y + 8, 6, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, Scale: 2f);
                        if (CNC.Random.NextBool(1.0 / 2)) Main.dust[index].scale = 1.5f;
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity.X *= 2f;
                        Main.dust[index].velocity.Y *= 2f;
                    }
                    switch (Main.player[projectile.owner].meleeEnchant)
                    {
                        case (byte)1://剧毒附魔
                            if (CNC.Random.NextBool(1.0 / 3))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 171, Alpha: 100);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].fadeIn = 1.5f;
                                Main.dust[index].velocity *= 0.25f;
                            }
                            if (CNC.Random.NextBool(1.0 / 3))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 171, Alpha: 100);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].fadeIn = 1.5f;
                                Main.dust[index].velocity *= 0.25f;
                            }
                            break;
                        case (byte)2://咒焰附魔
                            if (CNC.Random.NextBool(1.0 / 2))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 75, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, Scale: 2.5f);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].velocity *= 0.7f;
                                Main.dust[index].velocity.Y -= 0.5f;
                            }
                            break;
                        case (byte)3://火附魔
                            if (CNC.Random.NextBool(1.0 / 2))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 6, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, Scale: 2.5f);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].velocity *= 0.7f;
                                Main.dust[index].velocity.Y -= 0.5f;
                            }
                            break;
                        case (byte)4://金附魔
                            if (CNC.Random.NextBool(1.0 / 2))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 57, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, Scale: 1.1f);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].velocity.X /= 2f;
                                Main.dust[index].velocity.Y /= 2f;
                            }
                            break;
                        case (byte)5://灵液附魔
                            if (CNC.Random.NextBool(1.0 / 2))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 169, Alpha: 100);
                                Main.dust[index].velocity.X += (float)projectile.direction;
                                Main.dust[index].velocity.Y += 0.2f;
                                Main.dust[index].noGravity = true;
                            }
                            break;
                        case (byte)6://纳米附魔（原来用的和霜焰一个粒子么）
                            if (CNC.Random.NextBool(1.0 / 2))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 135, Alpha: 100);
                                Main.dust[index].velocity.X += (float)projectile.direction;
                                Main.dust[index].velocity.Y += 0.2f;
                                Main.dust[index].noGravity = true;
                            }
                            break;
                        case (byte)7://派对附魔
                            Vector2 velocity = projectile.velocity;
                            if ((double)velocity.Length() > 4.0) velocity *= 4f / velocity.Length();
                            if (CNC.Random.NextBool(1.0 / 20))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, Main.rand.Next(139, 143), velocity.X, velocity.Y, Scale: 1.2f);
                                Main.dust[index].velocity.X *= (float)(1.0 + (double)Main.rand.Next(-50, 51) * 0.00999999977648258);
                                Main.dust[index].velocity.Y *= (float)(1.0 + (double)Main.rand.Next(-50, 51) * 0.00999999977648258);
                                Main.dust[index].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                                Main.dust[index].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
                                Main.dust[index].scale *= (float)(1.0 + (double)Main.rand.Next(-30, 31) * 0.00999999977648258);
                            }
                            if (CNC.Random.NextBool(1.0 / 40))
                            {
                                int Type = Main.rand.Next(276, 283);
                                int index1 = Gore.NewGore(source, position, velocity, Type);
                                Main.gore[index1].velocity.X *= (float)(1.0 + (double)Main.rand.Next(-50, 51) * 0.00999999977648258);
                                Main.gore[index1].velocity.Y *= (float)(1.0 + (double)Main.rand.Next(-50, 51) * 0.00999999977648258);
                                Main.gore[index1].scale *= (float)(1.0 + (double)Main.rand.Next(-20, 21) * 0.00999999977648258);
                                Main.gore[index1].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                                Main.gore[index1].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
                            }
                            break;
                        case (byte)8://毒液附魔
                            if (CNC.Random.NextBool(1.0 / 4))
                            {
                                int index = Dust.NewDust(position, (int)size.X, (int)size.Y, 46, Alpha: 100);
                                Main.dust[index].noGravity = true;
                                Main.dust[index].fadeIn = 1.5f;
                                Main.dust[index].velocity *= 0.25f;
                            }
                            break;
                    }
                }
            }
            else if (timer++ % projectile.extraUpdates == 0) leftTimer = (float)((double)leftTimer - this.fadeSpeed * this.slashLength * this.fadeSpeedMult);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (this.attackInfo == null) return false;
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            Vector4 info = this.GetSlashInfo();
            float rot = info.W + projectile.rotation;
            Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot);
            Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
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
                int dir = (this.attackInfo[1].rotation + MyMath.pi2 > this.attackInfo[0].rotation + MyMath.pi2).ToDirectionInt(this.weaponDirection);
                var hit = CNCUtils.CreateSlashHit(projectile, Vector2.Zero);
                hit.SetDefaults(this.imageSlashHit, new Vector2(MyMath.phi - 1f, 1f) * .5f, target.Center, -10, rot * dir, 30);
                hit.SetColor(this.colorSlashHit.NewA(0), this.colorSlash.NewA(0), .1f);
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (CNC.IsMPServer || this.attackInfo == null) return false;
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            this.DrawSlash(Main.spriteBatch);
            this.DrawWeapon(Main.spriteBatch);
            return false;
        }

        private void DrawWeapon(SpriteBatch spriteBatch)
        {
            Texture2D image = this.imageWeapon ?? Terraria.GameContent.TextureAssets.Item[owner.HeldItem.type].Value;
            Vector4 info = this.GetSlashInfo();
            float rotation = info.W + projectile.rotation;
            Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rotation);
            Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rotation);
            Vector2 pos = owner.Center - Main.screenPosition;
            Vector2 origin = new Vector2(0, image.Height);
            int direction = projectile.direction * (this.attackInfo[0].rotation + MyMath.pi2 > this.attackInfo[1].rotation + MyMath.pi2).ToDirectionInt(1) * this.weaponDirection;
            direction *= (int)owner.gravDir;
            float alpha = 1f;
            float rotation2 = direction * MyMath.deg45;
            float scale = MyMath.Clamp(info.Z / image.Size().Length(), .75f, 1.75f);
            if (direction != 1) { origin.X = image.Width; rotation2 += MyMath.pi; }
            Main.EntitySpriteDraw(image, pos, null, Color.White * this.Alpha2 * alpha, rotation + rotation2, origin, scale, (SpriteEffects)(direction != 1).ToInt(1), 0);
        }

        private void DrawSlash(SpriteBatch spriteBatch)
        {
            if (this.slashLength == 0) return;
            List<VertexInfo> vertex = new List<VertexInfo>();
            if (this.slashStyle == 2)
            {
                double time = this.fading ? 1.0d : 1.0d - (double)projectile.timeLeft / this.timeLeft;
                float alpha = MyMath.Lerp(1f, 0f,  (float)(Math.Abs(time - .5f) * Math.Abs(time - .5f)) * 4f);
                float max = MyMath.Min(this.slashLength, this.attackInfo.Length);
                double dist = time > .5d ? ((1.0d - time) / max * 2.0d) : (time / max * 2.0d);
                for (int i = 0; i < max; i++)
                {
                    Vector4 info = this.GetSlashInfo(time - (i - max / 2.0d) * dist);
                    float rot = info.W + projectile.rotation;
                    Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot) * new Vector2(1f, owner.gravDir);
                    Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot) * new Vector2(1f, owner.gravDir);
                    float amount = i / max;
                    float amount2 = Math.Abs(i - max / 2.0f) / (max / 2.0f);
                    float w = MyMath.Lerp(1f, 0f, amount2);
                    Color color = Color.Lerp(this.colorHead, this.colorTail, (float)Math.Sqrt(amount2)) * w * Alpha2 * alpha;
                    vertex.Add(new VertexInfo(begin, color, new Vector3(amount, 0f, w)));
                    vertex.Add(new VertexInfo(end, color, new Vector3(amount, 1f, 0f)));
                }
            }
            else
            {
                float max = MyMath.Lerp(0f, this.slashLength, (float)(this.timeLeft - projectile.timeLeft) / (float)this.slashLength);
                for (int i = 0; i < max; i++)
                {
                    double time = 1.0d - ((double)leftTimer + i) / this.timeLeft;
                    Vector4 info = this.GetSlashInfo(time);
                    float rot = info.W + projectile.rotation;
                    Vector2 begin = projectile.Center + (this.hitboxOffset + info.XY()).RotatedBy(rot) * new Vector2(1f, owner.gravDir);
                    Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot) * new Vector2(1f, owner.gravDir);
                    //if (begin == Vector2.Zero || end == Vector2.Zero) break;
                    float amount = i / max;
                    float w = MyMath.Lerp(1f, 0f, amount);
                    Color color = Color.Lerp(this.colorHead, this.colorTail, amount) * w * Alpha2;
                    vertex.Add(new VertexInfo(begin, color, new Vector3(amount, 0f, w)));
                    vertex.Add(new VertexInfo(end, color, new Vector3(amount, 1f, 0f)));
                }
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
                this.ModifySlash(ref shader, ref vertex);
                shader.CurrentTechnique.Passes[0].Apply();
                
                Main.pixelShader.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex.ToArray(), 0, vertex.Count - 2);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (this.attackInfo == null)
            {
                writer.Write(-1);
                return;
            }
            writer.Write(attackInfo.Length);
            for(int i = 0; i < attackInfo.Length; i++)
            {
                writer.WritePackedVector2(attackInfo[i].offset);
                writer.Write(attackInfo[i].range);
                writer.Write(attackInfo[i].power);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if (reader.ReadInt32() == -1) return;
            attackInfo = new SlashInfo[reader.ReadInt32()];
            for (int i = 0; i < attackInfo.Length; i++)
            {
                attackInfo[i].offset = reader.ReadPackedVector2();
                attackInfo[i].range = reader.ReadSingle();
                attackInfo[i].power = reader.ReadSingle();
            }
        }

        public virtual void ModifySlash(ref Effect shader, ref List<VertexInfo> vertexs) { }

        /// <summary> 获取当前帧的斩击信息 <para> Vector4的XYZW分别对应：横坐标偏移，纵坐标偏移，攻击距离，攻击角度）</para> </summary>
        public virtual Vector4 GetSlashInfo() => this.GetSlashInfo(this.fading ? 1.0d : 1.0d - (double)projectile.timeLeft / this.timeLeft);
        /// <summary> 获取当前帧的斩击信息 <para> Vector4的XYZW分别对应：横坐标偏移，纵坐标偏移，攻击距离，攻击角度）</para> </summary>
        public virtual Vector4 GetSlashInfo(double amount)
        {
            if (this.attackInfo == null) return Vector4.Zero;
            int count = this.attackInfo.Length;
            float len = 1f / count;
            float amount2 = (float)amount % len / len;
            int index = MyMath.Clamp((int)(amount / len) + 1, 1, count - 1);
            float amount3 = MyMath.pow(amount2, this.attackInfo[index].power);
            Vector2 offset = MyMath.Lerp(this.attackInfo[index - 1].offset, this.attackInfo[index].offset, amount3);
            float range = MyMath.Lerp(this.attackInfo[index - 1].range, this.attackInfo[index].range, amount3);
            float rotation = Utils.AngleLerp(this.attackInfo[index - 1].rotation, this.attackInfo[index].rotation, amount3);
            return new Vector4(offset, range * this.scale, -rotation * projectile.direction * owner.gravDir);
        }
    }
}
