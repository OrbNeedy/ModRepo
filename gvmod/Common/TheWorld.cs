using gvmod.Content.Items.Accessories;
using gvmod.Content.Items.Placeable;
using gvmod.Content.Items.Weapons;
using gvmod.Content.Pets;
using System.Collections.Generic;
using System.Linq;
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
            int[] specialLolaLoot = { ModContent.ItemType<LolaPetItem>() };
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

            for (int i = 0; i < 6; i++)
            {
                Chest chest;
                do
                {
                    chest = Main.chest[Main.rand.Next(0, Main.maxChests)];
                } while (chest == null || Main.tile[chest.x, chest.y].TileType != TileID.Containers);
                int chestItemsChoice = 0;
                PutInChest(chest, ref chestItemsChoice, specialLolaLoot, false);
            }
        }

        private void PutInChest(Chest chest, ref int chestItemsChoice, int[] itemPool, bool skip)
        {
            if (skip) return;
            for (int inventoryIndex = 0; inventoryIndex < chest.item.Length; inventoryIndex++)
            {
                if (chest.item[inventoryIndex].type == ItemID.None)
                {
                    chest.item[inventoryIndex].SetDefaults(itemPool[chestItemsChoice]);
                    chestItemsChoice = (chestItemsChoice + 1) % itemPool.Length;
                    break;
                }
            }
        }

        private bool LolaCheck(Chest chest)
        {
            for (int inventoryIndex = 0; inventoryIndex < chest.item.Length; inventoryIndex++)
            {
                if (chest.item[inventoryIndex].type == ModContent.ItemType<LolaPetItem>())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
