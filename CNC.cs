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
using Terraria.GameContent;
using CNC.Content.UI;
using CNC.Content.UI.MainBar;
using CNC.Content.UI.ClassesMenu;
using CNC.Content.UI.CloneGUI;
using CNC.Common.Global.Players;
using CNC.Common.Localization;
using MonoMod.Cil;

namespace CNC
{
	public class CNC : Mod
	{
        public const int NewManaMax = 400;
        public override Version Version => new Version(0, 0, 2, 3);
        public Version TMLVersion => TModLoaderVersion;
        public static CNC Instance;

        public static Dictionary<string, Texture2D> Image;

        public override void Load()
        {
            IL.Terraria.Player.Update += new ILContext.Manipulator((il) => ResetManaMax(il, NewManaMax));
            ClassesSettings.ClassKey = KeybindLoader.RegisterKeybind(this, "Class Key", "Q");
            Instance = this;
            #region ·´ÉäÔØÈë
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
            IL.Terraria.Player.Update -= new ILContext.Manipulator((il) => ResetManaMax(il, NewManaMax));
            Logger.Info(CNCLocalization.GetText("Misc.Unloaded"));
            Logger.Info($"Mod {CNCLocalization.GetText("Misc.Version")}v{Version}");
            Logger.Info($"TML {CNCLocalization.GetText("Misc.Version")}v{TMLVersion}");
            if (Image != null)
            {
                Image.Clear();
                Image = null;
            }
            CNCLocalization.Unload();
            ClassesSettings.ClassKey = null;
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

            public UserInterface [] UI = new UserInterface[4];
            internal Content.UI.UIState[] UIState;
            public bool[] Visible => new bool[]
            {
                !ClassesMenu.Visible,
                ClassesMenu.Visible,
                true,
                true,
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
                UIState = new Content.UI.UIState[]
                {
                    new MainBar(),
                    new ClassesMenu(),
                    new DarkScreen(),
                    new CloneGUI(),
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
                base.ModifyInterfaceLayers(layers);
                AppendUI(layers, UI[0], Visible[0], 33, "Function Bar");
                AppendUI(layers, UI[1], Visible[1], -1, "Classes Menu");
                AppendUI(layers, UI[2], Visible[2], -1, "Dark Screen Effect");
                AppendUI(layers, UI[3], Visible[3], 25, "Clone Resource Bars");
                int idx = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (idx >= 0) layers[idx].Active = false; // layers.RemoveAt(idx);
            }

            public override void UpdateUI(GameTime gameTime)
            {
                base.UpdateUI(gameTime);
                for (int i = 0; i < UIState.Length; i++)
                    if (UI[i] != null && Visible[i]) UI[i].Update(gameTime);
            }

            public static void AppendUI(List<GameInterfaceLayer> layers, UserInterface UI, bool showing, string nameUI = "None", string layerName = "Vanilla: Mouse Text")
            {
                int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals(layerName));
                AppendUI(layers, UI, showing, mouseTextIndex, nameUI);
            }

            public static void AppendUI(List<GameInterfaceLayer> layers, UserInterface UI, bool showing, int layerID = 33, string nameUI = "None")
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
