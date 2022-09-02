using System.Collections.ObjectModel;

namespace MQTTBrokerDemo;

public class Message
{
    public string Payload { get; set; }
    public string ClientInfo { get; set; }
}

public partial class MessaggiPage : ContentPage
{
    private ObservableCollection<Message> _messages { get; set; }
    public ObservableCollection<Message> Messages
    {
        get { return _messages; }
        set
        {
            _messages = value;
            OnPropertyChanged(nameof(Messages));
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Broker.Instance.mqttServer != null)
        {
            Broker.Instance.mqttServer.InterceptingPublishAsync += MqttServer_InterceptingPublishAsync;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (Broker.Instance.mqttServer != null)
        {
            Broker.Instance.mqttServer.InterceptingPublishAsync -= MqttServer_InterceptingPublishAsync;
        }
    }

    private Task MqttServer_InterceptingPublishAsync(MQTTnet.Server.InterceptingPublishEventArgs arg)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Insert(0, new Message()
            {
                Payload = System.Text.Encoding.Default.GetString(arg.ApplicationMessage.Payload),
                ClientInfo = $"{arg.ClientId} - Topic: {arg.ApplicationMessage.Topic}"
            });
        });

        return Task.CompletedTask;
    }

    public MessaggiPage()
	{
		InitializeComponent();

        Messages = new ObservableCollection<Message>();

        BindingContext = this;
	}
}
