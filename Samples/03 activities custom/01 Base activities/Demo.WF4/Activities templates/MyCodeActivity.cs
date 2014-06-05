using System;
using System.Activities;

namespace Demo.WF4
{

    public sealed class MyCodeActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            // Job à exécuter
        }
    }

    public sealed class MyCodeActivityT : CodeActivity<String>
    {
        protected override string Execute(CodeActivityContext context)
        {
            return "... Retour du job à exécuter";
        }
    }

    public sealed class MyNativeActivity : NativeActivity
    {
        protected override void Execute(NativeActivityContext context)
        {
            // Job à exécuter ...  le context a une méthode Shedule… pour lancer d’autre activités 
        }
    }

    public sealed class MyNativeActivityT : NativeActivity<String>
    {
        protected override void Execute(NativeActivityContext context)
        {
            this.Result.Set(context, "...Retour du job à exécuter");
        }
    }

    public sealed class MyAsyncCodeActivity : AsyncCodeActivity
    {
        /// <summary>
        /// Lancement asynchrone de l'execution du job
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback,
                                                     object state)
        {
            Func<Int32, Boolean> job = new Func<Int32, Boolean>(this.Test);
            context.UserState = job;
            return job.BeginInvoke(-6, callback, state);
        }

        /// <summary>
        /// Récupération du résultat du traitemetn effectué dans le job
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            Func<Int32, Boolean> job = context.UserState as Func<Int32, Boolean>;
            if (job != null)
            {
                Boolean retour = job.EndInvoke(result);
            }
        }

        /// <summary>
        /// Job a réaliser
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private Boolean Test(Int32 argument)
        {
            System.Threading.Thread.Sleep(5000);
            return argument > 0;
        }
    }

    public sealed class MyAsyncCodeActivityT : AsyncCodeActivity<Boolean>
    {
        /// <summary>
        /// Lancement asynchrone de l'execution du job
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback,
                                                     object state)
        {
            Func<Int32, Boolean> job = new Func<int, bool>(this.Test);
            context.UserState = job;
            return job.BeginInvoke(6, callback, state);
        }

        /// <summary>
        /// Récupération du résultat du traitemetn effectué dans le job
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            Func<Int32, Boolean> job = context.UserState as Func<Int32, Boolean>;
            if (job != null)
            {
                return job.EndInvoke(result);
            }
            return false;
        }

        /// <summary>
        /// Job a réaliser
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private Boolean Test(Int32 argument)
        {
            System.Threading.Thread.Sleep(5000);
            return argument > 0;
        }
    }

}
