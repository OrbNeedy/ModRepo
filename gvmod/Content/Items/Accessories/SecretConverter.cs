using gvmod.Common.Players;
using gvmod.Content.Buffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class SecretConverter : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Upon picking up health or mana, increases AP.");

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
            converter.Secret = true;
        }
    }
}
