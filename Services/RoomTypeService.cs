using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Hotel_Management.DTOs.Facilities;
using Hotel_Management.DTOs.RoomTypes;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class RoomTypeService(GeneralRepository<RoomType> roomTypeRepository,
        GeneralRepository<Room> roomRepository, IMapper mapper)
    {
        private readonly GeneralRepository<RoomType> _roomTypeRepository = roomTypeRepository;
        private readonly GeneralRepository<Room> _roomRepository = roomRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<RoomTypeResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var roomTypesResponse = await _roomTypeRepository.Get(rt => rt.IsActive)
                .ProjectTo<RoomTypeResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return roomTypesResponse;
        }

        public async Task<ResponseVM<RoomTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var roomType = await _roomTypeRepository.Get(rt => rt.Id == id && rt.IsActive)
                    .SingleOrDefaultAsync(cancellationToken);
            if (roomType is null)
                return new FailureResponseVM<RoomTypeResponse>(ErrorCode.RoomTypeNotFound, "RoomType not found");

            var response = _mapper.Map<RoomTypeResponse>(roomType);
            return new SuccessResponseVM<RoomTypeResponse>(response, "Successful");
        }

        public async Task<ResponseVM<RoomTypeResponse>> AddAsync(RoomTypeRequest request, CancellationToken cancellationToken = default)
        {
            var isRoomTypeExists = await _roomTypeRepository.AnyAsync(rt => rt.Name == request.Name, cancellationToken);
            if (isRoomTypeExists)
                return new FailureResponseVM<RoomTypeResponse>(ErrorCode.RoomTypeAlreadyExists, "RoomType Already Exists");

            var roomType = _mapper.Map<RoomType>(request);
            var addedRoomType = await _roomTypeRepository.AddAsync(roomType);
            await _roomRepository.SaveChangesAsync();

            var response = _mapper.Map<RoomTypeResponse>(addedRoomType);
            return new SuccessResponseVM<RoomTypeResponse>(response, "Successful");
        }

        public async Task<ResponseVM<RoomTypeResponse>> UpdateAsync(int id, RoomTypeRequest request, CancellationToken cancellationToken = default)
        {
            var isRoomTypeExists = await _roomTypeRepository.AnyAsync(rt => rt.Name == request.Name && rt.Id != id, cancellationToken);
            if (isRoomTypeExists)
                return new FailureResponseVM<RoomTypeResponse>(ErrorCode.RoomTypeAlreadyExists, "RoomType Already Exists");

            var roomType = await _roomTypeRepository.GetOneWithTrackingAsync(rt => rt.Id == id && rt.IsActive);
            if (roomType is null)
                return new FailureResponseVM<RoomTypeResponse>(ErrorCode.RoomTypeNotFound, "RoomType not found");

            _mapper.Map(request, roomType);
            var addedRoomType = await _roomTypeRepository.SaveChangesAsync(cancellationToken);
            await _roomTypeRepository.SaveChangesAsync();

            var response = _mapper.Map<RoomTypeResponse>(addedRoomType);
            return new SuccessResponseVM<RoomTypeResponse>(response, "Successful");
        }

        public async Task<ResponseVM<RoomTypeResponse>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var isRoomTypeInUse = await _roomRepository.AnyAsync(r => r.RoomTypeId == id, cancellationToken);
            if (isRoomTypeInUse)
                return new FailureResponseVM<RoomTypeResponse>(ErrorCode.RoomTypeInUse, "RoomType In Use");

            var roomType = await _roomTypeRepository.GetOneWithTrackingAsync(x => x.Id == id && !x.IsDeleted);
            if (roomType is null)
                return new FailureResponseVM<RoomTypeResponse>(ErrorCode.RoomTypeNotFound, "RoomType not found");

            roomType.IsDeleted = true;
            await _roomRepository.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<RoomTypeResponse>(roomType);
            return new SuccessResponseVM<RoomTypeResponse>(response, "Successful");
        }

    }
}
