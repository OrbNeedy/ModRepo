using gvmod.Common.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content
{
    public class SeptimaDamageClass : DamageClass
    {
        public override void SetDefaultStats(Player player)
        {
            ClassName.SetDefault("Septima damage");
            player.GetCritChance<SeptimaDamageClass>() += 0.05f;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Generic)
                return StatInheritanceData.Full;
            return StatInheritanceData.None;
        }
    }
}
