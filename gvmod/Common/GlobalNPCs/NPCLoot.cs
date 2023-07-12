using gvmod.Content.Items.Accessories;
using gvmod.Content.Items.Drops;
using gvmod.Content.Items.Upgrades;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalNPCs
{
    public class NPCLoot : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, Terraria.ModLoader.NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.Nailhead:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuperMagnet>(), 1, 1, 8));
                    break;
                case NPCID.Tim:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArcanumConverter>(), 5, 1, 1));
                    break;
                case NPCID.CultistArcherBlue:
                case NPCID.CultistDevote:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunaticCloth>(), 1, 2, 6));
                    break;
                case NPCID.EyeofCthulhu:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NagaBolt>(), 1, 1));
                    break;
                case NPCID.SkeletronHead:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TechnosBolt>(), 1, 1));
                    break;
                case NPCID.QueenSlimeBoss:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MizuchiBolt>(), 1, 1));
                    break;
                case NPCID.Plantera:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrochiBolt>(), 1, 1));
                    break;
                case NPCID.DukeFishron:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OuroborosBolt>(), 1, 1));
                    break;
                case NPCID.CultistBoss:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunaticCloth>(), 1, 6, 12));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VasukiBolt>(), 1, 1));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DragonRadiation>(), 1, 1));
                    break;
                case NPCID.LunarTowerNebula:
                case NPCID.LunarTowerSolar:
                case NPCID.LunarTowerStardust:
                case NPCID.LunarTowerVortex:
                    if (Main.expertMode || Main.masterMode)
                    {
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulsarFragment>(), 1, 6, 25));
                    } else
                    {
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulsarFragment>(), 1, 3, 15));
                    }
                    break;
                case NPCID.MoonLordCore:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DullahanBolt>(), 1, 1));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Fetus>(), 1, 1));
                    break;
            }
        }
    }
}
