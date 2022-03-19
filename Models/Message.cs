namespace messageboard.Models {
    public class Message {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public string? UserId { get; set; }

        public IList<Like>? Likes { get; set; }

        public int AantalLikes {
            get {
                if (this.Likes == null) {
                    return 0;
                }
                return this.Likes.Count;
            }
        }

        public string CreationDateString {
            get {
                if (this.CreationDate == null) {
                    return "";
                }
                return this.CreationDate.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
}