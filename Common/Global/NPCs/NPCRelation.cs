using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CNC.Common.Global.Projectiles;

namespace CNC.Common.Global.NPCs
{
    public class NPCRelation : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        //public override bool CloneNewInstances => true;

        public NPC npc => _npc;
        private NPC _npc = null;
        public NPC RootNPC => _RootNPC < 0 || _RootNPC >= Main.maxNPCs ? null : Main.npc[_RootNPC];
        public NPC ParentNPC => _ParentNPC < 0 || _ParentNPC >= Main.maxNPCs ? null : Main.npc[_ParentNPC];
        public NPC[] ChildrenNPC { get { ClearDeadChildren(); return _ChildrenNPC.ToArray(); } }
        private int _RootNPC = -1;
        private int _ParentNPC = -1;
        private List<NPC> _ChildrenNPC = null;

        public Projectile RootProjectile => _RootProjectile < 0 || _RootProjectile >= Main.maxProjectiles ? null : Main.projectile[_RootProjectile];
        public Projectile ParentProjectile => _ParentProjectile < 0 || _ParentProjectile >= Main.maxProjectiles ? null : Main.projectile[_ParentProjectile];
        public Projectile[] ChildrenProjectile => _ChildrenProjectile.ToArray();
        private int _RootProjectile = -1;
        private int _ParentProjectile = -1;
        private List<Projectile> _ChildrenProjectile = null;

        /// <summary> 设置该NPC的亲/父NPC（认亲） </summary>
        /// <param name="newParent">亲NPC</param>
        /// <param name="ignoreOld">忽略已有的亲NPC（断绝原本的关系，强制替换）</param>
        public bool SetParent(NPC newParent, bool ignoreOld = false)
        {
            bool flag = newParent != null && newParent.whoAmI >= 0 && newParent.whoAmI < Main.maxNPCs && (this.ParentNPC == null || ignoreOld);
            this._ParentNPC = flag ? newParent.whoAmI : -1;
            return flag;
        }

        /// <summary> 设置该NPC的根/祖NPC（认亲） </summary>
        /// <param name="newRoot">根/祖NPC</param>
        /// <param name="ignoreOld">忽略已有的根/祖NPC（断绝原本的关系，强制替换）</param>
        public bool SetRoot(NPC newRoot, bool ignoreOld = false)
        {
            bool flag = newRoot != null && newRoot.whoAmI >= 0 && newRoot.whoAmI < Main.maxNPCs && (this.RootNPC == null || ignoreOld);
            NPCRelation root = newRoot.GetGlobalNPC<NPCRelation>();
            this._RootNPC = flag ? (root.RootNPC == null ? newRoot.whoAmI : root.RootNPC.whoAmI) : -1;
            return flag;
        }

        /// <summary> 为该NPC添加子NPC </summary>
        /// <param name="child">子NPC</param>
        public bool AddChild(NPC child)
        {
            bool flag = child != null && child.whoAmI >= 0 && child.whoAmI < Main.maxNPCs;
            if (_ChildrenNPC != null && flag)
            {
                this._ChildrenNPC.Add(child);
                child.GetGlobalNPC<NPCRelation>().SetParent(npc, true);
            }
            return flag;
        }

        /// <summary> 设置该NPC的亲/父Projectile（认亲） </summary>
        /// <param name="newParent">亲/父Projectile</param>
        /// <param name="ignoreOld">忽略已有的亲Projectile（断绝原本的关系，强制替换）</param>
        public bool SetParent(Projectile newParent, bool ignoreOld = false)
        {
            bool flag = newParent != null && newParent.whoAmI >= 0 && newParent.whoAmI < Main.maxProjectiles && (this.ParentProjectile == null || ignoreOld);
            this._ParentProjectile = flag ? newParent.whoAmI : -1;
            return flag;
        }

        /// <summary> 设置该NPC的根/祖Projectile（认亲） </summary>
        /// <param name="newRoot">根/祖Projectile</param>
        /// <param name="ignoreOld">忽略已有的根/祖Projectile（断绝原本的关系，强制替换）</param>
        public bool SetRoot(Projectile newRoot, bool ignoreOld = false)
        {
            bool flag = newRoot != null && newRoot.whoAmI >= 0 && newRoot.whoAmI < Main.maxProjectiles && (this.RootProjectile == null || ignoreOld);
            ProjectileRelation root = newRoot.GetGlobalProjectile<ProjectileRelation>();
            this._RootProjectile = flag ? (root.RootProjectile == null ? newRoot.whoAmI : root.RootProjectile.whoAmI) : -1;
            return flag;
        }

        /// <summary> 为该NPC添加子Projectile </summary>
        /// <param name="child">子Projectile</param>
        public bool AddChild(Projectile child)
        {
            bool flag = child != null && child.whoAmI >= 0 && child.whoAmI < Main.maxProjectiles;
            if (_ChildrenNPC != null && flag)
            {
                this._ChildrenProjectile.Add(child);
                child.GetGlobalProjectile<ProjectileRelation>().SetParent(npc, true);
            }
            return flag;
        }

        public int ClearDeadChildren()
        {
            int count = 0;
            if (_ChildrenNPC != null)
            {
                for(int i = 0; i < _ChildrenNPC.Count; i++)
                {
                    if (!_ChildrenNPC[i].active)
                    {
                        _ChildrenNPC.RemoveAt(i);
                        count++;
                    }
                }
            }
            return count;
        }

        public override void SetDefaults(NPC npc)
        {
            npc.GetGlobalNPC<NPCRelation>()._RootNPC = -1;
            npc.GetGlobalNPC<NPCRelation>()._ParentNPC = -1;
            npc.GetGlobalNPC<NPCRelation>()._ChildrenNPC = new List<NPC>();
            npc.GetGlobalNPC<NPCRelation>()._RootProjectile = -1;
            npc.GetGlobalNPC<NPCRelation>()._ParentProjectile = -1;
            npc.GetGlobalNPC<NPCRelation>()._ChildrenProjectile = new List<Projectile>();
            npc.GetGlobalNPC<NPCRelation>()._npc = npc;
        }
    }
}