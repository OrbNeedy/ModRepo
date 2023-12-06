using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using Terraria.ID;

namespace gvmod.Content.Buffs
{
    public class AnthemDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff<AnthemBuff>())
            {
                player.DelBuff(player.FindBuffIndex(ModContent.BuffType<AnthemBuff>()));
            }
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}
