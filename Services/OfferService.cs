using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hotel_Management.DTOs.Offers;
using Hotel_Management.Exceptions;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Models.ViewModels.Offers;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class OfferService
    {
        private readonly IMapper _mapper;
        private readonly GeneralRepository<Offer> _OfferRepository;
        private readonly GeneralRepository<Room> _RoomRepository;
        private readonly GeneralRepository<RoomOffer> _RoomOfferRepository;
        public OfferService(IMapper mapper, GeneralRepository<Offer> OfferRepository, GeneralRepository<Room> RoomRepository, GeneralRepository<RoomOffer> RoomOfferRepository) 
        {
            this._mapper = mapper;
            _OfferRepository = OfferRepository;
            _RoomRepository = RoomRepository;
            _RoomOfferRepository = RoomOfferRepository;
        }

        public async Task<IEnumerable<GetOffersVM>> GetAvaliableOffersAsync()
        {
            var response = _RoomOfferRepository.GetAll().Where(e=>e.Offer.StartDate <= DateTime.Today && e.Offer.EndDate >= DateTime.Today).ProjectTo<GetOffersVM>(_mapper.ConfigurationProvider);
            if(!await response.AnyAsync())
                throw new NotFoundException("No Offers Avaliable Yet",ErrorCode.OfferNotFound);
            return await response.ToListAsync();
        }
        public async Task<Offer?> IsOfferExistsAsync(int offerId)
        {
            var offer = await _OfferRepository.GetOneWithTrackingAsync(e=>e.Id==offerId);
            if (offer == null) return null;
            return offer;
        }

        private ResponseVM<T>? ValidateOfferDates<T>(DateTime startDate,DateTime endDate)
        {
            if (startDate < DateTime.Now)
                return new FailureResponseVM<T>(ErrorCode.StartDateAlreadyExceed);

            if (startDate > endDate)
                return new FailureResponseVM<T>(ErrorCode.StartDateMustBeBeforeEndDate);
            return null;
        }

        public async Task<ResponseVM<CreateOfferRequestDto>> CreateOfferAsync(CreateOfferRequestDto requestDto)
        {

            var dateValidationResult = ValidateOfferDates<CreateOfferRequestDto>(requestDto.StartDate, requestDto.EndDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            var offer = _mapper.Map<Offer>(requestDto);
            await _OfferRepository.AddAsync(offer);

            foreach (var roomId in requestDto.RoomIds)
            {
                //Need To Modify It and Call The RoomService Instead of using _RoomRepo
                var room = await _RoomRepository.GetOneByIdAsync(roomId);
                if(room == null)  return new FailureResponseVM<CreateOfferRequestDto>(ErrorCode.RoomNotFound);

                await _RoomOfferRepository.AddAsync(new RoomOffer
                {
                    RoomId = roomId,
                    OfferId = offer.Id,
                    IsActive = requestDto.IsActive,
                });
            }
            await _OfferRepository.SaveChangesAsync();
            
            return new SuccessResponseVM<CreateOfferRequestDto>(requestDto);
        }


        public async Task<ResponseVM<EditOfferRequestDto>> EditOfferAsync(int offerId, EditOfferRequestDto requestDto)
        {
            var offer = await IsOfferExistsAsync(offerId);
            if (offer == null)
                return new FailureResponseVM<EditOfferRequestDto>(ErrorCode.OfferNotFound);

            var dateValidationResult = ValidateOfferDates<EditOfferRequestDto>(requestDto.StartDate, requestDto.EndDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            if (!requestDto.RoomIds.Any())
                return new FailureResponseVM<EditOfferRequestDto>(ErrorCode.ThereShouldBeAtLeastOneRoom);

            var oldRoomIds = _RoomOfferRepository.Get(e=>e.OfferId == offer.Id && !e.IsDeleted);
            foreach (var oldRoomId in oldRoomIds)
            {
                await _RoomOfferRepository.DeleteAsync(oldRoomId.Id);
            }

            foreach(var newRoomId in requestDto.RoomIds)
            {
                await _RoomOfferRepository.AddAsync(new RoomOffer
                {
                    RoomId = newRoomId,
                    OfferId = offer.Id,
                    IsActive = requestDto.IsActive
                });
            }

            _mapper.Map(requestDto, offer);
            await _OfferRepository.UpdateAsync(offer);
            await _OfferRepository.SaveChangesAsync();
            return new SuccessResponseVM<EditOfferRequestDto>(requestDto);
        }

        public async Task<ResponseVM<bool>> DeleteOfferAsync(int offerId)
        {
            var offer = await IsOfferExistsAsync(offerId);
            if (offer == null)
                return new FailureResponseVM<bool>(ErrorCode.OfferNotFound);
            offer.IsActive = false;
            await _OfferRepository.DeleteAsync(offer.Id);
            await _OfferRepository.SaveChangesAsync();
            return new SuccessResponseVM<bool>(true);
        }

    }
}
