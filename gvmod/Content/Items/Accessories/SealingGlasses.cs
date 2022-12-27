using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class SealingGlasses : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunglasses of lightning");
			Tooltip.SetDefault("25% reduced septimal damage\n"
							 + "50% reduced SP usage\n"
							 + "90% chance not to consume AP when using a special ability\n"
							 + "\"Hasta la vista, GV\"");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 16;
			Item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			var adept = player.GetModPlayer<AdeptPlayer>();
			player.GetDamage(ModContent.GetInstance<SeptimaDamageClass>()) *= 0.75f;
			adept.PrimaryDamageEquipMult *= 0.75f;
			adept.SecondaryDamageEquipMult *= 0.75f;
			adept.SpecialDamageEquipMult *= 0.75f;
			// For developing purposes, it negates SP and AP usage, normally it just makes it so
			// the SP usage is halved and makes it so 90% of the time you don't use any AP
			adept.SPUsageModifier *= 0f;
			adept.APUsageModifier *= 0f;
        }
    }
}
