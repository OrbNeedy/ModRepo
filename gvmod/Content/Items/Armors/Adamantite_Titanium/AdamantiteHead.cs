using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Adamantite_Titanium
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteHead : ModItem
    {
        private int increaseInSP = 80;
        private float increaseInSPRecovery = 35;
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSP, increaseInSPRecovery);

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
            float totalIncrease = 1 + (increaseInSPRecovery / 100f);
            adept.SPRegenModifier *= totalIncrease;
            adept.SPRegenOverheatModifier *= totalIncrease;
            adept.MaxSeptimalPower2 += increaseInSP;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.AdamantiteLeggings && body.type == ItemID.AdamantiteBreastplate;
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "20% chance not to consume AP.";
            adept.APConsumeChance -= 0.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.AdamantiteBar, 12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
