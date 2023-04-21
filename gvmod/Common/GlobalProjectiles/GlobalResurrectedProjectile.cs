using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalProjectiles
{
    public class GlobalResurrectedProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => base.InstancePerEntity;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            base.OnSpawn(projectile, source);
        }
    }
}
