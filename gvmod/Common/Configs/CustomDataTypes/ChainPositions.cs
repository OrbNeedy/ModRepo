using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Configs.CustomDataTypes
{
    public class ChainPositions
    {
        public Player player;
        public Vector2 startingPosition;
        public Vector2 endingPosition;

        public ChainPositions(Player player)
        {
            this.player = player;
            InitializePositions();
        }

        public void InitializePositions()
        {
            int vertical = 440;
            int horizontal = 950;
            if (Main.rand.NextBool())
            {
                if (Main.rand.NextBool())
                {
                    startingPosition = player.Center + new Vector2(horizontal, Main.rand.Next(-vertical, vertical));
                    endingPosition = player.Center + new Vector2(-horizontal, Main.rand.Next(-vertical, vertical));
                } else
                {
                    startingPosition = player.Center + new Vector2(-horizontal, Main.rand.Next(-vertical, vertical));
                    endingPosition = player.Center + new Vector2(horizontal, Main.rand.Next(-vertical, vertical));
                }
            } else
            {
                if (Main.rand.NextBool())
                {
                    startingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), -vertical);
                    endingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), vertical);
                } else
                {
                    startingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), vertical);
                    endingPosition = player.Center + new Vector2(Main.rand.Next(-horizontal, horizontal), -vertical);
                }
            }
        }

        public Vector2 GetVelocity()
        {
            Vector2 distance = new Vector2((endingPosition.X - startingPosition.X), (endingPosition.Y - startingPosition.Y));
            return distance/20f;
        }
    }
}
