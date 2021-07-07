using DIPS.Arena.UI.Commands;
using DIPS.EmbeddedBrowser.Contracts;
using DIPS.EmbeddedBrowser.Contracts.Builders;
using System.Windows.Input;

namespace EmbeddedBrowserTest.ViewModels
{
    public class MainViewModel
    {
        public IEmbeddedBrowserBuilder EmbeddedBrowserBuilder { get; }

        public string Title { get; }
        public string Url { get; set; } = "http://localhost:9000";
        public IEmbeddedBrowserViewModel EmbeddedBrowser { get; }

        public ICommand NavigateCommand { get; }

        public ICommand SaveCommand { get; }

        public MainViewModel(IEmbeddedBrowserBuilder embeddedBrowserBuilder)
        {
            EmbeddedBrowserBuilder = embeddedBrowserBuilder;
            Title = "EmbeddedBrowser Test App";

            EmbeddedBrowser = EmbeddedBrowserBuilder.WithJavaScriptBindings(new Proxy()).Build();
            EmbeddedBrowser.LoadAsync(new System.Uri(Url));

            NavigateCommand = new DelegateCommand(
                async () => 
                {
                    await EmbeddedBrowser.LoadAsync(new System.Uri(Url));
                    

                });

            SaveCommand = new DelegateCommand(()=>
            {
                EmbeddedBrowser.ExecuteJavascriptAsync("var comment = document.getElementById('commentBox');" +
                                           "console.log(comment.value);");
            });
        }
    }
}
