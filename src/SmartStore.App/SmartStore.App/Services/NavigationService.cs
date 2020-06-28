using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Controls;
using SmartStore.App.ViewModels;
using SmartStore.App.ViewModels.Base;
using SmartStore.App.Views;
using Xamarin.Forms;

namespace SmartStore.App.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> Mappings;

        protected Application CurrentApplication => Application.Current;

        public NavigationService()
        {
            Mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public Task InitializeAsync()
        {
            return NavigateToAsync<SplashViewModel>();
        }

        public Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter = null)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                mainPage.Detail.Navigation.RemovePage(mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreateAndBindPage(viewModelType, parameter);

            if (page is SplashView || page is LoginView)
            {
                CurrentApplication.MainPage = new CustomNavigation(page);
            }
            else if (page is MainView)
            {
                CurrentApplication.MainPage = page;
            }
            else if (CurrentApplication.MainPage is MainView mainPage)
            {
                if (mainPage.Detail is CustomNavigation navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    navigationPage = new CustomNavigation(page);
                    mainPage.Detail = navigationPage;
                }

                mainPage.IsPresented = false;
            }
            else
            {
                if (CurrentApplication.MainPage is CustomNavigation navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new CustomNavigation(page);
                }
            }

            await ((BaseViewModel)page.BindingContext).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!Mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return Mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            var page = Activator.CreateInstance(pageType) as Page;
            var viewModel = LocatorViewModel.Instance.Resolve(viewModelType) as BaseViewModel;
            page.BindingContext = viewModel;

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            var viewsModels = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(a => a.IsClass && a.Namespace != null && a.Name.EndsWith("ViewModel"))
                .ToList();

            var views = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(a => a.IsClass && a.Namespace != null && a.Name.EndsWith("View"))
                .ToList();

            foreach (var view in views)
            {
                var vm = viewsModels.FirstOrDefault(v => v.Name.Equals($"{view.Name}Model"));
                if (vm != null)
                {
                    Mappings.Add(vm, view);
                }
            }
        }
    }
}
