namespace EpohUI.Lib
{
    public class HelloWorld
    {

        public static string SayHello(string name)
        {
            return $"Hello {name}";
        }

        public static string GetMethodIdMap()
        {
            return $"hello-world={typeof(HelloWorld).FullName}#SayHello";
        }

    }
}
