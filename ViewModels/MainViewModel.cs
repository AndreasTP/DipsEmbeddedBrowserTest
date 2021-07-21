﻿using DIPS.Arena.UI.Commands;
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

namespace EmbeddedBrowserTest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public IEmbeddedBrowserBuilder EmbeddedBrowserBuilder { get; }

        public string Title { get; }
        
        private string m_documentStatus;
        private string m_documentComment;
        private SolidColorBrush m_borderColor;
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
                    var comment = await GetCommentFromWebApp();
                    DocumentComment = "Text: " + comment;
                    BorderColor = System.Windows.Media.Brushes.LawnGreen;
                    DocumentStatus = "Clean document";
                   
                    await EmbeddedBrowser.ExecuteJavascriptAsync("document.getElementById('SaveDocumentButton').click()");
                });

            ApproveCommand = new DelegateCommand(()=> ApproveDocument());
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
