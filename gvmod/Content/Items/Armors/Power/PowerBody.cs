using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Power
{
    [AutoloadEquip(EquipType.Body)]
    public class PowerBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power Breastplate");
            Tooltip.SetDefault("Greatly amplifies your septima, but doesn't feel as comfy.\n" +
                "Increases all septima damage and SP usage.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            adept.SPUsageModifier += 0.3f;
            adept.SPRegenModifier -= 0.03f;
            adept.PrimaryDamageEquipMult += 0.15f;
            adept.SecondaryDamageEquipMult += 0.15f;
            adept.SpecialDamageEquipMult += 0.15f;
            player.GetDamage<SeptimaDamageClass>() += 0.15f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<PowerLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Increases most stats while not in overheat, while in overheat, life \n" +
                "regen becomes harmful.";
            if (adept.IsOverheated)
            {
                player.GetModPlayer<ReversePowerPlayer>().PowerShortage = true; 
            } else
            {
                player.statDefense += 8;
                player.endurance += 0.05f;

                player.moveSpeed += 0.2f;
                player.maxRunSpeed += 2;
                
                adept.PrimaryDamageEquipMult += 0.2f;
                adept.SecondaryDamageEquipMult += 0.2f;
                adept.SpecialDamageEquipMult += 0.2f;
                player.GetDamage<SeptimaDamageClass>() += 0.2f;
            }
            adept.SPUsageModifier += 0.5f;
            adept.SPRegenModifier -= 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
