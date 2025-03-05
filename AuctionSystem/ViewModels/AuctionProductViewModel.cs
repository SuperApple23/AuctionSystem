namespace AuctionSystem.ViewModels
{
    public class AuctionProductViewModel
    {
		public int AuctionId { get; set; }

		public string? MainImage { get; set; }

        public string? ProductName { get; set; }

        public double ListedPrice { get; set; }

        public double StartingPrice { get; set; }

        public double MinimumPriceIncrement { get; set; }

		public double InstantSellPrice { get; set; }

        public int NumberOfPeople { get; set; }

        public int IsAuctionFinished { get; set; }
	}
}
