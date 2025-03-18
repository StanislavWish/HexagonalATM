using Application.Infrastructure;

namespace Application.Core.Operations;

public record OperationInfo(int OperationId, int UserId, decimal Amount, OperationType OperationType);