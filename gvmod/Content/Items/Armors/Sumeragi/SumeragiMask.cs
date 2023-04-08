using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Sumeragi
{
    [AutoloadEquip(EquipType.Head)]
    public class SumeragiMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sumeragi mask");
            /* Tooltip.SetDefault("A mask to conceal Sumeragi agents' identity when dealing with secet\n" +
                "missions.\n" +
                "Increases SP recover and septima damage."); */
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            adept.SPRegenModifier += 0.6f;
            adept.SPRegenOverheatModifier += 0.6f;

            adept.PrimaryDamageEquipMult += 0.16f;
            adept.SecondaryDamageEquipMult += 0.16f;
            adept.SpecialDamageEquipMult += 0.16f;
            player.GetDamage<SeptimaDamageClass>() += 0.16f;
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
