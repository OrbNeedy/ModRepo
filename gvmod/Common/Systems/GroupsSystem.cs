using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Common.Systems
{
    public class GroupsSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} " +
                $"{Lang.GetItemNameValue(ItemID.PlatinumBar)}", ItemID.PlatinumBar, ItemID.GoldBar);
            
            RecipeGroup.RegisterGroup("GoldBar", group);
        }
    }
}
