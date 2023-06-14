using gvmod.Common.Players;
using gvmod.Content;
using gvmod.Content.Buffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class FalconQuill : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 52;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.HasBuff<ArmedPhenomenon3>())
            {
                player.AddBuff(ModContent.BuffType<ArmedPhenomenon2B>(), 2, true);
            }
        }
    }
}
