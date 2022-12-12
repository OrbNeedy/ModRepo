using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using gvmod.Content.Buffs;

namespace gvmod.Common.GlobalNPCs
{
    internal class ChainedNPC: GlobalNPC
    {
        public int ChainedTime { get; set; }
        public override bool InstancePerEntity => true;

        public override bool PreAI(NPC npc)
        {
            if (ChainedTime > 0) return false;
            return true;
        }

        public override void PostAI(NPC npc)
        {
            ChainedTime--;
            if (ChainedTime > 0)
            {
                npc.velocity *= 0.5f;
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
        }
    }
}
