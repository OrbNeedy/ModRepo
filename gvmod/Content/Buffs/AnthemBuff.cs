using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    public class AnthemBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] <= 1)
            {
                Main.NewText("Adding Anthem Debuff");
                player.AddBuff(ModContent.BuffType<AnthemDebuff>(), 18000);
            }

            if (!player.HasBuff<AnthemDebuff>())
            {
                player.GetModPlayer<AdeptMuse>().AnthemBuff = true;
            }
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}
