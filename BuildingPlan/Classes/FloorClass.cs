using System;
using System.Collections;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace BuildingPlan.Classes
{
    /// <summary> Класс описывающий этаж. Набор геометрических фигур. </summary>
    // Выбран массив, т.к.:
    // -- он быстрее, в этом месте память можно сэкономить, пригодится в будущем
    // -- более явный контроль, в каждый момент времени понятно что сейчас обрабатывается
    // -- нет необходимости в динамике, параметры этажа задаются на входе

    public class FloorClass: IEnumerable
    {
        #region const

        private int _empty = 0;

        #endregion

        #region fields

        private Construct[] _elements;
        private int _height;
        private Point _startPoint;
        private int _width;
        private int _exclude;
        private int _union;
        
        #endregion

        #region properties

        public int Height => _height;

        public int Occupancy => _exclude + _union;

        public int Size => _elements.Length;

        public Point StartPoint => _startPoint;

        public int Width => _width;

        public int ExcludeCount => _exclude;

        public int UnionCount => _union;

        #endregion

        /// <summary> Конструктор этажа. </summary>
        /// <param name="startPoint"> Координаты базовой фигуры </param>
        /// <param name="width"> Ширина этажа </param>
        /// <param name="height"> Высота этажа </param>
        /// <param name="complexity"> Сложность </param>
        public FloorClass(Point startPoint, int height, int width, int complexity)
        {
            InitVariables(startPoint);
            InitFloor(complexity);
            InitDimension(height, width);
            FillFloor();
        }

        /// <summary> Перечислитель класса Construct </summary>
        /// <returns> Перечислитель </returns>
        public IEnumerator GetEnumerator()
        {
            if (_elements != null) return new ConstructEnumerator(_elements);

            throw new InvalidOperationException();
        }

        #region Public Setters

        /// <summary> Установить значение длинны этажа </summary>
        /// <param name="height"> Длинна этажа </param>
        /// <returns> True - успех </returns>
        public bool SetHeight(int height)
        {
            if (height < 0)
                return false;

            _height = height;
            return true;
        }

        /// <summary> Установить значение ширины этажа </summary>
        /// <param name="width"> Ширина этажа </param>
        /// <returns> True - успех </returns>
        public bool SetWidth(int width)
        {
            if (width < 0)
                return false;

            _width = width;
            return true;
        }

        /// <summary> Установить новую координату X </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool SetStartPointX(int x)
        {
            if (x < 0) return false;

            _startPoint.X = x;
            return true;
        }

        /// <summary> Установить новую координату Y </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool SetStartPointY(int y)
        {
            if (y < 0) return false;

            _startPoint.Y = y;
            return true;
        }

        #endregion

        #region Initializing FloorClass

        private void InitVariables(Point startPoint)
        {
            _startPoint = startPoint;
            _exclude = _empty;
            _union = _empty;
        }

        /// <summary> Инициализация массива конструктов </summary>
        /// <param name="complexity"> Сложность </param>
        private void InitFloor(int complexity) => _elements = new Construct[complexity];

        /// <summary> Инициализация размеров </summary>
        /// <param name="height"> Длинна </param>
        /// <param name="width"> Ширина </param>
        private void InitDimension(int height, int width)
        {
            _height = height;
            _width = width;
        }

        /// <summary> Заполнение массива конструктов </summary>
        private void FillFloor()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                var counstruct = new Construct(_startPoint, _height, _width);
                if (counstruct.CombineMode == GeometryCombineMode.Exclude)
                {
                    _exclude++;
                }
                else
                {
                    _union++;
                }
                _elements[i] = counstruct;
                Thread.Sleep(1);
            }
        }

        #endregion
    }
}
