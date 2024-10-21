using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(TrailRenderer))]
public class PlayerBasedTrailColor : MonoBehaviour
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

    private ITarget target;
    private TrailRenderer trailRenderer;

    void Start()
    {
        Assert.IsNotNull(trailRenderer = GetComponent<TrailRenderer>(), "TrailRenderer cannot be found");
        Assert.IsNotNull(target = GetComponentInParent<ITarget>(), "Target cannot be found");

        if (target.Faction == Faction.Blues)
            trailRenderer.colorGradient = playerColors;
        else
            trailRenderer.colorGradient = enemyColors;
    }
}
