using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Cobalt_Palladium
{
    [AutoloadEquip(EquipType.Head)]
    public class CobaltHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cobalt headband");
            /* Tooltip.SetDefault("Cloth with some cobalt sprinkled in it.\n" +
                "Increases all septima damage by 10% and reduces SP usage."); */
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            adept.PrimaryDamageEquipMult += 0.1f;
            adept.SecondaryDamageEquipMult += 0.1f;
            adept.SpecialDamageEquipMult += 0.1f;
            adept.SPUsageModifier -= 0.15f;
            player.GetDamage<SeptimaDamageClass>() += 0.1f;
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
