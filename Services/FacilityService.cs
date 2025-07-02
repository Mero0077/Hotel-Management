
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Hotel_Management.DTOs.Facilities;
using Hotel_Management.DTOs.Rooms;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.api.Services.FacilitiesService
{
    public class FacilityService(GeneralRepository<Facility> facilityRepository, IMapper mapper) 
    {
        private readonly GeneralRepository<Facility> _facilityRepository = facilityRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<FacilityResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var roomsResponse = await _facilityRepository.GetAll()
                .ProjectTo<FacilityResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return roomsResponse;
        }

        public async Task<ResponseVM<FacilityResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var facility = await _facilityRepository.GetOneByIdAsync(id);

            if (facility is null)
                return new FailureResponseVM<FacilityResponse>(ErrorCode.FacilityNotFound, "Facility not found");

            var facilityResponse = _mapper.Map<FacilityResponse>(facility);

            return new SuccessResponseVM<FacilityResponse>(facilityResponse, "Successful");
        }

        public async Task<ResponseVM<FacilityResponse>> AddAsync(FacilityRequest request, CancellationToken cancellationToken = default)
        {
            var isfacilityExists = await _facilityRepository.AnyAsync(rt => rt.Name == request.Name, cancellationToken);
            if (isfacilityExists)
                return new FailureResponseVM<FacilityResponse>(ErrorCode.FacilityAlreadyExists, "Facility Already Exists");

            var facility = _mapper.Map<Facility>(request);
            var addedFacility = await _facilityRepository.AddAsync(facility);
            await _facilityRepository.SaveChangesAsync();

            var response = _mapper.Map<FacilityResponse>(addedFacility);

            return new SuccessResponseVM<FacilityResponse>(response, "Successful");
        }

        public async Task<ResponseVM<FacilityResponse>> UpdateAsync(int id, FacilityRequest request, CancellationToken cancellationToken = default)
        {
            var isfacilityExists = await _facilityRepository.AnyAsync(rt => rt.Name == request.Name && rt.Id != id, cancellationToken);
            if (isfacilityExists)
                return new FailureResponseVM<FacilityResponse>(ErrorCode.FacilityAlreadyExists, "Facility Already Exists");

            var facility = await _facilityRepository.GetOneWithTrackingAsync(rt => rt.Id == id && rt.IsActive);
            if (facility is null)
                return new FailureResponseVM<FacilityResponse>(ErrorCode.FacilityNotFound, "Facility not found");

            facility.Name = request.Name;
            await _facilityRepository.SaveChangesAsync(cancellationToken);
            await _facilityRepository.SaveChangesAsync();

            var response = _mapper.Map<FacilityResponse>(facility);

            return new SuccessResponseVM<FacilityResponse>(response, "Successful");
        }

        public async Task<ResponseVM<FacilityResponse>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var facility = await _facilityRepository.Get(f => f.Id == id)
                .Select(x => new
                {
                    Facility = x,
                    x.Rooms
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (facility is null || facility.Facility is null)
                return new FailureResponseVM<FacilityResponse>(ErrorCode.FacilityNotFound, "Facility not found");

            if (facility.Rooms.Count != 0)
                return new FailureResponseVM<FacilityResponse>(ErrorCode.FacilityInUse, "Facility In Use");

            var deletedFacility = await _facilityRepository.DeleteAsync(id);
            await _facilityRepository.SaveChangesAsync();

            var response = _mapper.Map<FacilityResponse>(deletedFacility);

            return new SuccessResponseVM<FacilityResponse>(response, "Successful");
        }
    }
}
