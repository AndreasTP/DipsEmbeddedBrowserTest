using DIPS.EmbeddedBrowser.Contracts.JavaScript;
using EmbeddedBrowserTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedBrowserTest
{
    public class Proxy : IJavaScriptBindObject
    {
        private readonly MainViewModel m_updater;
        public string Name => "javaScriptProxy";

        public Proxy(MainViewModel updater)
        {
            m_updater = updater;
        }

        public void changeDocumentStatus()
        {
            m_updater.ChangeDocumentStatus();
        }
    }
}


