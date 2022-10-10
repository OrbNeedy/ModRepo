using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Projectiles;
using gvmod.Content.Items.Ammo;
using Terraria.GameContent.Creative;

namespace gvmod.Content.Items.Weapons
{
    public class DartLeader : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fw-005 (TYPE-0) D:T/LEADER");
			Tooltip.SetDefault("A pistol that shoots hair darts, allows Azure Striker and Azure Thunderclap to \nmark an enemy and shock them with their Flashfield to deal damage.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
			Item.damage = 2;
			Item.knockBack = 0;
			Item.rare = ItemRarityID.Green;

			Item.shoot = ModContent.ProjectileType<HairDartProjectile>();
			Item.useAmmo = ModContent.ItemType<HairDart>();
			Item.shootSpeed = 10;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.UseSound = SoundID.Camera;
			Item.noMelee = true;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.Register();
		}
	}
}
