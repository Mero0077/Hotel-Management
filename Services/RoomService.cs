using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Hotel_Management.DTOs.Rooms;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
namespace Hotel_Management.Services
{
    public class RoomService(IMapper mapper, IWebHostEnvironment webHostEnvironment, 
            GeneralRepository<RoomType> roomTypeRepository, 
            GeneralRepository<Room> roomRepository, GeneralRepository<Facility> facilityRepository)
    {

        private readonly IMapper _mapper = mapper;
        private readonly GeneralRepository<RoomType> _roomTypeRepository = roomTypeRepository;
        private readonly GeneralRepository<Room> _roomRepository = roomRepository;
        private readonly GeneralRepository<Facility> _facilityRepository = facilityRepository;

        private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/wwwroot/roomImages";

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
        public async Task<ResponseVM<RoomResponse>> AddAsync(AddRoomRequest request, CancellationToken cancellationToken = default)
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

            List<RoomImage> images = [];
            foreach (var roomImage in request.RoomImages)
            {
                var imagePath = await UploadImageAsync(roomImage, cancellationToken);
                if (imagePath is null)
                    return new FailureResponseVM<RoomResponse>(ErrorCode.RoomImageExtensionIsNotValid, "Room Image Extension Is Not Valid");
                images.Add(new RoomImage { ImageUrl = imagePath });
            }

            var room = _mapper.Map<Room>(request);
            room.Facilities = facilities; // Assign existing facilities
            room.RoomImages = images;

            var addedRoom = await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
            var response = _mapper.Map<RoomResponse>(addedRoom);

            return new SuccessResponseVM<RoomResponse>(response, "Successful");
        }

        public async Task<ResponseVM<RoomResponse>> UpdateAsync(int id, UpdateRoomRequest request, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.Get(r => r.Id == id && r.IsActive)
                .Select(x => new
                {
                    Room = x,
                    x.RoomImages,
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (room is null || room.Room is null)
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomNotFound, "Room not found");

            if (await _roomRepository.AnyAsync(r => r.RoomNumber == request.RoomNumber && r.Id != id, cancellationToken))
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomTypeNotFound, "Room type not found");

            // it will be new if the sent room image id is 0
            var currentRoomImagesIds = room.RoomImages.Select(x => x.Id).ToHashSet();
            var sentRoomImagesIds = request.RoomImages.Where(img => img.Id != 0).Select(x => x.Id).ToHashSet();
            var invalidIds = sentRoomImagesIds.Except(currentRoomImagesIds).ToList();
            if(invalidIds.Count != 0)
                return new FailureResponseVM<RoomResponse>(ErrorCode.RoomImageNotFound, "Room Image not found");

            var idsToDelete = currentRoomImagesIds.Except(sentRoomImagesIds).ToList();
            if (idsToDelete.Count != 0)
            {
                var imagesToDelete = room.RoomImages.Where(img => idsToDelete.Contains(img.Id)).ToList();
                foreach(var image in imagesToDelete)
                {
                    await DeleteImageAsync(image.ImageUrl);
                    room.RoomImages.Remove(image);
                }
            }
            var imagesToAdd = request.RoomImages
                    .Where(img => img.Id == 0 && img.RoomImage is not null)
                    .ToList();
            if (imagesToAdd.Count != 0)
            {
                foreach(var newImage in imagesToAdd)
                {
                    var relativePath = await UploadImageAsync(newImage.RoomImage, cancellationToken);
                    if (relativePath is null)
                        return new FailureResponseVM<RoomResponse>(ErrorCode.RoomImageExtensionIsNotValid, "Room Image Extension Is Not Valid");
                    room.RoomImages.Add(new RoomImage { ImageUrl = relativePath });
                }
            }

            _mapper.Map(request, room);

            var facilities = await _facilityRepository.Get(f => request.FacilityIds.Contains(f.Id)).ToListAsync(cancellationToken);
            if (facilities.Count != request.FacilityIds.Count)
                return new FailureResponseVM<RoomResponse>(ErrorCode.FacilityNotFound, "Facility type not found");
            room.Room.Facilities = facilities;

            await _roomRepository.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<RoomResponse>(room);
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
            await _roomRepository.SaveChangesAsync();
            var response = _mapper.Map<RoomResponse>(deletedRoom);
            return new SuccessResponseVM<RoomResponse>(response, "Successful");
        }

        private async Task<string?> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
        {
            // --- Validation ---
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                throw null; //Invalid file type.
            }

            // --- File Processing ---

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var relativePath = Path.Combine("roomImages", uniqueFileName);

            var AbsolutePath = Path.Combine(_imagesPath, uniqueFileName);

            using var stream = File.Create(AbsolutePath);
            await image.CopyToAsync(stream, cancellationToken);

            return relativePath;
        }

        private async Task DeleteImageAsync(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var absolutePath = Path.Combine(_imagesPath, relativePath);

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }

            await Task.CompletedTask;
        }
    }
}
