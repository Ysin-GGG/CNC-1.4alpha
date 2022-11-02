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
    public class GenericSlashHit : CNCProjectile
    {
        public sealed override int countFrame => 1;
        
        public Texture2D image;
        public Color imageColor;
        public Color lightColor;
        public float light;
        public Vector2 scale;
        private Vector2 scale2;
        public float alpha;
        private float halfTime;
        private Vector2 begin;
        private Vector2 end;

        /// <summary> 设定常规项 </summary>
        /// <param name="image">斩击贴图</param>
        /// <param name="position">初始坐标</param>
        /// <param name="scale">贴图尺寸比例</param>
        /// <param name="direction">斩击朝向（一般填初速度）</param>
        /// <param name="frame">斩击贴图总帧数</param>
        /// <param name="tickPerFrame">斩击贴图每帧时长（总时长越长，打击框越精确，同时攻速越慢）</param>
        /// <param name="precision">精度（精度越高，打击框越精确，同时越消耗性能）</param>
        public GenericSlashHit SetDefaults(Texture2D image, Vector2 scale, Vector2 position, Vector2 velocity, int time = 20)
        {
            this.halfTime = time / 2f;
            this.image = image;
            this.scale = scale;
            this.begin = position - velocity * halfTime;
            this.end = position + velocity * halfTime;
            this.projectile.Resize(image == null ? 1 : image.Width, image == null ? 1 : image.Height);
            this.projectile.Center = begin;
            this.projectile.timeLeft = time;
            this.projectile.velocity = velocity;
            this.projectile.rotation = velocity.ToRotation();
            return this;
        }

        public GenericSlashHit SetDefaults(Texture2D image, Vector2 scale, Vector2 position, float velocity, float rotation, int time = 20)
            => SetDefaults(image, scale, position, new Vector2(velocity, 0).RotatedBy(rotation), time);

        /// <summary> 设定色彩项 </summary>
        /// <param name="imageColor">图像颜色（为Color.Transparent时自动获取环境色）</param>
        /// <param name="lightColor">光源颜色</param>
        /// <param name="light">光源亮度</param>
        public GenericSlashHit SetColor(Color imageColor, Color lightColor, float light = 0f)
        {
            this.imageColor = imageColor;
            this.lightColor = lightColor;
            this.light = MyMath.Max(0f, light);
            return this;
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
            projectile.extraUpdates = 1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.netUpdate2 = true;
        }

        public override void UpdateAI()
        {
            float factor = 1 - Math.Abs(projectile.timeLeft - halfTime) / halfTime;
            if (this.lightColor != null && this.lightColor != Color.Transparent && this.light > 0f)
                CNCUtils.AddLight(projectile.Center, this.lightColor, this.light);
            this.alpha = factor * .5f + .5f;
            this.scale2 = this.scale * factor;
            //this.scale2.Y = this.scale.Y;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = this.imageColor;
            Vector2 pos = projectile.Center - Main.screenPosition;//MyMath.Lerp(end, begin, projectile.timeLeft / halfTime / 2f) - Main.screenPosition;
            Vector2 origin = this.image.Size() / 2f;
            Main.EntitySpriteDraw(image, pos, null, lightColor * this.alpha, projectile.rotation, origin, this.scale2, 0, 0);
            return false;
        }
    }
}
