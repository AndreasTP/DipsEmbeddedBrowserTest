using DIPS.EmbeddedBrowser.Contracts.JavaScript;
using EmbeddedBrowserTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedBrowserTest
{
    public class WebAppProxy : IJavaScriptBindObject
    {
        private readonly MainViewModel m_updater;
        public string Name => "webAppProxy";

        public WebAppProxy(MainViewModel updater)
        {
            m_updater = updater;
        }

        public void updateDocumentStatus()
        {
            m_updater.UpdateDocumentStatus();
        }

        public void updateReadOnlyStatus()
        {
            m_updater.SetReadOnlyStatus();
        }

        
    }
}


