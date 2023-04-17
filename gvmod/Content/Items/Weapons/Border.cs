using gvmod.Content.Items.Ammo;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Weapons
{
    public class Border : ModItem
    {
        private bool greedSnatcher;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 32;
            Item.knockBack = 8;
            Item.rare = ItemRarityID.Orange;

            Item.shoot = ModContent.ProjectileType<Beowulf>();
            Item.useAmmo = ModContent.ItemType<BeowulfBullet>();
            Item.shootSpeed = 14;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                return base.Shoot(player, source, position, velocity, type, damage, knockback);
            } else
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
                    velocity *= 0.6f;
                    type = ModContent.ProjectileType<GreedSnatcher>();
                }
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool AltFunctionUse(Player player)
        {
            if (!greedSnatcher)
            {
                Item.useAmmo = ModContent.ItemType<GreedSnatcherBullet>();
                Item.UseSound = SoundID.Shatter;
                Item.useAnimation = 15;
                Item.useTime = 5;
                Item.reuseDelay = 20;
                greedSnatcher = true;
                Main.NewText("Switched to Greed Snatcher bullets.");
            } else
            {
                Item.useAmmo = ModContent.ItemType<BeowulfBullet>();
                Item.UseSound = SoundID.Item11;
                Item.useAnimation = 15;
                Item.useTime = 15;
                Item.reuseDelay = 0;
                greedSnatcher = false;
                Main.NewText("Switched to Beowulf bullets.");
            }
            return false;
        }
    }
}
