using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline
{
    public class PvPBuildableOutlineController : PvPPrefab
    {
        public AudioClip placementSound;
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        public Vector3 PuzzleRootPoint;
        public Action BuildableCreated;


        private void Awake()
        {
            Transform puzzleRootPoint = transform.FindNamedComponent<Transform>("PuzzleRootPoint");
            PuzzleRootPoint = puzzleRootPoint.position;
            BuildableCreated += OnBuildableCreated;
            Invoke("OnBuildableCreated", 2f); // to avoid network issue
        }

        private void OnBuildableCreated()
        {
            BuildableCreated -= OnBuildableCreated;
            Destroy(gameObject);
        }


        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);
        }


    }
}

