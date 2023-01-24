using gvmod.Common.Players;
using gvmod.Content.Items.Armors.Power;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Sumeragi
{
    [AutoloadEquip(EquipType.Legs)]
    public class SumeragiLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sumeragi uniform leggins");
            Tooltip.SetDefault("Made to be used in the battlefield, still feel absurdly comfortable.\n" +
                "Increases septima damage and movement speed.");

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
            adept.PrimaryDamageEquipMult += 0.08f;
            adept.SecondaryDamageEquipMult += 0.08f;
            adept.SpecialDamageEquipMult += 0.08f;
            player.GetDamage<SeptimaDamageClass>() += 0.08f;

            player.moveSpeed += 0.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
