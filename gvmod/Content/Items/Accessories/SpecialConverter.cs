using gvmod.Common.Players;
using gvmod.Content.Buffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class SpecialConverter : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Upon picking up health or mana, increases SP.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 56;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            ConversionPlayer converter = player.GetModPlayer<ConversionPlayer>();
            converter.Special = true;
        }
    }
}
