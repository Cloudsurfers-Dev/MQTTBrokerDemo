using System.Collections.ObjectModel;

namespace MQTTBrokerDemo;

public class Client
{
	public string Nome { get; set; }
}

public partial class ClientPage : ContentPage
{
    private ObservableCollection<Client> _clientList { get; set; }
    public ObservableCollection<Client> ClientList {
        get { return _clientList; }
        set
        {
            _clientList = value;
            OnPropertyChanged(nameof(ClientList));
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Broker.Instance.mqttServer != null)
        {
            Broker.Instance.mqttServer.ClientConnectedAsync += MqttServer_ClientConnectedAsync;
            Broker.Instance.mqttServer.ClientDisconnectedAsync += MqttServer_ClientDisconnectedAsync;

            UpdateList();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (Broker.Instance.mqttServer != null)
        {
            Broker.Instance.mqttServer.ClientConnectedAsync -= MqttServer_ClientConnectedAsync;
            Broker.Instance.mqttServer.ClientDisconnectedAsync -= MqttServer_ClientDisconnectedAsync;
        }
    }

    private void UpdateList()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ClientList.Clear();

            var clients = Broker.Instance.mqttServer.GetClientsAsync().GetAwaiter().GetResult();
            foreach (var client in clients)
            {
                ClientList.Add(new Client()
                {
                    Nome = client.Id
                });
            }
        });
    }

    private Task MqttServer_ClientDisconnectedAsync(MQTTnet.Server.ClientDisconnectedEventArgs arg)
    {
        UpdateList();

        return Task.CompletedTask;
    }

    private Task MqttServer_ClientConnectedAsync(MQTTnet.Server.ClientConnectedEventArgs arg)
    {
        UpdateList();

        return Task.CompletedTask;
    }

    public ClientPage()
	{
		InitializeComponent();

        ClientList = new ObservableCollection<Client>();

        BindingContext = this;
	}
}
