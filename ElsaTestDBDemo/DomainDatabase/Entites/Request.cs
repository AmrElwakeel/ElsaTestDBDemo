using System.ComponentModel.DataAnnotations;

namespace ElsaTestDBDemo.DomainDatabase.Entites
{
    public class Request
    {
        [Key]
        public string? Id { get; set; }
        public string WorkflowInstanceId { get; set; } = "";
        public string CurrentStatus { get; set; } = "";
        public string? UserId { get; set; }
        public string? UserRole { get; set; }
        public DateTime ExecutionDate { get; set; } = DateTime.Now;
    }
}
