using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using CNC.Common.Utils;
using CNC.Common.Localization;
using MonoMod.Cil;
using CNCUI = CNC.Content.UI;
using CNCGlobal = CNC.Common.Global;

namespace CNC
{
	public class CNC : Mod
	{
        public override Version Version => new Version(0, 0, 3, 0);
        public Version TMLVersion => TModLoaderVersion;
        public static CNC Instance;
        public static int me => Const.me;
        public static Player Me => Const.Me;
        public const int newStatManaMax2 = 1000000;
        public const int maxParticle = Content.ParticleRenderer.maxParticles;
        public const int maxClassTypes = CNCID.Clazz.MaxObject;
        public const int maxInscriptions = CNCGlobal.Items.InscriptionSettings.maxInscriptionList;
        public const int maxInscriptionTypes = CNCID.Inscription.MaxObject;
        public const int maxStateTypes = CNCID.State.MaxObject;

        public static Dictionary<string, Texture2D> Image;

        public override void Load()
        {
            IL.Terraria.Player.Update += new ILContext.Manipulator((il) => ResetManaMax(il, newStatManaMax2));
            CNCGlobal.Players.ClassesSettings.ClassKey = KeybindLoader.RegisterKeybind(this, "Class Key", Microsoft.Xna.Framework.Input.Keys.Q);
            CNCGlobal.Items.InscriptionSettings.InscriptionSystem.InscriptionKey = KeybindLoader.RegisterKeybind(this, "Inscription Key", Microsoft.Xna.Framework.Input.Keys.V);
            Instance = this;
            #region 映射载入
            TmodFile modFiles = (TmodFile)typeof(Mod).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            CNCLocalization.Load("CNC", modFiles);
            Image = new Dictionary<string, Texture2D>();
            Image.Clear();
            foreach (TmodFile.FileEntry file in modFiles)
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
                    Image.Add(key, ModContent.Request<Texture2D>("CNC/" + name.Substring(0, name.IndexOf('.'))).Value);
                }
            }
            #endregion
            Logger.Info(CNCLocalization.GetText("Misc.Loaded"));
            Logger.Info($"Mod {CNCLocalization.GetText("Misc.Version")}v{Version}");
            Logger.Info($"TML {CNCLocalization.GetText("Misc.Version")}v{TMLVersion}");
            
            //AddEquipTexture(ModContent.GetModItem(ModContent.ItemType<Content.Items.Developers.HHvs.HHvsRobe>()), EquipType.Legs, "CNC/Content/Items/Developers/HHvs/HHvsRobe_Legs");
            //UIManager.Instance.Load();
        }

        public override void Unload()
        {
            IL.Terraria.Player.Update -= new ILContext.Manipulator((il) => ResetManaMax(il, newStatManaMax2));
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
            Instance = null;
        }

        private void ResetManaMax(ILContext il, int max = 400)
        {
            if (max == 400) return;
            ILCursor cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdfld("Terraria.Player", "statManaMax2"), i => i.MatchLdcI4(400)))//thanks to AnotherRpgMod
                cursor.Next.Next.Operand = max;
        }

        class UIManager : ModSystem
        {
            public static UIManager Instance;

            public UserInterface [] UI = new UserInterface[8];
            internal CNCUI.UIState[] UIState;
            public bool[] Visible => new bool[]
            {
                !(CNCUI.ClassesMenu.ClassesMenu.Visible || CNCUI.InscriptionMenu.InscriptionMenu.Visible),
                CNCUI.ClassesMenu.ClassesMenu.Visible,
                true,
                true,
                CNCUI.InscriptionMenu.InscriptionMenu.Visible,
                CNCUI.InscriptionMenu.WeaponList.Visible,
                true,
                CNCUI.Misc.DrawParticle.Visible,
            };

            public override void Load()
            {
                Instance = this;
                LoadUI();
            }

            public override void Unload()
            {
                Instance = null;
            }

            public void LoadUI()
            {
                if (Main.dedServ) return;
                UIState = new CNCUI.UIState[]
                {
                    new CNCUI.MainBar.MainBar(),
                    new CNCUI.ClassesMenu.ClassesMenu(),
                    new CNCUI.ClassesMenu.DarkScreen(),
                    new CNCUI.Clone.CloneGUI(),
                    new CNCUI.InscriptionMenu.InscriptionMenu(),
                    new CNCUI.InscriptionMenu.WeaponList(),
                    new CNCUI.InscriptionMenu.DarkScreen(),
                    new CNCUI.Misc.DrawParticle(),
                };
                for(int i = 0; i < UIState.Length; i++)
                {
                    UIState[i].Activate();
                    UI[i] = new UserInterface();
                    UI[i].SetState(UIState[i]);
                }
            }

            public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
            {
                AppendUI(layers, UI[0], Visible[0], 33, "Function Bar");
                AppendUI(layers, UI[1], Visible[1], -1, "Classes Menu");
                AppendUI(layers, UI[2], Visible[2], -1, "Dark Screen Effect 1");
                AppendUI(layers, UI[3], Visible[3], 25, "Clone Resource Bars");
                AppendUI(layers, UI[4], Visible[4], -1, "Inscription Menu");//Enhancement
                AppendUI(layers, UI[5], Visible[5], -1, "Weapon List");
                AppendUI(layers, UI[6], Visible[6], -1, "Dark Screen Effect 2");
                AppendUI(layers, UI[7], Visible[7], 0, "Draw Particle");
                int idx = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (idx >= 0) layers[idx].Active = false; // layers.RemoveAt(idx);
                idx = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                //if (idx >= 0) layers[idx].Active = false; // layers.RemoveAt(idx);
            }

            public override void UpdateUI(GameTime gameTime)
            {
                for (int i = 0; i < UIState.Length; i++)
                    if (UI[i] != null && Visible[i]) UI[i].Update(gameTime);
            }

            private static void AppendUI(List<GameInterfaceLayer> layers, UserInterface UI, bool showing, string nameUI = "None", string layerName = "Vanilla: Mouse Text")
            {
                int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals(layerName));
                AppendUI(layers, UI, showing, mouseTextIndex, nameUI);
            }

            private static void AppendUI(List<GameInterfaceLayer> layers, UserInterface UI, bool showing, int layerID = 33, string nameUI = "None")
            {
                LegacyGameInterfaceLayer ui = new LegacyGameInterfaceLayer(
                        "CNC: " + nameUI,
                        delegate { if (showing) UI.Draw(Main.spriteBatch, new GameTime()); return true; },
                        InterfaceScaleType.UI);
                if (layerID != -1) layers.Insert(layerID, ui);
                else layers.Add(ui);
            }
        }
    }
}
