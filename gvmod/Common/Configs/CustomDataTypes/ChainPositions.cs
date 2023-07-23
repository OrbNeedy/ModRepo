using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace gvmod.Common.Configs.CustomDataTypes
{
    public class ChainPositions
    {
        private Player player;
        public Vector2 StartingPosition { get; set; }
        public Vector2 EndingPosition { get; set; }
        public List<(Vector2, Vector2)> OctisPositions = new List<(Vector2, Vector2)>();

        public ChainPositions(Player player)
        {
            this.player = player;
            RegularPositionsInitialize();
            OctisPositionInitialize();
        }

        public void RegularPositionsInitialize()
        {
            int vertical = 440;
            int horizontal = 950;
            if (Main.rand.NextBool())
            {
                if (Main.rand.NextBool())
                {
                    StartingPosition = player.Center + new Vector2(horizontal, Main.rand.Next(-vertical, vertical));
                    EndingPosition = player.Center + new Vector2(-horizontal, Main.rand.Next(-vertical, vertical));
                } else
                {
                    StartingPosition = player.Center + new Vector2(-horizontal, Main.rand.Next(-vertical, vertical));
                    EndingPosition = player.Center + new Vector2(horizontal, Main.rand.Next(-vertical, vertical));
                }
            } else
            {
                if (Main.rand.NextBool())
                {
                    StartingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), -vertical);
                    EndingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), vertical);
                } else
                {
                    StartingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), vertical);
                    EndingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), -vertical);
                }
            }
        }

        public void OctisPositionInitialize()
        {
            int vertical = 440;
            int horizontal = 950;
            List<(Vector2, Vector2)> octisPairs = new() { 
                // Chain 1
                (player.Center + new Vector2(horizontal*0.5f, -vertical), 
                    player.Center + new Vector2(horizontal*0.83f, vertical)), 
                // Chain 2
                (player.Center + new Vector2(-horizontal*0.83f, vertical), 
                    player.Center + new Vector2(-horizontal*0.5f, -vertical)),
                // Chain 3
                (player.Center + new Vector2(horizontal, vertical*0.62f),
                    player.Center + new Vector2(-horizontal*0.25f, vertical)),
                // Chain 4
                (player.Center + new Vector2(horizontal*0.25f, vertical),
                    player.Center + new Vector2(-horizontal, vertical*0.62f)),
                // Chain 5
                (player.Center + new Vector2(-horizontal, -vertical*0.1f),
                    player.Center + new Vector2(-horizontal*0.5f, -vertical)),
                // Chain 6
                (player.Center + new Vector2(horizontal*0.5f, -vertical),
                    player.Center + new Vector2(horizontal, -vertical*0.1f)),
                // Chain 7
                (player.Center + new Vector2(horizontal, -vertical*0.38f),
                    player.Center + new Vector2(horizontal*0.38f, vertical)),
                // Chain 8
                (player.Center + new Vector2(-horizontal*0.38f, vertical),
                    player.Center + new Vector2(-horizontal, -vertical*0.38f))
            };
            OctisPositions = octisPairs;
        }

        public Vector2 GetVelocity()
        {
            Vector2 distance = new Vector2((EndingPosition.X - StartingPosition.X), (EndingPosition.Y - StartingPosition.Y));
            return distance / 20f;
        }

        public Vector2 GetVelocity(Vector2 startingPosition, Vector2 endingPosition)
        {
            Vector2 distance = new Vector2((endingPosition.X - startingPosition.X), (endingPosition.Y - startingPosition.Y));
            return distance / 20f;
        }
    }
}
