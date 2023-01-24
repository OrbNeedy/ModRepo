using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Mythril_Orichalcum
{
    [AutoloadEquip(EquipType.Head)]
    public class MythrilHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril crown");
            Tooltip.SetDefault("A crown made of mythril. It makes you want to suffer.\n" +
                "Increases all septima damage by 15%. Increases overheat SP recovery.");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            adept.PrimaryDamageEquipMult += 0.15f;
            adept.SecondaryDamageEquipMult += 0.15f;
            adept.SpecialDamageEquipMult += 0.15f;
            adept.SPRegenOverheatModifier += 0.25f;
            player.GetDamage<SeptimaDamageClass>() += 0.15f;
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
            CreateRecipe(1).Register();
        }
    }
}
