using System;
using System.IO;
using System.Linq;
using Autofac;
using AutoMapper;
using SmartStore.App.Mapping;
using SQLite;

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
            var services = new[] { "DialogService", "SettingsService", "NavigationService" };
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

            // Repository, Autofac will register interface.
            var repositories = new[] { "RestRepository" };
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t =>
                    t.Name.EndsWith("Repository") &&
                    !repositories.Any(e => t.Name.Equals(e)))
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            // Repository, Singleton
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => repositories.Any(e => t.Name.Equals(e)))
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .SingleInstance();

            // Automapper
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            builder.RegisterInstance<IMapper>(mapper);

            // DataBase
            builder.Register(c => new SQLiteAsyncConnection(FilePathDb))
                .AsSelf()
                .SingleInstance();

            _container?.Dispose();

            _container = builder.Build();
        }

        public static string FilePathDb
        {
            get
            {
                const string fileNameDb = "SmartStore.db3";
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var path = Path.Combine(libraryPath, fileNameDb);
                return path;
            }
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
