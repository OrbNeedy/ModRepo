using gvmod.Common.Players;
using gvmod.Content.Items.Armors.Protective;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Improved
{
    [AutoloadEquip(EquipType.Body)]
    public class ImprovedBody : ModItem
    {
        private float increaseInDamage = 12;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Improved Breastplate");
            /* Tooltip.SetDefault("Improved with special minerals.\n" +
                "Increases all septima damage by 12%."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInDamage);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalDamageIncrease = increaseInDamage / 100f;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<ImprovedLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            APSPPlayer apsp = player.GetModPlayer<APSPPlayer>();
            player.setBonus = "Dealing damage increases current AP and SP.";
            apsp.ApAdded += 1;
            apsp.SpAdded += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
