using Clinic_Management_API.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Management_API.Core.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        public decimal Amount { get; set; }  
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; } 
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}
