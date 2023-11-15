using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPDroneConsumer : IPvPDroneConsumer
    {
        private readonly IPvPDroneManager _droneManager;

        private int _numOfDrones;
        public int NumOfDrones
        {
            get { return _numOfDrones; }
            set
            {
                if (value < 0
                    || (value != 0 && value < NumOfDronesRequired))
                {
                    throw new ArgumentException();
                }

                if (value != _numOfDrones)
                {
                    //Debug.Log($"required: {NumOfDronesRequired}   actual: {value}");
                    _numOfDrones = value;

                    DroneNumChanged?.Invoke(this, new PvPDroneNumChangedEventArgs(_numOfDrones));

                    PvPDroneConsumerState newState = FindDroneState(_numOfDrones, NumOfDronesRequired);

                    if (newState != State)
                    {
                        DroneStateChanged?.Invoke(this, new PvPDroneStateChangedEventArgs(State, newState));
                        State = newState;
                    }
                }
            }
        }

        public int NumOfDronesRequired { get; set; }
        public int NumOfSpareDrones => NumOfDrones - NumOfDronesRequired;
        public PvPDroneConsumerState State { get; private set; }

        public event EventHandler<PvPDroneNumChangedEventArgs> DroneNumChanged;
        public event EventHandler<PvPDroneStateChangedEventArgs> DroneStateChanged;

        public PvPDroneConsumer(int numOfDronesRequired, IPvPDroneManager droneManager)
        {
            if (numOfDronesRequired < 0)
            {
                throw new ArgumentException();
            }
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            NumOfDronesRequired = numOfDronesRequired;
            NumOfDrones = 0;
            State = PvPDroneConsumerState.Idle;
        }

        private PvPDroneConsumerState FindDroneState(int numOfDrones, int numOfDronesRequired)
        {
            if (numOfDrones == _droneManager.NumOfDrones)
            {
                return PvPDroneConsumerState.AllFocused;
            }
            else if (numOfDrones > numOfDronesRequired)
            {
                return PvPDroneConsumerState.Focused;
            }
            else if (numOfDrones == numOfDronesRequired)
            {
                return PvPDroneConsumerState.Active;
            }
            else if (numOfDrones == 0)
            {
                return PvPDroneConsumerState.Idle;
            }
            throw new InvalidProgramException();
        }
    }
}
