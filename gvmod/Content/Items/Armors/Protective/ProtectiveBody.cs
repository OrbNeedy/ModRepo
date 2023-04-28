using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Protective
{
    [AutoloadEquip(EquipType.Body)]
    public class ProtectiveBody : ModItem
    {
        private float increaseInSeptimaDamage = 5;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Protective Breastplate");
            /* Tooltip.SetDefault("It feels surprisingly comfy.\n" +
                "Increases all septima damge by 5%."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSeptimaDamage);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float increase = (increaseInSeptimaDamage / 100f);
            adept.PrimaryDamageEquipMult += increase;
            adept.SecondaryDamageEquipMult += increase;
            adept.SpecialDamageEquipMult += increase;
            player.GetDamage<SeptimaDamageClass>() += increase;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<ProtectiveLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Decreases SP usage by 25%.";
            adept.SPUsageModifier -= 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
