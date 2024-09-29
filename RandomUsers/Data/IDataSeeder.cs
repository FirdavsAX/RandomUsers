using RandomUsers.Models;

namespace RandomUsers.Data
{
    public interface IDataSeeder
    {
        List<User> GenerateUsers(string region, int count, int seed, int errorsPerRecord);
    }
}
