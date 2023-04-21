using gvmod.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Buffs
{
    public class AlchemicalFieldBuff : ModBuff
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
            player.GetModPlayer<AlchemicalFieldPlayer>().AlchemicalField = true;
        }
    }
}
