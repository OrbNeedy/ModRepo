using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    internal class AnthemBuff : ModBuff
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
            return false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AdeptMuse>().AnthemBuff = true;
        }
    }
}
