using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalNPCs
{
    public class ChaffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool Chaff { get; set; }

        public override void Load()
        {
            Chaff = false;
        }

        public override void ResetEffects(NPC npc)
        {
            Chaff = false;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (Chaff)
            {
                modifiers.FinalDamage *= 1.6f;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (Chaff)
            {
                modifiers.FinalDamage *= 1.45f;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage *= 0.9f;
            base.ModifyHitPlayer(npc, target, ref modifiers);
        }
    }
}
