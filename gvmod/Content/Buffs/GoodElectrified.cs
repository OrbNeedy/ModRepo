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
    public class GoodElectrified : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GoodDebuffPlayer>().rechargingElectricity = true;
            base.Update(player, ref buffIndex);
        }
    }
}
