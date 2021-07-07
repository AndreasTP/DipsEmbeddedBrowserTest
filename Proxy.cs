using DIPS.EmbeddedBrowser.Contracts.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedBrowserTest
{
    public class Proxy : IJavaScriptBindObject
    {
        public string Name => "javaScriptProxy";
    }
}

//public class MyJavaScriptProxy : IJavaScriptBindObject
//{
//    private readonly INeedToUpdateCounter m_updater;

//    public JavaScriptProxy(INeedToUpdateCounter updater)
//    {
//        m_updater = updater;
//    }

//    public string Name => "javaScriptProxy";

//    public void updateCsharpCounter(int number)
//    {
//        m_updater.UpdateCounter(number);
//    }
//}
