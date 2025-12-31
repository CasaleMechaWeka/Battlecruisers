using BattleCruisers.Buildables;
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
            new GradientColorKey(new Color(.129f, .396f, .761f), 0f),
            new GradientColorKey(new Color(.129f, .396f, .761f), 1f),
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
            new GradientColorKey(new Color(.761f, .231f, .129f), 0f),
            new GradientColorKey(new Color(.761f, .231f, .129f), 1f),
        },
        alphaKeys = new GradientAlphaKey[2]
        {
            new GradientAlphaKey(.196f, 0f),
            new GradientAlphaKey(0f, 1f)
        }
    };

    private ITarget target;
    private TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        target = GetComponentInParent<ITarget>();
        Assert.IsNotNull(trailRenderer, "TrailRenderer cannot be found");
        Assert.IsNotNull(target, "Target cannot be found");

        if (target.Faction == Faction.Blues)
            trailRenderer.colorGradient = playerColors;
        else
            trailRenderer.colorGradient = enemyColors;
    }
}
