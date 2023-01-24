using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Chlorophyte
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorophyteHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte tophat");
            Tooltip.SetDefault("Makes you more calmed.\n" +
                "Decreases SP usage and increases Septima damage.");
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            adept.SPUsageModifier -= 0.6f;
            adept.PrimaryDamageEquipMult += 0.1f;
            adept.SecondaryDamageEquipMult += 0.1f;
            adept.SpecialDamageEquipMult += 0.1f;
            player.GetDamage<SeptimaDamageClass>() += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.ChlorophyteGreaves && body.type == ItemID.ChlorophytePlateMail;
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Chlorophyte crystal buff";
            player.crystalLeaf = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
