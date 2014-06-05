using System;
using System.Activities.Presentation.Validation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehostingLib
{
    /// <summary>
    /// Service permettant l'affichage des erreur présentes dans le workflow
    /// </summary>
    public sealed class ValidationErrorService : IValidationErrorService
    {
        #region Declarations

        private readonly ObservableCollection<ValidationErrorInfo> _errors;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="errors"></param>
        internal ValidationErrorService()
        {
            _errors = new ObservableCollection<ValidationErrorInfo>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Errors
        /// </summary>
        public ObservableCollection<ValidationErrorInfo> Errors { get { return _errors; } }

        #endregion

        #region Methodes

        /// <summary>
        /// Peupler la liste des errors
        /// </summary>
        /// <param name="errors"></param>
        public void ShowValidationErrors(IList<ValidationErrorInfo> errors)
        {
            _errors.Clear();

            foreach (var error in errors)
            {
                this._errors.Add(error);
            }
        }

        #endregion
    }
}
