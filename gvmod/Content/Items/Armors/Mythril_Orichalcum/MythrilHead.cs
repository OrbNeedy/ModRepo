using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Mythril_Orichalcum
{
    [AutoloadEquip(EquipType.Head)]
    public class MythrilHead : ModItem
    {
        private float increaseInDamage = 15;
        private float increaseInSPRecovery = 50;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mythril crown");
            /* Tooltip.SetDefault("A crown made of mythril. It makes you want to suffer.\n" +
                "Increases all septima damage by 15%. Increases overheat SP recovery."); */
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInDamage, increaseInSPRecovery);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalDamageIncrease = increaseInDamage / 100f;
            float totalSPRecoveryIncrease = increaseInSPRecovery / 100f;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
            adept.SPRegenOverheatModifier += totalSPRecoveryIncrease;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.CobaltLeggings && body.type == ItemID.CobaltBreastplate;
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Decreases SP usage by 20%.";
            adept.SPUsageModifier -= 0.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.MythrilBar, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
