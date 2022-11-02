using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.DataStructures;
using CNC.Content;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Common.Global.NPCs;

namespace CNC.Common.Global.Projectiles
{
    public abstract class CNCProjectile : ModProjectile
    {
        public NPC RootNPC => projectile.GetGlobalProjectile<ProjectileRelation>().RootNPC;
        public NPC ParentNPC => projectile.GetGlobalProjectile<ProjectileRelation>().ParentNPC;
        public NPC[] ChildrenNPC => projectile.GetGlobalProjectile<ProjectileRelation>().ChildrenNPC;
        public Projectile RootProjectile => projectile.GetGlobalProjectile<ProjectileRelation>().RootProjectile;
        public Projectile ParentProjectile => projectile.GetGlobalProjectile<ProjectileRelation>().ParentProjectile;
        public Projectile[] ChildrenProjectile => projectile.GetGlobalProjectile<ProjectileRelation>().ChildrenProjectile;
        public Dictionary<string, object> data { get => projectile.GetGlobalProjectile<AllProjectile>().data; set => projectile.GetGlobalProjectile<AllProjectile>().data = value; }
        public Texture2D Texture2 => CNC.GetTexture(Texture);
        public string TexturePath => Texture.Remove(Texture.LastIndexOf('/')) + '/';

        public Mod mod => Mod;
        public Projectile projectile => Projectile;
        public Player owner => Main.player[MyMath.Clamp(projectile.owner, 0, 255)];
        public Player Target = null;
        public bool CannotHitSameEnemyAgain = false;
        public List<NPC> haveHit = new List<NPC>();
        public float Alpha1 { get => projectile.alpha / 255f; set => projectile.alpha = (int)(255 * value); }
        public float Alpha2 { get => 1 - Alpha1; set => Alpha1 = 1 - value; }
        public IEntitySource source => projectile.GetSource_FromThis();
        public float posX { get => projectile.position.X; set => projectile.position.X = value; }
        public float posY { get => projectile.position.Y; set => projectile.position.Y = value; }
        public float velX { get => projectile.velocity.X; set => projectile.velocity.X = value; }
        public float velY { get => projectile.velocity.Y; set => projectile.velocity.Y = value; }
        public int frame { get => projectile.frame; set => projectile.frame = value; }
        public int frameTick { get => projectile.frameCounter; set => projectile.frameCounter = value; }
        public Rectangle? hitBox = null;
        /// <summary> 对子弹幕生效 </summary>
        public float damageAdd => projectile.GetGlobalProjectile<AllProjectile>().damageAdd;
        /// <summary> 对子弹幕生效 </summary>
        public float damageMul => projectile.GetGlobalProjectile<AllProjectile>().damageMult;

        public int[] intData => projectile.GetGlobalProjectile<AllProjectile>().intData;
        public bool[] boolData => projectile.GetGlobalProjectile<AllProjectile>().boolData;
        public float[] floatData => projectile.GetGlobalProjectile<AllProjectile>().floatData;
        public double[] doubleData => projectile.GetGlobalProjectile<AllProjectile>().doubleData;
        public string[] stringData => projectile.GetGlobalProjectile<AllProjectile>().stringData;

        public void SetData<T>(T data, int index)
        {
            if (typeof(T).Equals(typeof(int))) projectile.GetGlobalProjectile<AllProjectile>().intData[index] = (int)(object)data;
            if (typeof(T).Equals(typeof(bool))) projectile.GetGlobalProjectile<AllProjectile>().boolData[index] = (bool)(object)data;
            if (typeof(T).Equals(typeof(float))) projectile.GetGlobalProjectile<AllProjectile>().floatData[index] = (float)(object)data;
            if (typeof(T).Equals(typeof(double))) projectile.GetGlobalProjectile<AllProjectile>().doubleData[index] = (double)(object)data;
            if (typeof(T).Equals(typeof(string))) projectile.GetGlobalProjectile<AllProjectile>().stringData[index] = (string)(object)data;
        }

        public T GetData<T>(int index)
        {
            T t = default(T);
            if (typeof(T).Equals(typeof(int))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().intData[index];
            if (typeof(T).Equals(typeof(bool))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().boolData[index];
            if (typeof(T).Equals(typeof(float))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().floatData[index];
            if (typeof(T).Equals(typeof(double))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().doubleData[index];
            if (typeof(T).Equals(typeof(string))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().stringData[index];
            return t;
        }

        internal delegate void Player_ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit);
        internal delegate void Player_OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit);
        internal delegate void Player_ModifyHitPlayer(Player player, Player target, ref int damage, ref bool crit);
        internal delegate void Player_OnHitPlayer(Player player, Player target, int damage, bool crit);

        internal delegate void Vanilla_ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit);
        internal delegate void Vanilla_OnHitNPC(NPC target, int damage, float knockBack, bool crit);
        internal delegate void Vanilla_ModifyHitPlayer(Player player, ref int damage, ref bool crit);
        internal delegate void Vanilla_OnHitPlayer(Player player, int damage, bool crit);

        internal delegate void Projectile_ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockBack, ref bool crit);
        internal delegate void Projectile_OnHitNPC(Projectile projectile, NPC target, int damage, float knockBack, bool crit);
        internal delegate void Projectile_ModifyHitPlayer(Projectile projectile, Player player, ref int damage, ref bool crit);
        internal delegate void Projectile_OnHitPlayer(Projectile projectile, Player player, int damage, bool crit);

        internal delegate void Vanilla_Kill(int timeLeft);
        internal delegate bool Vanilla_PreKill(int timeLeft);
        internal delegate void Projectile_Kill(Projectile projectile, int timeLeft);
        internal delegate bool Projectile_PreKill(Projectile projectile, int timeLeft);

        internal delegate void Vanilla_Update();
        internal delegate void Projectile_Update(Projectile projectile);

        internal Player_ModifyHitNPC Player_ModifyHitNPCEffect;
        internal Player_OnHitNPC Player_OnHitNPCEffect;
        internal Player_ModifyHitPlayer Player_ModifyHitPlayerEffect;
        internal Player_OnHitPlayer Player_OnHitPlayerEffect;

        internal Vanilla_ModifyHitNPC Vanilla_ModifyHitNPCEffect;
        internal Vanilla_OnHitNPC Vanilla_OnHitNPCEffect;
        internal Vanilla_ModifyHitPlayer Vanilla_ModifyHitPlayerEffect;
        internal Vanilla_OnHitPlayer Vanilla_OnHitPlayerEffect;

        internal Projectile_ModifyHitNPC Projectile_ModifyHitNPCEffect;
        internal Projectile_OnHitNPC Projectile_OnHitNPCEffect;
        internal Projectile_ModifyHitPlayer Projectile_ModifyHitPlayerEffect;
        internal Projectile_OnHitPlayer Projectile_OnHitPlayerEffect;

        internal Vanilla_Kill Vanilla_KillEffect;
        internal Vanilla_PreKill Vanilla_PreKillEffect;
        internal Projectile_Kill Projectile_KillEffect;
        internal Projectile_PreKill Projectile_PreKillEffect;

        internal Vanilla_Update Vanilla_UpdateEffect;
        internal Projectile_Update Projectile_UpdateEffect;

        public ProjDamageStyle DamageStyle
        {
            get => (ProjDamageStyle)(projectile.friendly.ToInt(1) + projectile.hostile.ToInt(2));
            set { projectile.hostile = ((int)value) > 1; projectile.friendly = ((int)value) % 2 == 1; }
        }

        public enum ProjDamageStyle
        {
            None = 0,
            Friendly = 1,
            Hostile = 2,
            Both = 3,
        }

        public NPC NewNPC(short CNCtype, int start = 0)
            => NewNPC(projectile, (int)projectile.Center.X, (int)projectile.Center.Y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(int type, int start = 0)
            => NewNPC(projectile, (int)projectile.Center.X, (int)projectile.Center.Y, type, start);

        public NPC NewNPC(Vector2 pos, short CNCtype, int start = 0)
            => NewNPC(projectile, (int)pos.X, (int)pos.Y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(Vector2 pos, int type, int start = 0)
            => NewNPC(projectile, (int)pos.X, (int)pos.Y, type, start);

        public NPC NewNPC(int x, int y, short CNCtype, int start = 0)
            => NewNPC(projectile, x, y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(int x, int y, int type, int start = 0)
            => NewNPC(projectile, x, y, type, start);

        public NPC NewNPC(Projectile projectile, int x, int y, short CNCtype, int start = 0)
            => NewNPC(projectile, x, y, CNCID.NPC.LegacyObject[CNCtype], start);

        public NPC NewNPC(Projectile projectile, int x, int y, int type, int start = 0)
        {
            int whoAmI = NPC.NewNPC(projectile.GetSource_FromThis(), x, y, type, start);
            Main.npc[whoAmI].Center = new Vector2(x, y);
            Main.npc[whoAmI].GetGlobalNPC<NPCRelation>().SetParent(projectile, true);
            Main.npc[whoAmI].GetGlobalNPC<NPCRelation>().SetRoot(projectile, true);
            if (Main.projectile[projectile.whoAmI].GetGlobalProjectile<ProjectileRelation>().ChildrenNPC != null)
                Main.projectile[projectile.whoAmI].GetGlobalProjectile<ProjectileRelation>().AddChild(Main.npc[whoAmI]);
            return Main.npc[whoAmI];
        }

        public Projectile NewProjectile(float velocity, float velocityRot, int type, float knockback = 0f, float rotation = 0f, float ai0 = 0f, float ai1 = 0f)
            => NewProjectile(projectile, projectile.Center, MyMath.GetXY(velocityRot) * velocity, type, projectile.damage, rotation, 0f, 1f, ai0, ai1);

        public Projectile NewProjectile(float velocity, float velocityRot, int type, float damage, float knockback, float rotation = 0f, float damageAdd = 0f, float damageMul = 1f, float ai0 = 0f, float ai1 = 0f)
            => NewProjectile(projectile, projectile.Center, MyMath.GetXY(velocityRot) * velocity, type, damage, rotation, damageAdd, damageMul, ai0, ai1);

        public Projectile NewProjectile(Vector2 velocity, int type, float damage, float knockback, float rotation = 0f, float damageAdd = 0f, float damageMul = 1f, float ai0 = 0f, float ai1 = 0f)
            => NewProjectile(projectile, projectile.Center, velocity, type, damage, rotation, damageAdd, damageMul, ai0, ai1);

        public Projectile NewProjectile(Vector2 position, Vector2 velocity, int type, float damage, float knockback, float rotation = 0f, float damageAdd = 0f, float damageMul = 1f, float ai0 = 0f, float ai1 = 0f)
            => NewProjectile(projectile, position, velocity, type, damage, rotation, damageAdd, damageMul, ai0, ai1);

        /// <summary> 生成子弹幕 </summary>
        /// <param name="projectile"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        /// <param name="knockBack"></param>
        /// <param name="rotation"></param>
        /// <param name="damageAdd"></param>
        /// <param name="damageMult"></param>
        public Projectile NewProjectile(Projectile projectile, Vector2 position, Vector2 velocity, int type, float damage, float knockBack = 0f, float rotation = 0f, float damageAdd = 0f, float damageMult = 1f, float ai0 = 0f, float ai1 = 0f)
        {
            int whoAmI = Projectile.NewProjectile(projectile.GetSource_FromThis(), position, velocity, type, (int)damage, knockBack, projectile.owner, ai0, ai1);
            Main.projectile[whoAmI].ai[0] = ai0;
            Main.projectile[whoAmI].ai[1] = ai1;
            Main.projectile[whoAmI].Center = position;
            Main.projectile[whoAmI].rotation = rotation;
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageAdd = damageAdd;
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageMult = damageMult;
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetParent(projectile, true);
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetRoot(projectile, true);
            if (Main.projectile[projectile.whoAmI].GetGlobalProjectile<ProjectileRelation>().ChildrenProjectile != null)
                Main.projectile[projectile.whoAmI].GetGlobalProjectile<ProjectileRelation>().AddChild(Main.projectile[whoAmI]);
            if (Main.projectile[whoAmI].ModProjectile != null && (CNCProjectile)Main.projectile[whoAmI].ModProjectile != null) ((CNCProjectile)Main.projectile[whoAmI].ModProjectile).OnSpawn();
            return Main.projectile[whoAmI];
        }

        public float[] var = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public virtual int countFrame => 1;

        public sealed override void SetStaticDefaults()
        {
            Main.projFrames[Type] = countFrame;
            DisplayName.SetDefault(CNCLocalization.GetText($"Projectile.{Name}", 1));
            DisplayName.AddTranslation(7, CNCLocalization.GetText($"Projectile.{Name}", 7));
            ResetStaticDefaults();
        }

        public sealed override void SetDefaults()
        {
            if (Main.dedServ) return;
            hitBox = null;
            string path = Texture.StartsWith(mod.Name) ? Texture.Substring(Texture.IndexOf('/') + 1) : Texture;
            Texture2D image = this.mod.Assets.Request<Texture2D>(path).Value;
            this.projectile.Resize(image.Width, image.Height / countFrame);
            ResetDefaults();
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (this.CannotHitSameEnemyAgain)
            {
                bool flag = base.CanHitNPC(target).HasValue ? base.CanHitNPC(target).Value : !target.friendly;
                return flag && !haveHit.Contains(target);
            }
            return base.CanHitNPC(target);
        }

        public void AfterImages(Projectile projectile, Color color, int count, Texture2D image, float scaleAdd, float scaleMult, Rectangle? source = null, float alphaAdd = 0f)
        {
            Main.instance.LoadProjectile(projectile.type);
            var stat = new Content.DataStructures.VariagleStatus(1, scaleAdd, 0f, 1f, 1f, scaleMult);
            Texture2D texture = image == null ? TextureAssets.Projectile[projectile.type].Value : image;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            int num = projectile.oldPos.Length > count ? count : projectile.oldPos.Length;
            for (int i = 0; i < num; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin;
                Color _color = projectile.GetAlpha(color) * ((num - i) / (float)num - alphaAdd * i);
                Main.EntitySpriteDraw(texture, drawPos, source, _color, projectile.oldRot[i], drawOrigin, projectile.scale * stat.GetNextValue(), SpriteEffects.None, 0);
            }
        }

        public void AfterImages(Projectile projectile, Color color, int count, Texture2D image, Vector2 scaleAdd, Vector2 scaleMult, float alphaAdd = 0f)
        {
            Main.instance.LoadProjectile(projectile.type);
            if (scaleAdd == default) scaleAdd = Vector2.Zero;
            if (scaleMult == default) scaleMult = Vector2.One;
            var statX = new Content.DataStructures.VariagleStatus(1, scaleAdd.X, 0f, 1f, 1f, scaleMult.X);
            var statY = new Content.DataStructures.VariagleStatus(1, scaleAdd.Y, 0f, 1f, 1f, scaleMult.Y);
            Texture2D texture = image == null ? TextureAssets.Projectile[projectile.type].Value : image;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            int num = projectile.oldPos.Length > count ? count : projectile.oldPos.Length;
            for (int i = 0; i < num; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin;
                Color _color = projectile.GetAlpha(color) * ((num - i) / (float)num - alphaAdd * i);
                Main.EntitySpriteDraw(texture, drawPos, null, _color, projectile.oldRot[i], drawOrigin, projectile.scale * new Vector2(statX.GetNextValue(), statY.GetNextValue()), SpriteEffects.None, 0);
            }
        }
        
        /// <summary> 寻找最近的NPC </summary>
        /// <param name="p">当前弹幕</param>
        /// <param name="range">搜索半径</param>
        /// <param name="friendly">是否为友好NPC</param>
        /// <returns></returns>
        public NPC FindClosestTarget(Projectile p, float range, bool friendly = false)
        {
            NPC target = null;
            float clostest = range;
            for (int i = 0; i < Const.max1; i++)
            {
                NPC npc = Main.npc[i];
                if (npc == null || !npc.active || npc.life <= 0 || npc.friendly != friendly || npc.townNPC != friendly || npc.type == 488) continue;
                float dist = Vector2.Distance(p.position, npc.position);
                bool canHit = Collision.CanHitLine(p.position, p.width, p.height, npc.position, npc.width, npc.height);
                if (dist < clostest && canHit)
                {
                    target = npc;
                    clostest = dist;
                }
            }
            return target;
        }
        /// <summary> 搜索最近玩家 </summary>
        /// <param name="p">当前弹幕</param>
        /// <param name="range">搜索半径</param>
        public Player FindClosestTarget(Projectile p, float range)
        {
            if (Main.netMode == 0) return Const.Me;
            Player target = null;
            float clostest = range;
            for (int i = 0; i < Const.max1; i++)
            {
                Player player = Main.player[i];
                if (player == null || !player.active || player.statLife <= 0) continue;
                float dist = Vector2.Distance(p.position, player.position);
                bool canHit = Collision.CanHitLine(p.position, p.width, p.height, player.position, player.width, player.height);
                if (dist < clostest && canHit)
                {
                    target = player;
                    clostest = dist;
                }
            }
            return target;
        }
        /// <summary> 记录多帧的旋转和坐标 </summary>
        /// <param name="p">当前弹幕</param>
        /// <param name="offset">所有坐标偏移</param>
        public void UpdateOldData(Projectile p, Vector2 offset = default)
        {
            if (offset == default) offset = Vector2.Zero;
            for(int i = p.oldPos.Length - 1; i > 0; i--)
                p.oldPos[i] = p.oldPos[i - 1];
            p.oldPos[0] = p.position + offset;
            for (int i = p.oldRot.Length - 1; i > 0; i--)
                p.oldRot[i] = p.oldRot[i - 1];
            p.oldRot[0] = p.rotation;
        }
        public int JustGetDamage(Inscription inscription, int dataIndex = 0, int originalDamage = -1) => Inscription.JustGetDamage(originalDamage, (string)inscription.Data[MyMath.Max(0, dataIndex)], this.damageAdd, this.damageMul);
        public int JustGetDamage(Inscription inscription, float damageAdd, float damageMul, int dataIndex = 0, int originalDamage = -1) => Inscription.JustGetDamage(originalDamage, (string)inscription.Data[MyMath.Max(0, dataIndex)], damageAdd, damageMul);

        protected Projectile AI001_RollingMissile(Projectile p, bool gravity = false, bool updateRot = false, float extraRot = 0)
        {
            Vector2 realVel = Const._02;
            float angle = MyMath.pi * extraRot / 180;
            if (p.type == 0 || p.velocity == Const._02) return p;
            p.velocity.Y = gravity ? p.velocity.Y + MyMath.g * .01f : p.oldVelocity.Y;
            if (updateRot && realVel == Const._02)
            {
                realVel = p.velocity;
                p.rotation = angle + realVel.ToRotation();
            }
            else p.rotation = angle + p.velocity.ToRotation();
            return p;
        }

        protected void Anim001_Loop(Projectile p, int tickPerFrame, int start = 0)
        {
            int[] tick = new int[Main.projFrames[p.type]];
            for (int i = 0; i < tick.Length; i++) tick[i] = tickPerFrame;
            Anim001_Loop(p, tick, start);
        }

        protected void Anim001_Loop(Projectile p, int[] tick, int start = 0)
        {
            int maxFrame = Main.projFrames[p.type];
            int count = tick[p.frame];
            if (p.frameCounter % count == count - 1) p.frame++;
            p.frameCounter++;
            if (p.frame >= maxFrame) p.frame = start;
        }

        public sealed override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!haveHit.Contains(target)) haveHit.Add(target);
            this.Player_OnHitNPCEffect?.Invoke(this.owner, target, damage, knockback, crit);
            this.Vanilla_OnHitNPCEffect?.Invoke(target, damage, knockback, crit);
            this.Projectile_OnHitNPCEffect?.Invoke(projectile, target, damage, knockback, crit);
            this.HitNPC(target, damage, knockback, crit);
            this.OnHit(target, damage, knockback, crit);
        }

        public sealed override void OnHitPlayer(Player target, int damage, bool crit)
        {
            this.Player_OnHitPlayerEffect?.Invoke(Main.player[projectile.owner], target, damage, crit);
            this.Vanilla_OnHitPlayerEffect?.Invoke(target, damage, crit);
            this.Projectile_OnHitPlayerEffect?.Invoke(projectile, target, damage, crit);
            this.HitPlayer(target, damage, crit);
            this.OnHit(target, damage, 0f, crit);
        }
        
        public sealed override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!haveHit.Contains(target)) haveHit.Add(target);
            this.Player_ModifyHitNPCEffect?.Invoke(this.owner, target, ref damage, ref knockback, ref crit);
            this.Vanilla_ModifyHitNPCEffect?.Invoke(target, ref damage, ref knockback, ref crit);
            this.Projectile_ModifyHitNPCEffect?.Invoke(projectile, target, ref damage, ref knockback, ref crit);
            this.HitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
            this.ModifyHit(target, ref damage, ref knockback, ref crit);
        }

        public sealed override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            float knockback = 0f;
            this.Player_ModifyHitPlayerEffect?.Invoke(Main.player[projectile.owner], target, ref damage, ref crit);
            this.Vanilla_ModifyHitPlayerEffect?.Invoke(target, ref damage, ref crit);
            this.Projectile_ModifyHitPlayerEffect?.Invoke(projectile, target, ref damage, ref crit);
            this.HitPlayer(target, ref damage, ref crit);
            this.ModifyHit(target, ref damage, ref knockback, ref crit);
        }

        public sealed override void Kill(int timeLeft)
        {
            this.Vanilla_KillEffect?.Invoke(timeLeft);
            this.Projectile_KillEffect?.Invoke(projectile, timeLeft);
            this.OnKill(timeLeft);
        }

        public sealed override bool PreKill(int timeLeft)
        {
            bool flag = base.PreKill(timeLeft);
            flag &= this.Vanilla_PreKillEffect == null ? true : (bool)this.Vanilla_PreKillEffect?.Invoke(timeLeft);
            flag &= this.Projectile_PreKillEffect == null ? true : (bool)this.Projectile_PreKillEffect?.Invoke(projectile, timeLeft);
            flag &= this.ModifyKill(timeLeft);
            return flag;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (this.hitBox == null) base.ModifyDamageHitbox(ref hitbox);
            else hitbox = (Rectangle)this.hitBox;
        }

        public override void AI()
        {
            this.UpdateAI();
            this.Vanilla_UpdateEffect?.Invoke();
            this.Projectile_UpdateEffect?.Invoke(projectile);
        }

        public virtual void ResetDefaults() { }
        public virtual void ResetStaticDefaults() { }
        public virtual void OnSpawn() { }
        public virtual void HitNPC(NPC target, int damage, float knockback, bool crit) { }
        public virtual void HitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) { }
        public virtual void HitPlayer(Player target, int damage, bool crit) { }
        public virtual void HitPlayer(Player target, ref int damage, ref bool crit) { }
        public virtual void OnHit(Entity target, int damage, float knockback, bool crit) { }
        public virtual void ModifyHit(Entity target, ref int damage, ref float knockback, ref bool crit) { }
        public virtual void OnKill(int timeLeft) { }
        public virtual bool ModifyKill(int timeLeft) => base.PreKill(timeLeft);
        public virtual void UpdateAI() { }
    }
}