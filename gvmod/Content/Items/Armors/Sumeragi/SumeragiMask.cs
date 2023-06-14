using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Sumeragi
{
    [AutoloadEquip(EquipType.Head)]
    public class SumeragiMask : ModItem
    {
        private float increaseInSPRecovery = 60;
        private float increaseInDamage = 16;
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInSPRecovery, increaseInDamage);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalSPRecoveryIncrease = increaseInSPRecovery / 100f;
            float totalDamageIncrease = increaseInDamage / 100f;
            adept.SPRegenModifier += totalSPRecoveryIncrease;
            adept.SPRegenOverheatModifier += totalSPRecoveryIncrease;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<SumeragiLegs>() && body.type == ModContent.ItemType<SumeragiBody>();
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Drastically increased AP regeneration while having max SP.";
            if (adept.SeptimalPower >= adept.MaxSeptimalPower2 && !adept.IsOverheated)
            {
                adept.APRegenModifier *= 2.234f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
