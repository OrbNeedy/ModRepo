using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Adamantite_Titanium
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumHead : ModItem
    {
        private float increaseInDamage = 20;
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(increaseInDamage);

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
                float totalIncrease = increaseInDamage / 100f;
                adept.PrimaryDamageEquipMult += totalIncrease;
                adept.SecondaryDamageEquipMult += totalIncrease;
                adept.SpecialDamageEquipMult += totalIncrease;
                player.GetDamage<SeptimaDamageClass>() += totalIncrease;
            }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.TitaniumLeggings && body.type == ItemID.TitaniumBreastplate;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Attacking generates a defensive barrier of titanium shards.";
            player.hasTitaniumStormBuff = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
