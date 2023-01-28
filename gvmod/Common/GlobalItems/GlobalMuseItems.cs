using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using System.Net.Sockets;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalItems
{
    public class GlobalMuseItems : GlobalItem
    {
        public override void UpdateEquip(Item item, Player player)
        {
            AdeptMuse muse = player.GetModPlayer<AdeptMuse>();
            foreach (int id in muse.MuseItems)
            {
                if (item.type == id)
                {
                    muse.HasAMuseItem = true;
                    break;
                }
            }
        }
    }
}
