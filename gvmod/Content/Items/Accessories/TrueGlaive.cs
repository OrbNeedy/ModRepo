using gvmod.Common.Players;
using gvmod.Content;
using gvmod.Content.Buffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class TrueGlaive : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Binding brand");
            /* Tooltip.SetDefault("Transforms the player into the ultimate armed phenomenon, incresing their \n"
                             + "capabilities a lot."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 78;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<ArmedPhenomenon3>(), 2, true);
        }
    }
}
