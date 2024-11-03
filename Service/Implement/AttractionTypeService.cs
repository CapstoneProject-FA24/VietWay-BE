﻿using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class AttractionTypeService(IUnitOfWork unitOfWork) : IAttractionTypeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<AttractionCategoryPreviewDTO>> GetAllAttractionType()
        {
            return await _unitOfWork.AttractionCategoryRepository
                .Query()
                .Select(x => new AttractionCategoryPreviewDTO { }).ToListAsync();
        }
    }
}
