﻿using Microsoft.Win32;
using MvvmLib;
using RehostingLib.Extensions;
using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.Validation;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.ServiceModel.Activities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RehostingLib
{
    public sealed class DesignerViewModel : NotifyPropertyChangedBase
    {
        #region Declarations

        public enum WorkflowType
        {
            Activiy,
            Service,
        }

        // Constantes pour cibler facilement le designer sans avoir à revoir tout le code
        private const String FrameworkName = ".NETFramework";
        private const Int32 FrameworVersionMajor = 4;
        private const Int32 FrameworVersionMinor = 5;

        private const String ActivityExtension = ".xaml";
        private const String ServiceExtension = ".xamlx";

        private const String DesignerViewPropertyName = "DesignerView";
        private const String OutlineViewPropertyName = "OutlineView";
        private const String PropertyViewPropertyName = "PropertyView";

        private WorkflowDesigner _designer;
        private readonly ToolboxControl _toolbox;
        
        private ValidationErrorService _validationErrorService;
        private VisualTracking _visualTracking;
        private readonly OutputExtension _output;

        private WorkflowType _type;
        private String _fileName;

        private WorkflowApplication _host;

        private readonly Command _openCommand;
        private readonly Command _saveCommand;
        private readonly Command _saveAsCommand;
        private readonly Command _newActivityCommand;
        private readonly Command _newServiceCommand;
        private readonly CommandWithCanExecute _executeCommand;
        private readonly CommandWithCanExecute _executeSlowCommand;
        private readonly CommandWithCanExecute _stopCommand;

        #endregion

        #region Contructors

        public DesignerViewModel()
        {
            #region Préparation du designer

            // Enregistrment des meta WF pour utiliser le designer
            new DesignerMetadata().Register();

            InitilizeDesigner();

            // Créer une zone de travail
            _designer.Load(new ActivityBuilder { Name = "Designer" });

            // Création de la toolbox
            _toolbox = new ToolboxControl();
            // Activité de base
            // AddToToolbox("Built In WF", typeof(Sequence).Assembly);
            ToolboxHelper.AddAll(_toolbox);

            _output = new OutputExtension();
            
            #endregion

            #region Préparation des commandes

            _openCommand = new Command(Properties.Resources.OpenCommand, Open);
            _saveCommand = new Command(Properties.Resources.SaveCommand, Save);
            _saveAsCommand = new Command(Properties.Resources.SaveAsCommand, SaveAs);
            _newActivityCommand = new Command(Properties.Resources.NewActivityCommand, NewActivity);
            _newServiceCommand = new Command(Properties.Resources.NewServiceCommand, NewService);
            _executeCommand = new CommandWithCanExecute(Properties.Resources.ExecuteCommand, Execute, CanExecute);
            _executeSlowCommand = new CommandWithCanExecute(Properties.Resources.ExecuteSlowCommand, ExecuteSlow, CanExecute);
            _stopCommand = new CommandWithCanExecute(Properties.Resources.StopCommand, Stop, CanStop);

            #endregion
        }

        /// <summary>
        /// Initialize du designer
        /// </summary>
        private void InitilizeDesigner()
        {
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

            // Ajout du service de validation (liste d'erreurs)
            if (_validationErrorService != null)
            {
                _validationErrorService.Errors.CollectionChanged -= Errors_CollectionChanged;
            }
            Errors = new ValidationErrorService();
            _validationErrorService.Errors.CollectionChanged += Errors_CollectionChanged;
            _designer.Context.Services.Publish<IValidationErrorService>(_validationErrorService);
        }

        /// <summary>
        /// Reset du designer
        /// </summary>
        private void ResetDesigner()
        {
            InitilizeDesigner();
            RaisePropertyChanged(DesignerViewPropertyName);
            RaisePropertyChanged(PropertyViewPropertyName);
            RaisePropertyChanged(OutlineViewPropertyName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Zone de design
        /// </summary>
        public UIElement DesignerView { get { return _designer.View; } }

        /// <summary>
        /// Boite à outils
        /// </summary>
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
        public ValidationErrorService Errors { get { return _validationErrorService; }
            private set
            {
                if(_validationErrorService== value)return;
                _validationErrorService = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Tracking
        /// </summary>
        public VisualTracking Tracking
        {
            get { return _visualTracking; }
            private set
            {
                if (_visualTracking == value) return;
                _visualTracking = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Output (sortie des textwriter)
        /// </summary>
        public OutputExtension Output { get { return _output; } }

        /// <summary>
        /// String contenant le Xaml généré via le designer
        /// </summary>
        public String Text
        {
            get
            {
                _designer.Flush();
                return _designer.Text;
            }
            set
            {
                if (Text == value) return;
                _designer.Text = value;
                _designer.Load();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Path vers le fichié édité
        /// </summary>
        public String FileName
        {
            get { return _fileName; }
            private set
            {
                if (_fileName == value) return;
                _fileName = value;
                RaisePropertyChanged();
                
                // MAJ du type de fichier si possible
                if (String.IsNullOrEmpty(_fileName)) return;
                String extension =  Path.GetExtension(_fileName);
                Type = extension == ActivityExtension
                    ? WorkflowType.Activiy
                    : WorkflowType.Service;                
            }
        }

        /// <summary>
        /// WorkflowType
        /// </summary>
        public WorkflowType Type
        {
            get { return _type; }
            private set
            {
                if (_type == value) return;
                _type = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commandes

        /// <summary>
        /// Open
        /// </summary>
        public Command OpenCommand { get { return _openCommand; } }
        
        /// <summary>
        /// Save
        /// </summary>
        public Command SaveCommand { get { return _saveCommand; } }

        /// <summary>
        /// Save As
        /// </summary>
        public Command SaveAsCommand { get { return _saveAsCommand; } }

        /// <summary>
        /// Nouvelle activité (xaml)
        /// </summary>
        public Command NewActivityCommand { get { return _newActivityCommand; } }

        /// <summary>
        /// Nouveau service (xamlx)
        /// </summary>
        public Command NewServiceCommand { get { return _newServiceCommand; } }

        /// <summary>
        /// Execute
        /// </summary>
        public CommandWithCanExecute ExecuteCommand { get { return _executeCommand; } }

        /// <summary>
        /// Execute Slow
        /// </summary>
        public CommandWithCanExecute ExecuteSlowCommand { get { return _executeSlowCommand; } }

        /// <summary>
        /// Stop
        /// </summary>
        public CommandWithCanExecute StopCommand { get { return _stopCommand; } }

        #endregion

        #region Gestion de la toolbox

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

        #region Gestion des commandes sur les fichiers xaml

        /// <summary>
        /// Ouverture d'un fichier
        /// </summary>
        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = Properties.Resources.Extensions
            };

            if (dialog.ShowDialog() == true)
            {
                ResetDesigner();
                FileName = dialog.FileName;
                _designer.Load(dialog.FileName);
            }
        }

        /// <summary>
        /// Enregistrer d'un workflow
        /// </summary>
        private void Save()
        {
            if (String.IsNullOrEmpty(_fileName))
            {
                SaveAs();
            }
            else
            {
                Save(_fileName);
            }
        }

        /// <summary>
        /// nregistrer d'un workflow das un autre fichier
        /// </summary>
        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = Properties.Resources.Extensions,
            };

            if (_type == WorkflowType.Activiy)
            {
                dialog.DefaultExt = ActivityExtension;
                dialog.FilterIndex = 1;
            }
            else
            {
                dialog.DefaultExt = ServiceExtension;
                dialog.FilterIndex = 2;

            }

            if (dialog.ShowDialog() == true)
            {
                Save(dialog.FileName);
            }
        }

        /// <summary>
        /// Enregistrer le workflow
        /// </summary>
        /// <param name="path"></param>
        private void Save(String path)
        {
            if (String.IsNullOrEmpty(path)) return;

            FileName = path;
            _designer.Save(path);
        }

        /// <summary>
        /// Créer un nouveau workflow (type activity)
        /// </summary>
        private void NewActivity()
        {
            ResetDesigner();
            _designer.Load(new ActivityBuilder { Name = "Activity" });
            FileName = null;
            Type = WorkflowType.Activiy;
        }

        /// <summary>
        /// Créer un nouveau workflow (type service)
        /// </summary>
        private void NewService()
        {
            ResetDesigner();
            _designer.Load(new WorkflowService { Name = "Service" });
            FileName = null;
            Type = WorkflowType.Service;
        }

        #endregion

        #region Gestion de l'execution

        private void Errors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _executeCommand.RaiseCanExecuteChanged();
        }

        private Boolean CanExecute()
        {
            return _validationErrorService.Errors.Count <= 0
                && !_designer.IsInErrorState();
        }

        private void Execute()
        {
            Execute(false);
        }
        
        private void ExecuteSlow()
        {
            Execute(true);
        }

        private void Execute(Boolean slow)
        {
            // Valider l'activité créé dynamiquement
            Activity activity = XamlHelper.GetActivity(Text);
            DynamicActivity d = (DynamicActivity)activity;
            if (d.Implementation == null) return;

            if (_type == WorkflowType.Activiy)
            {                
                // Pour avoir un tracking visual, il faut un mapping.
                // Le mapping n'est possible que si le designer est chargé à partir d'un fichier.
                // Afin de palier à ce problème, on enregistre le fichier dans un répertoire temporaire et on recharge
                String path = Path.GetTempFileName();
                _designer.Flush();
                _designer.Save(path);
                ResetDesigner();
                _designer.Load(path);

                _host = new WorkflowApplication(activity);
                
                Tracking = new VisualTracking(_designer, slow);                
                
                // Ajout des extension
                _host.Extensions.Add(_visualTracking);
                _host.Extensions.Add(_output);

                _host.Completed = (e) =>
                {
                    //This is to remove the final debug adornment
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, new Action(ClearDebug));
                    _host = null;
                    _stopCommand.RaiseCanExecuteChanged();
                };

                // Invoquer le workdflow
                _host.Run();
                _stopCommand.RaiseCanExecuteChanged();
            }
            else
            {
                throw new NotImplementedException("L'hébergement de service n'est pas supporté actuellement!");
            }
        }

        private void ClearDebug()
        {
            _designer.DebugManagerView.CurrentLocation = null;
        }

        /// <summary>
        /// CanStop
        /// </summary>
        /// <returns></returns>
        private Boolean CanStop()
        {
            return _host != null;
        }

        /// <summary>
        /// Stop
        /// </summary>
        private void Stop()
        {
            if (_host == null) return;
            _host.Cancel();
        }

        #endregion
    }
}
