using gvmod.Content.Items.Ammo;
using gvmod.Content.Projectiles;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace gvmod.Content.Items.Weapons
{
    public class Border2 : ModItem
    {
        // No, it does not tag anything, it will most likely not be able to do so without the White Tiger
        // White Tiger will not happen anytime soon
        // Therefore, tagging with the Border II will not happen anytime soon
        private bool greedSnatcher;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 78;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Orange;

            Item.shoot = ModContent.ProjectileType<PhotonLaser>();
            Item.useAmmo = 0;
            Item.shootSpeed = 16;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.UseSound = SoundID.Item33;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                return base.Shoot(player, source, position, velocity, type, damage, knockback);
            }
            else
            {
                return false;
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            if (player.altFunctionUse != 2)
            {
                if (greedSnatcher)
                {
                    velocity *= 0.4f;
                    type = ModContent.ProjectileType<GreedSnatcher>();
                } else
                {
                    damage = (int)(damage * 1.15);
                }
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool AltFunctionUse(Player player)
        {
            if (!greedSnatcher)
            {
                Item.useAmmo = ModContent.ItemType<GreedSnatcherBullet>();
                Item.UseSound = SoundID.DD2_EtherianPortalOpen;
                Item.useAnimation = 15;
                Item.useTime = 5;
                Item.reuseDelay = 20;
                greedSnatcher = true;
                Main.NewText("Switched to Greed Snatcher bullets.");
            }
            else
            {
                Item.useAmmo = 0;
                Item.UseSound = SoundID.Item33;
                Item.useAnimation = 8;
                Item.useTime = 8;
                Item.reuseDelay = 0;
                greedSnatcher = false;
                Main.NewText("Switched to Photon laser.");
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Border>()
                .AddIngredient(ItemID.ShroomiteBar, 7)
                .AddIngredient(ItemID.LihzahrdPowerCell, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
