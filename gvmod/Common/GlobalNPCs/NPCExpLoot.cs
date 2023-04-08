using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Common.GlobalNPCs
{
    public class NPCExpLoot : GlobalNPC
    {
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (!npc.friendly && !npc.immortal)
            {
                AdeptPlayer lastplayer = Main.player[npc.lastInteraction]?.GetModPlayer<AdeptPlayer>();
                float amount = (npc.lifeMax * 0.1f + 0.16f * npc.damage) * (1 + npc.defense * 0.15f);
                if (hit.Damage < npc.lifeMax)
                {
                    amount *= (float)(hit.Damage / npc.lifeMax);
                }
                if (npc.boss) amount *= 3f;
                if (Main.expertMode) amount *= 1.2f;
                if (Main.masterMode) amount *= 1.5f;
                if (Main.hardMode) amount *= 2f;
                if (lastplayer != null)
                {
                    lastplayer.Experience += (int)amount;
                }
            }
        }
    }
}
