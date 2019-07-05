using System;
using System.Windows;
using System.Windows.Media;

namespace BuildingPlan.Classes
{
    /// <summary> Конструкт - некая фигура, которая будет достраивать базовую площадку или "выпиливать" из нее часть </summary>
    public class Construct
    {
        #region const

        private readonly int _percent = 100;
        private readonly Random _random = new Random();
        private readonly int _variation = 10;

        #endregion

        #region fields

        private GeometryCombineMode _combineMode;
        private ConstructType _constructType;
        private Point _coordinates;
        private int _height;
        private int _scale;
        private SizeType _sizeType;
        private int _width;
        
        #endregion

        #region public properties

        public GeometryCombineMode CombineMode => _combineMode;

        public ConstructType ConstructType => _constructType;

        public SizeType SizeType => _sizeType;

        public double Height => (double) _height * _scale / _percent;

        public Point Coordinates => _coordinates;

        public double Width => (double) _width * _scale / _percent;

        public Size Size => new Size(Width, Height);

        #endregion

        #region private properties

        private double X => (double) _height / 2;

        private double Y => (double) _width / 2;

        #endregion

        #region Constructor

        /// <summary> Конструктор конструкта </summary>
        /// <param name="startPoint"> Начало координат базовой площадки </param>
        /// <param name="height"> Высота базовой площадки </param>
        /// <param name="width"> Ширина базовой площадки </param>
        public Construct(Point startPoint, int height, int width)
        {
            _combineMode = ConstructGenerator.GetCombineMode();
            _constructType = ConstructGenerator.GetConstructType();
            _sizeType = ConstructGenerator.GetSizeType();
            _scale = SetScale();
            _height = height;
            _width = width;

            GeneratePosition(startPoint);
        }

        #endregion

        #region Helpers
        
        /// <summary> Определение пропорции конструкта </summary>
        /// <returns> Процент пропорции </returns>
        private int SetScale()
        {
            int result;
            switch (SizeType)
            {
                case SizeType.Large:
                    result = _random.Next(21, 30);
                    break;
                case SizeType.Medium:
                    result = _random.Next(11, 20);
                    break;
                case SizeType.Small:
                default:
                    result = _random.Next(1, 10);
                    break;
            }

            return result;
        }

        /// <summary> Генерация положения конструкта на базовой площадке </summary>
        /// <param name="startPoint"> Начало координат базовой площадки </param>
        private void GeneratePosition(Point startPoint)
        {
            double x = 0;
            double y = 0;
            bool condition = true;

            //Это нужно, чтобы конструкты генерировались только внутри или по краям базовой площади
            //можно было придумать что-то по-элегантней, но я спешил, поэтому такой костыль
            while (condition)
            {
                x = startPoint.X + X + CalculateCoordinate(X);
                y = startPoint.Y + Y + CalculateCoordinate(Y);

                if (x + Width > startPoint.X && y + Height > startPoint.Y && startPoint.X + _width > x && startPoint.Y + _height > y)
                    condition = false;
            }

            _coordinates = new Point(x, y);
        }

        /// <summary> Получение координат </summary>
        /// <param name="coordinate"> Координата базовой площадки </param>
        /// <returns></returns>
        private double CalculateCoordinate(double coordinate) => coordinate * GetVariant();

        /// <summary> Генерация коэффициента отклонения от центра базовой площадки </summary>
        /// <returns> Процент отклонения</returns>
        // Идея была в том, чтобы конструкт генерировался внутри базовой площадки.
        // С бОльшей вероятностью около границ, чем внутри, а не снаружи.
        // Хотелось бы сделать более красиво, но придумал только такой примитив.
        private double GetVariant() => Math.Cos((double)_variation * _random.Next(0, 100) / _percent);

        #endregion
    }
}
