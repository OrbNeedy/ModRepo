﻿using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using gvmod.Content.Items.Weapons;
using gvmod.Content.Items.Ammo;
using gvmod.Content.Projectiles;
using System.Collections.Generic;
using Terraria.ID;

namespace gvmod.Content.Items.Upgrades
{
    internal class TechnosBolt : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Technos Upgrade");
            // Tooltip.SetDefault("An upgrade for Dart Leader. Only upgrades the weapon to the uppermost left of \nthe inventory. \nUse again to remove the upgrade.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.MaxMana;
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
                foreach (Item item in player.inventory)
                {
                    if (item.ModItem is DartLeader gun)
                    {
                        if (gun.Upgrades[2])
                        {
                            gun.Upgrades[2] = false;
                            Main.NewText("Removed Technos from your gun.");
                            break;
                        }
                        else
                        {
                            gun.Upgrades[2] = true;
                            Main.NewText("Added Technos to your gun.");
                            break;
                        }
                    }
                }
            }
            return true;
        }
    }
}
