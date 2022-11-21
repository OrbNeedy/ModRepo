using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using gvmod.Content.Buffs;

namespace gvmod.Common.GlobalNPCs
{
    internal class ChainedNPC: GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            return !npc.HasBuff<Chained>();
        }

        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff<Chained>())
            {
                npc.velocity *= 0.5f;
            }
        }
    }
}
