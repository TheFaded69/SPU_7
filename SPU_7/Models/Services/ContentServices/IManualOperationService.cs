using System;
using System.Collections.Generic;
using SPU_7.Common.Settings;
using SPU_7.Models.Scripts.Operations.Results.Extensions;

namespace SPU_7.Models.Services.ContentServices;

public interface IManualOperationService
{
   void ShowManualValidationResultDialog(Action< List<ValidationPointResult>> positiveAction, Action< List<ValidationPointResult>> negativeAction, List<ValidationPointResult> validationPointModels, ValidationType validationType, int deviceNumber);
}