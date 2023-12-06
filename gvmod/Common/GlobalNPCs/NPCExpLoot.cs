using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Common.GlobalNPCs
{
    public class NPCExpLoot : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (Main.player[npc.lastInteraction].GetModPlayer<AlchemicalFieldPlayer>().AlchemicalField)
            {
                npc.value *= 2;
            }
            base.OnKill(npc);
        }

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (!npc.friendly && !npc.immortal)
            {
                AdeptPlayer lastplayer = Main.player[npc.lastInteraction]?.GetModPlayer<AdeptPlayer>();
                float amount;

                amount = 1 + (npc.rarity * 5) + (((npc.lifeMax * 2) + npc.damage) * (1 + (npc.defense * 0.75f)));
                if (hit.Damage < npc.lifeMax)
                {
                    amount *= hit.Damage / npc.lifeMax;
                }

                if (npc.boss && npc.realLife == -1) amount *= 3f;
                if (Main.expertMode) amount *= 1.5f;
                if (Main.masterMode) amount *= 2f;
                if (Main.hardMode) amount *= 2f;
                if (lastplayer != null)
                {
                    if (Main.player[npc.lastInteraction].GetModPlayer<AlchemicalFieldPlayer>().AlchemicalField) amount *= 2;
                    lastplayer.Experience += (int)amount;
                }
            }
        }
    }
}
