using System;
using System.Collections.Generic;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Players;

using static CNC.Common.Utils.CNCID.Clazz;

namespace CNC.Content.UI.Clone
{
    public class CloneGUI : UIState
    {
        private const int max = 20;
        private const int max2 = 8;
        private Player player => Main.LocalPlayer;
        private AllPlayer allPlayer => player.GetModPlayer<AllPlayer>();
        private ClassesSettings modPlayer => player.GetModPlayer<ClassesSettings>();
        private int starCount;
        private int heartCount;
        private int heart2Count;
        private int init = 0;
        private int textTime = 0;
        private bool mouseRight = false;
        private bool mouseRightRelease = false;
        public static bool Visible;

        public UIImage rightHP;
        public UIImage rightMP;
        public UIImage rightExtra;
        public UIImage[] middleHP;
        public UIImage[] middleMP;
        public UIImage[] middleExtra;
        public UIImage[] leftPanel;
        public BarFiller fillHP;
        public BarFiller fillMP;
        public BarFiller fillExtra;
        public List<UIImage> mp;
        public List<UIImage> hp;
        public List<UIImage> hp2;
        public BuffShow[] buffs;
        public BuffShow[] states;

        public override string Name => "Clone";

        protected Texture2D[] img => new Texture2D[]
        {
            GetImage("HP_Panel_Right"),
            GetImage("HP_Panel_Middle"),
            GetImage("MP_Panel_Right"),
            GetImage("MP_Panel_Middle"),
            GetImage("Extra_Panel_Right"),
            GetImage("Extra_Panel_Middle"),
            GetImage("HP_Fill"),
            GetImage("HP_Fill_Honey"),
            GetImage("MP_Fill"),
            GetImage("Panel_Left"),
            GetImage("Rage_Fill"),
            GetImage("Power_Fill"),
            GetImage("Durability_Fill"),
            GetImage("Blessing_Fill"),
            GetImage("Sanity_Fill"),
            GetImage("Phantom_Fill"),
        };

        public override void Initialize()
        {
            CanDrag = false;
            CanOutScreen = true;
            Size = Vector2.One;
            Offset = new Vector2(0);

            leftPanel = new UIImage[3];
            middleHP = new UIImage[max];
            middleMP = new UIImage[max];
            middleExtra = new UIImage[max2];
            buffs = new BuffShow[22];
            states = new BuffShow[CNCID.State.MaxObject];
            init = 0;
            for (int i = 0; i < max; i++) children.Add(middleHP[i] = new UIImage());
            for (int i = 0; i < max2; i++) children.Add(middleExtra[i] = new UIImage());
            children.Add(rightExtra = new UIImage());
            children.Add(leftPanel[2] = new UIImage());
            for (int i = 0; i < max; i++) children.Add(middleMP[i] = new UIImage());
            children.Add(fillHP = new BarFiller());
            children.Add(fillMP = new BarFiller());
            children.Add(fillExtra = new BarFiller());
            for (int i = 0; i < 2; i++) children.Add(leftPanel[i] = new UIImage());
            children.Add(rightHP = new UIImage());
            children.Add(rightMP = new UIImage());
            for (int i = 0; i < 22; i++) children.Add(buffs[i] = new BuffShow());
            for (int i = 0; i < CNCID.State.MaxObject; i++) children.Add(states[i] = new BuffShow());
            id = 0;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            Size = img[0].Size();
            Offset = Vector2.Zero;//new Vector2(Main.screenWidth - 76f, 8f)
            mouseRight = !mouseRightRelease;
            mouseRightRelease = Main.mouseRightRelease;
        }

        private void DrawBars()
        {
            int GetCount(int value, int min = 0, int max = max)
            {
                if (value < min) return min;
                if (value > max) return max;
                return value;
            }
            starCount = GetCount(player.statManaMax2 / 20, 10);
            heartCount = GetCount(player.statLifeMax / 20, 10);
            heart2Count = GetCount((player.statLifeMax - 400) / 5);
            if (init < 2)
            {
                init++;
                rightHP.SetImage(img[0]);
                rightHP.SetRect(new Vector2(Main.screenWidth - 76f, 8f), img[0].Size());//HUD base pos
                fillHP.fill2 = img[6];
                fillHP.fill = img[7];

                rightMP.SetImage(img[2]);
                rightMP.SetRect(rightHP.Offset + new Vector2(-10, img[0].Size().Y - 10), img[2].Size());
                fillMP.fill = img[8];

                rightExtra.SetImage(img[4]);
                rightExtra.SetRect(rightMP.Offset + new Vector2(-20f, 26f), img[4].Size());

                for (int i = 0; i < max; i++)
                {
                    middleHP[i].SetImage(img[1]);
                    middleHP[i].Size = img[1].Size() + new Vector2(1f, 0);
                    middleHP[i].Offset = rightHP.Offset + new Vector2(3f - img[1].Width * i, 10f);
                }
                for (int i = 0; i < max; i++)
                {
                    middleMP[i].SetImage(img[3]);
                    middleMP[i].Size = img[3].Size() + new Vector2(1f, 0);
                    middleMP[i].Offset = rightMP.Offset + new Vector2(3f, 6f) - new Vector2(img[3].Width * i, 0);
                }
                for (int i = 0; i < max2; i++)
                {
                    middleExtra[i].SetImage(img[5]);
                    middleExtra[i].Size = img[5].Size() + new Vector2(1f, 0);
                    middleExtra[i].Offset = rightExtra.Offset + new Vector2(5f, 0) - new Vector2(img[5].Width * i, 0);
                }
                for (int i = 0; i < 3; i++)
                {
                    leftPanel[i].SetImage(img[9]);
                    leftPanel[i].Size = img[9].Size();
                    leftPanel[i].drawOffset = new Vector2(-1f, 0);
                }
            }
            for (int i = 0; i < max; i++) middleHP[i].color = i < heartCount ? Color.White : Color.Transparent;
            for (int i = 0; i < max; i++) middleMP[i].color = i < starCount ? Color.White : Color.Transparent;

            rightHP.Offset = new Vector2(Main.screenWidth - 76f, 8f);//HUD base pos
            rightExtra.Offset = rightMP.Offset + new Vector2(-20f, 26f);
            rightMP.Offset = rightHP.Offset + new Vector2(-10, img[0].Size().Y - 10);
            for (int i = 0; i < max; i++) middleHP[i].Offset = rightHP.Offset + new Vector2(3f - img[1].Width * i, 10f);
            for (int i = 0; i < max; i++) middleMP[i].Offset = rightMP.Offset + new Vector2(3f, 6f) - new Vector2(img[3].Width * i, 0);
            for (int i = 0; i < max2; i++) middleExtra[i].Offset = rightExtra.Offset + new Vector2(5f, 0) - new Vector2(img[5].Width * i, 0);
            for (int i = 0; i < 3; i++) leftPanel[i].drawOffset = new Vector2(-1f, 0);

            leftPanel[0].Offset = rightHP.Offset + new Vector2(5f - img[1].Width * (heartCount - 1) - img[9].Width, 10f);
            leftPanel[1].Offset = rightMP.Offset + new Vector2(5f, 6f) - new Vector2(img[3].Width * starCount - img[9].Width, 0);
            leftPanel[2].Offset = rightExtra.Offset - new Vector2(img[5].Width * (max2 - 1) - img[9].Width + 6f, 0);

            bool flag = allPlayer.IsModDeveloper && allPlayer.EquippedQuartzHead;

            fillHP.SetRect(rightHP.Offset + new Vector2(4f - img[1].Width * heartCount, 16f), new Vector2(img[1].Width * heartCount, fillHP.fill.Height));
            fillHP.value = player.statLife;
            fillHP.valueMax = player.statLifeMax2;
            fillHP.color = flag ? CNCID.Colors.Master : Color.White;
            fillHP.flash = fillHP.progress < .25;
            fillHP.flashSpeed = fillHP.progress < .1f ? .05f : (float)(.005f + .045f * (fillHP.progress / .25));
            fillHP.fillCount = heart2Count;

            fillMP.SetRect(rightMP.Offset + new Vector2(4f, 12f) - new Vector2(img[3].Width * starCount, 0), new Vector2(img[3].Width * starCount, fillMP.fill.Height));
            fillMP.value = player.statMana;
            fillMP.valueMax = player.statManaMax2;
            fillMP.color = flag ? CNCID.Colors.Expert : Color.White;
            fillMP.flash = fillMP.progress < .25;
            fillMP.flashSpeed = fillMP.progress < .1f ? .05f : (float)(.005f + .045f * (fillMP.progress / .25));

            fillExtra.SetRect(rightExtra.Offset + new Vector2(5f, 6f) - new Vector2(img[5].Width * max2, 0), new Vector2(img[5].Width * max2, 12f));
            string arg = "";
            if (modPlayer.clazz == CNCID.Clazz.Barbarian)
            {
                fillExtra.fill = img[10];
                fillExtra.value = modPlayer.clazz_Rage;
                fillExtra.valueMax = modPlayer.clazz_RageMax;
                fillExtra.flash = modPlayer.clazz_Raged;
                arg = (int)modPlayer.clazz_Rage + "/" + (int)modPlayer.clazz_RageMax;
            }
            else if (modPlayer.clazz == CNCID.Clazz.Ranger)
            {
                fillExtra.fill = img[11];
                fillExtra.value = modPlayer.clazz_Power;
                fillExtra.valueMax = modPlayer.clazz_PowerMax;
                fillExtra.flash = fillExtra.progress == 1;
                arg = Math.Round(fillExtra.progress * 100f, 1).ToString() + '%';
            }
            else if (modPlayer.clazz == CNCID.Clazz.Musketeer)
            {
                fillExtra.fill = img[12];
                fillExtra.value = modPlayer.clazz_Durability;
                fillExtra.valueMax = modPlayer.clazz_DurabilityMax;
                fillExtra.flash = modPlayer.clazz_DurabilityReload;
                arg = modPlayer.clazz_Durability + "/" + modPlayer.clazz_DurabilityMax;
            }
            else if (modPlayer.clazz == CNCID.Clazz.Crusader || modPlayer.clazz == CNCID.Clazz.Cleric)
            {
                fillExtra.fill = img[13];
                fillExtra.value = modPlayer.clazz_Blessing;
                fillExtra.valueMax = modPlayer.clazz_BlessingMax;
                fillExtra.flash = fillExtra.progress <= .2;
                arg = (int)modPlayer.clazz_Blessing + "/" + (int)modPlayer.clazz_BlessingMax;
            }
            else if (modPlayer.clazz == CNCID.Clazz.Wizard)
            {
                fillExtra.fill = img[14];
                fillExtra.value = modPlayer.clazz_Sanity;
                fillExtra.valueMax = modPlayer.clazz_SanityMax;
                fillExtra.flash = fillExtra.progress < .5f;
                if (fillExtra.progress < .50f) fillExtra.flashSpeed = .05f;
                if (fillExtra.progress < .25f) fillExtra.flashSpeed = .03f;
                if (fillExtra.progress < .05f) fillExtra.flashSpeed = .01f;
                arg = Math.Round(modPlayer.clazz_Sanity, 1).ToString() + "/" + Math.Round(modPlayer.clazz_SanityMax, 1).ToString();
            }
            else if (modPlayer.clazz == CNCID.Clazz.Assassin)
            {
                fillExtra.fill = img[15];
                fillExtra.value = modPlayer.clazz_Phantom;
                fillExtra.valueMax = modPlayer.clazz_PhantomMax;
                fillExtra.flash = fillExtra.progress == 1 || modPlayer.clazz_PhantomState;
                arg = Math.Round(fillExtra.progress * 100f, 1).ToString() + '%';
            }
            if (modPlayer.clazz > 0)
                MouseText.text = CNCLocalization.GetText("ClassAbility." + CNCID.Clazz.Collection[modPlayer.clazz], arg);
            else
            {
                fillExtra.value = 0;
                fillExtra.valueMax = 1;
                MouseText.text = "";
            }

            MouseText.Visible = false;
            MouseText.textOffset = new Vector2(20f);
            MouseText.textColor = CNCUtils.FlashColor(Color.White);
            if (rightHP.mouseOver || leftPanel[0].mouseOver) mouseText = player.statLife + "/" + player.statLifeMax2;
            if (rightMP.mouseOver || leftPanel[1].mouseOver) mouseText = player.statMana + "/" + player.statManaMax2;
            if (rightExtra.mouseOver || leftPanel[2].mouseOver) MouseText.Visible = true;
            for (int i = 0; i < max; i++) if (middleHP[i].mouseOver && i < heartCount) mouseText = player.statLife + "/" + player.statLifeMax2;
            for (int i = 0; i < max; i++) if (middleMP[i].mouseOver && i < starCount) mouseText = player.statMana + "/" + player.statManaMax2;
            for (int i = 0; i < max2 && !MouseText.Visible; i++) if (middleExtra[i].mouseOver) MouseText.Visible = true;
        }

        private void DrawBuffs()
        {
            int index = -1;
            Point[] basePos =
            {
                new Point(32, 76),
                new Point(32, 38 * 7),
                new Point(576, 24),
                new Point(0, 0),
            };
            int i = 0;
            for (int j = 0; j < 22; j++)
            {
                int type = player.buffType[j];
                int num = 0;
                int lineCount = 11;
                if (Main.playerInventory) num = 1;
                if (player.chest != -1 || Main.npcShop != 0 || Main.InReforgeMenu) num = 2;
                if (Main.EquipPage == 2) num = 3;
                int x = basePos[num].X + i % lineCount * 38;
                int y = basePos[num].Y + i / lineCount * 50;
                if (num == 2)
                {
                    lineCount = 6;
                    x = basePos[num].X + i / lineCount * 38;
                    y = basePos[num].Y + i % lineCount * 50;
                }
                buffs[i].active = type > 0;
                buffs[i].SetRect(new Vector2(x, y), new Vector2(32f, 32f));
                if (num == 3) buffs[i].active = false;
                if (buffs[i].active)
                {
                    int time = 0;
                    buffs[i].SetBuffIcon(type);
                    buffs[i].SetBuffString(type);
                    buffs[i].showTime = Main.TryGetBuffTime(i, out time) && time > 2;
                    buffs[i].time = time;
                    if (buffs[i].mouseOver) index = i;
                    i++;
                }
            }
            while (i < 22) buffs[i++].active = false;
            if (index >= 0)
            {
                bool TryRemovingBuff(int idx, int id)
                {
                    bool common = false;
                    if (Main.debuff[id] || id == 60 || id == 151) return false;
                    if (player.mount.Active && player.mount.CheckBuff(id))
                    {
                        player.mount.Dismount(player);
                        common = true;
                    }
                    for (int slot = 0; slot < 2; slot++)
                    {
                        if (player.hideMisc[slot]) continue;
                        bool flag = player.miscEquips[slot].buffType == id;
                        if (!flag && (id == 101 || id == 102)) flag = player.miscEquips[slot].buffType == 27;
                        if (flag) player.hideMisc[slot] = true;
                    }
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    //SoundEngine.PlaySound(12);
                    if (!common) player.DelBuff(idx);
                    return true;
                }
                int type = player.buffType[index];
                if (mouseRight && mouseRightRelease && TryRemovingBuff(index, type)) buffs[index].active = false;
                MouseText.Visible = true;
                MouseText.text = buffs[index].GetBuffString();
            }
        }

        private void DrawBreath()//这里是直接搬源码的
        {
            bool flag = false;
            if (player.dead) return;
            if (player.lavaTime < player.lavaMax && player.lavaWet) flag = true;
            else if (player.lavaTime < player.lavaMax && player.breath == player.breathMax) flag = true;
            Vector2 vector2_1 = player.Top + new Vector2(0.0f, player.gfxOffY);
            if (Main.playerInventory && Main.screenHeight < 1000) vector2_1.Y += (float)(player.height - 20);
            Vector2 vector2_2 = Vector2.Transform(vector2_1 - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix);
            if (!Main.playerInventory || Main.screenHeight >= 1000) vector2_2.Y -= 100f;
            Vector2 vector2_3 = vector2_2 / Main.UIScale;
            if (Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
            {
                vector2_3 = new Vector2((float)(Main.screenWidth / 2), (float)(Main.screenHeight / 2 + 236));
                if (Main.InGameUI.IsVisible) vector2_3.Y = (float)(Main.screenHeight - 64);
            }
            if (player.breath < player.breathMax && !player.ghost && !flag)
            {
                int num1 = player.breathMax / 20;
                int num2 = 20;
                for (int index = 1; index < player.breathMax / num2 + 1; ++index)
                {
                    float scale = 1f;
                    int num3;
                    if (player.breath >= index * num2) num3 = (int)byte.MaxValue;
                    else
                    {
                        float num4 = (float)(player.breath - (index - 1) * num2) / (float)num2;
                        num3 = (int)(30.0 + 225.0 * (double)num4);
                        if (num3 < 30) num3 = 30;
                        scale = (float)((double)num4 / 4.0 + 0.75);
                        if ((double)scale < 0.75) scale = 0.75f;
                    }
                    int num5 = 0;
                    int num6 = 0;
                    if (index > 10)
                    {
                        num5 -= 260;
                        num6 += 26;
                    }
                    Main.spriteBatch.Draw(TextureAssets.Bubble.Value, vector2_3 + new Vector2((float)(26 * (index - 1) + num5) - 125f, (float)(32.0 + ((double)TextureAssets.Bubble.Height() - (double)TextureAssets.Bubble.Height() * (double)scale) / 2.0) + (float)num6), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.Bubble.Width(), TextureAssets.Bubble.Height())), new Microsoft.Xna.Framework.Color(num3, num3, num3, num3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                }
            }
            if (((player.lavaTime >= player.lavaMax ? 0 : (!player.ghost ? 1 : 0)) & (flag ? 1 : 0)) == 0) return;
            int num7 = player.lavaMax / 10;
            int num8 = player.breathMax / num7;
            for (int index = 1; index < player.lavaMax / num7 + 1; ++index)
            {
                float scale = 1f;
                int num9;
                if (player.lavaTime >= index * num7) num9 = (int)byte.MaxValue;
                else
                {
                    float num10 = (float)(player.lavaTime - (index - 1) * num7) / (float)num7;
                    num9 = (int)(30.0 + 225.0 * (double)num10);
                    if (num9 < 30) num9 = 30;
                    scale = (float)((double)num10 / 4.0 + 0.75);
                    if ((double)scale < 0.75) scale = 0.75f;
                }
                int num11 = 0;
                int num12 = 0;
                if (index > 10)
                {
                    num11 -= 260;
                    num12 += 26;
                }
                Main.spriteBatch.Draw(TextureAssets.Flame.Value, vector2_3 + new Vector2((float)(26 * (index - 1) + num11) - 125f, (float)(32.0 + ((double)TextureAssets.Flame.Height() - (double)TextureAssets.Flame.Height() * (double)scale) / 2.0) + (float)num12), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.Bubble.Width(), TextureAssets.Bubble.Height())), new Microsoft.Xna.Framework.Color(num9, num9, num9, num9), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            }
        }

        private void DrawStates()
        {
            Point[] basePos =
            {
                new Point((int)(screen.X - 40), 80),
            };
            int i = 0;
            for (int j = 0; j < modPlayer.stateType.Length; j++)
            {
                short type = modPlayer.stateType[j];
                int num = 0;
                int lineCount = 11;
                int x = basePos[num].X + i / lineCount * 38;
                int y = basePos[num].Y + i % lineCount * 50;
                states[i].active = type > 0;
                states[i].SetRect(new Vector2(x, y), new Vector2(32f, 32f));
                if (states[i].active)
                {
                    string keyName = CNCID.State.Collection[type];
                    Texture2D icon = ModContent.Request<Texture2D>("CNC/Content/Buffs/Classes/" + keyName).Value;
                    states[i].SetBuffIcon(icon);
                    states[i].SetBuffString(CNCLocalization.GetText("ClassState." + keyName), CNCLocalization.GetText("ClassStateDesc." + keyName));
                    states[i].showTime = true;
                    states[i].time = modPlayer.stateTime[i];
                    if (states[i].mouseOver) mouseText = states[i].GetBuffString();
                    i++;
                }
            }
            while (i < CNCID.State.MaxObject) states[i++].active = false;
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!player.ShouldNotDraw)
            {
                DrawBars();
                DrawBuffs();
                DrawBreath();
                DrawStates();
            }
        }

        public class BarFiller : UIElement
        {
            public Texture2D fill;
            public Texture2D fill2;
            public float value;
            public float valueMax;
            public double progress;
            public int fillCount;
            public Color color;
            public bool flash;
            public float flashSpeed;
            private float alpha;
            private int mul;

            public BarFiller()
            {
                Size = Vector2.One;
                Offset = Vector2.Zero;
                value = 0;
                valueMax = 1;
                progress = 0;
                fillCount = 0;
                color = Color.White;
                flash = false;
                flashSpeed = .04f;
                alpha = 1f;
                mul = 1;
            }

            public override void UpdateUI(GameTime gameTime)
            {
                base.UpdateUI(gameTime);
                progress = value / valueMax;
                if (progress > 1) progress = 1;
                if (flash)
                {
                    alpha += mul * flashSpeed;
                    if (alpha < .25f)
                    {
                        mul = 1;
                        alpha = .25f;
                    }
                    if (alpha > 1f)
                    {
                        mul = -1;
                        alpha = 1f;
                    }
                }
                else
                {
                    alpha += flashSpeed;
                    if (alpha > 1f) alpha = 1f;
                }
            }

            protected override void Draw(SpriteBatch spriteBatch, Rectangle rect)
            {
                Texture2D image = (fill == null && fill2 != null) ? fill2 : fill;
                if (image != null)
                {
                    double max = width / image.Width * progress;
                    double _progress = max - (int)max;
                    for (int i = 0; i < (int)max; i++)
                    {
                        Texture2D _image = image;
                        if (fill2 != null && i >= fillCount) _image = fill2;
                        Rectangle _rect = new Rectangle((int)(rect.X + width - _image.Width * i), rect.Y, _image.Width, rect.Height);
                        spriteBatch.Draw(_image, _rect, color * alpha);
                    }
                    if(_progress > 0)
                    {
                        Texture2D _image = image;
                        if (fill2 != null && (int)max >= fillCount) _image = fill2;
                        int x = (int)(_image.Width * (1 - _progress));
                        Vector2 pos = rect.TopRight() - new Vector2(_image.Width * (int)max - x , 0);
                        Rectangle _rect = new Rectangle((int)pos.X, rect.Y, _image.Width - x, rect.Height);
                        Rectangle _rect2 = new Rectangle(x, 0, _image.Width - x, _image.Height);
                        spriteBatch.Draw(_image, _rect, _rect2, color * alpha);
                    }
                }
            }
        }

        public class BuffShow : UIElement
        {
            public Texture2D icon;
            public Color timeColor;
            public Color nameColor;
            public Color descColor;
            public Color iconColor;
            public Color iconColorOver;
            public string name;
            public string desc;
            public int index;
            public int time;
            public bool showTime;
            public bool show;
            private float alpha;
            private Player player => Main.LocalPlayer;
            public bool active;

            public BuffShow()
            {
                Size = new Vector2(32f, 32f);
                Offset = new Vector2(32f, 76f);
                timeColor = Color.White;
                nameColor = Color.White;
                descColor = Color.White;
                iconColor = Color.White * .4f;
                iconColorOver = Color.White;
                name = "";
                desc = "";
                index = 0;
                time = 0;
                active = true;
                showTime = false;
                alpha = 0f;
                mouseOver = false;
                show = true;
            }

            public void SetBuffIcon(Texture2D icon) => this.icon = icon;
            public void SetBuffIcon(int type)
            {
                if (type < Main.maxBuffTypes) icon = TextureAssets.Buff[type].Value;
                else icon = ModContent.Request<Texture2D>(BuffLoader.GetBuff(type).Texture).Value;
            }

            public void SetBuffString(string name, string desc)
            {
                this.name = name;
                this.desc = desc;
            }
            public void SetBuffString(int type)
            {
                if (type < Main.maxBuffTypes)
                {
                    name = Lang.GetBuffName(type);
                    desc = Lang.GetBuffDescription(type);
                    if (type == 94) desc += (object)((int)((double)Main.LocalPlayer.manaSickReduction * 100.0) + 1) + "%";
                    if (type == 26 && Main.expertMode) desc = Language.GetTextValue("BuffDescription.WellFed_Expert");
                    if (type == 206 && Main.expertMode) desc = Language.GetTextValue("BuffDescription.WellFed2_Expert");
                    if (type == 207 && Main.expertMode) desc = Language.GetTextValue("BuffDescription.WellFed3_Expert");
                }
                else
                {
                    ModBuff modBuff = BuffLoader.GetBuff(type);
                    name = modBuff.DisplayName.GetTranslation(CNCLocalization.language) + $" [{modBuff.Mod.Name}]";
                    desc = modBuff.Description.GetTranslation(CNCLocalization.language);
                }
            }

            public string GetBuffString()
            {
                string ColorToHex(Color _color)
                {
                    string res = "";
                    int[] num = new int[8];
                    byte[] ARGB = new byte[4] { _color.A, _color.R, _color.G, _color.B };
                    string[] hex = new string[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
                    for (int i = 0; i < 8; i++) num[i] = Math.Abs(ARGB[i / 2] / (int)Math.Pow(16, (i + 1) % 2) % 16);
                    for (int i = 0; i < 8; i++) res += hex[num[i]];
                    return res;
                }
                string str = name;// $"[c/{ ColorToHex(nameColor)}:{name}]";
                if (desc.Length > 0)
                {
                    if (str.Length > 0) str += '\n';
                    str += desc;// $"[c/{ ColorToHex(descColor)}:{desc}]";
                }
                return str;
            }

            public override void UpdateUI(GameTime gameTime)
            {
                if (!active)
                {
                    name = "";
                    desc = "";
                    alpha = .4f;
                    mouseOver = false;
                    return;
                }
                if (mouseOver && alpha < 1.0f) alpha += .10f;
                if (!mouseOver && alpha > .4f) alpha -= .05f;
                if (alpha > 1.0f) alpha = 1.0f;
                if (alpha < 0.4f) alpha = 0.4f;
                if (mouseOver) player.mouseInterface = true;
            }

            protected override void Draw(SpriteBatch spriteBatch, Rectangle rect)
            {
                if (!active || !show) return;
                Color color = Color.Lerp(iconColor, iconColorOver, alpha);
                if (icon != null) spriteBatch.Draw(icon, Rect, color);
                if (showTime && time > 2) DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Const.Font[3], Lang.LocalizedDuration(new TimeSpan(0, 0, time / 60), true, false), Offset + new Vector2(0, icon.Height), timeColor * alpha, 0, Vector2.Zero, .8f, 0, 0);
            }
        }
    }
}