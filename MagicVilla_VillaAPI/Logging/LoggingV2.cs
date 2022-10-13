namespace MagicVilla_VillaAPI.Logging
{
    public class LoggingV2 : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("ERROR - " + message);
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else if (type == "warning")
            {
                Console.WriteLine("WARNING - " + message);
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.WriteLine(message);
            }
        }

    }
}
