using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattlemageArena.Core.Entities;
using BattlemageArena.GameLogic.Entities;
using Microsoft.Xna.Framework.Net;

namespace BattlemageArena.GameLogic.Behaviors
{
    class NetPlayerBehavior : NetworkBehavior
    {
        public NetPlayerBehavior(Entity parent) : base(parent, "Player", 64, true)
        {
            
        }

        public override void SendData(PacketWriter writer)
        {
            Player player = (Entity as Player);

            if (player != null)
            {
                writer.Write(player.Position);
                writer.Write((int) player.Direction);
                writer.Write(player.Health);
            }
        }

        public override void ReceiveData(PacketReader reader)
        {
            Player player = (Entity as Player);

            if (player != null)
            {
                player.Position = reader.ReadVector2();
                player.Direction = (Direction) reader.ReadInt32();
                player.Health = reader.ReadInt32();
            }
        }
    }
}
