﻿using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Helpers.Binary;
using UCS.Logic;
using System.Diagnostics;

namespace UCS.Packets.Commands.Client
{
    // Packet 502
    internal class UpgradeBuildingCommand : Command
    {
        public int BuildingId;
        public uint Unknown1;
        public uint Unknown2;

        public UpgradeBuildingCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
            
        }
        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown2 = this.Reader.ReadByte();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var b = (ConstructionItem)go;
                if (b.CanUpgrade())
                {
                    var bd = b.GetConstructionItemData();
                    if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel() + 1),bd.GetBuildCost(b.GetUpgradeLevel() + 1)))
                    {
                        if (this.Device.Player.HasFreeWorkers())
                        {
                            string name = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId).GetData().GetName();
                            Logger.Say("Building To Upgrade : " + name + " (" + BuildingId + ')');
                            if (string.Equals(name, "Alliance Castle"))
                            {
                                ca.IncrementAllianceCastleLevel();
                                Building a = (Building)this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
                                BuildingData al = a.GetBuildingData();
                                ca.SetAllianceCastleTotalCapacity(al.GetUnitStorageCapacity(ca.GetAllianceCastleLevel()));
                            }
                            else if (string.Equals(name, "Town Hall"))
                                ca.IncrementTownHallLevel();

                            var rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                            ca.SetResourceCount(rd, ca.GetResourceCount(rd) - bd.GetBuildCost(b.GetUpgradeLevel() + 1));
                            b.StartUpgrading();
                        }
                    }
                    else
                    {
                        Debug.Write("[Debug] cannot upgrade not enough resources ?");
                    }
                }
                else
                {
                    Debug.Write("[Debug] cannot upgrade for some reason ");
                }
            }
            else
            {
                Debug.Write("[Debug] some how gameobject is equal to null");
            }
        }
    }
}
