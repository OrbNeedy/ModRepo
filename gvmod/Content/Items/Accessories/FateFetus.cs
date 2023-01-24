using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class FateFetus : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fate in a pod");
            Tooltip.SetDefault("Gives a secondary passive SP regeneration and 50% chance to not \n" +
                "consume any AP. Decreases overheat SP regeneration, and releases an attack upon \n" +
                "recovering from overheat.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 44;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MorbPlayer>().FateFetus = true;
        }
    }
}
