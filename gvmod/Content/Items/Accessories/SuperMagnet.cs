using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ModLoader;
using gvmod.Content.Buffs;

namespace gvmod.Content.Items.Accessories
{
    public class SuperMagnet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 26;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[ModContent.BuffType<Chaff>()] = true;
        }
    }
}
