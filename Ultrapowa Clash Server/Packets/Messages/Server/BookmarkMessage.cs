using System.Collections.Generic;
using UCS.Core;
using UCS.Helpers.List;
using UCS.Logic;
using UCS.Logic.DataSlots;

namespace UCS.Packets.Messages.Server
{
    // Packet 24340
    internal class BookmarkMessage : Message
    {
        public ClientAvatar Player;
        public int i;

        public BookmarkMessage(Device client) : base(client)
        {
            this.Identifier = 24340;
            Player = client.Player.Avatar;
        }

        internal override async void Encode()
        {

            List<byte> list = new List<byte>();
            List<BookmarkSlot> rem = new List<BookmarkSlot>();
            foreach (var p in Player.BookmarkedClan)
            {
                Alliance a = ObjectManager.GetAlliance(p.Value);
                if (a != null)
                {
                    list.AddLong(p.Value);
                    i++;
                }
                else
                {
                    rem.Add(p);
                    if (i > 0)
                        i--;
                }
            }
            this.Data.AddInt(i);
            this.Data.AddRange(list);
            foreach (BookmarkSlot im in rem)
            {
                Player.BookmarkedClan.RemoveAll(t => t == im);
            }
        }
    }
}