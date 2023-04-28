using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace gvmod.Content.Items.Armors.Quill
{
    [AutoloadEquip(EquipType.Legs)]
    public class QuillLegs : ModItem
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
            Item.defense = 4;
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
