﻿namespace scada.DTO
{
    public class FilterDTO
    {
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public string? SortingType { get; set; }

        public override string ToString()
        {
            return $"StartDate: {StartDate}, EndDate: {EndDate}";
        }
    }
}
