using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public class HelpLabelsController : MonoBehaviour
    {
        public Panel cruiserHealth;
        public Panel CruiserHealth => cruiserHealth;

        public Panel leftBottom;
        public Panel LeftBottom => leftBottom;

        public Panel buildingCategories;
        public Panel BuildingCategories => buildingCategories;

        public Panel rightBottom;
        public Panel RightBottom => rightBottom;

        public Panel informator;
        public Panel Informator => informator;

        public Panel buildMenu;
        public Panel BuildMenu => buildMenu;

        public void Initialise()
        {
            Helper.AssertIsNotNull(cruiserHealth, leftBottom, buildingCategories, rightBottom, informator, buildMenu);
        }
    }
}