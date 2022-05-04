﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSolution
{
    class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role {get; set;}   
    }
    public class CreateUserModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role {get; set;}   
    }
    public class UpdateUserModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role {get; set;}   
    }
}
