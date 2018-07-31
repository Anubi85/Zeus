using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Zeus.Exceptions;

namespace Zeus.UI.Mvvm
{
    /// <summary>
    /// Implment a common storage for services and allow to retrieve registered services.
    /// </summary>
    public static class ServiceLocator
    {
        #region Fields

        /// <summary>
        /// Stores the registerd service instances.
        /// </summary>
        private static Dictionary<Type, object> s_ServiceStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class fields.
        /// </summary>
        static ServiceLocator()
        {
            s_ServiceStore = new Dictionary<Type, object>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register a new service.
        /// </summary>
        /// <typeparam name="T">The type with which the service will be registered.</typeparam>
        /// <param name="instance">The service instance.</param>
        public static void Register<T>(T instance)
        {
            //check if already registred
            if (s_ServiceStore.ContainsKey(typeof(T)))
            {
                throw new ZeusException(ErrorCodes.RegistrationAlreadyExists, string.Format("A service type {0} is already registred.", typeof(T).FullName));
            }
            s_ServiceStore.Add(typeof(T), instance);
        }
        /// <summary>
        /// Update a registred service.
        /// </summary>
        /// <typeparam name="T">The type with which the service has been registred.</typeparam>
        /// <param name="instance">The new service instance.</param>
        public static void Update<T>(T instance)
        {
            //check for existance
            if (!s_ServiceStore.ContainsKey(typeof(T)))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("Service of type {0} was not found in service store.", typeof(T)));
            }
            s_ServiceStore[typeof(T)] = instance;
        }
        /// <summary>
        /// Register a new service or update an existing one.
        /// </summary>
        /// <typeparam name="T">The type with which the service will be registered.</typeparam>
        /// <param name="instance">The service instance.</param>
        public static void RegisterOrUpdate<T>(T instance)
        {
            if (s_ServiceStore.ContainsKey(typeof(T)))
            {
                Update<T>(instance);
            }
            else
            {
                Register<T>(instance);
            }
        }
        /// <summary>
        /// Unregister a registred service.
        /// </summary>
        /// <typeparam name="T">The type with which the service has been registred.</typeparam>
        public static void Unregister<T>()
        {
            //check for existance
            if (!s_ServiceStore.ContainsKey(typeof(T)))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("Service of type {0} was not found in service store.", typeof(T)));
            }
            s_ServiceStore.Remove(typeof(T));
        }
        /// <summary>
        /// Gets the instance of the service registred wih the requested type.
        /// </summary>
        /// <typeparam name="T">The type of the registered service that must be retrieved.</typeparam>
        /// <returns>The registred service instace.</returns>
        public static T Resolve<T>()
        {
            //check if in design mode
            if ((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue)
            {
                return default(T);
            }
            //check for existance
            if (!s_ServiceStore.ContainsKey(typeof(T)))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("Service of type {0} was not found in service store.", typeof(T)));
            }
            return (T)s_ServiceStore[typeof(T)];
        }

        #endregion
    }
}
