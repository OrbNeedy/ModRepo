using gvmod.Content.Items.Accessories;
using gvmod.Content.Items.Placeable;
using gvmod.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace gvmod.Common
{
    public class TheWorld : ModSystem
    {
        public override void PostWorldGen()
        {
            int[] goldChestItems = { ModContent.ItemType<SpiritualStone>(), ModContent.ItemType<ElectricWhip>() };
            int[] lockedGoldChestItems = { ModContent.ItemType<ForbiddenConverter>() };
            int[] shadowChestItems = { ModContent.ItemType<SecretConverter>() };
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                int chestItemsChoice = 0;
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers)
                {
                    switch (Main.tile[chest.x, chest.y].TileFrameX)
                    {
                        case 1 * 36:
                            PutInChest(chest, ref chestItemsChoice, goldChestItems, !Main.rand.NextBool(3));
                            break;
                        case 2 * 36:
                            PutInChest(chest, ref chestItemsChoice, lockedGoldChestItems, !Main.rand.NextBool(4));
                            break;
                        case 4 * 36:
                            PutInChest(chest, ref chestItemsChoice, shadowChestItems, !Main.rand.NextBool(7));
                            break;
                    }
                }
            }
        }

        private void PutInChest(Chest chest, ref int chestItemsChoice, int[] itemPool, bool skip)
        {
            if (skip) return;
            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
            {
                if (chest.item[inventoryIndex].type == ItemID.None)
                {
                    chest.item[inventoryIndex].SetDefaults(itemPool[chestItemsChoice]);
                    chestItemsChoice = (chestItemsChoice + 1) % itemPool.Length;
                    break;
                }
            }
        }
    }
}
