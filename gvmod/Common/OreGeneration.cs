using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;
using gvmod.Content.Tiles;
using Terraria.ID;

namespace gvmod.Common
{
    public class OreGeneration : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int TaskIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

            if (TaskIndex != -1)
            {
                tasks.Insert(TaskIndex + 1,new OreGenerationPass("A world of nightmares never seen before", 200f));
            }
        }
    }

    public class OreGenerationPass : GenPass
    {
        public OreGenerationPass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spiritualizing stones";
            for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 6E-03); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);

                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && tile.TileType != TileID.Sand && tile.TileType != TileID.Sandstone && 
                    tile.TileType != TileID.HardenedSand && tile.TileType != TileID.FossilOre)
                {
                    WorldGen.TileRunner(x, y, Main.rand.Next(1, 3), Main.rand.Next(2, 5), ModContent.TileType<SpiritualStone>());
                }
            }
        }
    }
}
