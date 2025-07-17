﻿using System;
using UCS.Core;
using UCS.Core.Network;
using UCS.Core.Threading;
using UCS.Logic;
using UCS.Logic.AvatarStreamEntry;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.GameOpCommands
{
    internal class HelpGameOpCommand: GameOpCommand
    {
        readonly string[] m_vArgs;

        public HelpGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 1)
                {
                    ClientAvatar avatar = level.Avatar;
                    GlobalChatLineMessage _MSG = new GlobalChatLineMessage(level.Client);
                    _MSG.PlayerName = "Server";
                    _MSG.LeagueId = 22;
                    _MSG.Message = "Your ID: " + level.Avatar.UserId;
                    _MSG.Message = @"/help" +
                                   "\n/ban" +
                                   "\n/kick" +
                                   "\n/rename " +
                                   "\n/setprivileges" +
                                   "\n/shutdown" +
                                   "\n/unban" +
                                   "\n/visit" +
                                   "\n/sysmsg" +
                                   "\n/id" +
                                   "\n/min" +
                                   "\n/max" +
                                   "\n/saveacc" +
                                   "\n/saveall" +
                                   "\n/becomeleader" +
                                   "\n/status" +
                                   "\n/help" +
                                   "\n/accinfo" +
                                   "\n/togglebroadcast";

                    _MSG.Send();
                    var mail = new AllianceMailStreamEntry();
                    mail.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    mail.SetSender(avatar);
                    mail.IsNew = 2;
                    mail.AllianceId = 0;
                    mail.AllianceBadgeData = 1526735450;
                    mail.AllianceName = "UCS Server Commands Help";
                    //var p = new AvatarStreamEntryMessage(level.Client);
                    //p.SetAvatarStreamEntry(mail);
                    //Processor.Send(p);
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }
    }
}
