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
    public class WarningLine : CNCProjectile
    {
        public Color color { get => CNCUtils.FlashColor((uint)projectile.ai[1], false); set => projectile.ai[1] = (value.R << 24) + (value.G << 16) + (value.B << 8) + (value.A << 0); }
        public ref float fade => ref projectile.ai[0];

        public override void ResetDefaults()
        {
            projectile.timeLeft = 25;
            projectile.damage = 0;
            projectile.width = 2;
            projectile.height = 2;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.scale = 3f;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void UpdateAI()
        {
            if (projectile.timeLeft < 2)
            {
                if (Alpha2 > 0f) { Alpha2 -= this.fade; projectile.timeLeft = 2; }
            }
            else Alpha2 = MyMath.Min(Alpha2 + this.fade, .5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(Texture2, projectile.Center - Main.screenPosition, null, this.color * this.Alpha2, projectile.rotation, Texture2.Left(), new Vector2(projectile.scale, 1f), (SpriteEffects)0, 0);
            Main.EntitySpriteDraw(Texture2, projectile.Center - Main.screenPosition, null, this.color * this.Alpha2, projectile.rotation, Texture2.Right(), new Vector2(projectile.scale, 1f), (SpriteEffects)1, 0);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
