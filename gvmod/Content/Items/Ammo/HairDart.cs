using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Ammo
{
    public class HairDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bolt");
            // Tooltip.SetDefault("A dart made out of your hair. \nThe name \"Hair dart\" is way too uncool.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 10;

            Item.damage = 1;
            Item.DamageType = ModContent.GetInstance<SeptimaDamageClass>();

            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.rare = ItemRarityID.Yellow;
            //Item.shoot = ModContent.ProjectileType<Projectiles.HairDartProjectile>();

            Item.ammo = Item.type;
        }

        public override void AddRecipes()
        {
            CreateRecipe(33)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
