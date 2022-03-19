using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
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
                P.GetDamage(DamageClass.Summon) += 99;
                P.wingTime = 1;
                P.statLifeMax2 *= 100;
                P.statManaMax2 *= 100;
                P.manaCost *= 0.1f;
                P.noKnockback = true;
                P.noFallDmg = true;
                //P.GetModPlayer<GPlayer>().CDReduction[0] = 0.1f;
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

        public override void PostUpdate()
        {
            base.PostUpdate();
            Main.curMusic = Main.newMusic = Terraria.ID.MusicID.EmpressOfLight;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            Color c = Color.White;
            //c = new Color(MyMethod.ColorFlash(0, 255, 255), MyMethod.ColorFlash(0, 255, 255), MyMethod.ColorFlash(0, 255, 255));
            if (drawPlayer.controlTorch)  c = Main.DiscoColor;
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
            Color color = CNCID.Colors.Expert; color.A = 0;
            //Drawing.DrawQuadrilateral(spriteBatch, image1, vertex, new Rectangle(0, 0, 256, 244), color, MyMath.deg45);
        }
    }
}