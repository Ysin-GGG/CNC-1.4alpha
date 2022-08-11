using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CNC.Content.DataStructures;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Projectiles;

namespace CNC.Content.Projectiles.Hostile
{
    public abstract class AnnulusOfPneuma : CNCProjectile
    {
        protected Projectile p => projectile;
        protected const int dustid1 = DustId.TrailingCyan;
        protected const int dustid2 = DustId.CyanShortFx1;
        protected virtual Color mainColor => new Color(178, 214, 231);
        protected virtual Color mainColor2 => new Color(178, 214, 231, 0);
        protected virtual bool update => true;
        protected int attackCD = 0;
        protected bool rage => RootNPC != null && RootNPC.type == CNCID.NPC.LegacyObject[CNCID.NPC.SpiritBoss] && ((NPCs.SpiritBoss.SpiritBoss)RootNPC.ModNPC).rage;
        private float rand;

        public override void ResetDefaults()
        {
            projectile.timeLeft = 360;
            projectile.alpha = 0;
            projectile.light = 0;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {

        }
    }
}