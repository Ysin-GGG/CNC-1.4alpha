using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using CNC.Common.Utils;
using CNC.Common.Localization;

namespace CNC
{
    public class Options
    {
        public static bool useModMusic;
        public static CNCConfigElement.CollapseBar.Element_Edge.Type closeEdge = 0;
        public static CNCConfigElement.HoverTextBox.Element_Style.Type[] hoverTextBox = new CNCConfigElement.HoverTextBox.Element_Style.Type[CNCConfigElement.HoverTextBox.Count];
        public static float[] hoverTextBoxAlpha = new float[CNCConfigElement.HoverTextBox.Count];
        public static float offset = 0;
        public static float movementSpeed = 5;
        public static CNCConfigElement.AllysCastTimeBar CastTimeBar = new CNCConfigElement.AllysCastTimeBar();
    }

    [Label("$Mods.CNC.Config.Client")]
    public class ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("$Mods.CNC.Config.UseModMusic")]
        [Tooltip("$Mods.CNC.Config.OnlyFor")]
        [DefaultValue(true)]
        public bool useModMusic = true;

        [Label("$Mods.CNC.Config.HoverTextBox_Label")]
        public CNCConfigElement.HoverTextBox hoverTextBox = new CNCConfigElement.HoverTextBox();

        [Label("$Mods.CNC.Config.CollapseBar_Label")]
        public CNCConfigElement.CollapseBar collapseBar = new CNCConfigElement.CollapseBar(0, 0f, 5f);

        [Label("$Mods.CNC.Config.CastTimeBar_Label")]
        public CNCConfigElement.AllysCastTimeBar.MyCastTimeBar castTimeBar = new CNCConfigElement.AllysCastTimeBar.MyCastTimeBar();

        public override void OnChanged()
        {
            Options.useModMusic = useModMusic;
            Options.hoverTextBox[0] = hoverTextBox.style1;
            Options.hoverTextBox[1] = hoverTextBox.style2;
            Options.hoverTextBox[2] = hoverTextBox.style3;
            Options.hoverTextBox[3] = hoverTextBox.style4;
            Options.hoverTextBox[4] = hoverTextBox.style5;
            Options.hoverTextBoxAlpha[0] = hoverTextBox.opacity1;
            Options.hoverTextBoxAlpha[1] = hoverTextBox.opacity2;
            Options.hoverTextBoxAlpha[2] = hoverTextBox.opacity3;
            Options.hoverTextBoxAlpha[3] = hoverTextBox.opacity4;
            Options.hoverTextBoxAlpha[4] = hoverTextBox.opacity5;
            Options.closeEdge = collapseBar.closeEdge;
            Options.offset = collapseBar.offset;
            Options.movementSpeed = collapseBar.movementSpeed;
            Options.CastTimeBar = this.castTimeBar;
        }
    }
    /*
    [Label("$Mods.CNC.Config.Server")]
    public class ServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        
        public override void OnChanged()
        {
        }
    }
    */
    public class CNCConfigElement
    {
        internal const float MinOpacity = 0.2f;
        internal const float MaxOpacity = 1.0f;
        internal const float DefaultOpacity = 0.785f;

        public class HoverTextBox
        {
            public const int Count = 5;

            [Header("$Mods.CNC.Config.HoverTextBox_Type_1")]
            [Label("$Mods.CNC.Config.HoverTextBox_Style")]
            [DefaultValue(1)]
            public Element_Style.Type style1;
            [Label("$Mods.CNC.Config.HoverTextBox_Opacity")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float opacity1;

            [Header("$Mods.CNC.Config.HoverTextBox_Type_2")]
            [Label("$Mods.CNC.Config.HoverTextBox_Style")]
            [DefaultValue(2)]
            public Element_Style.Type style2;
            [Label("$Mods.CNC.Config.HoverTextBox_Opacity")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float opacity2;

            [Header("$Mods.CNC.Config.HoverTextBox_Type_3")]
            [Label("$Mods.CNC.Config.HoverTextBox_Style")]
            [DefaultValue(2)]
            public Element_Style.Type style3;
            [Label("$Mods.CNC.Config.HoverTextBox_Opacity")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float opacity3;

            [Header("$Mods.CNC.Config.HoverTextBox_Type_4")]
            [Label("$Mods.CNC.Config.HoverTextBox_Style")]
            [DefaultValue(2)]
            public Element_Style.Type style4;
            [Label("$Mods.CNC.Config.HoverTextBox_Opacity")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float opacity4;

            [Header("$Mods.CNC.Config.HoverTextBox_Type_5")]
            [Label("$Mods.CNC.Config.HoverTextBox_Style")]
            [DefaultValue(1)]
            public Element_Style.Type style5;
            [Label("$Mods.CNC.Config.HoverTextBox_Opacity")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float opacity5;

            public override string ToString() => "";

            public HoverTextBox()
            {
                style1 = (Element_Style.Type)1;
                style2 = (Element_Style.Type)2;
                style3 = (Element_Style.Type)2;
                style4 = (Element_Style.Type)2;
                style5 = (Element_Style.Type)1;
                opacity1 = DefaultOpacity;
                opacity2 = DefaultOpacity;
                opacity3 = DefaultOpacity;
                opacity4 = DefaultOpacity;
                opacity5 = DefaultOpacity;
            }

            public override bool Equals(object obj)
            {
                if (obj is HoverTextBox other)
                {
                    bool flag = true;
                    float[] o1 = this.GetOpacityArray();
                    float[] o2 = other.GetOpacityArray();
                    var s1 = this.GetStyleArray();
                    var s2 = other.GetStyleArray();
                    for (int i = 0; i < Count && flag; i++) flag = o1[i] == o2[i] && s1[i] == s2[i];
                    return flag;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new
                {
                    style1,
                    style2,
                    style3,
                    style4,
                    style5,
                    opacity1,
                    opacity2,
                    opacity3,
                    opacity4,
                    opacity5,
                }.GetHashCode();
            }

            public float[] GetOpacityArray() => new float[] { opacity1, opacity2, opacity3, opacity4, opacity5 };
            public Element_Style.Type[] GetStyleArray() => new Element_Style.Type[] { style1, style2, style3, style4, style5 };

            public class Element_Style : GenericConfigElement
            {
                [CustomModConfigItem(typeof(Element_Style))]
                public enum Type
                {
                    None,
                    Origin,
                    Brown,
                }

                protected override string[] ValueString => new string[]
                {
                    "HoverTextBox_Style_None",
                    "HoverTextBox_Style_Origin",
                    "HoverTextBox_Style_Brown",
                };

                public override void Click(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().NextEnum());
                public override void RightClick(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().PreviousEnum());
            }
        }

        [BackgroundColor(87, 77, 91)]
        public class CollapseBar
        {
            [BackgroundColor(57, 49, 65)]
            [Label("$Mods.CNC.Config.CollapseBar_Edge")]
            [DefaultValue(0)]
            public Element_Edge.Type closeEdge;

            [Slider]
            [BackgroundColor(57, 49, 65)]
            [Label("$Mods.CNC.Config.CollapseBar_Offset")]
            [DefaultValue(0)]
            [Range(-1f, 1f)]
            public float offset;

            [BackgroundColor(57, 49, 65)]
            [Label("$Mods.CNC.Config.CollapseBar_MovementSpeed")]
            [DefaultValue(5f)]
            [Range(2f, 20f)]
            public float movementSpeed;

            public override string ToString() => "";

            public CollapseBar() { closeEdge = 0; offset = 0; movementSpeed = 4f; }
            public CollapseBar(int type, float value, float spd) { closeEdge = (Element_Edge.Type)type; offset = value; movementSpeed = spd; }

            public override bool Equals(object obj)
            {
                if (obj is CollapseBar other) return closeEdge == other.closeEdge && offset == other.offset && movementSpeed == other.movementSpeed;
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { closeEdge, offset, movementSpeed }.GetHashCode();
            }

            public class Element_Edge : GenericConfigElement
            {
                [CustomModConfigItem(typeof(Element_Edge))]
                public enum Type
                {
                    Bottom,
                    Left,
                    Top,
                    Right,
                }

                protected override string[] ValueString => new string[]
                {
                    "CollapseBar_Edge_Bottom",
                    "CollapseBar_Edge_Left",
                    "CollapseBar_Edge_Top",
                    "CollapseBar_Edge_Right",
                };

                public override void Click(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().NextEnum());
                public override void RightClick(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().PreviousEnum());
            }
        }

        public class AllysCastTimeBar
        {
            [Label("$Mods.CNC.Config.CastTimeBar_ShowUI")]
            [DefaultValue(true)]
            public bool showUI;

            [Label("$Mods.CNC.Config.CastTimeBar_Style")]
            [DefaultValue(1)]
            public Element_Style.Type style => Element_Style.Type.Fancy;

            [Label("$Mods.CNC.Config.CastTimeBar_Side")]
            [DefaultValue(1)]
            public Element_Side.Type side;

            [Label("$Mods.CNC.Config.CastTimeBar_Flip")]
            [DefaultValue(false)]
            public bool flip => false;

            [Label("$Mods.CNC.Config.CastTimeBar_ShowTime")]
            [DefaultValue(60)]
            [Range(1, 180)]
            [Slider]
            public int showTime;

            [Label("$Mods.CNC.Config.CastTimeBar_FadeTime")]
            [DefaultValue(30)]
            [Range(1, 180)]
            [Slider]
            public int fadeTime;

            [Label("$Mods.CNC.Config.CastTimeBar_Opacity")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float opacity;

            [Label("$Mods.CNC.Config.CastTimeBar_Scale")]
            [DefaultValue(1f)]
            [Range(0.75f, 1.25f)]
            public float scale;

            [Label("$Mods.CNC.Config.CastTimeBar_Offset")]
            [DefaultValue(typeof(Vector2), "100, 100")]
            [Range(50f, 150f)]
            public Vector2 offset;

            [Label("$Mods.CNC.Config.CastTimeBar_CancelColor")]
            [DefaultValue(typeof(Color), "255, 0, 0, 255")]
            public Color cancelColor;

            [Label("$Mods.CNC.Config.CastTimeBar_FullColor")]
            [DefaultValue(typeof(Color), "0, 255, 255, 255")]
            public Color fullColor;

            public AllysCastTimeBar()
            {
                showUI = true;
                //style = (Element_Style.Type)0;
                side = (Element_Side.Type)1;
                //flip = false;
                showTime = 30;
                fadeTime = 10;
                opacity = DefaultOpacity * DefaultOpacity;
                scale = 1f;
                offset = new Vector2(100f, 50f);
                cancelColor = new Color(0, 0, 0, 0);
                fullColor = new Color(255, 255, 255, 255);
            }

            public override bool Equals(object obj)
            {
                if (obj is CNCConfigElement.AllysCastTimeBar other)
                {
                    bool[] flags =
                    {
                        this.showUI = other.showUI,
                        this.style == other.style,
                        this.side == other.side,
                        this.flip == other.flip,
                        this.showTime == other.showTime,
                        this.fadeTime == other.fadeTime,
                        this.opacity == other.opacity,
                        this.scale == other.scale,
                        this.offset == other.offset,
                        this.cancelColor == other.cancelColor,
                    };
                    bool flag = true;
                    for (int i = 0; i < flags.Length && flag; i++) flag = flags[i];
                    return flag;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
                => new
                {
                    showUI,
                    style,
                    side,
                    flip,
                    showTime,
                    fadeTime,
                    opacity,
                    scale,
                    offset,
                    cancelColor,
                }.GetHashCode();

            public class MyCastTimeBar : AllysCastTimeBar
            {
                [Label("$Mods.CNC.Config.CastTimeBar_Allys")]
                [Tooltip("$Mods.CNC.Config.CastTimeBar_Allys_Tooltip")]
                [SeparatePage]
                public AllysCastTimeBar allysBar;

                public MyCastTimeBar()
                {
                    allysBar = new AllysCastTimeBar();
                    showTime = 60;
                    fadeTime = 20;
                    opacity = DefaultOpacity;
                    offset = new Vector2(100f, 100f);
                    cancelColor = new Color(255, 0, 0, 255);
                    fullColor = new Color(0, 255, 255, 255);
                }

                public override bool Equals(object obj)
                {
                    if (obj is MyCastTimeBar other) return this.allysBar == other.allysBar && base.Equals(obj);
                    return base.Equals(obj);
                }

                public override int GetHashCode()
                    => new
                    {
                        showUI,
                        style,
                        side,
                        flip,
                        showTime,
                        fadeTime,
                        opacity,
                        scale,
                        offset,
                        cancelColor,
                        allysBar,
                    }.GetHashCode();
            }

            public class Element_Style : GenericConfigElement
            {
                [CustomModConfigItem(typeof(Element_Style))]
                public enum Type
                {
                    Simple,
                    Fancy,
                }

                protected override string[] ValueString => new string[]
                {
                    "CastTimeBar_Style_Simple",
                    "CastTimeBar_Style_Fancy",
                };

                public override void Click(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().NextEnum());
                public override void RightClick(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().PreviousEnum());
            }

            public class Element_Side : GenericConfigElement
            {
                [CustomModConfigItem(typeof(Element_Side))]
                public enum Type
                {
                    Top,
                    Bottom,
                    Left,
                    Right,
                    Front,
                    Back,
                }

                protected override string[] ValueString => new string[]
                {
                    "CastTimeBar_Side_Top",
                    "CastTimeBar_Side_Bottom",
                    "CastTimeBar_Side_Left",
                    "CastTimeBar_Side_Right",
                    "CastTimeBar_Side_Front",
                    "CastTimeBar_Side_Back",
                };

                public override void Click(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().NextEnum());
                public override void RightClick(UIMouseEvent evt) => SetValue<Type>(GetValue<Type>().PreviousEnum());
            }
        }

        public abstract class GenericConfigElement : ConfigElement
        {
            protected virtual string[] ValueString { get; set; } = null;
            protected void SetValue<T>(T value) => SetObject(value);
            protected T GetValue<T>() => (T)GetObject();
            private string GetValueString() => CNCLocalization.GetText($"Config.{ValueString[GetValue<int>()]}");

            public sealed override void OnBind()
            {
                if (ValueString == null || ValueString.Length <= 0) return;
                base.OnBind();
                TextDisplayFunction = () => this.MemberInfo.Name + GetValueString();
                if (this.LabelAttribute != null) TextDisplayFunction = () => this.LabelAttribute.Label + GetValueString();
            }
        }
    }
}
