namespace AppReservasCanchas
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && string.Equals(args[0], "--verify", StringComparison.OrdinalIgnoreCase))
            {
                Environment.ExitCode = ReservationFlowVerifier.Run();
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new ReservationForm());
        }
    }
}
