using gvmod.Common.Players;
using gvmod.Content.Items.Placeable;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Power
{
    [AutoloadEquip(EquipType.Body)]
    public class PowerBody : ModItem
    {
        private float increaseInDamage = 25;
        private float increaseInSPUse = 30;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInDamage, increaseInSPUse);

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
            float totalDamageIncrease = increaseInDamage / 100f;
            float totalSPUseIncrease = increaseInSPUse / 100f;
            adept.PrimaryDamageEquipMult += totalDamageIncrease;
            adept.SecondaryDamageEquipMult += totalDamageIncrease;
            adept.SpecialDamageEquipMult += totalDamageIncrease;
            player.GetDamage<SeptimaDamageClass>() += totalDamageIncrease;
            adept.SPUsageModifier += totalSPUseIncrease;
            adept.SPRegenModifier -= totalSPUseIncrease;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<PowerLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            player.setBonus = "Increases most stats while not in overheat, while in overheat, life \n" +
                "regen becomes harmful.";
            if (adept.IsOverheated)
            {
                player.GetModPlayer<ReversePowerPlayer>().PowerShortage = true; 
            } else
            {
                player.statDefense += 8;
                player.endurance += 0.1f;

                player.moveSpeed += 0.25f;
                player.maxRunSpeed += 4;
                
                adept.PrimaryDamageEquipMult += 0.2f;
                adept.SecondaryDamageEquipMult += 0.2f;
                adept.SpecialDamageEquipMult += 0.2f;
                player.GetDamage<SeptimaDamageClass>() += 0.3f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<SpiritualStone>(14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
