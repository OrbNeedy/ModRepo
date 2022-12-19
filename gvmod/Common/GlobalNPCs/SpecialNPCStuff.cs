using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using gvmod.Content.Buffs;
using Terraria.DataStructures;
using System;

namespace gvmod.Common.GlobalNPCs
{
    public class SpecialNPCStuff : GlobalNPC
    {
        private bool resurrected;
        private bool septimaAmplified;
        public override bool InstancePerEntity => true;

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            return !(target.GetModPlayer<AdeptPlayer>().isUsingSpecialAbility && target.GetModPlayer<AdeptPlayer>().specialInvincibility);
        }

        // For a later release, have a chance for bosses to resurrect, and each time that happens, the boss will 
        // have it's stats adjusted and drop something special, either related to the muse or the djin
        // Maybe make that the way of getting a shard in the first place
    }
}
