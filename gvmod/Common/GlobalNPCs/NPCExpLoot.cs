using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Common.GlobalNPCs
{
    public class NPCExpLoot : GlobalNPC
    {
        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if (!npc.friendly && !npc.immortal)
            {
                AdeptPlayer lastplayer = Main.player[npc.lastInteraction]?.GetModPlayer<AdeptPlayer>();
                float amount = (npc.lifeMax * 0.1f + 0.16f * npc.damage) * (1 + npc.defense * 0.15f);
                if (damage < npc.lifeMax)
                {
                    amount *= (float)(damage / npc.lifeMax);
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
