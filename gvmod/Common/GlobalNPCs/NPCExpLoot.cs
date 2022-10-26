using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Common.GlobalNPCs
{
    public class NPCExpLoot : GlobalNPC
    {
        // TODO: Make it multiplayer compatible

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (target.GetModPlayer<AdeptPlayer>().isUsingSpecialAbility) return false;
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }

        public override bool PreKill(NPC npc)
        {
            if (!npc.friendly)
            {
                AdeptPlayer lastplayer = Main.player[npc.lastInteraction]?.GetModPlayer<AdeptPlayer>();
                float amount = (float)((npc.lifeMax * 0.10f + 0.16f * npc.damage) * (1 + npc.defense * 0.15f));
                if (npc.boss) amount *= 3f;
                if (Main.expertMode) amount *= 1.2f;
                if (Main.masterMode) amount *= 1.5f;
                if (Main.hardMode) amount *= 2f;
                if (lastplayer != null)
                {
                    lastplayer.experience += (int)amount;
                }
            }
            return base.PreKill(npc);
        }
    }
}
