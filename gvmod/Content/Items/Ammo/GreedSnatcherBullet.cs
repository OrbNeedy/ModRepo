using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace gvmod.Content.Items.Ammo
{
    public class GreedSnatcherBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 6;

            Item.damage = 18;
            Item.DamageType = ModContent.GetInstance<SeptimaDamageClass>();

            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 0, 0, 30);
            Item.rare = ItemRarityID.Green;

            Item.ammo = Item.type;
        }

        public override void AddRecipes()
        {
            // Future recipes might replace soul of might with Carrera's septimosome, if I ever add bosses
            CreateRecipe(33)
                .AddIngredient(ItemID.TungstenBar, 10)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient<BeowulfBullet>(33)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe(33)
                .AddIngredient(ItemID.SilverBar, 10)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient<BeowulfBullet>(33)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
