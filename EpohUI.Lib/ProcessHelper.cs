using System.Diagnostics;

namespace EpohUI.Lib
{
    public class ProcessHelper
    {

        public static void Kill(string _)
        {
            try
            {
                Process.GetCurrentProcess().Kill();
            }
            catch { }
        }

        public static string GetMethodIdMap()
        {
            return $"lib/program-kill={typeof(ProcessHelper).FullName}#Kill";
        }

    }
}
