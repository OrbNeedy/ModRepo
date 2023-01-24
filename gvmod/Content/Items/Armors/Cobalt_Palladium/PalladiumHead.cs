using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Cobalt_Palladium
{
    [AutoloadEquip(EquipType.Head)]
    public class PalladiumHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Paladium glasses");
            Tooltip.SetDefault("Glasses with a paladium frame, the lenses are purely aesthetic.\n" +
                "Increases all septima damage by 10% and increases SP regen.");
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            adept.PrimaryDamageEquipMult += 0.1f;
            adept.SecondaryDamageEquipMult += 0.1f;
            adept.SpecialDamageEquipMult += 0.1f;
            adept.SPRegenModifier += 0.15f;
            player.GetDamage<SeptimaDamageClass>() += 0.1f;
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
