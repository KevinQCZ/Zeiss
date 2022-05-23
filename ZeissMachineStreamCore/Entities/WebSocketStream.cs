
namespace ZeissMachineStreamCore.Entities
{
    public class WebSocketStream
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public StreamEvent StreamEvent { get; set; }

        public WebSocketStream(StreamEvent streamEvent, string createdBy = "backend")
        {
            StreamEvent = streamEvent;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
            UpdatedBy = CreatedBy;
            UpdatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
