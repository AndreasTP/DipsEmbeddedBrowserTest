using DIPS.Arena.UI.Commands;
using DIPS.EmbeddedBrowser.Contracts;
using DIPS.EmbeddedBrowser.Contracts.Builders;
using System.ComponentModel;
using System.Windows.Input;

namespace EmbeddedBrowserTest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public IEmbeddedBrowserBuilder EmbeddedBrowserBuilder { get; }

        public string Title { get; }
        
        private string m_documentStatus;
        public string Url { get; set; } = "http://localhost:9000";
        public IEmbeddedBrowserViewModel EmbeddedBrowser { get; }

        public ICommand NavigateCommand { get; }

        public ICommand SendScriptCommand { get; }

        public MainViewModel(IEmbeddedBrowserBuilder embeddedBrowserBuilder)
        {
            EmbeddedBrowserBuilder = embeddedBrowserBuilder;
            Title = "EmbeddedBrowser Test App";
            DocumentStatus = "Not dirty";
            
            EmbeddedBrowser = EmbeddedBrowserBuilder.WithJavaScriptBindings(new Proxy(this)).Build();
            EmbeddedBrowser.LoadAsync(new System.Uri(Url));

            NavigateCommand = new DelegateCommand(
                async () => 
                {
                    await EmbeddedBrowser.LoadAsync(new System.Uri(Url));
                });

            SendScriptCommand = new DelegateCommand(
                  () =>
                {
                    EmbeddedBrowser.ExecuteJavascriptAsync("var comment = document.getElementById('commentBox');" +
                                           "console.log(comment.value);");
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangeDocumentStatus()
        {
            DocumentStatus = "Dirty";
        }

        public string DocumentStatus
        {
            get =>  m_documentStatus;
            set
            {
                m_documentStatus = value;
                OnPropertyChanged("DocumentStatus");
            } 
        }

        
    }
}
