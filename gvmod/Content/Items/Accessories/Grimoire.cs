using gvmod.Common.Players;
using gvmod.Content;
using gvmod.Content.Buffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class Grimoire : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Transforms the player into an improved armed phenomenon, incresing their \n"
                             + "capabilities significantly."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 40;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.HasBuff<ArmedPhenomenon3>() && !player.HasBuff<ArmedPhenomenon2B>())
            {
                player.AddBuff(ModContent.BuffType<ArmedPhenomenon2A>(), 2, true);
            }
        }
    }
}
