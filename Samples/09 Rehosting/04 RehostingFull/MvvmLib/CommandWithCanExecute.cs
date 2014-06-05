using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmLib
{
    public sealed class CommandWithCanExecute : Command
    {
        private readonly Func<Boolean> _canExecute;

        public CommandWithCanExecute(String text, Action execute, Func<Boolean> canExecute)
            : base(text, execute)
        {
            this._canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }
    }
}
