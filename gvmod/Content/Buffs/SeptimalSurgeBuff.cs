using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace gvmod.Content.Buffs
{
    public class SeptimalSurgeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Septimal surge");
            Description.SetDefault("Your septima is getting to new heights.");
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
            player.GetModPlayer<SeptimalSurgePlayer>().septimalSurge = true;
        }
    }
}
