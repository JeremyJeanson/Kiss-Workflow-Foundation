using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MvvmLib
{
    /// <summary>
    /// Command basique toujorus autorisée à s'executer
    /// </summary>
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly String _text;
        private readonly Action _execute;
        private readonly String _image;


        public Command(String text, Action execute) : this(text, null, execute) { }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="text"></param>
        /// <param name="execute"></param>
        public Command(String text,String image, Action execute)
        {
            Contract.Requires(execute != null, "\"execute\" must be set!");

            _text = text;
            _image = image;
            _execute = execute;
        }

        /// <summary>
        /// Text
        /// </summary>
        public String Text { get { return _text; } }

        public String Image { get { return _image; } }

        /// <summary>
        /// CanExecute toujours vrai
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual Boolean CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(InnerRaiseCanExecuteChanged), DispatcherPriority.Normal, null);
        }

        private  void InnerRaiseCanExecuteChanged()
        {
            if (CanExecuteChanged == null) return;
            CanExecuteChanged(this, new EventArgs());
        }
    }
}
