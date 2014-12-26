using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight;

namespace Personadex.Suspension
{
    internal sealed class SuspensionTracker
    {
        private const string NavigationStateName = "NavigationState";

        private readonly IAppState _appState;
        private readonly Dictionary<Type, ViewModelBase> _pageStatesToTrack;

        private WeakReference<Frame> _weakFrameReference;

        public DateTime? LastUtcWriteTime
        {
            get
            {
                return _appState.LastUtcPersistTime;
            }
        }

        private Type CurrentPageType
        {
            get
            {
                Frame frame;
                if (!_weakFrameReference.TryGetTarget(out frame))
                {
                    return null;
                }

                var currentPage = frame.Content as Page;
                return currentPage == null ? null : currentPage.GetType();
            }
        }

        public SuspensionTracker(IAppState appState)
        {
            _appState          = appState;
            _pageStatesToTrack = new Dictionary<Type, ViewModelBase>();
        }

        public void TrackState(Frame frame)
        {
            _weakFrameReference = new WeakReference<Frame>(frame);

            frame.Navigating += OnFrameNavigating;
            frame.Navigated  += OnFrameNavigated;
        }

        public void TrackState<TPage>(ViewModelBase customState) where TPage : Page
        {
            _pageStatesToTrack[typeof(TPage)] = customState;
        }

        public async Task SaveStateAsync()
        {
            Frame frame;
            if (!_weakFrameReference.TryGetTarget(out frame))
            {
                return;
            }

            _appState.WriteOutFrom(NavigationStateName, frame.GetNavigationState());
            await _appState.PersistStateAsync();
        }

        public async Task RestoreStateAsync(params Type[] restorePageTypes)
        {
            Frame frame;
            if (!_weakFrameReference.TryGetTarget(out frame))
            {
                return;
            }

            await _appState.RestoreStateAsync();

            var navigationState = string.Empty;
            _appState.ReadInto(NavigationStateName, ref navigationState);

            frame.SetNavigationState(navigationState);

            var currentPage = frame.Content as Page;
            Debug.Assert(currentPage != null, "SuspensionTracker.RestoreStateAsync: error, no page restored from app termination.");

            RestorePageState(currentPage.GetType());
            RestorePageStates(restorePageTypes);
        }

        private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (ShouldSaveCurrentPageState(CurrentPageType, e.SourcePageType))
            {
                SavePageState(CurrentPageType);
            }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.New)
            {
                RestorePageState(CurrentPageType);
            }
        }

        private bool ShouldSaveCurrentPageState(Type currentPageType, Type nextPageType)
        {
            return currentPageType != null &&
                   currentPageType != nextPageType &&
                   _pageStatesToTrack.ContainsKey(currentPageType);
        }

        private bool ShouldRestoreCurrentPageState(Type currentPageType)
        {
            return currentPageType != null &&
                   _pageStatesToTrack.ContainsKey(currentPageType);
        }

        private void SavePageState(Type pageType)
        {
            Frame frame;
            if (!_weakFrameReference.TryGetTarget(out frame))
            {
                return;
            }

            _appState.WriteOutFrom(pageType.FullName, GetPageState(pageType));
        }

        private void RestorePageStates(IEnumerable<Type> restorePageTypes)
        {
            if (restorePageTypes == null)
            {
                return;
            }

            foreach (var pageType in restorePageTypes)
            {
                RestorePageState(pageType);
            }
        }

        private void RestorePageState(Type pageType)
        {
            if (!ShouldRestoreCurrentPageState(pageType))
            {
                Debug.WriteLine("{0} not registered for state restoration", pageType.Name);
                return;
            }

            Frame frame;
            if (!_weakFrameReference.TryGetTarget(out frame))
            {
                return;
            }

            var pageState = GetPageState(pageType);
            _appState.ReadInto(pageType.FullName, ref pageState);
        }

        private object GetPageState(Type pageType)
        {
            if (!_pageStatesToTrack.ContainsKey(pageType))
            {
                throw new InvalidOperationException(string.Format("{0} was not registered for state tracking.", pageType.GetType().Name));
            }

            return _pageStatesToTrack[pageType];
        }
    }
}
