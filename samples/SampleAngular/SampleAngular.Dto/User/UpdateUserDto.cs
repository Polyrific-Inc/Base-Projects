// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace SampleAngular.Dto
{
    public class UpdateUserDto
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// UserName of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First Name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the user
        /// </summary>
        public string LastName { get; set; }
    }
}
