using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Zeus.UI.Mvvm
{
    /// <summary>
    /// Provides common view model functionalities.
    /// </summary>
    public abstract class ViewModelBase : ObservableObjectBase
    {
        #region Fields

        /// <summary>
        /// A dictionary that contains the information about the registed property changes propagation.
        /// </summary>
        private Dictionary<INotifyPropertyChanged, Dictionary<string, List<string>>> m_RegisterdPropagation;

        #endregion

        #region Methods

        /// <summary>
        /// Register a model property for the automatic propagation.
        /// </summary>
        /// <typeparam name="TModel">The type of the model property.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model property.</typeparam>
        /// <param name="model">The model object.</param>
        /// <param name="modelPropExpression">The expression used to retrieve the model property.</param>
        /// <param name="viewModelPropExpression">The expression used to retrieve the view model property.</param>
        protected void RegisterPropagation<TModel, TViewModel>(INotifyPropertyChanged model, Expression<Func<TModel>> modelPropExpression, Expression<Func<TViewModel>> viewModelPropExpression)
        {
            //check if model already exists
            if (!m_RegisterdPropagation.ContainsKey(model))
            {
                m_RegisterdPropagation.Add(model, new Dictionary<string, List<string>>());
                model.PropertyChanged += Model_PropertyChanged;
            }
            //get model property name
            string modelProperty = ((modelPropExpression.Body as MemberExpression)?.Member as PropertyInfo)?.Name;
            //check if model property already exists
            if (!m_RegisterdPropagation[model].ContainsKey(modelProperty))
            {
                m_RegisterdPropagation[model].Add(modelProperty, new List<string>());
            }
            //get view model property name
            string viewModelProperty = ((viewModelPropExpression.Body as MemberExpression)?.Member as PropertyInfo)?.Name;
            //register the property propagation
            m_RegisterdPropagation[model][modelProperty].Add(viewModelProperty);
        }

        /// <summary>
        /// Unegister a model property for the automatic propagation.
        /// </summary>
        /// <typeparam name="TModel">The type of the model property.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model property.</typeparam>
        /// <param name="model">The model object.</param>
        /// <param name="modelPropExpression">The expression used to retrieve the model property.</param>
        /// <param name="viewModelPropExpression">The expression used to retrieve the view model property.</param>
        protected void UnregisterPropagation<TModel, TViewModel>(INotifyPropertyChanged model, Expression<Func<TModel>> modelPropExpression, Expression<Func<TViewModel>> viewModelPropExpression)
        {
            //check if model exists
            if (m_RegisterdPropagation.ContainsKey(model))
            {
                //get model property name
                string modelProperty = ((modelPropExpression.Body as MemberExpression)?.Member as PropertyInfo)?.Name;
                //check if model property exists
                if (m_RegisterdPropagation[model].ContainsKey(modelProperty))
                {
                    //get view model property name
                    string viewModelProperty = ((viewModelPropExpression.Body as MemberExpression)?.Member as PropertyInfo)?.Name;
                    //unregister the property propagation
                    m_RegisterdPropagation[model][modelProperty].Remove(viewModelProperty);
                    //check if cleanup is required
                    if (m_RegisterdPropagation[model][modelProperty].Count == 0)
                    {
                        m_RegisterdPropagation[model].Remove(modelProperty);
                    }
                    if (m_RegisterdPropagation[model].Count == 0)
                    {
                        m_RegisterdPropagation.Remove(model);
                        model.PropertyChanged -= Model_PropertyChanged;
                    }
                }
            }            
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Check if the property changed notification has to be automatically propagated.
        /// </summary>
        /// <param name="sender">The object that sends the notification.</param>
        /// <param name="e">The information about the proprierty that cause the notification.</param>
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //try to propagate the notification
            try
            {
                foreach (string propName in m_RegisterdPropagation[sender as INotifyPropertyChanged][e.PropertyName])
                {
                    NotifyPropertyChanged(propName);
                }
            }
            catch
            {
                //not registered, do nothing
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class fields.
        /// </summary>
        public ViewModelBase()
        {
            m_RegisterdPropagation = new Dictionary<INotifyPropertyChanged, Dictionary<string, List<string>>>();
        }

        #endregion
    }
}
