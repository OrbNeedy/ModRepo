using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Chlorophyte
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorophyteHead : ModItem
    {
        private float decreaseInSPUse = 60;
        private float increaseInDamage = 10;
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(decreaseInSPUse, increaseInDamage);

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
            float totalDecrease = decreaseInSPUse / 100f;
            float totalIncrease = increaseInDamage / 100f;
            adept.SPUsageModifier -= totalDecrease;
            adept.PrimaryDamageEquipMult += totalIncrease;
            adept.SecondaryDamageEquipMult += totalIncrease;
            adept.SpecialDamageEquipMult += totalIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalIncrease;
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
