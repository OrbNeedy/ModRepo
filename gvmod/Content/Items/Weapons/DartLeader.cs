using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Projectiles;
using gvmod.Content.Items.Ammo;
using Terraria.GameContent.Creative;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using gvmod.Common.Players.Septimas.Abilities;

namespace gvmod.Content.Items.Weapons
{
	public class DartLeader : ModItem
    {
        //Naga, Mizuchi, Technos, Orochi, Vasuki, and Dullahan respectively
        public bool[] Upgrades { get; set; } = new bool[6] { false, false, false, false, false, false };
        private int ai0;
        private int ai1;
        private List<int> orochiDroneIndex;

        private static string[] upgradeDescriptions = new string[6] { "\nNaga: Adds a piercing effect and increases speed.", 
			"\nMizuchi: Adds a slight homing effect to the darts.",  
			"\nTechnos: Adds two extra darts when shooting.",
			"\nOrochi: Deploys a drone when shooting that shoots extra darts, these darts \ndon't inherit other upgrades.", 
			"\nVasuki: After hitting an enemy, the enemy shoots an extra dart to the next \nnearest enemy, improves homing if paired with Mizuchi.",
			"\nDullahan: Increases damage and rate of fire."};
		private string tooltip;
		public override void SetStaticDefaults()		
		{
			DisplayName.SetDefault("Dart Leader");
			Tooltip.SetDefault("");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
			Item.damage = 2;
			Item.knockBack = 0;
			Item.rare = ItemRarityID.Green;

			Item.shoot = ModContent.ProjectileType<HairDartProjectile>();
			Item.useAmmo = ModContent.ItemType<HairDart>();
			Item.shootSpeed = 10;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.UseSound = SoundID.Camera; 
			Item.noMelee = true;
			Item.autoReuse = true;

			tooltip = "";
            ai0 = 0;
            ai1 = 0;
            orochiDroneIndex = new List<int>();        
        }

        public override ModItem Clone(Item item)
        {
            DartLeader clone = (DartLeader)base.Clone(item);
            clone.Upgrades = (bool[])Upgrades?.Clone();
            return clone;
        }

        public override void OnCreate(ItemCreationContext context)
        {
            for (int i = 0; i < Upgrades.Length; i++)
            {
                Upgrades[i] = false;
            }
        }

        public override void LoadData(TagCompound tag)
		{
			if (tag.ContainsKey("Naga"))
			{
				Upgrades[0] = tag.GetBool("Naga");
			}
            if (tag.ContainsKey("Mizuchi"))
            {
                Upgrades[1] = tag.GetBool("Mizuchi");
            }
            if (tag.ContainsKey("Technos"))
            {
                Upgrades[2] = tag.GetBool("Technos");
            }
            if (tag.ContainsKey("Orochi"))
            {
                Upgrades[3] = tag.GetBool("Orochi");
            }
            if (tag.ContainsKey("Vasuki"))
            {
                Upgrades[4] = tag.GetBool("Vasuki");
            }
            if (tag.ContainsKey("Dullahan"))
            {
                Upgrades[5] = tag.GetBool("Dullahan");
            }
        }

		public override void SaveData(TagCompound tag)
		{
			tag["Naga"] = Upgrades[0];
            tag["Mizuchi"] = Upgrades[1];
            tag["Technos"] = Upgrades[2];
            tag["Orochi"] = Upgrades[3];
            tag["Vasuki"] = Upgrades[4];
            tag["Dullahan"] = Upgrades[5];
        }

		public override void UpdateInventory(Player player)
		{
            for (int i = 0; i < orochiDroneIndex.Count; i++)
            {
                Projectile theProjectileInQuestion = Main.projectile[orochiDroneIndex[i]];
                if (theProjectileInQuestion.ModProjectile is not OrochiDrone || !theProjectileInQuestion.active)
                {
                    orochiDroneIndex.RemoveAt(i);
                }
            }

			int upgradeLevel = 0;
            tooltip = "A pistol that shoots hair darts, allows Azure Striker and Azure Thunderclap to \nmark an enemy and shock them with their Flashfield to deal damage.";
            for (int i = 0; i < Upgrades.Length; i++)
            {
                if (Upgrades[i])
                {
					upgradeLevel++;
                    tooltip += upgradeDescriptions[i];
                }
            }

            // Resets the item's stats
            Item.damage = 2;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<HairDartProjectile>();
            Item.shootSpeed = 10;

            Item.useTime = 10;
            ai0 = 0;
            ai1 = 0;

            if (Upgrades[1])
            {
                Item.shootSpeed = 16;
                ai0 = 2;
                ai1 = 1;
            }
            if (Upgrades[2])
            {
                ai0 = 4;
            }
            if (Upgrades[3])
            {
                ai0 = 6;
            }
            if (Upgrades[4])
            {
                ai0 = 8;
                ai1 = 2;
            }
            if (Upgrades[5])
            {
                ai0 = 10;
                Item.damage = 30;
                Item.knockBack = 2;
                Item.shoot = ModContent.ProjectileType<DullahanProjectile>();
                Item.useTime = 6;
            }
            if (Upgrades[1] && Upgrades[4])
            {
                ai1 = 3;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			for (int i = 0; i < tooltips.Count; i++)
			{
				if (tooltips[i].Name == "Tooltip0")
				{
					tooltips[i].Text = tooltip;
                }
			}
            base.ModifyTooltips(tooltips);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            // ai0 will be penetration, set to an odd number for penetration and an even number to disable penetration, ai1 will be homing, Mizuchi effects will be 2, Vasuki effects will be 2, and both will be 3
            // Maybe make two separate projectiles to shoot, like one for dullahan or Naga
            if (Upgrades[0]) ai0++;
            if (Upgrades[3] && orochiDroneIndex.Count <= 3)
            {
                orochiDroneIndex.Add(Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<OrochiDrone>(), 0, 0, player.whoAmI));
            }
            if (Upgrades[2])
			{
				Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(45)), type, damage*2, knockback, player.whoAmI, ai0, ai1);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-45)), type, damage*2, knockback, player.whoAmI, ai0, ai1);
            }
            Projectile.NewProjectile(source, position, velocity, type, damage * 2, knockback, player.whoAmI, ai0, ai1);
            return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.Register();
		}
	}
}
