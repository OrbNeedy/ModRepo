using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Common.Players;
using Terraria.DataStructures;

namespace gvmod.Content.Items.Drops
{
    public class Experience : ModItem
    {
        public int experience = 10;
        //TODO: Make sure this can only be picked up by a certain player

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Experience");
			Tooltip.SetDefault("Contains the life essence of enemies, collect it to make it yours.\n" +
                "A strange type of septima allowed you to see this, but I don't remember giving you any similar power, did I?.\n" +
                "If i'm being deadass with you " + Main.LocalPlayer.name + ", you're starting to seem a bit sussy to me.");
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
            Item.stack = 2;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = 10;
			Item.UseSound = SoundID.Drip;
            Item.consumable = true;
		}

        public override void OnSpawn(IEntitySource source)
        {
            if (source is NPC)
            {
                NPC npc = (NPC)source;
                Main.NewText("True");
                float lifeMax = npc.lifeMax;
                float damage = npc.damage;
                float defense = npc.defense;
                int calc = (int)(((defense * 0.2) + (damage * 0.1)) * (lifeMax * 0.02));
                experience = calc;
            }
        }

        public override void OnConsumeItem(Player player)
        {
            player.GetModPlayer<AdeptPlayer>().experience += experience;
            Main.NewText("Experience gained! Total experience: " + player.GetModPlayer<AdeptPlayer>().experience);
            base.OnConsumeItem(player);
        }

        public override bool OnPickup(Player player)
        {
            player.GetModPlayer<AdeptPlayer>().experience += experience;
            Main.NewText("Experience gained! Total experience: " + player.GetModPlayer<AdeptPlayer>().experience);
            return false;
        }
    }
}
