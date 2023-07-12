using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Ammo
{
    public class BeowulfBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 6;

            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 0, 0, 30);
            Item.rare = ItemRarityID.Green;

            Item.ammo = Item.type;
        }

        public override void AddRecipes()
        {
            CreateRecipe(132)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddIngredient(ItemID.Ruby, 2)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(132)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddIngredient(ItemID.Diamond, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
