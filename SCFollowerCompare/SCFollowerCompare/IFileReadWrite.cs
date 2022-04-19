using System.Threading.Tasks;

namespace SCFollowerCompare
{
    public interface IFileReadWrite
    {
        Task<bool> WriteToFile(string text);
        Task<string> ReadFromFile();
    }
}
