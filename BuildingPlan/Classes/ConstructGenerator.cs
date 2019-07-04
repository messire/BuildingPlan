using System;
using System.Windows.Media;

namespace BuildingPlan.Classes
{
    public static class ConstructGenerator
    {
        #region fields

        private static Random _random;

        #endregion

        #region Constructor

        static ConstructGenerator() => _random = new Random();

        #endregion

        #region Methods

        public static GeometryCombineMode GetCombineMode() => GenerateCombineMode();

        public static ConstructType GetConstructType() => GenerateConstructType();

        public static SizeType GetSizeType() => GenerateSizeType();

        #endregion

        #region Generators

        private static GeometryCombineMode GenerateCombineMode() => GetRandom<CombineMode>() == 0 ? GeometryCombineMode.Union : GeometryCombineMode.Exclude;

        private static ConstructType GenerateConstructType()
        {
            var result = GetRandom<ConstructType>();
            return result != 0 ? result : GetRandom<ConstructType>();
        }

        private static SizeType GenerateSizeType() => GetRandom<SizeType>();

        #endregion

        #region Helper

        private static T GetRandom<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_random.Next(v.Length));
        }

        #endregion
    }
}
