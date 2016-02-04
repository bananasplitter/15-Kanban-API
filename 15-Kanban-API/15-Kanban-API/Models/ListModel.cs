using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _15_Kanban_API.Models
{
    public class ListModel
    {
        
        
        public int ListId { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ListUrl
        {
            get
            {
                return $"http://localhost:50047/api/lists/{ListId}/cards";
            }
        }
    }
}