using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework;
using CNC.Common.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace CNC.Content.UI.CastTimeBar
{
    public class CastTimeBar : UIState
    {
        public UIImage IconBox;
        public UIImage Icon;
        public UIImage Filler;
        public UIImage Bar;
        public UIImage Lid;

        public override string Name => "CastTimeBar";

        public override void Initialize()
        {
            Size = Vector2.One * 16f;
            Offset = screen / 2f;
            CanDrag = false;
            CanOutScreen = false;
            Scale = new Vector2(Main.UIScale);
            IconBox = new UIImage();
            IconBox.Size = GetImage("IconBox").Size();
            IconBox.SetImage(GetImage("IconBox"), Color.White);
            Icon = new UIImage();
            Icon.Size = GetImage("Icon1").Size();
            Icon.SetImage(GetImage("Icon1"), Color.White);
            Filler = new UIImage();
            Filler.Size = GetImage("Filler").Size();
            Filler.SetImage(GetImage("Filler"), Color.White);
            Bar = new UIImage();
            Bar.Size = GetImage("Bar").Size();
            Bar.SetImage(GetImage("Bar"), Color.White);
            Lid = new UIImage();
            Lid.Size = GetImage("Lid").Size();
            Lid.SetImage(GetImage("Lid"), Color.White);
            IconBox.Append(Icon);
            children.Add(IconBox);
            children.Add(Filler);
            Bar.Append(Lid);
            children.Add(Bar);
            id = 0;
        }
        
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            IconBox.RightCenter = this.Center;
            Bar.LeftCenter = this.Center;
        }
    }
}