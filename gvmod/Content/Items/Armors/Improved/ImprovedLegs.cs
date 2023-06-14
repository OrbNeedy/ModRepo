using gvmod.Common.Players;
using gvmod.Content.Items.Armors.Protective;
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
        private float increaseInSpeed = 15;
        private float decreaseInSPUse = 20;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Protective Leggins");
            /* Tooltip.SetDefault("Thunder thights.\n" +
                "Increses movement speed by 15% and reduces SP usage."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSpeed, decreaseInSPUse);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalMoveIncrease = increaseInSpeed / 100f;
            float totalSPUseDecrease = decreaseInSPUse / 100f;
            player.moveSpeed += 0.15f;
            adept.SPUsageModifier -= 0.8f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
