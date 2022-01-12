using Chat.DesktopClient.Views;
namespace Chat.DesktopClient.ViewModels
{
    using Prism.Commands;
    using Prism.Mvvm;
    using Services;

    public class MainWindowViewModel : BindableBase
    {
        private readonly MessageService _messageService;

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public DelegateCommand SendMessageCommand { get; private set; }

        public MainWindowViewModel()
        {
            _messageService = new MessageService();
            SendMessageCommand = new DelegateCommand(SendMessage);
        }

        private void SendMessage()
        {
            _messageService.SendMessage(_message);
        }
    }

    public class MainWindowViewModel_ForGet
    {
        public void GetMsg(string message)
        {

            GetMVVM _GetMVVM = new GetMVVM();
            _GetMVVM.GetMsg(message);

        }
    }
}
