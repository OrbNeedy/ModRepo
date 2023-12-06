using gvmod.Content.Projectiles;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace gvmod.Content.Items.Weapons
{
    public class Divider : ModItem
    {
        private bool breakshift;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 48;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Red;

            Item.shoot = ModContent.ProjectileType<SmallRazor>();
            Item.useAmmo = 0;
            Item.shootSpeed = 14;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 10;
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
                if (breakshift)
                {
                    damage = (int)(damage * 1.5);
                    type = ModContent.ProjectileType<BigRazor>();
                }
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool AltFunctionUse(Player player)
        {
            if (!breakshift)
            {
                Item.useTime = 25;
                Item.useAnimation = 25;
                Item.damage = 64;
                Item.shootSpeed = 24;
                Item.channel = true;
                breakshift = true;
                Main.NewText("Switched to a melee Razor.");
            }
            else
            {
                Item.useTime = 10;
                Item.useAnimation = 10;
                Item.damage = 40;
                Item.shootSpeed = 16;
                Item.channel = false;
                breakshift = false;
                Main.NewText("Switched to long range Razor.");
            }
            return false;
        }
    }
}
