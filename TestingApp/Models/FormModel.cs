using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.Models;

class FormModel
{
    [Required]
    [Display(Name = "A form field name")]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public double Value { get; set; }
}

class SecondFormModel
{
    [Required]
    [Display(Name = "A form field name")]
    public string Field { get; set; }

    [Display(Name = "Date and time")]
    public DateTime DateTime { get; set; }
    public TimeSpan TimeSpan { get; set; }
}

class ThirdFormModel
{
    [Required]
    [Display(Name = "Custom Field Display Name")]
    public string Field { get; set; }
    [Required]
    public string Field2 { get; set; }
    public string? Field3 { get; set; }
}
