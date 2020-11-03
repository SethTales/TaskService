using System.Threading.Tasks;

namespace TaskService.Interfaces
{
    public interface ITaskService
    {
        Task Execute();
        Task Shutdown();
    }
}
