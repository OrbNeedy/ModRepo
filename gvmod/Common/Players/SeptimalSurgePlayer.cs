using gvmod.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    internal class SeptimalSurgePlayer : ModPlayer
    {
        public bool septimalSurge;

        public override void ResetEffects()
        {
            septimalSurge = false;
        }

        public override void PostUpdateBuffs()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (septimalSurge)
            {
                Player.GetDamage<SeptimaDamageClass>() *= 2;
                adept.PrimaryDamageEquipMult *= 2;
                adept.SpecialDamageEquipMult *= 2;
                adept.SecondaryDamageEquipMult *= 2;
            }
        }
    }
}
