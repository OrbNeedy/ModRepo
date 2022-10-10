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
			adept.primaryDamageEquipMult *= 0.75f;
			adept.secondaryDamageEquipMult *= 0.75f;
			adept.specialDamageEquipMult *= 0.75f;
			adept.SPUsageModifier *= 0.5f;
			if (!Main.rand.NextBool(10))
            {
				adept.APUsageModifier = 0;
			}
        }
    }
}
