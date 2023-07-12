using gvmod.Common.Players;
using gvmod.Content.Items.Drops;
using gvmod.Content.Items.Placeable;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class FateFetus : ModItem
    {
        private float apConsumeChance = 50;
        private float damageIncrease = 20;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(apConsumeChance, damageIncrease);

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 44;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalChance = apConsumeChance / 100;
            float totalDamageIncrease = damageIncrease / 100;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
            player.GetModPlayer<MorbPlayer>().FateFetus = true;
            adept.SPRegenOverheatModifier *= 0.8f;

            if (Main.rand.NextFloat() < totalChance)
            {
                adept.APConsumeChance -= totalChance;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Glass, 10)
                .AddIngredient<Fetus>()
                .AddIngredient<SpiritualStone>(40)
                .AddIngredient(ItemID.LunarBar, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}