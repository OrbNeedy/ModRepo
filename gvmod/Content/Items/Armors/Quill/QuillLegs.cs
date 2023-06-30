using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using gvmod.Content.Items.Placeable;
using gvmod.Common.Players;

namespace gvmod.Content.Items.Armors.Quill
{
    [AutoloadEquip(EquipType.Legs)]
    public class QuillLegs : ModItem
    {
        private float increaseInSpeed = 15;
        private float decreaseInSPUse = 20;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSpeed, decreaseInSPUse);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalMoveIncrease = increaseInSpeed / 100f;
            float totalSPUseDecrease = decreaseInSPUse / 100f;
            player.moveSpeed += totalMoveIncrease;
            adept.SPUsageModifier -= totalSPUseDecrease;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SpiritualStone>(9)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
