using gvmod.Common.Players;
using gvmod.Content.Items.Armors.Power;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.TrueQuill
{
    [AutoloadEquip(EquipType.Body)]
    public class TrueQuillBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Quill Breastplate");
            Tooltip.SetDefault("It's been upgraded with the best cloth stolen from the cultist.\n" +
                "Increases all septima damage and reduces SP usage.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 25;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            adept.PrimaryDamageEquipMult += 0.1f;
            adept.SecondaryDamageEquipMult += 0.1f;
            adept.SpecialDamageEquipMult += 0.1f;
            player.GetDamage<SeptimaDamageClass>() += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<TrueQuillLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Increases resistance to chaff and overheat SP recovery.";

            adept.SPRegenOverheatModifier *= 1.8f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
