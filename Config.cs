using System;
using System.ComponentModel;
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
        public static float[] hoverTextBoxAlpha = new float[4];
        public static float offset = 0;
        public static float movementSpeed = 5;
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
        public class HoverTextBox
        {
            [Header("$Mods.CNC.Config.HoverTextBoxType1")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(1)]
            public BoxStyle boxStyle1;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(.785f)]
            [Range(.2f, 1f)]
            public float alpha1;

            [Header("$Mods.CNC.Config.HoverTextBoxType2")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(2)]
            public BoxStyle boxStyle2;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(.785f)]
            [Range(.2f, 1f)]
            public float alpha2;

            [Header("$Mods.CNC.Config.HoverTextBoxType3")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(2)]
            public BoxStyle boxStyle3;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(.785f)]
            [Range(.2f, 1f)]
            public float alpha3;

            [Header("$Mods.CNC.Config.HoverTextBoxType4")]
            [Label("$Mods.CNC.Config.HoverTextBoxStyle")]
            [DefaultValue(2)]
            public BoxStyle boxStyle4;
            [Label("$Mods.CNC.Config.HoverTextBoxAlpha")]
            [DefaultValue(.785f)]
            [Range(.2f, 1f)]
            public float alpha4;

            public override string ToString() => "";

            public HoverTextBox()
            {
                boxStyle1 = (BoxStyle)1;
                boxStyle2 = (BoxStyle)2;
                boxStyle3 = (BoxStyle)2;
                boxStyle4 = (BoxStyle)2;
                alpha1 = .785f;
                alpha2 = .785f;
                alpha3 = .785f;
                alpha4 = .785f;
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
                    TextDisplayFunction = () => memberInfo.Name + GetStringValue();
                    if (labelAttribute != null) TextDisplayFunction = () => labelAttribute.Label + GetStringValue();
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
                    TextDisplayFunction = () => memberInfo.Name + GetStringValue();
                    if (labelAttribute != null) TextDisplayFunction = () => labelAttribute.Label + GetStringValue();
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
    }
}
