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
    public class TrueGlaive : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 78;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<ArmedPhenomenon3>(), 2, true);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Glaive>()
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddIngredient(ItemID.Ectoplasm, 12)
                .AddIngredient<SpiritualStone>(27)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
