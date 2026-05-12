namespace MvcCreditApp.ViewModels
{
    public class VarietySummaryViewModel
    {
        public int VarietyId { get; set; }
        public string VarietyName { get; set; }
        public string Crop { get; set; }
        public int Year { get; set; }
        public int Germination { get; set; }
        public int Productivity { get; set; }
        public int TotalScore { get; set; }
        public string GerminationText { get; set; }
        public string ProductivityText { get; set; }
    }
}
