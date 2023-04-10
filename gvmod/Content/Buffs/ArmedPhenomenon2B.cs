using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    public class ArmedPhenomenon2B : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            BetterArmedPhenomenonPlayer betterPlayer = player.GetModPlayer<BetterArmedPhenomenonPlayer>();
            betterPlayer.sumeragiType = true;
        }
    }
}
