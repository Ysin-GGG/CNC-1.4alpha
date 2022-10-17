using System;
using System.Timers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using CNC.Common.Utils;
using CNC.Content;
using Terraria.GameInput;

namespace CNC.Common.Global.Players
{
    public class AllPlayer : ModPlayer
    {
        Player player => Player;
        AllPlayer modPlayer => player.GetModPlayer<AllPlayer>();

        public int lifeRegen;
        public int lifeRegenCount;
        /// <summary> 震动数值 </summary>
        public Vector2 shakeValue;
        /// <summary> 震动方向 </summary>
        private Vector2 shakeDirection;
        /// <summary> 震动频率 </summary>
        private Vector2 shakeFrequency;
        /// <summary> 震动幅度 </summary>
        private Vector2 shakeAmplitude;
        /// <summary> 震动持续时长 </summary>
        private int shakeTime;
        /// <summary> 震动衰减程度 </summary>
        public float shakeCur;
        /// <summary> 震动衰减速度 </summary>
        private float shakeAttenuation;
        private Vector2 instance;

        private int mouseToCast;
        private int preMouse = 0;
        private int mouseTime = 0;
        private int ignoreMouseTime = 0;
        public bool pressingMouseLeft = false;
        public bool pressingMouseRight = false;
        public float castTime;
        private float castTime2;
        public float castTimeMax;
        public bool ShowCastTimeBar { get => CastTimeBarTime == Options.CastTimeBar.showTime; set => CastTimeBarTime = (value ? Options.CastTimeBar.showTime : 0); }
        private int CastTimeBarTime;
        public bool casting;
        public bool cancelCastingIfRelease;
        public float leftCastTime;
        public float rightCastTime;
        private Item heldItem;

        /// <summary> 视角锁定的坐标 </summary>
        public Vector2 lockScreenPosition;
        public int lockScreenTime;
        public float lockScreenAmount;
        public float lockScreenSpeedAdd;
        public float lockScreenSpeedMult;
        public float unlockScreenSpeedAdd;
        public float unlockScreenSpeedMult;

        public bool ZoneTranquility => zone[0];

        public int Tick { get; private set; } = 0;
        public Item AssistantItem { get; set; } = new Item();

        public float[] CDReduction = new float[9];
        public float manaReturn = 0;
        public float dealtDamage { get => player.GetModPlayer<ClassesSettings>().dealtDamage; set => player.GetModPlayer<ClassesSettings>().dealtDamage = value; }
        public float takenDamage { get => player.GetModPlayer<ClassesSettings>().takenDamage; set => player.GetModPlayer<ClassesSettings>().takenDamage = value; }
        public float takenDamageFromEnemies { get => player.GetModPlayer<ClassesSettings>().takenDamageFromEnemies; set => player.GetModPlayer<ClassesSettings>().takenDamageFromEnemies = value; }
        public bool[] buff = new bool[CNCID.Buff.MaxObject];
        public bool[] zone = new bool[1];
        public bool IsModDeveloper => name.Contains(player.name);
        public bool EquippedQuartzHead => player.armor[0].type == ModContent.ItemType<Content.Items.Developers.Quartz.QuartzBlock>();
        public string[] developers => name.ToArray();
        public int[] bleeding = new int[6];

        private List<string> name = new List<string>()
        {
            "Quartz", "quartz",
            "HHvs",
        };

        public override void ResetEffects()
        {
            this.manaReturn = 0;
            this.Tick = (Tick + 1) % 60;

            if (this.mouseTime > 0) this.mouseTime--;
            else preMouse = 0;

            if (this.CastTimeBarTime > 0) { if (mouseToCast == 0) this.CastTimeBarTime--; }
            else this.castTimeMax = this.castTime2 = 0;

            if (this.heldItem != player.HeldItem)
            {
                this.heldItem = player.HeldItem;
                this.ResetCasting(false);
            }

            if (this.mouseToCast == 1) this.castTime2 = this.castTime = this.leftCastTime++;
            if (this.mouseToCast == 2) this.castTime2 = this.castTime = this.rightCastTime++;
            if (this.leftCastTime < 0) this.leftCastTime++;
            if (this.rightCastTime < 0) this.rightCastTime++;

            for (int i = 0; i < this.CDReduction.Length; i++) this.CDReduction[i] = 1f;
            for (int i = 0; i < this.buff.Length; i++) this.buff[i] = false;
        }

        public void SetCasting(int mouse, float time, float timeMax, bool cancelCastingIfRelease = false)
        {
            this.preMouse = 0;
            this.casting = true;
            this.castTimeMax = timeMax;
            this.cancelCastingIfRelease = cancelCastingIfRelease;
            this.ShowCastTimeBar = true;
            if (this.mouseToCast == 0) this.mouseToCast = mouse;
            if (this.mouseToCast == 1) this.leftCastTime = this.castTime2 = time;
            if (this.mouseToCast == 2) this.rightCastTime = this.castTime2 = time;
        }

        public bool ResetCasting(bool resetTimeMax = false)
        {
            bool accepted = this.castTime >= this.castTimeMax;
            if (accepted)
            {
                this.castTime2 = 0f;
                this.leftCastTime = this.mouseToCast == 1 ? -2f : 0f;
                this.rightCastTime = this.mouseToCast == 2 ? -2f : 0f;
            }
            else
            {
                this.leftCastTime = 0f;
                this.rightCastTime = 0f;
            }
            this.casting = false;
            this.mouseToCast = 0;
            this.castTime = 0f;
            this.castTimeMax = resetTimeMax ? 0f : this.castTimeMax;
            return accepted;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            base.ProcessTriggers(triggersSet);
            if (triggersSet.MouseLeft && !pressingMouseLeft) this.OnClickMouse(this.pressingMouseLeft = true, false, false);
            if (triggersSet.MouseRight && !pressingMouseRight) this.OnClickMouse(false, this.pressingMouseRight = true, false);
            if (!triggersSet.MouseLeft && pressingMouseLeft) { this.OnReleaseMouse(!(this.pressingMouseLeft = false), false, false); this.preMouse = 1; }
            if (!triggersSet.MouseRight && pressingMouseRight) { this.OnReleaseMouse(false, !(this.pressingMouseRight = false), false); this.preMouse = 2; }

        }

        /// <summary> 屏幕震动 </summary>
        /// <param name="time"> 震动时长 </param>
        /// <param name="frequency"> 震动速率 </param>
        /// <param name="amplitude"> 震动幅度 </param>
        /// <param name="attenuation"> 震动衰减速度 </param>
        /// <param name="startOffset"> 初始震动偏移 </param>
        public void ScreenShake(int time, Vector2 frequency, Vector2 amplitude, float attenuation, Vector2 startOffset = default)
        {
            this.shakeTime = time;
            this.shakeDirection = new Vector2(frequency.X < 0 ? -1 : 1, frequency.Y < 0 ? -1 : 1);
            this.shakeFrequency = frequency;
            this.shakeValue = startOffset;
            this.shakeAmplitude = amplitude;
            this.shakeAttenuation = attenuation;
            this.shakeCur = 1f;
        }

        private void UpdateShakeData()
        {
            Vector2 amplitude = new Vector2(Math.Abs(this.shakeAmplitude.X), Math.Abs(this.shakeAmplitude.Y));
            if (this.shakeTime-- <= 0) this.shakeCur *= this.shakeCur < .1f ? 0f : this.shakeAttenuation;
            if (this.shakeValue.X <= -amplitude.X) this.shakeDirection.X = 1;
            if (this.shakeValue.X >= amplitude.X) this.shakeDirection.X = -1;
            if (this.shakeValue.Y <= -amplitude.Y) this.shakeDirection.Y = 1;
            if (this.shakeValue.Y >= amplitude.Y) this.shakeDirection.Y = -1;
            this.shakeValue.X = MyMath.Clamp(this.shakeValue.X + this.shakeDirection.X * this.shakeFrequency.X, -amplitude.X, amplitude.X);
            this.shakeValue.Y = MyMath.Clamp(this.shakeValue.Y + this.shakeDirection.Y * this.shakeFrequency.Y, -amplitude.Y, amplitude.Y);
        }

        public override void ModifyScreenPosition()
        {
            if (lockScreenTime > 0 && lockScreenAmount == 1) lockScreenTime--;
            if (lockScreenTime > 0 && lockScreenAmount < 1) lockScreenAmount = MyMath.Min((lockScreenAmount + lockScreenSpeedAdd) * lockScreenSpeedMult, 1f);
            if (lockScreenTime <= 0 && lockScreenAmount > 0) lockScreenAmount = MyMath.Max((lockScreenAmount - unlockScreenSpeedAdd) * unlockScreenSpeedMult, 0f);
            if (lockScreenAmount > 0) Main.screenPosition = MyMath.Lerp(player.Center, lockScreenPosition, lockScreenAmount);
            if (lockScreenTime > 0) Main.screenPosition = lockScreenPosition;
            if (this.shakeCur > 0) Main.screenPosition += this.shakeValue * this.shakeCur;
        }

        public override void PostUpdate()
        {
            if (this.shakeCur > 0) this.UpdateShakeData();
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (this.casting) this.casting = false;
        }

        public void OnClickMouse(bool left, bool right, bool middle)
        {
        }

        public void OnReleaseMouse(bool left, bool right, bool middle)
        {
            mouseTime = Options.CancelCasting.clickDelay;
            if (this.cancelCastingIfRelease)
            {
                if (left && this.mouseToCast == 1) this.ResetCasting(false);
                if (right && this.mouseToCast == 2) this.ResetCasting(false);
            }
            else
            {
                if (this.ignoreMouseTime <= 0)
                {
                    int type = (int)Options.CancelCasting.type;
                    bool cancel = false;
                    switch (type)
                    {
                        case 0:
                            cancel = true;
                            break;
                        case 1:
                            cancel = (left && this.preMouse == 1) || (right && this.preMouse == 2);
                            break;
                        case 2:
                            cancel = right && preMouse == 1;
                            break;
                        case 3:
                            cancel = left && preMouse == 2;
                            break;
                    }
                    if (cancel) { this.ResetCasting(false); Main.NewText("canceled"); }
                }
                else this.ignoreMouseTime = Options.CancelCasting.releaseDelay;
            }
        }

        public static void DrawCastTimeBar(Player player)
        {
            Texture2D[] t =
            {
                CNC.GetTexture("Content/UI/CastTimeBar/Images/IconBox"),
                CNC.GetTexture("Content/UI/CastTimeBar/Images/Icon1"),
                CNC.GetTexture("Content/UI/CastTimeBar/Images/Filler"),
                CNC.GetTexture("Content/UI/CastTimeBar/Images/Bar"),
                CNC.GetTexture("Content/UI/CastTimeBar/Images/Lid"),
            };
            if (player.IsMe())
            {
                AllPlayer ap = player.GetModPlayer<AllPlayer>();
                if (Options.CastTimeBar.showUI && ap.CastTimeBarTime > 0 && ap.castTimeMax > 0)
                {
                    int side = (int)Options.CastTimeBar.side;
                    bool flip = Options.CastTimeBar.flip;
                    float scale = Options.CastTimeBar.scale;
                    float opacity = Options.CastTimeBar.opacity * MyMath.Lerp(0f, 1f, (float)MyMath.Min(Options.CastTimeBar.showTime, Options.CastTimeBar.fadeTime) > 0 ? ap.CastTimeBarTime / (float)MyMath.Min(Options.CastTimeBar.showTime, Options.CastTimeBar.fadeTime) : 1f);
                    float progress = MyMath.Lerp(0f, (t[3].Width - 10) / t[2].Width, (ap.castTime2 / ap.castTimeMax));
                    float rotation = 0f;
                    float alpha = Math.Abs(MyMath.sin((ap.castTime - ap.castTimeMax) * MyMath.deg01 * 3f));
                    Vector2 pos = -new Vector2((t[0].Width + t[3].Width + t[4].Width - 8f) / 2f, t[0].Height / 2f) * scale;
                    Color color = (ap.castTime2 == ap.castTime ? Color.Lerp(Color.White, Options.CastTimeBar.fullColor, ap.castTime >= ap.castTimeMax ? alpha : 0f) : Options.CastTimeBar.cancelColor);
                    if (side == 0)
                    {
                        rotation = MyMath.deg90 * 2;
                        pos += player.Bottom;
                    }
                    if (side == 1)
                    {
                        rotation = MyMath.deg90 * 0;
                        pos += player.Top;
                    }
                    if (side == 2 || (side == 4 && player.direction != 1) || (side == 5 && player.direction == 1))
                    {
                        rotation = MyMath.deg90 * 1;
                        pos += player.Left;
                    }
                    if (side == 3 || (side == 4 && player.direction == 1) || (side == 5 && player.direction != 1))
                    {
                        rotation = MyMath.deg90 * 3;
                        pos += player.Right;
                    }
                    pos += new Vector2(Options.CastTimeBar.offset.X - 100f, Options.CastTimeBar.offset.Y).RotatedBy(rotation);
                    pos = pos.ToPoint().ToVector2();
                    drawData[] d =
                    {
                        new drawData(t[0], pos, flip, scale, opacity),
                        new drawData(t[1], pos, flip, scale, opacity),
                        new drawData(t[2], pos, flip, scale, opacity, flip ? t[2].TopRight() : t[2].TopLeft(), new Vector2(progress, 1f), color),
                        new drawData(t[3], pos, flip, scale, opacity),
                        new drawData(t[4], pos, flip, scale, opacity),
                    };
                    d[0].Center = d[1].Center;
                    d[3].Left = d[1].Right;
                    d[4].Left = d[3].Right;
                    d[4].pos.X -= 8f;
                    d[2].Left = d[3].Left;
                    d[2].pos.X += 8f;
                    for (int i = 0; i < d.Length; i++) d[i].Draw(Main.spriteBatch);
                }
            }
        }

        private struct drawData
        {
            public Texture2D tex;
            public Color color;
            public Vector2 origin;
            public Vector2 pos;
            public bool flip;
            public Vector2 scale;
            public float opacity;
            public float totalScale;

            public Vector2 TopLeft { get => pos + (flip ? tex.TopRight() : tex.TopLeft()) * totalScale; set => pos = value - (flip ? tex.TopRight() : tex.TopLeft()) * totalScale; }
            public Vector2 Top { get => pos + tex.Top() * totalScale; set => pos = value - tex.Top() * totalScale; }
            public Vector2 TopRight { get => pos + (flip ? tex.TopLeft() : tex.TopRight()) * totalScale; set => pos = value - (flip ? tex.TopLeft() : tex.TopRight()) * totalScale; }
            public Vector2 Left { get => pos + (flip ? tex.Right() : tex.Left()) * totalScale; set => pos = value - (flip ? tex.Right() : tex.Left()) * totalScale; }
            public Vector2 Center { get => pos + tex.Center() * totalScale; set => pos = value - tex.Center() * totalScale; }
            public Vector2 Right { get => pos + (flip ? tex.Left() : tex.Right()) * totalScale; set => pos = value - (flip ? tex.Left() : tex.Right()) * totalScale; }
            public Vector2 BottomLeft { get => pos + (flip ? tex.BottomRight() : tex.BottomLeft()) * totalScale; set => pos = value - (flip ? tex.BottomRight() : tex.BottomLeft()) * totalScale; }
            public Vector2 Bottom { get => pos + tex.Bottom() * totalScale; set => pos = value - tex.Bottom() * totalScale; }
            public Vector2 BottomRight { get => pos + (flip ? tex.BottomLeft() : tex.BottomRight()) * totalScale; set => pos = value - (flip ? tex.BottomLeft() : tex.BottomRight()) * totalScale; }

            public drawData(Texture2D tex, Vector2 pos, bool flip, float scale1, float opacity, Vector2? origin = null, Vector2? scale2 = null, Color? color = null)
            {
                this.tex = tex;
                this.pos = pos;
                this.flip = flip;
                this.opacity = opacity;
                this.totalScale = scale1;
                this.scale = scale2 ?? Vector2.One;
                this.color = color ?? Color.White;
                this.origin = origin ?? Vector2.Zero;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Vector2 _pos = pos - Main.screenPosition;
                spriteBatch.Draw(tex, _pos.ToPoint().ToVector2(), null, color * opacity, 0f, origin, scale * totalScale, (SpriteEffects)flip.ToInt(1), 0f);
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.IsMe() && drawInfo.drawPlayer.HasItem(ModContent.ItemType<Content.Items.Developers.Quartz.Anatta>()))
                switch (drawInfo.heldItem.type)
                {
                    case ItemID.TrueNightsEdge:
                    case ItemID.TrueExcalibur:
                    case ItemID.FieryGreatsword:
                    case ItemID.Meowmere:
                        drawInfo.ItemLocation = Vector2.Zero;
                        break;
                }
        }
    }
}