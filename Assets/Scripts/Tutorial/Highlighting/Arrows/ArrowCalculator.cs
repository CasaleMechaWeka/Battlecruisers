using System;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Arrows
{
    public class ArrowCalculator : IArrowCalculator
    {
        private readonly ICamera _camera;
        private readonly IScreen _screen;

        public const float HIGHLIGHTABLE_CUTOFF_SIZE_IN_PIXELS = 800;

        public ArrowCalculator(ICamera camera, IScreen screen)
        {
            Helper.AssertIsNotNull(camera, screen);

            _camera = camera;
            _screen = screen;
        }

        public bool ShouldShowArrow(Vector2 highlightableSize)
        {
            return highlightableSize.magnitude < HIGHLIGHTABLE_CUTOFF_SIZE_IN_PIXELS;
        }

        public ArrowDirection FindArrowDirection(Vector2 highlightableCenterPosition)
        {
            Logging.Log(Tags.MASKS, $"highlightableCenterPosition: {highlightableCenterPosition}  Camera size: {_camera.PixelWidth}x{_camera.PixelHeight}");

            // Highlightable is in middle of screen
            if (highlightableCenterPosition.x == _camera.PixelWidth / 2)
            {
                return ArrowDirection.North;
            }

            // Highlightable is in top third of screen, avoid hiding behind explanation text
            if (highlightableCenterPosition.y > (_screen.Height / 3))
            {
                return ArrowDirection.North;
            }

            // West
            if (highlightableCenterPosition.x < _camera.PixelWidth / 2)
            {
                // South
                if (highlightableCenterPosition.y < _camera.PixelHeight / 2)
                {
                    return ArrowDirection.SouthWest;
                }
                // North
                else
                {
                    return ArrowDirection.NorthWest;
                }
            }
            // East
            else
            {
                // South
                if (highlightableCenterPosition.y < _camera.PixelHeight / 2)
                {
                    return ArrowDirection.SouthEast;
                }
                // North
                else
                {
                    return ArrowDirection.NorthEast;
                }
            }
        }

        public Vector2 FindArrowHeadPosition(HighlightArgs args, ArrowDirection direction)
        {
            Assert.IsNotNull(args);

            switch (direction)
            {
                case ArrowDirection.North:
                    return
                        new Vector2(
                            args.CenterPosition.x,
                            args.CenterPosition.y - args.Size.y / 2);

                case ArrowDirection.NorthEast:
                    return
                        new Vector2(
                            args.CenterPosition.x - args.Size.x / 2,
                            args.CenterPosition.y - args.Size.y / 2);

                case ArrowDirection.NorthWest:
                    return
                        new Vector2(
                            args.CenterPosition.x + args.Size.x / 2,
                            args.CenterPosition.y - args.Size.y / 2);

                case ArrowDirection.SouthEast:
                    return
                        new Vector2(
                            args.CenterPosition.x - args.Size.x / 2,
                            args.CenterPosition.y + args.Size.y / 2);

                case ArrowDirection.SouthWest:
                    return
                        new Vector2(
                            args.CenterPosition.x + args.Size.x / 2,
                            args.CenterPosition.y + args.Size.y / 2);

                default:
                    throw new ArgumentException($"What the blazes is this enum type?!?: {direction}");
            }
        }

        public Vector2 FindArrowDirectionVector(Vector2 arrowHead, Vector2 highlightableCenterPosition)
        {
            return highlightableCenterPosition - arrowHead;
        }
    }
}