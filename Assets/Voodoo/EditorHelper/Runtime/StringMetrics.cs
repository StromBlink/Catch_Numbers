using System;
using System.Linq;

namespace Voodoo.Utils
{
    public static class StringMetrics
    {
        public static double JaccardDistance(this string source, string target)
        {
            return 1 - source.JaccardIndex(target);
        }

        public static double JaccardIndex(this string source, string target)
        {
            return Convert.ToDouble(source.Intersect(target).Count()) /
                   Convert.ToDouble(source.Union(target).Count());
        }

        public static double LevenshteinDistance(this string source, string target)
        {
            source = source?.Trim();
            target = target?.Trim();

            if (source == null || target == null) return 0;
            if (source.Length == 0 || target.Length == 0) return 0;
            if (source == target) return 0;

            var sourceWordCount = source.Length;
            var targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            var distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (var i = 0; i <= sourceWordCount; distance[i, 0] = i++)
            {
            }

            for (var j = 0; j <= targetWordCount; distance[0, j] = j++)
            {
            }

            for (var i = 1; i <= sourceWordCount; i++)
            for (var j = 1; j <= targetWordCount; j++)
            {
                // Step 3
                var cost = target[j - 1] == source[i - 1] ? 0 : 1;

                // Step 4
                distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                    distance[i - 1, j - 1] + cost);
            }

            return distance[sourceWordCount, targetWordCount];
        }

        public static double NormalizedLevenshteinDistance(this string source, string target)
        {
            var unnormalizedLevenshteinDistance = source.LevenshteinDistance(target);

            return unnormalizedLevenshteinDistance / (double)Math.Max(source.Length, target.Length);
        }

        public static double NormalizedLevenshteinProximity(this string source, string target)
        {
            var unnormalizedLevenshteinDistance = source.LevenshteinDistance(target);

            return 1D - unnormalizedLevenshteinDistance / (double)Math.Max(source.Length, target.Length);
        }

        public static double OverlapCoefficient(this string source, string target)
        {
            return Convert.ToDouble(source.Intersect(target).Count()) /
                   Convert.ToDouble(Math.Min(source.Length, target.Length));
        }

        /* The Winkler modification will not be applied unless the 
         * percent match was at or above the mWeightThreshold percent 
         * without the modification. 
         * Winkler's paper used a default value of 0.7
         */
        private static readonly double mWeightThreshold = 0.7;

        /* Size of the prefix to be concidered by the Winkler modification. 
         * Winkler's paper used a default value of 4
         */
        private static readonly int mNumChars = 4;

        /// <summary>
        /// Returns the Jaro-Winkler distance between the specified  
        /// strings. The distance is symmetric and will fall in the 
        /// range 0 (perfect match) to 1 (no match). 
        /// </summary>
        /// <param name="aString1">First String</param>
        /// <param name="aString2">Second String</param>
        /// <returns></returns>
        public static double JaroWinklerDistance(this string aString1, string aString2)
        {
            return 1.0 - JaroWinklerProximity(aString1, aString2);
        }

        /// <summary>
        /// Returns the Jaro-Winkler distance between the specified  
        /// strings. The distance is symmetric and will fall in the 
        /// range 0 (no match) to 1 (perfect match). 
        /// </summary>
        /// <param name="aString1">First String</param>
        /// <param name="aString2">Second String</param>
        /// <returns></returns>
        public static double JaroWinklerProximity(this string aString1, string aString2)
        {
            int lLen1 = aString1.Length;
            int lLen2 = aString2.Length;
            if (lLen1 == 0)
                return lLen2 == 0 ? 1.0 : 0.0;

            int lSearchRange = Math.Max(0, Math.Max(lLen1, lLen2) / 2 - 1);

            // default initialized to false
            bool[] lMatched1 = new bool[lLen1];
            bool[] lMatched2 = new bool[lLen2];

            int lNumCommon = 0;
            for (int i = 0; i < lLen1; ++i)
            {
                int lStart = Math.Max(0, i - lSearchRange);
                int lEnd = Math.Min(i + lSearchRange + 1, lLen2);
                for (int j = lStart; j < lEnd; ++j)
                {
                    if (lMatched2[j]) continue;
                    if (aString1[i] != aString2[j])
                        continue;
                    lMatched1[i] = true;
                    lMatched2[j] = true;
                    ++lNumCommon;
                    break;
                }
            }
            if (lNumCommon == 0) return 0.0;

            int lNumHalfTransposed = 0;
            int k = 0;
            for (int i = 0; i < lLen1; ++i)
            {
                if (!lMatched1[i]) continue;
                while (!lMatched2[k]) ++k;
                if (aString1[i] != aString2[k])
                    ++lNumHalfTransposed;
                ++k;
            }
            // System.Diagnostics.Debug.WriteLine("numHalfTransposed=" + numHalfTransposed);
            int lNumTransposed = lNumHalfTransposed / 2;

            // System.Diagnostics.Debug.WriteLine("numCommon=" + numCommon + " numTransposed=" + numTransposed);
            double lNumCommonD = lNumCommon;
            double lWeight = (lNumCommonD / lLen1
                             + lNumCommonD / lLen2
                             + (lNumCommon - lNumTransposed) / lNumCommonD) / 3.0;

            if (lWeight <= mWeightThreshold) return lWeight;
            int lMax = Math.Min(mNumChars, Math.Min(aString1.Length, aString2.Length));
            int lPos = 0;
            while (lPos < lMax && aString1[lPos] == aString2[lPos])
                ++lPos;
            if (lPos == 0) return lWeight;
            return lWeight + 0.1 * lPos * (1.0 - lWeight);

        }
    }
}