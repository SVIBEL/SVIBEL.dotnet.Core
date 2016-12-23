using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Common.Messaging
{
	/// <summary>
	/// Message broker
	/// <typeparam name="X">Type of the config client this IMessageBroker takes</typeparam>
	/// </summary>
	public interface IMessageBrokerConfigured<X> : IMessageBroker 
	{
		X ServerConfig { get; }
	}

	/// <summary>
	/// Message broker
	/// </summary>
	public interface IMessageBroker :IService, IStartableComponent, IBuildableComponent
	{
		bool IsAuthenticationRequired { get; }
		bool IsConnected { get; }



		Guid SubscribeTopic<T>(string topic, string subscriptionId, Action<IMessage<T>> onNext) where T : IEntity;
		void UnSubscribeTopic(Guid externalId);

		void CacheRequest<T, Z>(string cacheTopic, ICacheRequest<T> request, Action<ICacheResponse<Z>> onNext)where T : IEntity where Z : IEntity;
		void CacheResponder<T, Z>(string topic, string name, Func<ICacheRequest<T>, ICacheResponse<Z>> getPayload) where T : IEntity where Z : IEntity;
		void SubscribeTopic<T>(string v, object onSessionRequest);
		void Publish<T>(string topic, IMessage<T> msg) where T : IEntity;
	}
}
