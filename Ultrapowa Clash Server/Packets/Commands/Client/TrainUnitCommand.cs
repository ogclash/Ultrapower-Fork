using System;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Helpers.Binary;
using UCS.Logic;

namespace UCS.Packets.Commands.Client
{
    // Packet 508
    internal class TrainUnitCommand : Command
    {
        public TrainUnitCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
            
        }

        internal override void Decode()
        {
            int t1 = this.Reader.ReadInt32();
            uint t2 = this.Reader.ReadUInt32();
            this.UnitType = this.Reader.ReadInt32();
            this.Count    = this.Reader.ReadInt32();
            uint t3 = this.Reader.ReadUInt32();
            Tick     = this.Reader.ReadInt32();
        }

        public int Count;
        public int UnitType;
        public int Tick;

        internal override void Process()
        {
            ClientAvatar _Player = this.Device.Player.Avatar;
            
            if (UnitType.ToString().StartsWith("400"))
            {
                CombatItemData _TroopData = (CombatItemData)CSVManager.DataTables.GetDataById(UnitType);
                List<DataSlot> _PlayerUnits = this.Device.Player.Avatar.GetUnits();
                ResourceData _TrainingResource = _TroopData.GetTrainingResource();

                var gameobjects = this.Device.Player.GameObjectManager.GetComponentManager();
                var troops = 0;
                foreach (var unit in _PlayerUnits)
                {
                    var unitData = (UCS.Files.Logic.CharacterData) unit.Data;
                    var housingSpace = unitData.HousingSpace;
                    troops += unit.Value * housingSpace;
                }

                if (_TroopData != null)
                {
                    var unitData = (UCS.Files.Logic.CharacterData)_TroopData;
                    var housingSpace = unitData.HousingSpace;
                    troops += Count * housingSpace;
                }
                else
                    return;
                if (troops <= gameobjects.GetTotalMaxHousing())
                {
                    DataSlot _DataSlot = _PlayerUnits.Find(t => t.Data.GetGlobalID() == _TroopData.GetGlobalID());
                    if (_DataSlot != null)
                    {
                        _DataSlot.Value = _DataSlot.Value + this.Count;
                    }
                    else
                    {
                        DataSlot ds = new DataSlot(_TroopData, this.Count);
                        _PlayerUnits.Add(ds);
                    }

                    _Player.SetResourceCount(_TrainingResource, _Player.GetResourceCount(_TrainingResource) - _TroopData.GetTrainingCost(_Player.GetUnitUpgradeLevel(_TroopData)));
                }
            }
            else if (UnitType.ToString().StartsWith("260"))
            {
                SpellData _SpellData = (SpellData)CSVManager.DataTables.GetDataById(UnitType);
                List<DataSlot> _PlayerSpells = this.Device.Player.Avatar.GetSpells();
                ResourceData _CastResource = _SpellData.GetTrainingResource();

                var gameobjects = this.Device.Player.GameObjectManager.GetComponentManager();
                var spells = 0;
                foreach (var unit in _PlayerSpells)
                {
                    var unitData = (UCS.Files.Logic.SpellData) unit.Data;
                    var housingSpace = unitData.HousingSpace;
                    spells += unit.Value * housingSpace;
                }
                
                if (_SpellData != null && spells < gameobjects.GetTotalMaxHousing(true))
                {
                    DataSlot _DataSlot = _PlayerSpells.Find(t => t.Data.GetGlobalID() == _SpellData.GetGlobalID());
                    if (_DataSlot != null)
                    {
                        _DataSlot.Value = _DataSlot.Value + this.Count;
                    }
                    else
                    {
                        DataSlot ds = new DataSlot(_SpellData, this.Count);
                        _PlayerSpells.Add(ds);
                    }

                    _Player.SetResourceCount(_CastResource, _Player.GetResourceCount(_CastResource) - _SpellData.GetTrainingCost(_Player.GetUnitUpgradeLevel(_SpellData)));
                }
            }
        }
    }
}

