//namespace Hotel_Management.DTOs.Reservation
//{
//    public class RoomRequestValidator : AbstractValidator<RoomRequest>
//    {
//        public RoomRequestValidator()
//        {
//            RuleFor(x => x.RoomNumber)
//                .NotEmpty().WithMessage("Room number is required.")
//                .MaximumLength(20).WithMessage("Room number cannot exceed 20 characters.");

//            RuleFor(x => x.Description)
//                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

//            RuleFor(x => x.Status)
//                .IsInEnum().WithMessage("A valid room status is required.");

//            RuleFor(x => x.PricePerNight)
//                .GreaterThan(0).WithMessage("Price per night must be greater than zero.");

//            RuleFor(x => x.MaxOccupancy)
//                .GreaterThan(0).WithMessage("Maximum occupancy must be at least 1.");

//            RuleFor(x => x.RoomTypeId)
//                .GreaterThan(0).WithMessage("A valid Room Type ID is required.");

//            RuleFor(x => x.FacilityIds)
//                .NotNull().WithMessage("Facility list cannot be null.");
//        }
//    }
//}
