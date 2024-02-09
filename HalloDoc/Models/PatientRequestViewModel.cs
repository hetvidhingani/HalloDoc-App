﻿using HalloDoc.DataModels;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    public class PatientRequestViewModel
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]

        public string? LastName { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DocumentPath { get; set; }

        [Required]
        public string? Symptoms { get; set; }
        public string? RegionId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        [Required]

        public string? ClientFirstName { get; set; }
        [Required]

        public string? ClientLastName { get; set; }
        [Required]
        public string? ClientPhoneNumber { get; set; }
        public string? CLientEmail { get; set; }
        public DateOnly? DateOfBirth { get; set; }

    }
}
