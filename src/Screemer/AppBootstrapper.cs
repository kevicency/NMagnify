using System.Windows.Threading;
using LightCore;
using LightCore.Lifecycle;
using Screemer.Controls;
using Screemer.Model;
using Screemer.ViewModels;
using Screemer.Views;

namespace Screemer
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Caliburn.Micro;

	public class AppBootstrapper : Bootstrapper<IShell>
	{
		IContainer _container;

		/// <summary>
		/// By default, we are configured to use MEF
		/// </summary>
		protected override void Configure()
		{
            var builder = new ContainerBuilder();
            builder.DefaultControlledBy<SingletonLifecycle>();

		    builder.Register<IWindowManager, WindowManager>();
		    builder.Register<IEventAggregator, EventAggregator>();

		    builder.Register<IShell, ShellViewModel>();
		    builder.Register<ISelectCaptureRegion, SelectCaptureRegionOverlay>().ControlledBy<TransientLifecycle>();
		    builder.Register<ShellView>();
		    builder.Register<PlaybackStreamViewModel>();
		    builder.Register<CaptureRegionSettingsViewModel>();
		    builder.Register<SelectProfileViewModel>();
		    builder.Register<IActiveProfileProvider>(c => c.Resolve<SelectProfileViewModel>());

		    builder.Register<IScreenCapturer>(c => c.Resolve<BitmapScreenCapturer>());
		    builder.Register<BitmapScreenCapturer>();
		    builder.Register<IProfileManager, ProfileManager>();
		    builder.Register<Func<Profile>>(c => () => c.Resolve<SelectProfileViewModel>().ActiveProfile);

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
