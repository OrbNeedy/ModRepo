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
            if (Main.rand.NextBool())
            {
                if (Main.rand.NextBool())
                {
                    startingPosition = player.Center + new Vector2(1050, Main.rand.Next(-595, 595));
                    endingPosition = player.Center + new Vector2(-1050, Main.rand.Next(-595, 595));
                } else
                {
                    startingPosition = player.Center + new Vector2(-1050, Main.rand.Next(-595, 595));
                    endingPosition = player.Center + new Vector2(1050, Main.rand.Next(-595, 595));
                }
            } else
            {
                if (Main.rand.NextBool())
                {
                    startingPosition = player.Center + new Vector2(Main.rand.Next(-1050, 1050), -595);
                    endingPosition = player.Center + new Vector2(Main.rand.Next(-1050, 1050), 595);
                } else
                {
                    startingPosition = player.Center + new Vector2(Main.rand.Next(-1050, 1050), 595);
                    endingPosition = player.Center + new Vector2(Main.rand.Next(-1050, 1050), -595);
                }
            }
        }

        public Vector2 GetVelocity()
        {
            Vector2 velocity = (endingPosition - startingPosition)/16;
            return velocity;
        }
    }
}
