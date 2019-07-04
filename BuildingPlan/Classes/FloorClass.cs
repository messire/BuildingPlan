using System.Collections;
using System.Linq;
using System.Threading;
using System.Windows;

namespace BuildingPlan.Classes
{
    /// <summary>
    /// Класс описывающий этаж. Это набор геометрических фигур.
    /// Выбран массив, т.к.:
    /// -- он быстрее, в этом месте память можно сэкономить, пригодится в будущем
    /// -- более явный контроль, в каждый момент времени понятно что сейчас обрабатывается
    /// -- нет необходимости в динамике, параметры этажа задаются на входе
    /// </summary>
    public class FloorClass: IEnumerable
    {
        #region fields

        public Construct[] _elements;
        private int _height;
        private Point _startPoint;
        private int _width;
        

        #endregion

        #region properties

        public int Height => _height;

        public int Occupancy => _elements.Count(e => e != null);

        public int Size => _elements.Length;

        public Point StartPoint => _startPoint;

        public int Width => _width;

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
        public IEnumerator GetEnumerator() => new ConstructEnumerator(_elements);

        #region Initializing FloorClass

        private void InitVariables(Point startPoint)
        {
            _startPoint = startPoint;
        }

        /// <summary> Инициализация массива конструктов </summary>
        /// <param name="complexity"> Сложность </param>
        private void InitFloor(int complexity) => _elements = new Construct[complexity];

        /// <summary> Инициализация размеров </summary>
        /// <param name="height"> Высота </param>
        /// <param name="width"> Ширина </param>
        private void InitDimension(int height, int width)
        {
            _height = height;
            _width = width;
        }

        /// <summary> Заполнение массива конструктов </summary>
        /// <param name="startPoint"> Координата базовой фигуры </param>
        private void FillFloor()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                _elements[i] = new Construct(_startPoint, _height, _width);
                Thread.Sleep(1);
            }
        }

        #endregion
    }
}
