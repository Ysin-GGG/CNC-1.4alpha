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
    public class GenericExplosion : CNCProjectile
    {
        public sealed override int countFrame => 1;

        public Texture2D image;
        public int totalFrame => tickPerFrame.Length;
        public int[] tickPerFrame;
        public Color imageColor;
        public Color lightColor;
        public float light;
        public float scale;
        public float scaleAdd;
        public float scaleMult;
        public float alpha;
        public float alphaAdd;
        public float alphaMult;
        public float rotation;
        public float rotationAdd;
        public float rotationMult;
        public enum DrawLayer
        {
            behindNPCsAndTiles,
            behindNPCs,
            behindProjectiles,
            overPlayers,
            overWiresUI,
        }

        /// <summary> 设定常规项 </summary>
        /// <param name="image">斩击贴图</param>
        /// <param name="position">初始坐标</param>
        /// <param name="size">攻击范围</param>
        /// <param name="frame">爆炸贴图总帧数</param>
        /// <param name="tickPerFrame">爆炸贴图每帧时长</param>
        public GenericExplosion SetDefaults(Texture2D image, Vector2 position, Vector2 size, int frame, int tickPerFrame)
        {
            this.image = image;
            this.projectile.Size = size;
            this.projectile.Center = position;
            this.tickPerFrame = new int[frame];
            for (int i = 0; i < frame; i++) this.tickPerFrame[i] = tickPerFrame;
            return this;
        }

        /// <summary> 设定常规项 </summary>
        /// <param name="image">斩击贴图</param>
        /// <param name="position">初始坐标</param>
        /// <param name="size">攻击范围</param>
        /// <param name="frameTick">爆炸贴图每帧时长</param>
        public GenericExplosion SetDefaults(Texture2D image, Vector2 position, Vector2 size, int[] frameTick)
        {
            this.image = image;
            this.projectile.Size = size;
            this.projectile.Center = position;
            this.tickPerFrame = frameTick;
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
        public GenericExplosion SetDamage(int style, DamageClass type, int damage, float knockback, int crit, float add = 0f, float mult = 1f)
            => this.SetDamage((ProjDamageStyle)style, type, damage, knockback, crit, add, mult);
        /// <summary> 设定伤害项 </summary>
        /// <param name="style">友好/敌对型</param>
        /// <param name="type">伤害类型</param>
        /// <param name="damage">伤害值</param>
        /// <param name="knockback">击退</param>
        /// <param name="crit">暴击率</param>
        /// <param name="add">追加伤害</param>
        /// <param name="mult">伤害倍率</param>
        public GenericExplosion SetDamage(ProjDamageStyle style, DamageClass type, int damage, float knockback, int crit, float add = 0f, float mult = 1f)
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
        public GenericExplosion SetColor(Color imageColor, Color lightColor, float light = 0f)
        {
            this.imageColor = imageColor;
            this.lightColor = lightColor;
            this.light = MyMath.Max(0f, light);
            return this;
        }

        /// <summary> 设定透明项 </summary>
        /// <param name="alpha">初始值</param>
        /// <param name="add">每帧增加值</param>
        /// <param name="mult">每帧倍数值</param>
        /// <returns></returns>
        public GenericExplosion SetAlpha(float alpha = 0f, float add = 0f, float mult = 1f)
        {
            this.alpha = alpha;
            this.alphaAdd = add;
            this.alphaMult = mult;
            return this;
        }

        /// <summary> 设定比例项 </summary>
        /// <param name="scale">初始值</param>
        /// <param name="add">每帧增加值</param>
        /// <param name="mult">每帧倍数值</param>
        public GenericExplosion SetScale(float scale = 1f, float add = 0f, float mult = 1f)
        {
            this.scale = scale;
            this.scaleAdd = add;
            this.scaleMult = mult;
            return this;
        }

        /// <summary> 设定旋转项 </summary>
        /// <param name="rotation">初始值</param>
        /// <param name="add">每帧增加值</param>
        /// <param name="mult">每帧倍数值</param>
        public GenericExplosion SetRotation(float rotation = 0f, float add = 0f, float mult = 1f)
        {
            this.rotation = rotation;
            this.rotationAdd = add;
            this.rotationMult = mult;
            return this;
        }

        public override void ResetDefaults()
        {
            projectile.damage = 0;
            projectile.knockBack = 0f;
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 2;
            projectile.alpha = 0;
            projectile.light = 0f;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void UpdateAI()
        {
            if (++this.frameTick > this.tickPerFrame[this.frame])
            {
                this.frameTick = 0;
                this.frame++;
            }
            if (this.frame < this.totalFrame) projectile.timeLeft += 2;
            else projectile.Kill();
            if (this.lightColor != null && this.lightColor != Color.Transparent && this.light > 0f)
                CNCUtils.AddLight(this.projectile.Center, this.lightColor, this.light);
        }

        public override void PostAI()
        {
            this.alpha = (this.alpha + this.alphaAdd) * this.alphaMult;
            this.scale = (this.scale + this.scaleAdd) * this.scaleMult;
            this.rotation = (this.rotation + this.rotationAdd) * this.rotationMult;
            if (projectile.timeLeft > 6) projectile.friendly = projectile.hostile = false;
        }

        public override void PostDraw(Color lightColor)
        {
            if (image != null)
            {
                int height = this.image.Height / this.totalFrame;
                Rectangle source = new Rectangle(0, this.projectile.frame * height, image.Width, height);
                Vector2 origin = new Vector2(image.Width, height) / 2f;
                Color color = this.imageColor == Color.Transparent ? lightColor : this.imageColor;
                Main.EntitySpriteDraw(this.image, projectile.Center - Main.screenPosition, source, color * this.alpha, this.rotation, origin, this.scale, 0, 0);
            }
        }
        public override void HitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.direction == 1) hitDirection = -(target.position.X > this.projectile.position.X).ToDirectionInt(1);
            if (target.direction == -1) hitDirection = (target.position.X > this.projectile.position.X).ToDirectionInt(1);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
