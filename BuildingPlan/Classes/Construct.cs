using System;
using System.Windows;
using System.Windows.Media;

namespace BuildingPlan.Classes
{
    public class Construct
    {
        #region const

        private readonly int _percent = 100;

        #endregion

        #region fields

        private GeometryCombineMode _combineMode;
        private ConstructType _constructType;
        private Point _coordinates;
        private int _height;
        private readonly Random _random = new Random();
        private int _scale;
        public SizeType _sizeType;
        private int _variation;
        private int _width;
        
        #endregion

        #region public properties

        public GeometryCombineMode CombineMode => _combineMode;

        public ConstructType ConstructType => _constructType;

        public SizeType SizeType => _sizeType;

        public double Height => (double) _height * _scale / _percent;

        public Point Coordinates => _coordinates;

        public double Width => (double) _width * _scale / _percent;

        #endregion

        #region private properties

        private double X => (double) _height / 2;

        private double Y => (double) _width / 2;

        #endregion

        #region Constructor

        public Construct(Point startPoint, int height, int width)
        {
            _variation = 10;
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

        private void GeneratePosition(Point startPoint) => _coordinates =
            new Point(startPoint.X + X + CalculateCoordinate(X), startPoint.Y + Y + CalculateCoordinate(Y));

        private double CalculateCoordinate(double coordinate) => coordinate * GetVariant();

        private double GetVariant() => Math.Cos((double) _variation * _random.Next(0, 100) / _percent);

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

        #endregion
    }
}
