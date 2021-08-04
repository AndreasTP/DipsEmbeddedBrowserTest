using CefSharp;
using CefSharp.Wpf;
using DIPS.Arena.UI.Commands;
using DIPS.EmbeddedBrowser.Contracts;
using DIPS.EmbeddedBrowser.Contracts.Builders;
using DIPS.EmbeddedBrowser.Handlers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Nancy.Hosting.Self;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

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
            
            BorderColor = System.Windows.Media.Brushes.LawnGreen;
            
            EmbeddedBrowser = EmbeddedBrowserBuilder.WithJavaScriptBindings(new WebAppProxy(this)).Build();
            EmbeddedBrowser.LoadAsync(new System.Uri(Url));

            NavigateCommand = new DelegateCommand(
                async () => 
                {
                    await EmbeddedBrowser.LoadAsync(new System.Uri(Url));                   

                });

            SaveCommand = new DelegateCommand(
                async ()=>
                {
                    Console.WriteLine("heiheihei");
                    

                    await EmbeddedBrowser.ExecuteJavascriptAsync("document.getElementById('SaveDocumentButton').click()");
                    // await EmbeddedBrowser.EvaluateJavaScriptAsync("alertSomething()");
                    // var errorMessage = EmbeddedBrowser.EvaluateJavaScriptAsync("document.getElementById('SaveDocumentButton').click()");
                    var errorMessage = await EmbeddedBrowser.EvaluateJavaScriptAsync("function getError(){var error4 = document.getElementById('hiddenErrorElement'); return error4.innerText}; getError()");
                    Console.WriteLine("ERROR   " + errorMessage.Message);

                    if (errorMessage.Result == "")
                    {
                         var comment = await GetCommentFromWebApp();
                    

                                            DocumentComment = "Text: " + comment;
                                            BorderColor = System.Windows.Media.Brushes.LawnGreen;
                                            DocumentStatus = "Clean document";
                    }

                   

                  
                });

            ApproveCommand = new DelegateCommand(()=> ApproveDocument());

            //HttpListener(new[] { "http://localhost:9000/" });

           



        }

        


        //public static void HttpListener(string[] prefixes)
        //{
        //    if (prefixes == null || prefixes.Length == 0)
        //        throw new ArgumentException("Prefixes needed");

        //    HttpListener listener = new HttpListener();

        //    foreach (string s in prefixes)
        //    {
        //        listener.Prefixes.Add(s);
        //    }
        //    listener.Start();
        //    Console.WriteLine("Listening..");

        //    HttpListenerContext context = listener.GetContext();
        //    HttpListenerRequest request = context.Request;
        //    HttpListenerResponse response = context.Response;

        //    string responseString = "<HTML><BODY> Test </BODY></HTML>";
        //    byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        //    response.ContentLength64 = buffer.Length;
        //    Stream output = response.OutputStream;
        //    output.Write(buffer, 0, buffer.Length);

        //    output.Close();
        //    listener.Stop();
        //}

        //public async void UpdateErrorMessage()
        //{

        //}



        public async void SetReadOnlyStatus()
        {
            if (!readOnly)
                await EmbeddedBrowser.ExecuteJavascriptAsync("document.getElementById('ReadOnlyButton').click()");            
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
            BorderColor = System.Windows.Media.Brushes.Red;
        }

        public void UpdateErrorMessage(string error3)
        {
            Console.WriteLine(error3);

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
