using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Projectiles;
using Terraria.GameContent.Creative;

namespace gvmod.Content.Items.Weapons
{
	public class ElectricWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Whip");
			Tooltip.SetDefault("A whip charged with electric energy.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.SummonMeleeSpeed;
			Item.damage = 16;
			Item.knockBack = 15;
			Item.rare = ItemRarityID.Green;

			Item.shoot = ModContent.ProjectileType<ElectricWhipProjectile>();
			Item.shootSpeed = 8;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.UseSound = SoundID.Item152;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.Register();
		}
	}
}