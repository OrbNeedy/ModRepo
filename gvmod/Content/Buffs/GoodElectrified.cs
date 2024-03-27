using gvmod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Buffs
{
    public class GoodElectrified : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GoodDebuffPlayer>().rechargingElectricity = true;
            base.Update(player, ref buffIndex);
        }
    }
}
