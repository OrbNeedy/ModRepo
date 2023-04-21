using gvmod.Common.Players;
using gvmod.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    internal class MirrorShard : ModItem
    {
		public override void SetStaticDefaults()
		{
			/* Tooltip.SetDefault("You will be resurrected upon death with an Anthem.\n"
							 + "Part of a bigger picture."); */

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 12;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			var muse = player.GetModPlayer<AdeptMuse>();
			muse.HasMirrorShard = true;
			muse.HasAMuseItem = true;
		}
	}
}
