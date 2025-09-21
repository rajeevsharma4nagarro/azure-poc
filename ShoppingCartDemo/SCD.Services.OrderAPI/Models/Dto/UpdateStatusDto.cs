namespace SCD.Services.OrderAPI.Models.Dto
{
    public class UpdateStatusDto
    {
        public int OrderHeaderId { get; set; }
        public bool IsApproved { get; set; }
    }
}
