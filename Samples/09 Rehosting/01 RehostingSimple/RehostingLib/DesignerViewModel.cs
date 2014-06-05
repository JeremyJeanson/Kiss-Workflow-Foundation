using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.Validation;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RehostingLib
{
    public sealed class DesignerViewModel
    {
        #region Declarations

        // Constantes pour cibler facilement le designer sans avoir à revoir tout le code
        private const String FrameworkName = ".NETFramework";
        private const Int32 FrameworVersionMajor = 4;
        private const Int32 FrameworVersionMinor = 5;

        private readonly WorkflowDesigner _designer;
        private readonly ToolboxControl _toolbox;
        private readonly ValidationErrorService _validationErrorService;

        #endregion

        #region Contructors

        public DesignerViewModel()
        {
            // Enregistrment des meta WF pour utiliser le designer
            new DesignerMetadata().Register();

            // Instancition du designer
            _designer = new WorkflowDesigner();


            // Récupération duservice de configuration
            var configurationService = _designer.Context.Services.GetService<DesignerConfigurationService>();

            // Cibler le framework (voir constantes en haut de class)
            configurationService.TargetFrameworkName = new FrameworkName(FrameworkName, new Version(FrameworVersionMajor, FrameworVersionMinor));

            // Activer toutes les nouvelles fonctionnalité du designer présentes de puis .net 4.5    
            configurationService.AutoConnectEnabled = true;
            configurationService.AutoSplitEnabled = true;
            configurationService.AutoSurroundWithSequenceEnabled = true;
            configurationService.BackgroundValidationEnabled = true;
            configurationService.LoadingFromUntrustedSourceEnabled = true;
            configurationService.MultipleItemsContextMenuEnabled = true;
            configurationService.MultipleItemsDragDropEnabled = true;
            configurationService.NamespaceConversionEnabled = true;
            configurationService.PanModeEnabled = true;
            configurationService.RubberBandSelectionEnabled = true;
            
            // fcontionnalité présentes à partir de .net 4.5
            if (FrameworVersionMajor >= 4 && FrameworVersionMinor >= 5)
            {
                configurationService.AnnotationEnabled = true;
            }

            // Créer une zone de travail
            _designer.Load(new ActivityBuilder { Name = "Designer" });

            // Ajout du service de validation (liste d'erreurs)
            _validationErrorService = new ValidationErrorService();
            _designer.Context.Services.Publish<IValidationErrorService>(_validationErrorService);

            // Création de la toolbox
            _toolbox = new ToolboxControl();
            // Activité de base
            AddToToolbox("Built In WF", typeof(Sequence).Assembly);
            _toolbox.Loaded += _toolbox_Loaded;
        }

        void _toolbox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Zone de design
        /// </summary>
        public UIElement DesignerView { get { return _designer.View; } }

        public ToolboxControl ToolBoxView { get { return _toolbox; } }

        /// <summary>
        /// Zone de navigation
        /// </summary>
        public UIElement OutlineView{get{return  _designer.OutlineView;}}
        
        /// <summary>
        /// Zone des properties
        /// </summary>
        public UIElement PropertyView{get{return _designer.PropertyInspectorView;}}

        /// <summary>
        /// Liste d'erreurs de valdiation
        /// </summary>
        public ObservableCollection<ValidationErrorInfo> Errors { get { return _validationErrorService.Errors; } }

        #endregion

        #region Methodes

        /// <summary>
        /// Ajout d'une catégorie à la toolbox
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="assemblies"></param>
        private void AddToToolbox(String categoryName, params Assembly[] assemblies)
        {
            ToolboxCategory category = GetToolBoxCategory(categoryName, assemblies);
            if (category == null) return;
                _toolbox.Categories.Add(category);
        }

        /// <summary>
        /// Récupération d'une cétagorie
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static ToolboxCategory GetToolBoxCategory(String categoryName, params Assembly[] assemblies)
        {
            // Création d'une nouvelel category
            ToolboxCategory toolboxCategory = new ToolboxCategory(categoryName);
            // 
            Type[] defaultConstructorArgs = new Type[0];

            // Filtre utilisé pour trouver les activité à wrapper
            Func<Assembly, IEnumerable<ToolboxItemWrapper>> getItems = assembly =>
                from type in assembly.GetTypes()
                where type.IsPublic
                    && !type.IsNested
                    && !type.IsAbstract
                    && type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, defaultConstructorArgs, null) != null
                    && (typeof(Activity).IsAssignableFrom(type)
                    || typeof(IActivityTemplateFactory).IsAssignableFrom(type))
                orderby type.Name
                select new ToolboxItemWrapper(type);

            // Requete sur la liste des controls pour trouver ceux qui seront utilisables
            List<ToolboxItemWrapper> query = assemblies
                .SelectMany(c => getItems(c))
                .ToList();

            if (query.Count > 0)
            {
                try
                {
                    // Ajout des catégories à la toolbox
                    query.ForEach(c => toolboxCategory.Add(c));

                    // Retourner la nouvelle catégorie pour la toolbox
                    return toolboxCategory;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Impossbile de charger des activités dans la tollbox du designer.", ex.Message);
                    return null;
                }
            }
            return null;
        }

        #endregion
    }
}
