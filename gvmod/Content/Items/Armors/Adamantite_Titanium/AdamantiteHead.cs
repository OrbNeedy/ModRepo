using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Adamantite_Titanium
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Adamantite bow");
            /* Tooltip.SetDefault("Even boys look good with bows, manly even.\n" +
                "Increases SP by 80 and all SP recovery by 35%."); */
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            adept.SPRegenModifier *= 1.35f;
            adept.SPRegenOverheatModifier *= 1.35f;
            adept.MaxSeptimalPower2 += 80;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.AdamantiteLeggings && body.type == ItemID.AdamantiteBreastplate;
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "25% chance not to consume AP.";
            if (Main.rand.NextBool(1, 4)) adept.APUsageModifier *= 0f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
