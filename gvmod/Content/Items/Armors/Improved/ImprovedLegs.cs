using gvmod.Common.Players;
using gvmod.Content.Items.Armors.Protective;
using gvmod.Content.Items.Placeable;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Improved
{
    [AutoloadEquip(EquipType.Legs)]
    public class ImprovedLegs : ModItem
    {
        private float increaseInSpeed = 10;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSpeed);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            float totalMoveIncrease = increaseInSpeed / 100f;
            player.moveSpeed += totalMoveIncrease;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ProtectiveLegs>()
                .AddIngredient<SpiritualStone>(5)
                .AddRecipeGroup("CrimtaneBar", 5)
                .Register();
        }
    }
}
