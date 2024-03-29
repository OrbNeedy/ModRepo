﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Projectiles;
using gvmod.Content.Items.Ammo;
using Terraria.GameContent.Creative;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace gvmod.Content.Items.Weapons
{
	public class DartLeader : ModItem
    {
        // Naga, Mizuchi, Technos, Orochi, Vasuki, Dullahan, and Ouroboros respectively
        public bool[] Upgrades { get; set; } = new bool[7] { false, false, false, false, false, false, false };
        private int ai0;
        private int ai1;
        private int orochiTimer;
        private static Asset<Texture2D> glowmask;

        private static readonly string[] upgradeDescriptions = new string[7] { "\nNaga: Adds a piercing effect and increases speed.", 
			"\nMizuchi: Adds a slight homing effect to the darts.",  
			"\nTechnos: Adds two extra darts when shooting.",
			"\nOrochi: Deploys a drone when shooting that shoots extra darts, these darts \ndon't inherit other upgrades.", 
			"\nVasuki: After hitting an enemy, the enemy shoots an extra dart to the next \nnearest enemy, improves homing if paired with Mizuchi.",
			"\nDullahan: Increases damage and rate of fire.", 
            "\nOuroboros: 50% chance not to consume ammo."};
		private string tooltip;
		public override void SetStaticDefaults()		
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void Load()
        {
            if (!Main.dedServ)
            {
                glowmask = ModContent.Request<Texture2D>("gvmod/Content/Items/Weapons/DartLeaderGlowmask");
            }
        }

        public override void Unload()
        {
            glowmask = null;
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
            orochiTimer = 0;        
        }

        public override ModItem Clone(Item item)
        {
            DartLeader clone = (DartLeader)base.Clone(item);
            clone.Upgrades = (bool[])Upgrades?.Clone();
            return clone;
        }

        public override void OnCreated(ItemCreationContext context)
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
            if (tag.ContainsKey("Ouroboros"))
            {
                Upgrades[6] = tag.GetBool("Ouroboros");
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
            tag["Ouroboros"] = Upgrades[6];
        }

		public override void UpdateInventory(Player player)
		{
            if (orochiTimer > 0) orochiTimer--;

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

            Item.damage = 2;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<HairDartProjectile>();
            Item.shootSpeed = 10;

            Item.useTime = 10;
            Item.useAnimation = 10;
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
                Item.useAnimation = 6;
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
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Upgrades[6])
            {
                return Main.rand.NextFloat() >= 0.5f;
            } else
            {
                return true;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            int realDamage = damage;
            if (!Upgrades[5])
            {
                realDamage = 1;
            }
            if (Upgrades[0]) ai0++;
            if (Upgrades[3] && orochiTimer == 0)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<OrochiDrone>(), 0, 0, player.whoAmI);
                orochiTimer = 60;
            }
            if (Upgrades[2])
			{
				Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(45)), type, realDamage, knockback, player.whoAmI, ai0, ai1);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-45)), type, realDamage, knockback, player.whoAmI, ai0, ai1);
            }
            Projectile.NewProjectile(source, position, velocity, type, realDamage, knockback, player.whoAmI, ai0, ai1);
            return false;
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw(
                glowmask.Value,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - glowmask.Value.Height * 0.5f
                ),
                new Rectangle(0, 0, glowmask.Value.Width, glowmask.Value.Height),
                Color.White,
                rotation,
                glowmask.Value.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void AddRecipes()
		{
			CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 10)
                .AddIngredient(ItemID.Granite, 5)
                .AddIngredient(ItemID.Marble, 5)
                .AddTile(TileID.Anvils)
				.Register();
		}
	}
}
