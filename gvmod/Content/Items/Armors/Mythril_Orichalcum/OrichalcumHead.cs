using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Armors.Mythril_Orichalcum
{
    [AutoloadEquip(EquipType.Head)]
    public class OrichalcumHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum fedora");
            Tooltip.SetDefault("A fedora made of orichalcum and silk.\n" +
                "Increases all septima damage by 15%\nIncreases movement speed when overheated.");
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
            player.GetDamage<SeptimaDamageClass>() += 0.15f;
            if (adept.IsOverheated)
            {
                player.moveSpeed += 0.25f;
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ItemID.OrichalcumLeggings && body.type == ItemID.OrichalcumBreastplate;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Releases a septima energy bolt that homes in on the nearest enemy.";
            player.GetModPlayer<SeptimaShootingPlayer>().ShootsBolts = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).Register();
        }
    }
}
