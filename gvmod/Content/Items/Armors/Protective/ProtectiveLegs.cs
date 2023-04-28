using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace gvmod.Content.Items.Armors.Protective
{
    [AutoloadEquip(EquipType.Legs)]
    public class ProtectiveLegs : ModItem
    {
        private float increaseInSpeed = 10;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Protective Leggins");
            /* Tooltip.SetDefault("Easy to move with these.\n" +
                "Increses movement speed by 10%."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSpeed);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += (increaseInSpeed/100f);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
