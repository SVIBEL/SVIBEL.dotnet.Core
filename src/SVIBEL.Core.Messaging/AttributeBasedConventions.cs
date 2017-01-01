using System;
using System.Linq;
using System.Reflection;
using EasyNetQ;
using SVIBEL.Core.Common;

namespace SVIBEL.Core.Messaging
{
	public class AttributeBasedConventions : Conventions
	{
		private readonly ITypeNameSerializer _typeNameSerializer;

		public AttributeBasedConventions(ITypeNameSerializer typeNameSerializer)
			: base(typeNameSerializer)
		{
			if (typeNameSerializer == null)
				throw new ArgumentNullException("typeNameSerializer");

			_typeNameSerializer = typeNameSerializer;

			ExchangeNamingConvention = GenerateExchangeName;
			QueueNamingConvention = GenerateQueueName;
		}

		private string GenerateExchangeName(Type messageType)
		{
			var exchangeNameAtt = messageType.GetTypeInfo().GetCustomAttributes(typeof(MessagingAttribute), true).SingleOrDefault() as MessagingAttribute;

			return (exchangeNameAtt == null)
				? _typeNameSerializer.Serialize(messageType)
				: exchangeNameAtt.ExchangeName;
		}

		private string GenerateQueueName(Type messageType, string subscriptionId)
		{
			var queueNameAtt = messageType.GetTypeInfo().GetCustomAttributes(typeof(MessagingAttribute), true).SingleOrDefault() as MessagingAttribute;

			return string.IsNullOrEmpty(subscriptionId)
				? queueNameAtt.QueueName
				: string.Concat(queueNameAtt.QueueName, "_", subscriptionId);
		}
	}
}
