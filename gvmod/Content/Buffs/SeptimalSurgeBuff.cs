using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    public class SeptimalSurgeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override bool RightClick(int buffIndex)
        {
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.GetDamage<SeptimaDamageClass>() *= 2;
            adept.PrimaryDamageEquipMult *= 2;
            adept.SpecialDamageEquipMult *= 2;
            adept.SecondaryDamageEquipMult *= 2;
        }
    }
}
