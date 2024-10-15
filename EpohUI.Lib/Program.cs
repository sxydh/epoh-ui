using System.Diagnostics;

namespace EpohUI.Lib
{
    public class Program
    {

        public static void Kill()
        {
            try
            {
                Process.GetCurrentProcess().Kill();
            }
            catch { }
        }

        public static string GetMethodIdMap()
        {
            return $"lib/program-kill={typeof(Program).FullName}#Kill";
        }

    }
}
