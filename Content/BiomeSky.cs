using Terraria;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content
{
    public abstract class BiomeSky : CustomSky
    {
        public bool active = false;
        public bool visible = false;
        public float intensity = 1f;
        public float cloudAlpha = 0f;
        public float opacity = 0f;
        public float opacityIn = .05f;
        public float opacityOut = .05f;
        public Color skyColor = Color.White;
        public Color? tileColor  = null;
        public Texture2D skyImage = null;

        public sealed override void OnLoad()
        {
            this.active = false;
            this.visible = false;
            this.intensity = 1f;
            this.cloudAlpha = 0f;
            this.opacity = 0f;
            this.opacityIn = .05f;
            this.opacityOut = .05f;
            this.skyColor = Color.White;
            this.tileColor = null;
            this.skyImage = null;
            this.SetDefaults();
        }

        public sealed override void Reset()
        {
            this.active = false;
            this.visible = false;
            this.opacity = 0f;
        }

        public sealed override void Activate(Vector2 position, params object[] args)
        {
            this.active = true;
            this.visible = true;
            this.opacity = 0f;
        }

        public sealed override void Deactivate(params object[] args)
        {
            this.active = false;
            this.visible = true;
            this.opacity = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.active && this.opacity < 1f) this.opacity = MyMath.Min(this.opacity + this.opacityIn, 1f);
            if (!this.active && this.opacity > 0f) this.opacity = MyMath.Max(this.opacity - this.opacityOut, 0f);
            if (this.active && this.opacity >= 0f) this.visible = true;
            if (!this.active && this.opacity <= 0f) this.visible = false;
        }
        
        public void Update()
        {
            if (this.active && this.opacity < 1f) this.opacity = MyMath.Min(this.opacity + this.opacityIn, 1f);
            if (!this.active && this.opacity > 0f) this.opacity = MyMath.Max(this.opacity - this.opacityOut, 0f);
            if (this.active && this.opacity >= 0f) this.visible = true;
            if (!this.active && this.opacity <= 0f) this.visible = false;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (this.visible && maxDepth >= 3E+38f && minDepth < 3E+38f)
            {
                spriteBatch.Draw(this.skyImage ?? Terraria.GameContent.TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), this.skyColor * this.opacity * this.intensity);
            }
        }

        public sealed override bool IsActive() => this.active || this.opacity > 0f;
        public sealed override bool IsVisible() => this.visible;
        public override float GetCloudAlpha() => MyMath.Lerp(base.GetCloudAlpha(), this.cloudAlpha, this.opacity);
        public override Color OnTileColor(Color inColor) => (this.visible && this.tileColor.HasValue) ? Color.Lerp(inColor, this.tileColor.Value, this.opacity * this.intensity) : inColor;

        public virtual void SetDefaults() { }
    }
}