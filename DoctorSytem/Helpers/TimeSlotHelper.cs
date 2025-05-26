using System;
using System.Collections.Generic;

namespace DoctorSystem.Helpers
{
    public static class TimeSlotHelper
    {
        // Generates 30-minute slots between startTime and endTime for a given date
        public static List<TimeSpan> GenerateSlots(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var slots = new List<TimeSpan>();
            var current = startTime;

            while (current < endTime)
            {
                slots.Add(current);
                current = current.Add(TimeSpan.FromMinutes(30));
            }

            return slots;
        }
    }
}
