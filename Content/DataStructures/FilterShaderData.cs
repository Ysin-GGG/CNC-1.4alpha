
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using CNC.Common.Utils;

namespace CNC.Content.DataStructures
{
    public class FilterShaderData : ScreenShaderData
    {
        public FilterShaderData(Ref<Effect> shader, string passName) : base(shader, passName) { }

        public override void Apply() => base.Apply();

        public FilterShaderData UseFloat(string name, float value)
        {
            this.Shader.Parameters[name].SetValue(value);
            return this;
        }

        public FilterShaderData UseFloat(string name, float x, float y)
            => this.UseFloat(name, new Vector2(x, y));

        public FilterShaderData UseFloat(string name, float x, float y, float z)
            => this.UseFloat(name, new Vector3(x, y, z));

        public FilterShaderData UseFloat(string name, float x, float y, float z, float w)
            => this.UseFloat(name, new Vector4(x, y, z, w));

        public FilterShaderData UseFloat(string name, Vector2 value)
        {
            this.Shader.Parameters[name].SetValue(value);
            return this;
        }

        public FilterShaderData UseFloat(string name, Vector3 value)
        {
            this.Shader.Parameters[name].SetValue(value);
            return this;
        }

        public FilterShaderData UseFloat(string name, Vector4 value)
        {
            this.Shader.Parameters[name].SetValue(value);
            return this;
        }

        public FilterShaderData UsePosition(Vector2 position)
        {
            Vector2 offScreenRange = Main.offScreenRange * Vector2.One;
            Vector2 realScreenSize = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
            Vector2 screenCenter = Main.ScreenSize.ToVector2() / 2f;
            Vector2 realScreenPos = Main.screenPosition + screenCenter * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
            this.Shader.Parameters["uPosition"].SetValue((position - screenCenter) / realScreenSize);
            return this;
        }

        public new FilterShaderData UseIntensity(float value)
        {
            base.UseIntensity(value);
            return this;
        }
    }
}