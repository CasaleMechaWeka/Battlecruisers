using System;
using UnityEngine;

namespace BattleCruisers.Movement
{
    public class CruiserJoint : MonoBehaviour
    {
        private Transform _cruiser;

        public Transform Cruiser
        {
            get => _cruiser;
            set => _cruiser = value;
        }

        private void Update()
        {
            transform.position = new Vector3(_cruiser.transform.position.x,_cruiser.transform.position.y,transform.position.z);
        }
    }
}