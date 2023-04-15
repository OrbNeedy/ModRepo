using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class ChainedPlayer : ModPlayer
    {
        public bool Chained { get; set; }

        public override void Initialize()
        {
            Chained = false;
        }

        public override void ResetEffects()
        {
            Chained = false;
        }

        public override void UpdateLifeRegen()
        {
            if (Chained)
            {
                Player.lifeRegen *= 0;
                Player.lifeRegenTime *= 0;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Chained)
            {
                triggersSet.QuickHeal = false;
                triggersSet.QuickMana = false;
                triggersSet.QuickMount = false;
            }
        }
    }
}
