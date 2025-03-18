using Application.Core.Operations;
using Application.Core.Users;
using System.Collections.ObjectModel;

namespace Application.Ports;

public interface IOperationRepository
{
    Collection<OperationInfo> GetByUser(User account);

    IEnumerable<OperationInfo> GetAllOperations();

    void AddOperation(OperationInfo operation);
}