using AutoMapper;
using AutoMapper.Features;
using Hotel_Management.DTOs.Reviews;
using Hotel_Management.Filters;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Models.ViewModels.Reservations;
using Hotel_Management.Models.ViewModels.Reviews;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private CustomerReviewService _customerReviewService;
        private IMapper _mapper;
        public ReviewsController(CustomerReviewService customerReviewService, IMapper mapper)
        {
            _customerReviewService = customerReviewService;
            _mapper = mapper;
        }

        [HttpPost("Review")]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.AddReview})]
        public async Task<ResponseVM<ReviewVM>> ReviewReservation(ReviewVM reviewVM)
        {
          var result= await _customerReviewService.CustomerReviewsVisit(_mapper.Map<CustomerReviewDTO>(reviewVM));
          if(!result.IsSuccess)
                return new FailureResponseVM<ReviewVM>(result.errorCode, result.Message);

          return new SuccessResponseVM<ReviewVM>(_mapper.Map<ReviewVM>(result.Data),result.Message);
        }

        [HttpPost("ApproveReview")]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.ApproveReview})]
        public async Task<ResponseVM<ReviewVM>> ApproveReview([FromQuery] int Id)
        {
            var result = await _customerReviewService.AdminApproveReview(Id);
            if (!result.IsSuccess)
                return new FailureResponseVM<ReviewVM>(result.errorCode, result.Message);

            return new SuccessResponseVM<ReviewVM>(_mapper.Map<ReviewVM>(result.Data), result.Message);
        }

    }
}
