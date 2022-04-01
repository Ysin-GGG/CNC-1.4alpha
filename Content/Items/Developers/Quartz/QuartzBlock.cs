using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using System.Linq;
using System.Collections.Generic;
using CNC.Common.Utils;
using CNC.Common.Global.Items;

namespace CNC.Content.Items.Developers.Quartz
{
    [AutoloadEquip(EquipType.Head)]
    public class QuartzBlock : CNCItem
    {
        public override void ResetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.rare = 9;
            item.value = 100000;
            item.vanity = true;
            //Alakazam!
        }

        public override void UpdateEquip(Player P)
        {
            base.UpdateEquip(P);
            if (P.name == "Quartz" || P.name == "HHvs")
            {
                //P.statDefense = 9999;
                P.GetModPlayer<Common.Global.Players.InscriptionPlayer>().cooldownReductionMul = .1f;
                P.GetModPlayer<Common.Global.Players.InscriptionPlayer>().skillDamageAdd = 1f;
                P.wingTime = 1;
                P.statLifeMax2 *= 100;
                P.statManaMax2 *= 100;
                //P.manaCost *= 0.1f;
                P.noKnockback = true;
                P.noFallDmg = true;
                if (P.controlDown && P.controlJump)
                {
                    P.velocity.Y = 0.1f;
                    P.position.Y = P.oldPosition.Y;
                    if (P.controlLeft) { P.velocity.X -= 0.5f; }
                    if (P.controlRight) { P.velocity.X += 0.5f; }
                    { P.velocity.X *= .99f; }
                }
                if (P.controlDown && P.velocity.Y > 0)
                { P.velocity.Y += 1f; }
                if (P.controlUp && P.controlJump)
                { P.velocity.Y -= 1f; P.velocity.Y *= .99f; }
                if (P.controlUp && !P.controlJump)
                { P.velocity.Y *= .9f; }
                for (int i = 0; i < Main.maxBuffTypes; i++)
                {
                    if (i == 21) continue;
                    if (i == 86) continue;
                    //if (i == 194) continue;
                    if (i == 199) continue;
                    if (Main.debuff[i]) P.buffImmune[i] = true;
                }
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Player P = Main.player[Const.me];
            Color c = Color.White;
            if (P.controlTorch) { c = Main.DiscoColor; }
            Texture2D texture = TextureAssets.Item[item.type].Value;
            Vector2 position = item.position - Main.screenPosition + new Vector2(item.width / 2, item.height - texture.Height * 0.5f + 2f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
                spriteBatch.Draw(texture, position + offsetPositon, null, c, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player P = Main.player[Const.me];
            Color c = Color.White;
            if (P.controlTorch) { c = Main.DiscoColor; }
            Texture2D texture = TextureAssets.Item[item.type].Value;
            for (int i = 0; i < 4; i++)
            {
                Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
                spriteBatch.Draw(texture, position + offsetPositon, null, c, 0, origin, scale, SpriteEffects.None, 0f);
            }
            if (P.name == "Quartz" || P.name == "HHvs") { item.rare = -12; }
            else { item.rare = 9; }
            return true;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            Color c = Color.White;
            //c = new Color(MyMethod.ColorFlash(0, 255, 255), MyMethod.ColorFlash(0, 255, 255), MyMethod.ColorFlash(0, 255, 255));
            if (drawPlayer.controlTorch) c = Main.DiscoColor;
			color = drawPlayer.GetImmuneAlphaPure(c, shadow);
		}

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 pos = Main.ScreenSize.ToVector2() / 2;
            Vector2[] vertex = {
                    pos + new Vector2(-50,-50),
                    pos + new Vector2(+50,-50),
                    pos + new Vector2(+50,+50),
                    pos + new Vector2(-50,+50),
                };
            Texture2D image1 = ModContent.Request<Texture2D>("CNC/Assets/Effects/Slash1").Value;
            Texture2D image2 = ModContent.Request<Texture2D>("CNC/Assets/Effects/Slash2").Value;
            Texture2D image3 = ModContent.Request<Texture2D>("CNC/Content/Projectiles/IceTooth/IceAura").Value;
            Texture2D image4 = ModContent.Request<Texture2D>("CNC/Content/Projectiles/IceTooth/IceCrack").Value;
            Texture2D image5 = ModContent.Request<Texture2D>("CNC/Content/Projectiles/IceTooth/FrostWave").Value;
            Color color = new Color(178, 214, 231, 0); color.A = 0;
            //spriteBatch.Draw(image3, new Rectangle((int)(pos.X - 250), (int)(pos.Y - 250), 500, 500), color);
            //spriteBatch.Draw(image4, new Rectangle((int)(pos.X - 160), (int)(pos.Y - 160), 320, 320), color);
            //Drawing.DrawQuadrilateral(spriteBatch, image1, vertex, new Rectangle(0, 0, 256, 244), color, MyMath.deg45);
            float num131 = 0.0f;
            /*
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, null, Main.Transform);
            Vector2 vector2_9 = Main.ScreenSize.ToVector2() / 2f - new Vector2(300f, 310f);
            Terraria.Graphics.Effects.Filters.Scene["Nebula"].GetShader().UseIntensity(1f + num131).UseProgress(0.0f);
            DrawData drawData2 = new DrawData(Main.Assets.Request<Texture2D>("Images/Misc/Perlin", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, vector2_9 + new Vector2(300f, 300f), new Rectangle(0, 0, 600, 600), CNCID.Colors.Expert, 0, new Vector2(300f, 300f), 1, 0, 0);
            GameShaders.Misc["ForceField"].UseColor(new Vector3(1f));
            GameShaders.Misc["ForceField"].Apply(new DrawData?(drawData2));
            drawData2.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            */
        }
    }
}