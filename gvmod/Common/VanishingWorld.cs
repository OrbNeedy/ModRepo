using gvmod.Content.Items.Placeable;
using gvmod.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common
{
    public class VanishingWorld : ModSystem
    {
        public override void PostWorldGen()
        {
            int[] chestItems= { ModContent.ItemType<SpiritualStone>(), ModContent.ItemType<ElectricWhip>() };
            int chestItemsChoice = 0;
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36)
                {
                    if (Main.rand.NextBool()) continue;
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(chestItems[chestItemsChoice]);
                            chestItemsChoice = (chestItemsChoice + 1) % chestItems.Length;
                            break;
                        }
                    }
                }
            }
        }
    }
}
