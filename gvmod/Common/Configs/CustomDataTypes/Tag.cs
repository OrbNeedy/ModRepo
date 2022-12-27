using Terraria;

namespace gvmod.Common.Configs.CustomDataTypes
{
    public class Tag
    {
        public int NpcIndex { get; set; }
        public int Level { get; set; }
        public int Timer { get; set; }
        public bool Active { get; set; }
        public int ShockIframes { get; set; }

        public Tag(int npcIndex)
        {
            NpcIndex = npcIndex;
            Level = 1;
            Timer = 0;
            ShockIframes = 0;
            Active = true;
        }

        public void Update()
        {
            Timer++;
            NPC theNpcInQuestion = Main.npc[NpcIndex];
            if (!theNpcInQuestion.active || theNpcInQuestion.life <= 0 || Timer >= 600)
            {
                Active = false;
            }
            
            if (ShockIframes > 0)
            {
                ShockIframes--;
            }
            
            if (!Active)
            {
                Level = 0;
            }
        }

        public void IncreaseTag()
        {
            if (Level < 3) Level++;
            Timer = 0;
        }
    }
}
