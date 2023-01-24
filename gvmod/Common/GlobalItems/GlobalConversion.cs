using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using System.Net.Sockets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalItems
{
    public class GlobalConversion : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemID.Heart || 
                item.type == ItemID.CandyCane || 
                item.type == ItemID.CandyApple || 
                item.type == ItemID.SugarPlum || 
                item.type == ItemID.SoulCake || 
                item.type == ItemID.Star;
        }

        public override bool OnPickup(Item item, Player player)
        {
            ConversionPlayer conversionNature = player.GetModPlayer<ConversionPlayer>();
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            if (item.type == ItemID.Heart || item.type == ItemID.CandyCane || item.type == ItemID.CandyApple)
            {
                if (conversionNature.Forbidden || conversionNature.Special)
                {
                    adept.SeptimalPower += 20;
                }
                if (conversionNature.Secret)
                {
                    adept.AbilityPower += 1 / 800f;
                }
            } 
            
            if (item.type == ItemID.SugarPlum || item.type == ItemID.SoulCake || item.type == ItemID.Star)
            {
                if (conversionNature.Arcanum || conversionNature.Special)
                {
                    adept.SeptimalPower += 10;
                }
                if (conversionNature.Secret)
                {
                    adept.AbilityPower += 1 / 1200f;
                }
            }
            return true;
        }
    }
}
