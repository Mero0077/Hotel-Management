using AutoMapper;
using Hotel_Management.DTOs.Offers;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
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
        public OfferService(IMapper mapper) 
        {
            this._mapper = mapper;
            _OfferRepository = new GeneralRepository<Offer>();
            _RoomRepository =  new GeneralRepository<Room>();
            _RoomOfferRepository = new GeneralRepository<RoomOffer>();
        }

        public async Task<Offer?> IsOfferExistsAsync(int offerId)
        {
            var offer = await _OfferRepository.GetOneWithTracking(e=>e.Id==offerId);
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
            await _OfferRepository.Add(offer);

            foreach (var roomId in requestDto.RoomIds)
            {
                //Need To Modify It and Call The RoomService Instead of using _RoomRepo
                var room = await _RoomRepository.GetOneById(roomId);
                if(room == null)  return new FailureResponseVM<CreateOfferRequestDto>(ErrorCode.RoomNotFound);

                await _RoomOfferRepository.Add(new RoomOffer
                {
                    RoomId = roomId,
                    OfferId = offer.Id,
                    IsActive = requestDto.IsActive,
                });
            }
            
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
                await _RoomOfferRepository.Delete(oldRoomId.Id);
            }

            foreach(var newRoomId in requestDto.RoomIds)
            {
                await _RoomOfferRepository.Add(new RoomOffer
                {
                    RoomId = newRoomId,
                    OfferId = offer.Id,
                    IsActive = requestDto.IsActive
                });
            }

            _mapper.Map(requestDto, offer);
            await _OfferRepository.Update(offer);
            return new SuccessResponseVM<EditOfferRequestDto>(requestDto);
        }

        public async Task<ResponseVM<bool>> DeleteOfferAsync(int offerId)
        {
            var offer = await IsOfferExistsAsync(offerId);
            if (offer == null)
                return new FailureResponseVM<bool>(ErrorCode.OfferNotFound);
            offer.IsActive = false;
            await _OfferRepository.Delete(offer.Id);
            return new SuccessResponseVM<bool>(true);
        }

    }
}
