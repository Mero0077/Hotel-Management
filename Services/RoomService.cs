//using HotelReservationSystem.api.Contracts.Rooms;

//namespace HotelReservationSystem.api.Services.RoomsService
//{
//    public class RoomService(IGeneralRepository<Room> roomRepository,
//            IGeneralRepository<RoomType> roomTypeRepository,
//            IGeneralRepository<Facility> facilityRepository) : IRoomService
//    {
//        private readonly IGeneralRepository<Room> _roomRepository = roomRepository;
//        private readonly IGeneralRepository<RoomType> _roomTypeRepository = roomTypeRepository;
//        private readonly IGeneralRepository<Facility> _facilityRepository = facilityRepository;

//        public async Task<IEnumerable<RoomResponse>> GetAllAsync(CancellationToken cancellationToken = default)
//        {
//            return await _roomRepository.Get(r => r.IsActive)
//                .Include(r => r.RoomType)
//                .Include(r => r.Facilities)
//                .ProjectToType<RoomResponse>()
//                .ToListAsync(cancellationToken);
//        }

//        public async Task<Result<RoomResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
//        {
//            var room = await _roomRepository.Get(r => r.Id == id && r.IsActive)
//                .Include(r => r.RoomType)
//                .Include(r => r.Facilities)
//                .ProjectToType<RoomResponse>()
//                .FirstOrDefaultAsync(cancellationToken);

//            return room is null
//                ? Result.Failure<RoomResponse>(RoomErrors.NotFound)
//                : Result.Success(room);
//        }

//        public async Task<Result<RoomResponse>> AddAsync(RoomRequest request, CancellationToken cancellationToken = default)
//        {
//            if (await _roomRepository.AnyAsync(r => r.RoomNumber == request.RoomNumber, cancellationToken))
//                return Result.Failure<RoomResponse>(RoomErrors.AlreadyExists);

//            if (!await _roomTypeRepository.AnyAsync(rt => rt.Id == request.RoomTypeId, cancellationToken))
//                return Result.Failure<RoomResponse>(RoomTypeErrors.NotFound);

//            var facilities = await _facilityRepository.Get(f => request.FacilityIds.Contains(f.Id))
//                    .AsTracking()
//                    .ToListAsync(cancellationToken);
//            if (facilities.Count != request.FacilityIds.Count)
//                return Result.Failure<RoomResponse>(FacilityErrors.NotFound);

//            var room = request.Adapt<Room>();
//            room.Facilities = facilities; // Assign existing facilities

//            var addedRoom = await _roomRepository.AddAsync(room);
//            var response = addedRoom.Adapt<RoomResponse>();

//            return Result.Success(response);
//        }

//        public async Task<Result> UpdateAsync(int id, RoomRequest request, CancellationToken cancellationToken = default)
//        {
//            var room = await _roomRepository.Get(r => r.Id == id && r.IsActive)
//                .AsTracking()
//                .Include(r => r.Facilities)
//                .FirstOrDefaultAsync(cancellationToken);

//            if (room is null)
//                return Result.Failure(RoomErrors.NotFound);

//            if (await _roomRepository.AnyAsync(r => r.RoomNumber == request.RoomNumber && r.Id != id, cancellationToken))
//                return Result.Failure(RoomErrors.AlreadyExists);

//            // room = request.Adapt(room);
//            request.Adapt(room);

//            var facilities = await _facilityRepository.Get(f => request.FacilityIds.Contains(f.Id)).ToListAsync(cancellationToken);
//            if (facilities.Count != request.FacilityIds.Count)
//            {
//                return Result.Failure(FacilityErrors.NotFound);
//            }
//            room.Facilities = facilities;

//            await _roomRepository.SaveChangesAsync(cancellationToken);
//            return Result.Success();
//        }

//        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
//        {
//            var room = await _roomRepository.Get(r => r.Id == id)
//                .Include(r => r.Reservations)
//                .FirstOrDefaultAsync(cancellationToken);

//            if (room is null)
//                return Result.Failure(RoomErrors.NotFound);

//            if (room.Reservations.Count != 0)
//                return Result.Failure(RoomErrors.InUse);

//            room.IsActive = false;
//            await _roomRepository.SaveChangesAsync(cancellationToken);
//            return Result.Success();
//        }
//    }
//}
