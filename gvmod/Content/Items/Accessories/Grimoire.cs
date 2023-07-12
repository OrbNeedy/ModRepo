using gvmod.Common.Players;
using gvmod.Content;
using gvmod.Content.Buffs;
using gvmod.Content.Items.Drops;
using gvmod.Content.Items.Placeable;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class Grimoire : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 40;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.HasBuff<ArmedPhenomenon3>() && !player.HasBuff<ArmedPhenomenon2B>())
            {
                player.AddBuff(ModContent.BuffType<ArmedPhenomenon2A>(), 2, true);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient<SpiritualStone>(15)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
