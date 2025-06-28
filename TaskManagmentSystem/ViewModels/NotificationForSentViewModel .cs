namespace TaskManagmentSystem.ViewModels
{
    public class NotificationForSentViewModel
    {
        public int Id { get; set; }
        public string Details { get; set; } = null!;
        public DateTime DateToSend { get; set; }
        public bool IsRead { get; set; }
    }
}
