using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace messageboard.Models {
    public class User {
        public int id { get; set;}
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}