using gvmod.Common.Players;
using gvmod.Content.Items.Armors.Power;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.TrueQuill
{
    [AutoloadEquip(EquipType.Legs)]
    public class TrueQuillLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Quill legs");
            Tooltip.SetDefault("Improved with the finest cloth, stolen from a madman.\n" +
                "Increases all septima damage, movement speed, and decreases SP usage.");

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
