﻿using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace MyToolkit.Paging
{
    public abstract class MtApplication : Application
    {
        protected MtApplication()
        {
            Suspending += OnSuspending;
        }

        /// <summary>Gets the type of the start page (first page when launching application). </summary>
        public abstract Type StartPageType { get; }

        /// <summary>Called when a new instance of the application has been created. </summary>
        /// <param name="frame">The frame. </param>
        /// <param name="args">The launch arguments.</param>
        public virtual Task OnInitializedAsync(MtFrame frame, ApplicationExecutionState args)
        {
            return null; // Must be empty
        }

        /// <summary>Creates the root frame. </summary>
        /// <returns>The root frame. </returns>
        public virtual MtFrame CreateFrame()
        {
            return new MtFrame();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            await InitializeFrameAsync(args.PreviousExecutionState);
        }

        /// <summary>Creates the application's root frame and loads the first page if needed. 
        /// Also calls <see cref="OnInitializedAsync"/> when the application is instantiated the first time. </summary>
        /// <param name="executionState">The application execution state. </param>
        /// <returns>The task. </returns>
        protected async Task InitializeFrameAsync(ApplicationExecutionState executionState)
        {
            var rootFrame = Window.Current.Content as MtFrame;
            if (rootFrame == null)
            {
                rootFrame = CreateFrame();

                MtSuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                if (executionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await MtSuspensionManager.RestoreAsync();
                    }
                    catch { }
                }

                var task = OnInitializedAsync(rootFrame, executionState);
                if (task != null)
                    await task;

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
                rootFrame.Initialize(StartPageType);
            
            Window.Current.Activate();
        }

        /// <summary>Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact. </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await MtSuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
