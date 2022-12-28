using gvmod.Common.Players;
using gvmod.Content;
using gvmod.Content.Buffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class Glaive : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Transforms the player into an armed phenomenon, incresing their \n"
                             + "capabilities.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 56;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.color = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>().Septima.MainColor;
            player.AddBuff(ModContent.BuffType<ArmedPhenomenon>(), 2, true);
        }
    }
}
