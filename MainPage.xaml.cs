using MQTTnet;
using MQTTnet.Server;

namespace MQTTBrokerDemo;

public partial class MainPage : ContentPage
{
	bool started = false;

	public MainPage()
	{
		InitializeComponent();		
	}

	private async void OnAvviaTerminaBrokerClicked(object sender, EventArgs e)
	{
		if (!started)
		{
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(Convert.ToInt32(entryPorta.Text))
                .Build();

            Broker.Instance.mqttServer = new MqttFactory().CreateMqttServer(options);

			await Broker.Instance.mqttServer.StartAsync();

			started = true;

			entryStatoBroker.Text = "AVVIATO";
			buttonAvviaTerminaBroker.Text = "Termina Broker";
        }
		else
		{
			if (Broker.Instance.mqttServer != null && Broker.Instance.mqttServer.IsStarted)
			{
				await Broker.Instance.mqttServer.StopAsync();

			}

			started = false;

            entryStatoBroker.Text = "NON AVVIATO";
			buttonAvviaTerminaBroker.Text = "Avvia Broker";
        }
	}
}


