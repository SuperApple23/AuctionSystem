namespace AuctionSystem.ViewModels
{
    public class AuctionCampaignViewModel
    {
        public int CampaignId { get; set; }

        public string? CampaignName { get; set; }

        public string? Description { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
