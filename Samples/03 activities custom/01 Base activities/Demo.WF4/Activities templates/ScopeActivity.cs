using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Demo.WF4
{

    public sealed class MySequence : NativeActivity
    {
        #region "Déclarations"

        private readonly Collection<Variable> _variables;

        #endregion

        #region "Constructeur"

        /// 
        /// Constructeur
        /// 
        public MySequence()
        {
            this._variables = new Collection<Variable>();
        }

        #endregion

        #region "Propriétés"

        /// 
        /// Liste de variables
        /// 
        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get { return this._variables; }
        }

        #endregion

        // ...

        protected override void Execute(NativeActivityContext context)
        {
            //
        }
    }
}
