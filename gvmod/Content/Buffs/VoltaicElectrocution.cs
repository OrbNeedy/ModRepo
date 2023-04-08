using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using gvmod.Content.Buffs;

namespace gvmod.Content.Buffs
{
    internal class VoltaicElectrocution: ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Voltaic Electrocution");
            // Description.SetDefault("Shocked by Voltaic Chains.\nThat's gonna leave a mark.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 600;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<StrikerElectrifiedPlayer>().strikerElectricityDebuff = true;
        }
    }
}
