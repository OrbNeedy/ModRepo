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
	}
}