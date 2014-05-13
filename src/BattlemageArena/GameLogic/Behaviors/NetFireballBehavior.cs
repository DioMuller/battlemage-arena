using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.GameLogic.Entities;
using Microsoft.Xna.Framework.Net;

namespace BattlemageArena.GameLogic.Behaviors
{
    class NetFireballBehavior : NetworkBehavior
    {
        public NetFireballBehavior(Entity parent) : base(parent, "Fireball", 64, true)
        {
            
        }

        public override void SendData(PacketWriter writer)
        {
            Fireball fireball = (Entity as Fireball);

            if (fireball != null)
            {
                writer.Write(fireball.Position);
                writer.Write(fireball.Removed);
            }
        }

        public override void ReceiveData(PacketReader reader)
        {
            Fireball fireball = (Entity as Fireball);

            if (fireball != null)
            {
                fireball.Position = reader.ReadVector2();
                fireball.Removed = reader.ReadBoolean();

                if(fireball.Removed) GameMain.CurrentLevel.RemoveEntity(fireball);
            }
        }
    }
}
