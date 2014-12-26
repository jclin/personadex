using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Personadex.Suspension;
using Personadex.View;
using Personadex.ViewModel;

namespace Personadex
{
    public sealed partial class App
    {
        private readonly SuspensionTracker _suspensionTracker;
        private TransitionCollection _transitions;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            _suspensionTracker = new SuspensionTracker(new JsonAppState());
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame { CacheSize = 2 };

                Window.Current.Content = rootFrame;

                _suspensionTracker.TrackState(rootFrame);
                _suspensionTracker.TrackState<MainPage>(((ViewModelLocator)Resources["ViewModelLocator"]).MainViewModel);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated &&
                    _suspensionTracker.LastUtcWriteTime.HasValue &&
                    DateTime.UtcNow.Subtract(_suspensionTracker.LastUtcWriteTime.Value).TotalMinutes <= 10d)
                {
                    await _suspensionTracker.RestoreStateAsync(typeof(MainPage));
                }
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    _transitions = new TransitionCollection();
                    foreach (Transition c in rootFrame.ContentTransitions)
                    {
                        _transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;

                if (!rootFrame.Navigate(typeof (View.MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.Navigated -= RootFrame_FirstNavigated;

            rootFrame.ContentTransitions = _transitions ?? new TransitionCollection {new NavigationThemeTransition()};
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            await _suspensionTracker.SaveStateAsync();

            deferral.Complete();
        }
    }
}