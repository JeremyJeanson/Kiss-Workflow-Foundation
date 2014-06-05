using MvvmLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rehosting.ViewModels
{
    public sealed class ApplicationViewModel
    {
        private readonly Command _exitCommand;

        public ApplicationViewModel()
        {
            _exitCommand = new Command(Properties.Resources.ExitCommand, Exit);
        }

        public Command ExitCommand
        {
            get { return _exitCommand; }
        }

        public void Exit()
        {
            Application.Current.Shutdown(0);
        }
    }
}
