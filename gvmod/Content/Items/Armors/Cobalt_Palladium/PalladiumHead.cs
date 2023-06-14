using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Cobalt_Palladium
{
    [AutoloadEquip(EquipType.Head)]
    public class PalladiumHead : ModItem
    {
        private float increaseInDamage = 10;
        private float increaseInSPRegen = 15;
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInDamage, increaseInSPRegen);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 6;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float totalDamageIncrease = increaseInDamage / 100f;
            float totalRegenIncrease = increaseInSPRegen / 100f;
            adept.PrimaryDamageEquipMult += increaseInDamage;
            adept.SecondaryDamageEquipMult += increaseInDamage;
            adept.SpecialDamageEquipMult += increaseInDamage;
            player.GetDamage<SeptimaDamageClass>() += increaseInDamage;
            adept.SPRegenModifier += increaseInSPRegen;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.PalladiumLeggings && body.type == ItemID.PalladiumBreastplate;
        }

        public override void UpdateArmorSet(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>(); 
            player.setBonus = "Increases health regen while overheated.";
            if (adept.IsOverheated)
            {
                player.AddBuff(BuffID.RapidHealing, 2);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
