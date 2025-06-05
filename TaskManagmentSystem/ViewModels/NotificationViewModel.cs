namespace TaskManagmentSystem.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Details { get; set; } = null!;
        public DateTime DateToSend { get; set; }
        public bool IsRead { get; set; }
        public int? TaskId { get; set; }
    }
}
