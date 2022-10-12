using System.Runtime.InteropServices;

namespace PGS.Print
{
    [ComVisible(true)]
    public interface ICOMPrinter
    {
        void Abort();
        void AddPage(int templateID, string data);
        void Config();
        void doPrint();
    }
}