using System.Collections;
using System.Collections.Generic;
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
    /// 
    public class FloorClass: IEnumerable
    {
        #region properties

        public int Size => _elements.Length;
        public int Occupancy => _elements.Count(e => e != null);

        #endregion

        #region fields

        public Construct[] _elements;
        private double _height;
        private double _width;

        #endregion
        
        /// <summary> Конструктор этажа. </summary>
        /// <param name="width"> Ширина этажа </param>
        /// <param name="height"> Высота этажа </param>
        /// <param name="complexity"> Сложность </param>
        public FloorClass(Point startPoint, int width, int height, int complexity)
        {
            _elements = new Construct[complexity];
            _height = height;
            _width = width;

            for (int i = 0; i < _elements.Length; i++)
            {
                _elements[i] = new Construct(startPoint, height, width);
                Thread.Sleep(1);
            }
        }

        /// <summary> Инициализируем перечислитель </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => new ConstructEnumerator(_elements);
    }
}
