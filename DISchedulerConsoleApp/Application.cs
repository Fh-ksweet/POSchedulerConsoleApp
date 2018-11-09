using DISchedulerConsoleApp.Interfaces;

namespace DISchedulerConsoleApp
{
    public class Application
    {
        private readonly ILog _logger;
        private readonly IProcessor _processor;

        public Application(ILog logger, IProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        public void Run()
        {
            _logger.Info(nameof(Application) + " started.");

            _processor.RunApp();

            _logger.Info(nameof(Application) + " finished.");
        }
    }
}
