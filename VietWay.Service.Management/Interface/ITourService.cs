﻿using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourService
    {
        public Task<string> CreateTour(Tour tour, string staffId);
        public Task EditTour(string tourId, Tour updatedTour, string accountId);
        public Task<(int totalCount, List<TourPreviewDTO> items)> GetAllTour(string? nameSearch, string? codeSearch, List<string>? provinceIds, List<string>? tourCategoryIds,List<string>? durationIds, TourStatus? status, int pageSize, int pageIndex,DateTime? startDateFrom, DateTime? startDateTo);
        public Task<TourDetailDTO?> GetTourById(string id);
        public Task<(int totalCount, List<Tour> items)> GetAllScheduledTour(int pageSize, int pageIndex);
        public Task<List<TourDetailDTO>> GetAllToursByTemplateIdsAsync(string tourTemplateIds);
        public Task ChangeTourStatusAsync(string tourId, string accountId, TourStatus tourStatus, string? reason);
        public Task CancelTourAsync(string tourId, string managerId, string? reason);
        public Task DeleteTourAsync(string tourId, string accountId);
    }
}
