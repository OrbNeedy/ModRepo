using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
namespace gvmod.Content.Buffs
{
    internal class Chained: ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chained");
            // Description.SetDefault("You've been pierced by a chain and can't move.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<StrikerElectrifiedPlayer>().strikerElectricityDebuff = true;
        }
    }
}
