using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    public class ArmedPhenomenon : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armed Phenomenon");
            Description.SetDefault("The glaive has driven out your septima's power.");
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
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.statDefense += 8;
            player.GetDamage<SeptimaDamageClass>() += 0.1f;
            adept.MaxEXP += 80;
            adept.PrimaryDamageEquipMult += 0.1f;
        }
    }
}
