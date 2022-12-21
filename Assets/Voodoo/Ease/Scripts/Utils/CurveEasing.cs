using System;
using UnityEngine;

namespace Voodoo.Utils.Ease
{
    public static class CurveEasing
    {
        #region Public Functions

        public static AnimationCurve GenerateAnimationCurve(Ease easing)
        {
            switch (easing)
            {
                case Ease.EaseInQuad:
                    return CreateInCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, .3333f)));
                case Ease.EaseOutQuad:
                    return CreateOutCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, .3333f)));
                case Ease.EaseInOutQuad:
                    return CreateInOutCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, .3333f)));

                case Ease.EaseInCubic:
                    return CreateInCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, 0)));
                case Ease.EaseOutCubic:
                    return CreateOutCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, 0)));
                case Ease.EaseInOutCubic:
                    return CreateInOutCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, 0)));
                case Ease.EaseInQuart:
                    return CreateInCurve(GetInQuarticKeyframes());
                case Ease.EaseOutQuart:
                    return CreateOutCurve(GetInQuarticKeyframes());
                case Ease.EaseInOutQuart:
                    return CreateInOutCurve(GetInQuarticKeyframes());

                case Ease.EaseInQuint:
                    return CreateInCurve(GetInQuinticKeyframes());
                case Ease.EaseOutQuint:
                    return CreateOutCurve(GetInQuinticKeyframes());
                case Ease.EaseInOutQuint:
                    return CreateInOutCurve(GetInQuinticKeyframes());

                case Ease.EaseInSine:
                    return CreateInCurve(GetBezierKeyFrames(P(0.3619f, 0), P(0.6739f, 0.4877f)));
                case Ease.EaseOutSine:
                    return CreateOutCurve(GetBezierKeyFrames(P(0.3619f, 0), P(0.6739f, 0.4877f)));
                case Ease.EaseInOutSine:
                    return CreateInOutCurve(GetBezierKeyFrames(P(0.3619f, 0), P(0.6739f, 0.4877f)));

                case Ease.EaseInExpo:
                    return CreateInCurve(GetInExponentialKeyframes());
                case Ease.EaseOutExpo:
                    return CreateOutCurve(GetInExponentialKeyframes());
                case Ease.EaseInOutExpo:
                    return CreateInOutCurve(GetInExponentialKeyframes());

                case Ease.EaseInCirc:
                    return CreateInCurve(GetBezierKeyFrames(P(0.5523f, 0), P(.9999f, 0.4477f)));
                case Ease.EaseOutCirc:
                    return CreateOutCurve(GetBezierKeyFrames(P(0.5523f, 0), P(.9999f, 0.4477f)));
                case Ease.EaseInOutCirc:
                    return CreateInOutCurve(GetBezierKeyFrames(P(0.5523f, 0), P(.9999f, 0.4477f)));

                case Ease.Linear:
                    return CreateInCurve(GetBezierKeyFrames(P(0, 0), P(0, 0)));

                case Ease.Spring:
                    return CreateInCurve(GetSpringKeyframes());

                case Ease.EaseInBounce:
                    return CreateInCurve(GetInBounceKeyframes());
                case Ease.EaseOutBounce:
                    return CreateOutCurve(GetInBounceKeyframes());
                case Ease.EaseInOutBounce:
                    return CreateInOutCurve(GetInBounceKeyframes());

                case Ease.EaseInBack:
                    return CreateInCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, -.567193f)));
                case Ease.EaseOutBack:
                    return CreateOutCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, -.567193f)));
                case Ease.EaseInOutBack:
                    return CreateInOutCurve(GetBezierKeyFrames(P(.3333f, 0), P(.6666f, -.86497f)));

                case Ease.EaseInElastic:
                    return CreateInCurve(GetInElasticKeyframes());
                case Ease.EaseOutElastic:
                    return CreateOutCurve(GetInElasticKeyframes());
                case Ease.EaseInOutElastic:
                    return CreateInOutCurve(GetInElasticKeyframes());

                default:
                    Debug.LogWarning("Can't find this curve. Returning linear.");
                    return CreateInCurve(GetBezierKeyFrames(P(0, 0), P(0, 0)));
            }
        }

        #endregion

        #region AnimationCurves: In/Out/InOut

        private static AnimationCurve CreateInCurve(Keyframe[] inKeyframes)
        {
            return new AnimationCurve(inKeyframes);
        }

        private static AnimationCurve CreateOutCurve(Keyframe[] inKeyframes)
        {
            var outKeyframes = new Keyframe[inKeyframes.Length];
            inKeyframes.CopyTo(outKeyframes, 0);
            RotateKeyframes(outKeyframes);
            return new AnimationCurve(outKeyframes);
        }

        private static AnimationCurve CreateInOutCurve(Keyframe[] inKeyframes)
        {
            var outKeyframes = new Keyframe[inKeyframes.Length];
            inKeyframes.CopyTo(outKeyframes, 0);
            RotateKeyframes(outKeyframes);
            var inOutKeyframes = new Keyframe[inKeyframes.Length * 2];
            MoveAndScaleKeyframes(inKeyframes, Vector2.zero, .5f);
            MoveAndScaleKeyframes(outKeyframes, Vector2.one * .5f, .5f);
            inKeyframes.CopyTo(inOutKeyframes, 0);
            outKeyframes.CopyTo(inOutKeyframes, inKeyframes.Length);
            return new AnimationCurve(inOutKeyframes);
        }

        #endregion

        #region Keyframes

        private static Keyframe[] GetInQuarticKeyframes()
        {
            var keyframes = new Keyframe[3];
            for (var i = 0; i < keyframes.Length; i++)
                keyframes[i].weightedMode = WeightedMode.Both;
            keyframes[0] = new Keyframe(0, 0, 0, 0, 0, 0.35f);
            keyframes[1] = new Keyframe(0.4023629f, 0.02591691f, 0.2671903f, 0.2671903f, 0.2314838f, 0.3768896f);
            keyframes[2] = new Keyframe(1, 1, 4.165794f, 4.165794f, 0.2858976f, 0);
            return keyframes;
        }

        private static Keyframe[] GetInQuinticKeyframes()
        {
            var keyframes = new Keyframe[3];
            for (var i = 0; i < keyframes.Length; i++)
                keyframes[i].weightedMode = WeightedMode.Both;
            keyframes[0] = new Keyframe(0, 0, 0, 0, 0, 0.5808958f);
            keyframes[1] = new Keyframe(0.5020252f, 0.03216228f, 0.3849036f, 0.3389311f, 0.1664644f, 0.4185665f);
            keyframes[2] = new Keyframe(1, 1, 4.973197f, 0, 0.2820365f, 0);
            return keyframes;
        }

        private static Keyframe[] GetInExponentialKeyframes()
        {
            var keyframes = new Keyframe[3];
            for (var i = 0; i < keyframes.Length; i++)
                keyframes[i].weightedMode = WeightedMode.Both;
            keyframes[0] = new Keyframe(0, 0, 0, 0.01230408f, 0f, 0.6417668f);
            keyframes[1] = new Keyframe(0.5990406f, 0.06219158f, 0.424782f, 0.424782f, 0.1918493f, 0.5062984f);
            keyframes[2] = new Keyframe(1, 1, 6.769706f, 0, 0.2266997f, 0);
            return keyframes;
        }

        private static Keyframe[] GetInElasticKeyframes()
        {
            var keyframes = new Keyframe[7];
            for (var i = 0; i < keyframes.Length; i++)
                keyframes[i].weightedMode = WeightedMode.Both;
            keyframes[0] = new Keyframe(0, 0, 0, 0, 0.3333333f, 0.0943521f);
            keyframes[1] = new Keyframe(0.2375961f, -0.004900184f, -0.09512027f, -0.09512027f, 0.4763626f, 0.4502789f);
            keyframes[2] = new Keyframe(0.4253662f, 0.01661617f, -0.02957249f, -0.02957249f, 0.3228389f, 0.3695113f);
            keyframes[3] = new Keyframe(0.5672264f, -0.04623402f, -0.01410099f, -0.01410099f, 0.3134277f, 0.4208294f);
            keyframes[4] = new Keyframe(0.7112442f, 0.1319214f, 0.2673996f, 0.2673996f, 0.2878268f, 0.4619742f);
            keyframes[5] = new Keyframe(0.8663773f, -0.3729638f, 0.01841709f, 0.01841709f, 0.3062344f, 0.4383868f);
            keyframes[6] = new Keyframe(1, 1, 5.844434f, 0, 0.2356896f, 0);
            return keyframes;
        }

        private static Keyframe[] GetInBounceKeyframes()
        {
            const float x = 1f / 11;
            return GetBezierKeyFrames(
                P(.5f * x * .3333f, 1 / 64f * .6666f),
                P(.5f * x - .5f * x * .3333f, 1 / 64f), P(.5f * x, 1 / 64f), P(.5f * x + .5f * x * .3333f, 1 / 64f),
                P(1 * x - .5f * x * .3333f, 1 / 64f * .6666f), P(1 * x, 0), P(1 * x + 1 * x * .3333f, 1 / 16f * .6666f),
                P(2 * x - 1 * x * .3333f, 1 / 16f), P(2 * x, 1 / 16f), P(2 * x + 1 * x * .3333f, 1 / 16f),
                P(3 * x - 1 * x * .3333f, 1 / 16f * .6666f), P(3 * x, 0), P(3 * x + 2 * x * .3333f, 1 / 4f * .6666f),
                P(5 * x - 2 * x * .3333f, 1 / 4f), P(5 * x, 1 / 4f), P(5 * x + 2 * x * .3333f, 1 / 4f),
                P(7 * x - 2 * x * .3333f, 1 / 4f * .6666f), P(7 * x, 0), P(7 * x + 4 * x * .3333f, 1 * .6666f),
                P(11 * x - 4 * x * .3333f, 1));
        }

        private static Keyframe[] GetSpringKeyframes()
        {
            var keyframes = new Keyframe[5];
            for (var i = 0; i < keyframes.Length; i++)
                keyframes[i].weightedMode = WeightedMode.Both;
            keyframes[0] = new Keyframe(0, 0, 0, 4.198298f, 0, 0.1125816f);
            keyframes[1] = new Keyframe(0.580668f, 1.085675f, -0.01432603f, -0.01432603f, 0.3377516f, 0.3333333f);
            keyframes[2] = new Keyframe(0.7965281f, 0.9724002f, 0.01279934f, 0.01279934f, 0.3085437f, 0.3732702f);
            keyframes[3] = new Keyframe(0.9290999f, 1.008701f, 0.01008692f, 0.01008692f, 0.3721488f, 0.3323579f);
            keyframes[4] = new Keyframe(1, 1, -0.016674f, 0, 0.1967346f, 0);
            return keyframes;
        }

        private static Keyframe[] GetBezierKeyFrames(params Vector2[] points)
        {
            if (points.Length % 3 != 2 || points.Length < 2)
                throw new ArgumentException("Need 3k + 2 control points, k >= 0. Thanks. :)");

            var internalPointCoint = points.Length / 3;
            var keyframes = new Keyframe[internalPointCoint + 2];

            for (var i = 0; i < keyframes.Length; i++)
                keyframes[i].weightedMode = WeightedMode.Both;

            if (internalPointCoint == 0) //simple case
                UpdateSegment(ref keyframes[0], ref keyframes[1], Vector2.zero, points[0], points[1], Vector2.one);
            else
            {
                //first segment
                UpdateSegment(ref keyframes[0], ref keyframes[1], Vector2.zero, points[0], points[1], points[2]);
                //internal segments
                for (var i = 0; i < internalPointCoint - 1; i++)
                    UpdateSegment(ref keyframes[i + 1], ref keyframes[i + 2], points[i * 3 + 2], points[i * 3 + 3],
                        points[i * 3 + 4], points[i * 3 + 5]);
                //last segment
                UpdateSegment(ref keyframes[internalPointCoint], ref keyframes[internalPointCoint + 1],
                    points[points.Length - 3], points[points.Length - 2], points[points.Length - 1], Vector2.one);
            }

            return keyframes;
        }

        #endregion

        #region Helpers

        private static void MoveAndScaleKeyframes(Keyframe[] keyframes, Vector2 move, float scale)
        {
            for (var i = 0; i < keyframes.Length; i++)
            {
                keyframes[i].time = move.x + keyframes[i].time * scale;
                keyframes[i].value = move.y + keyframes[i].value * scale;
            }
        }

        private static void RotateKeyframes(Keyframe[] keyframes)
        {
            var newKeyframes = new Keyframe[keyframes.Length];
            for (var i = 0; i < keyframes.Length; i++)
            {
                var kf = keyframes[keyframes.Length - i - 1];
                newKeyframes[i].time = 1 - kf.time;
                newKeyframes[i].value = 1 - kf.value;
                newKeyframes[i].inTangent = kf.outTangent;
                newKeyframes[i].outTangent = kf.inTangent;
                newKeyframes[i].inWeight = kf.outWeight;
                newKeyframes[i].outWeight = kf.inWeight;
                newKeyframes[i].weightedMode = WeightedMode.Both;
            }

            for (var i = 0; i < keyframes.Length; i++)
                keyframes[i] = newKeyframes[i];
        }

        private static void UpdateSegment(ref Keyframe left, ref Keyframe right, Vector2 p0, Vector2 p1, Vector2 p2,
                                          Vector2 p3)
        {
            left.time = p0.x;
            left.value = p0.y;
            left.outTangent = GetTangent(p1 - p0);
            left.outWeight = p3.x == p0.x ? 1 : (p1.x - p0.x) / (p3.x - p0.x);

            right.time = p3.x;
            right.value = p3.y;
            right.inTangent = GetTangent(p2 - p3);
            right.inWeight = p3.x == p0.x ? 1 : (p3.x - p2.x) / (p3.x - p0.x);
        }

        private static float GetTangent(float x, float y)
        {
            if (x != 0) return y / x;
            if (y > 0) return 999999999; //Note: float.MaxValue caused some calculation errors later on
            if (y < 0) return 999999999;
            return 0;
        }

        private static float GetTangent(Vector2 v) => GetTangent(v.x, v.y);

        private static Vector2 P(float x, float y) => new Vector2(x, y);

        #endregion
    }
}
