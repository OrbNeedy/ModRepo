using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Placeable
{
    public class SpiritualStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SpiritualStone>());
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }
    }
}
