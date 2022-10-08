using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CNC.Common.Utils;

namespace CNC.Content
{
    public abstract class VisualEffect
    {
        public bool active;
        public int time;
        public int deadTime;
        public float opacity;
        public float rotation;
        public Vector2 position;
        public Vector2 scale;
        public Entity owner;

        public enum DrawLayer
        {
            behindTiles,
            overTiles,
            overNPCs,
            overProjectlies,
            overPlayers,
            overDusts,
            behindUIs,
            overUIs,
        }

        public static T NewVisualEffect<T>(Vector2 position, float rotation, Entity owner = null)
        {
            if (Activator.CreateInstance<T>() is VisualEffect)
            {
                for (int i = 0; i < CNC.maxEffects; i++)
                {
                    if (VisualEffectManager.effect[i] == null)
                    {
                        VisualEffectManager.effect[i] = Activator.CreateInstance<T>() as VisualEffect;
                        VisualEffectManager.effect[i].active = true;
                        VisualEffectManager.effect[i].time = 0;
                        VisualEffectManager.effect[i].deadTime = 2;
                        VisualEffectManager.effect[i].opacity = 1f;
                        VisualEffectManager.effect[i].rotation = rotation;
                        VisualEffectManager.effect[i].position = position;
                        VisualEffectManager.effect[i].scale = Vector2.One;
                        VisualEffectManager.effect[i].owner = owner;
                        VisualEffectManager.effect[i].SetDefaults();
                        return (T)(object)VisualEffectManager.effect[i];
                    }
                }
            }
            return default(T);
        }

        public void Kill()
        {
            this.active = false;
            this.time = 0;
            this.deadTime = 0;
        }

        public virtual bool CheckDead() => true;
        public virtual void SetDefaults() { }
        public virtual void Update(bool isPaused) { }
        public virtual void Draw(SpriteBatch spriteBatch, DrawLayer layer) { }
    }

    public class VisualEffectManager : ModSystem
    {
        public static VisualEffect[] effect = new VisualEffect[CNC.maxEffects + 1];

        public override void PostUpdateEverything() => this.UpdateVisualEffect();

        private void UpdateVisualEffect()
        {
            for (int i = 0; i < CNC.maxEffects; i++)
            {
                if (VisualEffectManager.effect[i] != null)
                {
                    if (VisualEffectManager.effect[i].time > VisualEffectManager.effect[i].deadTime && VisualEffectManager.effect[i].CheckDead())
                    {
                        VisualEffectManager.effect[i].Kill();
                        VisualEffectManager.effect[i] = null;
                    }
                    else
                    {
                        VisualEffectManager.effect[i].Update(Main.gamePaused);
                        if (!Main.gamePaused) VisualEffectManager.effect[i].time++;
                    }
                }
            }
        }
    }
}