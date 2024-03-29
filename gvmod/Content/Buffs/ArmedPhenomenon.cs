﻿using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;

namespace gvmod.Content.Buffs
{
    public class ArmedPhenomenon : ModBuff
    {
        public override void SetStaticDefaults()
        {
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
            adept.MaxSeptimalPower2 += 80;
            adept.PrimaryDamageEquipMult += 0.05f;
            adept.SecondaryDamageEquipMult += 0.05f;
            adept.SpecialDamageEquipMult += 0.05f;
        }
    }
}
