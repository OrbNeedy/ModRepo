using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using gvmod.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Upgrades
{
    public class DragonRadiation : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item4;
            Item.noMelee = true;
            Item.autoReuse = false;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
                if (!adept.UnlockedPotential)
                {
                    adept.UnlockedPotential = true;
                    Main.NewText("Your body reacts to the radiation.", adept.Septima.MainColor);
                } else
                {
                    Main.NewText("The radiation ignores your body.");
                }
            }
            return true;
        }
    }
}
