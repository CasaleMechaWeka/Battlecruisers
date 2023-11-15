using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public static class PvPHelper
    {
        private const float Y_DEGREES_MIRRORED = 180;
        private const float Y_DEGREES_NOT_MIRRORED = 0;

        public static PvPFaction GetOppositeFaction(PvPFaction faction)
        {
            return faction == PvPFaction.Blues ? PvPFaction.Reds : PvPFaction.Blues;
        }

        public static List<IPvPPatrolPoint> ConvertVectorsToPatrolPoints(IList<Vector2> positions)
        {
            return positions
                .Select(position => new PvPPatrolPoint(position) as IPvPPatrolPoint)
                .ToList();
        }

        [Conditional("ENABLE_LOGS")]
        public static void AssertIsNotNull(params object[] objs)
        {
            foreach (object obj in objs)
            {
                // Unity has custom null behaviour, so handle that
                if (obj is UnityEngine.Object unityObject)
                {
                    Assert.IsTrue(unityObject);
                }
                else
                {
                    Assert.IsNotNull(obj);
                }
            }
        }

        public static int Half(int number, bool roundUp)
        {
            int half = number / 2;

            if (roundUp)
            {
                half += number % 2;
            }

            return half;
        }

        public static Quaternion MirrorAccrossYAxis(Quaternion rotation)
        {
            float yDegrees;

            if (rotation.eulerAngles.y == Y_DEGREES_MIRRORED)
            {
                yDegrees = Y_DEGREES_NOT_MIRRORED;
            }
            else if (rotation.eulerAngles.y == Y_DEGREES_NOT_MIRRORED)
            {
                yDegrees = Y_DEGREES_MIRRORED;
            }
            else
            {
                throw new ArgumentException("Expect y angle to be " + Y_DEGREES_MIRRORED + " or " + Y_DEGREES_NOT_MIRRORED + " not " + rotation.eulerAngles.y);
            }

            Vector3 mirroredEulerAngles = new Vector3(rotation.eulerAngles.x, yDegrees, rotation.eulerAngles.z);
            rotation.eulerAngles = mirroredEulerAngles;

            return rotation;
        }

        public static bool IsFacingTarget(Vector2 target, Vector2 source, bool isSourceMirrored)
        {
            return
                isSourceMirrored && target.x < source.x
                || !isSourceMirrored && target.x > source.x;
        }
    }
}
