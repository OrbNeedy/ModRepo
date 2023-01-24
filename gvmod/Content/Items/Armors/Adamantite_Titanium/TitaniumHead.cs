using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Adamantite_Titanium
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium hairclip");
            Tooltip.SetDefault("Might not be helpful if your hair isn't long enough.\n" +
                "25% increased main attack damage and septimal damage.");
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            adept.PrimaryDamageEquipMult += 0.25f;
            player.GetDamage<SeptimaDamageClass>() += 0.25f;
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
