namespace Service;

using Bison;
using SimpleDB;

public interface IUserAdditionService<T> where T : UserAddition
{
    IDatabaseRepository<Cheep> observations { get; }
    void AddUserAddition(T ua);

}
