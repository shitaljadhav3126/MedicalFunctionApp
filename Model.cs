using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSolution
{
    class MedicalModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Location { get; set; }
        public string ProviderType { get; set; }
    }
    public class CreateModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public string Location { get; set; }
        public string ProviderType { get; set; }
    }
    public class UpdateModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public string Location { get; set; }
        public string ProviderType { get; set; }
    }
}
