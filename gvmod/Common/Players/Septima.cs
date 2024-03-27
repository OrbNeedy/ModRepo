using System.Collections.Generic;
using Terraria;
using gvmod.Common.Players.Septimas.Skills;
using Microsoft.Xna.Framework;

namespace gvmod.Common.Players
{
    public abstract class Septima
    {
        public Player Player { get; }
        public AdeptPlayer Adept { get; }
        public AdeptMuse Muse { get; }

        // Key is the id, Value is the type of interaction
        public virtual Dictionary<int, float> ProjectileInteractions { get => new Dictionary<int, float>(); }
        public virtual Dictionary<int, float> NPCInteractions { get => new Dictionary<int, float>(); }

        public abstract string Name { get; }

        public static int SortAbilities(Special special1, Special special2)
        {
            if (special1 == null || special1 is None)
            {
                if (special2 == null || special2 is None)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (special2 == null || special2 is None)
                {
                    return 1;
                }
                else
                {
                    int retval = special1.UnlockLevel.CompareTo(special2.UnlockLevel);

                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return special1.UnlockLevel.CompareTo(special2.UnlockLevel);
                    }
                }
            }
        }

        public int SecondaryTimer { get; set; }
        public int SecondaryCooldownTime { get; set; }

        public List<Special> Abilities { get; } = new List<Special>();
        public float ApBaseRegen { get; set; }

        public float SpBaseRegen { get; set; }
        public float SpBaseOverheatRegen { get; set; }
        public float SpBaseUsage { get; set; }

        public Vector2 VelocityMultiplier { get; set; }

        public abstract bool CanRecharge { get; }

        public abstract Color ClearColor { get; }
        public abstract Color MainColor { get; }
        public abstract Color DarkColor { get; }

        protected Septima(AdeptPlayer adept, AdeptMuse muse, Player player)
        {
            Adept = adept;
            Muse = muse;
            Player = player;
            VelocityMultiplier = new Vector2(1, 1);
        }

        public virtual void InitializeAbilitiesList()
        {
            Abilities.Clear();
            Abilities.Add(new None(Player, Adept));
        }

        public void MoveOverride()
        {
            VelocityMultiplier = new Vector2(1, 1);
            MiscMoveOverride();

            foreach (Special special in Abilities)
            {
                if (special.BeingUsed)
                {
                    special.MoveOverride();
                    Player.velocity *= special.VelocityMultiplier;
                }
            }
            Player.velocity *= VelocityMultiplier;
        }

        public virtual void PassiveStatChanges()
        {

        }

        public virtual void MiscMoveOverride()
        {

        }

        public virtual void OnOverheat()
        {

        }

        public virtual void OnRecovery()
        {

        }

        public virtual void DuringOverheat()
        {

        }

        public virtual void FirstAbilityEffects()
        {

        }

        public virtual void FirstAbility()
        {

        }

        public virtual void SecondAbilityEffects()
        {

        }
        
        public virtual void SecondAbility()
        {

        }

        /// <summary> 
        /// <para>Disables the secondary under certain circunstances without changing AdeptPlayer.CanUseSecondary.</para>
        /// </summary>
        /// <returns>False to disable secondary, true to allow it.</returns>
        public virtual bool CanUseSecondary()
        {
            return true;
        }

        /// <summary> 
        /// <para>Runs before prevasion.</para>
        /// </summary>
        /// <returns>False to skip <seealso cref="Septima.OnPrevasion(Player.HurtInfo)"/>.</returns>

        public virtual bool PrePrevasion(Player.HurtInfo info)
        {
            return true;
        }

        /// <summary> 
        /// <para>Runs before damage is taken, is skipped if the projectile or NPC is in any of the 
        /// <seealso cref="AdeptPlayer.projectileGlobalPrevasionPenetration"/>, 
        /// <seealso cref="AdeptPlayer.projectileGlobalPrevasionIgnore"/>, 
        /// <seealso cref="AdeptPlayer.npcGlobalPrevasionPenetration"/>, 
        /// <seealso cref="AdeptPlayer.npcGlobalPrevasionIgnore"/> lists.</para>
        /// </summary>
        /// <returns>True to ignore damage for this attack.</returns>
        public virtual bool OnPrevasion(Player.HurtInfo info)
        {
            return false;
        }

        /// <summary> 
        /// <para>Runs before damage is taken, does not run if <seealso cref="Septima.OnPrevasion(Player.HurtInfo)"/> returns true.</para>
        /// </summary>
        public virtual void OnHit(ref Player.HurtModifiers modifiers)
        {
            
        }

        // This method will not run if timer is less than 1, so take that in mind when making attacks
        public virtual void MorbAttack(int timer)
        {

        }

        public virtual void MiscEffects()
        {

        }

        public virtual void Updates()
        {

        }

        public List<Special> AvaliableSpecials()
        {
            List<Special> specials = new List<Special>();
            for (int i = 0; i < Abilities.Count; i++)
            {
                if (Abilities[i] == null)
                {
                    specials.Add(new None(Player, Adept, ""));
                }
                else
                {
                    if (Adept.Level >= Abilities[i].UnlockLevel)
                    {
                        specials.Add(Abilities[i]);
                    }
                }
            }
            return specials;
        }

        public List<int> AvaliableSpecialsIndex()
        {
            List<int> specials = new List<int>();
            for (int i = 0; i < Abilities.Count; i++)
            {
                if (Abilities[i] == null)
                {
                    specials.Add(0);
                } else
                {
                    if (Adept.Level >= Abilities[i].UnlockLevel)
                    {
                        specials.Add(i);
                    }
                }
            }
            if (specials.Count == 0)
            {
                specials.Add(0);
            }
            return specials;
        }

        public virtual void CheckEvolution()
        {

        }

        public virtual void UpdateEvolution()
        {

        }
    }
}
