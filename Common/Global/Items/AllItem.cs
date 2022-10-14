
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
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Content;
using System.Collections.ObjectModel;

namespace CNC.Common.Global.Items
{
    public class AllItem : GlobalItem
    {
        public byte clazz;
        public int comboCur;
        public int comboNext;
        public int comboCount;
        public int comboTime;
        public int comboDelay;
        public int castTimeMax;
        public int castTimeMax2;
        public Mod mod => Mod;

        public AllItem()
        {
            this.clazz = 0;
            this.comboCur = 0;
            this.comboNext = 0;
            this.comboCount = 1;
            this.comboTime = 0;
            this.comboDelay = 0;
        }

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            AllItem clone = (AllItem)base.Clone(item, itemClone);
            if (clone != null) clone.clazz = clazz;
            return clone;
        }

        public override void UpdateInventory(Item item, Player player)
        {
            AllItem modItem = item.GetGlobalItem<AllItem>();
            if (player.itemTime <= 0) modItem.comboTime = MyMath.Max(0, modItem.comboTime - 1);
            if (player.itemTime <= 1) modItem.comboCur = modItem.comboNext;
            if (modItem.comboTime <= 0) modItem.comboCur = 0;
        }

        /// <summary> Ö°Òµ±àºÅ </summary>
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(item, tooltips);
            Dictionary<byte, string> ClazzID = CNCID.Clazz.Collection;
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
            if (EnableAnatta(CNC.Me))
            {
                bool flag = true;
                Color color = Color.White;
                switch (item.type)
                {
                    case ItemID.TrueNightsEdge:
                        color = Color.Violet;
                        break;
                    case ItemID.Meowmere:
                        color = Color.LightPink;
                        break;
                    case ItemID.FieryGreatsword:
                        color = new Color(242, 92, 34);
                        break;
                    case ItemID.TrueExcalibur:
                        color = Color.LightGoldenrodYellow;
                        break;
                    default:
                        flag = false;
                        break;
                }
                TooltipLine tt2 = new TooltipLine(mod, item.Name, CNCLocalization.GetText("Misc.AnattaPower")) { OverrideColor = color.Flash() };
                if (flag) tooltips.Add(tt2);
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (EnableAnatta(player))
            {
                bool reset = true;
                switch (item.type)
                {
                    case ItemID.TrueNightsEdge:
                        comboCount = 3;
                        comboDelay = 15;
                        break;
                    case ItemID.TrueExcalibur:
                        comboCount = 5;
                        comboDelay = 10;
                        break;
                    case ItemID.FieryGreatsword:
                        item.shoot = 1;
                        comboCount = 4;
                        comboDelay = 10;
                        break;
                    case ItemID.Meowmere:
                        break;
                    default:
                        reset = false;
                        break;
                }
                if (reset)
                {
                    item.noMelee = true;
                    if (player.All().controlMouseRight) return false;
                }
            }
            else
            {
                Item clone = new Item(item.type);
                item.noMelee = clone.noMelee;
                item.shoot = clone.shoot;
            }
            return base.CanUseItem(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            bool? flag = base.UseItem(item, player);
            int time = this.comboDelay;
            this.comboNext = (this.comboCur + 1) % comboCount;
            this.comboTime = time;
            return flag;
        }

        public override void HoldItem(Item item, Player player)
        {
            if (EnableAnatta(player))
            {
                bool control = player.All().controlMouseRight;
                bool release = player.All().releaseMouseRight;
                float ct = player.All().castTime;
                float ctMax = 100;
                float damage = player.GetWeaponDamage(item);
                float knockback = player.GetWeaponKnockback(item);
                float crit = player.GetWeaponCrit(item);
                float type = 0f;
                IEntitySource source = item.GetSource_ItemUse(item);
                Vector2 position = player.Center;
                Vector2 velocity = new Vector2(item.shootSpeed, 0f).RotatedBy((Main.MouseWorld - player.Center).ToRotation());
                Color mainColor = Color.White;
                switch (item.type)
                {
                    case ItemID.Meowmere:
                        mainColor = CNCID.Colors.Expert;
                        if (control)
                        {
                            Vector2 vel = new Vector2(CNC.Random.NextFloat(4f) + MyMath.Lerp(12f, 0f, ct / ctMax), 0f).RotatedBy(CNC.Random.NextDegree());
                            int idx = Particle.NewParticle(
                                CNC.GetTexture("Assets/Particles/Star1"),
                                player.Center + vel * 10f,
                                vel * -.5f,
                                CNC.Random.NextDegree(),
                                30,
                                Vector2.One * 1.5f,
                                mainColor.NewA(0),
                                mainColor.NewA(3)
                                );
                            ParticleManager.particle[idx].velocityMul = Vector2.One * .95f;
                            ParticleManager.particle[idx].rotationAdd = CNC.Random.NextBool().ToDirectionInt(16);
                            ParticleManager.particle[idx].scaleAdd = new Vector2(-.05f);
                            ParticleManager.particle[idx].alphaAdd = -.033f;
                            ParticleManager.particle[idx].alpha = 1f;
                        }
                        if (release && ct >= ctMax)
                        {
                            CNCUtils.NewProj(source, position, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.SolarHalo], (int)damage, 0f, (int)crit, player.whoAmI, 0f, 1f, 0, 0);
                            damage *= 3;
                            knockback *= 3f;
                            crit += 20f;
                            type = 2f;
                        }
                        break;
                    case ItemID.FieryGreatsword:
                        ctMax = 60;
                        mainColor = new Color(242, 92, 34);
                        if (control)
                        {
                            if (ct < ctMax)
                            {
                                Vector2 vel = new Vector2(CNC.Random.NextFloat(4f) + MyMath.Lerp(12f, 0f, ct / ctMax), 0f).RotatedBy(CNC.Random.NextDegree());
                                int idx = Particle.NewParticle(
                                    CNC.GetTexture("Assets/Particles/Light1"),
                                    player.Center + vel * 10f,
                                    vel * -.5f,
                                    CNC.Random.NextDegree(),
                                    30,
                                    new Vector2(Main.rand.NextFloat(.03f, .09f)),
                                    mainColor.NewA(0),
                                    mainColor.NewA(15)
                                    );
                                ParticleManager.particle[idx].velocityMul = Vector2.One * .95f;
                                ParticleManager.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0003f, .0010f));
                                ParticleManager.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
                            }
                            else
                            {
                                Vector2 vel = CNC.Random.NextVector2(-4f, -4f, 4f, -2f);
                                int idx = Particle.NewParticle(
                                    CNC.GetTexture("Assets/Particles/Light1"),
                                    player.gravDir == 1 ? player.Bottom : player.Top,
                                    vel * new Vector2(1f, player.gravDir),
                                    0,
                                    60,
                                    new Vector2(Main.rand.NextFloat(.03f, .09f)),
                                    mainColor.NewA(0),
                                    mainColor.NewA(15)
                                    );
                                ParticleManager.particle[idx].velocityMul = new Vector2(.9f, .975f);
                                ParticleManager.particle[idx].scaleAdd = -new Vector2(Main.rand.NextFloat(.0003f, .0010f));
                                ParticleManager.particle[idx].alphaAdd = (Main.rand.Next(5) + 1) * -.01f;
                            }
                            if (release && ct >= ctMax)
                            {
                                velocity = new Vector2((Main.MouseWorld.X > player.Center.X).ToDirectionInt(1), 0f);
                                damage *= 1.5f;
                                knockback *= 1.5f;
                                crit += 5f;
                                type = 4f;
                            }
                        }
                        break;
                    case ItemID.TrueExcalibur:
                        mainColor = Color.LightGoldenrodYellow;
                        ctMax = 240;
                        if (control)
                        {
                            if (ct < ctMax)
                            {
                                if (ct % 20 == 10)
                                {
                                    for (int i = 0; i < 18; i++)
                                    {
                                        float amount = i / 18f;
                                        Vector2 vel = new Vector2(MyMath.Lerp(8f, 2f, ct / ctMax), 0f).RotatedBy(amount * MyMath.pi2);
                                        int idx = Particle.NewParticle(
                                            CNC.GetTexture("Assets/Particles/Star1"),
                                            player.Center + vel * 8f,
                                            -vel,
                                            CNC.Random.NextDegree(),
                                            30,
                                            Vector2.One * MyMath.Lerp(1f, .5f, ct / ctMax),
                                            mainColor.NewA(0),
                                            mainColor.NewA(3)
                                            );
                                        ParticleManager.particle[idx].velocityMul = Vector2.One * .95f;
                                        ParticleManager.particle[idx].rotationAdd = CNC.Random.NextBool().ToDirectionInt(16);
                                        ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                                        ParticleManager.particle[idx].alphaAdd = -.04f;
                                        ParticleManager.particle[idx].alpha = 1f;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    float rot = i * MyMath.deg90;
                                    Vector2 vel = new Vector2(1f, 0f).RotatedBy(rot);
                                    vel.X *= MyMath.phi;
                                    int idx = Particle.NewParticle(
                                        CNC.GetTexture("Assets/Particles/Star1"),
                                        player.Center,
                                        vel * 3f,
                                        rot + (ct - ctMax) * MyMath.deg01,
                                        30,
                                        Vector2.One * .75f,
                                        mainColor.NewA(0),
                                        mainColor.NewA(3)
                                        );
                                    ParticleManager.particle[idx].velocityMul = new Vector2(.98f);
                                    ParticleManager.particle[idx].scaleAdd = new Vector2(-.05f);
                                    ParticleManager.particle[idx].alphaAdd = -.033f;
                                    ParticleManager.particle[idx].alpha = 1f;
                                }
                            }
                        }
                        if (release && ct >= ctMax)
                        {
                            velocity = new Vector2((Main.MouseWorld.X > player.Center.X).ToDirectionInt(1), 0f);
                            damage *= 3;
                            knockback *= 1.5f;
                            crit += 20f;
                            type = 6f;
                            CNCUtils.NewProj(source, position, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.PhantomExcalibur], 0, 0f, 0, player.whoAmI, 0f, 1f, 0, 0);
                        }
                        break;
                    case ItemID.TrueNightsEdge:
                        mainColor = Color.Lerp(new Color(96, 0, 192), new Color(48, 0, 112), .5f);
                        ctMax = 240;
                        if (control)
                        {
                            if (ct < ctMax)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    float rot = i * MyMath.deg90 + MyMath.deg01 * 3f * ct;
                                    Vector2 vel = new Vector2(1f, 0f).RotatedBy(rot);
                                    vel.X *= MyMath.phi;
                                    int idx = Particle.NewParticle(
                                        CNC.GetTexture("Assets/Particles/Star1"),
                                        player.Center,
                                        vel * MyMath.Lerp(6f, 1f, ct / ctMax),
                                        rot + (ct - ctMax) * MyMath.deg01,
                                        30,
                                        Vector2.One * .75f,
                                        mainColor.NewA(0),
                                        mainColor.NewA(3)
                                        );
                                    ParticleManager.particle[idx].velocityMul = new Vector2(.98f);
                                    ParticleManager.particle[idx].scaleAdd = new Vector2(-.05f);
                                    ParticleManager.particle[idx].alphaAdd = -.033f;
                                    ParticleManager.particle[idx].alpha = 1f;
                                }
                            }
                            else
                            {
                                if (ct % 20 == 0)
                                {
                                    for (int i = 0; i < 36; i++)
                                    {
                                        float amount = i / 36f;
                                        Vector2 vel = new Vector2(2f, 0f).RotatedBy(amount * MyMath.pi2);
                                        int idx = Particle.NewParticle(
                                            CNC.GetTexture("Assets/Particles/Star1"),
                                            player.Center,
                                            vel + player.velocity * .2f,
                                            CNC.Random.NextDegree(),
                                            30,
                                            Vector2.One,
                                            mainColor.NewA(0),
                                            mainColor.NewA(3)
                                            );
                                        ParticleManager.particle[idx].velocityMul = Vector2.One * .95f;
                                        ParticleManager.particle[idx].rotationAdd = CNC.Random.NextBool().ToDirectionInt(16);
                                        ParticleManager.particle[idx].scaleAdd = new Vector2(-.025f);
                                        ParticleManager.particle[idx].alphaAdd = .04f;
                                        ParticleManager.particle[idx].alpha = 0f;
                                    }
                                }
                            }
                        }
                        if (release && ct >= ctMax)
                        {
                            CNCUtils.NewProj(source, position + velocity * 24f, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.VoidNightsEdge], (int)(damage * 15f), 0f, (int)crit, player.whoAmI, 0f, 1f, 0, 0);
                            damage *= 3;
                            knockback *= 1.5f;
                            crit += 20f;
                            type = 8f;
                        }
                        break;
                }
                if (control && ct == ctMax) CNCUtils.PlaySound(SoundID.Item4, player);
                if (release && ct >= ctMax)
                {
                    CNCUtils.NewProj(source, position, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.AnattaSlash], (int)damage, knockback, (int)crit, player.whoAmI, 0f, 1f, type, 0);
                    this.comboCur = 0;
                    this.comboNext = 0;
                    player.direction = (velocity.X < 0).ToDirectionInt(-1);
                    ApplyUse(player, item, item.useAnimation);
                }
            }
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (EnableAnatta(player))
            {
                switch (item.type)
                {
                    case ItemID.Meowmere:
                        break;
                    case ItemID.FieryGreatsword:
                        break;
                    case ItemID.TrueExcalibur:
                        velocity *= 2f;
                        break;
                    case ItemID.TrueNightsEdge:
                        velocity *= this.comboCur + 1f;
                        knockback += this.comboCur + 1f;
                        break;
                }
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool shoot = base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            type = CNCID.Projectile.LegacyObject[CNCID.Projectile.AnattaSlash];
            velocity = new Vector2(1f, 0f).RotatedBy((Main.MouseWorld - player.Center).ToRotation());
            float dmg = player.GetWeaponDamage(item);
            float kb = player.GetWeaponKnockback(item);
            float crit = player.GetWeaponCrit(item);
            if (EnableAnatta(player))
            {
                int ti = 0, ta = 0;
                float ai0 = 0f;
                switch (item.type)
                {
                    case ItemID.Meowmere:
                        ti = item.useTime;
                        ta = item.useAnimation;
                        ai0 = 1f;
                        break;
                    case ItemID.FieryGreatsword:
                        ta = ti = 30;
                        dmg *= new float[] { .9f, .95f, 1.05f, 1.2f }[this.comboCur];
                        ai0 = 3f + this.comboCur * .1f;
                        break;
                    case ItemID.TrueExcalibur:
                        ta = ti = new int[] { 30, 40, 10, 20, 40 }[comboCur];
                        dmg *= new float[] { 1.1f, .95f, 1.5f }[this.comboCur / 2];
                        ai0 = 5f + this.comboCur * .1f;
                        if (this.comboCur < 2) shoot = false;
                        break;
                    case ItemID.TrueNightsEdge:
                        ta = ti = new int[] { 32, 44, 50 }[comboCur];
                        dmg *= new float[] { 1.05f, 1.2f, 1.3f }[this.comboCur];
                        ai0 = 7f + this.comboCur * .1f;
                        if (this.comboCur < 1) shoot = false;
                        break;
                }
                if (ti > 0 && ta > 0)
                {
                    player.direction = (velocity.X < 0).ToDirectionInt(-1);
                    player.itemTimeMax = player.itemTime = (int)(ti / player.GetAttackSpeed(DamageClass.Melee));
                    player.itemAnimationMax = player.itemAnimation = (int)(ta / player.GetAttackSpeed(DamageClass.Melee));
                    (CNCUtils.NewProj(source, position, velocity, type, (int)dmg, kb, (int)crit, player.whoAmI, 0f, 1f, ai0, 0f).ModProjectile as Content.Projectiles.ShaderSlash).scale *= player.GetAdjustedItemScale(item);
                }
            }
            switch (item.type)
            {
                case ItemID.FieryGreatsword:
                    shoot = false;
                    break;
            }
            return shoot;
        }

        private bool EnableAnatta(Player player) => player.IsMe() && player.HasItem(ModContent.ItemType<Content.Items.Developers.Quartz.Anatta>());// && player.GetModPlayer<Players.AllPlayer>().EquippedQuartzHead;
        private void ApplyUse(Player player, Item item, int time) => player.itemAnimationMax = player.itemAnimation = player.itemTimeMax = player.itemTime = (int)(time / player.GetWeaponAttackSpeed(item));
    }
}