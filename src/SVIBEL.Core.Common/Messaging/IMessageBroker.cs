using System;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;
using SVIBEL.Core.Common.Messaging.Messages;

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



		Guid SubscribeTopic<T, U>(string topic, string subscriptionId, Action<IMessage<T>> onNext) where U : class, IMessage<T>;
		void UnSubscribeTopic(Guid externalId);

		void CacheRequest<T, Z, U>(string cacheTopic, ICacheRequest<T> request, Action<ICacheResponse<Z>> onNext) 
			where Z : IEntity 
			where U : class, IMessage<Z>;
		void CacheResponder<T, Z, Y, W>(string topic, string name, Func<ICacheRequest<T>, ICacheResponse<Z>> getPayload)
			where Z : IEntity
			where Y : class, IMessage<T>
			where W : class, IMessage<Z>, new();
		void SubscribeTopic<T>(string v, object onSessionRequest);
		void Publish<T>(string topic, IMessage<T> msg);
	}
}
