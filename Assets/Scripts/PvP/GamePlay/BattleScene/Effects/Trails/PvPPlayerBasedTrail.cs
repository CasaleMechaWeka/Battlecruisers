using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(TrailRenderer))]
public class PvPPlayerBasedTrail : MonoBehaviour
{
    [SerializeField]
    private Gradient playerColors = new Gradient
    {
        colorKeys = new GradientColorKey[2]
        {
            new GradientColorKey(Color.blue, 0f),
            new GradientColorKey(Color.blue, 1f),
        },
        alphaKeys = new GradientAlphaKey[2]
        {
            new GradientAlphaKey(.196f, 0f),
            new GradientAlphaKey(0f, 1f)
        }
    };

    [SerializeField]
    private Gradient enemyColors = new Gradient
    {
        colorKeys = new GradientColorKey[2]
        {
            new GradientColorKey(Color.red, 0f),
            new GradientColorKey(Color.red, 1f),
        },
        alphaKeys = new GradientAlphaKey[2]
        {
            new GradientAlphaKey(.196f, 0f),
            new GradientAlphaKey(0f, 1f)
        }
    };

    private IPvPTarget target;
    private TrailRenderer trailRenderer;

    void Start()
    {
        Assert.IsNotNull(trailRenderer = GetComponent<TrailRenderer>(), "TrailRenderer cannot be found");
        Assert.IsNotNull(target = GetComponentInParent<IPvPTarget>(), "Target cannot be found");

        if (target.Faction == PvPFaction.Blues)
            trailRenderer.colorGradient = playerColors;
        else
            trailRenderer.colorGradient = enemyColors;
    }
}
