using System;
using System.Linq;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Config
{
	public abstract class PersistedConfigClientBase<T> : ConfigClientBase<T> where T: IConfig
	{
		private Guid _subscription;
		private IMessageBroker _messenger;

		public PersistedConfigClientBase():base()
		{
			IsConfigEditable = true;
			_messenger = ServiceLocator.Locator.Locate<IMessageBroker>();
		}

		public override void Start(object args)
		{
			if (_messenger == null)
			{
				_messenger = ServiceLocator.Locator.Locate<IMessageBroker>();
			}

			GetCache();
			SubscribeToUpdates();
		}

		public override void Stop()
		{
			UnScubscribe();
		}

		public override void Build(BuildParams buildParams)
		{
			ConfigBuildParams<T> param = buildParams as ConfigBuildParams<T>;
			if (param != null)
			{
				CacheTopic = param.CacheTopic;
				LiveTopic = param.LiveTopic;
			}
		}


		private void UnScubscribe()
		{
			_messenger.UnSubscribeTopic(_subscription);
		}

		private void SubscribeToUpdates()
		{
			_messenger.SubscribeTopic<T>(LiveTopic, "CONFIG_LIVE", OnConfigUpdate);
		}

		private void GetCache()
		{
			var request = new CacheRequest<ConfigCacheBase>();
			request.Token = MessagingConstants.ServerToken;

			_messenger.CacheRequest<ConfigCacheBase, T>(CacheTopic, request, OnCacheReceived);
		}

		private void OnConfigUpdate(IMessage<T> update)
		{
			Snapshot = update.MessageContent;
			RaiseConfigChanged();
		}

		private void OnCacheReceived(ICacheResponse<T> cache)
		{
			var config = cache.Payload.FirstOrDefault();
			if (config != null)
			{
				Snapshot = config;
				IsStarted = true;
				RaiseStarted();
			}
		}
}
}
