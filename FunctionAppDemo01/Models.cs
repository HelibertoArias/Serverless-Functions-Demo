using System;

namespace FunctionAppDemo01
{
    public class Company
    {
        // Guid format 00000000000000000000000000000000
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class CompanyCreateDto
    {
        public string CompanyName { get; set; }
    }

    public class CompanyUpdateDto
    {
        public string CompanyName { get; set; }
    }
}
