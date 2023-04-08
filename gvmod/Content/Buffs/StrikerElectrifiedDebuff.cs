using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    public class StrikerElectrifiedDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Azure electricity");
            // Description.SetDefault("Getting shocked by an azure striker. \nResist with: Azure Striker.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 6;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<StrikerElectrifiedPlayer>().strikerElectricityDebuff = true;
        }
    }
}