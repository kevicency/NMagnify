using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Caliburn.Micro.Contrib;
using LightCore;
using LightCore.Lifecycle;
using NMagnify.Controls;
using NMagnify.Model;
using NMagnify.ViewModels;
using NMagnify.Views;

namespace NMagnify
{
    public class AppBootstrapper : Bootstrapper<IShell>
    {
        IContainer _container;

        /// <summary>
        ///   By default, we are configured to use MEF
        /// </summary>
        protected override void Configure()
        {
            //FrameworkExtensions.ViewLocator.EnableContextFallback();
            Func<string, object, IEnumerable<string>> baseTransformName = Caliburn.Micro.ViewLocator.TransformName;
            Func<string, object, IEnumerable<string>> fallbackNameTransform = (typeName, context) =>
            {
                var names = baseTransformName(typeName, context);
                if (context != null)
                {
                    names = names.Union(baseTransformName(typeName, null));
                }

                return names;
            };
            Caliburn.Micro.ViewLocator.TransformName = fallbackNameTransform;

            ViewLocator.AddSubNamespaceMapping("Caliburn.Micro.Contrib.Dialogs", "NMagnify.Views");

            var builder = new ContainerBuilder();
            builder.DefaultControlledBy<SingletonLifecycle>();

            builder.Register<IWindowManager, WindowManager>();
            builder.Register<IEventAggregator, EventAggregator>();

            builder.Register<IShell, ShellViewModel>();
            builder.Register<ISelectCaptureRegion, SelectCaptureRegionOverlay>().ControlledBy<TransientLifecycle>();
            builder.Register<ShellView>();
            builder.Register<PlaybackStreamViewModel>();
            builder.Register<CaptureRegionSettingsViewModel>();
            builder.Register<ProfileManagerViewModel>();
            builder.Register<IActiveProfileProvider>(c => c.Resolve<ProfileManagerViewModel>());

            builder.Register<IScreenCapturer>(c => c.Resolve<BitmapScreenCapturer>());
            builder.Register<BitmapScreenCapturer>();
            builder.Register<IProfileManager, ProfileManager>();
            builder.Register<Func<Profile>>(c => () => c.Resolve<ProfileManagerViewModel>().ActiveProfile);

            _container = builder.Build();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.Resolve(serviceType);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }
    }
}