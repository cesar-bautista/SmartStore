using System;
using Autofac;
using SmartStore.App.Abstractions;
using SmartStore.App.Services;

namespace SmartStore.App.ViewModels.Base
{
    public class LocatorViewModel
    {
        private static readonly IContainer Container;
        private static readonly Lazy<LocatorViewModel> Lazy = new Lazy<LocatorViewModel>(() => new LocatorViewModel());

        public static LocatorViewModel Instance => Lazy.Value;

        static LocatorViewModel()
        {
            var builder = new ContainerBuilder();

            // View models, Autofac will register concrete classes as multi-instance.
            Register<SplashViewModel>(builder);
            Register<MainViewModel>(builder);
            Register<MenuViewModel>(builder);
            Register<LoginViewModel>(builder);
            Register<HomeViewModel>(builder);

            // Services, Autofac will register interface.
            Register<IMenuService, MenuService>(builder);

            // Services, Singleton
            RegisterSingleton<IDialogService, DialogService>(builder);
            RegisterSingleton<ISettingsService, SettingsService>(builder);
            RegisterSingleton<INavigationService, NavigationService>(builder);
            RegisterSingleton<IRequestService, RequestService>(builder);

            Container?.Dispose();

            Container = builder.Build();
        }

        private static void Register<T>(ContainerBuilder builder)
        {
            builder
                .RegisterType<T>()
                .AsSelf();
        }

        private static void Register<TInterface, T>(ContainerBuilder builder)
            where TInterface : class where T : class, TInterface
        {
            builder
                .RegisterType<T>()
                .As<TInterface>();
        }

        private static void RegisterSingleton<TInterface, T>(ContainerBuilder builder)
            where TInterface : class where T : class, TInterface
        {
            builder
                .RegisterType<T>()
                .As<TInterface>()
                .SingleInstance();
        }

        public static T Resolve<T>()
            where T : class
        {
            return Container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }
    }
}
