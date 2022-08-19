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
    public class GenericSlash : CNCProjectile
    {
        public sealed override int countFrame => 1;

        public Texture2D image;
        public Texture2D imageTrail;
        public Texture2D imageTrace;
        public Texture2D imageHeatmap;
        public int totalFrame => tickPerFrame.Length;
        public int[] tickPerFrame;
        public Color imageColor;
        public Color lightColor;
        public float light;
        public Vector2 scale;
        public SlashInfo[] attackInfo;
        public Vector2 direction;
        public Vector2 imageOffset;
        public int[] afterImageData;
        /// <summary> 弹幕追随玩家 </summary>
        public bool bindOwner;
        /// <summary> 用粒子显现大体的打击框 </summary>
        public int dustToShowHitbox;
        public int fadeTime;
        public int fadeInterval;
        public float fadeAlphaMult;
        private int timer;

        /// <summary> 设定常规项 </summary>
        /// <param name="image">斩击贴图</param>
        /// <param name="position">初始坐标</param>
        /// <param name="scale">贴图尺寸比例</param>
        /// <param name="direction">斩击朝向（一般填初速度）</param>
        /// <param name="frame">斩击贴图总帧数</param>
        /// <param name="tickPerFrame">斩击贴图每帧时长（总时长越长，打击框越精确，同时攻速越慢）</param>
        /// <param name="precision">精度（精度越高，打击框越精确，同时越消耗性能）</param>
        public GenericSlash SetDefaults(Texture2D image, Vector2 position, Vector2 scale, Vector2 direction, int frame, int tickPerFrame, int precision = 1)
        {
            this.image = image;
            this.scale = scale;
            this.direction = direction;
            this.tickPerFrame = new int[frame];
            for (int i = 0; i < frame; i++) this.tickPerFrame[i] = tickPerFrame;
            this.projectile.Resize(image == null ? 1 : image.Width, image == null ? 1 : image.Height / frame);
            this.projectile.Center = position;
            this.projectile.timeLeft = frame * tickPerFrame * precision;
            this.projectile.extraUpdates = precision - 1;
            return this;
        }

        /// <summary> 设定常规项 </summary>
        /// <param name="image">斩击贴图</param>
        /// <param name="position">初始坐标</param>
        /// <param name="scale">贴图尺寸比例</param>
        /// <param name="direction">斩击朝向（一般填初速度）</param>
        /// <param name="frameTick">斩击贴图每帧时长（总时长越长，打击范围越精确，同时攻速越慢）</param>
        /// <param name="precision">精度（精度越高，打击范围越精确，同时越消耗性能）</param>
        public GenericSlash SetDefaults(Texture2D image, Vector2 position, Vector2 scale, Vector2 direction, int[] frameTick, int precision = 1)
        {
            this.image = image;
            this.scale = scale;
            this.direction = direction;
            this.tickPerFrame = frameTick;
            this.projectile.Resize(image == null ? 1 : image.Width, image == null ? 1 : image.Height / frameTick.Length);
            this.projectile.Center = position;
            this.projectile.timeLeft = MyMath.Sum(frameTick) * precision;
            this.projectile.extraUpdates = precision - 1;
            return this;
        }
        /// <summary> 设定打击判定 <para>数组长度不宜大于这个值：总帧数 * 精度 + 1</para> </summary>
        /// <param name="info">打击判定信息</param>
        public GenericSlash SetAttackInfo(SlashInfo[] info)
        {
            this.attackInfo = info;
            return this;
        }
        /// <summary> 设定打击判定 <para>数组长度不宜大于这个值：总帧数 * 精度 + 1</para> <para>四个数组的长度相同为宜</para> </summary>
        /// <param name="offset">坐标偏移</param>
        /// <param name="range">攻击距离</param>
        /// <param name="rotation">攻击角度</param>
        /// <param name="power">插值变化的幂（通常为1）</param>
        public GenericSlash SetAttackInfo(Vector2[] offset, float[] range, float[] rotation, float[] power)
        {
            int len = MyMath.Min(offset.Length, range.Length, rotation.Length, power.Length);
            this.attackInfo = new SlashInfo[len];
            for (int i = 0; i < len; i++)
            {
                this.attackInfo[i].offset = offset[i];
                this.attackInfo[i].range = range[i];
                this.attackInfo[i].rotation = rotation[i];
                this.attackInfo[i].power = power[i];
            }
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
        public GenericSlash SetDamage(int style, DamageClass type, int damage, float knockback, int crit, float add = 0f, float mult = 1f)
            => this.SetDamage((ProjDamageStyle)style, type, damage, knockback, crit, add, mult);
        /// <summary> 设定伤害项 </summary>
        /// <param name="style">友好/敌对型</param>
        /// <param name="type">伤害类型</param>
        /// <param name="damage">伤害值</param>
        /// <param name="knockback">击退</param>
        /// <param name="crit">暴击率</param>
        /// <param name="add">追加伤害</param>
        /// <param name="mult">伤害倍率</param>
        public GenericSlash SetDamage(ProjDamageStyle style, DamageClass type, int damage, float knockback, int crit, float add = 0f, float mult = 1f)
        {
            this.DamageStyle = style;
            this.projectile.DamageType = type;
            this.projectile.damage = (int)((damage + add) * mult);
            this.projectile.knockBack = knockback;
            this.projectile.CritChance = crit;
            return this;
        }
        /// <summary> 设定色彩项 </summary>
        /// <param name="imageColor">图像颜色（为Color.Transparent时自动获取环境色）</param>
        /// <param name="lightColor">光源颜色</param>
        /// <param name="light">光源亮度</param>
        public GenericSlash SetColor(Color imageColor, Color lightColor, float light = 0f)
        {
            this.imageColor = imageColor;
            this.lightColor = lightColor;
            this.light = MyMath.Max(0f, light);
            return this;
        }

        public GenericSlash SetAfterImages(int count, int interval = 1, float alphaMult = .75f)
        {
            this.afterImageData = new int[count];
            for (int i = 0; i < count; i++) afterImageData[i] = frame;
            this.fadeTime = -count * interval;
            this.fadeInterval = interval;
            this.fadeAlphaMult = alphaMult;
            return this;
        }

        /// <summary> 获取当前帧的斩击信息 <para> Vector4的XYZW分别对应：横坐标偏移，纵坐标偏移，攻击距离，攻击角度）</para> </summary>
        public virtual Vector4 GetSlashInfo()
        {
            int count = this.attackInfo.Length;
            float len = 1f / count;
            float amount = 1 - (float)projectile.timeLeft / (MyMath.Sum(this.tickPerFrame) * projectile.extraUpdates);
            float amount2 = amount % len / len;
            if (projectile.direction == 1)
            {
                int index = MyMath.Clamp((int)(amount / len) + 1, 1, count - 1);
                float amount3 = MyMath.pow(amount2, this.attackInfo[index].power);
                Vector2 offset = MyMath.Lerp(this.attackInfo[index - 1].offset, this.attackInfo[index].offset, amount3);
                float range = MyMath.Lerp(this.attackInfo[index - 1].range, this.attackInfo[index].range, amount3);
                float rotation = MyMath.Lerp(this.attackInfo[index - 1].rotation, this.attackInfo[index].rotation, amount3);
                return new Vector4(offset, range, -rotation);
            }
            else
            {
                int index = MyMath.Clamp((int)(amount / len) + 1, 1, count - 1);
                float amount3 = MyMath.pow(amount2, this.attackInfo[index].power);
                Vector2 offset = MyMath.Lerp(this.attackInfo[index - 1].offset, this.attackInfo[index].offset, amount3);
                float range = MyMath.Lerp(this.attackInfo[index - 1].range, this.attackInfo[index].range, amount3);
                float rotation = MyMath.Lerp(this.attackInfo[index - 1].rotation, this.attackInfo[index].rotation, amount3);
                return new Vector4(offset, range, rotation);
                /*
                int index = MyMath.Clamp(count - (int)(amount / len) - 1, 1, count - 1);
                float amount3 = MyMath.pow(amount2, this.attackInfo[index].power);
                Vector2 offset = MyMath.Lerp(this.attackInfo[index].offset, this.attackInfo[index - 1].offset, amount3);
                float range = MyMath.Lerp(this.attackInfo[index].range, this.attackInfo[index - 1].range, amount3);
                float rotation = MyMath.Lerp(this.attackInfo[index].rotation, this.attackInfo[index - 1].rotation, amount3);
                return new Vector4(offset, range, -rotation);
                */
            }
        }

        public override void ResetDefaults()
        {
            projectile.timeLeft = 5;
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
            projectile.velocity = Vector2.Zero;
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            if (this.bindOwner) projectile.Center = owner.Center;
            if (this.dustToShowHitbox >= 0 && this.fadeTime <= 0)
            {
                Vector4 info = this.GetSlashInfo();
                for (int i = 10; i <= 10; i++)
                {
                    float rot = info.W + projectile.rotation;
                    Vector2 begin = projectile.Center + info.XY().RotatedBy(rot);
                    Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
                    CNCUtils.NewDust(MyMath.Lerp(begin, end, i / 10f), dustToShowHitbox, 1.5f, Vector2.Normalize(end - begin) * 1f).noGravity = true;
                }
            }
            if (afterImageData.Length > 0 && timer % this.fadeInterval == 0)
            {
                for (int i = afterImageData.Length - 1; i > 0; i--)
                    afterImageData[i] = afterImageData[i - 1];
                afterImageData[0] = frame;
            }
            if (frame < this.totalFrame)
            {
                while (this.tickPerFrame[this.frame] <= 0)//跳过0帧的动画
                {
                    this.frameTick = 0;
                    this.frame++;
                    if (this.frame >= this.totalFrame) goto jump;
                }
                if (++this.frameTick > this.tickPerFrame[this.frame] * projectile.extraUpdates)
                {
                    this.frameTick = 0;
                    this.frame++;
                }
            }
            else if (this.fadeTime < 0) this.fadeTime *= -1;
            jump :
            if (this.fadeTime == 1) projectile.Kill();
            else if (this.fadeTime > 0) projectile.timeLeft = this.fadeTime--;
            if (this.lightColor != null && this.lightColor != Color.Transparent && this.light > 0f)
                CNCUtils.AddLight(this.projectile.Center, this.lightColor, this.light);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (this.fadeTime > 0) return false;
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            Vector4 info = this.GetSlashInfo();
            float rot = info.W + projectile.rotation;
            Vector2 begin = projectile.Center + info.XY().RotatedBy(rot);
            Vector2 end = begin + new Vector2(info.Z, 0).RotatedBy(rot);
            projHitbox.X = (int)MyMath.Min(begin.X, end.X);
            projHitbox.Y = (int)MyMath.Min(begin.Y, end.Y);
            projHitbox.Width = (int)Math.Abs(end.X - begin.X);
            projHitbox.Height = (int)Math.Abs(end.Y - begin.Y);
            bool inRange = false;
            bool noBarrier = true;
            bool noBarrier2 = true;
            int size = 4;
            float max = begin.Distance(end) / size;
            for (int i = 0; i <= max; i++)
            {
                Vector2 point = MyMath.Lerp(begin, end, i / max);
                Vector2 point2 = MyMath.Lerp(owner.Center, end, i / max);
                if (!inRange) inRange = targetHitbox.Contains(point.ToPoint());
                if (noBarrier) noBarrier = !Collision.SolidTiles(point, size, size);
                if (noBarrier2) noBarrier2 = !Collision.SolidTiles(point2, size, size);
                if (inRange && noBarrier && noBarrier2) return true;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            projectile.direction = (direction.X >= 0).ToDirectionInt(1);
            Rectangle source = new Rectangle(0, frame * projectile.height, projectile.width, projectile.height);
            Vector2 offset = new Vector2(projectile.direction * imageOffset.X, imageOffset.Y).RotatedBy(projectile.rotation);
            Vector2 pos = projectile.Center - Main.screenPosition + imageOffset.RotatedBy(projectile.rotation);
            Vector2 origin = projectile.Center - projectile.position ;
            for (int i = afterImageData.Length - 1; i >= 0; i--)
            {
                if (afterImageData[i] >= totalFrame) continue;
                float alpha = (1 - (float)i / afterImageData.Length) / (projectile.extraUpdates + 1);
                Rectangle source2 = new Rectangle(0, afterImageData[i] * projectile.height, projectile.width, projectile.height);
                Main.EntitySpriteDraw(this.image, pos, source2, this.imageColor * this.fadeAlphaMult * alpha, projectile.rotation, origin, this.scale, projectile.direction == 1 ? 0 : SpriteEffects.FlipVertically, 0);
            }
            if (this.fadeTime <= 0) Main.EntitySpriteDraw(this.image, pos, source, this.imageColor, projectile.rotation, origin, this.scale, projectile.direction == 1 ? 0 : SpriteEffects.FlipVertically, 0);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public struct SlashInfo
        {
            /// <summary> 
            /// 坐标偏移
            /// <para> 坐标系以TR为准 </para>
            /// </summary>
            public Vector2 offset;
            /// <summary> 攻击距离  </summary>
            public float range;
            /// <summary> 攻击角度  </summary>
            public float rotation;
            /// <summary> 
            /// 插值（Lerp）变化的幂 
            /// <para> 取1时，为线性插值 </para>
            /// </summary>
            public float power;

            public SlashInfo(Vector2 offset, float range, float rotation, float power = 1f)
            {
                this.offset = offset;
                this.range = range;
                this.rotation = rotation;
                this.power = power;
            }
        }
    }
}
