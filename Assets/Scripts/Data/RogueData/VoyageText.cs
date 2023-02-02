using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    public class VoyageText : MonoBehaviour
    {
        public string voyageNumberText;
        [SerializeField]
        private DisplayVariable _variableToDisplay;
        public enum DisplayVariable
        {
            VoyageNumber,
            LegNumber
        }

        void Update()
        {
           /* var voyageController = FindObjectOfType<VoyageController>();
            if (_variableToDisplay == DisplayVariable.VoyageNumber)
                voyageNumberText.text = voyageController.VoyageNumber.ToString();
            else if (_variableToDisplay == DisplayVariable.LegNumber)
                voyageNumberText.text = voyageController.LegNumber.ToString();*/
        }
    }
}
