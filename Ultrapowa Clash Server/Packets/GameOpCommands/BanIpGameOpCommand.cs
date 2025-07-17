using System;
using UCS.Core;
using UCS.Core.Network;
using UCS.Logic;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.GameOpCommands
{
    internal class BanIpGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public BanIpGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(3);
        }

        public override async void Execute(Level level)
        {
            if (GetRequiredAccountPrivileges())
                if (m_vArgs.Length >= 1)
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var l = await ResourcesManager.GetPlayer(id);
                        if (l != null)
                            if (l.Avatar.AccountPrivileges < level.Avatar.AccountPrivileges)
                            {
                                l.Avatar.AccountBanned = true;
                                l.Avatar.AccountPrivileges = 0; ;
                                if (ResourcesManager.IsPlayerOnline(l))
                                {
                                    Processor.Send(new OutOfSyncMessage(l.Client));
                                }
                            }
                            else
                            {
                            }
                        else
                        {
                        }
                    }
                    catch 
                    {
                    }
                else
                    SendCommandFailedMessage(level.Client);
        }
    }
}
