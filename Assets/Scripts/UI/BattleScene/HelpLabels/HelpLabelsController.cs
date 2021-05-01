using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public class HelpLabelsController : MonoBehaviour, IHelpLabels
    {
        public Panel cruiserHealth;
        public IPanel CruiserHealth => cruiserHealth;

        public Panel leftBottom;
        public IPanel LeftBottom => leftBottom;

        public Panel buildingCategories;
        public IPanel BuildingCategories => buildingCategories;

        public Panel rightBottom;
        public IPanel RightBottom => rightBottom;

        public Panel informator;
        public IPanel Informator => informator;

        public Panel buildMenu;
        public IPanel BuildMenu => buildMenu;

        public void Initialise()
        {
            Helper.AssertIsNotNull(cruiserHealth, leftBottom, buildingCategories, rightBottom, informator, buildMenu);
        }
    }
}