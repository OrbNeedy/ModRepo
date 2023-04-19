using gvmod.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class ChaffedPlayer : ModPlayer
    {
        public bool Chaffed { get; set; }

        public override void Initialize()
        {
            Chaffed = false;
        }

        public override void ResetEffects()
        {
            Chaffed = false;
        }

        public override void PreUpdate()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (Chaffed)
            {
                adept.SeptimalPower = 0;
                adept.IsOverheated = true;
            }
        }
    }
}
