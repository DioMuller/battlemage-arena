using System;
using System.Collections.Generic;
using System.Net.Configuration;
using BattlemageArena.Core.Entities;
using BattlemageArena.GameLogic.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace BattlemageArena.GameLogic.Behaviors
{
    public abstract class NetworkBehavior : Behavior
    {
        #region Attributes
        private float _timeBetween;
        private float _timeElapsed;
        private LocalNetworkGamer _player;
        #endregion Attributes

        #region Properties
        public string Type { get; private set; }
        public int Id { get; private set; }
        #endregion Properties

        #region Constructor
        public NetworkBehavior(Entity parent, string type, float timeBetween, bool unique = true) : base(parent)
        {
            this.Id = Convert.ToInt32(GameMain.CurrentSession.LocalGamers[0].Id);

            if (!unique)
            {
                Id += GameMain.Connection.GetUniqueValue()*1000;
            }
            else
            {
                GameMain.CurrentLevel.NetController = this;
            }

            this.Type = type;

            _player = GameMain.CurrentSession.LocalGamers[0];

            _timeBetween = _timeElapsed = timeBetween;

            GameMain.Connection.AddBehavior(this);
        }

        public NetworkBehavior(Entity parent, string type, float timeBetween, int id)
            : base(parent)
        {
            this.Id = id;
            this.Type = type;

            _player = GameMain.CurrentSession.LocalGamers[0];

            _timeBetween = _timeElapsed = timeBetween;

            GameMain.Connection.AddBehavior(this);
        }
        #endregion Constructor

        #region Methods
        public abstract void SendData(PacketWriter writer);

        public abstract void ReceiveData(PacketReader writer);

        public override void Update(GameTime gameTime)
        {
            if (!GameMain.CurrentSession.IsHost &&
                Id != GameMain.CurrentSession.LocalGamers[0].Id)
            {
                // Not my object!
                return;
            }

            _timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (_timeElapsed >= _timeBetween)
            {
                /* Protocol will always be:
                 *  Type (string)
                 *  Id (int)
                 *  Data (varies)
                 */
                PacketWriter writer = new PacketWriter();
                writer.Write(Type);
                writer.Write(Id);

                SendData(writer);

                _player.SendData(writer, SendDataOptions.ReliableInOrder);

                _timeElapsed -= _timeBetween;
            }
        }
        #endregion Methods
    }
}