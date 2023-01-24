using gvmod.Common.Players;
using gvmod.Content.Items.Armors.TrueQuill;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Sumeragi
{
    [AutoloadEquip(EquipType.Body)]
    public class SumeragiBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sumeragi uniform jacket");
            Tooltip.SetDefault("Made to resist the harshest environment while allowing great movement.\n" +
                "Increases septima damage and reduces SP usage.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 24;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            adept.APUsageModifier *= 0.6f;

            adept.PrimaryDamageEquipMult += 0.12f;
            adept.SecondaryDamageEquipMult += 0.12f;
            adept.SpecialDamageEquipMult += 0.12f;
            player.GetDamage<SeptimaDamageClass>() += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
