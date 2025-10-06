using System.Collections.Generic;
using UCS.Helpers.List;
using UCS.Logic;

namespace UCS.Packets.Messages.Server
{
    // Packet 24310
    internal class AllianceListMessage : Message
    {
        public AllianceListMessage(Device client) : base(client)
        {
            this.Identifier = 24310;
            this.m_vAlliances = new List<Alliance>();
        }

        internal List<Alliance> m_vAlliances;
        internal string m_vSearchString;

        internal override void Encode()
        {
            this.Data.AddString(m_vSearchString);
            this.Data.AddInt(m_vAlliances.Count);

            foreach(Alliance alliance in m_vAlliances)
            {                
                if(alliance != null)
                {
                    this.Data.AddRange(alliance.EncodeFullEntry());
                }
                else
                {
                    // Remove alliance from db
                }
            }
        }
    }
}
