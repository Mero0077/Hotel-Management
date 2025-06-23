//using Hotel_Management.DTOs.Reservation
//using Mapster;
//using Microsoft.EntityFrameworkCore;

//namespace HotelReservationSystem.api.Services.FacilitiesService
//{
//    public class FacilityService(IGeneralRepository<Facility> facilityRepository) : IFacilityService
//    {
//        private readonly IGeneralRepository<Facility> _facilityRepository = facilityRepository;

//        public async Task<IEnumerable<FacilityResponse>> GetAllAsync(CancellationToken cancellationToken = default)
//        {
//            var facilities = await _facilityRepository.Get(f => f.IsActive)
//                        .ToListAsync(cancellationToken);

//            return facilities.Adapt<IEnumerable<FacilityResponse>>();
//        }

//        public async Task<Result<FacilityResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
//        {
//            var facility = await _facilityRepository.Get(f => f.Id == id && f.IsActive)
//                    .SingleOrDefaultAsync(cancellationToken);
//            if (facility is null)
//                return Result.Failure<FacilityResponse>(FacilityErrors.NotFound);

//            var response = facility.Adapt<FacilityResponse>();
//            return Result.Success(response);
//        }

//        public async Task<Result<FacilityResponse>> AddAsync(FacilityRequest request, CancellationToken cancellationToken = default)
//        {
//            var isfacilityExists = await _facilityRepository.AnyAsync(rt => rt.Name == request.Name, cancellationToken);
//            if (isfacilityExists)
//                return Result.Failure<FacilityResponse>(FacilityErrors.AlreadyExists);

//            var facility = request.Adapt<Facility>();
//            var addedFacility = await _facilityRepository.AddAsync(facility);

//            return Result.Success(addedFacility.Adapt<FacilityResponse>());
//        }

//        public async Task<Result> UpdateAsync(int id, FacilityRequest request, CancellationToken cancellationToken = default)
//        {
//            var isfacilityExists = await _facilityRepository.AnyAsync(rt => rt.Name == request.Name && rt.Id != id, cancellationToken);
//            if (isfacilityExists)
//                return Result.Failure(FacilityErrors.AlreadyExists);

//            var facility = await _facilityRepository.Get(rt => rt.Id == id && rt.IsActive)
//                    .SingleOrDefaultAsync(cancellationToken);
//            if (facility is null)
//                return Result.Failure(FacilityErrors.NotFound);

//            await _facilityRepository.UpdateAsync(x => x.Id == id,
//                s => s
//                    .SetProperty(y => y.Name, request.Name)

//                );

//            return Result.Success();
//        }

//        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
//        {
//            var facility = await _facilityRepository.Get(f => f.Id == id)
//                .Include(f => f.Rooms)
//                .FirstOrDefaultAsync(cancellationToken);

//            if (facility is null)
//                return Result.Failure(FacilityErrors.NotFound);

//            if (facility.Rooms.Count != 0)
//                return Result.Failure(FacilityErrors.InUse);

//            var isDeleted = await _facilityRepository.DeleteAsync(id);
//            if (!isDeleted)
//                return Result.Failure(RoomTypeErrors.NotFound);

//            return Result.Success();
//        }
//    }
//}
