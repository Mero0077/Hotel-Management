using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Hotel_Management.DTOs.Rooms;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Hotel_Management.Services
{
    public class RoomService
    {

        private IMapper _mapper;
        private GeneralRepository<RoomType> _roomTypeRepository;
        private GeneralRepository<Room> _roomRepository;
        private GeneralRepository<Facility> _facilityRepository;
        public RoomService(IMapper mapper)
        {
            _roomRepository = new GeneralRepository<Room>();
            _roomTypeRepository = new GeneralRepository<RoomType>();
            _facilityRepository = new GeneralRepository<Facility>();
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var roomsResponse = await _roomRepository.GetAll()
                .ProjectTo<RoomResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return roomsResponse;
        }

        public async Task<ResponseVM<RoomResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.GetOneByIdAsync(id);

            if (room is null)
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomNotFound, "Room not found");

            var roomResponse = _mapper.Map<RoomResponse>(room);

            return new SuccessResponseVM<RoomResponse>(roomResponse, "Successful");
        }

        //ERROR::Cannot insert explicit value for identity column in table 'Facilities' when IDENTITY_INSERT is set to OFF.
        public async Task<ResponseVM<RoomResponse>> AddAsync(RoomRequest request, CancellationToken cancellationToken = default)
        {
            if (await _roomRepository.AnyAsync(r => r.RoomNumber == request.RoomNumber, cancellationToken))
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomAlreadyExists, "Room already exists");

            if (!await _roomTypeRepository.AnyAsync(rt => rt.Id == request.RoomTypeId, cancellationToken))
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomTypeNotFound, "Room type not found");

            var facilities = await _facilityRepository.Get(f => request.FacilityIds.Contains(f.Id))
                    .AsTracking()
                    .ToListAsync(cancellationToken);
            if (facilities.Count != request.FacilityIds.Count)
                return new FailureResponseVM<RoomResponse>(ErrorCode.FacilityNotFound, "Facility type not found");

            var room = _mapper.Map<Room>(request);
            room.Facilities = facilities; // Assign existing facilities

            var addedRoom = await _roomRepository.AddAsync(room);
            var response = _mapper.Map<RoomResponse>(addedRoom);

            return new SuccessResponseVM<RoomResponse>(response, "Successful");
        }

        public async Task<ResponseVM<RoomResponse>> UpdateAsync(int id, RoomRequest request, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.GetOneWithTrackingAsync(r => r.Id == id && r.IsActive);

            if (room is null)
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomNotFound, "Room not found");

            if (await _roomRepository.AnyAsync(r => r.RoomNumber == request.RoomNumber && r.Id != id, cancellationToken))
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomTypeNotFound, "Room type not found");

            // room = request.Adapt(room);
            _mapper.Map(request, room);

            var facilities = await _facilityRepository.Get(f => request.FacilityIds.Contains(f.Id)).ToListAsync(cancellationToken);
            if (facilities.Count != request.FacilityIds.Count)
                return new FailureResponseVM<RoomResponse>(ErrorCode.FacilityNotFound, "Facility type not found");
            room.Facilities = facilities;

            var response = _mapper.Map<RoomResponse>(room);
            await _roomRepository.SaveChangesAsync(cancellationToken);
            return new SuccessResponseVM<RoomResponse>(response, "Successful");
        }

        public async Task<ResponseVM<RoomResponse>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.Get(r => r.Id == id)
                .Select(x => new
                {
                    Room = x,
                    x.Reservations
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (room is null || room.Room is null)
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomNotFound, "Room not found");

            if (room.Reservations.Count != 0)
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomBooked, "Room is booked");

            var deletedRoom = _roomRepository.DeleteAsync(id);
            var response = _mapper.Map<RoomResponse>(deletedRoom);
            return new SuccessResponseVM<RoomResponse>(response, "Successful");
        }
    }
}
