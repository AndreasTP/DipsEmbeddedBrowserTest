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

        //Sets document to readonly/not readOnly
        public void readOnlyUpdater()
        {
            m_updater.readOnlyUpdater();
        }

        //Temporary: Gets error-message from webapp
        public void updateErrorMessage(string error2)
        {
            m_updater.UpdateErrorMessage(error2);
        }


    }
}


