using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Movement.Deciders
{
    /// <summary>
    /// Decides whether a ship should start or stop moving.
    /// 
    /// Ship stops moving when:
    /// 1. Blocking friendly
    /// 2. Blocking enemy
    /// 3. Have in range target, and no higher priority target is attacking us.
    /// 
    /// Otherwise ships starts moving.
    /// </summary>
    public class ShipMovementDecider : IMovementDecider
    {


        public void DisposeManagedState()
        {
            throw new System.NotImplementedException();
        }
    }
}
