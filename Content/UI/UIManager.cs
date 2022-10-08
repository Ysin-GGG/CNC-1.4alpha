using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;

namespace CNC.Content.UI
{
    public class UIManager : ModSystem
    {
        public static UIManager Instance;

        public UserInterface[] UI = new UserInterface[7];
        internal UIState[] UIState;
        public bool[] Visible => new bool[]
        {
                !(ClassesMenu.ClassesMenu.Visible || InscriptionMenu.InscriptionMenu.Visible),
                ClassesMenu.ClassesMenu.Visible,
                true,
                true,
                InscriptionMenu.InscriptionMenu.Visible,
                InscriptionMenu.WeaponList.Visible,
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
            UIState = new UIState[]
            {
                    new MainBar.MainBar(),
                    new ClassesMenu.ClassesMenu(),
                    new ClassesMenu.DarkScreen(),
                    new Clone.CloneGUI(),
                    new InscriptionMenu.InscriptionMenu(),
                    new InscriptionMenu.WeaponList(),
                    new InscriptionMenu.DarkScreen(),
            };
            for (int i = 0; i < UIState.Length; i++)
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

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (SkyManager.Instance["CNC:SpiritBossSky"].IsActive())
            {
                var sky = (Skies.SpiritBossSky)SkyManager.Instance["CNC:SpiritBossSky"];
                sky.Update();
                backgroundColor = Color.Lerp(backgroundColor, sky.skyColor, sky.opacity * sky.intensity);
            }
        }
    }
}