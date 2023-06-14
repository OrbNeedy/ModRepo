using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Hallowed
{
    [AutoloadEquip(EquipType.Head)]
    public class HallowedHead : ModItem
    {
        private float increaseInDamage = 50;
        private float increaseInSPUse = 35;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hallowed hardhat");
            /* Tooltip.SetDefault("If you find something better that could be put in this slot, please tell me.\n" +
                "Increases SP usage and greatly increases all Septima attack."); */
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSPUse, increaseInDamage);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalSPUseIncrease = increaseInSPUse / 100f;
            float totalDamageIncrease = increaseInDamage / 100f;
            adept.SPUsageModifier += totalSPUseIncrease;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (legs.type == ItemID.HallowedGreaves && body.type == ItemID.HallowedPlateMail) ||
                (legs.type == ItemID.AncientHallowedGreaves && body.type == ItemID.AncientHallowedPlateMail);
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Shadow dodge buff";
            player.shadowDodge = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
