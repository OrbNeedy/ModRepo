using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using System;

namespace gvmod.Common.GlobalNPCs
{
    public class KudosLoot : GlobalNPC
    {
        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            base.HitEffect(npc, hitDirection, damage);
            if (!npc.friendly && !npc.immortal)
            {
                AdeptPlayer adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
                int kudosToBeWon = 0;
                kudosToBeWon += (int)((npc.lifeMax * 0.15f + 0.15f * npc.damage) * (1 + npc.defense * 0.15f));
                if (damage < npc.lifeMax)
                {
                    kudosToBeWon *= (int)(damage / npc.lifeMax);
                }
                else
                {
                    kudosToBeWon += 100;
                }

                if (kudosToBeWon == 0) kudosToBeWon = 1;
                if (npc.boss)
                {
                    kudosToBeWon *= 2;
                }
                adept.Kudos += kudosToBeWon;
            }
        }
    }
}
