using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class ForbiddenConverter : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 26;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            ConversionPlayer converter = player.GetModPlayer<ConversionPlayer>();
            converter.Forbidden = true;
        }
    }
}
