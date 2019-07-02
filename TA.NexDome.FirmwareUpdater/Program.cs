using System.Threading.Tasks;

namespace TA.NexDome.FirmwareUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
        var app = new FirmwareUpdaterApp(args);
        await app.Run();
        }
    }
}
