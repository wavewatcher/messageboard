using System.ComponentModel.DataAnnotations;

namespace messageboard.Models {
    public class Like {
        public int id { get; set; }
        public int MessageId { get; set; }
        public string? UserId { get; set; }

        public DateTime likeDate { get; set; }
    }
}