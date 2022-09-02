using System;
using MQTTnet;
using MQTTnet.Server;

namespace MQTTBrokerDemo
{
	public class Broker
	{
		private static Broker instance = null;
		private static readonly object padlock = new object();

		public MqttServer mqttServer;

		public Broker()
		{
			
		}

		public static Broker Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new Broker();
					}
					return instance;
				}
			}
		}

		
	}
}

