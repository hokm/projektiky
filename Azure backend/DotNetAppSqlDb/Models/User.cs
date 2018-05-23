using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace DotNetAppSqlDb.Models
{
    public class User
    {
        public int ID { get; private set; }

        public string Role { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        private string _password;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength=6, ErrorMessage = "Password must contain 6-20 characters")]
        public string Password
        {
            get { return _password; }
            set {
                //TODO HASH
                _password = value;
            }
        }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "This is not your First Name!")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "This is not your Last Name!")]
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }

        public ICollection<IoTDevice> IoTDevices { get; set; }

        public User()
        {

        }
    }
}