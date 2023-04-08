using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Power
{
    [AutoloadEquip(EquipType.Legs)]
    public class PowerLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Power armored pants");
            /* Tooltip.SetDefault("Made to increase your physical abilities, with no regard for your comfort.\n" +
                "Increases all septima damage, movement speed, and all SP usage."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            adept.SPUsageModifier += 0.2f;
            adept.SPRegenModifier -= 0.02f;
            adept.PrimaryDamageEquipMult += 0.15f;
            adept.SecondaryDamageEquipMult += 0.15f;
            adept.SpecialDamageEquipMult += 0.15f;
            player.GetDamage<SeptimaDamageClass>() += 0.15f;
            player.moveSpeed += 0.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
