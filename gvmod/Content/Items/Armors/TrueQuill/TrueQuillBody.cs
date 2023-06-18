using gvmod.Common.Players;
using gvmod.Content.Buffs;
using gvmod.Content.Items.Armors.Quill;
using gvmod.Content.Items.Drops;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.TrueQuill
{
    [AutoloadEquip(EquipType.Body)]
    public class TrueQuillBody : ModItem
    {
        private float decreaseInSPUsage = 30;
        private float increaseInDamage = 12;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(decreaseInSPUsage, increaseInDamage);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 25;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalSPUseDecrease = decreaseInSPUsage / 100f;
            float totalDamageIncrease = increaseInDamage / 100f;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
            adept.SPRegenModifier += totalSPUseDecrease;
            adept.SPRegenOverheatModifier += totalSPUseDecrease;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<TrueQuillLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Increases resistance to chaff and overheat SP recovery.";

            adept.SPRegenOverheatModifier += 0.6f;
            player.buffImmune[ModContent.BuffType<Chaff>()] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<LunaticCloth>(4)
                .AddIngredient(ItemID.ChlorophyteOre, 8)
                .AddIngredient<QuillBody>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
