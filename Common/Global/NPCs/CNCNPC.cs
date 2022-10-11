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
        public Texture2D Texture2 => CNC.GetTexture(Texture);
        public string TexturePath => Texture.Remove(Texture.LastIndexOf('/')) + '/';

        public NPC npc => NPC;
        public Mod mod => Mod;
        public Player Target => (0 <= npc.target && npc.target <= 255) ? Main.player[npc.target] : null;
        public int countFrame { get => Main.npcFrameCount[Type]; set => Main.npcFrameCount[Type] = value; }
        public int attackFrame { get => NPCID.Sets.AttackFrameCount[Type]; set => NPCID.Sets.AttackFrameCount[Type] = value; }
        public int extraFrame { get => NPCID.Sets.ExtraFramesCount[Type]; set => NPCID.Sets.ExtraFramesCount[Type] = value; }
        public int tick { get => npc.GetGlobalNPC<AllNPC>().Tick; }
        public int frame { get => npc.frame.Y / Texture2.Height; set => npc.frame.Y = Texture2.Height / countFrame * value; }
        public double frameTick { get => npc.frameCounter; set => npc.frameCounter = value; }
        public float Alpha1 { get => npc.alpha / 255f; set => npc.alpha = (int)(255 * value); }
        public float Alpha2 { get => 1 - Alpha1; set => Alpha1 = 1 - value; }
        public IEntitySource source => npc.GetSource_FromThis();

        public int[] intData => npc.GetGlobalNPC<AllNPC>().intData;
        public bool[] boolData => npc.GetGlobalNPC<AllNPC>().boolData;
        public float[] floatData => npc.GetGlobalNPC<AllNPC>().floatData;
        public double[] doubleData => npc.GetGlobalNPC<AllNPC>().doubleData;
        public string[] stringData => npc.GetGlobalNPC<AllNPC>().stringData;
        
        public void SetData<T>(T data, int index)
        {
            if (typeof(T).Equals(typeof(int))) npc.GetGlobalNPC<AllNPC>().intData[index] = (int)(object)data;
            if (typeof(T).Equals(typeof(bool))) npc.GetGlobalNPC<AllNPC>().boolData[index] = (bool)(object)data;
            if (typeof(T).Equals(typeof(float))) npc.GetGlobalNPC<AllNPC>().floatData[index] = (float)(object)data;
            if (typeof(T).Equals(typeof(double))) npc.GetGlobalNPC<AllNPC>().doubleData[index] = (double)(object)data;
            if (typeof(T).Equals(typeof(string))) npc.GetGlobalNPC<AllNPC>().stringData[index] = (string)(object)data;
        }

        public T GetData<T>(int index)
        {
            T t = default(T);
            if (typeof(T).Equals(typeof(int))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().intData[index];
            if (typeof(T).Equals(typeof(bool))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().boolData[index];
            if (typeof(T).Equals(typeof(float))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().floatData[index];
            if (typeof(T).Equals(typeof(double))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().doubleData[index];
            if (typeof(T).Equals(typeof(string))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().stringData[index];
            return t;
        }

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

        public static NPC NewNPC(NPC npc, int x, int y, short CNCtype, int start = 0)
            => NewNPC(npc, x, y, CNCID.NPC.LegacyObject[CNCtype], start);

        public static NPC NewNPC(NPC npc, int x, int y, int type, int start = 0)
        {
            int whoAmI = NPC.NewNPC(npc.GetSource_FromAI(), x, y, type, start);
            Main.npc[whoAmI].Center = new Vector2(x, y);
            Main.npc[whoAmI].GetGlobalNPC<NPCRelation>().SetParent(npc, true);
            Main.npc[whoAmI].GetGlobalNPC<NPCRelation>().SetRoot(npc, true);
            if (Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().ChildrenNPC != null)
                Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().AddChild(Main.npc[whoAmI]);
            return Main.npc[whoAmI];
        }

        public Projectile NewProjectile(float velocity, float velocityRot, int type, float rotation = 0f)
            => NewProjectile(npc, npc.Center, MyMath.GetXY(velocityRot) * velocity, type, npc.damage, rotation);

        public Projectile NewProjectile(float velocity, float velocityRot, int type, float damage, float rotation = 0f)
            => NewProjectile(npc, npc.Center, MyMath.GetXY(velocityRot) * velocity, type, damage, rotation);

        public Projectile NewProjectile(Vector2 velocity, int type, float damage, float rotation = 0f)
            => NewProjectile(npc, npc.Center, velocity, type, damage, rotation);

        public Projectile NewProjectile(Vector2 position, Vector2 velocity, int type, float damage, float rotation = 0f)
            => NewProjectile(npc, position, velocity, type, damage, rotation);

        public static Projectile NewProjectile(NPC npc, Vector2 position, Vector2 velocity, int type, float damage, float rotation = 0f)
        {
            int whoAmI = Projectile.NewProjectile(npc.GetSource_FromAI(), position, velocity, type, (int)damage, 0, Main.myPlayer);
            Main.projectile[whoAmI].Center = position;
            Main.projectile[whoAmI].rotation = rotation;
            Main.projectile[whoAmI].damage = (int)damage;
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetParent(npc, true);
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetRoot(npc, true);
            if (Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().ChildrenProjectile != null)
                Main.npc[npc.whoAmI].GetGlobalNPC<NPCRelation>().AddChild(Main.projectile[whoAmI]);
            if (Main.projectile[whoAmI].ModProjectile != null && (CNCProjectile)Main.projectile[whoAmI].ModProjectile != null) ((CNCProjectile)Main.projectile[whoAmI].ModProjectile).OnSpawn();
            return Main.projectile[whoAmI];
        }

        public static int GetDamage(int normal, int expert, int master)
        {
            int dmg = 0;
            switch (CNC.GameMode)
            {
                case 1: dmg = normal; break;
                case 2: dmg = expert; break;
                case 3: dmg = master; break;
            }
            return dmg / CNC.GameMode;
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

        public sealed override void SetDefaults()
        {
            if (Main.dedServ) return;
            string path = Texture.StartsWith(mod.Name) ? Texture.Substring(Texture.IndexOf('/') + 1) : Texture;
            Texture2D image = this.mod.Assets.Request<Texture2D>(path).Value;
            this.npc.Size = new Vector2(image.Width, image.Height / countFrame);
            ResetDefaults();
        }

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
        /// <summary> 修改动画 </summary>
        /// <param name="autoAnim">自动播放动画</param>
        /// <param name="frameTime">每帧时长</param>
        /// <param name="frame">第几帧</param>
        /// <param name="frameHeight">每帧贴图高度</param>
        public virtual void ModifyFrame(ref bool autoAnim, ref int[] frameTime, ref int frame, int frameHeight) { }
    }
}