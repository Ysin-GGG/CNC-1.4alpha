
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using CNC.Common.Localization;
using System.Collections.ObjectModel;

namespace CNC.Common.Global.Items
{
    public class AllItem : GlobalItem
    {
        public byte clazz;
        public Mod mod => Mod;

        public AllItem()
        {
            clazz = 0;
        }

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            AllItem clone = (AllItem)base.Clone(item, itemClone);
            if (clone != null)
            {
                clone.clazz = clazz;
            }
            return clone;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(item, tooltips);
            Dictionary<byte, string> ClazzID = Utils.CNCID.Clazz.Collection;
            Dictionary<int, Color> ClazzColor = new Dictionary<int, Color>()
            {
                { 11, new Color(168, 61, 51,255) },
                { 12, new Color( 46,109,153,255) },
                { 13, new Color(219,173, 89,255) },
                { 21, new Color( 57,153, 46,255) },
                { 22, new Color(126,229,218,255) },
                { 23, new Color(109, 81, 57,255) },
                { 31, new Color(232,232, 11,255) },
                { 32, new Color(112, 45,179,255) },
                { 33, new Color(126,161,229,255) },
                { 41, new Color(188,242,170,255) },
                { 42, new Color(122, 36, 70,255) },
                { 43, new Color( 76, 46, 77,255) },
                { 51, new Color(255,255,255,255) },
                { 52, new Color( 31, 32, 38,255) },
            };
            AllItem ai = item.GetGlobalItem<AllItem>();
            if (ai.clazz > 0)
            {
                string Class = CNCLocalization.GetText($"Class.{ClazzID[ai.clazz]}");
                TooltipLine tt1 = new TooltipLine(mod, item.Name, CNCLocalization.GetText("Class.Format", Class)) { OverrideColor = ClazzColor[ai.clazz] };
                tooltips.Add(tt1);
            }
        }
    }
}