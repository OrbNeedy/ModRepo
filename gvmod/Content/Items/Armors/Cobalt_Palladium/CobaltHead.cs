using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Cobalt_Palladium
{
    [AutoloadEquip(EquipType.Head)]
    public class CobaltHead : ModItem
    {
        private float increaseInDamage = 10;
        private float decreaseInSPUse = 15;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cobalt headband");
            /* Tooltip.SetDefault("Cloth with some cobalt sprinkled in it.\n" +
                "Increases all septima damage by 10% and reduces SP usage."); */
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInDamage, decreaseInSPUse);

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 10;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalIncrease = increaseInDamage / 100f;
            float totalDecrease = decreaseInSPUse / 100f;
            adept.PrimaryDamageEquipMult += totalIncrease;
            adept.SecondaryDamageEquipMult += totalIncrease;
            adept.SpecialDamageEquipMult += totalIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalIncrease;
            adept.SPUsageModifier -= totalDecrease;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.CobaltLeggings && body.type == ItemID.CobaltBreastplate;
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Decreases SP usage by 25%.";
            adept.SPUsageModifier -= 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
