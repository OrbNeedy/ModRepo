using gvmod.Common.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class FateFetus : ModItem
    {
        public static readonly int apConsumeChance = 50;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fate in a pod");
            /* Tooltip.SetDefault(); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(apConsumeChance);

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 44;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MorbPlayer>().FateFetus = true;
        }
    }
}
