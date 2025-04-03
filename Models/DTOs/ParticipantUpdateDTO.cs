
using System;
using AdilBooks.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdilBooks.Models.DTOs

{
  public class ParticipantUpdateDTO
  {
      [Required]
      public int ParticipantId { get; set; }

      [Required(ErrorMessage = "Name is required.")]
      public string Name { get; set; }

      [Required(ErrorMessage = "Email is required.")]
      [EmailAddress]
      public string Email { get; set; }

      public int[] SelectedShowIds { get; set; } // Updated list of Show IDs
  }
}