using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EasyIotSharp.GateWay.Core.Domain
{
    public partial class RabbitProject
    {
        public int Id { get; set; }
        public int MqId { get; set; }
        public int ProjectId { get; set; }
        public string Extendstr { get; set; }
        public int? Extendint { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
