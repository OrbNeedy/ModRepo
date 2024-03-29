﻿using gvmod.Common.GlobalNPCs;
using gvmod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Buffs
{
    public class Chaff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ChaffNPC>().Chaff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ChaffedPlayer>().Chaffed = true;
        }
    }
}
