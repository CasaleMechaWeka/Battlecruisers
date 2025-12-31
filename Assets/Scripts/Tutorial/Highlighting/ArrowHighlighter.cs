using System;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class ArrowHighlighter : MonoBehaviour, ICoreHighlighter
    {
        public enum ArrowDirection
        {
            NorthWest,
            North,  // For when highlightable is exactly in the center of the screen
            NorthEast,
            SouthEast,
            SouthWest
        }

        private ICamera _camera;

        public const float HIGHLIGHTABLE_CUTOFF_SIZE_IN_PIXELS = 800;


        public void Initialise(ICamera camera)
        {
            _camera = camera;
        }

        public void Highlight(HighlightArgs args)
        {
            if (args.Size.magnitude > HIGHLIGHTABLE_CUTOFF_SIZE_IN_PIXELS)
            {
                return;
            }

            ArrowDirection arrowDirection = FindArrowDirection(args.CenterPosition);
            Vector2 arrowHeadPosition = FindArrowHeadPosition(args, arrowDirection);
            Vector2 upDirection = args.CenterPosition - arrowHeadPosition;

            Logging.Log(Tags.MASKS, $"arrowDirection: {arrowDirection}  vector: {upDirection}  Head position: {arrowHeadPosition}");

            gameObject.transform.position = arrowHeadPosition;
            gameObject.transform.up = upDirection;

            gameObject.SetActive(true);
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }

        public ArrowDirection FindArrowDirection(Vector2 center)
        {
            Logging.Log(Tags.MASKS,
                $"highlightableCenterPosition: {center}  Camera size: {_camera.PixelWidth}x{_camera.PixelHeight}");

            float halfW = _camera.PixelWidth * 0.5f;
            float halfH = _camera.PixelHeight * 0.5f;
            float topBand = _camera.PixelHeight * 0.667f;

            // Top-band (avoid explainer text) or dead-centre → point straight up
            if (center.y > topBand || Mathf.Approximately(center.x, halfW))
                return ArrowDirection.North;

            bool left = center.x < halfW;
            bool south = center.y < halfH;

            return (left, south) switch
            {
                (true, true) => ArrowDirection.SouthWest,
                (true, false) => ArrowDirection.NorthWest,
                (false, true) => ArrowDirection.SouthEast,
                _ => ArrowDirection.NorthEast
            };
        }

        public Vector2 FindArrowHeadPosition(HighlightArgs args, ArrowDirection direction)
        {
            Assert.IsNotNull(args);

            return direction switch
            {
                ArrowDirection.North => new Vector2(
                                            args.CenterPosition.x,
                                            args.CenterPosition.y - args.Size.y / 2),
                ArrowDirection.NorthEast => new Vector2(
                                            args.CenterPosition.x - args.Size.x / 2,
                                            args.CenterPosition.y - args.Size.y / 2),
                ArrowDirection.NorthWest => new Vector2(
                                            args.CenterPosition.x + args.Size.x / 2,
                                            args.CenterPosition.y - args.Size.y / 2),
                ArrowDirection.SouthEast => new Vector2(
                                            args.CenterPosition.x - args.Size.x / 2,
                                            args.CenterPosition.y + args.Size.y / 2),
                ArrowDirection.SouthWest => new Vector2(
                                            args.CenterPosition.x + args.Size.x / 2,
                                            args.CenterPosition.y + args.Size.y / 2),
                _ => throw new ArgumentException($"What the blazes is this enum type?!?: {direction}"),
            };
        }
    }
}