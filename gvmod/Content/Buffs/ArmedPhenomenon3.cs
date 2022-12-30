using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    public class ArmedPhenomenon3 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armed Phenomenon");
            Description.SetDefault("The true glaive has driven out your septima's power.");
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
            betterPlayer.improved = true;
        }
    }
}
