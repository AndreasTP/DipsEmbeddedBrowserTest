using DIPS.Arena.UI.Commands;
using DIPS.EmbeddedBrowser.Contracts;
using DIPS.EmbeddedBrowser.Contracts.Builders;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.ComponentModel;
using System.Drawing;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Net;
using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace EmbeddedBrowserTest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public IEmbeddedBrowserBuilder EmbeddedBrowserBuilder { get; }

        public string Title { get; }
        
        private string m_documentStatus;
        private string m_documentComment;
        private SolidColorBrush m_borderColor;
        private bool readOnly = false;
        private bool m_enableApproving;
        public string Url { get; set; } = "http://localhost:9000";

        public IEmbeddedBrowserViewModel EmbeddedBrowser { get; }

        public ICommand NavigateCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand ApproveCommand { get; }

        public MainViewModel(IEmbeddedBrowserBuilder embeddedBrowserBuilder)
        {
            EmbeddedBrowserBuilder = embeddedBrowserBuilder;
            Title = "EmbeddedBrowser Test App";
            DocumentStatus = "Clean document";
            DocumentComment = "Text: ";

            EnableApproving = false;

            BorderColor = System.Windows.Media.Brushes.LawnGreen;
            
            EmbeddedBrowser = EmbeddedBrowserBuilder.WithJavaScriptBindings(new WebAppProxy(this)).Build();
            EmbeddedBrowser.LoadAsync(new Uri(Url));

            NavigateCommand = new DelegateCommand(
                async () => 
                {
                    await EmbeddedBrowser.LoadAsync(new System.Uri(Url));                   

                });

            SaveCommand = new DelegateCommand(
                async ()=>
                {
                    var comment = await GetCommentFromWebApp();
                    DocumentComment = "Text: " + comment;
                    BorderColor = System.Windows.Media.Brushes.LawnGreen;
                    DocumentStatus = "Clean document";

                    var res = await EmbeddedBrowser.EvaluateJavaScriptAsync("dipsExtensions.save()");
                    //TODO use EvaluateJavaScriptAsPromiseAsync when CefSharp and EmbeddedBrowser is updated to > 8.6

                    if (res.Result == null)
                    {
                        Console.WriteLine("Response is currently not available, TODO use EvaluateJavaScriptAsPromiseAsync when CefSharp and EmbeddedBrowser is updated to > 8.6");
                    }
                    else
                    {
                        Console.WriteLine("/////////////" + res.Result);
                    }
                });

            ApproveCommand = new DelegateCommand(()=> ApproveDocument());
        }

        public async void LoadBrowser()
        {
            await EmbeddedBrowser.LoadAsync(new Uri(Url));
            if (readOnly)
                await EmbeddedBrowser.ExecuteJavascriptAsync("dipsExtensions.setReadOnlyStatus(true);");
            else
                await EmbeddedBrowser.ExecuteJavascriptAsync("dipsExtensions.setReadOnlyStatus(false);");

            //if (!readOnly)
            //    await EmbeddedBrowser.ExecuteJavascriptAsync("tester.setReadOnlyStatus();");
        }

        public void EnableApproveDocument()
        {
            EnableApproving = true;
        }

        public async void readOnlyUpdater()
        {
            if (readOnly)
                await EmbeddedBrowser.ExecuteJavascriptAsync("dipsExtensions.readOnlyUpdate(true);");
            else
                await EmbeddedBrowser.ExecuteJavascriptAsync("dipsExtensions.readOnlyUpdate(false);");                      
        }

        private async void ApproveDocument()
        {
            var comment = await GetCommentFromWebApp();
            DocumentComment = "Text: " + comment;
            BorderColor = System.Windows.Media.Brushes.Yellow;
            DocumentStatus = "Approved document";
        }

        public void UpdateDocumentStatus()
        {
            DocumentStatus = "Dirty document";
            EnableApproving = false;
            BorderColor = System.Windows.Media.Brushes.Red;
        }

        public void UpdateErrorMessage(string error3)
        {
            Console.WriteLine(error3);
            JObject json = JObject.Parse(error3);
            Console.WriteLine(json);           
        }

        private async Task<string> GetCommentFromWebApp()
        {
            var s = await EmbeddedBrowser.EvaluateJavaScriptAsync("function foo() {" +
                                                            "var comment = document.getElementById('commentBox');" +
                                                            "return comment.value; }" +
                                                            "foo();");
            return s?.Result?.ToString();
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

        public bool EnableApproving
        {
            get => m_enableApproving;
            set
            {
                m_enableApproving = value;
                OnPropertyChanged("EnableApproving");
            }
        }

        public string DocumentComment
        {
            get => m_documentComment;
            set
            {
                m_documentComment = value;
                OnPropertyChanged("DocumentComment");
            }
        }

        public SolidColorBrush BorderColor
        {
            get => m_borderColor;
            set
            {
                m_borderColor = value;
                OnPropertyChanged("BorderColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }  
    }
}
