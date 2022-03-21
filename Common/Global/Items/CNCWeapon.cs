using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using CNC.Common.Utils;
using CNC.Common.Localization;

namespace CNC.Common.Global.Items
{
    public abstract class CNCWeapon : CNCItem
    {
        public sealed override void ResetDefaults() => this.NewDefaults();
        public sealed override void ResetStaticDefaults() => this.NewStaticDefaults();
        public virtual void NewDefaults() { }
        public virtual void NewStaticDefaults() { }

        public abstract class Melee : CNCWeapon
        {
            public sealed override void NewDefaults() => this.SetDefaults();
            public sealed override void NewStaticDefaults() => this.SetStaticDefaults();
            public new virtual void SetDefaults() { }
            public new virtual void SetStaticDefaults() { }

            public abstract class Claymore : Melee
            {
                public sealed override void SetDefaults() => this.ResetDefaults();
                public sealed override void SetStaticDefaults() => this.ResetStaticDefaults();
                public new virtual void ResetDefaults() { }
                public new virtual void ResetStaticDefaults() { }
            }
        }
        public abstract class Ranged : CNCWeapon
        {

        }
        public abstract class Magic : CNCWeapon
        {

        }
        public abstract class Raid : CNCWeapon
        {

        }
    }
}