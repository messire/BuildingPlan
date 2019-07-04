using System;
using System.Collections;

namespace BuildingPlan.Classes
{
    public class ConstructEnumerator : IEnumerator
    {
        #region fields

        readonly Construct[] _elements;
        int _position = -1;

        #endregion

        #region Constructor

        public ConstructEnumerator(Construct[] elements)
        {
            _elements = elements;
        }

        #endregion

        #region Interface Implementation

        public object Current
        {
            get
            {
                if (_position > -1 && _position < _elements.Length)
                    return _elements[_position];

                throw new InvalidOperationException();
            }
        }

        public bool MoveNext()
        {
            if (_position >= _elements.Length - 1) return false;

            _position++;
            return true;
        }

        public void Reset() => _position = -1;

        #endregion
    }
}
