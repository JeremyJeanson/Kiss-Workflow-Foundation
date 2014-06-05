using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLib
{
    /// <summary>
    /// EventArgs generic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EventArgs<T>:EventArgs
    {
        private readonly T _value;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="value"></param>
        public EventArgs(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Value
        /// </summary>
        public T Value { get { return _value; } }
    }
}
