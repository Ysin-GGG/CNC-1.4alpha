using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.Projectiles;

namespace CNC.Common.Global.NPCs
{
    public abstract class CNCNPC : ModNPC
    {
        public NPC RootNPC => npc.GetGlobalNPC<NPCRelation>().RootNPC;
        public NPC ParentNPC => npc.GetGlobalNPC<NPCRelation>().ParentNPC;
        public NPC[] ChildrenNPC => npc.GetGlobalNPC<NPCRelation>().ChildrenNPC;
        public Projectile RootProjectile => npc.GetGlobalNPC<NPCRelation>().RootProjectile;
        public Projectile ParentProjectile => npc.GetGlobalNPC<NPCRelation>().ParentProjectile;
        public Projectile[] ChildrenProjectile => npc.GetGlobalNPC<NPCRelation>().ChildrenProjectile;
        
        public NPC npc => NPC;
        public Mod mod => Mod;
        public Player Target = null;
        public int countFrame { get => Main.npcFrameCount[Type]; set => Main.npcFrameCount[Type] = value; }
        public int attackFrame { get => NPCID.Sets.AttackFrameCount[Type]; set => NPCID.Sets.AttackFrameCount[Type] = value; }
        public int extraFrame { get => NPCID.Sets.ExtraFramesCount[Type]; set => NPCID.Sets.ExtraFramesCount[Type] = value; }
        public IEntitySource source => npc.GetSpawnSource_ForProjectile();

        public float var1 { get => npc.ai[0]; set => npc.ai[0] = value; }
        public float var2 { get => npc.ai[1]; set => npc.ai[1] = value; }
        public float var3 { get => npc.ai[2]; set => npc.ai[2] = value; }
        public float var4 { get => npc.ai[3]; set => npc.ai[3] = value; }
        public float var5 { get => npc.localAI[0]; set => npc.localAI[0] = value; }
        public float var6 { get => npc.localAI[1]; set => npc.localAI[1] = value; }
        public float var7 { get => npc.localAI[2]; set => npc.localAI[2] = value; }
        public float var8 { get => npc.localAI[3]; set => npc.localAI[3] = value; }

        public NPC NewNPC(short CNCtype, int start = 0)
            => NewNPC(npc, (int)npc.Center.X, (int)npc.Center.Y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(int type, int start = 0)
            => NewNPC(npc, (int)npc.Center.X, (int)npc.Center.Y, type, start);

        public NPC NewNPC(Vector2 pos, short CNCtype, int start = 0)
            => NewNPC(npc, (int)pos.X, (int)pos.Y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(Vector2 pos, int type, int start = 0)
            => NewNPC(npc, (int)pos.X, (int)pos.Y, type, start);

        public NPC NewNPC(int x, int y, short CNCtype, int start = 0)
            => NewNPC(npc, x, y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(int x, int y, int type, int start = 0)
            => NewNPC(npc, x, y, type, start);

        public NPC NewNPC(NPC npc, int x, int y, short CNCtype, int start = 0)
            => NewNPC(npc, x, y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(NPC npc, int x, int y, int type, int start = 0)
        {
            int whoAmI = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), x, y, type, start);
            Main.npc[whoAmI].GetGlobalNPC<NPCRelation>().SetParent(npc, true);
            Main.npc[whoAmI].GetGlobalNPC<NPCRelation>().SetRoot(npc, true);
            if (Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().ChildrenNPC != null)
                Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().AddChild(Main.npc[whoAmI]);
            return Main.npc[whoAmI];
        }

        public Projectile NewProjectile(float vel, float rot, int type)
            => NewProjectile(npc, npc.Center, new Vector2(vel), type, npc.damage);

        public Projectile NewProjectile(float vel, float rot, int type, float damage)
            => NewProjectile(npc, npc.Center, MyMath.GetXY(rot) * vel, type, damage);

        public Projectile NewProjectile(Vector2 vel, int type, float damage)
            => NewProjectile(npc, npc.Center, vel, type, damage);

        public Projectile NewProjectile(Vector2 pos, Vector2 vel, int type, float damage)
            => NewProjectile(npc, pos, vel, type, damage);

        public Projectile NewProjectile(NPC npc, Vector2 pos, Vector2 vel, int type, float damage)
        {
            int whoAmI = Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), pos, vel, type, (int)damage, 0, Main.myPlayer);
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetParent(npc, true);
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetRoot(npc, true);
            if (Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().ChildrenProjectile != null)
                Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().AddChild(Main.projectile[whoAmI]);
            return Main.projectile[whoAmI];
        }

        public sealed override void SetStaticDefaults()
        {
            countFrame = 1;
            attackFrame = 0;
            extraFrame = 0;
            DisplayName.SetDefault(CNCLocalization.GetText($"NPC.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"NPC.{Name}", 7));
            ResetStaticDefaults();
        }

        public sealed override void SetDefaults() => ResetDefaults();

        public sealed override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            IBestiaryInfoElement[] info = new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CNCLocalization.GetText($"Bestiary.{Name}"))
            };
            bestiaryEntry.Info.AddRange(info);
            ResetBestiary(ref database, ref bestiaryEntry);
        }

        public sealed override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            if (countFrame <= 1) return; 
            int frame = npc.frame.Y / frameHeight;
            int frame2 = frame;
            int frame3 = frame;
            int frameMax = countFrame;
            int[] frameTime = new int[frameMax];
            for (int i = 0; i < frameTime.Length; i++) { frameTime[i] = 10; }
            bool autoAnim = true;
            ModifyFrame(ref autoAnim, ref frameTime, ref frame, frameHeight);
            if (autoAnim)
            {
                if (npc.frameCounter < frameTime[frame]) { npc.frameCounter++; }
                if (npc.frameCounter >= frameTime[frame])
                {
                    npc.frameCounter = 0;
                    if (frame2 >= frameMax) { npc.frame.Y = 0; }
                    else { npc.frame.Y += frameHeight; }
                }
            }
            if (frame != frame3) { npc.frame.Y = frame * frameHeight; }
        }

        public sealed override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
            => HitBy(player, item, damage, knockback, crit, true);

        public sealed override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
            => HitBy(Main.player[projectile.owner] != null ? Main.player[projectile.owner] : Main.LocalPlayer, projectile, damage, knockback, crit, false);

        public sealed override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
            => ModifyHitBy(player, item, ref damage, ref knockback, ref crit, true);

        //注：联机下需要服务端来执行此方法
        public sealed override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
            => ModifyHitBy(Main.player[projectile.owner] != null ? Main.player[projectile.owner] : Main.LocalPlayer, projectile, ref damage, ref knockback, ref crit, false);

        public sealed override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            HitNPC(target, damage, knockback, crit);
            OnHit(target, damage, knockback, crit);
        }

        public sealed override void OnHitPlayer(Player target, int damage, bool crit)
        {
            HitPlayer(target, damage, crit);
            OnHit(target, damage, 0f, crit);
        }

        public sealed override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            HitNPC(target, ref damage, ref knockback, ref crit);
            ModifyHit(target, ref damage, ref knockback, ref crit);
        }

        public sealed override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            float knockback = 0f;
            HitPlayer(target, ref damage, ref crit);
            ModifyHit(target, ref damage, ref knockback, ref crit);
        }

        public virtual void ResetDefaults() { }
        public virtual void ResetStaticDefaults() { }
        public virtual void ResetBestiary(ref BestiaryDatabase database, ref BestiaryEntry bestiaryEntry) { }
        public virtual void HitNPC(NPC target, int damage, float knockback, bool crit) { }
        public virtual void HitNPC(NPC target, ref int damage, ref float knockback, ref bool crit) { }
        public virtual void HitPlayer(Player target, int damage, bool crit) { }
        public virtual void HitPlayer(Player target, ref int damage, ref bool crit) { }
        public virtual void OnHit(Entity target, int damage, float knockback, bool crit) { }
        public virtual void ModifyHit(Entity target, ref int damage, ref float knockback, ref bool crit) { }
        public virtual void HitBy(Player attacker, Entity entity, int damage, float knockback, bool crit, bool isItem) { }
        public virtual void ModifyHitBy(Player attacker, Entity entity, ref int damage, ref float knockback, ref bool crit, bool isItem) { }
        public virtual void ModifyFrame(ref bool autoAnim, ref int[] frameTime, ref int frame, int frameHeight) { }
    }
}