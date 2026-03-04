namespace ReportesdeInformación
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.ThreadException += (s, e) =>
            {
                try { MessageBox.Show($"Excepción en hilo de UI: {e.Exception}", "Error no controlado", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                catch { }
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                try { MessageBox.Show($"Excepción no controlada de dominio: {e.ExceptionObject}", "Error no controlado", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                catch { }
            };

            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                try { MessageBox.Show($"Excepción en tarea no observada: {e.Exception}", "Error no controlado", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                catch { }
            };

            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fallo al iniciar la aplicación: {ex}", "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}