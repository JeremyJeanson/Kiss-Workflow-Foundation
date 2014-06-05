using MvvmLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RehostingLib.Extensions
{
    /// <summary>
    /// Extension pour avoir une sortie des appel des TextWriters
    /// </summary>
    public sealed class OutputExtension:TextWriter
    {
        #region Declarations

        private readonly ObservableCollection<String> _lignes;
        private static Object _lockLines;
        public readonly CommandWithCanExecute _clearCommand;

        #endregion

        #region Contructors

        /// <summary>
        /// Constructeur static
        /// </summary>
        static OutputExtension()
        {
            _lockLines = new Object();
        }

        internal OutputExtension()
        {
            _lignes = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(_lignes, _lockLines);
            _clearCommand = new CommandWithCanExecute(Properties.Resources.ClearCommand, Clear, CanClear);
            _lignes.CollectionChanged += _lignes_CollectionChanged;
        }

        void _lignes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _clearCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Items
        /// </summary>
        public ObservableCollection<String> Items { get { return _lignes; } }

        /// <summary>
        /// ClearCommand
        /// </summary>
        public CommandWithCanExecute ClearCommand { get { return _clearCommand; } }

        #endregion

        #region Methodes

        public override void WriteLine(string value)
        {
            _lignes.Add(value);
        }


        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        private Boolean CanClear()
        {
            return _lignes.Count > 0;
        }

        private void Clear()
        {
            _lignes.Clear();
        }

        #endregion
    }
}
