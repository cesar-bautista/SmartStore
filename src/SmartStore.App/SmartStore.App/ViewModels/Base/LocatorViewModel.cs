using System;
using System.Linq;
using Autofac;

namespace SmartStore.App.ViewModels.Base
{
    public sealed class LocatorViewModel
    {
        private readonly IContainer _container;
        private static readonly Lazy<LocatorViewModel> Lazy = new Lazy<LocatorViewModel>(() => new LocatorViewModel());

        public static LocatorViewModel Instance => Lazy.Value;

        public LocatorViewModel()
        {
            var builder = new ContainerBuilder();

            // View models, Autofac will register concrete classes as multi-instance.
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => 
                    t.Name.EndsWith("ViewModel") &&
                    t.Name != "LocatorViewModel" &&
                    t.Name != "BaseViewModel")
                .PropertiesAutowired()
                .AsSelf();

            // Services, Autofac will register interface.
            var services = new[] { "DialogService", "SettingsService", "NavigationService", "RequestService" }; 
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => 
                    t.Name.EndsWith("Service") &&
                    !services.Any(e => t.Name.Equals(e)))
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            // Services, Singleton
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => services.Any(e => t.Name.Equals(e)))
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .SingleInstance();

            _container?.Dispose();

            _container = builder.Build();
        }

        public T Resolve<T>()
            where T : class
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }
    }
}
