using System;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using CNC.Common.Global.NPCs;
using CNC.Common.Global.Players;
using CNC.Common.Global.Projectiles;
using CNC.Content.NPCs;
using CNC.Content.Items;
using CNC.Content.Projectiles;

namespace CNC.Common.Utils
{
    public static class CNCUtils
    {
        /// <summary> 生成弹幕 </summary>
        /// <param name="pos">世界坐标</param>
        /// <param name="vel">初速度</param>
        /// <param name="type">弹幕id</param>
        /// <param name="dmg">伤害</param>
        /// <param name="kb">击退</param>
        /// <param name="owner">生成者</param>
        /// <param name="data0">数据变量1</param>
        /// <param name="data1">数据变量2</param>
        public static Projectile NewProj(Terraria.DataStructures.IEntitySource source, Vector2 position, Vector2 velocity, int type, int dmg, float kb, int owner = -1, float damageAdd = 0, float damageMul = 1)
        {
            if (owner < 0) { owner = Const.me; }
            int whoAmI = Projectile.NewProjectile(source, position, velocity, type, dmg, kb, owner);
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageAdd = damageAdd;
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageMult = damageMul;
            if (Main.projectile[whoAmI].ModProjectile != null && (CNCProjectile)Main.projectile[whoAmI].ModProjectile != null) ((CNCProjectile)Main.projectile[whoAmI].ModProjectile).OnSpawn();
            return Main.projectile[whoAmI];
        }

        /// <summary> 生成弹幕 </summary>
        /// <param name="pos">世界坐标</param>
        /// <param name="vel">初速度</param>
        /// <param name="type">弹幕id</param>
        /// <param name="dmg">伤害</param>
        /// <param name="kb">击退</param>
        /// <param name="crit">暴击率</param>
        /// <param name="owner">生成者</param>
        public static Projectile NewProj(Terraria.DataStructures.IEntitySource source, Vector2 position, Vector2 velocity, int type, int dmg, float kb, int crit, int owner = -1, float damageAdd = 0, float damageMult = 1)
        {
            if (owner < 0) { owner = Const.me; }
            int whoAmI = Projectile.NewProjectile(source, position, velocity, type, dmg, kb, owner);
            Main.projectile[whoAmI].CritChance = crit;
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageAdd = damageAdd;
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageMult = damageMult;
            if (Main.projectile[whoAmI].ModProjectile != null && (CNCProjectile)Main.projectile[whoAmI].ModProjectile != null) ((CNCProjectile)Main.projectile[whoAmI].ModProjectile).OnSpawn();
            return Main.projectile[whoAmI];
        }


        public static Projectile NewProjectile(this Projectile projectile, Vector2 position, Vector2 velocity, int type, float damage, float knockBack, int crit, float rotation, float damageAdd = 0f, float damageMult = 1f)
        {
            int whoAmI = Projectile.NewProjectile(projectile.GetSource_FromThis(), position, velocity, type, (int)damage, knockBack, projectile.owner);
            Main.projectile[whoAmI].Center = position;
            Main.projectile[whoAmI].oldPosition = position + projectile.Size / 2f;
            Main.projectile[whoAmI].velocity = velocity;
            Main.projectile[whoAmI].rotation = rotation;
            Main.projectile[whoAmI].damage = (int)damage;
            Main.projectile[whoAmI].knockBack = knockBack;
            Main.projectile[whoAmI].CritChance = crit;
            Main.projectile[whoAmI].DamageType = projectile.DamageType;
            Main.projectile[whoAmI].hostile = projectile.hostile;
            Main.projectile[whoAmI].friendly = projectile.friendly;
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageAdd = damageAdd;
            Main.projectile[whoAmI].GetGlobalProjectile<AllProjectile>().damageMult = damageMult;
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetParent(projectile, true);
            Main.projectile[whoAmI].GetGlobalProjectile<ProjectileRelation>().SetRoot(projectile, true);
            if (Main.projectile[projectile.whoAmI].GetGlobalProjectile<ProjectileRelation>().ChildrenProjectile != null)
                Main.projectile[projectile.whoAmI].GetGlobalProjectile<ProjectileRelation>().AddChild(Main.projectile[whoAmI]);
            if (Main.projectile[whoAmI].ModProjectile != null && (CNCProjectile)Main.projectile[whoAmI].ModProjectile != null) ((CNCProjectile)Main.projectile[whoAmI].ModProjectile).OnSpawn();
            return Main.projectile[whoAmI];
        }

        public static Dust NewDust(Vector2 pos, int id, float scale = 1f, Vector2 vel = default, Color color = default)
        {
            int index = Dust.NewDust(pos, 0, 0, id, vel.X, vel.Y, color.A, color, scale);
            Main.dust[index].position = pos;
            Main.dust[index].velocity = vel;
            Main.dust[index].scale = scale;
            Main.dust[index].color = color;
            Main.dust[index].alpha = color.A;
            return Main.dust[index];
        }

        public static Dust NewDust(Entity target, int id, float scale = 1f, Vector2 vel = default, Color color = default)
            => Dust.NewDustDirect(target.position + CNC.Random.NextVector2(target.Size), 0, 0, id, vel.X, vel.Y, color.A, color, scale);
        
        public static Dust NewDust(Vector4 pos_size, int id, float angle, float scale = 1f, Vector2 range = default, Vector2 vel = default, Color color = default)
            => NewDust(pos_size, id, angle, scale, range, color, vel);

        public static Dust NewDust(Vector4 pos_size, int id, float angle, float scale = 1f, Vector2 range = default, Color color = default, Vector2 vel = default)
        {
            Vector2 pos = pos_size.XY();
            int width = (int)pos_size.Z;
            int height = (int)pos_size.W / 2;
            vel = vel == default ? Const._02 : vel;
            float x = Main.rand.NextFloat(-range.X, range.X);
            float y = Main.rand.NextFloat(-range.Y, range.Y);
            Vector2 _range = range == default ? Const._02 : new Vector2((float)(x * Math.Cos(angle) + y * Math.Sin(angle)), (float)(x * Math.Sin(angle) + y * Math.Cos(angle)));
            return Dust.NewDustDirect(pos + _range, width, height, id, vel.X, vel.Y, color.A, color, scale);
        }

        public static Dust NewDust(Entity target, int id, float angle, float scale = 1f, Vector2 range = default, Color color = default, Vector2 vel = default)
            => NewDust(target, id, angle, scale, range, vel, color);

        public static Dust NewDust(Entity target, int id, float angle, float scale = 1f, Vector2 range = default, Vector2 vel = default, Color color = default)
        {
            Vector2 pos = target.position - target.velocity;
            vel = vel == default ? Const._02 : vel;
            float x = Main.rand.NextFloat(-range.X, range.X);
            float y = Main.rand.NextFloat(-range.Y, range.Y);
            Vector2 _range = range == default ? Const._02 : new Vector2((float)(x * Math.Cos(angle) + y * Math.Sin(angle)), (float)(x * Math.Sin(angle) + y * Math.Cos(angle)));
            return Dust.NewDustDirect(pos + _range + CNC.Random.NextVector2(target.Size), 0, 0, id, vel.X, vel.Y, color.A, color, scale);
        }

        public static void PlaySound(SoundStyle sound, Entity target)
            => PlaySound(sound, target.position);

        public static void PlaySound(SoundStyle sound, Vector2 position)
            => SoundEngine.PlaySound(sound, position);

        public static void PlaySound(string path, Entity target, Mod mod = null)
            => PlaySound(path, target.position, 1f, mod ?? CNC.Instance);

        public static void PlaySound(string path, Entity target, float volume, Mod mod = null)
            => PlaySound(path, target.position, volume, mod ?? CNC.Instance);

        public static void PlaySound(string path, Vector2 position, Mod mod = null)
            => PlaySound(path, position, 1f, mod ?? CNC.Instance);

        public static void PlaySound(string path, Vector2 position, float volume, Mod mod = null)
        {
            mod = mod ?? CNC.Instance;
            if (!path.StartsWith("Content/Sounds/Custom/")) path = "Content/Sounds/Custom/" + path;
            SoundEngine.PlaySound(new SoundStyle(mod.Name + "/" + path, SoundType.Sound), position);
            //Terraria.Audio.SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)position.X, (int)position.Y, SoundLoader.GetSoundSlot(mod == default ? CNC.Instance : mod, path), volume);
        }

        public static void PlaySound(string path, Vector2 position, float volume, float pitch, Mod mod = null)
        {
            mod = mod ?? CNC.Instance;
            if (!path.StartsWith("Content/Sounds/Custom/")) path = "Content/Sounds/Custom/" + path;
            SoundEngine.PlaySound(new SoundStyle(mod.Name + "/" + path, SoundType.Sound), position);
           // Terraria.Audio.SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)position.X, (int)position.Y, SoundLoader.GetSoundSlot(mod == default ? CNC.Instance : mod, path), volume, pitch);
        }

        /// <summary> 添加光源 </summary>
        /// <param name="position">世界内坐标</param>
        /// <param name="color">光源颜色</param>
        /// <param name="light">亮度</param>
        public static void AddLight(Vector2 position, Color color, float light = .5f)
        {
            float a = color.A / (float)byte.MaxValue * light;
            float r = color.R / (float)byte.MaxValue * light;
            float g = color.G / (float)byte.MaxValue * light;
            float b = color.B / (float)byte.MaxValue * light;
            Lighting.AddLight((int)(position.X / 16f), (int)(position.Y / 16f), r, g, b);
        }

        /// <summary> 设置Buff </summary>
        /// <param name="target">目标</param>
        /// <param name="cncid">CNC内部ID</param>
        /// <param name="time">持续时间</param>
        /// <param name="quiet">？？</param>
        public static void SetBuff(NPC target, short cncid, int time, bool quiet = false)
        {
            int id = CNCID.Buff.LegacyObject[cncid];
            if (time > 0) target.AddBuff(id, time, quiet);
            else if (target.HasBuff(id)) target.DelBuff(target.FindBuffIndex(id));
        }

        /// <summary> 设置Buff </summary>
        /// <param name="target">目标</param>
        /// <param name="cncid">CNC内部ID</param>
        /// <param name="time">持续时间</param>
        /// <param name="quiet">？？</param>
        public static void SetBuff(Player target, short cncid, int time, bool quiet = false)
        {
            int id = CNCID.Buff.LegacyObject[cncid];
            if (time > 0) target.AddBuff(id, time, quiet);
            else if (target.HasBuff(id)) target.DelBuff(target.FindBuffIndex(id));
        }

        /// <summary> 真实伤害 </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">百分比伤害值</param>
        /// <param name="max">伤害值上限</param>
        /// <param name="kill">致死</param>
        /// <param name="color">伤害字体颜色</param>
        public static void TrueDamage(NPC target, float damage, int max, bool kill = true, Color color = default)
            => TrueDamage(target, (int)(damage * target.lifeMax), max, kill, default);

        /// <summary> 真实伤害 </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害值</param>
        /// <param name="max">伤害值上限</param>
        /// <param name="kill">致死</param>
        /// <param name="color">伤害字体颜色</param>
        public static void TrueDamage(NPC target, int damage, int max, bool kill = true, Color color = default)
        {
            if (target.type == 488) return;
            if (color == default) color = Color.White;
            damage = MyMath.Min(damage, max);
            target.life = MyMath.Max(target.life - damage, (!kill).ToInt(1));
            if (damage > 0) CombatText.NewText(target.getRect(), color, damage, false, true);
            if (target.life <= 0) target.checkDead();
        }

        /// <summary> 真实伤害 </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">百分比伤害值</param>
        /// <param name="max">伤害值上限</param>
        /// <param name="kill">致死</param>
        /// <param name="color">伤害字体颜色</param>
        /// <param name="damageSource">死亡因素</param>
        public static void TrueDamage(Player target, float damage, int max, bool kill = true, Color color = default, Terraria.DataStructures.PlayerDeathReason damageSource = null)
            => TrueDamage(target, (int)(damage * target.statLifeMax2), max, kill, color, damageSource);

        /// <summary> 真实伤害 </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害值</param>
        /// <param name="max">伤害值上限</param>
        /// <param name="kill">致死</param>
        /// <param name="color">伤害字体颜色</param>
        /// <param name="damageSource">死亡因素</param>
        public static void TrueDamage(Player target, int damage, int max, bool kill = true, Color color = default, Terraria.DataStructures.PlayerDeathReason damageSource = null)
        {
            if (color == default) color = Color.White;
            damage = MyMath.Min(damage, max);
            target.statLife = MyMath.Max(target.statLife - damage, (!kill).ToInt(1));
            target.lifeRegenTime = 0;
            if (damage > 0) CombatText.NewText(target.getRect(), color, damage, false, true);
            if (target.statLife <= 0) target.KillMe(damageSource, int.MaxValue, target.direction);
        }
   
        public static void SetTile(Point position, int type, bool isWall = false, bool flag = true, int size = 0)
        {
            if (size == 0)
            {
                int x = position.X;
                int y = position.Y;
                if (WorldGen.InWorld(x, y, 1))
                {
                    int _type = isWall ? Main.tile[x, y].WallType : Main.tile[x, y].TileType;
                    if (!isWall && flag) Main.tile[x, y].TileType = (ushort)type;
                    else if (_type != 0 && flag) Main.tile[x, y].WallType = (ushort)type;
                    WorldGen.SquareWallFrame(x, y, true);
                    NetMessage.SendTileSquare(-1, x, y, 1);
                }
            }
            else for (int x = position.X - size; x <= position.X + size; x++)
                {
                    for (int y = position.Y - size; y <= position.Y + size; y++)
                    {
                        if (WorldGen.InWorld(x, y, 1) && Math.Abs(x - position.X) + Math.Abs(y - position.Y) < Math.Sqrt(size * size * 2))
                        {
                            int _type = isWall ? Main.tile[x, y].WallType : Main.tile[x, y].TileType;
                            if (!isWall && flag) Main.tile[x, y].TileType = (ushort)type;
                            else if (_type != 0 && flag) Main.tile[x, y].WallType = (ushort)type;
                            WorldGen.SquareWallFrame(x, y, true);
                            NetMessage.SendTileSquare(-1, x, y, 1);
                        }
                    }
                }
        }

        /// <summary> 将颜色转换成对应的整型数据 </summary>
        public static int ToInt(this Color color)
            => (int)(color.R << 16) + (int)(color.G << 8) + (int)(color.B << 0);

        /// <summary> 将颜色转换成对应的整型数据 </summary>
        public static uint ToUInt(this Color color)
            => (uint)(color.R << 24) + (uint)(color.G << 16) + (uint)(color.B << 8) + (uint)(color.A << 0);

        /// <summary> 附加原版闪烁的颜色 </summary>
        /// <param name="RGB">颜色编号（可用16进制）</param>
        /// <param name="flash">是否闪烁</param>
        public static Color FlashColor(int RGB, bool flash = true)
        {
            var alpha = Main.mouseTextColor / 255f;
            if (!flash) return new Color((byte)((0xFF0000 & RGB) >> 16), (byte)((0x00FF00 & RGB) >> 8), (byte)((0x0000FF & RGB) >> 0), (byte)255);
            else return new Color((byte)(((0xFF0000 & RGB) >> 16) * alpha), (byte)(((0x00FF00 & RGB) >> 8) * alpha), (byte)(((0x0000FF & RGB) >> 0) * alpha), Main.mouseTextColor);
        }

        /// <summary> 附加原版闪烁的颜色 </summary>
        /// <param name="RGBA">颜色编号（可用16进制）</param>
        /// <param name="flash">是否闪烁</param>
        public static Color FlashColor(uint RGBA = 0x00000000, bool flash = true)
        {
            var alpha = Main.mouseTextColor / 255f;
            if (!flash) return new Color((byte)((0xFF000000 & RGBA) >> 24), (byte)((0x00FF0000 & RGBA) >> 16), (byte)((0x0000FF00 & RGBA) >> 8), (byte)((0x000000FF & RGBA) >> 0));
            else return new Color((byte)(((0xFF000000 & RGBA) >> 24) * alpha), (byte)(((0x00FF0000 & RGBA) >> 16) * alpha), (byte)(((0x0000FF00 & RGBA) >> 8) * alpha), (byte)(((0x000000FF & RGBA) >> 0) * alpha));
        }

        /// <summary> 附加原版闪烁的颜色 </summary>
        /// <param name="R">红原色</param>
        /// <param name="G">绿原色</param>
        /// <param name="B">蓝原色</param>
        /// <param name="A">透明度</param>
        /// <param name="flash">是否闪烁</param>
        public static Color FlashColor(int R, int G, int B, int A = 255, bool flash = true)
        {
            var alpha = Main.mouseTextColor / 255f;
            if (!flash) return new Color((byte)R, (byte)G, (byte)B, (byte)A);
            else return new Color((byte)(R * alpha), (byte)(G * alpha), (byte)(B * alpha), (byte)(A * alpha));
        }

        /// <summary> 附加原版闪烁的颜色 </summary>
        /// <param name="color">输入颜色</param>
        /// <param name="flash">是否闪动</param>
        public static Color FlashColor(Color color, bool flash = true)
        {
            var alpha = Main.mouseTextColor / 255f;
            var R = color.R;
            var G = color.G;
            var B = color.B;
            var A = color.A;
            if (!flash) return new Color((byte)R, (byte)G, (byte)B, (byte)A);
            else return new Color((byte)(R * alpha), (byte)(G * alpha), (byte)(B * alpha), (byte)(A * alpha));
        }

        /// <summary> 附加原版闪烁的颜色 </summary>
        /// <param name="color">输入颜色</param>
        /// <param name="flash">是否闪动</param>
        public static Color Flash(this Color color, bool flash = true)
        {
            var alpha = Main.mouseTextColor / 255f;
            var R = color.R;
            var G = color.G;
            var B = color.B;
            var A = color.A;
            if (!flash) return new Color((byte)R, (byte)G, (byte)B, (byte)A);
            else return new Color((byte)(R * alpha), (byte)(G * alpha), (byte)(B * alpha), (byte)(A * alpha));
        }

        /// <summary> value, type, counter, min, max, addCD, delCD, num </summary>
        public static void UpdateEffectVar(ref int[] variable)
        {
            if (variable[2]-- <= 0)
            {
                if (variable[1] < 0 && variable[0] <= variable[3])
                {
                    variable[1] = 1;
                    variable[2] = variable[5];
                }
                if (variable[1] > 0 && variable[0] >= variable[4])
                {
                    variable[1] = -1;
                    variable[2] = variable[6];
                }
            }
            if (variable[2] <= 0) variable[0] += variable[1] * variable[7];
            if (variable[0] < variable[3]) variable[0] = variable[3];
            if (variable[0] > variable[4]) variable[0] = variable[4];
        }

        public static string ColourfulString(string text, Color[] color, int delay = 0)
        {
            char[] s = text.ToCharArray();
            string res = "";
            int idx = 0;
            int counter = 0;
            for (int i = 0; i < s.Length; i++)
            {
                res += $"[c/{color[idx].ToString()}:{s[i]}]";
                if (s[i] != ' ')
                {
                    if (++counter > delay)
                    {
                        counter = 0;
                        idx = (idx + 1) % color.Length;
                    }
                }
            }
            return res;
        }

        public static void LockScreen(Vector2 center, int lockTime, float lockSpeedAdd = .1f, float lockSpeedMult = 1f, float unlockSpeedAdd = .1f, float unlockSpeedMult = 1f, Player target = null)
        {
            if (target == null) foreach (Player player in Main.player) if (player != null) LockScreen(center, lockTime, lockSpeedAdd, lockSpeedMult, unlockSpeedAdd, unlockSpeedMult, player);
            else
            {
                var gt = target.GetModPlayer<AllPlayer>();
                gt.lockScreenPosition = center;
                gt.lockScreenTime = lockTime;
                gt.lockScreenSpeedAdd = lockSpeedAdd;
                gt.lockScreenSpeedMult = lockSpeedMult;
                gt.unlockScreenSpeedAdd = unlockSpeedAdd;
                gt.unlockScreenSpeedMult = unlockSpeedMult;
            }
        }

        /// <summary> 直接为玩家施加震动视觉效果 </summary>
        /// <param name="time"> 震动时长 </param>
        /// <param name="frequency"> 震动速率 </param>
        /// <param name="amplitude"> 震动幅度 </param>
        /// <param name="attenuation"> 震动衰减速度 </param>
        /// <param name="startOffset"> 初始震动偏移 </param>
        /// <param name="target">目标玩家，为null则全体玩家</param>
        public static void ScreenShake(int time, Vector2 frequency, Vector2 amplitude, float attenuation = .9f, Vector2 startOffset = default, Player target = null)
        {
            if (target == null)
            {
                foreach (Player player in Main.player)
                {
                    if (player != null && player.active)
                    {
                        player.GetModPlayer<AllPlayer>().ScreenShake(time, frequency, amplitude, attenuation, startOffset);
                    }
                }
            }
            else target.GetModPlayer<AllPlayer>().ScreenShake(time, frequency, amplitude, attenuation, startOffset);
        }

        /// <summary> 半径内施加震动视觉效果 </summary>
        /// <param name="center">震动中心</param>
        /// <param name="radius">影响半径（越远震动效果越弱，设为非正数时只对当前坐标的目标完全生效）</param>
        /// <param name="time"> 震动时长 </param>
        /// <param name="frequency"> 震动速率 </param>
        /// <param name="amplitude"> 震动幅度 </param>
        /// <param name="attenuation"> 震动衰减速度 </param>
        /// <param name="startOffset"> 初始震动偏移 </param>
        public static void ScreenShake(Vector2 center, float radius, int time, Vector2 frequency, Vector2 amplitude, float attenuation = .9f, Vector2 startOffset = default)
        {
            foreach (Player player in Main.player)
            {
                Vector2 pos = player.Center;
                float mult = radius > 0 ? MyMath.Max(1 - pos.Distance(center) / radius, 0f) : (player.Center != center).ToInt(1);
                if(mult > 0) player.GetModPlayer<AllPlayer>().ScreenShake((int)(time * mult), frequency * mult, amplitude * mult, attenuation, startOffset);
            }
        }

        /// <summary> 生成爆炸弹幕 </summary>
        /// <param name="source">实体源</param>
        public static GenericExplosion CreateExplosion(Terraria.DataStructures.IEntitySource source)
        {
            Projectile p = CNCUtils.NewProj(source, Vector2.Zero, Vector2.Zero, CNCID.Projectile.LegacyObject[CNCID.Projectile.GenericExplosion], 0, 0);
            GenericExplosion explosion = (GenericExplosion)p.ModProjectile;
            return explosion.
                SetDefaults(null, Vector2.Zero, Vector2.Zero, null).
                SetDamage(0, DamageClass.Default, 0, 0f, 0, 0f, 1f).
                SetAlpha(1f, 0f, 1f).
                SetScale(0f, 0f, 1f).
                SetRotation(0f, 0f, 1f).
                SetColor(Color.White, Color.Transparent, 0f);
        }

        /// <summary> 生成爆炸弹幕 </summary>
        /// <param name="proj">父弹幕</param>
        public static GenericExplosion CreateExplosion(Projectile proj)
        {
            if (proj != null && proj.ModProjectile is Global.Projectiles.CNCProjectile)
            {
                Projectile p = ((Global.Projectiles.CNCProjectile)proj.ModProjectile).NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.GenericExplosion], 0, 0);
                p.direction = proj.direction;
                GenericExplosion explosion = (GenericExplosion)p.ModProjectile;
                return explosion.
                    SetDefaults(null, proj.Center, Vector2.Zero, null).
                    SetDamage(0, DamageClass.Default, 0, 0f, 0, 0f, 1f).
                    SetAlpha(1f, 0f, 1f).
                    SetScale(0f, 0f, 1f).
                    SetRotation(0f, 0f, 1f).
                    SetColor(Color.White, Color.Transparent, 0f);
            }
            return null;
        }

        /// <summary> 生成爆炸弹幕 </summary>
        /// <param name="player">玩家</param>
        public static GenericExplosion CreateExplosion(Player player)
        {
            if (player != null)
            {
                Projectile p = CNCUtils.NewProj(player.GetSource_FromThis(),player.Center,Vector2.Zero, CNCID.Projectile.LegacyObject[CNCID.Projectile.GenericExplosion], 0, 0f, 0, player.whoAmI, 0f, 1f);
                p.direction = player.direction;
                GenericExplosion explosion = (GenericExplosion)p.ModProjectile;
                return explosion.
                    SetDefaults(null, player.Center, Vector2.Zero, null).
                    SetDamage(0, DamageClass.Default, 0, 0f, 0, 0f, 1f).
                    SetAlpha(1f, 0f, 1f).
                    SetScale(0f, 0f, 1f).
                    SetRotation(0f, 0f, 1f).
                    SetColor(Color.White, Color.Transparent, 0f);
            }
            return null;
        }

        /// <summary> 生成爆炸弹幕 </summary>
        /// <param name="npc">父NPC</param>
        public static GenericExplosion CreateExplosion(NPC npc)
        {
            if (npc != null && npc.ModNPC is Global.NPCs.CNCNPC)
            {
                Projectile p = ((Global.NPCs.CNCNPC)npc.ModNPC).NewProjectile(0, 0, CNCID.Projectile.LegacyObject[CNCID.Projectile.GenericExplosion], 0, 0);
                p.direction = npc.direction;
                GenericExplosion explosion = (GenericExplosion)p.ModProjectile;
                return explosion.
                    SetDefaults(null, npc.Center, Vector2.Zero, null).
                    SetDamage(0, DamageClass.Default, 0, 0f, 0, 0f, 1f).
                    SetAlpha(1f, 0f, 1f).
                    SetScale(0f, 0f, 1f).
                    SetRotation(0f, 0f, 1f).
                    SetColor(Color.White, Color.Transparent, 0f);
            }
            return null;
        }

        /// <summary> 生成斩击弹幕 </summary>
        /// <param name="player">生成者</param>
        /// <param name="velocity">方向</param>
        public static GenericSlash CreateSlash(Player player, Vector2 velocity, int hitboxDust = -1)
        {
            if (player != null)
            {
                Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.GenericSlash], 0, 0, player.whoAmI);
                p.direction = player.direction;
                p.rotation = velocity.ToRotation();
                GenericSlash slash = (GenericSlash)p.ModProjectile;
                slash.bindOwner = true;
                slash.dustToShowHitbox = hitboxDust;
                return slash.
                    SetDefaults(null, player.Center, Vector2.One, velocity, 1, 10).
                    SetDamage(0, DamageClass.Default, 0, 0f, 0, 0f, 1f).
                    SetColor(Color.White, Color.Transparent, 0f);
            }
            return null;
        }

        /// <summary> 生成斩击弹幕 </summary>
        /// <param name="npc">生成者</param>
        /// <param name="velocity">方向</param>
        public static GenericSlash CreateSlash(NPC npc, Vector2 velocity, int hitboxDust = -1)
        {
            if (npc != null)
            {
                Projectile p = Projectile.NewProjectileDirect(npc.GetSource_FromThis(), npc.Center, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.GenericSlash], 0, 0, 255);
                p.direction = npc.direction;
                p.rotation = velocity.ToRotation();
                GenericSlash slash = (GenericSlash)p.ModProjectile;
                slash.bindOwner = true;
                slash.dustToShowHitbox = hitboxDust;
                return slash.
                    SetDefaults(null, npc.Center, Vector2.One, velocity, 1, 10).
                    SetDamage(0, DamageClass.Default, 0, 0f, 0, 0f, 1f).
                    SetColor(Color.White, Color.Transparent, 0f);
            }
            return null;
        }

        /// <summary> 生成斩击弹幕 </summary>
        /// <param name="player">生成者</param>
        /// <param name="velocity">方向</param>
        public static GenericSlashHit CreateSlashHit(Projectile projectile, Vector2 velocity)
        {
            if (projectile != null)
            {
                Projectile p = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, velocity, CNCID.Projectile.LegacyObject[CNCID.Projectile.GenericSlashHit], 0, 0, projectile.owner);
                p.rotation = velocity.ToRotation();
                GenericSlashHit slash = (GenericSlashHit)p.ModProjectile;
                return slash.
                    SetDefaults(null, Vector2.One, projectile.Center, velocity, 10).
                    SetColor(Color.White, Color.Transparent, 0f);
            }
            return null;
        }

        /// <summary> Boolean转Int64（0/1） </summary>
        /// <param name="mult">返回值倍数</param>
        public static int ToInt(this bool value, int mult = 1) => value ? mult : 0;

        /// <summary> Boolean转Int64（-1/1） </summary>
        /// <param name="mult">返回值倍数</param>
        public static int ToDirectionInt(this bool value, int mult = 1) => value ? mult : -mult;

        private static Vector2 GetPoint(Rectangle rect, int w, int h)
        {
            float[] w2 = { 0f, rect.Width / 2f, rect.Width };
            float[] h2 = { 0f, rect.Height / 2f, rect.Height };
            return new Vector2(w2[w], h2[h]);
        }

        public static Vector2 TopLeft(this Texture2D texture) => GetPoint(texture.Bounds, 0, 0);
        public static Vector2 Top(this Texture2D texture) => GetPoint(texture.Bounds, 1, 0);
        public static Vector2 TopRight(this Texture2D texture) => GetPoint(texture.Bounds, 2, 0);
        public static Vector2 Left(this Texture2D texture) => GetPoint(texture.Bounds, 0, 0);
        public static Vector2 Center(this Texture2D texture) => GetPoint(texture.Bounds, 1, 1);
        public static Vector2 Right(this Texture2D texture) => GetPoint(texture.Bounds, 2, 1);
        public static Vector2 BottomLeft(this Texture2D texture) => GetPoint(texture.Bounds, 0, 2);
        public static Vector2 Bottom(this Texture2D texture) => GetPoint(texture.Bounds, 1, 2);
        public static Vector2 BottomRight(this Texture2D texture) => GetPoint(texture.Bounds, 2, 2);

        public static void SetData<T>(this NPC npc, T data, int index)
        {
            if (typeof(T).Equals(typeof(int))) npc.GetGlobalNPC<AllNPC>().intData[index] = (int)(object)data;
            if (typeof(T).Equals(typeof(bool))) npc.GetGlobalNPC<AllNPC>().boolData[index] = (bool)(object)data;
            if (typeof(T).Equals(typeof(float))) npc.GetGlobalNPC<AllNPC>().floatData[index] = (float)(object)data;
            if (typeof(T).Equals(typeof(double))) npc.GetGlobalNPC<AllNPC>().doubleData[index] = (double)(object)data;
            if (typeof(T).Equals(typeof(string))) npc.GetGlobalNPC<AllNPC>().stringData[index] = (string)(object)data;
        }

        public static T GetData<T>(this NPC npc, int index)
        {
            T t = default(T);
            if (typeof(T).Equals(typeof(int))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().intData[index];
            if (typeof(T).Equals(typeof(bool))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().boolData[index];
            if (typeof(T).Equals(typeof(float))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().floatData[index];
            if (typeof(T).Equals(typeof(double))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().doubleData[index];
            if (typeof(T).Equals(typeof(string))) t = (T)(object)npc.GetGlobalNPC<AllNPC>().stringData[index];
            return t;
        }

        public static void SetData<T>(this Projectile projectile, T data, int index)
        {
            if (typeof(T).Equals(typeof(int))) projectile.GetGlobalProjectile<AllProjectile>().intData[index] = (int)(object)data;
            if (typeof(T).Equals(typeof(bool))) projectile.GetGlobalProjectile<AllProjectile>().boolData[index] = (bool)(object)data;
            if (typeof(T).Equals(typeof(float))) projectile.GetGlobalProjectile<AllProjectile>().floatData[index] = (float)(object)data;
            if (typeof(T).Equals(typeof(double))) projectile.GetGlobalProjectile<AllProjectile>().doubleData[index] = (double)(object)data;
            if (typeof(T).Equals(typeof(string))) projectile.GetGlobalProjectile<AllProjectile>().stringData[index] = (string)(object)data;
        }

        public static T GetData<T>(this Projectile projectile, int index)
        {
            T t = default(T);
            if (typeof(T).Equals(typeof(int))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().intData[index];
            if (typeof(T).Equals(typeof(bool))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().boolData[index];
            if (typeof(T).Equals(typeof(float))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().floatData[index];
            if (typeof(T).Equals(typeof(double))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().doubleData[index];
            if (typeof(T).Equals(typeof(string))) t = (T)(object)projectile.GetGlobalProjectile<AllProjectile>().stringData[index];
            return t;
        }

        public static Vector2 XX(this Vector2 vector2)
        {
            vector2.Y = vector2.X;
            return vector2;
        }
        public static Vector2 YX(this Vector2 vector2)
        {
            vector2.X = vector2.Y - vector2.X;
            vector2.Y = vector2.Y - vector2.X;
            vector2.X = vector2.Y + vector2.X;
            return vector2;
        }
        public static Vector2 YY(this Vector2 vector2)
        {
            vector2.X = vector2.Y;
            return vector2;
        }
        public static Color NewR(this Color color, int r)
        {
            color.R = (byte)r;
            return color;
        }
        public static Color NewG(this Color color, int g)
        {
            color.G = (byte)g;
            return color;
        }
        public static Color NewB(this Color color, int b)
        {
            color.B = (byte)b;
            return color;
        }
        public static Color NewA(this Color color, int a)
        {
            color.A = (byte)a;
            return color;
        }
    }
}