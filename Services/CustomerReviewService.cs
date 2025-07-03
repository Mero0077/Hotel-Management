using AutoMapper;
using Hotel_Management.DTOs.Error;
using Hotel_Management.DTOs.Reviews;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class CustomerReviewService
    {
        private readonly GeneralRepository<CustomerFeedback> _customerFeedbackRepository;
        private readonly GeneralRepository<Reservation> _reservationRepository;
        private IMapper _mapper;
        public CustomerReviewService(GeneralRepository<CustomerFeedback> generalRepository,IMapper mapper, GeneralRepository<Reservation> reservationRepository)
        {
            _customerFeedbackRepository = generalRepository;
            _mapper = mapper;
            _reservationRepository = reservationRepository;
        }

        private async Task<ResponseDTO<CustomerFeedback>> CheckReviewState(CustomerReviewDTO customerReviewDTO)
        {
            var reservation = await _reservationRepository.Get(r =>r.Id == customerReviewDTO.ReservationId &&
                                                                                   r.CustomerId == customerReviewDTO.UserId).FirstOrDefaultAsync();

            if (reservation == null)
                return new ErrorFailDTO<CustomerFeedback>(ErrorCode.UnauthorizedAccess, "You can only review your own reservations.");

            var existingReview = await _customerFeedbackRepository.Get(r => r.UserId == customerReviewDTO.UserId
                                                                             && r.ReservationId == customerReviewDTO.ReservationId).FirstOrDefaultAsync();

            if (existingReview != null)
                return new ErrorFailDTO<CustomerFeedback>(ErrorCode.DuplicateReview, "You've already reviewed this reservation.");

            return null;
        }
        public async Task<ResponseDTO<CustomerFeedback>> AdminApproveReview(int ReviewId)
        {
           var review= await _customerFeedbackRepository.GetOneByIdAsync(ReviewId);
            if (review == null)
                return new ErrorFailDTO<CustomerFeedback>(ErrorCode.ReviewNotFound, "Review Not Found");

            if (review.IsApproved)
                return new ErrorFailDTO<CustomerFeedback>(ErrorCode.AlreadyApproved, "Review is already approved.");

          review.IsApproved = true;
          await  _customerFeedbackRepository.UpdateIncludeAsync(review, nameof(review.IsApproved));
          await _customerFeedbackRepository.SaveChangesAsync();

          return new ErrorSuccessDTO<CustomerFeedback>(review, "Review is approved successfully.");
        }
        public async Task<ResponseDTO<CustomerFeedback>> CustomerReviewsVisit(CustomerReviewDTO customerReviewDTO)
        {
            var validationResult = await CheckReviewState(customerReviewDTO);
            if (validationResult != null)
                return validationResult;

           var review= await _customerFeedbackRepository.AddAsync(_mapper.Map<CustomerFeedback>(customerReviewDTO));
           await _customerFeedbackRepository.SaveChangesAsync();
           return new ErrorSuccessDTO<CustomerFeedback>(review, "Review is posted successfully.");
        }
    }
}
