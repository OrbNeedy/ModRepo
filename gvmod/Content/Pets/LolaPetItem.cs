using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using gvmod.Common.Players;
using Terraria.ModLoader.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace gvmod.Content.Pets
{
    public class LolaPetItem : ModItem
    {
        private static Asset<Texture2D> exterior;
        private Color bodyColor = Color.White;
        // UID: {Charname: relation}
        // private Dictionary<string, Dictionary<string, int>> peopleRelationships = new Dictionary<string, Dictionary<string, int>>();
        // UID, Charname
        // private (string, string) originalOwner = (string.Empty, string.Empty);

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        /*public override void OnCreated(ItemCreationContext context)
        {
            if (context is RecipeItemCreationContext)
            {
                originalOwner.Item1 = typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString();
                originalOwner.Item2 = Main.LocalPlayer.name;
                peopleRelationships.Add(originalOwner.Item1, new Dictionary<string, int>() { [originalOwner.Item2] = 0 });
            }
        }*/

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            /*// Owned by a player
            if (originalOwner.Item1 != string.Empty)
            {
                // Owned by you
                if (originalOwner.Item1 == typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString())
                {
                    // Owned by a character
                    if (originalOwner.Item2 != string.Empty)
                    {
                        // Owned by your current character
                        if (Main.LocalPlayer.name == originalOwner.Item2)
                        {
                            tooltips.Add(new TooltipLine(Mod, "OwnLola", "She seems affectionate towards you."));
                        }
                        // Not owned by your current character
                        else
                        {
                            tooltips.Add(new TooltipLine(Mod, "NotOwnLola", "You remind her of someone, but she doesn't know you."));
                        }
                    }
                    // Not owned by a character
                    else
                    {
                        tooltips.Add(new TooltipLine(Mod, "SemiOwnLola", "She's considering to call you her boss."));
                    }
                }
                // Not owned by you
                else
                {
                    // Owned by a character
                    if (originalOwner.Item2 != string.Empty)
                    {
                        // Same name as owner
                        if (Main.LocalPlayer.name == originalOwner.Item2)
                        {
                            tooltips.Add(new TooltipLine(Mod, "ConfusedLola", "She says you have the same name as a dear friend."));
                        }
                        // Complete strangers
                        else
                        {
                            tooltips.Add(new TooltipLine(Mod, "NotOwnLola", "."));
                        }
                    }
                    // Not owned by a character
                    else
                    {
                        tooltips.Add(new TooltipLine(Mod, "AmnesiaLola", "She can't recall her original owner."));
                    }
                }
            }
            // Not owned by a player
            else
            {
                // Owned by a character
                if (originalOwner.Item2 != string.Empty)
                {
                    tooltips.Add(new TooltipLine(Mod, "ImpossibleLola", "UID empty, Name not empty."));
                } 
                // Not owned by anyone
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "FreeLola", "She knows no one, might get attached to you."));
                }
            }*/
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                exterior = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetItemColorChange");
            }
        }

        public override void Unload()
        {
            exterior = null;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<LolaPetProjectile>(), ModContent.BuffType<LolaPetBuff>());

            Item.width = 38;
            Item.height = 38;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(0, 0, 50);
        }

        public override void LoadData(TagCompound tag)
        {
            /*if (tag.ContainsKey("Relationships"))
            {
                TagCompound keyValuePairs = tag.GetCompound("Relationships");
                foreach ((string key, TagCompound value) in keyValuePairs)
                {
                    TagCompound new = keyValuePairs[key];
                    foreach ()
                }
            }*/
            if (tag.ContainsKey("Color"))
            {
                byte[] color = tag.GetByteArray("Color");
                bodyColor = new Color(color[0], color[1], color[2], color[3]);
            }
        }

        public override void SaveData(TagCompound tag)
        {
            // "UID:character:relation:character2:relation2"

            /*TagCompound myData = new();
            foreach ((string user, Dictionary<string, int> characters) in peopleRelationships)
            {
                var subTag = new TagCompound();
                foreach ((string character, int friendship) in peopleRelationships[user])
                {
                    subTag[character] = friendship;
                }
                myData[user] = subTag;
            }
            tag["Relationships"] = myData;*/

            tag["Color"] = new byte[] { bodyColor.R, bodyColor.G, bodyColor.B, bodyColor.A};
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            return false;
        }

        public override void UpdateInventory(Player player)
        {
            bodyColor = player.GetModPlayer<AdeptPlayer>().Septima.MainColor;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            bodyColor = player.GetModPlayer<AdeptPlayer>().Septima.MainColor;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(
                exterior.Value,
                position,
                frame,
                bodyColor,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0
            );
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw(
                exterior.Value,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - exterior.Value.Height * 0.5f
                ),
                new Rectangle(0, 0, exterior.Value.Width, exterior.Value.Height),
                Lighting.GetColor(Item.Center.ToTileCoordinates(), bodyColor),
                rotation,
                exterior.Value.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
