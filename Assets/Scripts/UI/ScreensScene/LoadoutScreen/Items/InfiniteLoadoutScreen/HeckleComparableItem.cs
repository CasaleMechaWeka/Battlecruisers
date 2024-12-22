using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HeckleComparableItem : IComparableItem
    {
        public Sprite Sprite { get; }
        public string Description { get; }
        public string Name { get; }
        public HeckleComparableItem(Sprite sprite, string description, string name)
        {
            Sprite = sprite;
            Name = name;
            Description = description;
        }
    }
}
