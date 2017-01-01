using System;
using System.Linq;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Common.Messaging.Messages;

namespace SVIBEL.Core.Config
{
	/// <summary>
	/// <typeparam name="T">Type of the Entity</typeparam>
	/// <typeparam name="U">Type of the CacheRequest</typeparam>
	/// <typeparam name="Z">Type of the Message to receive</typeparam>
	/// </summary>
	public abstract class PersistedConfigClientBase<T, U, Z> : ConfigClientBase<T> 
		where T: IConfig
		where Z : class, IMessage<T>
	{
		private Guid _subscription;
		protected IMessageBroker _messenger;

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

		protected virtual void GetCache()
		{
			ICacheRequest<U> request = new CacheRequest<U>();
			request.Token = MessagingConstants.ServerToken;

			_messenger.CacheRequest<U, T, Z>(CacheTopic, request, OnCacheReceived);
		}


		private void UnScubscribe()
		{
			_messenger.UnSubscribeTopic(_subscription);
		}

		private void SubscribeToUpdates()
		{
			_messenger.SubscribeTopic<T,Z>(LiveTopic, "CONFIG_LIVE", OnConfigUpdate);
		}

		private void OnConfigUpdate(IMessage<T> update)
		{
			Snapshot = update.MessageContent;
			RaiseConfigChanged();
		}

		protected void OnCacheReceived(ICacheResponse<T> cache)
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
