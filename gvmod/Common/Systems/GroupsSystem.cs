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
                $"{Lang.GetItemNameValue(ItemID.IronBar)}", ItemID.IronBar, ItemID.LeadBar);

            RecipeGroup.RegisterGroup("IronBar", group);

            RecipeGroup group2 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} " +
                $"{Lang.GetItemNameValue(ItemID.PlatinumBar)}", ItemID.PlatinumBar, ItemID.GoldBar);
            
            RecipeGroup.RegisterGroup("GoldBar", group2);

            RecipeGroup group3 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} " +
                $"{Lang.GetItemNameValue(ItemID.CrimtaneBar)}", ItemID.CrimtaneBar, ItemID.DemoniteBar);

            RecipeGroup.RegisterGroup("CrimtaneBar", group3);

            RecipeGroup group4 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} " +
                $"{Lang.GetItemNameValue(ItemID.BloodButcherer)}", ItemID.BloodButcherer, ItemID.LightsBane);

            RecipeGroup.RegisterGroup("BloodButcherer", group4);
        }
    }
}
