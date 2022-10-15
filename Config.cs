using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using CNC.Common.Localization;
using CNC.Content.UI.MainBar;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using Newtonsoft.Json.Linq;

namespace CNC
{
    public class Options
    {
        public static bool useModMusic;
        public static CNCConfigElement.CollapseBar.EdgeStyle closeEdge = 0;
        public static CNCConfigElement.HoverTextBox.BoxStyle[] hoverTextBox = new CNCConfigElement.HoverTextBox.BoxStyle[4];
        public static float[] hoverTextBoxAlpha = new float[5];
        public static float offset = 0;
        public static float movementSpeed = 5;
        public static CNCConfigElement.CastTimeBar CastTimeBar = new CNCConfigElement.CastTimeBar();
    }

    [Label("$Mods.CNC.Config.Client")]
    public class ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("$Mods.CNC.Config.UseModMusic")]
        [Tooltip("$Mods.CNC.Config.OnlyFor")]
        [DefaultValue(true)]
        public bool useModMusic = true;

        [Label("$Mods.CNC.Config.HoverTextBox")]
        public CNCConfigElement.HoverTextBox hoverTextBox = new CNCConfigElement.HoverTextBox();

        [Label("$Mods.CNC.Config.CollapseBar")]
        public CNCConfigElement.CollapseBar collapseBar = new CNCConfigElement.CollapseBar(0, 0f, 5f);

        [Label("$Mods.CNC.Config.CastTimeBar_Title")]
        public CNCConfigElement.CastTimeBar castTimeBar = new CNCConfigElement.CastTimeBar();

        public override void OnChanged()
        {
            Options.useModMusic = useModMusic;
            Options.hoverTextBox[0] = hoverTextBox.boxStyle1;
            Options.hoverTextBox[1] = hoverTextBox.boxStyle2;
            Options.hoverTextBox[2] = hoverTextBox.boxStyle3;
            Options.hoverTextBox[3] = hoverTextBox.boxStyle4;
            Options.hoverTextBoxAlpha[0] = hoverTextBox.alpha1;
            Options.hoverTextBoxAlpha[1] = hoverTextBox.alpha2;
            Options.hoverTextBoxAlpha[2] = hoverTextBox.alpha3;
            Options.hoverTextBoxAlpha[3] = hoverTextBox.alpha4;
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
            [Header("$Mods.CNC.Config.HoverTextBoxType1")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(1)]
            public BoxStyle boxStyle1;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float alpha1;

            [Header("$Mods.CNC.Config.HoverTextBoxType2")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(2)]
            public BoxStyle boxStyle2;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float alpha2;

            [Header("$Mods.CNC.Config.HoverTextBoxType3")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(2)]
            public BoxStyle boxStyle3;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float alpha3;

            [Header("$Mods.CNC.Config.HoverTextBoxType4")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(2)]
            public BoxStyle boxStyle4;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float alpha4;

            public override string ToString() => "";

            public HoverTextBox()
            {
                boxStyle1 = (BoxStyle)1;
                boxStyle2 = (BoxStyle)2;
                boxStyle3 = (BoxStyle)2;
                boxStyle4 = (BoxStyle)2;
                alpha1 = DefaultOpacity;
                alpha2 = DefaultOpacity;
                alpha3 = DefaultOpacity;
                alpha4 = DefaultOpacity;
            }

            public override bool Equals(object obj)
            {
                if (obj is HoverTextBox other)
                {
                    bool[] flags =
                    {
                    boxStyle1 == other.boxStyle1,
                    boxStyle2 == other.boxStyle2,
                    boxStyle3 == other.boxStyle3,
                    boxStyle4 == other.boxStyle4,
                    alpha1 == other.alpha1,
                    alpha2 == other.alpha2,
                    alpha3 == other.alpha3,
                    alpha4 == other.alpha4,
                };
                    bool flag = true;
                    for (int i = 0; i < flags.Length && flag; i++) flag = flags[i];
                    return flag;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new
                {
                    boxStyle1,
                    boxStyle2,
                    boxStyle3,
                    boxStyle4,
                    alpha1,
                    alpha2,
                    alpha3,
                    alpha4,
                }.GetHashCode();
            }

            [CustomModConfigItem(typeof(HoverTextBoxElement))]
            public enum BoxStyle
            {
                None,
                Origin,
                Brown,
            }

            class HoverTextBoxElement : ConfigElement
            {
                string[] valueStrings;

                void SetValue(BoxStyle value) => SetObject(value);

                BoxStyle GetValue() => (BoxStyle)GetObject();

                string GetStringValue() => valueStrings[(int)GetValue()];

                public override void OnBind()
                {
                    base.OnBind();
                    valueStrings = new string[]
                    {
                        CNCLocalization.GetText("Config.HoverTextBoxStyleNone"),
                        CNCLocalization.GetText("Config.HoverTextBoxStyleOrigin"),
                        CNCLocalization.GetText("Config.HoverTextBoxStyleBrown"),
                    };
                    TextDisplayFunction = () => this.MemberInfo.Name + GetStringValue();
                    if (this.LabelAttribute != null) TextDisplayFunction = () => this.LabelAttribute.Label + GetStringValue();
                }

                public override void Click(UIMouseEvent evt)
                {
                    base.Click(evt);
                    SetValue(GetValue().NextEnum());
                }

                public override void RightClick(UIMouseEvent evt)
                {
                    base.RightClick(evt);
                    SetValue(GetValue().PreviousEnum());
                }
            }
        }

        [BackgroundColor(87, 77, 91)]
        public class CollapseBar
        {
            [BackgroundColor(57, 49, 65)]
            [Label("$Mods.CNC.Config.CloseEdge")]
            [DefaultValue(0)]
            public EdgeStyle closeEdge;

            [Slider]
            [BackgroundColor(57, 49, 65)]
            [Label("$Mods.CNC.Config.Offset")]
            [DefaultValue(0)]
            [Range(-1f, 1f)]
            public float offset;

            [BackgroundColor(57, 49, 65)]
            [Label("$Mods.CNC.Config.MovementSpeed")]
            [DefaultValue(5f)]
            [Range(2f, 20f)]
            public float movementSpeed;

            public override string ToString() => "";

            public CollapseBar() { closeEdge = 0; offset = 0; movementSpeed = 4f; }
            public CollapseBar(int type, float value, float spd) { closeEdge = (EdgeStyle)type; offset = value; movementSpeed = spd; }

            public override bool Equals(object obj)
            {
                if (obj is CollapseBar other) return closeEdge == other.closeEdge && offset == other.offset && movementSpeed == other.movementSpeed;
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { closeEdge, offset, movementSpeed }.GetHashCode();
            }

            [CustomModConfigItem(typeof(EdgeElement))]
            public enum EdgeStyle
            {
                Bottom,
                Left,
                Top,
                Right,
            }

            class EdgeElement : ConfigElement
            {
                string[] valueStrings;

                void SetValue(EdgeStyle value) => SetObject(value);

                EdgeStyle GetValue() => (EdgeStyle)GetObject();

                string GetStringValue() => valueStrings[(int)GetValue()];

                public override void OnBind()
                {
                    base.OnBind();
                    valueStrings = new string[]
                    {
                        CNCLocalization.GetText("Config.BottomEdge"),
                        CNCLocalization.GetText("Config.LeftEdge"),
                        CNCLocalization.GetText("Config.TopEdge"),
                        CNCLocalization.GetText("Config.RightEdge"),
                    };
                    TextDisplayFunction = () => this.MemberInfo.Name + GetStringValue();
                    if (this.LabelAttribute != null) TextDisplayFunction = () => this.LabelAttribute.Label + GetStringValue();
                }

                public override void Click(UIMouseEvent evt)
                {
                    base.Click(evt);
                    SetValue(GetValue().NextEnum());
                }

                public override void RightClick(UIMouseEvent evt)
                {
                    base.RightClick(evt);
                    SetValue(GetValue().PreviousEnum());
                }
            }
        }

        public class CastTimeBar
        {
            [Label("$Mods.CNC.Config.CastTimeBar_Style")]
            [DefaultValue(1)]
            public Element_Style.Type style => Element_Style.Type.Fancy;

            [Label("$Mods.CNC.Config.CastTimeBar_Side")]
            [DefaultValue(1)]
            public Element_Side.Type side;

            [Label("$Mods.CNC.Config.CastTimeBar_Flip")]
            [DefaultValue(false)]
            public bool flip => false;

            [Label("$Mods.CNC.Config.CastTimeBar_Offset")]
            [DefaultValue(typeof(Vector2), "100, 100")]
            [Range(50f, 150f)]
            public Vector2 offset;

            [Label("$Mods.CNC.Config.CastTimeBar_Scale")]
            [DefaultValue(1f)]
            [Range(0.75f, 1.25f)]
            public float scale;

            [Label("$Mods.CNC.Config.CastTimeBar_Opacity")]
            [DefaultValue(DefaultOpacity)]
            [Range(MinOpacity, MaxOpacity)]
            public float opacity;

            [Label("$Mods.CNC.Config.CastTimeBar_CancelColor")]
            [DefaultValue(typeof(Color), "255, 0, 0, 255")]
            public Color cancelColor;

            [Label("$Mods.CNC.Config.CastTimeBar_Allys")]
            public AllysCastTimeBar allysBar;

            public CastTimeBar()
            {
                //style = (Element_Style.Type)1;
                side = (Element_Side.Type)1;
                //flip = false;
                offset = new Vector2(100f, 100f);
                scale = 1f;
                opacity = DefaultOpacity;
                cancelColor = new Color(255, 0, 0, 255);
                allysBar = new AllysCastTimeBar();
            }

            public override bool Equals(object obj)
            {
                if (obj is CastTimeBar other)
                {
                    bool[] flags =
                    {
                        this.style == other.style,
                        this.side == other.side,
                        this.flip == other.flip,
                        this.offset == other.offset,
                        this.scale == other.scale,
                        this.opacity == other.opacity,
                        this.cancelColor == other.cancelColor,
                        this.allysBar == other.allysBar,
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
                    style,
                    side,
                    flip,
                    offset,
                    scale,
                    opacity,
                    cancelColor,
                    allysBar,
                }.GetHashCode();

            public override string ToString() => "";

            public class AllysCastTimeBar
            {
                [Label("$Mods.CNC.Config.CastTimeBar_ShowAllys")]
                [DefaultValue(true)]
                public bool showAllys;

                [Label("$Mods.CNC.Config.CastTimeBar_Style")]
                [DefaultValue(0)]
                public Element_Style.Type style;

                [Label("$Mods.CNC.Config.CastTimeBar_Side")]
                [DefaultValue(1)]
                public Element_Side.Type side;

                [Label("$Mods.CNC.Config.CastTimeBar_Flip")]
                [DefaultValue(false)]
                public bool flip;

                [Label("$Mods.CNC.Config.CastTimeBar_Offset")]
                [DefaultValue(typeof(Vector2), "100, 50")]
                [Range(50f, 150f)]
                public Vector2 offset;

                [Label("$Mods.CNC.Config.CastTimeBar_Scale")]
                [DefaultValue(1f)]
                [Range(0.75f, 1.25f)]
                public float scale;

                [Label("$Mods.CNC.Config.CastTimeBar_Opacity")]
                [DefaultValue(DefaultOpacity * DefaultOpacity)]
                [Range(MinOpacity, MaxOpacity)]
                public float opacity;

                [Label("$Mods.CNC.Config.CastTimeBar_CancelColor")]
                [DefaultValue(typeof(Color), "192, 0, 0, 255")]
                public Color cancelColor;

                public AllysCastTimeBar()
                {
                    showAllys = true;
                    style = (Element_Style.Type)0;
                    side = (Element_Side.Type)1;
                    flip = false;
                    offset = new Vector2(100f, 50f);
                    scale = 1f;
                    opacity = DefaultOpacity * DefaultOpacity;
                    cancelColor = new Color(0, 0, 0, 0);
                }

                public override bool Equals(object obj)
                {
                    if (obj is AllysCastTimeBar other)
                    {
                        bool[] flags =
                        {
                            this.style == other.style,
                            this.side == other.side,
                            this.flip == other.flip,
                            this.offset == other.offset,
                            this.scale == other.scale,
                            this.opacity == other.opacity,
                            this.cancelColor == other.cancelColor,
                            this.showAllys == other.showAllys,
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
                        style,
                        side,
                        flip,
                        offset,
                        scale,
                        opacity,
                        cancelColor,
                        showAllys,
                    }.GetHashCode();

                public override string ToString() => "";
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
