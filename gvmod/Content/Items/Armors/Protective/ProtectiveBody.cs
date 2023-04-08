using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Protective
{
    [AutoloadEquip(EquipType.Body)]
    public class ProtectiveBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Protective Breastplate");
            /* Tooltip.SetDefault("It feels surprisingly comfy.\n" +
                "Increases all septima damge by 5%."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            adept.PrimaryDamageEquipMult += 0.05f;
            adept.SecondaryDamageEquipMult += 0.05f;
            adept.SpecialDamageEquipMult += 0.05f;
            player.GetDamage<SeptimaDamageClass>() += 0.05f;
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
