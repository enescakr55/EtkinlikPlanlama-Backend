using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.Dtos
{
    public class EventRepeatDto
    {
        public int UserId { get; set; }
        public EventRepeat EventRepeat { get; set; }
    }
}
