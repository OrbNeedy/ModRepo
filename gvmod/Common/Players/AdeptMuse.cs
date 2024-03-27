using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using gvmod.Content.Items.Accessories;
using gvmod.Content;

namespace gvmod.Common.Players
{
    public class AdeptMuse : ModPlayer
    {
        public bool AnthemBuff { get; set; }
        public bool HasAMuseItem { get; set; }
        public int MinutesWithMuseItems { get; set; }
        public int SecondsWithMuseItems { get; set; }
        public int[] MuseItems { get; private set; }
        public bool HasMirrorShard { get; set; }
        public bool HasWholeMirror { get; set; }
        public bool HasDjinnLamp { get; set; }
        public int AnthemLevel { get; set; }

        public override void Initialize()
        {
            AnthemBuff = false;
            MinutesWithMuseItems = 0;
            SecondsWithMuseItems = 0;
            MuseItems = new int[] { ModContent.ItemType<MirrorShard>(), 
                ModContent.ItemType<FalconQuill>(),
                ModContent.ItemType<Grimoire>() };
            HasMirrorShard = false;
            HasWholeMirror = false;
            HasDjinnLamp = false;
            AnthemLevel = 0;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("MuseMinutes"))
            {
                MinutesWithMuseItems = tag.GetInt("MuseMinutes");
            }
            if (tag.ContainsKey("MuseSeconds"))
            {
                SecondsWithMuseItems = tag.GetInt("MuseSeconds");
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["MuseMinutes"] = MinutesWithMuseItems;
            tag["MuseSeconds"] = SecondsWithMuseItems;
        }

        public override void ResetEffects()
        {
            HasMirrorShard = false;
            HasWholeMirror = false;
            HasDjinnLamp = false;
            AnthemBuff = false;
            AnthemLevel = 0;
            HasAMuseItem = false;
        }

        public override void PreUpdateBuffs()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (AnthemBuff)
            {
                if (HasDjinnLamp)
                {
                    AnthemLevel = 5;
                }
                else if (HasWholeMirror)
                {
                    AnthemLevel = 3;
                }
                else
                {
                    AnthemLevel = 1;
                }
                if (AnthemLevel > 1)
                {
                    adept.SPUsageModifier *= 0;
                }
                adept.SPRegenModifier += (0.33f * AnthemLevel);
                adept.SPRegenOverheatModifier += 2;
                adept.SpecialDamageEquipMult += (0.01f * AnthemLevel);
                adept.SecondaryDamageEquipMult += (0.01f * AnthemLevel);
                Player.GetDamage<SeptimaDamageClass>() += (0.01f * AnthemLevel);
            }
            base.PostUpdateBuffs();
        }

        public override void PostUpdate()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (HasAMuseItem)
            {
                SecondsWithMuseItems++;
            }

            if (SecondsWithMuseItems >= 3600)
            {
                SecondsWithMuseItems = 0;
                MinutesWithMuseItems++;
            }

            if (MinutesWithMuseItems >= 5 && !adept.SPUpgrades[0] && Main.hardMode)
            {
                adept.SPUpgrades[0] = true;
                adept.MaxSeptimalPower += 75;
                Main.NewText("Your septimal energy has increased!", adept.Septima.MainColor);
            }

            if (MinutesWithMuseItems >= 20 && !adept.SPUpgrades[1] && NPC.downedPlantBoss)
            {
                adept.SPUpgrades[1] = true;
                adept.MaxSeptimalPower += 75;
                Main.NewText("Your septimal energy has increased yet again!", adept.Septima.MainColor);
            }

            if (MinutesWithMuseItems >= 45 && !adept.SPUpgrades[2] && NPC.downedMoonlord)
            {
                adept.SPUpgrades[2] = true;
                adept.MaxSeptimalPower += 100;
                Main.NewText("Your septimal energy has been maxed!", adept.Septima.MainColor);
            }
        }
    }
}
