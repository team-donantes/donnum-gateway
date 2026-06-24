namespace Donnum.Gateway.Application.Models.Donor;

public class CreateParticipationDto
{
    public Guid DonorId { get; set; }
    public Guid DonationRequestId { get; set; }
    public int Status { get; set; }
}
