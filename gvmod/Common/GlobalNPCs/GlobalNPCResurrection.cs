using gvmod.Content.Items.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalNPCs
{
    public class GlobalNPCResurrection : GlobalNPC
    {
        public bool reincarnation;
        private double oldTime;

        public override bool InstancePerEntity => true;

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (reincarnation)
            {
                modifiers.FinalDamage *= 3.5f;
            }
            base.ModifyHitByItem(npc, player, item, ref modifiers);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (reincarnation)
            {
                modifiers.FinalDamage *= 2.5f;
            }
            base.ModifyHitByProjectile(npc, projectile, ref modifiers);
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (reincarnation)
            {
                modifiers.FinalDamage *= 1.5f;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (reincarnation)
            {
                Main.time = oldTime;
            }
            return base.PreAI(npc);
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.ai[0] == 298)
            {
                oldTime = Main.time;
                reincarnation = true;
            } else
            {
                reincarnation = false;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new CanDropMirrorPiece(), ModContent.ItemType<MirrorShard>(), 1, 1, 1));
        }

        public override bool PreKill(NPC npc)
        {
            if (canReincarnate(npc) && Main.rand.NextBool(4))
            {
                switch (npc.type)
                {
                    case NPCID.KingSlime:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.EyeofCthulhu:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.BrainofCthulhu:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.SkeletronHead:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.QueenBee:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.WallofFlesh:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.TheDestroyer:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.Retinazer:
                    case NPCID.Spazmatism:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.SkeletronPrime:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.Plantera:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.Golem:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.DukeFishron:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.CultistBoss:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.MoonLordCore:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    case NPCID.QueenSlimeBoss:
                        if (NPC.downedSlimeKing)
                        {
                        }
                        NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        break;
                    case NPCID.HallowBoss:
                        if (NPC.downedSlimeKing)
                        {
                            NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        }
                        break;
                    default:
                        NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, ai0: 298);
                        break;
                }
            }
            return base.PreKill(npc);
        }

        private bool canReincarnate(NPC npc)
        {
            return (npc.boss && !reincarnation && Main.BestiaryTracker.Kills.GetKillCount(npc) >= 2 && 
                Main.expertMode);
        }
    }

    public class CanDropMirrorPiece : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => info.npc.GetGlobalNPC<GlobalNPCResurrection>().reincarnation;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "If an enemy has a mirror shard, it's power will resurrect it after defeat." +
            "\nExpert mode only.";
    }
}
