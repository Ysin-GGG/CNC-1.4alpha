using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Global.NPCs;

namespace CNC.Common.Global.Projectiles
{
    public class ProjectileRelation : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        public Projectile projectile => _projectile;
        private Projectile _projectile = null;
        public NPC RootNPC => _Root1 < 0 || _Root1 >= Main.maxNPCs ? null : Main.npc[_Root1];
        public NPC ParentNPC => _Parent1 < 0 || _Parent1 >= Main.maxNPCs ? null : Main.npc[_Parent1];
        public NPC[] ChildrenNPC => _Children1.ToArray();
        private int _Root1 = -1;
        private int _Parent1 = -1;
        private List<NPC> _Children1 = null;

        public Projectile RootProjectile => _Root2 < 0 || _Root2 >= Main.maxProjectiles ? null : Main.projectile[_Root2];
        public Projectile ParentProjectile => _Parent2 < 0 || _Parent2 >= Main.maxProjectiles ? null : Main.projectile[_Parent2];
        public Projectile[] ChildrenProjectile => _Children2.ToArray();
        private int _Root2 = -1;
        private int _Parent2 = -1;
        private List<Projectile> _Children2 = null;

        public bool SetParent(NPC newParent, bool ignoreOld = false)
        {
            bool flag = newParent != null && newParent.whoAmI >= 0 && newParent.whoAmI < Main.maxNPCs && (this.ParentNPC == null || ignoreOld);
            this._Parent1 = flag ? newParent.whoAmI : -1;
            return flag;
        }

        public bool SetRoot(NPC newRoot, bool ignoreOld = false)
        {
            bool flag = newRoot != null && newRoot.whoAmI >= 0 && newRoot.whoAmI < Main.maxNPCs && (this.RootNPC == null || ignoreOld);
            NPCRelation root = newRoot.GetGlobalNPC<NPCRelation>();
            this._Root1 = flag ? (root.RootNPC == null ? newRoot.whoAmI : root.RootNPC.whoAmI) : -1;
            return flag;
        }

        public bool AddChild(NPC child)
        {
            bool flag = child != null && child.whoAmI >= 0 && child.whoAmI < Main.maxNPCs;
            if (_Children1 != null && flag)
            {
                this._Children1.Add(child);
                child.GetGlobalNPC<NPCRelation>().SetParent(projectile, true);
            }
            return flag;
        }

        public bool SetParent(Projectile newParent, bool ignoreOld = false)
        {
            bool flag = newParent != null && newParent.whoAmI >= 0 && newParent.whoAmI < Main.maxProjectiles && (this.ParentProjectile == null || ignoreOld);
            this._Parent2 = flag ? newParent.whoAmI : -1;
            return flag;
        }

        public bool SetRoot(Projectile newRoot, bool ignoreOld = false)
        {
            bool flag = newRoot != null && newRoot.whoAmI >= 0 && newRoot.whoAmI < Main.maxProjectiles && (this.RootProjectile == null || ignoreOld);
            ProjectileRelation root = newRoot.GetGlobalProjectile<ProjectileRelation>();
            this._Root2 = flag ? (root.RootProjectile == null ? newRoot.whoAmI : root.RootProjectile.whoAmI) : -1;
            return flag;
        }

        public bool AddChild(Projectile child)
        {
            bool flag = child != null && child.whoAmI >= 0 && child.whoAmI < Main.maxProjectiles;
            if (_Children1 != null && flag)
            {
                this._Children2.Add(child);
                child.GetGlobalProjectile<ProjectileRelation>().SetParent(projectile, true);
            }
            return flag;
        }

        public int ClearDeadChildren()
        {
            int count = 0;
            if (_Children1 != null)
            {
                for(int i = 0; i < _Children1.Count; i++)
                {
                    if (!_Children1[i].active)
                    {
                        _Children1.RemoveAt(i);
                        count++;
                    }
                }
            }
            return count;
        }

        public override void SetDefaults(Projectile projectile)
        {
            projectile.GetGlobalProjectile<ProjectileRelation>()._Root1 = -1;
            projectile.GetGlobalProjectile<ProjectileRelation>()._Parent1 = -1;
            projectile.GetGlobalProjectile<ProjectileRelation>()._Children1 = new List<NPC>();
            projectile.GetGlobalProjectile<ProjectileRelation>()._Root2 = -1;
            projectile.GetGlobalProjectile<ProjectileRelation>()._Parent2 = -1;
            projectile.GetGlobalProjectile<ProjectileRelation>()._Children2 = new List<Projectile>();
            projectile.GetGlobalProjectile<ProjectileRelation>()._projectile = projectile;
        }
    }
}