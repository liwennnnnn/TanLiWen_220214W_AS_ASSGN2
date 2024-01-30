namespace TanLiWen_220214W_AS_ASSGN2.Models
{
	public class AuditLog
	{
		public int Id { get; set; }
		public string UserId { get; set; }	// User interacting
		public string Action { get; set; }	// Description of action
		public DateTime Timestamp { get; set; }		// Timestamp of action
	}
}
