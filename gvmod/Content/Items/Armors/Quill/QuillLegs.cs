using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using gvmod.Common.Players;

namespace gvmod.Content.Items.Armors.Quill
{
    [AutoloadEquip(EquipType.Legs)]
    public class QuillLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Protective Leggins");
            /* Tooltip.SetDefault("Your legs feel more powerful.\n" +
                "Increses movement speed by 10%."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
