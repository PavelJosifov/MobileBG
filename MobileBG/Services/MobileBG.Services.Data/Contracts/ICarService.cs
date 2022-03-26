﻿namespace MobileBG.Services.Data.Contracts;

using MobileBG.Web.ViewModels.Cars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICarService
{
    public Task<ICollection<DropdownDataViewModel>> GetAllMakesAsync();

    public Task<ICollection<DropdownDataViewModel>> GetAllPetrolTypesAsync();

    public Task<ICollection<DropdownDataViewModel>> GetAllCitiesAsync();

    public Task<Guid> CreateCarAsync(
        CreateCarInputViewModel model,
        string userId);
}
