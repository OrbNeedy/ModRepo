using gvmod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class SealingGlasses : ModItem
    {
		int noConsumeChance = 50;
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 16;
			Item.accessory = true;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(noConsumeChance);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			var adept = player.GetModPlayer<AdeptPlayer>();
            player.GetDamage(ModContent.GetInstance<SeptimaDamageClass>()) -= 0.1f;
			adept.PrimaryDamageEquipMult -= 0.1f;
			adept.SecondaryDamageEquipMult -= 0.1f;
			adept.SpecialDamageEquipMult -= 0.1f;

            player.GetDamage(ModContent.GetInstance<SeptimaDamageClass>()) *= 0.8f;
            adept.PrimaryDamageEquipMult *= 0.8f;
			adept.SecondaryDamageEquipMult *= 0.8f;
			adept.SpecialDamageEquipMult *= 0.8f;

			adept.SPUsageModifier *= 0.5f;
			adept.APConsumeChance -= (noConsumeChance/100);
        }
    }
}
