using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.Graphics.Effects;
using CNC.Common.Utils;
using CNC.Common.Localization;
using CNC.Content.DataStructures;
using CNCUI = CNC.Content.UI;
using CNCGlobal = CNC.Common.Global;

namespace CNC
{
    public class CNC : Mod
    {
        public override Version Version => new Version(0, 0, 3, 3);
        public Version TMLVersion => TModLoaderVersion;
        public static PseudoRandom Random => CNC.random;
        private static PseudoRandom random;
        public static CNC Instance;
        public static int me => Const.me;
        public static Player Me => Const.Me;
        public static int GameMode => Main.masterMode ? 3 : (Main.expertMode ? 2 : 1);
        public static Color? CombatTextColor { get; set; } = null;

        public static bool IsSPClient => Main.netMode == NetmodeID.SinglePlayer;
        public static bool IsMPClient => Main.netMode == NetmodeID.MultiplayerClient;
        public static bool IsMPServer => Main.netMode == NetmodeID.Server;
        public static bool NotSPClient => Main.netMode != NetmodeID.SinglePlayer;
        public static bool NotMPClient => Main.netMode != NetmodeID.MultiplayerClient;
        public static bool NotMPServer => Main.netMode != NetmodeID.Server;
        //注：MPClient是指多人游戏下的客户端（玩家），MPServer是指多人游戏的服务端（系统）

        public const int newStatManaMax2 = 1000000;
        public const int maxClassTypes = CNCID.Clazz.MaxObject;
        public const int maxInscriptions = CNCGlobal.Items.InscriptionSettings.maxInscriptionList;
        public const int maxInscriptionTypes = CNCID.Inscription.MaxObject;
        public const int maxStateTypes = CNCID.State.MaxObject;
        public const int maxParticles = 10000;
        public const int maxEffects = 200;
        public const int maxNPCData = 12;
        public const int maxProjectileData = 6;

        public static Dictionary<string, Texture2D> Image { get; private set; }
        public static Dictionary<string, Effect> Shader { get; private set; }

        public static Texture2D GetTexture(string path)
        {
            try
            {
                if (!path.StartsWith("CNC/")) path = "CNC/" + path;
                return ModContent.Request<Texture2D>(path).Value;
            }
            catch { return null; }
        }

        public static Effect GetEffect(string name)
        {
            try
            {
                name = "CNC/Content/Effects/" + name;
                return ModContent.Request<Effect>(name).Value;
            }
            catch { return null; }
        }

        public static void SetSky(string name, bool activate)
        {
            name = "CNC:" + name + "Sky";
            if (activate && !SkyManager.Instance[name].IsActive() && !(SkyManager.Instance[name] as Content.BiomeSky).active) SkyManager.Instance.Activate(name);
            if (!activate && SkyManager.Instance[name].IsActive() && (SkyManager.Instance[name] as Content.BiomeSky).active) SkyManager.Instance.Deactivate(name);
        }

        public static Filter SetFilter(string name, bool activate)
        {
            name = "CNC:" + name;
            if (activate && !Filters.Scene[name].IsActive()) Filters.Scene.Activate(name);
            if (!activate && Filters.Scene[name].IsActive()) Filters.Scene.Deactivate(name);
            return Filters.Scene[name];
        }

        public override void Load()
        {
            CNCGlobal.Players.ClassesSettings.ClassKey = KeybindLoader.RegisterKeybind(this, "Class Key", Microsoft.Xna.Framework.Input.Keys.Q);
            CNCGlobal.Items.InscriptionSettings.InscriptionSystem.InscriptionKey = KeybindLoader.RegisterKeybind(this, "Inscription Key", Microsoft.Xna.Framework.Input.Keys.V);
            Instance = this;
            TmodFile modFiles = (TmodFile)typeof(Mod).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            CNCLocalization.Load("CNC", modFiles);
            random = new PseudoRandom();
            Logger.Info(CNCLocalization.GetText("Misc.Loaded"));
            Logger.Info($"Mod {CNCLocalization.GetText("Misc.Version")}v{Version}");
            Logger.Info($"TML {CNCLocalization.GetText("Misc.Version")}v{TMLVersion}");

            if (!Main.dedServ)
            {
                this.LoadEffects();
                this.LoadImages();
                this.LoadFilters();
                this.LoadSkies();
            }
            this.LoadHooks();
            base.Load();
        }

        public override void Unload()
        {
            Logger.Info(CNCLocalization.GetText("Misc.Unloaded"));
            Logger.Info($"Mod {CNCLocalization.GetText("Misc.Version")}v{Version}");
            Logger.Info($"TML {CNCLocalization.GetText("Misc.Version")}v{TMLVersion}");
            if (Image != null)
            {
                Image.Clear();
                Image = null;
            }
            CNCLocalization.Unload();
            CNCGlobal.Players.ClassesSettings.ClassKey = null;
            CNCGlobal.Items.InscriptionSettings.InscriptionSystem.InscriptionKey = null;
            random = null;
            Instance = null;
            this.UnloadHooks();
        }

        public override void PostSetupContent()
        {
            if (Main.dedServ) return;
            #region 映射载入
            TmodFile modFiles = (TmodFile)typeof(Mod).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            CNCLocalization.Load("CNC", modFiles);
            Image = new Dictionary<string, Texture2D>();
            Image.Clear();
            Shader = new Dictionary<string, Effect>();
            Shader.Clear();

            int[] count = { 0, 0 };
            int[] countProgress = { 0, 0 };
            List<string> fx = new List<string>();
            foreach (TmodFile.FileEntry file in modFiles)
            {
                if (file.Name.EndsWith(".rawimg")) count[0]++;
                if (file.Name.EndsWith(".fx")) { count[1]++; fx.Add(Path.GetFileNameWithoutExtension(file.Name)); }
            }
            foreach (TmodFile.FileEntry file in modFiles)
            {
                try
                {
                    if (file.Name.EndsWith(".rawimg"))// || file.Name.EndsWith(".png") || file.Name.EndsWith(".jpg"))
                    {
                        string name = file.Name;
                        string path = Path.GetFullPath(name);
                        string key = Path.GetFileNameWithoutExtension(name);
                        while (Image.ContainsKey(key))
                        {
                            int idx = path.LastIndexOf('\\');
                            int idx2 = path.Substring(0, idx).LastIndexOf('\\');
                            key = path.Substring(idx2 + 1, idx - idx2 - 1) + '.' + key;
                            path = path.Substring(0, idx);
                        }
                        countProgress[0]++;
                        Image.Add(key, ModContent.Request<Texture2D>("CNC/" + name.Substring(0, name.IndexOf('.'))).Value);
                        //Logger.Info($"Loading Image: {key}");
                    }
                }
                catch
                {
                    Logger.Info($"Error Loading Image: {Path.GetFileNameWithoutExtension(file.Name)}");
                }
                try
                {
                    if (file.Name.EndsWith(".xnb") && fx.Contains(Path.GetFileNameWithoutExtension(file.Name)))
                    {
                        string name = file.Name;
                        string path = Path.GetFullPath(name);
                        string key = Path.GetFileNameWithoutExtension(name);
                        Shader.Add(key, ModContent.Request<Effect>("CNC/" + name.Substring(0, name.IndexOf('.'))).Value);
                        countProgress[1]++;
                        //Logger.Info($"Loading Shader: {key}");
                    }
                }
                catch
                {
                    Logger.Info($"Error Loading Shader: {Path.GetFileNameWithoutExtension(file.Name)}");
                }
            }
            Logger.Info($"Loaded Image ({countProgress[0]}/{count[0]})");
            Logger.Info($"Loaded Shader ({countProgress[1]}/{count[1]})");
            this.LoadEffects();
            this.LoadImages();
            this.LoadFilters();
            this.LoadSkies();
            #endregion
        }

        private void LoadEffects()
        {
        }

        private void LoadImages()
        {
        }

        private void LoadFilters()
        {
            string[] names =//把滤镜的文件名加到这里即可
            {
                "ShockWave",
                "RadialBlur",
                "Absorption",
                "Swirl",
            };
            foreach (string name in names) (Filters.Scene["CNC:" + name] = new Filter(new FilterShaderData(new Ref<Effect>(CNC.GetEffect(name)), name), EffectPriority.Medium)).Load();
        }

        private void LoadSkies()
        {
            SkyManager.Instance["CNC:SpiritBossSky"] = new Content.Skies.SpiritBossSky();
            SkyManager.Instance["CNC:TranquilitySky"] = new Content.Skies.TranquilitySky();
        }

        private void LoadHooks()
        {
            On.Terraria.Main.DrawDust += new On.Terraria.Main.hook_DrawDust(this.DrawParticle);
            On.Terraria.Main.DrawDust += new On.Terraria.Main.hook_DrawDust(this.DrawVisualEffect);
            On.Terraria.Main.DoDraw_DrawNPCsBehindTiles += new On.Terraria.Main.hook_DoDraw_DrawNPCsBehindTiles(this.DrawVisualEffect);
            On.Terraria.Main.DoDraw_DrawNPCsOverTiles += new On.Terraria.Main.hook_DoDraw_DrawNPCsOverTiles(this.DrawVisualEffect);
            On.Terraria.Main.DrawNPC += new On.Terraria.Main.hook_DrawNPC(this.DrawVisualEffect);
            On.Terraria.Main.DrawProjectiles += new On.Terraria.Main.hook_DrawProjectiles(this.DrawVisualEffect);
            On.Terraria.Main.DrawPlayers_AfterProjectiles += new On.Terraria.Main.hook_DrawPlayers_AfterProjectiles(this.DrawVisualEffect);
            On.Terraria.Main.DrawInterface += new On.Terraria.Main.hook_DrawInterface(this.DrawVisualEffect);
            On.Terraria.Player.DropTombstone += new On.Terraria.Player.hook_DropTombstone(this.NoGrave);
            On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool += new On.Terraria.CombatText.hook_NewText_Rectangle_Color_string_bool_bool(this.ReplaceCombatTextColor);
            IL.Terraria.Player.Update += new ILContext.Manipulator((il) => ResetManaMax(il, newStatManaMax2));
        }

        private void UnloadHooks()
        {
            IL.Terraria.Player.Update -= new ILContext.Manipulator((il) => ResetManaMax(il, newStatManaMax2));
        }

        private void NoGrave(global::On.Terraria.Player.orig_DropTombstone orig, Player self, int coinsOwned, Terraria.Localization.NetworkText deathText, int hitDirection)
        {
            if (!self.All().IsModDeveloper) orig.Invoke(self, coinsOwned, deathText, hitDirection);
        }

        private int ReplaceCombatTextColor(global::On.Terraria.CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
        {
            color = CNC.CombatTextColor ?? color;
            CNC.CombatTextColor = null;
            return orig.Invoke(location, color, text, dramatic, dot);
        }

        private void DrawParticle(global::On.Terraria.Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            for (int i = 0; i < Content.ParticleManager.maxParticles; i++)
                if (Content.ParticleManager.particle[i] != null && Content.ParticleManager.particle[i].active)
                {
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                    Content.ParticleManager.particle[i].Draw(Main.spriteBatch);
                    Main.spriteBatch.End();
                }
        }

        private void DrawVisualEffect(global::On.Terraria.Main.orig_DoDraw_DrawNPCsBehindTiles orig, Main self)
        {
            orig.Invoke(self);
            this.DrawVisualEffect(Content.VisualEffect.DrawLayer.overTiles);
        }

        private void DrawVisualEffect(global::On.Terraria.Main.orig_DoDraw_DrawNPCsOverTiles orig, Main self)
        {
            this.DrawVisualEffect(Content.VisualEffect.DrawLayer.behindTiles);
            orig.Invoke(self);
        }

        private void DrawVisualEffect(global::On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCIndex, bool behindTiles)
        {
            orig.Invoke(self, iNPCIndex, behindTiles);
            this.DrawVisualEffect(Content.VisualEffect.DrawLayer.overNPCs, true);
        }

        private void DrawVisualEffect(global::On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            orig.Invoke(self);
            this.DrawVisualEffect(Content.VisualEffect.DrawLayer.overProjectlies);
        }

        private void DrawVisualEffect(global::On.Terraria.Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
        {
            orig.Invoke(self);
            this.DrawVisualEffect(Content.VisualEffect.DrawLayer.overPlayers);
        }

        private void DrawVisualEffect(global::On.Terraria.Main.orig_DrawInterface orig, Main self, GameTime gametime)
        {
            if (!Main.drawToScreen)
            {
                this.DrawVisualEffect(Content.VisualEffect.DrawLayer.behindUIs);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                Common.Global.Players.AllPlayer.DrawCastTimeBar(CNC.Me);
                Main.spriteBatch.End();
            }
            orig.Invoke(self, gametime);
            if (!Main.drawToScreen)
            {
                this.DrawVisualEffect(Content.VisualEffect.DrawLayer.overUIs);
            }
        }

        private void DrawVisualEffect(global::On.Terraria.Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            this.DrawVisualEffect(Content.VisualEffect.DrawLayer.overDusts);
        }

        private void DrawVisualEffect(Content.VisualEffect.DrawLayer layer, bool end = false)
        {
            for (int i = 0; i < CNC.maxEffects; i++)
                if (Content.VisualEffectManager.effect[i] != null && Content.VisualEffectManager.effect[i].active)
                {
                    if (end) Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                    Content.VisualEffectManager.effect[i].Draw(Main.spriteBatch, layer);
                    Main.spriteBatch.End();
                    if (end) Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                }
        }

        private void ResetManaMax(ILContext il, int max = 400)
        {
            if (max == 400) return;
            ILCursor cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdfld("Terraria.Player", "statManaMax2"), i => i.MatchLdcI4(400)))//thanks to AnotherRpgMod
                cursor.Next.Next.Operand = max;
        }
    }
}
