using System;
using System.Windows.Media;
using static System.Windows.Media.GeometryCombineMode;
using static BuildingPlan.ConstructType;

namespace BuildingPlan.Classes
{
    /// <summary> Генератор конструктов </summary>
    public static class ConstructGenerator
    {
        #region fields

        private static Random _random;

        #endregion

        #region Constructor

        static ConstructGenerator() => _random = new Random();

        #endregion

        #region Methods

        /// <summary> Получить метод комбинирования с базовой площадкой </summary>
        /// <returns> Union, Exclude</returns>
        public static GeometryCombineMode GetCombineMode() => GenerateCombineMode();

        /// <summary> Получить тип конструкта </summary>
        /// <returns> Rectangle, Circle </returns>
        public static ConstructType GetConstructType() => GenerateConstructType();

        /// <summary> Получить тип размера </summary>
        /// <returns> Large, Medium Small </returns>
        public static SizeType GetSizeType() => GenerateSizeType();

        #endregion

        #region Generators

        /// <summary> Сгенерировать метод комбинирования с базовой площадкой </summary>
        /// <returns></returns>
        private static GeometryCombineMode GenerateCombineMode() => GetRandom<CombineMode>() == 0 ? Union : Exclude;

        /// <summary> Сгенерировать тип конструкта </summary>
        /// <returns></returns>
        private static ConstructType GenerateConstructType() => _random.Next(0, 10) % 4 != 0 ? Rectangle : Circle;

        /// <summary> Сгенерировать тип размера </summary>
        /// <returns></returns>
        private static SizeType GenerateSizeType() => GetRandom<SizeType>();

        #endregion

        #region Helper

        /// <summary> Получить рандом из типа </summary>
        /// <typeparam name="T"> Тип </typeparam>
        /// <returns></returns>
        private static T GetRandom<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_random.Next(v.Length));
        }

        #endregion
    }
}
