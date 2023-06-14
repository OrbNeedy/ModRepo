using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Power
{
    [AutoloadEquip(EquipType.Legs)]
    public class PowerLegs : ModItem
    {
        private float increaseInDamage = 12;
        private float increaseInSpeed = 20;
        private float increaseInSPUse = 20;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInDamage, increaseInSpeed, increaseInSPUse);

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
            float totalDamageIncrease = increaseInDamage / 100f;
            float totalSpeedIncrease = increaseInSpeed / 100f;
            float totalSPUseIncrease = increaseInSPUse / 100f;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
            player.moveSpeed += totalSpeedIncrease;
            adept.SPUsageModifier += totalSPUseIncrease;
            adept.SPRegenModifier -= totalSPUseIncrease;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
