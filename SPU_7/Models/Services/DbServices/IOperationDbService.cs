using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;

namespace SPU_7.Models.Services.DbServices;

public interface IOperationDbService
{
    void AddOperation(OperationViewModel operation);
    Task AddOperationAsync(OperationViewModel operation);
    void UpdateOperation(OperationViewModel operation);
    Task UpdateOperationAsync(OperationViewModel operation);
    List<OperationViewModel> GetOperations(Guid scriptId);
    Task<List<OperationViewModel>> GetOperationsAsync(Guid scriptId);
    void DeleteOperation(Guid id);
    Task DeleteOperationAsync(Guid id);
}