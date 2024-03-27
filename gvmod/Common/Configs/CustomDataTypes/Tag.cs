using Terraria;

namespace gvmod.Common.Configs.CustomDataTypes
{
    public class Tag
    {
        public int Index { get; set; }
        public bool IsPlayer { get; set; }
        public int Level { get; set; }
        public int Timer { get; set; }
        public bool Active { get; set; }
        public int ShockIframes { get; set; }

        public Tag(int Index, bool IsPlayer=false)
        {
            this.Index = Index;
            this.IsPlayer = IsPlayer;
            Level = 1;
            Timer = 0;
            ShockIframes = 0;
            Active = true;
        }

        public void Update()
        {
            Timer++;

            if (IsPlayer)
            {

            }else
            {
                NPC theNpcInQuestion = Main.npc[Index];
                if (!theNpcInQuestion.active || theNpcInQuestion.life <= 0 || Timer >= 600)
                {
                    Active = false;
                }
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
